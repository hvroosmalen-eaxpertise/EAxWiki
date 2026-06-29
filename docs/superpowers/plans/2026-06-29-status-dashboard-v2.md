# Status Dashboard v2 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Extend the Status Dashboard with CSS bar charts, inline collapsible drill-down, and a new By Type breakdown.

**Architecture:** Two files change: `StatusDashboardExporter.cs` (rewritten to track element lists and generate bar charts + details blocks) and `InfrastructureWriter.cs` (CSS additions). No new exporter files or output directories.

**Tech Stack:** .NET 10, C# raw-string literals for CSS, HTML details/summary for collapsible sections.

## Global Constraints

- All generated content goes into `wiki/status/index.md` — no new sub-pages
- Use `MarkdownHelpers.SanitizeName()` for package name sanitization in anchor IDs
- Use `MarkdownHelpers.ParseStereotype()` for type detection
- Use `MarkdownHelpers.EscapeCell()` for all table cell content
- CSS bar widths proportional to max status count (max = 100%)
- No JS, no Mermaid, no external dependencies

---
### Task 1: Add CSS for bar charts and detail sections

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (CSS section within `WriteExtraCssAsync`)

- [ ] **Step 1: Add CSS classes for status bars and details/summary**

Insert after the `.status-invalid { ... }` block (line 302) and before the `.sl {` block (line 304):

```css
.status-bar-container {
  margin: 0.25em 0;
}
.status-bar {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0.15em 0.6em;
  font-size: 0.85em;
  font-weight: 600;
  color: #fff;
  white-space: nowrap;
  border-radius: 4px;
  min-width: fit-content;
  height: 1.6em;
  box-sizing: border-box;
}

details.status-details {
  margin: 0.3em 0;
  padding: 0.15em 0.6em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-code-bg-color);
}
details.status-details summary {
  cursor: pointer;
  font-weight: 600;
  padding: 0.15em 0;
}
details.status-details[open] summary {
  margin-bottom: 0.15em;
  border-bottom: 1px solid var(--md-default-fg-color--lightest);
  padding-bottom: 0.3em;
}
details.status-details ul {
  margin: 0.25em 0;
  padding-left: 1.5em;
}
details.status-details li {
  margin: 0.1em 0;
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build src/EAxWiki/EAxWiki.csproj 2>&1 | Select-String -Pattern "error|Build succeeded|Build FAILED"
```

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs
git commit -m "feat(status-dashboard): add CSS for bar charts and collapsible drill-down sections"
```

---
### Task 2: Rewrite StatusDashboardExporter with bars, drill-down, and By Type

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/StatusDashboardExporter.cs` (replace entire content)

- [ ] **Step 1: Replace the file with the new implementation**

The new exporter:
1. Iterates elements, collecting counts per (package, status) and per (type, status), plus element lists for drill-down
2. Generates Summary section with proportional CSS bar charts
3. Generates By Package table (counts link to anchor IDs) + collapsible `<details>` blocks
4. Generates By Type table + collapsible `<details>` blocks

```csharp
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
        WriteTable(lines, "By Package", statusList,
            packageRows.Values.OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase).ToList(),
            r => r.Name, r => r.Link,
            (r, s) => r.Counts.TryGetValue(s, out var c) ? c : 0,
            pkgDrilldown,
            (key, s) => key.Pkg == s,
            key => SanitizeForAnchor($"pkg-{key.Pkg}-{key.Status}"));

        // ── By Type ──
        var typeNames = typeDrilldown.Keys.Select(k => k.Type).Distinct()
            .OrderBy(t => t, StringComparer.OrdinalIgnoreCase).ToList();
        var typeRows = typeNames.Select(t => new TypeRow(t)).ToList();
        foreach (var row in typeRows)
            foreach (var ((tn, s), elements) in typeDrilldown)
                if (string.Equals(tn, row.Name, StringComparison.OrdinalIgnoreCase))
                    row.Counts[s] = row.Counts.GetValueOrDefault(s) + elements.Count;

        WriteTable(lines, "By Type", statusList, typeRows,
            r => r.Name, _ => "",
            (r, s) => r.Counts.TryGetValue(s, out var c) ? c : 0,
            typeDrilldown,
            (key, s) => key.Type == s,
            key => SanitizeForAnchor($"type-{key.Type}-{key.Status}"));

        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(dashboardDir, "index.md"),
            string.Join(Environment.NewLine, lines));
    }

    private static string CreateElementLink(Core.Models.EaElement element, string pkgDir, string fromDir)
    {
        var elemName = MarkdownHelpers.SanitizeName(element.Name);
        return Path.GetRelativePath(fromDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
    }

    private static string SanitizeForAnchor(string raw)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var ch in raw.ToLowerInvariant())
            sb.Append(char.IsLetterOrDigit(ch) ? ch : '_');
        return sb.ToString();
    }

    private static void WriteTable(
        List<string> lines, string heading, List<string> statusList,
        System.Collections.IList rows,
        Func<dynamic, string> getName, Func<dynamic, string> getLink,
        Func<dynamic, string, int> getCount,
        Dictionary<(string, string), List<(string Name, string Link)>> drilldown,
        Func<(string, string), string, bool> keyMatch,
        Func<(string, string), string> getAnchor)
    {
        lines.Add($"## {heading}");
        lines.Add(string.Empty);

        if (rows.Count == 0)
        {
            lines.Add("No elements with a status value found.");
            lines.Add(string.Empty);
            return;
        }

        var header = "| " + heading + " |"
            + string.Join("|", statusList.Select(s => $" {MarkdownHelpers.EscapeCell(s)} "))
            + "| Total |";
        var sep = "|---|" + string.Join("|", statusList.Select(_ => ":---:")) + "|:---:|";
        lines.Add(header);
        lines.Add(sep);

        foreach (var row in rows)
        {
            var name = getName(row);
            var link = getLink(row);
            var nameCell = string.IsNullOrEmpty(link)
                ? MarkdownHelpers.EscapeCell(name)
                : $"[{MarkdownHelpers.EscapeCell(name)}]({link})";
            var cells = statusList.Select(s =>
            {
                var c = getCount(row, s);
                if (c <= 0) return " — ";
                var anchor = getAnchor((name, s));
                return $" [{c}]({anchor}) ";
            });
            var total = statusList.Sum(s => getCount(row, s));
            lines.Add($"| {nameCell} |{string.Join("|", cells)}| **{total}** |");
        }

        // Totals row
        var totalCells = statusList.Select(s =>
        {
            var t = 0;
            foreach (var row in rows) t += getCount(row, s);
            return t > 0 ? $" **{t}** " : " — ";
        });
        var grandTotal = 0;
        foreach (var row in rows)
            foreach (var s in statusList) grandTotal += getCount(row, s);
        lines.Add($"| **Total** |{string.Join("|", totalCells)}| **{grandTotal}** |");
        lines.Add(string.Empty);

        // Drill-down details
        var drillPrefix = heading.ToLowerInvariant().Replace(" ", "-");
        foreach (var kvp in drilldown.OrderBy(k => k.Key.Item1).ThenBy(k => k.Key.Item2))
        {
            var (key, keyStatus) = kvp.Key;
            if (!keyMatch(kvp.Key, keyStatus)) continue;

            var elements = kvp.Value;
            var css = keyStatus.ToLowerInvariant();
            var anchor = SanitizeForAnchor($"{drillPrefix}-{keyStatus}");
            lines.Add($"<details class=\"status-details\" id=\"{anchor}\">");
            lines.Add($"  <summary>{MarkdownHelpers.EscapeCell(key)} — {MarkdownHelpers.EscapeCell(keyStatus)} ({elements.Count})</summary>");
            lines.Add(string.Empty);
            foreach (var (elemName, elemLink) in elements.OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase))
                lines.Add($"- [{MarkdownHelpers.EscapeCell(elemName)}]({elemLink}) <span class=\"status-badge status-{css}\">{MarkdownHelpers.EscapeCell(keyStatus)}</span>");
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
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build src/EAxWiki/EAxWiki.csproj 2>&1 | Select-String -Pattern "error|Build succeeded|Build FAILED"
```

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/Exporters/StatusDashboardExporter.cs
git commit -m "feat(status-dashboard): add bar charts, inline drill-down, and By Type section"
```<｜end▁of▁thinking｜>Let me clean up the code — the `dynamic`/`IList` approach is messy. I'll write it with direct inline sections instead:

<｜｜DSML｜｜tool_calls>
<｜｜DSML｜｜invoke name="edit">
<｜｜DSML｜｜parameter name="filePath" string="true">E:\Users\Han\Repos\EAxWiki\docs\superpowers\plans\2026-06-29-status-dashboard-v2.md