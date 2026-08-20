# Scheduler UI Updates (Issue #88) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Apply the 14 approved UI changes from issue #88 across all five tabs of the EAxWiki Scheduler GUI.

**Architecture:** Layout/behavior-only changes confined to `src/EAxWiki.SchedulerUI/SchedulerForm.cs`. No data-layer changes. Tasks are grouped by tab so each is independently buildable and reviewable. Verification is `dotnet build` 0 errors + the unchanged .NET test suite (480 baseline).

**Tech Stack:** C# / WinForms (.NET 10, `net10.0-windows`). Webhook tests reuse `scripts/send-alert.ps1` via `PowerShellRunner.RunScriptAsync(scriptPath, List<string> args, repoRoot)` (same call shape as `RegisterAsync` at SchedulerForm.cs:1202).

## Global Constraints

- LF line endings + UTF-8 no BOM for changed files (do not re-encode the whole file; edit only targeted locations matching surrounding style; preserve the file's existing encoding).
- Exact lowercase conventional commit messages listed per task, suffix `(issue #88)`.
- Every task: `dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug --nologo -v q` must show `0 Error(s)` after edits. Set `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\';` before the build. A running SchedulerUI may lock `bin\Debug\net10.0-windows\EAxWiki.SchedulerUI.dll` (MSB3027/MSB3021) — if so, do NOT kill the user's GUI; re-run with `--output C:\Users\hanva\AppData\Local\Temp\opencode\ea-build-verify` and confirm `0 Error(s)` there.
- Do NOT change: `HealthDashboardReader`, `PowerShellRunner`, monitor/exporter logic, or any file outside `src/EAxWiki.SchedulerUI/SchedulerForm.cs`.
- Do NOT stage `bin/`, `obj/`, `.eaxwiki-monitor/*/`, or pre-existing dirty `wiki/**` / `wiki/status/*.md` (monitor re-exports).
- Do NOT push, do NOT run the full test suite, do NOT run the GUI. (Task 7 does push + suite.)
- Existing usings at top of file: `System.Diagnostics`, `System.Net.Http`, `System.Net.Http.Json`, `System.Text.Json`, `EAxWiki.Core.Configuration`, `EAxWiki.EA`. Task 4 adds `using System.Net.Sockets;`.

---

### Task 1: Align the .qea path row and move Test Connection under Browse

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — `BuildConfigTab` (bottom button row ~line 251-254), `BuildRepoTypeSection` (file row ~line 272-277).

**Interfaces:**
- Consumes: existing fields `_browseRepoFileButton`, `_repoFilePathBox`, `_testConnectionButton`.
- Produces: `_testConnectionButton` no longer in the bottom button row; it renders stacked below `_browseRepoFileButton`; the `.qea` label, textbox, and Browse are on one line.

- [ ] **Step 1: Remove `_testConnectionButton` from the bottom button row**

In `BuildConfigTab`, locate:

```csharp
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 8, 0, 0) };
        buttons.Controls.Add(_saveConfigButton);
        buttons.Controls.Add(_testConnectionButton);
        buttons.Controls.Add(_refreshConfigButton);
```

Replace with:

```csharp
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 8, 0, 0) };
        buttons.Controls.Add(_saveConfigButton);
        buttons.Controls.Add(_refreshConfigButton);
```

- [ ] **Step 2: Rebuild the file row as a single horizontal row with Test Connection stacked under Browse**

In `BuildRepoTypeSection`, locate:

```csharp
        var fileRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
        fileRow.Controls.Add(_repoFilePathBox);
        fileRow.Controls.Add(_browseRepoFileButton);
        var fileTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        AddRow(fileTable, "Path to .qea file:", fileRow);
        _repoFilePanel.Controls.Add(fileTable);
```

Replace with:

```csharp
        var browseStack = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        browseStack.Controls.Add(_browseRepoFileButton);
        browseStack.Controls.Add(_testConnectionButton);

        var fileRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
        fileRow.Controls.Add(new Label { Text = "Path to .qea file:", AutoSize = true, Margin = new Padding(3, 6, 10, 3) });
        fileRow.Controls.Add(_repoFilePathBox);
        fileRow.Controls.Add(browseStack);
        _repoFilePanel.Controls.Add(fileRow);
```

This puts the label, textbox, and Browse on one line (item 2), with Test Connection directly underneath Browse (item 1). Remove nothing else in this method.

- [ ] **Step 3: Build and verify it compiles**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug --nologo -v q
```
Expected: `0 Error(s)` (if DLL locked, use the `-o` temp-dir workaround from Global Constraints).

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "refactor(schedulerui): align qea path row and move test connection under browse (issue #88)"
```

- [ ] **Step 5: Verify the diff**

Run `git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs`. Confirm the only changes are: (a) `_testConnectionButton` removed from the bottom buttons row in `BuildConfigTab`; (b) the file row rebuilt in `BuildRepoTypeSection` with the label on the same line and `_testConnectionButton` in `browseStack` below `_browseRepoFileButton`.

---

### Task 2: Per-channel webhook test buttons (Slack / Teams / Telegram)

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — fields (~line 44-47), constructor wiring (~line 144-189), `BuildConfigTab` webhook rows (~line 237-240), new `TestWebhookAsync` method.

**Interfaces:**
- Consumes: `_webhookBox`, `_teamsWebhookBox`, `_telegramBotTokenBox`, `_telegramChatIdBox`; `PowerShellRunner.RunScriptAsync`; `AppendOutput`.
- Produces: three `Test` buttons (`_testSlackButton`, `_testTeamsButton`, `_testTelegramButton`) that call `scripts/send-alert.ps1` for their own channel only.

- [ ] **Step 1: Add the three button fields**

In the field declarations block, after `_telegramChatIdBox` (line 47), add:

```csharp
    private readonly Button _testSlackButton = new() { Text = "Test", AutoSize = true };
    private readonly Button _testTeamsButton = new() { Text = "Test", AutoSize = true };
    private readonly Button _testTelegramButton = new() { Text = "Test", AutoSize = true };
```

- [ ] **Step 2: Wire the buttons in the constructor**

In `SchedulerForm()`, near the other `.Click +=` wiring (after the `_aiSaveButton.Click` line), add:

```csharp
        _testSlackButton.Click += async (_, _) => await TestWebhookAsync("Slack");
        _testTeamsButton.Click += async (_, _) => await TestWebhookAsync("Teams");
        _testTelegramButton.Click += async (_, _) => await TestWebhookAsync("Telegram");
```

- [ ] **Step 3: Place the buttons after their text fields**

In `BuildConfigTab`, locate:

```csharp
        AddRow(table, "Slack Webhook:", _webhookBox);
        AddRow(table, "Teams Webhook:", _teamsWebhookBox);
        AddRow(table, "Telegram Bot Token:", _telegramBotTokenBox);
```

Replace with:

```csharp
        AddRow(table, "Slack Webhook:", MakeBrowseRow(_webhookBox, _testSlackButton));
        AddRow(table, "Teams Webhook:", MakeBrowseRow(_teamsWebhookBox, _testTeamsButton));
        AddRow(table, "Telegram Bot Token:", MakeBrowseRow(_telegramBotTokenBox, _testTelegramButton));
```

(`MakeBrowseRow` already exists at line 479 and returns a `FlowLayoutPanel` containing box + button on one row.)

- [ ] **Step 4: Add the `TestWebhookAsync` method**

Add this method after `TestConnectionAsync` (which ends at line 908):

```csharp
    private async Task TestWebhookAsync(string channel)
    {
        if (_repoRoot == null) return;
        var script = Path.Combine(_repoRoot, "scripts", "send-alert.ps1");
        var args = new List<string> { "-Message", $"Test {channel} webhook from EAxWiki Scheduler.", "-Kind", "Test" };

        switch (channel)
        {
            case "Slack":
                var slack = _webhookBox.Text.Trim();
                if (slack.Length == 0) { AppendOutput("Enter a Slack webhook URL first."); return; }
                args.Add("-WebhookUrl"); args.Add(slack);
                break;
            case "Teams":
                var teams = _teamsWebhookBox.Text.Trim();
                if (teams.Length == 0) { AppendOutput("Enter a Teams webhook URL first."); return; }
                args.Add("-TeamsWebhookUrl"); args.Add(teams);
                break;
            case "Telegram":
                var token = _telegramBotTokenBox.Text.Trim();
                var chatId = _telegramChatIdBox.Text.Trim();
                if (token.Length == 0 || chatId.Length == 0) { AppendOutput("Enter the Telegram bot token and chat ID first."); return; }
                args.Add("-TelegramBotToken"); args.Add(token);
                args.Add("-TelegramChatId"); args.Add(chatId);
                break;
        }

        AppendOutput($"> send-alert.ps1 ({channel} test)");
        var result = await PowerShellRunner.RunScriptAsync(script, args, _repoRoot);
        AppendOutput(result.Output.Length > 0 ? result.Output : $"(no output, exit code {result.ExitCode})");
    }
```

- [ ] **Step 5: Build and verify it compiles**

Run the build command from Task 1 Step 3. Expected: `0 Error(s)`.

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(schedulerui): add per-channel webhook test buttons (issue #88)"
```

- [ ] **Step 7: Verify the diff**

`git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs` — confirm the only additions are the three fields, three Click wirings, the three `MakeBrowseRow` rows, and the `TestWebhookAsync` method.

---

### Task 3: Run Monitor Now clarification + Register/Apply dirty tracking

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — fields (add `_scheduleDirty`), constructor wiring (add `MarkScheduleDirty` hooks), `RunMonitorAsync` (~line 1218-1227), `RefreshTaskStatusAsync` (~line 1030-1041), new `MarkScheduleDirty` method.

**Interfaces:**
- Consumes: `_registerButton`, `_isAdmin`, schedule controls (`_simpleModeRadio`, `_dayNightModeRadio`, `_simpleIntervalMinutes`, `_workStart`, `_workEnd`, `_workIntervalMinutes`, `_offHoursIntervalMinutes`, `_noForceRadio`, `_forceEveryRunRadio`, `_forceEveryNRadio`, `_forceEveryN`, `_wakeToRunCheckbox`).
- Produces: `_registerButton` enables on first schedule-field change and disables after Register/Refresh; `RunMonitorNow` output text clarifies it starts all configured services.

- [ ] **Step 1: Add the dirty-tracking field**

After `_connectionValid` (line 22), add:

```csharp
    private bool _scheduleDirty;
```

- [ ] **Step 2: Wire `MarkScheduleDirty` to all schedule controls**

In `SchedulerForm()`, after the existing `_forceEveryNRadio.CheckedChanged += (_, _) => _forceEveryN.Enabled = _forceEveryNRadio.Checked;` line (170), add:

```csharp
        _simpleModeRadio.CheckedChanged += (_, _) => MarkScheduleDirty();
        _dayNightModeRadio.CheckedChanged += (_, _) => MarkScheduleDirty();
        _simpleIntervalMinutes.ValueChanged += (_, _) => MarkScheduleDirty();
        _workStart.ValueChanged += (_, _) => MarkScheduleDirty();
        _workEnd.ValueChanged += (_, _) => MarkScheduleDirty();
        _workIntervalMinutes.ValueChanged += (_, _) => MarkScheduleDirty();
        _offHoursIntervalMinutes.ValueChanged += (_, _) => MarkScheduleDirty();
        _noForceRadio.CheckedChanged += (_, _) => MarkScheduleDirty();
        _forceEveryRunRadio.CheckedChanged += (_, _) => MarkScheduleDirty();
        _forceEveryNRadio.CheckedChanged += (_, _) => MarkScheduleDirty();
        _forceEveryN.ValueChanged += (_, _) => MarkScheduleDirty();
        _wakeToRunCheckbox.CheckedChanged += (_, _) => MarkScheduleDirty();
```

- [ ] **Step 3: Add the `MarkScheduleDirty` method**

Add this method before `BuildConfigTab` (line 232):

```csharp
    private void MarkScheduleDirty()
    {
        if (!_isAdmin) return;
        _scheduleDirty = true;
        _registerButton.Enabled = true;
    }
```

- [ ] **Step 4: Reset dirty after refresh**

In `RefreshTaskStatusAsync`, after `ApplyScheduleFromTask(root);` (line 1041), add:

```csharp
            ApplyScheduleFromTask(root);
            _scheduleDirty = false;
            _registerButton.Enabled = false;
```

Also in the not-found branch (`_stateValue.Text = "Not registered";`, line 1031), add after `_triggersBox.Text = "";`:

```csharp
                _scheduleDirty = false;
                _registerButton.Enabled = false;
```

- [ ] **Step 5: Clarify Run Monitor Now output text**

In `RunMonitorAsync` (line 1218), replace:

```csharp
        var repoPath = BuildRepoPath();
        if (repoPath.Length == 0)
        {
            AppendOutput("Configure the repository on the Configuration tab first.");
            return;
        }
```

with:

```csharp
        var repoPath = BuildRepoPath();
        if (repoPath.Length == 0)
        {
            AppendOutput("No repository selected. Configure the repository on the Configuration tab first.");
            return;
        }
```

and replace:

```csharp
        AppendOutput($"> Starting monitor in new window...");
```

with:

```csharp
        AppendOutput($"> Starting monitor in new window (starts all configured services: exporter, wiki server, write-back, LLM)...");
```

- [ ] **Step 6: Build and verify it compiles**

Run the build command from Task 1 Step 3. Expected: `0 Error(s)`.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(schedulerui): enable register button on schedule changes (issue #88)"
```

- [ ] **Step 8: Verify the diff**

`git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs` — confirm: `_scheduleDirty` field, the 12 event hooks, `MarkScheduleDirty` method, the two dirty resets in `RefreshTaskStatusAsync`, and the two `RunMonitorAsync` text changes.

---

### Task 4: AI tab — remove Start/Stop LLM, add local probe-start test

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — usings (add `System.Net.Sockets`), fields (remove `_llmStartButton`/`_llmStopButton`/`_llmProcess`), constructor wiring (remove 188-189, keep 165 `_stopLlmButton`), `BuildAiTab` (remove `localButtons` block), `UpdateAiModeEnablement` (remove 704-705), delete `StartLlmAsync` (557-625) and `StopLlm` (627-643), `TestAiConnectionAsync` (branch for local mode), `StopLlmAsync` (remove `_llmProcess = null;` at 1352), `StopAllAsync` (remove `_llmProcess = null;` at 1390), new `TestLocalLlmAsync`.

**Interfaces:**
- Consumes: `_llmModeLocal`, `_llmExeBox`, `_llmModelPathBox`, `_llmPortBox`, `_aiTestButton`, `_aiTestResult`, `AppendOutput`.
- Produces: AI tab has only `Test LLM Connection` + `Save AI Config` buttons; local-mode test probe-starts `llama-server` and waits up to 90s for the port.

- [ ] **Step 1: Add the `System.Net.Sockets` using**

After `using System.Text.Json;` (line 4), add:

```csharp
using System.Net.Sockets;
```

- [ ] **Step 2: Remove the Start/Stop LLM fields**

Remove these three lines (62-64):

```csharp
    private readonly Button _llmStartButton = new() { Text = "Start LLM", AutoSize = true };
    private readonly Button _llmStopButton = new() { Text = "Stop LLM", AutoSize = true, Enabled = false };
    private Process? _llmProcess;
```

- [ ] **Step 3: Remove the Start/Stop wiring**

Remove lines 188-189:

```csharp
        _llmStartButton.Click += async (_, _) => await StartLlmAsync();
        _llmStopButton.Click += (_, _) => StopLlm();
```

(Keep line 165 `_stopLlmButton.Click += async (_, _) => await StopLlmAsync();` — that is the Task Status tab's Stop LLM, which stays.)

- [ ] **Step 4: Remove the local buttons row from the AI tab**

In `BuildAiTab`, locate:

```csharp
        var localButtons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(3, 4, 3, 3) };
        localButtons.Controls.Add(_llmStartButton);
        localButtons.Controls.Add(_llmStopButton);
        localGroup.Controls.Add(localTable);
        localGroup.Controls.Add(localButtons);
        localTable.Location = new Point(6, 16);
        localButtons.Location = new Point(6, localTable.Bottom + 2);
```

Replace with:

```csharp
        localGroup.Controls.Add(localTable);
        localTable.Location = new Point(6, 16);
```

- [ ] **Step 5: Clean up `UpdateAiModeEnablement`**

Remove these two lines (704-705):

```csharp
        _llmStartButton.Enabled = local;
        _llmStopButton.Enabled = local && _llmProcess != null;
```

- [ ] **Step 6: Delete `StartLlmAsync` and `StopLlm` methods**

Delete the entire `StartLlmAsync` method (lines 557-625) and the entire `StopLlm` method (lines 627-643).

- [ ] **Step 7: Remove `_llmProcess` references in `StopLlmAsync` and `StopAllAsync`**

In `StopLlmAsync`, remove the line `        _llmProcess = null;` (line 1352). In `StopAllAsync`, remove the line `        _llmProcess = null;` (line 1390). Keep the `UpdateAiModeEnablement();` calls that follow each.

- [ ] **Step 8: Branch `TestAiConnectionAsync` for local mode**

In `TestAiConnectionAsync`, add at the very top of the method body (before `var endpoint = _aiEndpointBox.Text.Trim();`):

```csharp
        if (_llmModeLocal.Checked)
        {
            await TestLocalLlmAsync();
            return;
        }
```

- [ ] **Step 9: Add the `TestLocalLlmAsync` method**

Add this method immediately after `TestAiConnectionAsync` (which ends at line 554):

```csharp
    private async Task TestLocalLlmAsync()
    {
        var exePath = _llmExeBox.Text.Trim();
        var modelPath = _llmModelPathBox.Text.Trim();
        if (exePath.Length == 0 || modelPath.Length == 0)
        {
            _aiTestResult.Text = "Set both the LLM server executable and model file first.";
            _aiTestResult.ForeColor = Color.Red;
            return;
        }
        if (!File.Exists(exePath))
        {
            _aiTestResult.Text = $"LLM server not found: {exePath}";
            _aiTestResult.ForeColor = Color.Red;
            return;
        }
        if (!File.Exists(modelPath))
        {
            _aiTestResult.Text = $"LLM model not found: {modelPath}";
            _aiTestResult.ForeColor = Color.Red;
            return;
        }

        var port = (int)_llmPortBox.Value;
        _aiTestButton.Enabled = false;
        _aiTestButton.Text = "Starting LLM...";
        _aiTestResult.Text = "";
        AppendOutput($"Probe-starting LLM server: {exePath}");

        var psi = new ProcessStartInfo(exePath, $"-m \"{modelPath}\" -c 4096 --port {port} --n-gpu-layers 0")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        try
        {
            using var process = Process.Start(psi);
            if (process == null)
            {
                _aiTestResult.Text = "Failed to start the LLM server.";
                _aiTestResult.ForeColor = Color.Red;
                return;
            }

            var started = DateTime.UtcNow;
            var reachable = false;
            while ((DateTime.UtcNow - started).TotalSeconds < 90)
            {
                if (process.HasExited) break;
                using var client = new TcpClient();
                try
                {
                    var connectTask = client.ConnectAsync("localhost", port);
                    if (await Task.WhenAny(connectTask, Task.Delay(1000)) == connectTask && client.Connected)
                    {
                        reachable = true;
                        break;
                    }
                }
                catch { /* server not up yet — keep waiting */ }
                await Task.Delay(1000);
            }

            if (reachable)
            {
                _aiTestResult.Text = $"LLM reachable on http://localhost:{port}/v1";
                _aiTestResult.ForeColor = Color.Green;
                AppendOutput($"LLM test successful: http://localhost:{port}/v1");
            }
            else
            {
                _aiTestResult.Text = "LLM server did not accept connections within 90 seconds.";
                _aiTestResult.ForeColor = Color.Red;
            }

            process.Kill(entireProcessTree: true);
        }
        catch (Exception ex)
        {
            _aiTestResult.Text = $"Error: {ex.Message}";
            _aiTestResult.ForeColor = Color.Red;
            AppendOutput($"LLM test failed: {ex.Message}");
        }
        finally
        {
            _aiTestButton.Enabled = true;
            _aiTestButton.Text = "Test LLM Connection";
        }
    }
```

- [ ] **Step 10: Build and verify it compiles**

Run the build command from Task 1 Step 3. Expected: `0 Error(s)`. Note: `Process` may become unused if `StopLlmAsync`/`StopAllAsync` no longer use it — keep `using System.Diagnostics;` only if still needed (it is, for `ProcessStartInfo`/`Process` in the new method; also check `Process.Start` usage elsewhere). Verify no unused-using compile warnings introduced beyond pre-existing ones.

- [ ] **Step 11: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(schedulerui): replace start/stop llm with probe-start test (issue #88)"
```

- [ ] **Step 12: Verify the diff**

`git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs` — confirm: `System.Net.Sockets` added; `_llmStartButton`/`_llmStopButton`/`_llmProcess` gone everywhere; `StartLlmAsync`/`StopLlm` deleted; AI tab has no local buttons row; `TestAiConnectionAsync` branches to `TestLocalLlmAsync`; the two `_llmProcess = null;` lines removed; `_stopLlmButton`/`StopLlmAsync` intact.

---

### Task 5: Task Status tab — align values, friendly triggers, remove Unregister

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — fields (remove `_unregisterButton` line 98), constructor wiring (remove line 167), admin gates (remove `_unregisterButton.Enabled = false;` at 198 and 227), `BuildTaskStatusTab` (remove `_unregisterButton` from buttonRow1; add tooltip), `RefreshTaskStatusAsync` (replace raw triggers with friendly lines), new `FormatTrigger`/`FormatIsoInterval` helpers.

**Interfaces:**
- Consumes: `_stateValue`, `_nextRunValue`, `_triggersBox`, `triggerDetails` JSON (type, `startBoundary`, `intervalIso`, `durationIso` — already returned by the status query at line 1014-1016), `_enableButton`, `_disableButton`.
- Produces: `_stateValue`/`_nextRunValue` vertically aligned with their labels; friendly trigger lines with local time zone; no Unregister button.

- [ ] **Step 1: Remove the Unregister field**

Remove line 98:

```csharp
    private readonly Button _unregisterButton = new() { Text = "Unregister", AutoSize = true };
```

- [ ] **Step 2: Remove the Unregister wiring**

Remove line 167:

```csharp
        _unregisterButton.Click += async (_, _) => await RunTaskCommandAsync("Unregister-ScheduledTask -Confirm:$false");
```

- [ ] **Step 3: Remove the Unregister admin gates**

Remove `            _unregisterButton.Enabled = false;` at line 198 (in the `_repoRoot == null` branch) and at line 227 (in the `!_isAdmin` branch).

- [ ] **Step 4: Remove Unregister from the button row and add a tooltip**

In `BuildTaskStatusTab`, locate:

```csharp
        var buttonRow1 = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow1.Controls.Add(_refreshStatusButton);
        buttonRow1.Controls.Add(_enableButton);
        buttonRow1.Controls.Add(_disableButton);
        buttonRow1.Controls.Add(_unregisterButton);
```

Replace with:

```csharp
        var buttonRow1 = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow1.Controls.Add(_refreshStatusButton);
        buttonRow1.Controls.Add(_enableButton);
        buttonRow1.Controls.Add(_disableButton);
        var buttonTooltip = new ToolTip();
        buttonTooltip.SetToolTip(_enableButton, "Enable the scheduled task and clear skip flags (requires Administrator).");
        buttonTooltip.SetToolTip(_disableButton, "Disable the scheduled task (requires Administrator).");
```

- [ ] **Step 5: Align the State and Next run values with their labels**

In `SchedulerForm()`, after the `_browseRepoFileButton.Click` wiring (around line 152-157), add:

```csharp
        _stateValue.MinimumSize = new Size(0, 23);
        _stateValue.TextAlign = ContentAlignment.MiddleLeft;
        _stateValue.Anchor = AnchorStyles.Left;
        _nextRunValue.MinimumSize = new Size(0, 23);
        _nextRunValue.TextAlign = ContentAlignment.MiddleLeft;
        _nextRunValue.Anchor = AnchorStyles.Left;
```

- [ ] **Step 6: Replace raw triggers with friendly lines**

In `RefreshTaskStatusAsync`, locate:

```csharp
            _stateValue.Text = root.GetProperty("state").GetString() ?? "-";
            _nextRunValue.Text = root.GetProperty("nextRun").GetString() ?? "-";
            var triggerLines = root.GetProperty("triggers").EnumerateArray().Select(t => t.GetString() ?? "");
            _triggersBox.Text = string.Join(Environment.NewLine, triggerLines);
            ApplyScheduleFromTask(root);
```

Replace with:

```csharp
            _stateValue.Text = root.GetProperty("state").GetString() ?? "-";
            _nextRunValue.Text = root.GetProperty("nextRun").GetString() ?? "-";
            var triggerLines = root.GetProperty("triggerDetails").EnumerateArray().Select(FormatTrigger);
            _triggersBox.Text = string.Join(Environment.NewLine, triggerLines);
            ApplyScheduleFromTask(root);
```

- [ ] **Step 7: Add the `FormatTrigger` and `FormatIsoInterval` helpers**

Add these methods immediately after `RefreshTaskStatusAsync` (which ends at line 1051):

```csharp
    private static string FormatTrigger(JsonElement t)
    {
        var type = t.TryGetProperty("type", out var tp) ? tp.GetString() ?? "" : "";
        var start = t.TryGetProperty("startBoundary", out var sb) ? sb.GetString() ?? "" : "";
        var interval = t.TryGetProperty("intervalIso", out var iv) ? iv.GetString() ?? "" : "";

        var kind = type switch
        {
            "MSFT_TaskDailyTrigger" => "Daily",
            "MSFT_TaskWeeklyTrigger" => "Weekly",
            "MSFT_TaskTimeTrigger" => "Once",
            _ => type,
        };

        var when = "";
        if (DateTimeOffset.TryParse(start, out var startOffset))
            when = $" — starts {startOffset.ToLocalTime():g}";

        var intervalText = FormatIsoInterval(interval);
        var tz = TimeZoneInfo.Local.StandardName;
        return intervalText.Length > 0 ? $"{kind}, every {intervalText}{when} ({tz})" : $"{kind}{when} ({tz})";
    }

    private static string FormatIsoInterval(string iso)
    {
        if (string.IsNullOrEmpty(iso)) return "";
        if (!iso.StartsWith("PT", StringComparison.Ordinal)) return iso;

        var hours = 0;
        var minutes = 0;
        var seconds = 0;
        var num = "";
        foreach (var ch in iso[2..])
        {
            if (ch >= '0' && ch <= '9') { num += ch; continue; }
            if (int.TryParse(num, out var v))
            {
                switch (ch)
                {
                    case 'H': hours = v; break;
                    case 'M': minutes = v; break;
                    case 'S': seconds = v; break;
                }
            }
            num = "";
        }

        var parts = new List<string>();
        if (hours > 0) parts.Add($"{hours}h");
        if (minutes > 0) parts.Add($"{minutes}m");
        if (seconds > 0) parts.Add($"{seconds}s");
        return parts.Count > 0 ? string.Join(" ", parts) : iso;
    }
```

- [ ] **Step 8: Build and verify it compiles**

Run the build command from Task 1 Step 3. Expected: `0 Error(s)`.

- [ ] **Step 9: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(schedulerui): friendly task triggers and aligned status values (issue #88)"
```

- [ ] **Step 10: Verify the diff**

`git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs` — confirm: `_unregisterButton` removed everywhere (field, wiring, two admin gates, buttonRow1); tooltip added; `_stateValue`/`_nextRunValue` alignment lines; `FormatTrigger`/`FormatIsoInterval` helpers; `_triggersBox` now fed from `triggerDetails`.

---

### Task 6: Health Dashboard — auto-refresh on open + flip layout (table on top)

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — fields (add `_dashboardRefreshed`, `_dashboardTab`, `_dashboardSplit`), constructor (wire `tabs.SelectedIndexChanged`), `BuildDashboardTab` (flip the SplitContainer).

**Interfaces:**
- Consumes: `_dashboardGrid`, `_refreshDashboardButton`, `RefreshDashboard()`, `tabs` (`TabControl` created at line 131).
- Produces: grid fills `Panel1` (top), Refresh row in `Panel2` (bottom); first selection of the tab auto-refreshes.

- [ ] **Step 1: Add fields**

After the `_dashboardGrid` field (line 106-111), add:

```csharp
    private bool _dashboardRefreshed;
    private TabPage? _dashboardTab;
    private SplitContainer? _dashboardSplit;
```

- [ ] **Step 2: Wire the first-select auto-refresh**

In `SchedulerForm()`, after the tab pages are added (line 136), add:

```csharp
        tabs.SelectedIndexChanged += (_, _) =>
        {
            if (tabs.SelectedTab != _dashboardTab || _dashboardRefreshed) return;
            _dashboardRefreshed = true;
            if (_dashboardSplit != null)
                _dashboardSplit.SplitterDistance = Math.Max(40, _dashboardSplit.Height - 40);
            RefreshDashboard();
        };
```

- [ ] **Step 3: Flip the SplitContainer in `BuildDashboardTab`**

Locate the current `BuildDashboardTab`:

```csharp
    private TabPage BuildDashboardTab()
    {
        var buttonRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow.Controls.Add(_refreshDashboardButton);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterWidth = 6,
            Panel1MinSize = 28, // Refresh button row
        };
        split.Panel1.Controls.Add(buttonRow);
        split.Panel1.AutoScroll = true;
        split.Panel2.Controls.Add(_dashboardGrid);
        split.SplitterDistance = 40; // button row only; the grid gets the rest

        _refreshDashboardButton.Click += (_, _) => RefreshDashboard();
        return new TabPage("Health Dashboard") { Padding = new Padding(10), Controls = { split } };
    }
```

Replace with:

```csharp
    private TabPage BuildDashboardTab()
    {
        var buttonRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow.Controls.Add(_refreshDashboardButton);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterWidth = 6,
            Panel2MinSize = 28, // Refresh button row
        };
        split.Panel1.Controls.Add(_dashboardGrid);
        split.Panel2.Controls.Add(buttonRow);
        split.Panel2.AutoScroll = true;
        _dashboardSplit = split;

        _refreshDashboardButton.Click += (_, _) => RefreshDashboard();
        _dashboardTab = new TabPage("Health Dashboard") { Padding = new Padding(10), Controls = { split } };
        return _dashboardTab;
    }
```

Note: the splitter position is set in the first-select handler (Step 2), which runs after the tab is laid out and has a real height. `RefreshDashboard()` guards `if (_repoRoot == null) return;` at its top, so it is safe at first selection.

- [ ] **Step 4: Build and verify it compiles**

Run the build command from Task 1 Step 3. Expected: `0 Error(s)`.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(schedulerui): auto-refresh and flip health dashboard layout (issue #88)"
```

- [ ] **Step 6: Verify the diff**

`git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs` — confirm: three new fields, `SelectedIndexChanged` handler, and the flipped `BuildDashboardTab` (grid in `Panel1`, button row in `Panel2`, splitter distance set on first select).

---

### Task 7: Whole-branch verification, push, issue comment

**Files:**
- None (verification only).

**Interfaces:**
- Consumes: Tasks 1-6 results (commits `refactor(schedulerui): align qea path row...`, `feat(schedulerui): add per-channel webhook test buttons...`, `feat(schedulerui): enable register button on schedule changes...`, `feat(schedulerui): replace start/stop llm with probe-start test...`, `feat(schedulerui): friendly task triggers and aligned status values...`, `feat(schedulerui): auto-refresh and flip health dashboard layout...`, all `(issue #88)`).

- [ ] **Step 1: Build the full solution**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build EAxWiki.slnx --configuration Debug --nologo -v q
```
Expected: `0 Error(s)` (use the `-o` temp-dir workaround if the running Monitor/UI lock `bin` DLLs).

- [ ] **Step 2: Run the .NET test suite**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet test src\EAxWiki.Tests\EAxWiki.Tests.csproj --configuration Debug --nologo -v q
```
Expected: all pass, 0 failed (480 tests at baseline; no tests touch the SchedulerUI layout). Use the `--output` temp-dir workaround if DLL locks block the build phase. If a known pre-existing flake (FsCheck `EscapeCell` CRLF, `ProcessSupervisorTests` port-probe) appears, re-run that test in isolation and confirm it passes.

- [ ] **Step 3: Push to origin/master**

```bash
git push origin master
```

- [ ] **Step 4: Comment on issue #88**

Post a short comment listing the six feature commits and their scope, noting verification results (build 0 errors, .NET suite pass count).