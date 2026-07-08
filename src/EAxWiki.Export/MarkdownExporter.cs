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

    public async Task<ExportResult> ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null, bool force = false, CancellationToken cancellationToken = default)
    {
        MarkdownHelpers.ClearCache();
        var totalStopwatch = Stopwatch.StartNew();
        try
        {
            var parentDir = Path.GetDirectoryName(outputPath) ?? outputPath;
            var probeDir = Directory.Exists(outputPath) ? outputPath : parentDir;
            var testFile = Path.Combine(probeDir, ".write-test");
            try { File.WriteAllText(testFile, ""); File.Delete(testFile); }
            catch (Exception ex) { throw new InvalidOperationException($"Output path is not writable: {outputPath}", ex); }

            cancellationToken.ThrowIfCancellationRequested();

            if (force && Directory.Exists(outputPath))
                SafeDeleteContents(outputPath);

            Directory.CreateDirectory(outputPath);

            _logger.LogInformation("Export mode: {Mode}", force ? "full (--force)" : "incremental");

            var packages = startPackage != null
                ? new List<EaPackage> { startPackage }
                : repository.RootPackages;

            var statusTypes = reader?.GetStatusTypes() ?? [];
            int.TryParse(Environment.GetEnvironmentVariable("EAXWIKI_API_PORT"), out var apiPort);
            var apiToken = apiPort > 0 ? ApiTokenStore.GetOrCreate(outputPath) : string.Empty;
            var ctx = ContextBuilder.Build(packages, outputPath, force) with
            {
                StatusTypes = statusTypes,
                ApiPort = apiPort,
                ApiToken = apiToken,
            };

            var packageExporter = new PackageExporter(_writer, _logger);
            var totalElements = ctx.Elements.Count;
            var processedElements = 0;
            var totalFailed = 0;
            var totalDiagramsExported = 0;
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
            {
                cancellationToken.ThrowIfCancellationRequested();
                var (succeeded, failed) = await packageExporter.ExportAsync(pkg, ctx, OnElementsWritten, cancellationToken);
                processedElements += succeeded;
                totalFailed += failed;
            }

            await WriteRootIndexAsync(packages, outputPath, repository.ConnectionString, cancellationToken);
            ctx.WrittenMdFiles.Add(Path.Combine(outputPath, "index.md"));

            var diagramExporter = new DiagramExporter(_writer, _logger);
            var infrastructure = new InfrastructureWriter(_writer);

            var viewTasks = new List<Task>
            {
                new TypesExporter(_writer, _logger).ExportAsync(ctx, cancellationToken),
                new GlossaryExporter(_writer).ExportAsync(ctx, cancellationToken),
                new RecentChangesExporter(_writer).ExportAsync(ctx, cancellationToken),
                new StatusDashboardExporter(_writer).ExportAsync(ctx, cancellationToken),
                new ModelHealthExporter(_writer).ExportAsync(ctx, cancellationToken),
                diagramExporter.WriteIndexAsync(ctx, cancellationToken),
                infrastructure.WritePagesFileAsync(outputPath, cancellationToken),
                infrastructure.WriteExtraCssAsync(outputPath, cancellationToken),
                infrastructure.WriteGraphScriptsAsync(outputPath, cancellationToken),
                infrastructure.WriteStatusEditorScriptAsync(outputPath, cancellationToken),
                infrastructure.WriteNotesEditorScriptAsync(outputPath, cancellationToken),
                infrastructure.WriteRowNotesEditorScriptAsync(outputPath, cancellationToken),
            };

            if (reader != null)
                viewTasks.Add(diagramExporter.ExportPagesAsync(ctx, reader, cancellationToken));

            await Task.WhenAll(viewTasks).WaitAsync(cancellationToken);

            await InfrastructureWriter.CleanupOrphanedFilesAsync(ctx, cancellationToken);

            if (ctx.WrittenMdFiles.Count > 0)
            {
                var report = ExportValidator.Validate(ctx.WrittenMdFiles, outputPath);
                _logger.LogInformation("Validation: {Passed} passed, {Warnings} warnings, {Errors} errors across {Count} files",
                    report.Passed, report.Warnings, report.Errors, report.FilesValidated);

                if (report.Errors > 0 || report.Warnings > 0)
                {
                    var reportPath = Path.Combine(outputPath, ".validation-report.json");
                    await File.WriteAllTextAsync(reportPath, ExportValidator.ToJson(report), cancellationToken);
                }
            }

            totalStopwatch.Stop();
            var succeededElements = totalElements - totalFailed;
            _logger.LogInformation("Export complete: {TotalElapsedMs}ms total ({Succeeded} succeeded, {Failed} failed)",
                totalStopwatch.ElapsedMilliseconds, succeededElements, totalFailed);

            return new ExportResult(totalElements, succeededElements, totalFailed, totalDiagramsExported, totalStopwatch.Elapsed);
        }
        catch (OperationCanceledException)
        {
            totalStopwatch.Stop();
            _logger.LogInformation("Export cancelled after {TotalElapsedMs}ms", totalStopwatch.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            totalStopwatch.Stop();
            _logger.LogError(ex, "Export failed unexpectedly");
            return new ExportResult(0, 0, 0, 0, totalStopwatch.Elapsed);
        }
    }

    private static void SafeDeleteContents(string path)
    {
        foreach (var file in Directory.EnumerateFiles(path))
        {
            try { File.Delete(file); } catch { }
        }
        foreach (var dir in Directory.EnumerateDirectories(path))
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    private static string GetFriendlyRepoName(string repositoryPath)
    {
        if (string.IsNullOrEmpty(repositoryPath)) return "Wiki";
        if (!repositoryPath.Contains('=')) return Path.GetFileNameWithoutExtension(repositoryPath);
        var m = System.Text.RegularExpressions.Regex.Match(
            repositoryPath, @"(?i)(?:Database|Initial\s*Catalog)\s*=\s*([^;]+)");
        if (m.Success) return m.Groups[1].Value.Trim();
        m = System.Text.RegularExpressions.Regex.Match(
            repositoryPath, @"(?i)Data\s*Source\s*=\s*([^;]+)");
        if (m.Success) return m.Groups[1].Value.Trim();
        return "Wiki";
    }

    private async Task WriteRootIndexAsync(List<EaPackage> rootPackages, string outputDir, string repositoryPath, CancellationToken ct = default)
    {
        var siteName = GetFriendlyRepoName(repositoryPath);
        var lines = new List<string> { $"# {siteName}", string.Empty };

        if (!string.IsNullOrEmpty(repositoryPath))
        {
            lines.Add("## Repository");
            lines.Add(string.Empty);
            lines.Add(EaRepository.Redact(repositoryPath));
            lines.Add(string.Empty);
        }

        lines.Add("## Repository Structure");
        lines.Add(string.Empty);

        foreach (var pkg in rootPackages)
            lines.Add($"- [{pkg.Name}]({MarkdownHelpers.SanitizeName(pkg.Name)}/index.md)");

        lines.Add(string.Empty);
        lines.Add(MarkdownHelpers.FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(outputDir, "index.md"), string.Join(Environment.NewLine, lines), ct);
    }
}
