# Where Used Cross-References (Issue #11) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add "Appears on Diagrams" and "Referenced By" sections to element pages.

**Architecture:** Build two reverse indices (diagram → elements, incoming connectors) once in `ExportAsync` after element collection, pass to `WriteElementAsync` as new parameters. Follows existing pattern from `elements` list and `packageLookup`.

**Tech Stack:** .NET 10, C#

## Global Constraints

- All user-derived filenames sanitized via `SanitizeName()`
- Relative links use `Path.GetRelativePath` + `.Replace('\\', '/')`
- Target framework: net10.0

---

### Task 1: Build indices and render "Appears on Diagrams" + "Referenced By"

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (EntryAsync and WriteElementAsync)

**Interfaces:**
- Consumes: existing `EaElement.CreatedDate`, `EaElement.Status`, `EaElement.Connectors`, `EaDiagram.DiagramObjects`, `EaDiagramObject.ElementId`
- Produces: renders two new sections on element pages under `## Relationships`

- [ ] **Step 1: Build the diagram index in `ExportAsync`**

After the element collection loop (line 47) and before the package export loop (line 50), add:

```csharp
// Build diagram index: element ID → list of (diagram, package dir)
var diagramIndex = new Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>>();
foreach (var (diagram, pkgDir) in CollectDiagrams(packages, outputPath))
{
    foreach (var dob in diagram.DiagramObjects)
    {
        if (!diagramIndex.ContainsKey(dob.ElementId))
            diagramIndex[dob.ElementId] = new List<(EaDiagram, string)>();
        diagramIndex[dob.ElementId].Add((diagram, pkgDir));
    }
}
```

- [ ] **Step 2: Build the incoming connector index in `ExportAsync`**

After the diagram index block, add:

```csharp
// Build incoming connector index: element ID → list of (connector, source element ID)
var incomingIndex = new Dictionary<int, List<(EaConnector Connector, int SourceId)>>();
foreach (var (elem, _) in elements)
{
    foreach (var conn in elem.Connectors)
    {
        if (!incomingIndex.ContainsKey(conn.TargetId))
            incomingIndex[conn.TargetId] = new List<(EaConnector, int)>();
        incomingIndex[conn.TargetId].Add((conn, conn.SourceId));
    }
}
```

- [ ] **Step 3: Thread both indices through `ExportPackageAsync` and `WriteElementAsync`**

In `ExportAsync`, pass indices to the package export loop:

```csharp
foreach (var pkg in packages)
{
    await ExportPackageAsync(pkg, outputPath, elements, packageLookup, diagramIndex, incomingIndex);
}
```

Update the `ExportPackageAsync` signature to add both parameters:

```csharp
private async Task ExportPackageAsync(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup, Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> diagramIndex, Dictionary<int, List<(EaConnector Connector, int SourceId)>> incomingIndex)
```

Update the call to `WriteElementAsync` inside `ExportPackageAsync` (line 136):

```csharp
elementTasks.Add(WriteElementAsync(elem, dir, outputDir, elements, packageLookup, diagramIndex, incomingIndex));
```

Update the recursive child call at line 193:

```csharp
await ExportPackageAsync(child, outputDir, elements, packageLookup, diagramIndex, incomingIndex);
```

- [ ] **Step 4: Update the method signature**

Change `WriteElementAsync` signature from:

```csharp
private async Task WriteElementAsync(EaElement element, string dir, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup)
```

to:

```csharp
private async Task WriteElementAsync(EaElement element, string dir, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup, Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> diagramIndex, Dictionary<int, List<(EaConnector Connector, int SourceId)>> incomingIndex)
```

- [ ] **Step 5: Render "Appears on Diagrams" section**

In `WriteElementAsync`, after the existing "Relationships" / "Connector" block (line 569), add:

```csharp
if (diagramIndex.TryGetValue(element.Id, out var elementDiagrams))
{
    lines.Add("### Appears on Diagrams");
    lines.Add(string.Empty);
    foreach (var (diagram, pkgDir) in elementDiagrams)
    {
        var diagDir = Path.Combine(pkgDir, "diagrams");
        var diagLink = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
        lines.Add($"- [{diagram.Name}]({diagLink})");
    }
    lines.Add(string.Empty);
}
```

- [ ] **Step 6: Render "Referenced By" section**

After the "Appears on Diagrams" block, add:

```csharp
if (incomingIndex.TryGetValue(element.Id, out var incomingConns))
{
    lines.Add("### Referenced By");
    lines.Add(string.Empty);
    lines.Add("| Type | Stereotype | Source |");
    lines.Add("|------|------------|--------|");
    var lookup = elements.GroupBy(e => e.Element.Id)
                         .ToDictionary(g => g.Key, g => g.First());
    foreach (var (conn, sourceId) in incomingConns)
    {
        string source;
        if (lookup.TryGetValue(sourceId, out var srcElem))
        {
            var srcName = SanitizeName(srcElem.Element.Name);
            var relativePath = Path.GetRelativePath(dir, Path.Combine(srcElem.PackageDir, $"{srcName}.md")).Replace('\\', '/');
            source = $"[{srcElem.Element.Name}]({relativePath})";
        }
        else
        {
            source = $"Element ID {sourceId} (not in export)";
        }
        lines.Add($"| {conn.Type} | {conn.Stereotype} | {source} |");
    }
    lines.Add(string.Empty);
}
```

- [ ] **Step 7: Build to verify**

```bash
dotnet build src/EAxWiki.Export/
```

Expected: Build succeeded, 0 warnings, 0 errors.

- [ ] **Step 8: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add Appears on Diagrams and Referenced By sections to element pages (Issue #11)"
```
