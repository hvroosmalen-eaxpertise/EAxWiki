# Unattended export/serve monitoring & alerting (Issue #37)

## Problem

`export-and-serve.ps1` (and its siblings) are meant to run unattended on a schedule, but nothing today notices when they stop working. Export can crash outright, EA COM can hang or leave an orphaned `EA.exe` that blocks the *next* run, `mkdocs serve` can die and take the wiki down, or export can "succeed" (exit 0) while producing near-empty output because of a bad repo path or filter. In every case, the current failure mode is silence — the first sign is a human noticing stale pages.

Issue #37 asks for three things: log to a file, report the last log lines to an admin, and attempt restarts. This doc captures the design arrived at after grilling through the options.

## Decisions

- **Alert channels**: a Teams/Slack incoming webhook, built now, posting to a team channel (not a DM — audience is "me + a small team"). Email (SMTP) is designed for but deferred — no relay exists yet, and Gmail-app-password setup is friction not worth blocking this on. Alert dispatch is written as a small abstraction (channel-agnostic call site) so email slots in later without touching the retry/detection logic.
- **A passive freshness signal** in addition to push alerts: a new `wiki/status/health.md` page, separate from the existing element Status Dashboard (`wiki/status/index.md`) — different subject (pipeline health vs. EA model element status), same section.
- **Failure definition**: non-zero exit / unhandled exception, *and* a basic sanity check on output size (e.g. element count collapsing to near-zero) — catches an export that exits 0 but produced garbage.
- **Retry policy**: bounded retries with backoff, then alert and stop. No infinite retry — a permanently broken config (bad connection string, EA not installed) should surface, not loop silently forever.
- **Alert throttling**: notify once, on final give-up — not once per retry attempt. A transient blip that resolves on retry 2 shouldn't page anyone.
- **Recovery notification**: yes — the next successful run after a prior failure also sends a "back to normal" message, so the team knows it's resolved without checking manually.
- **Pre-flight cleanup**: kill orphaned `EA.exe` processes *before* each run starts, not just after. Extends the existing PID-snapshot pattern already used in `export-and-serve.ps1` / `writeback.ps1`, which today only cleans up post-run.
- **`mkdocs serve` watchdog**: same bounded-retry-then-alert policy applied to the serve process, since it's a separate long-running process from export and can die independently.
- **Scheduling**: a *simple fixed interval* only. Task Scheduler wiring doesn't exist yet and needs to be added, but the "right" interval depends on the deploying organization (model change rate, single-timezone vs. global team, day/night cadence) — that's a genuinely different problem, split out as **#38**. #37 ships with one documented default interval, exposed as a script parameter so it's not hardcoded, and #38 will replace the scheduling *input* without needing to touch the monitoring/alerting/retry logic built here.

## Architecture

The key constraint driving the design: **a broken exporter can't be trusted to report its own breakage.** If `MarkdownExporter` crashes, nothing inside the C# export pipeline can reliably write "export is broken" to the wiki. So logging, the health page, alert dispatch, and retry/backoff all live in the PowerShell wrapper layer — outside and around the `dotnet run` calls — not inside `EAxWiki.Export`.

- **`scripts/monitor-export-and-serve.ps1`** (new) — the orchestrator. Responsibilities:
  - Pre-flight: snapshot and kill orphaned `EA.exe` PIDs (reuses the existing pattern).
  - Run export with bounded retry + backoff; on each attempt, check exit code and a basic output-size sanity check (element/file count vs. a floor, or vs. the previous run's count).
  - Append structured, rotating log lines (timestamp, phase, exit code, tail of `dotnet run` output) to a local log directory.
  - Write/update a small state file (`.eaxwiki-health.json`, gitignored, same pattern as `.eaxwiki-token`) holding last-success time, last-failure time, consecutive-failure count, last exit code.
  - Generate `wiki/status/health.md` directly from that state file — independent of whether export itself succeeded, since this file's whole job is to report when export *didn't*.
  - On final give-up (after bounded retries): dispatch a webhook alert with the log tail. On the first success following a prior failure: dispatch a recovery notification.
  - Wrap `mkdocs serve` with the same watchdog-and-alert treatment.
- **Task Scheduler wiring** (new registration script, e.g. `scripts/register-scheduled-task.ps1`): registers `monitor-export-and-serve.ps1` on a fixed-interval trigger. The wrapper's own retry/backoff happens *within* one invocation, bounded so it can't run past the next scheduled trigger — this keeps Task Scheduler as a simple periodic trigger rather than needing to become a daemon that makes its own scheduling decisions. This is the piece #38 will replace with something timezone/day-night aware.
- **Alert dispatch**: a small PowerShell function (e.g. `Send-Alert -Message ... -Kind Failure|Recovery`) that today only implements the webhook path. Keeping the call site channel-agnostic is what lets email slot in later per the deferred-SMTP decision above.

## Open questions / deferred

- Exact interval value and its config surface are placeholders until #38 lands — ship a documented default (e.g. every 4 hours) as a script parameter.
- SMTP/email wiring — deferred; revisit once a relay or account is available.
- Output-size sanity check threshold (what counts as "suspiciously thin") needs a concrete rule once there's real run history to calibrate against — start conservative (e.g. flag if element count drops below some fraction of the previous successful run).

## New/affected files (not yet implemented)

- `scripts/monitor-export-and-serve.ps1` — new orchestrator wrapper
- `scripts/register-scheduled-task.ps1` — new Task Scheduler registration
- `.eaxwiki-health.json` — new, gitignored, per-instance run-state file (add to `.gitignore` alongside `.eaxwiki-token`)
- `wiki/status/health.md` — new page, written by the wrapper (not the C# exporter)
- A local rotating log directory (gitignored)

## Related

- Follow-up: #38 — configurable, timezone-aware scheduling (replaces the fixed-interval piece above)
- Existing PID-cleanup pattern to extend: `scripts/export-and-serve.ps1`, `scripts/writeback.ps1`
- Existing per-instance secret pattern to follow for `.eaxwiki-health.json`: `ApiTokenStore` / `.eaxwiki-token` (see [docs/design-decisions.md](../../design-decisions.md))
