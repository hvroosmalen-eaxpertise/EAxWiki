using EAxWiki.Monitor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class StaMarkdownExporterTests
{
    [Fact]
    public void ExporterLogger_IsNonNullTypedLogger()
    {
        var exporter = new StaMarkdownExporter(NullLoggerFactory.Instance);
        var logger = exporter.CreateExporterLogger();
        Assert.NotNull(logger);
        logger.LogInformation("probe");
    }
}