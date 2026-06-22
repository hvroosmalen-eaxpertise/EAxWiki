# Recent Changes Page (Issue #13) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Generate a `recent/index.md` page listing the 50 most recently modified elements and diagrams.

**Architecture:** New `GenerateRecentChangesAsync` method in `MarkdownExporter.cs`, called from `ExportAsync`. Merges elements and diagrams into one list, sorts by ModifiedDate descending, renders table. Same pattern as glossary and types pages.

**Tech Stack:** .NET 10, C#

## Global Constraints

- All user-derived filenames sanitized via `SanitizeName()`
- Relative links use `Path.GetRelativePath` + `.Replace('\\', '/')`
- Target framework: net10.0

---

### Task 1: Implement `GenerateRecentChangesAsync` and wire it up

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

**Interfaces:**
- Consumes: `elements` list (from `ExportAsync`), `packageLookup` (from `ExportAsync`), `CollectDiagrams` helper
- Produces: `recent/index.md` page, root `.pages` nav entry

- [ ] **Step 1: Add call to `GenerateRecentChangesAsync` in `ExportAsync`**

After `GenerateGlossaryAsync(elements, outputPath)` and before `WritePagesFileAsync(outputPath)`, add:

```csharp
await GenerateRecentChangesAsync(packages, elements, outputPath, packageLookup);
```

- [ ] **Step 2: Add recent nav entry to root `.pages`**

In `WritePagesFileAsync`, change the root `.pages` nav block from:

```csharp
"  - Glossary: glossary/",
```

to:

```csharp
"  - Glossary: glossary/",
"  - Recent: recent/",
```

- [ ] **Step 3: Implement `GenerateRecentChangesAsync` method**

Add this new method after `GenerateGlossaryAsync`:

```csharp
private async Task GenerateRecentChangesAsync(
    List<EaPackage> rootPackages,
    List<(EaElement Element, string PackageDir)> elements,
    string outputDir,
    Dictionary<int, (string Name, int? ParentId)> packageLookup)
{
    var recentDir = Path.Combine(outputDir, "recent");
    await _writer.CreateDirectoryAsync(recentDir);

    const int topN = 50;

    // Build element entries
    var entries = new List<(string Name, string Type, DateTime? ModifiedDate, string Path)>();

    foreach (var (elem, pkgDir) in elements)
    {
        var elemName = SanitizeName(elem.Name);
        var link = Path.GetRelativePath(recentDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
        var path = BuildBreadcrumb(elem.PackageId, recentDir, outputDir, packageLookup);
        var modified = elem.ModifiedDate == DateTime.MinValue ? (DateTime?)null : elem.ModifiedDate;
        entries.Add(($"[{elem.Name}]({link})", elem.Type, modified, path));
    }

    // Build diagram entries
    var diagrams = CollectDiagrams(rootPackages, outputDir);
    foreach (var (diagram, pkgDir) in diagrams)
    {
        var diagramPage = Path.GetRelativePath(recentDir, Path.Combine(pkgDir, "diagrams", $"{SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
        var path = BuildBreadcrumb(diagram.PackageId, recentDir, outputDir, packageLookup);
        DateTime? modified = null;
        if (!string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt))
            modified = dt;
        entries.Add(($"[{diagram.Name}]({diagramPage})", "Diagram", modified, path));
    }

    // Sort: most recent first, nulls at bottom
    var sorted = entries
        .OrderByDescending(e => e.ModifiedDate.HasValue)
        .ThenByDescending(e => e.ModifiedDate)
        .Take(topN)
        .ToList();

    var lines = new List<string>
    {
        "# Recent Changes",
        string.Empty,
    };

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
    lines.Add(FormatTimestamp());
    await _writer.WriteFileAsync(Path.Combine(recentDir, "index.md"), string.Join(Environment.NewLine, lines));
}
```

- [ ] **Step 4: Build to verify**

```bash
dotnet build src/EAxWiki.Export/
```

Expected: Build succeeded, 0 warnings, 0 errors.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: generate recent changes page (Issue #13)"
```
