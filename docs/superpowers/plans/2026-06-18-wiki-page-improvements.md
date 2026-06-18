# Wiki Page Improvements Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add repository path, generation date, namespace breadcrumb, reordered sections, and diagram status/description to generated wiki pages.

**Architecture:** Five self-contained changes to `MarkdownExporter.cs`, `EaDiagram.cs`, `EaReader.cs`, and `IEaReader.cs`. Each task builds successfully and can be verified independently (build-only since EA COM required for export testing).

**Tech Stack:** .NET 10, C#, EA COM interop

## Global Constraints

- No test project exists — verification is via `dotnet build --nologo`
- Must run `dotnet build --nologo` after each task and fix any errors
- All path comparisons use `SanitizeName()` for directory/file names
- File paths use `Path.Combine` throughout
- Relative links must use forward slashes (`.Replace('\\', '/')`)
- Target framework: `net10.0`
- Three-layer COM error isolation pattern already in place

---
### Task 1: Add Status to EaDiagram model + map in EaReader

**Files:**
- Modify: `src/EAxWiki.Core/Models/EaDiagram.cs:11`
- Modify: `src/EAxWiki.EA/EaReader.cs:154`

**Interfaces:**
- Consumes: `EA.Diagram.Status` (string property from COM)
- Produces: `EaDiagram.Status` (string?)

- [ ] **Add `Status` property to `EaDiagram`**

```csharp
// In EaDiagram.cs, after DiagramObjects line
public string? Status { get; set; }
```

- [ ] **Map `eaDiagram.Status` in `MapDiagram`**

```csharp
// In EaReader.cs MapDiagram, after PackageId line
Status = eaDiagram.Status,
```

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded, 0 warnings, 0 errors

- [ ] **Commit**

```bash
git add src/EAxWiki.Core/Models/EaDiagram.cs src/EAxWiki.EA/EaReader.cs
git commit -m "feat: add Status property to EaDiagram model"
```

---
### Task 2: Add RepositoryPath to IEaReader + implement in EaReader

**Files:**
- Modify: `src/EAxWiki.Core/Interfaces/IEaReader.cs:9`
- Modify: `src/EAxWiki.EA/EaReader.cs:172`

**Interfaces:**
- Produces: `IEaReader.RepositoryPath` (string getter)

- [ ] **Add `RepositoryPath` to `IEaReader`**

```csharp
// After ExportDiagramImage line
string RepositoryPath { get; }
```

- [ ] **Implement in `EaReader`**

```csharp
// Add as a class field alongside _repository
private string _repositoryPath = string.Empty;

// In Open(), after _repository.OpenFile(connectionString):
_repositoryPath = connectionString;

// Add property:
public string RepositoryPath => _repositoryPath;
```

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded

- [ ] **Commit**

```bash
git add src/EAxWiki.Core/Interfaces/IEaReader.cs src/EAxWiki.EA/EaReader.cs
git commit -m "feat: add RepositoryPath to IEaReader interface"
```

---
### Task 3: Reorder package pages — Diagrams before Elements

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:90-134`

- [ ] **Move the `## Diagrams` block above the `## Elements` block**

Swap the two blocks in `ExportPackageAsync` so the section order becomes:
1. Heading + Notes (unchanged)
2. `## Diagrams` (was second, now first)
3. `## Elements` (was first, now second)
4. `## Sub-packages` (unchanged)

The code for the Diagrams block (lines 116-134) and Elements block (lines 90-114) should swap positions. No content changes.

Result (visual sketch of the reordered method):
```csharp
// heading + notes (unchanged)

// ## Diagrams block (moved up)
if (package.Diagrams.Count > 0)
{
    indexLines.Add("## Diagrams");
    ...
}

// ## Elements block (moved down)
if (package.Elements.Count > 0)
{
    indexLines.Add("## Elements");
    ...
}

// ## Sub-packages block (unchanged)
```

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: show Diagrams before Elements on package pages"
```

---
### Task 4: Add generation date footer to all pages

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

- [ ] **Add `FormatTimestamp` helper**

Add to `MarkdownExporter` class:
```csharp
private static string FormatTimestamp()
{
    return $"---\n\n*Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";
}
```

- [ ] **Append footer to root index page** in `WriteRootIndexAsync` after line 179 (before `await _writer.WriteFileAsync`):
```csharp
lines.Add(FormatTimestamp());
```

- [ ] **Append footer to package index pages** in `ExportPackageAsync` after line 149 (before `await _writer.WriteFileAsync` at line 151):
```csharp
indexLines.Add(FormatTimestamp());
```

- [ ] **Append footer to element pages** in `WriteElementAsync` after line 352 (before `await _writer.WriteFileAsync` at line 354):
```csharp
lines.Add(FormatTimestamp());
```

- [ ] **Append footer to diagram pages** in `ExportDiagramsAsync` after line 419 (before `await _writer.WriteFileAsync` at line 422):
```csharp
lines.Add(FormatTimestamp());
```

- [ ] **Append footer to types index page** in `GenerateTypesPagesAsync` after line 214 (before `await _writer.WriteFileAsync` at line 216):
```csharp
indexLines.Add(FormatTimestamp());
```

- [ ] **Append footer to stereotype pages** in `GenerateTypesPagesAsync` after line 237 (before `await _writer.WriteFileAsync` at line 239):
```csharp
lines.Add(FormatTimestamp());
```

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add generation date footer to all pages"
```

---
### Task 5: Add namespace breadcrumb to package, element, and diagram pages

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

**Interfaces:**
- Consumes: `EaPackage.ParentId` (int?), existing `SanitizeName()`
- Produces: `BuildPackageLookup()`, `BuildBreadcrumb()` helpers

- [ ] **Add `BuildPackageLookup` helper**

```csharp
private static Dictionary<int, (string Name, int? ParentId)> BuildPackageLookup(List<EaPackage> packages)
{
    var lookup = new Dictionary<int, (string Name, int? ParentId)>();
    foreach (var pkg in packages)
    {
        BuildPackageLookupRecursive(pkg, lookup);
    }
    return lookup;
}

private static void BuildPackageLookupRecursive(EaPackage package, Dictionary<int, (string Name, int? ParentId)> lookup)
{
    lookup[package.Id] = (package.Name, package.ParentId);
    foreach (var child in package.Children)
    {
        BuildPackageLookupRecursive(child, lookup);
    }
}
```

- [ ] **Add `BuildBreadcrumb` helper**

```csharp
private static string BuildBreadcrumb(int packageId, string currentPageDir, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
{
    var segments = new List<string>();
    var id = packageId;
    while (id != 0 && packageLookup.TryGetValue(id, out var info))
    {
        segments.Add((info.Name, id));
        id = info.ParentId ?? 0;
    }
    segments.Reverse();

    var breadcrumbParts = new List<string>();
    foreach (var (name, id) in segments)
    {
        var pkgDir = Path.Combine(outputDir, SanitizeName(name));
        var relPath = Path.GetRelativePath(currentPageDir, Path.Combine(pkgDir, "index.md")).Replace('\\', '/');
        breadcrumbParts.Add($"[{name}]({relPath})");
    }
    return string.Join(" / ", breadcrumbParts);
}
```

- [ ] **Build lookup in `ExportAsync`**

Add before the `foreach (var pkg in packages)` loop (after line 37):
```csharp
var packageLookup = BuildPackageLookup(packages);
```

- [ ] **Add breadcrumb to package pages** in `ExportPackageAsync` after the heading + newline (after line 82):
```csharp
indexLines.Add(BuildBreadcrumb(package.Id, dir, outputDir, packageLookup));
indexLines.Add(string.Empty);
```

Need to pass `packageLookup` and `outputDir` into `ExportPackageAsync`. Update the signature on line 69:
```csharp
private async Task ExportPackageAsync(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup)
```

Also update the recursive call on line 159 to pass `packageLookup`.

- [ ] **Add breadcrumb to element pages** in `WriteElementAsync` after line 276 (after `$"**Stereotype:** ..."` line):
```csharp
lines.Add(string.Empty);
lines.Add(BuildBreadcrumb(element.PackageId, dir, outputDir, packageLookup));
lines.Add(string.Empty);
```

Need to pass `packageLookup`, `outputDir` into `WriteElementAsync`. Update signature:
```csharp
private async Task WriteElementAsync(EaElement element, string dir, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
```

Update the call on line 98 to pass `outputDir` and `packageLookup`.

- [ ] **Add breadcrumb to diagram pages** in `ExportDiagramsAsync` after line 387 (after the heading `$"# {diagram.Name}"` block):
```csharp
lines.Add(string.Empty);
// Find the package ID for this diagram — we need the package that contains it
// diagram.PackageId is available from the EaDiagram model
var (diagramPkgId, _) = diagrams.First(d => d.Diagram == diagram);
lines.Add(BuildBreadcrumb(diagramPkgId, diagramsDir, outputDir, packageLookup));
lines.Add(string.Empty);
```

Wait, this won't work easily — `diagrams` is a `List<(EaDiagram Diagram, string PackageDir)>`. I need to retrieve the diagram's package ID. I can change the collect to also return the package ID.

Actually, the `EaDiagram` has `PackageId`. So:
```csharp
lines.Add(BuildBreadcrumb(diagram.PackageId, diagramsDir, outputDir, packageLookup));
```

But `diagramsDir` = `Path.Combine(pkgDir, "diagrams")` where `pkgDir` is the diagram's package dir. So the breadcrumb relative paths from `diagramsDir` will need `../index.md` to get to the current package. Let me verify: from `outputDir/Organisation/diagrams/`, the Organisation index is at `../index.md`. `Path.GetRelativePath(diagramsDir, organisationDir)` = `..`. Then the link is `../index.md`. Good.

Do I need to pass `packageLookup` and `outputDir` into `ExportDiagramsAsync`? Let me check the current signature:
```csharp
private async Task ExportDiagramsAsync(List<EaPackage> rootPackages, List<(EaElement Element, string PackageDir)> elements, string outputDir, IEaReader reader)
```

Yes, I need to add `packageLookup`. Update signature:
```csharp
private async Task ExportDiagramsAsync(List<EaPackage> rootPackages, List<(EaElement Element, string PackageDir)> elements, string outputDir, IEaReader reader, Dictionary<int, (string Name, int? ParentId)> packageLookup)
```

And update the call in `ExportAsync` on line 52.

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add namespace breadcrumb to all pages"
```

---
### Task 6: Add repository path to root index page

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

- [ ] **Add `## Repository` section to `WriteRootIndexAsync`**

After the `## Structure` section (after line 178), add:
```csharp
if (!string.IsNullOrEmpty(repositoryPath))
{
    lines.Add("## Repository");
    lines.Add(string.Empty);
    lines.Add(repositoryPath);
    lines.Add(string.Empty);
}
```

Need to pass `repositoryPath` into `WriteRootIndexAsync`. Update signature:
```csharp
private async Task WriteRootIndexAsync(List<EaPackage> rootPackages, string outputDir, string repositoryPath)
```

Update call in `ExportAsync` on line 44:
```csharp
await WriteRootIndexAsync(packages, outputPath, repository.ConnectionString);
```

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add repository path to root index page"
```

---
### Task 7: Add Description label + Status to diagram pages

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:389-393`

- [ ] **Replace the notes block in `ExportDiagramsAsync`**

Replace lines 389-393:
```csharp
if (!string.IsNullOrWhiteSpace(diagram.Notes))
{
    lines.Add(diagram.Notes);
    lines.Add(string.Empty);
}
```

With:
```csharp
lines.Add($"**Description:** {diagram.Notes ?? "-"}");
lines.Add($"**Status:** {diagram.Status ?? "-"}");
lines.Add(string.Empty);
```

- [ ] **Build to verify**

Run: `dotnet build --nologo`
Expected: Build succeeded

- [ ] **Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add Description label and Status to diagram pages"
```

