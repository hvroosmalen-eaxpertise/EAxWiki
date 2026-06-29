using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class StatusDashboardExporter(IOutputWriter writer)
{
    public async Task ExportAsync(ExportContext ctx)
    {
        var dashboardDir = Path.Combine(ctx.OutputPath, "status");
        await writer.CreateDirectoryAsync(dashboardDir);

        var statuses = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
        var packageRows = new Dictionary<string, PackageRow>(StringComparer.OrdinalIgnoreCase);
        var pkgDrilldown = new Dictionary<(string Pkg, string Status), List<(string Name, string Link)>>();
        var typeDrilldown = new Dictionary<(string Type, string Status), List<(string Name, string Link)>>();
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

            var elemLink = CreateElementLink(elem, pkgDir, dashboardDir);
            var elemName = string.IsNullOrWhiteSpace(elem.Name) ? "unnamed" : elem.Name;

            var pkgKey = (pkgName, status);
            if (!pkgDrilldown.TryGetValue(pkgKey, out var pkgList))
            {
                pkgList = [];
                pkgDrilldown[pkgKey] = pkgList;
            }
            pkgList.Add((elemName, elemLink));

            var (_, type) = MarkdownHelpers.ParseStereotype(
                !string.IsNullOrWhiteSpace(elem.FQStereotype) ? elem.FQStereotype
                : !string.IsNullOrWhiteSpace(elem.StereotypeEx) ? elem.StereotypeEx
                : elem.Stereotype);

            var typeKey = (type, status);
            if (!typeDrilldown.TryGetValue(typeKey, out var typeList))
            {
                typeList = [];
                typeDrilldown[typeKey] = typeList;
            }
            typeList.Add((elemName, elemLink));
        }

        var statusList = statuses.ToList();

        var totals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in packageRows.Values)
            foreach (var (s, c) in row.Counts)
            {
                totals.TryGetValue(s, out var t);
                totals[s] = t + c;
            }

        var maxCount = totals.Values.DefaultIfEmpty(0).Max();

        var lines = new List<string> { "# Status Dashboard", string.Empty };

        // ── Summary ──
        lines.Add("## Summary");
        lines.Add(string.Empty);

        if (maxCount > 0)
        {
            foreach (var s in statusList)
            {
                var css = s.ToLowerInvariant();
                var count = totals.GetValueOrDefault(s, 0);
                var pct = (int)Math.Round((double)count / maxCount * 100);
                lines.Add("<div class=\"status-bar-container\">");
                lines.Add($"  <div class=\"status-bar status-{css}\" style=\"width: {Math.Max(pct, 5)}%\">{MarkdownHelpers.EscapeCell(s)} {count}</div>");
                lines.Add("</div>");
            }
            lines.Add(string.Empty);
        }

        if (totalNoStatus > 0)
            lines.Add($"*{totalNoStatus} element{(totalNoStatus == 1 ? "" : "s")} have no status set.*");
        lines.Add(string.Empty);
        lines.Add(string.Empty);

        // ── By Package ──
        WritePackageSection(lines, statusList, packageRows.Values
            .OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase).ToList(), pkgDrilldown);

        // ── By Type ──
        var typeNames = typeDrilldown.Keys.Select(k => k.Type).Distinct()
            .OrderBy(t => t, StringComparer.OrdinalIgnoreCase).ToList();
        var typeRowList = typeNames.Select(t => new TypeRow(t)).ToList();
        foreach (var row in typeRowList)
            foreach (var ((tn, s), elements) in typeDrilldown)
                if (string.Equals(tn, row.Name, StringComparison.OrdinalIgnoreCase))
                    row.Counts[s] = row.Counts.GetValueOrDefault(s) + elements.Count;

        WriteTypeSection(lines, statusList, typeRowList, typeDrilldown);

        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(dashboardDir, "index.md"),
            string.Join(Environment.NewLine, lines));
    }

    private static string CreateElementLink(Core.Models.EaElement element, string pkgDir, string fromDir)
    {
        var elemName = MarkdownHelpers.SanitizeName(element.Name);
        var link = Path.GetRelativePath(fromDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
        return link.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ? link[..^3] : link;
    }

    private static string SanitizeForAnchor(string raw)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var ch in raw.ToLowerInvariant())
            sb.Append(char.IsLetterOrDigit(ch) ? ch : '_');
        return sb.ToString();
    }

    private static void WritePackageSection(List<string> lines, List<string> statusList,
        List<PackageRow> rows,
        Dictionary<(string Pkg, string Status), List<(string Name, string Link)>> drilldown)
    {
        lines.Add("## By Package");
        lines.Add(string.Empty);

        if (rows.Count == 0)
        {
            lines.Add("No elements with a status value found.");
            lines.Add(string.Empty);
            return;
        }

        var header = "| Package |" + string.Join("|", statusList.Select(s => $" {MarkdownHelpers.EscapeCell(s)} ")) + "| Total |";
        var sep = "|---|" + string.Join("|", statusList.Select(_ => ":---:")) + "|:---:|";
        lines.Add(header);
        lines.Add(sep);

        foreach (var row in rows)
        {
            var nameCell = $"[{MarkdownHelpers.EscapeCell(row.Name)}]({row.Link})";
            var cells = statusList.Select(s =>
            {
                var c = row.Counts.TryGetValue(s, out var v) ? v : 0;
                if (c <= 0) return " — ";
                var anchor = SanitizeForAnchor($"pkg-{row.Name}-{s}");
                return $" <a href=\"#{anchor}\">{c}</a> ";
            });
            var total = row.Counts.Values.Sum();
            lines.Add($"| {nameCell} |{string.Join("|", cells)}| **{total}** |");
        }

        // Totals row
        var totalCells = statusList.Select(s =>
        {
            var t = rows.Sum(r => r.Counts.TryGetValue(s, out var v) ? v : 0);
            return t > 0 ? $" **{t}** " : " — ";
        });
        var grandTotal = rows.Sum(r => r.Counts.Values.Sum());
        lines.Add($"| **Total** |{string.Join("|", totalCells)}| **{grandTotal}** |");
        lines.Add(string.Empty);

        // Drill-down: one details block per (package, status)
        foreach (var ((pkgName, drillStatus), elements) in drilldown
            .OrderBy(k => k.Key.Pkg, StringComparer.OrdinalIgnoreCase)
            .ThenBy(k => k.Key.Status, StringComparer.OrdinalIgnoreCase))
        {
            var css = drillStatus.ToLowerInvariant();
            var anchor = SanitizeForAnchor($"pkg-{pkgName}-{drillStatus}");
            lines.Add($"<details class=\"status-details\" id=\"{anchor}\">");
            lines.Add($"  <summary>{MarkdownHelpers.EscapeCell(pkgName)} &mdash; {MarkdownHelpers.EscapeCell(drillStatus)} ({elements.Count})</summary>");
            lines.Add(string.Empty);
            lines.Add("<ul>");
            foreach (var (elemName, elemLink) in elements.OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase))
                lines.Add($"  <li><a href=\"{elemLink}\">{MarkdownHelpers.EscapeCell(elemName)}</a> <span class=\"status-badge status-{css}\">{MarkdownHelpers.EscapeCell(drillStatus)}</span></li>");
            lines.Add("</ul>");
            lines.Add($"</details>");
        }
        lines.Add(string.Empty);
    }

    private static void WriteTypeSection(List<string> lines, List<string> statusList,
        List<TypeRow> rows,
        Dictionary<(string Type, string Status), List<(string Name, string Link)>> drilldown)
    {
        lines.Add("## By Type");
        lines.Add(string.Empty);

        if (rows.Count == 0)
        {
            lines.Add("No elements with a status value found.");
            lines.Add(string.Empty);
            return;
        }

        var header = "| Type |" + string.Join("|", statusList.Select(s => $" {MarkdownHelpers.EscapeCell(s)} ")) + "| Total |";
        var sep = "|---|" + string.Join("|", statusList.Select(_ => ":---:")) + "|:---:|";
        lines.Add(header);
        lines.Add(sep);

        foreach (var row in rows)
        {
            var cells = statusList.Select(s =>
            {
                var c = row.Counts.TryGetValue(s, out var v) ? v : 0;
                if (c <= 0) return " — ";
                var anchor = SanitizeForAnchor($"type-{row.Name}-{s}");
                return $" <a href=\"#{anchor}\">{c}</a> ";
            });
            var total = row.Counts.Values.Sum();
            lines.Add($"| {MarkdownHelpers.EscapeCell(row.Name)} |{string.Join("|", cells)}| **{total}** |");
        }

        // Totals row
        var totalCells = statusList.Select(s =>
        {
            var t = rows.Sum(r => r.Counts.TryGetValue(s, out var v) ? v : 0);
            return t > 0 ? $" **{t}** " : " — ";
        });
        var grandTotal = rows.Sum(r => r.Counts.Values.Sum());
        lines.Add($"| **Total** |{string.Join("|", totalCells)}| **{grandTotal}** |");
        lines.Add(string.Empty);

        // Drill-down: one details block per (type, status)
        foreach (var ((typeName, drillStatus), elements) in drilldown
            .OrderBy(k => k.Key.Type, StringComparer.OrdinalIgnoreCase)
            .ThenBy(k => k.Key.Status, StringComparer.OrdinalIgnoreCase))
        {
            var css = drillStatus.ToLowerInvariant();
            var anchor = SanitizeForAnchor($"type-{typeName}-{drillStatus}");
            lines.Add($"<details class=\"status-details\" id=\"{anchor}\">");
            lines.Add($"  <summary>{MarkdownHelpers.EscapeCell(typeName)} &mdash; {MarkdownHelpers.EscapeCell(drillStatus)} ({elements.Count})</summary>");
            lines.Add(string.Empty);
            lines.Add("<ul>");
            foreach (var (elemName, elemLink) in elements.OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase))
                lines.Add($"  <li><a href=\"{elemLink}\">{MarkdownHelpers.EscapeCell(elemName)}</a> <span class=\"status-badge status-{css}\">{MarkdownHelpers.EscapeCell(drillStatus)}</span></li>");
            lines.Add("</ul>");
            lines.Add($"</details>");
        }
        lines.Add(string.Empty);
    }

    private sealed class PackageRow(string name, string link)
    {
        public string Name { get; } = name;
        public string Link { get; } = link;
        public Dictionary<string, int> Counts { get; } = new(StringComparer.OrdinalIgnoreCase);
    }

    private sealed class TypeRow(string name)
    {
        public string Name { get; } = name;
        public Dictionary<string, int> Counts { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
