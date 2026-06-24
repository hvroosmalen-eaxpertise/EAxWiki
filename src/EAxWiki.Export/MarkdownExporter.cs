using System.Diagnostics;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Exporters;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export;

public class MarkdownExporter : IWikiExporter
{
    private readonly IOutputWriter _writer;
    private readonly ILogger<MarkdownExporter> _logger;

    public MarkdownExporter(IOutputWriter writer, ILogger<MarkdownExporter> logger)
    {
        _writer = writer;
        _logger = logger;
    }

    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null, bool force = false)
    {
        MarkdownHelpers.ClearCache();
        try
        {
            if (force && Directory.Exists(outputPath))
                Directory.Delete(outputPath, recursive: true);

            Directory.CreateDirectory(outputPath);

            var testFile = Path.Combine(outputPath, ".write-test");
            try { File.WriteAllText(testFile, ""); File.Delete(testFile); }
            catch (Exception ex) { throw new InvalidOperationException($"Output path is not writable: {outputPath}", ex); }

            var totalStopwatch = Stopwatch.StartNew();

            var packages = startPackage != null
                ? new List<EaPackage> { startPackage }
                : repository.RootPackages;

            var ctx = ContextBuilder.Build(packages, outputPath);

            var packageExporter = new PackageExporter(_writer, _logger);
            var totalElements = ctx.Elements.Count;
            var processedElements = 0;
            const int progressInterval = 50;
            void OnElementsWritten(int count)
            {
                var previous = processedElements;
                processedElements += count;
                var previousMilestone = previous / progressInterval;
                var currentMilestone = processedElements / progressInterval;
                if (currentMilestone > previousMilestone && totalElements > 0)
                    _logger.LogInformation("[Export] Processing element {Processed} / {Total}...", processedElements, totalElements);
            }

            foreach (var pkg in packages)
                await packageExporter.ExportAsync(pkg, ctx, OnElementsWritten);

            await WriteRootIndexAsync(packages, outputPath, repository.ConnectionString);

            var diagramExporter = new DiagramExporter(_writer, _logger);
            var infrastructure = new InfrastructureWriter(_writer);

            var viewTasks = new List<Task>
            {
                new TypesExporter(_writer, _logger).ExportAsync(ctx),
                new GlossaryExporter(_writer).ExportAsync(ctx),
                new RecentChangesExporter(_writer).ExportAsync(ctx),
                diagramExporter.WriteIndexAsync(ctx),
                infrastructure.WritePagesFileAsync(outputPath),
                infrastructure.WriteExtraCssAsync(outputPath),
            };

            if (reader != null)
                viewTasks.Add(diagramExporter.ExportPagesAsync(ctx, reader));

            await Task.WhenAll(viewTasks);

            await InfrastructureWriter.CleanupOrphanedFilesAsync(ctx.Elements);

            totalStopwatch.Stop();
            _logger.LogInformation("Export complete: {TotalElapsedMs}ms total", totalStopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Export failed unexpectedly");
        }
    }

    private async Task WriteRootIndexAsync(List<EaPackage> rootPackages, string outputDir, string repositoryPath)
    {
        var lines = new List<string> { "# EurSuRA Wiki", string.Empty };

        if (!string.IsNullOrEmpty(repositoryPath))
        {
            lines.Add("## Repository");
            lines.Add(string.Empty);
            lines.Add(repositoryPath);
            lines.Add(string.Empty);
        }

        lines.Add("## Repository Structure");
        lines.Add(string.Empty);

        foreach (var pkg in rootPackages)
            lines.Add($"- [{pkg.Name}]({MarkdownHelpers.SanitizeName(pkg.Name)}/index.md)");

        lines.Add(string.Empty);
        lines.Add(MarkdownHelpers.FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(outputDir, "index.md"), string.Join(Environment.NewLine, lines));
    }
}
