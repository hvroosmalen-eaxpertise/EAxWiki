using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class HealthPageRendererTests : IDisposable
{
    private readonly string _dir;

    public HealthPageRendererTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_healthpage_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private static readonly string Template = """
        **Overall:** @@OVERALL@@
        | Last success | @@LAST_SUCCESS_TIME@@ |
        | Consecutive failures | @@CONSECUTIVE_FAILURES@@ |
        | Last exit code | @@LAST_EXIT_CODE@@ |
        | Last page count | @@LAST_ELEMENT_COUNT@@ |
        | Runs since force | @@RUNS_SINCE_FORCE@@ |
        | Serve failures | @@SERVE_CONSECUTIVE_FAILURES@@ |
        """;

    [Fact]
    public void Render_Healthy_AllZeros()
    {
        var templatePath = Path.Combine(_dir, "health-template.md");
        File.WriteAllText(templatePath, Template);
        var renderer = new HealthPageRenderer(templatePath, Path.Combine(_dir, "wiki"));
        var state = new HealthState();

        renderer.Render(state);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "health.md"));
        Assert.Contains("**Overall:** Healthy", output);
        Assert.Contains("| Last success |  |", output); // null → ""
        Assert.Contains("| Consecutive failures | 0 |", output);
        Assert.Contains("| Runs since force | 0 |", output);
    }

    [Fact]
    public void Render_Degraded_WhenAnyCounterNonZero()
    {
        var templatePath = Path.Combine(_dir, "health-template.md");
        File.WriteAllText(templatePath, Template);
        var renderer = new HealthPageRenderer(templatePath, Path.Combine(_dir, "wiki"));
        var state = new HealthState { ServeConsecutiveFailures = 2 };

        renderer.Render(state);

        Assert.Contains("**Overall:** Degraded", File.ReadAllText(Path.Combine(_dir, "wiki", "status", "health.md")));
    }

    [Fact]
    public void Render_FormatsValues()
    {
        var templatePath = Path.Combine(_dir, "health-template.md");
        File.WriteAllText(templatePath, Template);
        var renderer = new HealthPageRenderer(templatePath, Path.Combine(_dir, "wiki"));
        var state = new HealthState
        {
            LastSuccessTime = DateTimeOffset.Parse("2026-08-01T10:00:00Z"),
            LastExitCode = 0,
            LastElementCount = 150,
            RunsSinceForce = 3,
        };

        renderer.Render(state);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "health.md"));
        Assert.Contains("| Last success | 08/01/2026 10:00:00 +00:00 |", output);
        Assert.Contains("| Last exit code | 0 |", output);
        Assert.Contains("| Last page count | 150 |", output);
        Assert.Contains("| Runs since force | 3 |", output);
    }
}