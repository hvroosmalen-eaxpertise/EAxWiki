using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class ErrorLogPageRendererTests : IDisposable
{
    private readonly string _dir;

    public ErrorLogPageRendererTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_errors_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private const string Template = """
        # Error Log
        Last checked: @@GENERATED_AT@@
        ## Issues (last 7 days)
        @@ERRORS@@
        ## Recent activity (last 20 lines, all levels)
        @@RECENT@@
        """;

    private ErrorLogPageRenderer Create(string[] secrets, string logDate, string[] lines)
    {
        var logsDir = Path.Combine(_dir, "logs");
        Directory.CreateDirectory(logsDir);
        File.WriteAllLines(Path.Combine(logsDir, $"monitor-{logDate}.log"), lines);
        File.WriteAllText(Path.Combine(_dir, "errors-template.md"), Template);
        return new ErrorLogPageRenderer(Path.Combine(_dir, "errors-template.md"), Path.Combine(_dir, "wiki"), logsDir, secrets);
    }

    [Fact]
    public void Render_KeepsOnlyWarnErrorWithin7Days_NewestFirst()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var sixDaysAgo = now.AddDays(-6).ToString("yyyy-MM-dd");
        var eightDaysAgo = now.AddDays(-8).ToString("yyyy-MM-dd");
        var renderer = Create([], eightDaysAgo, [$"{eightDaysAgo} 09:00:00 [ERR] [MonitorLoop] old failure"]);
        File.WriteAllLines(Path.Combine(_dir, "logs", $"monitor-{sixDaysAgo}.log"),
            [$"{sixDaysAgo} 10:00:00 [ERR] [MonitorLoop] six days ago failure"]);
        File.WriteAllLines(Path.Combine(_dir, "logs", $"monitor-{today}.log"),
            [$"{today} 11:00:00 [INF] [MonitorLoop] ok", $"{today} 11:01:00 [WRN] [Supervisor] retrying serve"]);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("[WRN] [Supervisor] retrying serve", output);
        Assert.Contains("[ERR] [MonitorLoop] six days ago failure", output);
        var issuesSection = output.Substring(output.IndexOf("## Issues", StringComparison.Ordinal),
            output.IndexOf("## Recent activity", StringComparison.Ordinal) - output.IndexOf("## Issues", StringComparison.Ordinal));
        Assert.DoesNotContain("[INF] [MonitorLoop] ok", issuesSection);
        Assert.DoesNotContain("old failure", issuesSection);
        Assert.True(output.IndexOf("[WRN]", StringComparison.Ordinal) < output.IndexOf("[ERR] [MonitorLoop] six", StringComparison.Ordinal),
            "newest entries come first");
    }

    [Fact]
    public void Render_NoIssues_ShowsHappyState()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var renderer = Create([], today, [$"{today} 11:00:00 [INF] [MonitorLoop] ok"]);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("No issues found in the last 7 days.", output);
    }

    [Fact]
    public void Render_CapsAt100Entries()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var lines = Enumerable.Range(0, 120)
            .Select(i => $"{today} 10:00:{i:00} [ERR] [MonitorLoop] failure {i}").ToArray();
        var renderer = Create([], today, lines);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        var issuesSection = output.Substring(output.IndexOf("## Issues", StringComparison.Ordinal),
            output.IndexOf("## Recent activity", StringComparison.Ordinal) - output.IndexOf("## Issues", StringComparison.Ordinal));
        Assert.Equal(100, System.Text.RegularExpressions.Regex.Matches(issuesSection, "failure \\d+").Count);
        Assert.Contains("failure 20", issuesSection);   // oldest kept (newest 100 of 120)
        Assert.Contains("failure 119", issuesSection);  // newest
        Assert.DoesNotContain("failure 19", issuesSection);
    }

    [Fact]
    public void Render_RecentBlock_ShowsLast20AllLevels()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var lines = Enumerable.Range(0, 25)
            .Select(i => $"{today} 10:00:{i:00} [INF] [MonitorLoop] line {i}").ToArray();
        var renderer = Create([], today, lines);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("line 5", output);   // oldest of the last 20 (indices 5..24)
        Assert.Contains("line 24", output);  // newest
        Assert.DoesNotContain("line 4", output);
    }

    [Fact]
    public void Render_RedactsSecretsAndConnectionStringPassword()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        const string webhook = "https://hooks.slack.com/services/T00000000/B00000000/secretToken";
        var renderer = Create([webhook], today,
            [$"{today} 10:00:00 [ERR] [Alert] alert failed posting to {webhook}",
             $"{today} 10:01:00 [ERR] [ExportRunner] Data Source=server;Initial Catalog=ea;Password=hunter2"]);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.DoesNotContain(webhook, output);
        Assert.Contains("posting to ***", output);
        Assert.DoesNotContain("hunter2", output);
        Assert.Contains("Password=***", output);
    }

    [Fact]
    public void Render_MissingLogsDir_ShowsEmptyStates()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        File.WriteAllText(Path.Combine(_dir, "errors-template.md"), Template);
        var renderer = new ErrorLogPageRenderer(Path.Combine(_dir, "errors-template.md"),
            Path.Combine(_dir, "wiki"), Path.Combine(_dir, "no-logs"), []);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("No issues found in the last 7 days.", output);
        Assert.Contains("(no log lines yet)", output);
        Assert.Contains("2026-08-18 12:00:00", output);
    }
}
