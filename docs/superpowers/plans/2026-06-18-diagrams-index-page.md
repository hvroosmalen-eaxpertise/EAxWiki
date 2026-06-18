# Diagrams Index Page Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a nav-level "Diagrams" page listing all diagrams across the wiki in a Markdown table.

**Architecture:** Static generated page at export time — a new `WriteDiagramsIndexAsync` method in `MarkdownExporter` collects all diagrams, builds a table sorted by repository path then name, and writes `wiki/diagrams/index.md`. Root `.pages` updated to include the Diagrams nav entry.

**Tech Stack:** C# (.NET), Markdown, EA COM Interop

---

### Task 1: Add ModifiedDate to EaDiagram model

**Files:**
- Modify: `src/EAxWiki.Core/Models/EaDiagram.cs`

- [ ] **Add ModifiedDate property**

```csharp
public string? ModifiedDate { get; set; }
```

Add after `public string? Notes { get; set; }`.

- [ ] **Commit**

```bash
git add src/EAxWiki.Core/Models/EaDiagram.cs
git commit -m "feat: add ModifiedDate to EaDiagram model"
```

---

### Task 2: Read ModifiedDate from EA COM in EaReader

**Files:**
- Modify: `src/EAxWiki.EA/EaReader.cs` (line ~157)

- [ ] **Read ModifiedDate in MapDiagram**

In `MapDiagram`, add after `Notes = eaDiagram.Notes,`:
```csharp
ModifiedDate = eaDiagram.ModifiedDate,
```

- [ ] **Commit**

```bash
git add src/EAxWiki.EA/EaReader.cs
git commit -m "feat: read ModifiedDate from EA.Diagram COM object"
```

---

### Task 3: Add WriteDiagramsIndexAsync to MarkdownExporter

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

- [ ] **Add method after WritePagesFileAsync**

Add this method before `FormatTimestamp`:

```csharp
private async Task WriteDiagramsIndexAsync(List<EaPackage> rootPackages, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
{
    var diagramsDir = Path.Combine(outputDir, "diagrams");
    await _writer.CreateDirectoryAsync(diagramsDir);

    var allDiagrams = CollectDiagrams(rootPackages, outputDir);

    var sorted = allDiagrams
        .Select(d => (d.Diagram, d.PackageDir, Path: BuildBreadcrumb(d.Diagram.PackageId, diagramsDir, outputDir, packageLookup)))
        .OrderBy(d => d.Path)
        .ThenBy(d => d.Diagram.Name)
        .ToList();

    var lines = new List<string>
    {
        "# Diagrams",
        string.Empty,
    };

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
            var diagramPage = Path.GetRelativePath(diagramsDir, Path.Combine(pkgDir, "diagrams", $"{SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
            var link = $"[{diagram.Name}]({diagramPage})";

            var modified = !string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt)
                ? dt.ToString("yyyy-MM-dd")
                : "-";

            var desc = string.IsNullOrWhiteSpace(diagram.Notes) ? "-" : diagram.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Trim();
            if (desc.Length > 100) desc = desc[..100] + "...";

            lines.Add($"| {link} | {modified} | {desc} | {path} |");
        }
    }

    lines.Add(string.Empty);
    lines.Add(FormatTimestamp());
    await _writer.WriteFileAsync(Path.Combine(diagramsDir, "index.md"), string.Join(Environment.NewLine, lines));
}
```

- [ ] **Call it from ExportAsync**

In `ExportAsync`, after the `ExportDiagramsAsync` block (line ~59), add:

```csharp
await WriteDiagramsIndexAsync(packages, outputPath, packageLookup);
```

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add WriteDiagramsIndexAsync for global diagram listing"
```

---

### Task 4: Add Diagrams nav entry to .pages

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (WritePagesFileAsync)

- [ ] **Update WritePagesFileAsync**

Change the `content` list from:
```csharp
"nav:",
"  - Structure: ''",
"  - Types: types/",
```

To:
```csharp
"nav:",
"  - Structure: ''",
"  - Diagrams: diagrams/",
"  - Types: types/",
```

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "fix: add Diagrams to nav menu .pages"
```

---

### Task 5: Run export and commit wiki output

**Files:**
- Modify: wiki/ (regenerated output)

- [ ] **Run export**

```bash
cd scripts && ./export-and-serve.ps1
```

- [ ] **Commit and push**

```bash
git add wiki/ docs/superpowers/plans/2026-06-18-diagrams-index-page.md
git commit -m "docs: regenerate wiki with diagrams index page"
git push
```
