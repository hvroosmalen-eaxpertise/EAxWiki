# Connector Names Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace raw numeric EA element IDs in connector relationship tables with linked element names.

**Architecture:** Single file change to `MarkdownExporter.cs`. Move `elements.Add` before `WriteElementAsync`, pass `elements` list as a parameter, build a `Dictionary<int, (EaElement, string)>` lookup inside the method, and render the opposite element's name as a clickable page link.

**Tech Stack:** .NET 10, C#

## Global Constraints

- All user-derived filenames sanitized via `SanitizeName()`
- Relative links use `Path.GetRelativePath` + `.Replace('\\', '/')`
- Target framework: net10.0
- No test project — verify via `dotnet build`

---

### Task 1: Resolve connector names via element lookup

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:76-145` (ExportPackageAsync)
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:424-528` (WriteElementAsync)

**Interfaces:**
- Consumes: `List<(EaElement Element, string PackageDir)> elements` (already passed to `ExportPackageAsync`, now passed to `WriteElementAsync`)
- Produces: `WriteElementAsync` with new `elements` parameter; connector table shows linked element names

- [ ] **Step 1: Move `elements.Add` before `WriteElementAsync` and pass `elements`**

In `ExportPackageAsync`, change:

```csharp
elementTasks.Add(WriteElementAsync(elem, dir, outputDir, packageLookup));
elements.Add((elem, dir));
```

To:

```csharp
elements.Add((elem, dir));
elementTasks.Add(WriteElementAsync(elem, dir, outputDir, elements, packageLookup));
```

- [ ] **Step 2: Update the recursive sub-package call**

Find the call to `ExportPackageAsync` for sub-packages and verify it already passes `elements`. It should — the method signature already includes `List<(EaElement Element, string PackageDir)> elements`.

- [ ] **Step 3: Add `elements` parameter to `WriteElementAsync`**

Change signature from:

```csharp
private async Task WriteElementAsync(EaElement element, string dir, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
```

To:

```csharp
private async Task WriteElementAsync(EaElement element, string dir, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup)
```

- [ ] **Step 4: Replace connector rendering with lookup-based names**

Find the `## Relationships` section (around line 512). Replace this block:

```csharp
if (element.Connectors.Count > 0)
{
    lines.Add("## Relationships");
    lines.Add(string.Empty);
    lines.Add("| Type | Stereotype | Source → Target |");
    lines.Add("|------|------------|-----------------|");

    foreach (var conn in element.Connectors)
    {
        lines.Add($"| {conn.Type} | {conn.Stereotype} | {conn.SourceId} → {conn.TargetId} |");
    }

    lines.Add(string.Empty);
}
```

With:

```csharp
if (element.Connectors.Count > 0)
{
    lines.Add("## Relationships");
    lines.Add(string.Empty);
    lines.Add("| Type | Stereotype | Connected To |");
    lines.Add("|------|------------|-------------|");

    var lookup = elements.ToDictionary(e => e.Element.Id, e => e);

    foreach (var conn in element.Connectors)
    {
        var otherId = conn.SourceId == element.Id ? conn.TargetId
            : conn.TargetId == element.Id ? conn.SourceId
            : -1;

        string connectedTo;
        if (lookup.TryGetValue(otherId, out var other))
        {
            var otherName = SanitizeName(other.Element.Name);
            var relativePath = Path.GetRelativePath(dir, Path.Combine(other.PackageDir, $"{otherName}.md")).Replace('\\', '/');
            connectedTo = $"[{other.Element.Name}]({relativePath})";
        }
        else
        {
            connectedTo = $"Element ID {otherId} (not in export)";
        }

        lines.Add($"| {conn.Type} | {conn.Stereotype} | {connectedTo} |");
    }

    lines.Add(string.Empty);
}
```

- [ ] **Step 5: Build and fix any errors**

```bash
dotnet build src\EAxWiki\EAxWiki.csproj
```

Expected: Build succeeded with 0 warnings, 0 errors.

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: resolve connector IDs to element names with page links (Issue #15)"
```
