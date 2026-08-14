# Design: C# monitor (issue #86 item #3)

## Overview

`scripts/monitor-export-and-serve.ps1` (1296 lines) is a whole application in a shell
script: alert dispatch (Slack/Teams/Telegram), health tracking, retry policy, daily
digest computation, force-rebuild scheduling, and three service watchdogs (mkdocs
serve, write-back API server, llama-server). This design ports it to a new
`src/EAxWiki.Monitor` console project so the logic becomes unit-testable in .NET.

The PowerShell monitor is **fully replaced** (not coexisting): `monitor-export-and-serve.ps1`
and its ~47 Pester tests are removed; coverage moves to xUnit in `EAxWiki.Tests`.

## Goals / non-goals

Goals:
- A `EAxWiki.Monitor.exe` that reproduces the current monitor's behavior 1:1
  (same flags, same state file, same alert kinds/formatting, same watchdog semantics).
- The retry / health / digest / force logic becomes plain C# testable without EA/COM.
- Live `--force` / `--force-every N` semantics (the current PS monitor hardcodes
  `$effectiveForce = $true`, making these flags dead — this is a deliberate fix).
- All ports configurable, including a new configurable LLM port.
- A read-only health dashboard in the SchedulerUI.
- System.CommandLine adopted for the monitor's argument parsing. The monitor has its
  own System.CommandLine root command; the existing EAxWiki `Config.Load` hand-rolled
  parser is NOT touched here (issue #4 later refactors it to System.CommandLine as its
  own follow-up, which the monitor's parser sets a precedent for).

Non-goals:
- No HTTP control endpoint for the SchedulerUI (file-based health.json flags are kept).
- No reimplementing the mkdocs/Python toolchain in C#; serve.ps1 stays as-is and is
  supervised as a child process.
- SchedulerUI Stop buttons are not rewired (they are process-kill + send-alert.ps1
  ops that work regardless of which exe is the monitor).
- Not touching the EAxWiki console/API project's existing `Config.Load` hand-rolled
  parser in this item.

## Architecture

New project `src/EAxWiki.Monitor` (console, net10.0), added to the sln. It is the
monitor; no `--monitor` flag on the existing exe. References:
- `EAxWiki.Core` — LocalConfigStore, models, shared types
- `EAxWiki.EA` — EaReader, EaReaderStaDispatcher (STA threading for in-process export)
- `EAxWiki.Export` — MarkdownExporter, WriteBackScanner, JsonExporter
- `System.CommandLine` — argument parsing

Dependency flow (all components except ExportRunner need no EA/COM):

```
Program (System.CommandLine parse + .eaxwiki resolve)
  └─ MonitorOptions
  └─ MonitorLoop                 (the while(true) cycle)
       ├─ ExportRunner           (in-process export on STA, retry/backoff, sanity check, force)
       ├─ AlertDispatcher        (Slack / Teams / Telegram)
       ├─ HealthStore            (health.json read/write + backfill)
       ├─ HealthPageRenderer     (health-template.md → wiki/status/health.md)
       ├─ DigestTracker          (page-read/writeback offsets, daily digest)
       ├─ ProcessSupervisor      (generic child watchdog: serve.ps1, llama-server, EAxWiki --api)
       ├─ PortProbe              (TCP 500ms probe)
       ├─ PortKiller             (netstat -ano parse → Stop-Process by PID)
       └─ EditLock               (.data/edit-lock.json read + expiry)
```

Components are plain classes instantiated in `Program` and injected into
`MonitorLoop`; `MonitorLoop` depends on an `IMonitorDependencies`-style set of
interfaces (`IAlertDispatcher`, `IHealthStore`, `IProcessSupervisor`, etc.) so every
unit can be substituted with a fake in tests. `EAxWiki.Monitor` uses the standard
`Microsoft.Extensions.Logging` `ILogger` for its own log output, writing to the same
per-instance `logs/monitor-yyyy-MM-dd.log` path the PS monitor uses.

## Process model

- `EAxWiki.Monitor` is always a **detached, independently-running process**.
  SchedulerUI's Run Monitor Now launches it via `Process.Start(UseShellExecute = true)`
  (a separate window); the scheduled task launches it via Task Scheduler. The UI can
  close immediately after; the monitor keeps running.
- On startup the monitor performs the existing duplicate-instance check: write
  `monitor.pid`, and if an existing live PID is found, exit 0. Remove `monitor.pid`
  on shutdown.
- The monitor never hosts the write-back API server in-process. The API server stays a
  child process (`EAxWiki --api`), independently restartable, so it survives monitor
  restarts.

## Ports

| Port | Today | C# monitor |
|---|---|---|
| Wiki (mkdocs serve) | `.eaxwiki wikiPort`, `--port` override | unchanged (`--port`) |
| API (write-back) | `.eaxwiki apiPort` | unchanged (`--api-port`) |
| LLM (llama-server) | **hardcoded 8080** | **new** `--llm-port` flag + `.eaxwiki llmPort` field (default 8080) |

`LlmPort` is added to `LocalConfigStore.Config` (nullable int). The monitor reads it;
SchedulerUI's AI tab gets an LLM port box and its `StartLlmAsync` switches from the
hardcoded `--port 8080` to the configured value.

## Argument parsing (System.CommandLine)

The root command mirrors the PS monitor's exact flag surface so
`register-scheduled-task.ps1` and SchedulerUI's arg-building code keep working with
only an exe-name swap:

- `--repo, -r`, `--output, -o`, `--port, -p`
- `--max-retries`, `--retry-delay`, `--min-element-fraction`
- `--webhook-url`, `--teams-webhook-url`, `--telegram-bot-token`, `--telegram-chat-id`
- `--brand`
- `--test-alert`, `--no-notify-start`
- `--force, -f`, `--force-every`
- `--export-interval`, `--check-interval`
- `--llm-port` (new)

Resolution order for webhooks/telegram/brand/ports (unchanged from today):
CLI arg → env var (`EAXWIKI_ALERT_WEBHOOK`, etc.) → `.eaxwiki` (DPAPI-decrypted via
`LocalConfigStore.Load`).

## ExportRunner

In-process export on an STA thread (reusing the `EaReaderStaDispatcher` pattern from
`EAxWiki.EA`):

1. `EaReader.Open(repoPath)`
2. Optional `WriteBackScanner.Scan(outputPath)` when `ApiPort > 0` (write-back)
3. `MarkdownExporter.ExportAsync(...)` (or `JsonExporter` when requested), passing the
   effective force flag, brand, and API-port env var (`EAXWIKI_API_PORT` etc.)
4. Element/diagram counts read back from the output

Retry loop: up to `--max-retries` attempts with `retry-delay * attempt` backoff.
Sanity check: element count must be ≥ floor(previous count × `--min-element-fraction`),
else the attempt is treated as failed.

**Crash boundary:** each export is wrapped in a broad try/catch. A native COM fault
inside export is caught, logged, recorded as a failure (with a Failure alert), and the
loop continues rather than killing the monitor. Child-process starts are likewise guarded.

## Force semantics (deliberate fix)

- `--force` → every run is a full rebuild.
- `--force-every N` → full rebuild when `runsSinceForce >= N`; reset to 0 on a
  successful forced run.
- Neither → incremental (matches export.ps1's own default; important for large models).

Tracked in health state as `runsSinceForce` (already present).

## AlertDispatcher

C# port of `Send-Alert` + `Send-TelegramMessage` + `Format-TelegramAlertText` with
exact parity:

- Slack: attachments payload with color/pretext/text/footer/ts, emoji per kind.
- Teams: MessageCard with themeColor/summary/sections.
- Telegram: HTML parse_mode; emoji title; `<b>` label; `<i>` footer with timestamp;
  HTML-escaping; fence → `<pre>` with inner escaping; 4000-char truncation with
  "... (truncated)"; one-shot 400 retry that drops `parse_mode`.

Kind → emoji/color maps identical to the PS version (`Start`/`Finish`/`Failure`/
`Recovery`/`ServeFailure`/`ServeRecovery`/`LlmFailure`/`LlmRecovery`/`ApiFailure`/
`ApiRecovery`/`Test`/`DailyDigest`/`UserStop`). Instance label
`$COMPUTERNAME - $wikiDir`. Injectable `HttpClient` (via `HttpMessageHandler`) so
alert dispatch is unit-tested with a stub handler.

## HealthStore

Typed `HealthState` model matching the PS default shape, with backfill: load
`health.json`, start from all-defaults, overlay on-disk values so older state files
never silently drop a field (equivalent of `Add-Member -Force`). `skipExport` /
`skipServe` round-trip intact for the SchedulerUI.

State fields (unchanged names): `lastSuccessTime`, `lastFailureTime`,
`consecutiveFailures`, `lastExitCode`, `lastElementCount`, `lastDiagramCount`,
`serveConsecutiveFailures`, `lastServeFailureTime`, `lastServeSuccessTime`,
`llmConsecutiveFailures`, `lastLlmFailureTime`, `lastLlmSuccessTime`,
`apiConsecutiveFailures`, `lastApiFailureTime`, `lastApiSuccessTime`, `lastApiPort`,
`runsSinceForce`, `lastMode`, `pageReadsToday`, `writebacksToday`, `lastDigestDate`,
`pageReadLogFile`, `pageReadLogOffset`, `writebackLogFile`, `writebackLogOffset`,
`skipExport`, `skipServe`.

State directory: `.eaxwiki-monitor/<12-char hash of lowercased wikiDir>/` with
`health.json`, `serve.pid`, `api.pid`, `llm.pid`, `monitor.pid`, `logs/` — identical
paths to today.

## HealthPageRenderer

Reads `health-template.md`, replaces `@@TOKEN@@` placeholders from the `HealthState`,
writes `wiki/status/health.md`. Template files stay in `.eaxwiki-monitor/`.

## DigestTracker

Offset-based page-read counting (mkdocs dev-server log) and write-back counting
(`wiki/status/writeback.log`), tracked as `pageReadLogFile/pageReadLogOffset` and
`writebackLogFile/writebackLogOffset` in state. On a calendar-day boundary, sends a
`DailyDigest` alert with the previous day's totals and resets the counters.

## ProcessSupervisor

Generic child-process watchdog used for serve.ps1 (via pwsh), llama-server, and
`EAxWiki --api`:

- Alive check: pid file exists AND PID is running AND recorded start time matches the
  actual process start time within 2s (prevents stale-pid false positives after reboot).
- Port-probe fallback for serve (leave an already-listening port alone even if not
  tracked by this monitor instance).
- Start with retry/backoff (same `--max-retries`/`--retry-delay`), PID+startTime pid
  file written on success.
- API-specific: kill stale port occupant via `Clear-Port`, remove stale `api-ready`,
  start `EAxWiki --api`, poll `wiki/status/api-ready` for up to 120s at 1s intervals.
- Per-service consecutive-failure counters + `Last*SuccessTime`/`Last*FailureTime` in
  health state; `*Failure`/`*Recovery` alerts on give-up/recovery.

## EditLock

Reads `.data/edit-lock.json` (relative to wiki dir parent): absent → unlocked; expired
→ remove stale lock file and report unlocked; active → defer export this cycle.

## MonitorLoop

`while (true)` cycle with `--check-interval` sleep:

1. Reset `skipExport`/`skipServe` each cycle.
2. Export due? (`lastExportTime` unset or elapsed ≥ `--export-interval`).
3. Edit-lock check (defer export if active).
4. Export (if due) via ExportRunner; update health state; send Start/Finish/Recovery/
   Failure alerts; collect writeback + validation summary for the Finish alert.
5. Daily digest accounting.
6. Render health page + save health state.
7. Serve watchdog.
8. API watchdog (if `ApiPort > 0`).
9. LLM watchdog (if `AiMode == local` and paths valid).
10. Sleep.

## SchedulerUI dashboard (new, read-only)

A new tab in the SchedulerUI (next to Configuration / Schedule Settings / AI LLM /
Task Status) that reads `.eaxwiki-monitor/<hash>/health.json` + `*.pid` files and
displays a per-service status table:

- **Export**: last success/failure, consecutive failures, last exit code, page counts,
  last mode, runs since force.
- **Serve**: running / not running (pid-file alive + port probe), last success/failure,
  consecutive failures.
- **API**: same, plus "not configured" when `ApiPort` unset.
- **LLM**: same, plus "not configured" when `AiMode != local` or paths missing.

Reads are pure file reads + `Get-Process` pid checks — no HTTP surface, no new auth.
A Refresh button re-reads. SchedulerUI's AI tab additionally gains an LLM port box.

## Caller updates

- `register-scheduled-task.ps1`: action changes from
  `pwsh -File monitor-export-and-serve.ps1` to `EAxWiki.Monitor.exe`, same args.
- `_bootstrap.ps1`: add a `Get-EAxWikiMonitorExePath` helper (repo-root-based
  resolution, like `Get-EAxWikiDllPath`).
- `SchedulerUI`: Run Monitor Now → launch `EAxWiki.Monitor.exe` (detached); AI tab LLM
  port box; new health dashboard tab. Stop buttons unchanged.

## Testing

xUnit (in `EAxWiki.Tests`), no EA/COM except where noted:

- **AlertDispatcher** — mocked `HttpMessageHandler`: Slack attachments payload, Teams
  MessageCard, Telegram HTML body (emoji/footer/fence→pre/truncation), Telegram 400
  one-shot `parse_mode` retry. Ported 1:1 from the removed Pester coverage.
- **HealthStore** — round-trip, backfill of older on-disk JSON, skip-flag preservation,
  corrupt-file fallback.
- **HealthPageRenderer** — token replacement incl. null handling.
- **ExportRunner force logic** — `--force` / `--force-every N` / incremental from
  `runsSinceForce`; retry/backoff; `min-element-fraction` pass/fail — using a fake
  `IWikiExporter` (already an interface).
- **DigestTracker** — offset counting from a temp log; day-boundary trigger + reset.
- **ProcessSupervisor** — alive-check via pid file + start-time match, port-probe
  fallback, retry/backoff, ready-file timeout — with a real short-lived child process.
- **EditLock** — active/expired/absent.
- **PortProbe/PortKiller** — TCP connect probe; `netstat -ano` parse behind an interface.
- **System.CommandLine parsing** — every flag + defaults (mirrors the removed Pester
  `Get-MonitorArgs` tests).

Pester: `tests/scripts/monitor-export-and-serve.Tests.ps1` and
`tests/scripts/send-alert.Tests.ps1` removed.

## Verification

- Full .NET suite green (including new monitor tests).
- Pester suite green (minus removed files).
- Real smoke run: `EAxWiki.Monitor --test-alert` dispatches a Test alert to all
  configured channels; one live export cycle against the dev `.qea` completes and
  writes health.json/health.md.
