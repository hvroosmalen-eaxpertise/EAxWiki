# Design: Error log + configuration/schedule pages in the wiki (issue #87)

Status: approved design.

## Goal

Add two monitor-generated status pages to the wiki so a user can see operation failures and the current runtime/schedule configuration without digging in the machine's log path:

- `wiki/status/errors.md` - a filtered error log page linked from the Health page.
- `wiki/status/config.md` - a read-only page presenting the resolved configuration and the registered schedule.

## Problem statement (from issue #87)

The user must currently open the machine's log directory to see whether scheduled exports fail and why. The monitor already keeps a day-scoped operation log (`.eaxwiki-monitor/<hash>/logs/monitor-*.log`) and knows its resolved options, but nothing surfaces this in the wiki. The schedule lives only in Task Scheduler, invisible from the wiki. The issue asks for an error-log page ("see any failures, inconsistencies and such without digging in the computers log path") linked from the Health page, plus a read-only presentation of the current configuration settings.

## Decisions locked in during brainstorming

- **Approach:** the monitor generates both pages every loop cycle, mirroring the existing `HealthPageRenderer` pattern. Not the exporter (would couple export to monitor state + Task Scheduler) and not a separate `--status-pages` mode (YAGNI - the monitor already regenerates every cycle).
- **Error-log scope:** monitor + in-process export operation log only. Export-validation findings (`.validation-report.json`) stay separate - a different "health" concept.
- **Schedule source:** live `Get-ScheduledTask` query through pwsh (already located via `MonitorPaths.FindPowerShell()`), cached ~5 minutes, robust to manual task edits. Not a `schedule.json` copy that can drift.
- **Redaction:** operational values shown; secrets masked. The `wiki/status/` pages are committed with the wiki and published to GitHub Pages, so this is effectively public.
- **Nav:** entries in the status/ `.pages` (existence-gated, same pattern as `health.html`) **and** links from the Health page.

## Non-goals

- No changes to the write-back server, the export pipeline, alerting, or the scheduler itself.
- No editing of configuration from the wiki - the config page is strictly read-only.
- No surfacing of the export-validation report on the error page.
- No `--status-pages` / on-demand generation mode.
- `DigestTracker` is untouched (it reads `serve-*.err.log` and `writeback.log`, not `monitor-*.log`).

## Architecture

### 1. Log format: add severity to `MonitorFileLoggerProvider`

`MonitorFileLoggerProvider` (`src/EAxWiki.Monitor/MonitorFileLoggerProvider.cs`) currently writes `yyyy-MM-dd HH:mm:ss [phase] message`. `IsEnabled` already returns `true` for all levels, so nothing is filtered out today. Add the severity so the error page can filter:

```
yyyy-MM-dd HH:mm:ss [INF|WRN|ERR] [phase] message
```

The level token is derived from `LogLevel` (`Information`→INF, `Warning`→WRN, everything else incl. `Error`/`Critical`→ERR). Existing human-oriented readers (the scheduled-task-diagnostics skill, manual inspection) remain usable. No other component parses this file.

### 2. `ErrorLogPageRenderer` → `wiki/status/errors.md`

New class mirroring `HealthPageRenderer`:

- Constructor: `(string templatePath, string wikiDir, string logsDir, string[] secretValues)` where `logsDir = {stateDir}/logs` (the instance's own hash dir - never the legacy top-level `.eaxwiki-monitor/logs/`).
- `Render(DateTime now)`:
  1. Collect `monitor-{yyyy-MM-dd}.log` files for `now-7d .. now` (missing days skipped).
  2. Parse each line for the `[WRN]`/`[ERR]` severity; keep those, newest first, capped at 100 entries.
  3. Render each kept line after redaction; zero matches → "No issues in the last 7 days" state.
  4. Append a collapsed "recent all-level lines (last 20)" block for context.
  5. Fill `errors-template.md` tokens `@@GENERATED_AT@@` (timestamp) and `@@ERRORS@@`.
- **Redaction per line:** the existing connection-string redaction (mask `Password=`/`Pwd=` values - same rule as `Program.Redact`, `EAxWiki.Core.Models.EaRepository.Redact`) **plus** a mask of this instance's resolved secrets (`WebhookUrl`, `TeamsWebhookUrl`, `TelegramBotToken`, `aiKey`). Per-instance correctness: each instance renders only its own `{hash}` log dir and masks with its own resolved secrets, so multiple monitors on one PC (different ports/wiki dirs) never leak each other's configuration.
- `errors-template.md` ships tracked under `.eaxwiki-monitor/` like `health-template.md`/`digest-template.md`.

### 3. `ScheduledTaskSnapshot` service

New testable service in `EAxWiki.Monitor`:

- Public interface `IScheduledTaskSnapshot` with `ScheduledTaskInfo? Get()`, impl `ScheduledTaskSnapshot`:
  - Finds pwsh via `MonitorPaths.FindPowerShell()` and runs a `Get-ScheduledTask` query, locating the task by action executable path == `Environment.ProcessPath` (the monitor exe), so a custom `--task-name` works automatically. This matches how `register-scheduled-task.ps1` registers the task (`New-ScheduledTaskAction -Execute $monitorExe`, the `.exe` apphost). A hand-registered `dotnet EAxWiki.Monitor.dll` action won't match and renders as "schedule unavailable" - acceptable, no fallback.
  - Parses trigger descriptions ("Daily at 00:00, every 240 min (for 8 h)", "Mon-Fri at 08:00, every 10 min"), wake-to-run, execution-time-limit, MultipleInstances mode, task state.
  - Caches the parsed result for ~5 minutes (the monitor loop runs every 30 s; do not query every cycle).
  - On any failure returns a "schedule unavailable" state - never throws into the loop.
- Tests inject a fake provider; the renderer never shells out itself.

### 4. `ConfigPageRenderer` → `wiki/status/config.md`

New class; fully code-generated markdown (structured tables), **not** template-driven - config is a fixed data table (~25 fields) where a per-token template adds no customization value.

Content, all from the resolved `MonitorOptions` + the task snapshot:

- **Run settings:** wiki dir; repo through the existing connection-string redaction (plain `.qea` paths shown, DB `Password=`/`Pwd=` masked); wiki/API/LLM ports; export interval; check interval; max-retries + retry backoff; min-element-fraction; force mode; `--force-every-N-runs`; brand; AI mode/endpoint/model (never the key).
- **Alert destinations:** `Slack: configured` / `Teams: configured` / `Telegram: configured` or `not configured` - never URLs or tokens.
- **Schedule:** from `IScheduledTaskSnapshot` - trigger descriptions, wake-to-run, execution-time-limit, MultipleInstances, task state; or a "Schedule info unavailable" block.
- **Meta:** generated-at timestamp.

### 5. Wiring

- `MonitorApp.Build` constructs `ErrorLogPageRenderer`, `ScheduledTaskSnapshot`, and `ConfigPageRenderer` and passes them to `MonitorLoop`.
- `MonitorLoop.RenderAndSave` (`MonitorLoop.cs:223`) renders all three pages in sequence, each wrapped in the existing try/catch → warn-on-failure pattern so one bad render never breaks the loop.
- `InfrastructureWriter.WritePagesFileAsync` (`src/EAxWiki.Export/Exporters/InfrastructureWriter.cs:31`) extends the `statusLines` array with `"  - Error Log: status/errors.html"` and `"  - Configuration: status/config.html"`, gated on `File.Exists(Path.Combine(outputDir, "status", "errors.md"))` / `"config.md"` respectively - same existence-check pattern as `health.md`, same awesome-pages `.html` quirk workaround. Plain `export.ps1`/`export-and-serve.ps1` runs (no monitor) get no links to missing pages.
- `.eaxwiki-monitor/health-template.md` (tracked) gains "Error log" and "Configuration" links near the top, per the issue's "linked from the Health page" requirement.
- Both new pages carry generated-at timestamps; when the monitor is stopped they go stale exactly like `health.md` today (visible from the timestamp).

## Testing

- **Unit (EAxWiki.Monitor.Tests):**
  - `MonitorFileLoggerProvider`: lines carry `[INF|WRN|ERR]` in the correct slot (update any existing format assertions).
  - `ErrorLogPageRenderer`: WRN/ERR only from the 7-day window; newest-first; 100-entry cap; empty-window happy state; all-levels fallback; per-instance secret masking; connection-string redaction.
  - `ConfigPageRenderer`: all fields render; secrets only as configured/not-configured; repo redaction; schedule present + "unavailable" paths.
  - `ScheduledTaskSnapshot`: parses representative `Get-ScheduledTask` output (interval + day/night trigger shapes); locates task by exe path; cache expiry; provider failure → unavailable.
  - `InfrastructureWriter`: status `.pages` contains error/config entries only when the files exist.
- **Integration/smoke:** run the monitor briefly; verify `errors.md`/`config.md` appear in `wiki/status/`, survive an export's orphan cleanup, and are served by mkdocs.
- **Pester:** a plain `export-and-serve.ps1` run (no monitor) produces no error/config nav links.

## Out of scope / follow-ups

- Surfacing the export-validation report on the error page (different "health" concept; left to a future issue).
- Editing configuration or the schedule from the wiki.