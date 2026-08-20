# Scheduler UI Updates (Issue #88) Design

**Date:** 2026-08-20

**Status:** Approved by user (consolidated decisions from brainstorming Q&A).

## Problem

Issue #88 requests 14 UI changes across all five tabs of the EAxWiki Scheduler GUI (`src/EAxWiki.SchedulerUI/SchedulerForm.cs`). All changes are layout/behavior changes to this single WinForms form.

## Design

### Configuration tab

1. **Test Connection under Browse** — Move `_testConnectionButton` out of the bottom button row (`BuildConfigTab` line 253) and stack it vertically below `_browseRepoFileButton` inside the `.qea` file row (`BuildRepoTypeSection`). The bottom button row keeps Save Configuration + Refresh.

2. **`.qea` path label on the same row** — Replace the `AddRow`/`TableLayoutPanel` construction for the file path (`BuildRepoTypeSection` line 275-277) with a single horizontal `FlowLayoutPanel` (`WrapContents = false`) holding label + `_repoFilePathBox` + `_browseRepoFileButton` inline, matching how the repository-type row (`typeRow`) is built. This guarantees the label, field, and Browse sit on one line.

3-5. **Per-channel webhook test buttons** — Add a `Test` button after each of: `Slack Webhook` (`_webhookBox`), `Teams Webhook` (`_teamsWebhookBox`), `Telegram Bot Token` (`_telegramBotTokenBox`). Pressing one calls `scripts/send-alert.ps1` with only that channel's configured value (via the existing `PowerShellRunner`), sending a real test message. Only the relevant channel is exercised.

### Schedule Settings tab

6. **Run Monitor Now** — Behavior unchanged (it already launches `EAxWiki.Monitor.exe`, which starts export/serve/writeback/LLM from `.eaxwiki`). Update the output text to state it starts all configured services, and short-circuit with a clear message when no repository path is configured (already partially present at line 1222-1227).

7. **Register/Apply dirty tracking** — The button is enabled whenever any schedule field differs from the values of the registered task as last loaded (`ApplyScheduleFromTask`), and greyed again after Register or Refresh completes. Tracked fields: mode (simple vs day/night), interval(s), force mode, wake-to-run. Reuse the loaded `triggerDetails` snapshot as the baseline.

### AI LLM tab

8. **Remove Start/Stop LLM** — Delete `_llmStartButton`, `_llmStopButton`, `_llmProcess`, `StartLlmAsync`, `StopLlm`, and the Start/Stop wiring (lines 62-64, 188-189, and the enablement logic in `UpdateAiModeEnablement`). The runtime now starts the LLM; the GUI should not manage the process directly.

9. **Test LLM Connection stays** — In **Local** mode it probe-starts `llama-server` from the configured exe/model/port, waits up to ~90s for the port to accept a connection, then reports. In **Remote** mode it keeps the current HTTP `/chat/completions` probe (line 487-554) with its existing 30s timeout. Result rendered in `_aiTestResult`.

### Task Status tab

10. **Align State / Next run values** — `State:` and `Next run:` rows already use `AddRow` (a 2-column TableLayoutPanel). Ensure `_stateValue` and `_nextRunValue` are anchored left and vertically centered against the label (consistent row height, `MinimumSize`/`Anchor` alignment).

11. **Friendly trigger text** — Replace the raw `triggers` strings (line 1039-1040) with friendly lines formatted from `triggerDetails` (type, `startBoundary`, `intervalIso`, `durationIso`), including local time zone, e.g. `Daily, every 3h 59m — starts 8/20/2026 1:19 PM (UTC+2)`. `triggerDetails` is already returned by the status query (line 1014-1016) and consumed by `ApplyScheduleFromTask`.

12. **Remove Unregister** — Delete `_unregisterButton` and its wiring (line 98, 167). Keep Enable/Disable, adding tooltips that explain they manage the scheduled task and require Administrator privileges.

### Health Dashboard tab

13. **Auto-refresh on first open** — Call `RefreshDashboard()` once the first time the Health Dashboard tab is selected (guard on `TabControl.SelectedIndexChanged`).

14. **Flip layout** — The `SplitContainer` in `BuildDashboardTab` gets the grid in `Panel1` (top, `Fill`) and the Refresh button row in `Panel2` (bottom), reversing the current arrangement (button row on top).

## Non-goals

- No changes to `HealthDashboardReader`, monitor logic, or PowerShell scripts.
- No new automated tests (layout-only GUI changes; verification is `dotnet build` 0 errors + manual GUI check).
- No push until all changes verified.

## Verification

- `dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug` → 0 errors.
- Full-solution build + .NET test suite (480 baseline) unchanged, 0 failed.
- Manual GUI check of all five tabs.