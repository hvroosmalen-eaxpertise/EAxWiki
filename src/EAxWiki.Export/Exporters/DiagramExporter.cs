using System.Diagnostics;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class DiagramExporter(IOutputWriter writer, ILogger logger)
{
    /// <summary>Maximum length of diagram descriptions in the index table. Longer descriptions are truncated.</summary>
    private const int MaxDescriptionLength = 100;
    public async Task ExportPagesAsync(ExportContext ctx, IEaReader reader)
    {
        logger.LogInformation("Exporting {DiagramCount} diagrams with PNG images", ctx.AllDiagrams.Count);

        var failures = new List<string>();

        foreach (var (diagram, pkgDir) in ctx.AllDiagrams)
        {
            try
            {
                var diagramsDir = Path.Combine(pkgDir, "diagrams");
                await writer.CreateDirectoryAsync(diagramsDir);

                var fileName = MarkdownHelpers.SanitizeName(diagram.Name);
                var pngPath = Path.Combine(diagramsDir, $"{fileName}.png");
                var mdPath = Path.Combine(diagramsDir, $"{fileName}.md");
                var sw = Stopwatch.StartNew();

                var pngSuccess = reader.ExportDiagramImage(diagram.Guid, pngPath);
                if (!pngSuccess)
                    logger.LogWarning("Failed to export PNG for diagram {DiagramName}", diagram.Name);

                var lines = new List<string>
                {
                    $"# {diagram.Name}",
                    string.Empty,
                    string.Empty,
                    MarkdownHelpers.BuildBreadcrumb(diagram.PackageId, diagramsDir, ctx.OutputPath, ctx.PackageLookup),
                    string.Empty,
                };

                if (File.Exists(pngPath))
                {
                    lines.Add($"![{diagram.Name}]({fileName}.png)");
                    lines.Add(string.Empty);
                }

                lines.Add($"**Description:** {diagram.Notes ?? "-"}");
                lines.Add(string.Empty);

                var diagramElements = new List<(EaElement Element, string PackageDir)>();
                foreach (var dob in diagram.DiagramObjects)
                {
                    if (ctx.ElementLookup.TryGetValue(dob.ElementId, out var e))
                        diagramElements.Add(e);
                }

                if (diagramElements.Count > 0)
                {
                    lines.Add("## Elements");
                    lines.Add(string.Empty);

                    foreach (var (elemEa, elemDir) in diagramElements.OrderBy(e => e.Element.Name))
                    {
                        var elemLink = Path.GetRelativePath(diagramsDir, Path.Combine(elemDir, $"{MarkdownHelpers.SanitizeName(elemEa.Name)}.md")).Replace('\\', '/');
                        lines.Add($"- [{elemEa.Name}]({elemLink})");
                    }

                    lines.Add(string.Empty);
                }

                lines.Add(MarkdownHelpers.FormatTimestamp());
                await writer.WriteFileAsync(mdPath, string.Join(Environment.NewLine, lines));

                sw.Stop();
                logger.LogInformation("Exported diagram {DiagramName} in {ElapsedMs}ms", diagram.Name, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to export diagram {DiagramName}", diagram.Name);
                failures.Add(diagram.Name);
            }
        }

        if (failures.Count > 0)
            logger.LogWarning("[Export] {FailureCount} diagram(s) failed to export: {DiagramNames}",
                failures.Count, string.Join(", ", failures));
    }

    public async Task WriteIndexAsync(ExportContext ctx)
    {
        var diagramsDir = Path.Combine(ctx.OutputPath, "diagrams");
        await writer.CreateDirectoryAsync(diagramsDir);

        var sorted = ctx.AllDiagrams
            .Select(d => (d.Diagram, d.PackageDir, Path: MarkdownHelpers.BuildBreadcrumb(d.Diagram.PackageId, diagramsDir, ctx.OutputPath, ctx.PackageLookup)))
            .OrderBy(d => d.Path)
            .ThenBy(d => d.Diagram.Name)
            .ToList();

        var lines = new List<string> { "# Diagrams", string.Empty };

        if (sorted.Count == 0)
        {
            lines.Add("No diagrams found in model.");
        }
        else
        {
            lines.Add("| Diagram | Modified | Description | Path |");
            lines.Add("|---------|----------|-------------|------|");

            foreach (var (diagram, pkgDir, path) in sorted)
            {
                var diagramPage = Path.GetRelativePath(diagramsDir, Path.Combine(pkgDir, "diagrams", $"{MarkdownHelpers.SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
                var link = $"[{MarkdownHelpers.EscapeCell(diagram.Name)}]({diagramPage})";

                var modified = !string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt)
                    ? dt.ToString("yyyy-MM-dd")
                    : "-";

                var desc = string.IsNullOrWhiteSpace(diagram.Notes) ? "-" : MarkdownHelpers.EscapeCell(diagram.Notes.Trim());
                if (desc.Length > MaxDescriptionLength) desc = desc[..MaxDescriptionLength] + "...";

                lines.Add($"| {link} | {modified} | {desc} | {path} |");
            }
        }

        lines.Add(string.Empty);
        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(diagramsDir, "index.md"), string.Join(Environment.NewLine, lines));
    }
}
