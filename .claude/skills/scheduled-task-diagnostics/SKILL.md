---
name: scheduled-task-diagnostics
description: Use when the unattended export/serve/monitor pipeline seems to have missed a run, drifted from its intended schedule, or an alert didn't fire — inspects the actual registered Windows Task Scheduler task and the on-disk health/log state together
---

# Scheduled Task Diagnostics

## Overview

`register-scheduled-task.ps1` and `EAxWiki.Monitor.exe` are two independent layers (`scripts/register-scheduled-task.ps1:1-7`: "EAxWiki.Monitor.exe itself is completely unaware [the scheduled task] exists"). Most "the schedule isn't working" reports are actually a mismatch between what one layer intended and what the other is doing. Check both, don't assume either from the script source alone — the registered task can silently diverge from what the script last requested (e.g. someone re-ran it with different flags, or an old registration was never replaced).

## Where to look

1. **The actual registered task** (ground truth for what Task Scheduler will do):
   ```powershell
   Get-ScheduledTask -TaskName "EAxWiki-Monitor" | Format-List *
   Get-ScheduledTask -TaskName "EAxWiki-Monitor" | Select-Object -ExpandProperty Triggers
   Get-ScheduledTaskInfo -TaskName "EAxWiki-Monitor"   # LastRunTime, LastTaskResult, NextRunTime
   ```
   Compare trigger count/type against what was intended: single-interval mode registers one `-Once` trigger with `RepetitionInterval`/`RepetitionDuration`; day/night mode (issue #38) registers two — a 24h/day baseline `-Daily` trigger plus a weekday-only `-Weekly` boost trigger. If `Get-ScheduledTask` shows a different trigger count than expected, the task was likely registered with different flags than you think, or an old registration wasn't replaced by re-running with `-Force`.
   - `WakeToRun` (issue #44, `register-scheduled-task.ps1:48-52`): confirm it's actually on if the machine is expected to wake mid-sleep for a run — `$registered.Settings.WakeToRun`. Off by default expectation mismatch here is exactly the bug fixed in commit `8f71db94`.
   - `MultipleInstances` should be `IgnoreNew` — this is what makes day/night mode's legitimate trigger overlap harmless, and what prevents a slow EA export from stacking runs.

2. **`wiki/status/health.md`** — human-readable pipeline health, regenerated every monitor pass (rendered by `EAxWiki.Monitor`'s `HealthPageRenderer`). Read this first for a quick "Healthy"/"Degraded" verdict, last success/failure times, consecutive failure counts, and `runsSinceForce`.

3. **`.eaxwiki-monitor/<instanceHash>/health.json`** — the underlying state the health page is rendered from, plus fields not shown on the page: `pageReadsToday`/`writebacksToday` (issue #41 daily digest counters) and the `*LogOffset` pairs that track how far the monitor has already scanned `wiki/status/writeback.log` and mkdocs' own serve log. If a digest alert seems to be over/under-counting, check whether an offset looks stale (e.g. pointing past the end of a log file that got rotated/replaced).

4. **Alert delivery** — webhooks resolve in this order: CLI flag → `EAXWIKI_ALERT_WEBHOOK`/`EAXWIKI_ALERT_TEAMS_WEBHOOK` env var → `.eaxwiki` encrypted config (resolved by `EAxWiki.Monitor`'s `MonitorOptionsResolver`). A scheduled run never gets the CLI-flag path deliberately (`register-scheduled-task.ps1:16-19`: Task Scheduler stores action args in a readable way), so if alerts fire when run manually but not from the scheduled task, check the env var is actually set in the *scheduled task's own execution context* (a user's interactive shell env var doesn't carry over) or that `.eaxwiki` is present and decryptable from wherever Task Scheduler actually runs (`SYSTEM`-vs-user context matters for file access).

## Quick triage flow

- Task shows `LastTaskResult` non-zero, or `LastRunTime` far in the past → the registration/trigger layer is broken; fix with `register-scheduled-task.ps1`, not the monitor script.
- Task ran recently and `LastTaskResult` is 0, but `health.md` shows `Degraded` or stale timestamps → the monitor/export layer itself is failing; check `consecutiveFailures`/`serveConsecutiveFailures` and the monitor's own log output for the actual export/serve error.
- Task and health both look fine but no alert arrived → webhook resolution/delivery problem; check which channel(s) are configured and whether the scheduled task's execution context can actually read `.eaxwiki` or see the env var.

## Common mistakes

- Assuming the registered task matches the script's current flags — re-run `register-scheduled-task.ps1` to confirm/replace rather than reading old notes.
- Treating `health.md`'s "Healthy" as proof the *schedule* is firing on time — it only reflects the last run's outcome, not whether Task Scheduler is triggering runs at the intended cadence. Cross-check `NextRunTime`/`LastRunTime` against the configured interval.
- Forgetting day/night mode requires all four flags together (`--work-start`, `--work-end`, `--work-interval-minutes`, `--off-hours-interval-minutes`) — a partial set falls back to simple-interval mode silently unless the script's own validation catches it.
