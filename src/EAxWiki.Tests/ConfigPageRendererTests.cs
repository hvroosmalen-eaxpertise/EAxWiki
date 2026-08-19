using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class ConfigPageRendererTests : IDisposable
{
    private readonly string _dir;

    public ConfigPageRendererTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_config_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private sealed class StubSnapshot : IScheduledTaskSnapshot
    {
        public ScheduledTaskInfo? Value { get; set; }
        public ScheduledTaskInfo? Get() => Value;
    }

    private static MonitorOptions Options() => new()
    {
        RepoPath = @"C:\models\repo.qea",
        WikiDir = @"C:\wiki",
        WikiPort = 8000,
        ApiPort = 8001,
        LlmPort = 8080,
        ExportIntervalMinutes = 30,
        CheckIntervalSeconds = 30,
        MaxRetries = 3,
        RetryDelaySeconds = 30,
        MinElementFraction = 0.5,
        Force = false,
        ForceEveryNRuns = 4,
        Brand = "ACME",
        AiMode = "openai",
        AiEndpoint = "https://api.openai.com/v1",
        AiModel = "gpt-4o-mini",
        WebhookUrl = "https://hooks.slack.com/services/secret",
    };

    [Fact]
    public void Render_ShowsOperationalValuesAndSchedule()
    {
        var stub = new StubSnapshot
        {
            Value = new ScheduledTaskInfo("EAxWiki-Monitor", "Ready", false, "PT72H", "IgnoreNew",
                ["Daily at 00:00, every 4 h (for 8 h)"]),
        };
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), stub);

        renderer.Render(Options(), new DateTime(2026, 8, 18, 12, 0, 0));

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("| Wiki port | 8000 |", output);
        Assert.Contains("| API port | 8001 |", output);
        Assert.Contains("| Export interval | 30 min |", output);
        Assert.Contains("| Max retries | 3 |", output);
        Assert.Contains("| Force every N runs | every 4 runs |", output);
        Assert.Contains("| AI model | gpt-4o-mini |", output);
        Assert.Contains("| Task name | `EAxWiki-Monitor` |", output);
        Assert.Contains("Daily at 00:00, every 4 h (for 8 h)", output);
        Assert.Contains("2026-08-18 12:00:00", output);
    }

    [Fact]
    public void Render_RedactsRepoPassword()
    {
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), new StubSnapshot());
        var options = Options() with { RepoPath = "Data Source=server;Initial Catalog=ea;Password=hunter2" };

        renderer.Render(options, DateTime.Now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("Password=***", output);
        Assert.DoesNotContain("hunter2", output);
    }

    [Fact]
    public void Render_AlertDestinations_ConfiguredOrNot_NoSecrets()
    {
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), new StubSnapshot());

        renderer.Render(Options(), DateTime.Now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("| Slack | configured |", output);
        Assert.Contains("| Teams | not configured |", output);
        Assert.Contains("| Telegram | not configured |", output);
        Assert.DoesNotContain("hooks.slack.com", output);
    }

    [Fact]
    public void Render_ScheduleUnavailable_ShowsMessage()
    {
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), new StubSnapshot());

        renderer.Render(Options(), DateTime.Now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("Schedule info unavailable", output);
    }
}