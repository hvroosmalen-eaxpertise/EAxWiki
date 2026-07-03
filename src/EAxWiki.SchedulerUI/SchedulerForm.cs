using System.Text.Json;
using EAxWiki.Core.Configuration;

namespace EAxWiki.SchedulerUI;

/// <summary>
/// Settings GUI for scheduling only (issue #38's slice of the broader unified-config ask in #40).
/// Shows the current .eaxwiki repo/port config read-only for context, shows the currently
/// registered EAxWiki-Monitor scheduled task's state, and lets you construct + register either a
/// simple fixed-interval schedule or a day/night (weekday work-hours boost + all-day baseline)
/// schedule. All registration/query logic is delegated to scripts/register-scheduled-task.ps1 and
/// plain Get-ScheduledTask calls via pwsh.exe — this form never touches Task Scheduler directly.
/// </summary>
public class SchedulerForm : Form
{
    private readonly string? _repoRoot;

    // Current config display
    private readonly Label _repoValue = new() { AutoSize = true, Text = "(not found)" };
    private readonly Label _wikiPortValue = new() { AutoSize = true, Text = "-" };
    private readonly Label _apiPortValue = new() { AutoSize = true, Text = "-" };
    private readonly Label _webhookValue = new() { AutoSize = true, Text = "-" };

    // Task status display
    private readonly TextBox _taskNameBox = new() { Text = "EAxWiki-Monitor", Width = 220 };
    private readonly Label _stateValue = new() { AutoSize = true, Text = "-" };
    private readonly Label _nextRunValue = new() { AutoSize = true, Text = "-" };
    private readonly TextBox _triggersBox = new() { Multiline = true, ReadOnly = true, Height = 70, Width = 560, ScrollBars = ScrollBars.Vertical };

    // Schedule mode
    private readonly RadioButton _simpleModeRadio = new() { Text = "Simple interval", Checked = true, AutoSize = true };
    private readonly RadioButton _dayNightModeRadio = new() { Text = "Day / Night", AutoSize = true };
    private readonly NumericUpDown _simpleIntervalMinutes = new() { Minimum = 5, Maximum = 100000, Value = 240, Width = 80 };

    private readonly DateTimePicker _workStart = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Value = DateTime.Today.AddHours(8) };
    private readonly DateTimePicker _workEnd = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Value = DateTime.Today.AddHours(18) };
    private readonly NumericUpDown _workIntervalMinutes = new() { Minimum = 5, Maximum = 100000, Value = 10, Width = 80 };
    private readonly NumericUpDown _offHoursIntervalMinutes = new() { Minimum = 5, Maximum = 100000, Value = 240, Width = 80 };

    // Force mode
    private readonly RadioButton _noForceRadio = new() { Text = "Incremental (default)", Checked = true, AutoSize = true };
    private readonly RadioButton _forceEveryRunRadio = new() { Text = "Force every run", AutoSize = true };
    private readonly RadioButton _forceEveryNRadio = new() { Text = "Force every N runs, N =", AutoSize = true };
    private readonly NumericUpDown _forceEveryN = new() { Minimum = 2, Maximum = 100000, Value = 5, Width = 80, Enabled = false };

    private readonly NumericUpDown _portBox = new() { Minimum = 1, Maximum = 65535, Value = 8000, Width = 80 };

    private readonly TextBox _outputBox = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Font = new Font(FontFamily.GenericMonospace, 9) };
    private readonly Button _registerButton = new() { Text = "Register / Apply Schedule", AutoSize = true };
    private readonly Button _enableButton = new() { Text = "Enable", AutoSize = true };
    private readonly Button _disableButton = new() { Text = "Disable", AutoSize = true };
    private readonly Button _unregisterButton = new() { Text = "Unregister", AutoSize = true };
    private readonly Button _refreshStatusButton = new() { Text = "Refresh Status", AutoSize = true };
    private readonly Button _refreshConfigButton = new() { Text = "Refresh", AutoSize = true };

    public SchedulerForm()
    {
        _repoRoot = RepoLocator.FindRepoRoot();

        Text = "EAxWiki Scheduler";
        MinimumSize = new Size(700, 600);
        Size = new Size(700, 600);
        Padding = new Padding(10);

        // Output is deliberately not a tab — it shows results from actions taken on any tab
        // (Register, Enable, Disable, Refresh...), so it stays visible underneath all of them
        // rather than being scoped to whichever tab happens to be selected.
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 35));

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildConfigTab());
        tabs.TabPages.Add(BuildTaskStatusTab());
        tabs.TabPages.Add(BuildScheduleTab());

        root.Controls.Add(tabs, 0, 0);
        root.Controls.Add(BuildOutputGroup(), 0, 1);

        Controls.Add(root);

        _refreshConfigButton.Click += (_, _) => LoadEaxwikiConfig();
        _refreshStatusButton.Click += async (_, _) => await RefreshTaskStatusAsync();
        _registerButton.Click += async (_, _) => await RegisterAsync();
        _enableButton.Click += async (_, _) => await RunTaskCommandAsync("Enable-ScheduledTask");
        _disableButton.Click += async (_, _) => await RunTaskCommandAsync("Disable-ScheduledTask");
        _unregisterButton.Click += async (_, _) => await RunTaskCommandAsync("Unregister-ScheduledTask -Confirm:$false");

        _simpleModeRadio.CheckedChanged += (_, _) => UpdateModeEnablement();
        _forceEveryNRadio.CheckedChanged += (_, _) => _forceEveryN.Enabled = _forceEveryNRadio.Checked;

        if (_repoRoot == null)
        {
            AppendOutput("Could not locate the EAxWiki repo root (looked for scripts/register-scheduled-task.ps1 above this exe's folder). Registration and status queries are disabled.");
            _registerButton.Enabled = false;
            _refreshStatusButton.Enabled = false;
            _enableButton.Enabled = false;
            _disableButton.Enabled = false;
            _unregisterButton.Enabled = false;
        }
        else
        {
            LoadEaxwikiConfig();
        }

        UpdateModeEnablement();
    }

    private TabPage BuildConfigTab()
    {
        var table = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Dock = DockStyle.Top };
        AddRow(table, "Repository:", _repoValue);
        AddRow(table, "Wiki port:", _wikiPortValue);
        AddRow(table, "API port:", _apiPortValue);
        AddRow(table, "Slack Webhook:", _webhookValue);

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown };
        panel.Controls.Add(table);
        panel.Controls.Add(_refreshConfigButton);

        return new TabPage("Configuration") { Padding = new Padding(10), AutoScroll = true, Controls = { panel } };
    }

    private TabPage BuildTaskStatusTab()
    {
        var table = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Dock = DockStyle.Top };
        AddRow(table, "Task name:", _taskNameBox);
        AddRow(table, "State:", _stateValue);
        AddRow(table, "Next run:", _nextRunValue);
        AddRow(table, "Triggers:", _triggersBox);

        var buttons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttons.Controls.Add(_refreshStatusButton);
        buttons.Controls.Add(_enableButton);
        buttons.Controls.Add(_disableButton);
        buttons.Controls.Add(_unregisterButton);

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown };
        panel.Controls.Add(table);
        panel.Controls.Add(buttons);

        return new TabPage("Task Status") { Padding = new Padding(10), AutoScroll = true, Controls = { panel } };
    }

    private TabPage BuildScheduleTab()
    {
        var modeRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        modeRow.Controls.Add(_simpleModeRadio);
        modeRow.Controls.Add(_dayNightModeRadio);

        var simpleTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        AddRow(simpleTable, "Interval (minutes):", _simpleIntervalMinutes);

        var dayNightTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        AddRow(dayNightTable, "Work start (HH:mm):", _workStart);
        AddRow(dayNightTable, "Work end (HH:mm):", _workEnd);
        AddRow(dayNightTable, "Work interval (min):", _workIntervalMinutes);
        AddRow(dayNightTable, "Off-hours interval (min):", _offHoursIntervalMinutes);

        var forceRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        forceRow.Controls.Add(_noForceRadio);
        forceRow.Controls.Add(_forceEveryRunRadio);
        forceRow.Controls.Add(_forceEveryNRadio);
        forceRow.Controls.Add(_forceEveryN);

        var portTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        AddRow(portTable, "Wiki port:", _portBox);

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        panel.Controls.Add(modeRow);
        panel.Controls.Add(simpleTable);
        panel.Controls.Add(dayNightTable);
        panel.Controls.Add(new Label { Text = "Export mode:", AutoSize = true, Margin = new Padding(3, 10, 3, 3) });
        panel.Controls.Add(forceRow);
        panel.Controls.Add(portTable);

        // Kept out of the FlowLayoutPanel above deliberately: with TopDown flow, once content
        // exceeds the available height the panel wraps into a new column instead of scrolling,
        // which pushed this button to the top-right. Docked to the bottom and right-aligned here
        // instead, so it stays put regardless of how much the panel above needs to scroll.
        var buttonRow = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 8, 0, 0) };
        buttonRow.Controls.Add(_registerButton);

        var tabPage = new TabPage("Schedule Settings") { Padding = new Padding(10), AutoScroll = true };
        tabPage.Controls.Add(panel);
        tabPage.Controls.Add(buttonRow);
        return tabPage;
    }

    private GroupBox BuildOutputGroup()
    {
        return new GroupBox { Text = "Output", Dock = DockStyle.Fill, Padding = new Padding(8), Controls = { _outputBox } };
    }

    private static void AddRow(TableLayoutPanel table, string caption, Control value)
    {
        var row = table.RowCount;
        table.RowCount = row + 1;
        table.Controls.Add(new Label { Text = caption, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 6, 3, 3) }, 0, row);
        value.Margin = new Padding(3, 3, 3, 3);
        table.Controls.Add(value, 1, row);
    }

    private void UpdateModeEnablement()
    {
        var simple = _simpleModeRadio.Checked;
        _simpleIntervalMinutes.Enabled = simple;
        _workStart.Enabled = !simple;
        _workEnd.Enabled = !simple;
        _workIntervalMinutes.Enabled = !simple;
        _offHoursIntervalMinutes.Enabled = !simple;
    }

    private void AppendOutput(string text)
    {
        _outputBox.AppendText(text.TrimEnd('\r', '\n') + Environment.NewLine + Environment.NewLine);
    }

    private void LoadEaxwikiConfig()
    {
        if (_repoRoot == null) return;
        var path = Path.Combine(_repoRoot, ".eaxwiki");
        if (!File.Exists(path))
        {
            _repoValue.Text = "(no .eaxwiki found)";
            _wikiPortValue.Text = "-";
            _apiPortValue.Text = "-";
            _webhookValue.Text = "-";
            return;
        }

        try
        {
            var config = LocalConfigStore.Load(path, out _);
            _repoValue.Text = string.IsNullOrEmpty(config.RepoPath) ? "(not set)" : config.RepoPath;
            _wikiPortValue.Text = config.WikiPort?.ToString() ?? "(default 8000)";
            _apiPortValue.Text = config.ApiPort?.ToString() ?? "(default 8001)";
            _webhookValue.Text = string.IsNullOrEmpty(config.WebhookUrl) ? "Not configured" : "Configured";
            if (config.WikiPort.HasValue)
                _portBox.Value = config.WikiPort.Value;
        }
        catch (Exception ex)
        {
            _repoValue.Text = "(failed to read)";
            AppendOutput($"Failed to read .eaxwiki: {ex.Message}");
        }
    }

    private async Task RefreshTaskStatusAsync()
    {
        if (_repoRoot == null) return;
        var taskName = _taskNameBox.Text.Trim();
        if (taskName.Length == 0) return;

        _refreshStatusButton.Enabled = false;
        try
        {
            // Query state/next-run/triggers via a single script call, JSON-serialized, so we don't
            // have to parse PowerShell's default table formatting in C#.
            var command = $$"""
                $task = Get-ScheduledTask -TaskName '{{taskName}}' -ErrorAction SilentlyContinue
                if (-not $task) { @{ found = $false } | ConvertTo-Json; exit 0 }
                $info = Get-ScheduledTaskInfo -TaskName '{{taskName}}'
                $triggers = @($task.Triggers | ForEach-Object {
                    $t = $_
                    $days = if ($t.DaysOfWeek) { " days=$($t.DaysOfWeek)" } else { "" }
                    "$($t.CimClass.CimClassName) at=$($t.StartBoundary)$days interval=$($t.Repetition.Interval) duration=$($t.Repetition.Duration)"
                })
                @{ found = $true; state = [string]$task.State; nextRun = [string]$info.NextRunTime; triggers = $triggers } | ConvertTo-Json
                """;
            var result = await PowerShellRunner.RunCommandAsync(command, _repoRoot);
            if (result.ExitCode != 0)
            {
                AppendOutput($"Failed to query task status (exit {result.ExitCode}):\n{result.Output}");
                return;
            }

            using var doc = JsonDocument.Parse(result.Output);
            var root = doc.RootElement;
            if (!root.GetProperty("found").GetBoolean())
            {
                _stateValue.Text = "Not registered";
                _nextRunValue.Text = "-";
                _triggersBox.Text = "";
                return;
            }

            _stateValue.Text = root.GetProperty("state").GetString() ?? "-";
            _nextRunValue.Text = root.GetProperty("nextRun").GetString() ?? "-";
            var triggerLines = root.GetProperty("triggers").EnumerateArray().Select(t => t.GetString() ?? "");
            _triggersBox.Text = string.Join(Environment.NewLine, triggerLines);
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to query task status: {ex.Message}");
        }
        finally
        {
            _refreshStatusButton.Enabled = true;
        }
    }

    private async Task RunTaskCommandAsync(string cmdlet)
    {
        if (_repoRoot == null) return;
        var taskName = _taskNameBox.Text.Trim();
        if (taskName.Length == 0) return;

        var command = $"{cmdlet} -TaskName '{taskName}'";
        AppendOutput($"> {command}");
        var result = await PowerShellRunner.RunCommandAsync(command, _repoRoot);
        AppendOutput(result.Output.Length > 0 ? result.Output : $"(no output, exit code {result.ExitCode})");
        await RefreshTaskStatusAsync();
    }

    private async Task RegisterAsync()
    {
        if (_repoRoot == null) return;

        var args = new List<string> { "--task-name", _taskNameBox.Text.Trim(), "--port", ((int)_portBox.Value).ToString() };

        if (_dayNightModeRadio.Checked)
        {
            args.AddRange([
                "--work-start", _workStart.Value.ToString("HH:mm"),
                "--work-end", _workEnd.Value.ToString("HH:mm"),
                "--work-interval-minutes", ((int)_workIntervalMinutes.Value).ToString(),
                "--off-hours-interval-minutes", ((int)_offHoursIntervalMinutes.Value).ToString(),
            ]);
        }
        else
        {
            args.AddRange(["--interval-minutes", ((int)_simpleIntervalMinutes.Value).ToString()]);
        }

        if (_forceEveryRunRadio.Checked)
            args.Add("--force");
        else if (_forceEveryNRadio.Checked)
            args.AddRange(["--force-every", ((int)_forceEveryN.Value).ToString()]);

        var scriptPath = Path.Combine(_repoRoot, "scripts", "register-scheduled-task.ps1");
        AppendOutput($"> register-scheduled-task.ps1 {string.Join(' ', args)}");

        _registerButton.Enabled = false;
        try
        {
            var result = await PowerShellRunner.RunScriptAsync(scriptPath, args, _repoRoot);
            AppendOutput(result.Output);
            AppendOutput(result.ExitCode == 0 ? "Succeeded." : $"Failed (exit code {result.ExitCode}).");
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to run register-scheduled-task.ps1: {ex.Message}");
        }
        finally
        {
            _registerButton.Enabled = true;
        }

        await RefreshTaskStatusAsync();
    }
}
