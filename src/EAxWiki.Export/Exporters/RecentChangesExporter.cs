using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class RecentChangesExporter(IOutputWriter writer)
{
    /// <summary>Maximum entries shown in the Recent Changes view. Adjust to show more or fewer.</summary>
    private const int TopN = 50;

    public async Task ExportAsync(ExportContext ctx)
    {
        var outputDir = ctx.OutputPath;
        var recentDir = Path.Combine(outputDir, "recent");
        await writer.CreateDirectoryAsync(recentDir);

        var entries = new List<(string Name, string Type, DateTime? ModifiedDate, string Path)>();

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            var elemName = MarkdownHelpers.SanitizeName(elem.Name);
            var link = Path.GetRelativePath(recentDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
            var path = MarkdownHelpers.BuildBreadcrumb(elem.PackageId, recentDir, outputDir, ctx.PackageLookup);
            var modified = elem.ModifiedDate == DateTime.MinValue ? (DateTime?)null : elem.ModifiedDate;
            entries.Add(($"[{MarkdownHelpers.EscapeCell(elem.Name)}]({link})", MarkdownHelpers.EscapeCell(elem.Type), modified, path));
        }

        foreach (var (diagram, pkgDir) in ctx.AllDiagrams)
        {
            var diagramPage = Path.GetRelativePath(recentDir, Path.Combine(pkgDir, "diagrams", $"{MarkdownHelpers.SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
            var path = MarkdownHelpers.BuildBreadcrumb(diagram.PackageId, recentDir, outputDir, ctx.PackageLookup);
            DateTime? modified = null;
            if (!string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt))
                modified = dt;
            entries.Add(($"[{MarkdownHelpers.EscapeCell(diagram.Name)}]({diagramPage})", "Diagram", modified, path));
        }

        var sorted = entries
            .OrderByDescending(e => e.ModifiedDate.HasValue)
            .ThenByDescending(e => e.ModifiedDate)
            .Take(TopN)
            .ToList();

        var lines = new List<string> { "# Recent Changes", string.Empty };

        if (sorted.Count > 0)
        {
            lines.Add("| Name | Type | Modified | Path |");
            lines.Add("|------|------|----------|------|");

            foreach (var (name, type, modified, path) in sorted)
            {
                var modifiedStr = modified?.ToString("yyyy-MM-dd") ?? "-";
                lines.Add($"| {name} | {type} | {modifiedStr} | {path} |");
            }
        }
        else
        {
            lines.Add("No recent changes found.");
        }

        lines.Add(string.Empty);
        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(recentDir, "index.md"), string.Join(Environment.NewLine, lines));
    }
}
