using EAxWiki.Monitor;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Tests;

public class MonitorFileLoggerTests : IDisposable
{
    private readonly string _dir;

    public MonitorFileLoggerTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_flog_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void LogsToDateStampedFile()
    {
        using var provider = new MonitorFileLoggerProvider(_dir);
        var logger = provider.CreateLogger("EAxWiki.Monitor.ExportRunner");
        logger.LogInformation("hello {X}", 42);
        provider.Dispose();

        var logDir = Path.Combine(_dir, "logs");
        var file = Directory.GetFiles(logDir, "monitor-*.log").Single();
        var content = File.ReadAllText(file);
        Assert.Contains("[INF] [ExportRunner] hello 42", content);
        Assert.Matches("^\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2} \\[INF\\] \\[ExportRunner\\] hello 42", content);
    }

    [Fact]
    public void LogsSeverityTokenPerLevel()
    {
        using var provider = new MonitorFileLoggerProvider(_dir);
        var logger = provider.CreateLogger("EAxWiki.Monitor.ExportRunner");
        logger.LogInformation("plain message");
        logger.LogWarning("careful now");
        logger.LogError(new InvalidOperationException("boom"), "failed");
        provider.Dispose();

        var content = File.ReadAllText(Directory.GetFiles(Path.Combine(_dir, "logs"), "monitor-*.log").Single());
        Assert.Contains("[INF] [ExportRunner] plain message", content);
        Assert.Contains("[WRN] [ExportRunner] careful now", content);
        Assert.Contains("[ERR] [ExportRunner] failed boom", content);
    }
}