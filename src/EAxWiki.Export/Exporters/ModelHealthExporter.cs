using System.Threading;
using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

/// <summary>
/// Reports content-quality issues in the EA model itself (orphan elements, missing descriptions,
/// untouched elements, duplicate names within a package) — distinct from ExportValidator, which
/// checks export/rendering mechanics, and from status/health.md, which reports export/serve
/// pipeline health. See issue #68.
/// </summary>
internal class ModelHealthExporter(IOutputWriter writer)
{
    /// <summary>
    /// Elements with a Status set whose ModifiedDate is older than this many days are flagged as
    /// untouched. ModifiedDate bumps on any field change (Notes, tagged values, relationships, etc.),
    /// not specifically on a Status change — EA COM exposes no cheaper way to track how long the
    /// Status value itself has been unchanged without the (usually disabled) audit-trail feature.
    /// </summary>
    private const int StaleThresholdDays = 90;

    public async Task ExportAsync(ExportContext ctx, CancellationToken ct = default)
    {
        var healthDir = Path.Combine(ctx.OutputPath, "status");
        await writer.CreateDirectoryAsync(healthDir, ct);

        var orphans = new List<(string Name, string Link)>();
        var missingDescriptions = new List<(string Name, string Link)>();
        var stale = new List<(string Name, string Link, string Status, DateTime Modified)>();
        var byPackageName = new Dictionary<(int PackageId, string Name), List<(string Name, string Link)>>();

        var now = DateTime.Now;

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            var link = MarkdownHelpers.CreateElementLink(elem, pkgDir, healthDir);
            var name = string.IsNullOrWhiteSpace(elem.Name) ? "unnamed" : elem.Name;
            var linkedName = $"[{MarkdownHelpers.EscapeCell(name)}]({link})";

            // Orphan: EA's Element.Connectors COM collection returns connectors where the element is
            // either client or supplier, so an element's own Connectors list already covers both
            // incoming and outgoing relationships — no need to cross-reference ctx.IncomingIndex too.
            var hasConnectors = elem.Connectors.Count > 0;
            var onDiagram = ctx.DiagramIndex.ContainsKey(elem.Id);
            if (!hasConnectors && !onDiagram)
                orphans.Add((name, linkedName));

            if (string.IsNullOrWhiteSpace(MarkdownHelpers.StripHtml(elem.Notes ?? string.Empty)))
                missingDescriptions.Add((name, linkedName));

            var status = elem.Status?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(status) && elem.ModifiedDate != DateTime.MinValue
                && (now - elem.ModifiedDate).TotalDays > StaleThresholdDays)
                stale.Add((name, linkedName, status, elem.ModifiedDate));

            if (!string.IsNullOrWhiteSpace(elem.Name))
            {
                var key = (elem.PackageId, elem.Name.Trim());
                if (!byPackageName.TryGetValue(key, out var group))
                {
                    group = [];
                    byPackageName[key] = group;
                }
                group.Add((name, linkedName));
            }
        }

        var duplicates = byPackageName.Values.Where(g => g.Count > 1).ToList();

        var lines = new List<string>
        {
            "# Model Health",
            string.Empty,
            "*Reports content-quality issues in the EA model itself — not export/serve pipeline health.*",
            string.Empty,
        };

        var totalIssues = orphans.Count + missingDescriptions.Count + stale.Count + duplicates.Sum(g => g.Count);
        if (totalIssues == 0)
        {
            lines.Add("No issues found.");
            lines.Add(string.Empty);
            lines.Add(MarkdownHelpers.FormatTimestamp());
            await writer.WriteFileAsync(Path.Combine(healthDir, "model-health.md"), string.Join(Environment.NewLine, lines), ct);
            return;
        }

        WriteOrphanSection(lines, orphans);
        WriteMissingDescriptionSection(lines, missingDescriptions);
        WriteStaleSection(lines, stale);
        WriteDuplicateSection(lines, duplicates);

        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(healthDir, "model-health.md"), string.Join(Environment.NewLine, lines), ct);
    }

    private static void WriteOrphanSection(List<string> lines, List<(string Name, string Link)> orphans)
    {
        lines.Add($"## Orphan Elements ({orphans.Count})");
        lines.Add(string.Empty);
        lines.Add("*No connectors and not placed on any diagram.*");
        lines.Add(string.Empty);
        if (orphans.Count == 0)
        {
            lines.Add("None.");
        }
        else
        {
            foreach (var (_, link) in orphans.OrderBy(o => o.Name, StringComparer.OrdinalIgnoreCase))
                lines.Add($"- {link}");
        }
        lines.Add(string.Empty);
    }

    private static void WriteMissingDescriptionSection(List<string> lines, List<(string Name, string Link)> missing)
    {
        lines.Add($"## Missing Descriptions ({missing.Count})");
        lines.Add(string.Empty);
        lines.Add("*Notes field is empty or whitespace-only.*");
        lines.Add(string.Empty);
        if (missing.Count == 0)
        {
            lines.Add("None.");
        }
        else
        {
            foreach (var (_, link) in missing.OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase))
                lines.Add($"- {link}");
        }
        lines.Add(string.Empty);
    }

    private static void WriteStaleSection(List<string> lines, List<(string Name, string Link, string Status, DateTime Modified)> stale)
    {
        lines.Add($"## Untouched {StaleThresholdDays}+ Days ({stale.Count})");
        lines.Add(string.Empty);
        lines.Add($"*Has a Status set, but no field on the element (Notes, tagged values, relationships, etc.) has changed in over {StaleThresholdDays} days. Does not track how long the Status value itself has been unchanged.*");
        lines.Add(string.Empty);
        if (stale.Count == 0)
        {
            lines.Add("None.");
        }
        else
        {
            lines.Add("| Element | Status | Last Modified |");
            lines.Add("|---|---|---|");
            foreach (var (_, link, status, modified) in stale.OrderBy(s => s.Modified))
                lines.Add($"| {link} | {MarkdownHelpers.EscapeCell(status)} | {modified:yyyy-MM-dd} |");
        }
        lines.Add(string.Empty);
    }

    private static void WriteDuplicateSection(List<string> lines, List<List<(string Name, string Link)>> duplicateGroups)
    {
        var total = duplicateGroups.Sum(g => g.Count);
        lines.Add($"## Duplicate Names Within a Package ({duplicateGroups.Count} name{(duplicateGroups.Count == 1 ? "" : "s")}, {total} elements)");
        lines.Add(string.Empty);
        lines.Add("*Two or more elements share a name within the same package. Duplicates across different packages are expected and not shown here.*");
        lines.Add(string.Empty);
        if (duplicateGroups.Count == 0)
        {
            lines.Add("None.");
        }
        else
        {
            foreach (var group in duplicateGroups.OrderBy(g => g[0].Name, StringComparer.OrdinalIgnoreCase))
                lines.Add($"- {string.Join(", ", group.Select(g => g.Link))}");
        }
        lines.Add(string.Empty);
    }
}
