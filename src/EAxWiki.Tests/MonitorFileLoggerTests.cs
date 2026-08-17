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
        Assert.Contains("[ExportRunner] hello 42", content);
        Assert.Matches("^\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2} \\[ExportRunner\\] hello 42", content);
    }
}