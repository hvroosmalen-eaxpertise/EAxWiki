using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class StatusDashboardExporter(IOutputWriter writer)
{
    public async Task ExportAsync(ExportContext ctx)
    {
        var dashboardDir = Path.Combine(ctx.OutputPath, "status");
        await writer.CreateDirectoryAsync(dashboardDir);

        // Discover all status values and count per package, dynamically.
        var statuses = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
        var packageRows = new Dictionary<string, PackageRow>(StringComparer.OrdinalIgnoreCase);
        int totalNoStatus = 0;

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            var status = elem.Status?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(status))
            {
                totalNoStatus++;
                continue;
            }

            statuses.Add(status);

            var pkgName = ctx.PackageLookup.TryGetValue(elem.PackageId, out var pkg) ? pkg.Name : "Unknown";
            if (!packageRows.TryGetValue(pkgName, out var row))
            {
                var relLink = Path.GetRelativePath(dashboardDir, Path.Combine(pkgDir, "index.md"))
                    .Replace('\\', '/');
                row = new PackageRow(pkgName, relLink);
                packageRows[pkgName] = row;
            }

            row.Counts.TryGetValue(status, out var current);
            row.Counts[status] = current + 1;
        }

        var statusList = statuses.ToList();

        // Totals per status across all packages
        var totals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in packageRows.Values)
            foreach (var (s, c) in row.Counts)
            {
                totals.TryGetValue(s, out var t);
                totals[s] = t + c;
            }

        var lines = new List<string> { "# Status Dashboard", string.Empty };

        // Summary badges
        lines.Add("## Summary");
        lines.Add(string.Empty);

        if (statuses.Count > 0)
        {
            var badges = statusList.Select(s =>
            {
                var css = s.ToLowerInvariant();
                var count = totals.TryGetValue(s, out var c) ? c : 0;
                return $"<span class=\"status-badge status-{css}\">{MarkdownHelpers.EscapeCell(s)}</span>&nbsp;**{count}**";
            });
            lines.Add(string.Join(" &nbsp;&nbsp; ", badges));
            lines.Add(string.Empty);
        }

        if (totalNoStatus > 0)
            lines.Add($"*{totalNoStatus} element{(totalNoStatus == 1 ? "" : "s")} have no status set.*");

        lines.Add(string.Empty);

        // Package breakdown table
        lines.Add("## By Package");
        lines.Add(string.Empty);

        if (packageRows.Count > 0)
        {
            var header = "| Package |" + string.Join("|", statusList.Select(s => $" {MarkdownHelpers.EscapeCell(s)} ")) + "| Total |";
            var sep = "|---|" + string.Join("|", statusList.Select(_ => ":---:")) + "|:---:|";
            lines.Add(header);
            lines.Add(sep);

            foreach (var row in packageRows.Values.OrderBy(r => r.Name))
            {
                var cells = statusList.Select(s =>
                    row.Counts.TryGetValue(s, out var c) ? $" {c} " : " — ");
                var total = row.Counts.Values.Sum();
                lines.Add($"| [{MarkdownHelpers.EscapeCell(row.Name)}]({row.Link}) |{string.Join("|", cells)}| **{total}** |");
            }

            // Totals row
            var totalCells = statusList.Select(s =>
                totals.TryGetValue(s, out var c) ? $" **{c}** " : " — ");
            var grandTotal = totals.Values.Sum();
            lines.Add($"| **Total** |{string.Join("|", totalCells)}| **{grandTotal}** |");
        }
        else
        {
            lines.Add("No elements with a status value found.");
        }

        lines.Add(string.Empty);
        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(dashboardDir, "index.md"),
            string.Join(Environment.NewLine, lines));
    }

    private sealed class PackageRow(string name, string link)
    {
        public string Name { get; } = name;
        public string Link { get; } = link;
        public Dictionary<string, int> Counts { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
