using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using EAxWiki.Core.Configuration;
using EAxWiki.EA;

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
    private readonly bool _isAdmin;
    private bool _connectionValid;

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
    private readonly TextBox _telegramBotTokenBox = new() { Width = 400, UseSystemPasswordChar = true };
    private readonly TextBox _telegramChatIdBox = new() { Width = 400 };
    private readonly TextBox _aiEndpointBox = new() { Width = 400, Text = "https://api.openai.com/v1" };
    private readonly TextBox _aiModelBox = new() { Width = 400, Text = "gpt-4o-mini" };
    private readonly TextBox _aiKeyBox = new() { Width = 400, UseSystemPasswordChar = true };
    private readonly Button _aiTestButton = new() { Text = "Test LLM Connection", AutoSize = true };
    private readonly Button _aiSaveButton = new() { Text = "Save AI Config", AutoSize = true };
    private readonly Label _aiTestResult = new() { AutoSize = true };
    private readonly RadioButton _llmModeNone = new() { Text = "No LLM", AutoSize = true };
    private readonly RadioButton _llmModeLocal = new() { Text = "Local LLM", AutoSize = true, Checked = true };
    private readonly RadioButton _llmModeRemote = new() { Text = "Remote LLM", AutoSize = true };
    private readonly TextBox _llmExeBox = new() { Width = 340, Height = 23 };
    private readonly Button _browseLlmExeButton = new() { Text = "Browse...", AutoSize = true };
    private readonly TextBox _llmModelPathBox = new() { Width = 340, Height = 23 };
    private readonly Button _browseLlmModelButton = new() { Text = "Browse...", AutoSize = true };
    private readonly NumericUpDown _llmPortBox = new() { Minimum = 1, Maximum = 65535, Value = 8080, Width = 80 };
    private readonly Button _llmStartButton = new() { Text = "Start LLM", AutoSize = true };
    private readonly Button _llmStopButton = new() { Text = "Stop LLM", AutoSize = true, Enabled = false };
    private Process? _llmProcess;
    private readonly Button _testConnectionButton = new() { Text = "Test Connection", AutoSize = true };
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

    // Wake behavior (issue #44) — on by default, matching register-scheduled-task.ps1's own default.
    private readonly CheckBox _wakeToRunCheckbox = new() { Text = "Wake the computer to run this task", Checked = true, AutoSize = true };

    private readonly TextBox _outputBox = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Font = new Font(FontFamily.GenericMonospace, 9) };
    private readonly Button _registerButton = new() { Text = "Register / Apply Schedule", AutoSize = true };
    private readonly Button _runMonitorButton = new() { Text = "Run Monitor Now", AutoSize = true };
    private readonly Button _enableButton = new() { Text = "Enable", AutoSize = true };
    private readonly Button _disableButton = new() { Text = "Disable", AutoSize = true };
    private readonly Button _stopExportButton = new() { Text = "Stop Export", AutoSize = true };
    private readonly Button _stopServeButton = new() { Text = "Stop Serve", AutoSize = true };
    private readonly Button _stopLlmButton = new() { Text = "Stop LLM", AutoSize = true };
    private readonly Button _stopAllButton = new() { Text = "Stop All", AutoSize = true };
    private readonly Button _refreshStatusButton = new() { Text = "Refresh Status", AutoSize = true };
    private readonly Button _refreshConfigButton = new() { Text = "Refresh", AutoSize = true };
    private readonly Button _refreshDashboardButton = new() { Text = "Refresh", AutoSize = true };
    private readonly DataGridView _dashboardGrid = new()
    {
        ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        AllowUserToAddRows = false, AllowUserToDeleteRows = false, Dock = DockStyle.Fill,
        AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells,
    };
    private bool _dashboardRefreshed;
    private TabPage? _dashboardTab;
    private SplitContainer? _dashboardSplit;

    public SchedulerForm()
    {
        _repoRoot = RepoLocator.FindRepoRoot();
        _dashboardGrid.ColumnAdded += OnDashboardColumnAdded;

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
        tabs.TabPages.Add(BuildAiTab());
        tabs.TabPages.Add(BuildTaskStatusTab());
        tabs.TabPages.Add(BuildDashboardTab());
        tabs.SelectedIndexChanged += (_, _) =>
        {
            if (tabs.SelectedTab != _dashboardTab || _dashboardRefreshed) return;
            _dashboardRefreshed = true;
            if (_dashboardSplit != null)
                _dashboardSplit.SplitterDistance = Math.Max(40, _dashboardSplit.Height - 40);
            RefreshDashboard();
        };

        root.Controls.Add(tabs, 0, 0);
        root.Controls.Add(BuildOutputGroup(), 0, 1);

        Controls.Add(root);

        _refreshConfigButton.Click += (_, _) => LoadEaxwikiConfig();
        _saveConfigButton.Click += (_, _) => SaveEaxwikiConfig();
        _testConnectionButton.Click += async (_, _) => await TestConnectionAsync();
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
        _runMonitorButton.Click += async (_, _) => await RunMonitorAsync();
        _enableButton.Click += async (_, _) => await EnableAndResumeAsync();
        _disableButton.Click += async (_, _) => await RunTaskCommandAsync("Disable-ScheduledTask");
        _stopExportButton.Click += async (_, _) => await StopExportAsync();
        _stopServeButton.Click += async (_, _) => await StopServeAsync();
        _stopLlmButton.Click += async (_, _) => await StopLlmAsync();
        _stopAllButton.Click += async (_, _) => await StopAllAsync();

        _simpleModeRadio.CheckedChanged += (_, _) => UpdateModeEnablement();
        _forceEveryNRadio.CheckedChanged += (_, _) => _forceEveryN.Enabled = _forceEveryNRadio.Checked;
        _llmModeNone.CheckedChanged += (_, _) => UpdateAiModeEnablement();
        _llmModeLocal.CheckedChanged += (_, _) => UpdateAiModeEnablement();
        _llmModeRemote.CheckedChanged += (_, _) => UpdateAiModeEnablement();
        _aiTestButton.Click += async (_, _) => await TestAiConnectionAsync();
        _aiSaveButton.Click += (_, _) => SaveAiConfig();
        _browseLlmExeButton.Click += (_, _) =>
        {
            using var dialog = new OpenFileDialog { Filter = "llama-server.exe|llama-server.exe|All files (*.*)|*.*", CheckFileExists = true };
            if (dialog.ShowDialog() == DialogResult.OK)
                _llmExeBox.Text = dialog.FileName;
        };
        _browseLlmModelButton.Click += (_, _) =>
        {
            using var dialog = new OpenFileDialog { Filter = "GGUF models (*.gguf)|*.gguf|All files (*.*)|*.*", CheckFileExists = true };
            if (dialog.ShowDialog() == DialogResult.OK)
                _llmModelPathBox.Text = dialog.FileName;
        };
        _llmStartButton.Click += async (_, _) => await StartLlmAsync();
        _llmStopButton.Click += (_, _) => StopLlm();

        if (_repoRoot == null)
        {
            AppendOutput("Could not locate the EAxWiki repo root (looked for scripts/register-scheduled-task.ps1 above this exe's folder). Registration and status queries are disabled.");
            _registerButton.Enabled = false;
            _refreshStatusButton.Enabled = false;
            _enableButton.Enabled = false;
            _disableButton.Enabled = false;
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

        if (_aiEndpointBox.Text.Length == 0) _aiEndpointBox.Text = "https://api.openai.com/v1";
        if (_aiModelBox.Text.Length == 0) _aiModelBox.Text = "gpt-4o-mini";
        UpdateAiModeEnablement();

        using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
        {
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            _isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        if (!_isAdmin)
        {
            AppendOutput("This tool requires Administrator privileges to manage scheduled tasks. Please restart as Administrator.");
            _registerButton.Enabled = false;
            _enableButton.Enabled = false;
            _disableButton.Enabled = false;
            _refreshStatusButton.Enabled = false;
        }
    }

    private TabPage BuildConfigTab()
    {
        var table = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Dock = DockStyle.Top };
        AddRow(table, "Wiki port:", _wikiPortConfigBox);
        AddRow(table, "API port:", _apiPortConfigBox);
        AddRow(table, "Slack Webhook:", _webhookBox);
        AddRow(table, "Teams Webhook:", _teamsWebhookBox);
        AddRow(table, "Telegram Bot Token:", _telegramBotTokenBox);
        AddRow(table, "Telegram Chat ID:", _telegramChatIdBox);

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
        fileRow.Controls.Add(new Label { Text = "Path to .qea file:", AutoSize = true, Margin = new Padding(3, 6, 10, 3) });
        fileRow.Controls.Add(_repoFilePathBox);
        fileRow.Controls.Add(_browseRepoFileButton);
        var testConnStack = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        testConnStack.Controls.Add(_testConnectionButton);
        _repoFilePanel.Controls.Add(fileRow);

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

        var buttonRow1 = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow1.Controls.Add(_refreshStatusButton);
        buttonRow1.Controls.Add(_enableButton);
        buttonRow1.Controls.Add(_disableButton);

        var buttonRow2 = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow2.Controls.Add(_stopExportButton);
        buttonRow2.Controls.Add(_stopServeButton);
        buttonRow2.Controls.Add(_stopLlmButton);
        buttonRow2.Controls.Add(_stopAllButton);

        var buttonStack = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown };
        buttonStack.Controls.Add(buttonRow1);
        buttonStack.Controls.Add(buttonRow2);

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown };
        panel.Controls.Add(table);
        panel.Controls.Add(buttonStack);

        return new TabPage("Task Status") { Padding = new Padding(10), AutoScroll = true, Controls = { panel } };
    }

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

    private void RefreshDashboard()
    {
        if (_repoRoot == null) return;
        var snapshot = new HealthDashboardReader().ReadAll(_repoRoot);
        _dashboardGrid.DataSource = snapshot.Services
            .Select(s => new
            {
                s.Name,
                Status = s.NotConfigured ? "not configured" : s.Running ? "running" : "not running",
                s.LastSuccess,
                s.LastFailure,
                s.ConsecutiveFailures,
            })
            .ToList();
    }

    private void OnDashboardColumnAdded(object? sender, DataGridViewColumnEventArgs e)
    {
        e.Column.FillWeight = e.Column.Name switch
        {
            "LastSuccess" or "LastFailure" => 35,
            "Name" or "Status" or "ConsecutiveFailures" => 10,
            _ => 10,
        };
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
        _wakeToRunCheckbox.Margin = new Padding(3, 10, 3, 3);
        panel.Controls.Add(_wakeToRunCheckbox);

        // Kept out of the FlowLayoutPanel above deliberately: with TopDown flow, once content
        // exceeds the available height the panel wraps into a new column instead of scrolling,
        // which pushed this button to the top-right. Docked to the bottom and right-aligned here
        // instead, so it stays put regardless of how much the panel above needs to scroll.
        var buttonRow = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 8, 0, 0) };
        buttonRow.Controls.Add(_registerButton);
        buttonRow.Controls.Add(_runMonitorButton);

        var tabPage = new TabPage("Schedule Settings") { Padding = new Padding(10), AutoScroll = true };
        tabPage.Controls.Add(panel);
        tabPage.Controls.Add(buttonRow);
        return tabPage;
    }

    private TabPage BuildAiTab()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };

        // Mode selector
        var modeRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0, 0, 0, 8) };
        modeRow.Controls.Add(_llmModeNone);
        modeRow.Controls.Add(_llmModeLocal);
        modeRow.Controls.Add(_llmModeRemote);
        panel.Controls.Add(modeRow);

        // Local LLM section
        var localGroup = new GroupBox { Text = "Local LLM", Width = 560, Height = 180, Padding = new Padding(6) };
        var localTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        localTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        localTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        AddRow(localTable, "Server executable:", MakeBrowseRow(_llmExeBox, _browseLlmExeButton));
        AddRow(localTable, "Model file (.gguf):", MakeBrowseRow(_llmModelPathBox, _browseLlmModelButton));
        AddRow(localTable, "Port:", _llmPortBox);
        var localButtons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(3, 4, 3, 3) };
        localButtons.Controls.Add(_llmStartButton);
        localButtons.Controls.Add(_llmStopButton);
        localGroup.Controls.Add(localTable);
        localGroup.Controls.Add(localButtons);
        localTable.Location = new Point(6, 16);
        localButtons.Location = new Point(6, localTable.Bottom + 2);
        panel.Controls.Add(localGroup);

        // Remote LLM section
        var remoteGroup = new GroupBox { Text = "Remote LLM", Width = 560, Height = 180, Padding = new Padding(6) };
        var remoteTable = new TableLayoutPanel { ColumnCount = 2, AutoSize = true };
        remoteTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        remoteTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        AddRow(remoteTable, "Remote LLM Endpoint:", _aiEndpointBox);
        AddRow(remoteTable, "Model:", _aiModelBox);
        AddRow(remoteTable, "API Key:", _aiKeyBox);
        var remoteButtons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(3, 4, 3, 3), WrapContents = false };
        remoteButtons.Controls.Add(_aiTestButton);
        remoteButtons.Controls.Add(_aiSaveButton);
        remoteGroup.Controls.Add(remoteTable);
        remoteGroup.Controls.Add(_aiTestResult);
        remoteGroup.Controls.Add(remoteButtons);
        remoteTable.Location = new Point(6, 16);
        _aiTestResult.Location = new Point(6, remoteTable.Bottom + 4);
        remoteButtons.Location = new Point(6, _aiTestResult.Bottom + 2);
        panel.Controls.Add(remoteGroup);

        return new TabPage("AI LLM") { Padding = new Padding(10), AutoScroll = true, Controls = { panel } };
    }

    private static FlowLayoutPanel MakeBrowseRow(TextBox box, Button button)
    {
        var row = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
        row.Controls.Add(box);
        row.Controls.Add(button);
        return row;
    }

    private async Task TestAiConnectionAsync()
    {
        var endpoint = _aiEndpointBox.Text.Trim();
        if (endpoint.Length == 0)
        {
            _aiTestResult.Text = "Enter an AI endpoint first.";
            _aiTestResult.ForeColor = Color.Red;
            return;
        }

        _aiTestButton.Enabled = false;
        _aiTestButton.Text = "Testing...";
        _aiTestResult.Text = "";
        AppendOutput("Testing LLM connection...");

        try
        {
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            var model = _aiModelBox.Text.Trim();
            if (model.Length == 0) model = "gpt-4o-mini";

            var body = new
            {
                model,
                messages = new[] { new { role = "user", content = "Say OK" } },
                max_tokens = 5
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{endpoint.TrimEnd('/')}/chat/completions")
            {
                Content = JsonContent.Create(body)
            };

            var key = _aiKeyBox.Text;
            if (!string.IsNullOrEmpty(key))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                var content = "";
                if (result.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                    content = choices[0].GetProperty("message").GetProperty("content").GetString() ?? "";

                _aiTestResult.Text = $"LLM reachable — response: {content.Trim()}";
                _aiTestResult.ForeColor = Color.Green;
                AppendOutput($"LLM test successful: {content.Trim()}");
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _aiTestResult.Text = $"Error: HTTP {(int)response.StatusCode}";
                _aiTestResult.ForeColor = Color.Red;
                AppendOutput($"LLM test failed (HTTP {(int)response.StatusCode}): {errorBody}");
            }
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

    private async Task StartLlmAsync()
    {
        var exePath = _llmExeBox.Text.Trim();
        var modelPath = _llmModelPathBox.Text.Trim();
        if (exePath.Length == 0 || modelPath.Length == 0)
        {
            AppendOutput("Set both LLM Server path and LLM Model path first.");
            return;
        }
        if (!File.Exists(exePath))
        {
            AppendOutput($"LLM server not found: {exePath}");
            return;
        }
        if (!File.Exists(modelPath))
        {
            AppendOutput($"LLM model not found: {modelPath}");
            return;
        }

        _llmStartButton.Enabled = false;
        _llmStartButton.Text = "Starting...";
        AppendOutput($"Starting LLM server: {exePath}");

        try
        {
            var port = (int)_llmPortBox.Value;
            var psi = new ProcessStartInfo(exePath, $"-m \"{modelPath}\" -c 4096 --port {port} --n-gpu-layers 0")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
            process.Start();

            _llmProcess = process;
            _llmStopButton.Enabled = true;
            _llmStartButton.Text = "Running";
            // Do NOT write the local llama URL into _aiEndpointBox. That box belongs to the
            // Remote LLM section; leaking the local URL there made the two sections indistinguishable
            // and persisted a phantom "remote" endpoint into .eaxwiki. Local LLM is fully addressed
            // by _llmExeBox / _llmModelPathBox / _llmPortBox.
            AppendOutput($"LLM server started (PID {process.Id}) on port {port}.");

            // Read output in background to detect failures
            _ = Task.Run(async () =>
            {
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                var fullLog = (output + error).Trim();
                if (fullLog.Length > 0)
                    BeginInvoke(() => AppendOutput($"LLM exited: {fullLog[..Math.Min(fullLog.Length, 500)]}"));

                BeginInvoke(() =>
                {
                    _llmProcess = null;
                    _llmStartButton.Enabled = true;
                    _llmStartButton.Text = "Start LLM";
                    _llmStopButton.Enabled = false;
                    AppendOutput("LLM server stopped.");
                });
            });
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to start LLM: {ex.Message}");
            _llmStartButton.Enabled = true;
            _llmStartButton.Text = "Start LLM";
        }
    }

    private void StopLlm()
    {
        if (_llmProcess == null || _llmProcess.HasExited) return;
        try
        {
            _llmProcess.Kill(entireProcessTree: true);
            AppendOutput("LLM server stopped.");
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to stop LLM: {ex.Message}");
        }
        _llmProcess = null;
        _llmStartButton.Enabled = true;
        _llmStartButton.Text = "Start LLM";
        _llmStopButton.Enabled = false;
    }

    private void SaveAiConfig()
    {
        if (_repoRoot == null) return;
        var path = Path.Combine(_repoRoot, ".eaxwiki");

        if (_aiEndpointBox.Text.Trim() is { Length: > 0 } endpoint &&
            !Uri.TryCreate(endpoint, UriKind.Absolute, out _))
        {
            AppendOutput($"Invalid AI endpoint URL: {endpoint}");
            return;
        }

        try
        {
            var config = File.Exists(path)
                ? LocalConfigStore.Load(path, out _)
                : new LocalConfigStore.Config();

            config.AiMode = _llmModeNone.Checked ? "none" : _llmModeLocal.Checked ? "local" : "remote";
            config.AiEndpoint = _aiEndpointBox.Text.Trim() is { Length: > 0 } ai ? ai : null;
            config.AiModel = _aiModelBox.Text.Trim() is { Length: > 0 } model ? model : null;
            config.AiKey = _aiKeyBox.Text is { Length: > 0 } key ? key : null;
            config.LlamaExePath = _llmExeBox.Text.Trim() is { Length: > 0 } exe ? exe : null;
            config.LlamaModelPath = _llmModelPathBox.Text.Trim() is { Length: > 0 } mp ? mp : null;
            config.LlmPort = (int)_llmPortBox.Value;

            LocalConfigStore.Save(path, config);
            AppendOutput("AI config saved.");
        }
        catch (Exception ex)
        {
            AppendOutput($"Failed to save AI config: {ex.Message}");
        }
    }

    private GroupBox BuildOutputGroup()
    {
        return new GroupBox { Text = "Output", Dock = DockStyle.Fill, Padding = new Padding(8), Controls = { _outputBox } };
    }

    private static void AddRow(TableLayoutPanel table, string caption, Control value)
    {
        var row = table.RowCount;
        table.RowCount = row + 1;
        table.Controls.Add(new Label { Text = caption, AutoSize = true, MinimumSize = new Size(0, 23), TextAlign = ContentAlignment.MiddleRight, Anchor = AnchorStyles.Right, Margin = new Padding(3, 3, 8, 3) }, 0, row);
        value.Margin = new Padding(3, 3, 3, 3);
        table.Controls.Add(value, 1, row);
    }

    private void UpdateAiModeEnablement()
    {
        var none = _llmModeNone.Checked;
        var local = _llmModeLocal.Checked;
        var remote = _llmModeRemote.Checked;
        _llmExeBox.Enabled = local;
        _browseLlmExeButton.Enabled = local;
        _llmModelPathBox.Enabled = local;
        _browseLlmModelButton.Enabled = local;
        _llmPortBox.Enabled = local;
        _llmStartButton.Enabled = local;
        _llmStopButton.Enabled = local && _llmProcess != null;
        _aiEndpointBox.Enabled = remote;
        _aiModelBox.Enabled = remote;
        _aiKeyBox.Enabled = remote;
        _aiTestButton.Enabled = !none;
        _aiSaveButton.Enabled = !none;
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
            _telegramBotTokenBox.Text = "";
            _telegramChatIdBox.Text = "";
            _aiEndpointBox.Text = "https://api.openai.com/v1";
            _aiModelBox.Text = "gpt-4o-mini";
            _aiKeyBox.Text = "";
            // Local LLM paths are per-machine — leave blank so the Browse buttons drive discovery.
            // Hardcoded E:\ paths here silently activated the local LLM on one specific machine
            // and misled every other install into saving broken paths.
            _llmExeBox.Text = "";
            _llmModelPathBox.Text = "";
            _llmPortBox.Value = 8080;
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
            _telegramBotTokenBox.Text = config.TelegramBotToken ?? "";
            _telegramChatIdBox.Text = config.TelegramChatId ?? "";
            var aiMode = config.AiMode ?? "local";
            _llmModeNone.Checked = aiMode == "none";
            _llmModeLocal.Checked = aiMode == "local";
            _llmModeRemote.Checked = aiMode == "remote";
            // Load-time defaults must not inject values the user didn't set — they lie about
            // config state and get silently persisted on Save. Remote endpoint / model / llama
            // paths stay blank when the config doesn't have them; the fresh-form Reset button
            // (below) provides sensible starting values instead. Also strip any leaked
            // "http://localhost" AiEndpoint left over from the old StartLlmAsync bug — that
            // value belonged to Local LLM state, not the Remote endpoint.
            var loadedEndpoint = config.AiEndpoint ?? "";
            if (loadedEndpoint.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase) ||
                loadedEndpoint.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase))
                loadedEndpoint = "";
            _aiEndpointBox.Text = loadedEndpoint;
            _aiModelBox.Text = config.AiModel ?? "";
            _aiKeyBox.Text = config.AiKey ?? "";
            _llmExeBox.Text = config.LlamaExePath ?? "";
            _llmModelPathBox.Text = config.LlamaModelPath ?? "";
            _llmPortBox.Value = Math.Clamp(config.LlmPort ?? 8080, (int)_llmPortBox.Minimum, (int)_llmPortBox.Maximum);
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

        if (_webhookBox.Text.Trim() is { Length: > 0 } slackUrl &&
            !Uri.TryCreate(slackUrl, UriKind.Absolute, out _))
        {
            AppendOutput($"Invalid Slack webhook URL: {slackUrl}");
            return;
        }
        if (_teamsWebhookBox.Text.Trim() is { Length: > 0 } teamsUrl &&
            !Uri.TryCreate(teamsUrl, UriKind.Absolute, out _))
        {
            AppendOutput($"Invalid Teams webhook URL: {teamsUrl}");
            return;
        }
        if (_telegramChatIdBox.Text.Trim() is { Length: > 0 } chatId &&
            !long.TryParse(chatId, out _))
        {
            AppendOutput($"Invalid Telegram chat ID (must be numeric, e.g. 123456789 or -1001234567890): {chatId}");
            return;
        }

        var config = new LocalConfigStore.Config
        {
            RepoPath = repoPath,
            WikiPort = (int)_wikiPortConfigBox.Value,
            ApiPort = (int)_apiPortConfigBox.Value,
            WebhookUrl = _webhookBox.Text.Trim() is { Length: > 0 } slack ? slack : null,
            TeamsWebhookUrl = _teamsWebhookBox.Text.Trim() is { Length: > 0 } teams ? teams : null,
            TelegramBotToken = _telegramBotTokenBox.Text.Trim() is { Length: > 0 } token ? token : null,
            TelegramChatId = _telegramChatIdBox.Text.Trim() is { Length: > 0 } id ? id : null,
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

    private async Task TestConnectionAsync()
    {
        var repoPath = BuildRepoPath();
        if (repoPath.Length == 0)
        {
            AppendOutput("Enter repository details first.");
            return;
        }

        _testConnectionButton.Enabled = false;
        _testConnectionButton.Text = "Testing...";
        AppendOutput("Testing repository connection...");

        try
        {
            var tcs = new TaskCompletionSource<(bool ok, string? error)>();
            var thread = new Thread(() =>
            {
                try
                {
                    var reader = new EaReader();
                    var ok = reader.TestConnection(repoPath, out var error);
                    reader.Dispose();
                    tcs.SetResult((ok, error));
                }
                catch (Exception ex)
                {
                    tcs.SetResult((false, ex.Message));
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            var (ok, error) = await tcs.Task;
            _connectionValid = ok;

            if (ok)
                AppendOutput("Connection successful.");
            else
                AppendOutput($"Connection failed: {error}");
        }
        finally
        {
            _testConnectionButton.Enabled = true;
            _testConnectionButton.Text = "Test Connection";
        }
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
                @{ found = $true; state = [string]$task.State; nextRun = [string]$info.NextRunTime; triggers = $triggers; triggerDetails = $triggerDetails; actionArguments = $actionArguments; wakeToRun = [bool]$task.Settings.WakeToRun } | ConvertTo-Json -Depth 5
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

        if (root.TryGetProperty("wakeToRun", out var wakeToRunEl))
            _wakeToRunCheckbox.Checked = wakeToRunEl.GetBoolean();
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
        if (!_connectionValid)
        {
            AppendOutput("Test the repository connection on the Configuration tab first.");
            return;
        }

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

        if (!_wakeToRunCheckbox.Checked)
            args.Add("--no-wake-to-run");

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

    private async Task RunMonitorAsync()
    {
        if (_repoRoot == null) return;

        var repoPath = BuildRepoPath();
        if (repoPath.Length == 0)
        {
            AppendOutput("Configure the repository on the Configuration tab first.");
            return;
        }

        // Save config so .eaxwiki has webhooks, ports, etc.
        SaveEaxwikiConfig();
        SaveAiConfig();  // Also save AI endpoint/mode/model to .eaxwiki
        AppendOutput("Config saved to .eaxwiki.");

        var monitorExe = Path.Combine(_repoRoot, "src", "EAxWiki.Monitor", "bin", "Debug", "net10.0", "EAxWiki.Monitor.exe");
        if (!File.Exists(monitorExe))
        {
            AppendOutput($"Monitor executable not found: {monitorExe}");
            return;
        }

        var args = new List<string>
        {
            "--repo", repoPath,
            "--port", ((int)_wikiPortConfigBox.Value).ToString(),
            "--llm-port", ((int)_llmPortBox.Value).ToString(),
        };
        var webhook = _webhookBox.Text.Trim();
        if (webhook.Length > 0) { args.Add("--webhook-url"); args.Add(webhook); }
        var teamsWebhook = _teamsWebhookBox.Text.Trim();
        if (teamsWebhook.Length > 0) { args.Add("--teams-webhook-url"); args.Add(teamsWebhook); }
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        if (tgBotToken.Length > 0) { args.Add("--telegram-bot-token"); args.Add(tgBotToken); }
        var tgChatId = _telegramChatIdBox.Text.Trim();
        if (tgChatId.Length > 0) { args.Add("--telegram-chat-id"); args.Add(tgChatId); }
        if (_forceEveryRunRadio.Checked) args.Add("--force");
        else if (_forceEveryNRadio.Checked) { args.Add("--force-every"); args.Add(((int)_forceEveryN.Value).ToString()); }

        AppendOutput($"> Starting monitor in new window...");
        var psi = new ProcessStartInfo
        {
            FileName = monitorExe,
            Arguments = string.Join(" ", args.Select(a => a.Contains(' ') ? $"\"{a}\"" : a)),
            WorkingDirectory = _repoRoot,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal,
        };
        Process.Start(psi);
        AppendOutput($"Monitor launched in separate window.");
    }

    private async Task EnableAndResumeAsync()
    {
        if (_repoRoot == null) return;
        await RunTaskCommandAsync("Enable-ScheduledTask");

        var cmd = $@"
$sf = Get-ChildItem -Path '{_repoRoot}\.eaxwiki-monitor\*\health.json' -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName
if ($sf) {{ $s = Get-Content $sf -Raw | ConvertFrom-Json; $s.skipExport = $false; $s.skipServe = $false; $s | ConvertTo-Json | Set-Content $sf; Write-Host 'Cleared skipExport and skipServe flags.' }}
";
        var result = await PowerShellRunner.RunCommandAsync(cmd, _repoRoot);
        AppendOutput(result.Output);
    }

    private async Task StopExportAsync()
    {
        if (_repoRoot == null) return;
        var repoRoot = _repoRoot;
        var webhookUrl = _webhookBox.Text.Trim();
        var teamsUrl = _teamsWebhookBox.Text.Trim();
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        var tgChatId = _telegramChatIdBox.Text.Trim();

        var cmd = $@"
# Kill any process with EAxWiki in its command line (dotnet.exe, EAxWiki.exe, etc.)
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.CommandLine -match 'EAxWiki' -and $_.Name -notmatch 'powershell|pwsh' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
$sf = Get-ChildItem -Path '{repoRoot}\.eaxwiki-monitor\*\health.json' -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName
if ($sf) {{ $s = Get-Content $sf -Raw | ConvertFrom-Json; $s.skipExport = $true; $s | ConvertTo-Json | Set-Content $sf; Write-Host 'skipExport=$true' }}
& '{repoRoot}\scripts\send-alert.ps1' -WebhookUrl '{webhookUrl.Replace("'", "''")}' -TeamsWebhookUrl '{teamsUrl.Replace("'", "''")}' -TelegramBotToken '{tgBotToken.Replace("'", "''")}' -TelegramChatId '{tgChatId.Replace("'", "''")}' -Message 'Export stopped by user.' -Kind UserStop
";
        AppendOutput("> Stopping export...");
        var result = await PowerShellRunner.RunCommandAsync(cmd, _repoRoot);
        AppendOutput(result.Output);
    }

    private async Task StopServeAsync()
    {
        if (_repoRoot == null) return;
        var repoRoot = _repoRoot;
        var webhookUrl = _webhookBox.Text.Trim();
        var teamsUrl = _teamsWebhookBox.Text.Trim();
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        var tgChatId = _telegramChatIdBox.Text.Trim();

        var cmd = $@"
# Kill pwsh.exe running serve.ps1
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.Name -match 'pwsh|powershell' -and $_.CommandLine -match 'serve\.ps1|mkdocs' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
# Kill mkdocs.exe and its python workers
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.Name -match 'mkdocs|python' -and $_.CommandLine -match 'mkdocs' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
$sf = Get-ChildItem -Path '{repoRoot}\.eaxwiki-monitor\*\health.json' -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName
if ($sf) {{ $s = Get-Content $sf -Raw | ConvertFrom-Json; $s.skipServe = $true; $s | ConvertTo-Json | Set-Content $sf; Write-Host 'skipServe=$true' }}
& '{repoRoot}\scripts\send-alert.ps1' -WebhookUrl '{webhookUrl.Replace("'", "''")}' -TeamsWebhookUrl '{teamsUrl.Replace("'", "''")}' -TelegramBotToken '{tgBotToken.Replace("'", "''")}' -TelegramChatId '{tgChatId.Replace("'", "''")}' -Message 'Serve stopped by user.' -Kind UserStop
";
        AppendOutput("> Stopping serve...");
        var result = await PowerShellRunner.RunCommandAsync(cmd, _repoRoot);
        AppendOutput(result.Output);
    }

    private async Task StopLlmAsync()
    {
        if (_repoRoot == null) return;
        var repoRoot = _repoRoot;
        var webhookUrl = _webhookBox.Text.Trim();
        var teamsUrl = _teamsWebhookBox.Text.Trim();
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        var tgChatId = _telegramChatIdBox.Text.Trim();

        var cmd = $@"
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.Name -match 'llama-server' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
& '{repoRoot}\scripts\send-alert.ps1' -WebhookUrl '{webhookUrl.Replace("'", "''")}' -TeamsWebhookUrl '{teamsUrl.Replace("'", "''")}' -TelegramBotToken '{tgBotToken.Replace("'", "''")}' -TelegramChatId '{tgChatId.Replace("'", "''")}' -Message 'Local LLM stopped by user.' -Kind UserStop
";
        AppendOutput("> Stopping LLM...");
        var result = await PowerShellRunner.RunCommandAsync(cmd, _repoRoot);
        AppendOutput(result.Output);

        _llmProcess = null;
        UpdateAiModeEnablement();
    }

    private async Task StopAllAsync()
    {
        if (_repoRoot == null) return;
        var repoRoot = _repoRoot;
        var webhookUrl = _webhookBox.Text.Trim();
        var teamsUrl = _teamsWebhookBox.Text.Trim();
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        var tgChatId = _telegramChatIdBox.Text.Trim();

        var cmd = $@"
# Kill any process with EAxWiki in command line
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.CommandLine -match 'EAxWiki' -and $_.Name -notmatch 'powershell|pwsh' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
# Kill pwsh.exe running serve.ps1
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.Name -match 'pwsh|powershell' -and $_.CommandLine -match 'serve\.ps1|mkdocs' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
# Kill mkdocs.exe and its python workers
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.Name -match 'mkdocs|python' -and $_.CommandLine -match 'mkdocs' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
# Kill llama-server
Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
    Where-Object {{ $_.Name -match 'llama-server' }} |
    ForEach-Object {{ Write-Host ""Killing $($_.Name) PID $($_.ProcessId)""; Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }}
$sf = Get-ChildItem -Path '{repoRoot}\.eaxwiki-monitor\*\health.json' -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName
if ($sf) {{ $s = Get-Content $sf -Raw | ConvertFrom-Json; $s.skipExport = $true; $s.skipServe = $true; $s | ConvertTo-Json | Set-Content $sf; Write-Host 'skipExport=$true, skipServe=$true' }}
& '{repoRoot}\scripts\send-alert.ps1' -WebhookUrl '{webhookUrl.Replace("'", "''")}' -TeamsWebhookUrl '{teamsUrl.Replace("'", "''")}' -TelegramBotToken '{tgBotToken.Replace("'", "''")}' -TelegramChatId '{tgChatId.Replace("'", "''")}' -Message 'All processes stopped by user.' -Kind UserStop
";
        AppendOutput("> Stopping all processes...");
        var result = await PowerShellRunner.RunCommandAsync(cmd, _repoRoot);
        AppendOutput(result.Output);

        _llmProcess = null;
        UpdateAiModeEnablement();
    }
}
