using System.Text.Json;
using EAxWiki.Core.Configuration;

namespace EAxWiki.SchedulerUI;

/// <summary>
/// Settings GUI for scheduling only (issue #38's slice of the broader unified-config ask in #40).
/// Shows and edits the current .eaxwiki repo/port/webhook config, shows the currently registered
/// EAxWiki-Monitor scheduled task's state, and lets you construct + register either a simple
/// fixed-interval schedule or a day/night (weekday work-hours boost + all-day baseline) schedule.
/// All registration/query logic is delegated to scripts/register-scheduled-task.ps1 and plain
/// Get-ScheduledTask calls via pwsh.exe — this form never touches Task Scheduler directly.
/// </summary>
public class SchedulerForm : Form
{
    private readonly string? _repoRoot;

    // Current config, editable
    // Repository type mirrors the console wizard's choice (BuildConnectionStringInteractively in
    // EAxWiki/Program.cs) so the two entry points produce identical .eaxwiki connection strings.
    private readonly RadioButton _repoTypeFile = new() { Text = "File (.qea)", AutoSize = true, Checked = true };
    private readonly RadioButton _repoTypeSqlServer = new() { Text = "SQL Server", AutoSize = true };
    private readonly RadioButton _repoTypeMySql = new() { Text = "MySQL / MariaDB", AutoSize = true };
    private readonly RadioButton _repoTypeOracle = new() { Text = "Oracle", AutoSize = true };
    private readonly RadioButton _repoTypePostgres = new() { Text = "PostgreSQL", AutoSize = true };
    private readonly TextBox _repoFilePathBox = new() { Width = 340 };
    private readonly Button _browseRepoFileButton = new() { Text = "Browse...", AutoSize = true };
    private readonly TextBox _dbServerBox = new() { Width = 200 };
    private readonly TextBox _dbPortBox = new() { Width = 200 };
    private readonly TextBox _dbDatabaseBox = new() { Width = 200 };
    private readonly TextBox _dbUserBox = new() { Width = 200 };
    private readonly TextBox _dbPasswordBox = new() { Width = 200, UseSystemPasswordChar = true };
    private readonly Panel _repoFilePanel = new() { AutoSize = true };
    private readonly Panel _dbFieldsPanel = new() { AutoSize = true };

    private readonly NumericUpDown _wikiPortConfigBox = new() { Minimum = 1, Maximum = 65535, Value = 8000, Width = 80 };
    private readonly NumericUpDown _apiPortConfigBox = new() { Minimum = 1, Maximum = 65535, Value = 8001, Width = 80 };
    private readonly TextBox _webhookBox = new() { Width = 400 };
    private readonly TextBox _teamsWebhookBox = new() { Width = 400 };
    private readonly Button _saveConfigButton = new() { Text = "Save Configuration", AutoSize = true };

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
        // Tall enough that the Configuration tab's repo-type section (up to 5 DB fields, plus
        // ports and both webhooks) fits without scrolling at the 65/35 tab/output split below.
        MinimumSize = new Size(700, 780);
        Size = new Size(700, 780);
        Padding = new Padding(10);

        // Output is deliberately not a tab — it shows results from actions taken on any tab
        // (Register, Enable, Disable, Refresh...), so it stays visible underneath all of them
        // rather than being scoped to whichever tab happens to be selected.
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 35));

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildConfigTab());
        tabs.TabPages.Add(BuildScheduleTab());
        tabs.TabPages.Add(BuildTaskStatusTab());

        root.Controls.Add(tabs, 0, 0);
        root.Controls.Add(BuildOutputGroup(), 0, 1);

        Controls.Add(root);

        _refreshConfigButton.Click += (_, _) => LoadEaxwikiConfig();
        _saveConfigButton.Click += (_, _) => SaveEaxwikiConfig();
        _repoTypeFile.CheckedChanged += (_, _) => UpdateRepoTypeEnablement();
        _repoTypeSqlServer.CheckedChanged += (_, _) => UpdateRepoTypeEnablement();
        _repoTypeMySql.CheckedChanged += (_, _) => UpdateRepoTypeEnablement();
        _repoTypeOracle.CheckedChanged += (_, _) => UpdateRepoTypeEnablement();
        _repoTypePostgres.CheckedChanged += (_, _) => UpdateRepoTypeEnablement();
        _browseRepoFileButton.Click += (_, _) =>
        {
            using var dialog = new OpenFileDialog { Filter = "EA project files (*.qea)|*.qea|All files (*.*)|*.*" };
            if (dialog.ShowDialog() == DialogResult.OK)
                _repoFilePathBox.Text = dialog.FileName;
        };
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
            // Fire-and-forget: reflects whatever schedule is actually registered on open, rather
            // than only after the user manually clicks Refresh Status. Constructors can't be async.
            _ = RefreshTaskStatusAsync();
        }

        UpdateModeEnablement();
        UpdateRepoTypeEnablement();
    }

    private TabPage BuildConfigTab()
    {
        var table = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Dock = DockStyle.Top };
        AddRow(table, "Wiki port:", _wikiPortConfigBox);
        AddRow(table, "API port:", _apiPortConfigBox);
        AddRow(table, "Slack Webhook:", _webhookBox);
        AddRow(table, "Teams Webhook:", _teamsWebhookBox);

        // WrapContents = false and no AutoSize here, matching BuildScheduleTab: with AutoSize +
        // TopDown flow, once content exceeds the tab's visible height the panel wraps into a new
        // column instead of scrolling, shoving everything after it off the right edge of the
        // window. The buttons are kept out of this panel and docked to the tab bottom instead, so
        // they stay put regardless of how tall the panel above needs to scroll.
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        panel.Controls.Add(BuildRepoTypeSection());
        panel.Controls.Add(table);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 8, 0, 0) };
        buttons.Controls.Add(_saveConfigButton);
        buttons.Controls.Add(_refreshConfigButton);

        var tabPage = new TabPage("Configuration") { Padding = new Padding(10), AutoScroll = true };
        tabPage.Controls.Add(panel);
        tabPage.Controls.Add(buttons);
        return tabPage;
    }

    private Control BuildRepoTypeSection()
    {
        var typeRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        typeRow.Controls.Add(new Label { Text = "Repository type:", AutoSize = true, Margin = new Padding(3, 6, 10, 3) });
        typeRow.Controls.Add(_repoTypeFile);
        typeRow.Controls.Add(_repoTypeSqlServer);
        typeRow.Controls.Add(_repoTypeMySql);
        typeRow.Controls.Add(_repoTypeOracle);
        typeRow.Controls.Add(_repoTypePostgres);

        var fileRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
        fileRow.Controls.Add(_repoFilePathBox);
        fileRow.Controls.Add(_browseRepoFileButton);
        var fileTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        AddRow(fileTable, "Path to .qea file:", fileRow);
        _repoFilePanel.Controls.Add(fileTable);

        var dbTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        AddRow(dbTable, "Server / host:", _dbServerBox);
        AddRow(dbTable, "Port (optional):", _dbPortBox);
        AddRow(dbTable, "Database:", _dbDatabaseBox);
        AddRow(dbTable, "Username:", _dbUserBox);
        AddRow(dbTable, "Password:", _dbPasswordBox);
        _dbFieldsPanel.Controls.Add(dbTable);

        var section = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        section.Controls.Add(typeRow);
        section.Controls.Add(_repoFilePanel);
        section.Controls.Add(_dbFieldsPanel);
        return section;
    }

    private void UpdateRepoTypeEnablement()
    {
        var isFile = _repoTypeFile.Checked;
        _repoFilePanel.Visible = isFile;
        _dbFieldsPanel.Visible = !isFile;

        // Oracle's TNS "Data Source" embeds host/port/service, so there's no separate port or
        // database field to fill in — matches BuildConnectionStringInteractively's Oracle branch.
        var isOracle = _repoTypeOracle.Checked;
        _dbPortBox.Enabled = !isOracle;
        _dbDatabaseBox.Enabled = !isOracle;
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

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        panel.Controls.Add(modeRow);
        panel.Controls.Add(simpleTable);
        panel.Controls.Add(dayNightTable);
        panel.Controls.Add(new Label { Text = "Export mode:", AutoSize = true, Margin = new Padding(3, 10, 3, 3) });
        panel.Controls.Add(forceRow);

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
            ApplyRepoPathToFields("");
            _wikiPortConfigBox.Value = 8000;
            _apiPortConfigBox.Value = 8001;
            _webhookBox.Text = "";
            _teamsWebhookBox.Text = "";
            return;
        }

        try
        {
            var config = LocalConfigStore.Load(path, out _);
            ApplyRepoPathToFields(config.RepoPath ?? "");
            _wikiPortConfigBox.Value = Math.Clamp(config.WikiPort ?? 8000, (int)_wikiPortConfigBox.Minimum, (int)_wikiPortConfigBox.Maximum);
            _apiPortConfigBox.Value = Math.Clamp(config.ApiPort ?? 8001, (int)_apiPortConfigBox.Minimum, (int)_apiPortConfigBox.Maximum);
            _webhookBox.Text = config.WebhookUrl ?? "";
            _teamsWebhookBox.Text = config.TeamsWebhookUrl ?? "";
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to read .eaxwiki: {ex.Message}");
        }
    }

    private void SaveEaxwikiConfig()
    {
        if (_repoRoot == null) return;
        var path = Path.Combine(_repoRoot, ".eaxwiki");

        var repoPath = BuildRepoPath();
        if (repoPath.Length == 0)
        {
            AppendOutput("Cannot save: repository details are incomplete.");
            return;
        }

        var config = new LocalConfigStore.Config
        {
            RepoPath = repoPath,
            WikiPort = (int)_wikiPortConfigBox.Value,
            ApiPort = (int)_apiPortConfigBox.Value,
            WebhookUrl = _webhookBox.Text.Trim() is { Length: > 0 } slack ? slack : null,
            TeamsWebhookUrl = _teamsWebhookBox.Text.Trim() is { Length: > 0 } teams ? teams : null,
        };

        try
        {
            LocalConfigStore.Save(path, config);
            AppendOutput($"Saved configuration to {path}.");
            LoadEaxwikiConfig();
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to save .eaxwiki: {ex.Message}");
        }
    }

    // Mirrors BuildConnectionStringInteractively in EAxWiki/Program.cs exactly, so the GUI and the
    // console wizard produce identical connection strings for the same inputs.
    private string BuildRepoPath()
    {
        if (_repoTypeFile.Checked)
            return _repoFilePathBox.Text.Trim();

        var server = _dbServerBox.Text.Trim();
        if (server.Length == 0) return "";

        var port = _dbPortBox.Text.Trim();
        var database = _dbDatabaseBox.Text.Trim();
        var user = _dbUserBox.Text.Trim();
        var password = _dbPasswordBox.Text;

        // SQL Server appends port with a comma: "SERVER,1433". MySQL/PostgreSQL use a separate Port= key.
        var sqlServerHost = port.Length == 0 ? server : $"{server},{port}";
        var portSegment = port.Length == 0 ? "" : $"Port={port};";

        if (_repoTypeSqlServer.Checked)
            return $"DBType=1;Connect=Provider=SQLOLEDB.1;Data Source={sqlServerHost};Initial Catalog={database};User Id={user};Password={password};";
        if (_repoTypeMySql.Checked)
            return $"DBType=3;Connect=Server={server};{portSegment}Database={database};Uid={user};Pwd={password};";
        if (_repoTypeOracle.Checked)
            return $"DBType=2;Connect=Data Source={server};User Id={user};Password={password};";
        if (_repoTypePostgres.Checked)
            return $"DBType=7;Connect=Server={server};{portSegment}Database={database};User Id={user};Password={password};";

        return "";
    }

    // Reverse of BuildRepoPath: populates the repo-type radio and its fields from a saved
    // connection string (or plain .qea path) so re-opening the form shows what's actually saved.
    private void ApplyRepoPathToFields(string repoPath)
    {
        const string dbTypePrefix = "DBType=";
        const string connectMarker = ";Connect=";
        var connectIdx = repoPath.StartsWith(dbTypePrefix, StringComparison.Ordinal)
            ? repoPath.IndexOf(connectMarker, StringComparison.Ordinal)
            : -1;

        if (connectIdx < 0)
        {
            _repoTypeFile.Checked = true;
            _repoFilePathBox.Text = repoPath;
            _dbServerBox.Text = "";
            _dbPortBox.Text = "";
            _dbDatabaseBox.Text = "";
            _dbUserBox.Text = "";
            _dbPasswordBox.Text = "";
            return;
        }

        var dbType = repoPath[dbTypePrefix.Length..connectIdx];
        var parts = ParseConnectString(repoPath[(connectIdx + connectMarker.Length)..]);
        _repoFilePathBox.Text = "";

        switch (dbType)
        {
            case "1":
                _repoTypeSqlServer.Checked = true;
                var dataSource = parts.GetValueOrDefault("Data Source", "");
                var commaIdx = dataSource.IndexOf(',');
                _dbServerBox.Text = commaIdx < 0 ? dataSource : dataSource[..commaIdx];
                _dbPortBox.Text = commaIdx < 0 ? "" : dataSource[(commaIdx + 1)..];
                _dbDatabaseBox.Text = parts.GetValueOrDefault("Initial Catalog", "");
                _dbUserBox.Text = parts.GetValueOrDefault("User Id", "");
                _dbPasswordBox.Text = parts.GetValueOrDefault("Password", "");
                break;
            case "3":
                _repoTypeMySql.Checked = true;
                _dbServerBox.Text = parts.GetValueOrDefault("Server", "");
                _dbPortBox.Text = parts.GetValueOrDefault("Port", "");
                _dbDatabaseBox.Text = parts.GetValueOrDefault("Database", "");
                _dbUserBox.Text = parts.GetValueOrDefault("Uid", "");
                _dbPasswordBox.Text = parts.GetValueOrDefault("Pwd", "");
                break;
            case "2":
                _repoTypeOracle.Checked = true;
                _dbServerBox.Text = parts.GetValueOrDefault("Data Source", "");
                _dbPortBox.Text = "";
                _dbDatabaseBox.Text = "";
                _dbUserBox.Text = parts.GetValueOrDefault("User Id", "");
                _dbPasswordBox.Text = parts.GetValueOrDefault("Password", "");
                break;
            case "7":
                _repoTypePostgres.Checked = true;
                _dbServerBox.Text = parts.GetValueOrDefault("Server", "");
                _dbPortBox.Text = parts.GetValueOrDefault("Port", "");
                _dbDatabaseBox.Text = parts.GetValueOrDefault("Database", "");
                _dbUserBox.Text = parts.GetValueOrDefault("User Id", "");
                _dbPasswordBox.Text = parts.GetValueOrDefault("Password", "");
                break;
            default:
                // Unrecognized DBType — show the raw string as a file/path value so nothing is lost.
                _repoTypeFile.Checked = true;
                _repoFilePathBox.Text = repoPath;
                break;
        }
    }

    private static Dictionary<string, string> ParseConnectString(string connect)
    {
        var result = new Dictionary<string, string>();
        foreach (var segment in connect.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            var eq = segment.IndexOf('=');
            if (eq < 0) continue;
            result[segment[..eq].Trim()] = segment[(eq + 1)..].Trim();
        }
        return result;
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
            // have to parse PowerShell's default table formatting in C#. triggerDetails/actionArguments
            // (structured, unlike the human-readable "triggers" strings) let ApplyScheduleFromTask
            // below reconstruct the Schedule Settings tab from whatever is actually registered.
            var command = $$"""
                $task = Get-ScheduledTask -TaskName '{{taskName}}' -ErrorAction SilentlyContinue
                if (-not $task) { @{ found = $false } | ConvertTo-Json; exit 0 }
                $info = Get-ScheduledTaskInfo -TaskName '{{taskName}}'
                $triggers = @($task.Triggers | ForEach-Object {
                    $t = $_
                    $days = if ($t.DaysOfWeek) { " days=$($t.DaysOfWeek)" } else { "" }
                    "$($t.CimClass.CimClassName) at=$($t.StartBoundary)$days interval=$($t.Repetition.Interval) duration=$($t.Repetition.Duration)"
                })
                $triggerDetails = @($task.Triggers | ForEach-Object {
                    @{ type = $_.CimClass.CimClassName; startBoundary = $_.StartBoundary; intervalIso = $_.Repetition.Interval; durationIso = $_.Repetition.Duration }
                })
                $actionArguments = if ($task.Actions.Count -gt 0) { $task.Actions[0].Arguments } else { "" }
                @{ found = $true; state = [string]$task.State; nextRun = [string]$info.NextRunTime; triggers = $triggers; triggerDetails = $triggerDetails; actionArguments = $actionArguments } | ConvertTo-Json -Depth 5
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
            ApplyScheduleFromTask(root);
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

    // Reverse of RegisterAsync's argument construction: reconstructs the Schedule Settings tab
    // from whatever schedule is actually registered, so a hardcoded default (Simple / 240 min /
    // no force) is never silently shown in place of a real Day/Night schedule — and never gets
    // silently re-applied over it if the user clicks Register/Apply without noticing the mismatch.
    private void ApplyScheduleFromTask(JsonElement root)
    {
        if (!root.TryGetProperty("triggerDetails", out var triggerDetailsElement)) return;
        var triggers = triggerDetailsElement.EnumerateArray().ToList();
        if (triggers.Count == 0) return; // nothing registered to reflect — leave fields as-is

        var daily = triggers.FirstOrDefault(t => t.GetProperty("type").GetString() == "MSFT_TaskDailyTrigger");
        var weekly = triggers.FirstOrDefault(t => t.GetProperty("type").GetString() == "MSFT_TaskWeeklyTrigger");
        var once = triggers.FirstOrDefault(t => t.GetProperty("type").GetString() == "MSFT_TaskTimeTrigger");

        if (daily.ValueKind == JsonValueKind.Object && weekly.ValueKind == JsonValueKind.Object)
        {
            _dayNightModeRadio.Checked = true;

            if (TryParseIsoMinutes(daily, "intervalIso", out var offHoursMinutes))
                _offHoursIntervalMinutes.Value = Math.Clamp(offHoursMinutes, _offHoursIntervalMinutes.Minimum, _offHoursIntervalMinutes.Maximum);

            if (TryParseIsoMinutes(weekly, "intervalIso", out var workMinutes))
                _workIntervalMinutes.Value = Math.Clamp(workMinutes, _workIntervalMinutes.Minimum, _workIntervalMinutes.Maximum);

            if (weekly.TryGetProperty("startBoundary", out var startEl) &&
                DateTimeOffset.TryParse(startEl.GetString(), out var startDto))
            {
                var workStartTime = startDto.TimeOfDay;
                _workStart.Value = DateTime.Today.Add(workStartTime);

                if (weekly.TryGetProperty("durationIso", out var durationEl) &&
                    !string.IsNullOrEmpty(durationEl.GetString()) &&
                    TryParseIsoDuration(durationEl.GetString()!, out var workDuration))
                {
                    _workEnd.Value = DateTime.Today.Add(workStartTime + workDuration);
                }
            }
        }
        else if (once.ValueKind == JsonValueKind.Object)
        {
            _simpleModeRadio.Checked = true;
            if (TryParseIsoMinutes(once, "intervalIso", out var intervalMinutes))
                _simpleIntervalMinutes.Value = Math.Clamp(intervalMinutes, _simpleIntervalMinutes.Minimum, _simpleIntervalMinutes.Maximum);
        }

        UpdateModeEnablement();

        // Force mode isn't part of the trigger — register-scheduled-task.ps1 bakes --force /
        // --force-every N into the scheduled action's own command line instead (see its $scriptArgs).
        var actionArguments = root.TryGetProperty("actionArguments", out var actionArgsEl) ? actionArgsEl.GetString() ?? "" : "";
        var forceEveryMatch = System.Text.RegularExpressions.Regex.Match(actionArguments, @"--force-every\s+(\d+)");
        if (System.Text.RegularExpressions.Regex.IsMatch(actionArguments, @"(?<!-)--force(?!-every)\b"))
        {
            _forceEveryRunRadio.Checked = true;
        }
        else if (forceEveryMatch.Success)
        {
            _forceEveryNRadio.Checked = true;
            _forceEveryN.Value = Math.Clamp(int.Parse(forceEveryMatch.Groups[1].Value), _forceEveryN.Minimum, _forceEveryN.Maximum);
        }
        else
        {
            _noForceRadio.Checked = true;
        }
        _forceEveryN.Enabled = _forceEveryNRadio.Checked;
    }

    private static bool TryParseIsoMinutes(JsonElement trigger, string property, out decimal minutes)
    {
        minutes = 0;
        if (!trigger.TryGetProperty(property, out var el) || string.IsNullOrEmpty(el.GetString())) return false;
        if (!TryParseIsoDuration(el.GetString()!, out var span)) return false;
        minutes = (decimal)span.TotalMinutes;
        return true;
    }

    private static bool TryParseIsoDuration(string iso, out TimeSpan span)
    {
        try
        {
            span = System.Xml.XmlConvert.ToTimeSpan(iso);
            return true;
        }
        catch (FormatException)
        {
            span = TimeSpan.Zero;
            return false;
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

        // Wiki port lives only on the Configuration tab now — the Schedule Settings tab used to
        // have its own separate NumericUpDown that just duplicated it (kept in sync one-way from
        // .eaxwiki on load), which meant editing the Configuration tab's port and registering from
        // here without a reload could silently apply a stale value.
        var args = new List<string> { "--task-name", _taskNameBox.Text.Trim(), "--port", ((int)_wikiPortConfigBox.Value).ToString() };

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
