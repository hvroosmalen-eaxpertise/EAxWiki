# Diagram Pages Implementation Plan

> **For agentic workers:** Use subagent-driven-development or executing-plans to implement this plan task-by-task.

**Goal:** Add per-diagram pages with PNG images and element listings, linked from package index pages.

**Architecture:** New `EaDiagramObject` model tracks which elements appear on each diagram. The reader reads `EA.DiagramObject` from the COM API. The exporter generates `diagrams/{Name}.md` + `diagrams/{Name}.png` per diagram.

**Tech Stack:** .NET 10, C#, EA COM Interop

## Global Constraints

- PNG format for diagram images (format code `1` in `PutDiagramImageToFile`)
- `SanitizeName` used for all file/folder names
- Diagrams stored in `diagrams/` subfolder within each package directory
- Target framework: `net10.0`
- `System.Linq` available via implicit usings

---

### Task 1: Add EaDiagramObject model and update diagram reader

**Files:**
- Create: `src/EAxWiki.Core/Models/EaDiagramObject.cs`
- Modify: `src/EAxWiki.Core/Models/EaDiagram.cs`
- Modify: `src/EAxWiki.EA/EaReader.cs`

**Interfaces:**
- Consumes: `EA.DiagramObject` COM type (has `DiagramID`, `ElementID`, `Sequence`)
- Produces: `EaDiagramObject` in `EaDiagram.DiagramObjects`

- [ ] **Step 1: Create EaDiagramObject model**

Create `src/EAxWiki.Core/Models/EaDiagramObject.cs`:

```csharp
namespace EAxWiki.Core.Models;

public class EaDiagramObject
{
    public int DiagramId { get; set; }
    public int ElementId { get; set; }
    public int Sequence { get; set; }
}
```

- [ ] **Step 2: Add DiagramObjects to EaDiagram**

Edit `src/EAxWiki.Core/Models/EaDiagram.cs` — add `DiagramObjects` property:

```csharp
namespace EAxWiki.Core.Models;

public class EaDiagram
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int PackageId { get; set; }
    public List<EaDiagramObject> DiagramObjects { get; set; } = new();
}
```

- [ ] **Step 3: Read diagram objects in EaReader**

Edit `src/EAxWiki.EA/EaReader.cs` — update `MapDiagram` to read diagram objects:

```csharp
    private static EaDiagram MapDiagram(EA.Diagram eaDiagram)
    {
        var diagram = new EaDiagram
        {
            Id = eaDiagram.DiagramID,
            Name = eaDiagram.Name,
            Type = eaDiagram.Type,
            Notes = eaDiagram.Notes,
            PackageId = eaDiagram.PackageID
        };

        var diagramObjects = (EA.Collection)eaDiagram.DiagramObjects;
        for (short i = 0; i < diagramObjects.Count; i++)
        {
            var eaDO = (EA.DiagramObject)diagramObjects.GetAt(i);
            diagram.DiagramObjects.Add(new EaDiagramObject
            {
                DiagramId = eaDO.DiagramID,
                ElementId = eaDO.ElementID,
                Sequence = eaDO.Sequence
            });
        }

        return diagram;
    }
```

- [ ] **Step 4: Build and verify**

Run:
```
dotnet build --nologo
```

Expected: Build succeeded, 0 errors.

- [ ] **Step 5: Commit**

```
git add src/EAxWiki.Core/Models/EaDiagramObject.cs src/EAxWiki.Core/Models/EaDiagram.cs src/EAxWiki.EA/EaReader.cs
git commit -m "feat: add EaDiagramObject model and read diagram objects from EA"
```

---

### Task 2: Add diagram PNG export wiring

**Files:**
- Modify: `src/EAxWiki.Core/Interfaces/IWikiExporter.cs`
- Modify: `src/EAxWiki.Core/Interfaces/IEaReader.cs`
- Modify: `src/EAxWiki.EA/EaReader.cs`
- Modify: `src/EAxWiki/Program.cs`

**Interfaces:**
- Consumes: `EaReader` has access to EA `Repository` COM object
- Produces: `ExportDiagramImageAsync` delegate or method on exporter

- [ ] **Step 1: Add PNG export method to IEaReader**

Edit `src/EAxWiki.Core/Interfaces/IEaReader.cs` — add method for diagram export:

```csharp
namespace EAxWiki.Core.Interfaces;

public interface IEaReader
{
    EaRepository Open(string connectionString);
    void Close();
    bool ExportDiagramImage(int diagramId, string filePath);
}
```

- [ ] **Step 2: Implement in EaReader**

Edit `src/EAxWiki.EA/EaReader.cs` — add implementation:

```csharp
    public bool ExportDiagramImage(int diagramId, string filePath)
    {
        if (_repository == null) return false;
        try
        {
            var project = _repository.GetProjectInterface();
            project.PutDiagramImageToFile(diagramId, filePath, 1); // 1 = PNG
            return true;
        }
        catch
        {
            return false;
        }
    }
```

- [ ] **Step 3: Update IWikiExporter signature**

Edit `src/EAxWiki.Core/Interfaces/IWikiExporter.cs` — pass reader reference:

```csharp
using EAxWiki.Core.Models;

namespace EAxWiki.Core.Interfaces;

public interface IWikiExporter
{
    Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null);
}
```

- [ ] **Step 4: Build and verify**

```
dotnet build --nologo
```

Expected: Build succeeded (may have warnings about the existing `MarkdownExporter` not implementing the new parameter — ignore, next task fixes that).

- [ ] **Step 5: Commit**

```
git add src/EAxWiki.Core/Interfaces/IEaReader.cs src/EAxWiki.Core/Interfaces/IWikiExporter.cs src/EAxWiki.EA/EaReader.cs
git commit -m "feat: add diagram PNG export via IEaReader interface"
```

---

### Task 3: Generate diagram pages and update package index links

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`
- Modify: `src/EAxWiki/Program.cs`

**Interfaces:**
- Consumes: `IEaReader.ExportDiagramImage(id, path)`, `IOutputWriter`
- Produces: `diagrams/{Name}.md` and `diagrams/{Name}.png` per diagram

- [ ] **Step 1: Update MarkdownExporter.ExportAsync**

Edit `src/EAxWiki.Export/MarkdownExporter.cs`:

Update method signature to accept optional `IEaReader`:

```csharp
    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null)
```

After `await WritePagesFileAsync(outputPath);` (at end of ExportAsync), add:

```csharp
        if (reader != null)
        {
            await ExportDiagramsAsync(packages, elements, outputPath, reader);
        }
```

- [ ] **Step 2: Add ExportDiagramsAsync method**

Add after `WritePagesFileAsync`:

```csharp
    private async Task ExportDiagramsAsync(List<EaPackage> rootPackages, List<(EaElement Element, string PackageDir)> elements, string outputDir, IEaReader reader)
    {
        var elementLookup = elements
            .GroupBy(e => e.Element.Id)
            .ToDictionary(g => g.Key, g => g.First());

        var diagrams = CollectDiagrams(rootPackages);

        foreach (var (diagram, pkgDir) in diagrams)
        {
            var diagramsDir = Path.Combine(pkgDir, "diagrams");
            await _writer.CreateDirectoryAsync(diagramsDir);

            var fileName = SanitizeName(diagram.Name);
            var pngPath = Path.Combine(diagramsDir, $"{fileName}.png");
            var mdPath = Path.Combine(diagramsDir, $"{fileName}.md");

            reader.ExportDiagramImage(diagram.Id, pngPath);

            var lines = new List<string>
            {
                $"# {diagram.Name}",
                string.Empty,
            };

            if (!string.IsNullOrWhiteSpace(diagram.Notes))
            {
                lines.Add(diagram.Notes);
                lines.Add(string.Empty);
            }

            if (File.Exists(pngPath))
            {
                lines.Add($"![{diagram.Name}]({fileName}.png)");
                lines.Add(string.Empty);
            }

            var diagramElements = diagram.DiagramObjects
                .Select(dob => elementLookup.TryGetValue(dob.ElementId, out var elem) ? elem : null)
                .Where(e => e != null)
                .ToList();

            if (diagramElements.Count > 0)
            {
                lines.Add("## Elements");
                lines.Add(string.Empty);

                foreach (var elem in diagramElements)
                {
                    var elemLink = Path.GetRelativePath(diagramsDir, Path.Combine(pkgDir, $"{SanitizeName(elem!.Name)}.md")).Replace('\\', '/');
                    lines.Add($"- [{elem.Name}]({elemLink})");
                }

                lines.Add(string.Empty);
            }

            await _writer.WriteFileAsync(mdPath, string.Join(Environment.NewLine, lines));
        }
    }

    private static List<(EaDiagram Diagram, string PackageDir)> CollectDiagrams(List<EaPackage> packages)
    {
        var result = new List<(EaDiagram, string)>();
        foreach (var pkg in packages)
        {
            CollectDiagramsRecursive(pkg, string.Empty, result);
        }
        return result;
    }

    private static void CollectDiagramsRecursive(EaPackage package, string parentDir, List<(EaDiagram, string)> result)
    {
        var pkgDir = string.IsNullOrEmpty(parentDir)
            ? SanitizeName(package.Name)
            : Path.Combine(parentDir, SanitizeName(package.Name));

        foreach (var diagram in package.Diagrams)
        {
            result.Add((diagram, pkgDir));
        }

        foreach (var child in package.Children)
        {
            CollectDiagramsRecursive(child, pkgDir, result);
        }
    }
```

- [ ] **Step 3: Update package index.md diagram links**

In `ExportPackageAsync`, change the `## Diagrams` section. Find this block:

```csharp
        if (package.Diagrams.Count > 0)
        {
            indexLines.Add("## Diagrams");
            indexLines.Add(string.Empty);

            foreach (var diag in package.Diagrams)
            {
                indexLines.Add($"- {diag.Name} ({diag.Type})");

                if (!string.IsNullOrWhiteSpace(diag.Notes))
                {
                    var notesPreview = diag.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    indexLines.Add($"  - *{notesPreview}*");
                }
            }

            indexLines.Add(string.Empty);
        }
```

Replace with:

```csharp
        if (package.Diagrams.Count > 0)
        {
            indexLines.Add("## Diagrams");
            indexLines.Add(string.Empty);

            foreach (var diag in package.Diagrams)
            {
                var diagFile = $"diagrams/{SanitizeName(diag.Name)}.md";
                indexLines.Add($"- [{diag.Name}]({diagFile}) ({diag.Type})");

                if (!string.IsNullOrWhiteSpace(diag.Notes))
                {
                    var notesPreview = diag.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    indexLines.Add($"  - *{notesPreview}*");
                }
            }

            indexLines.Add(string.Empty);
        }
```

- [ ] **Step 4: Update Program.cs to pass reader to exporter**

Edit `src/EAxWiki/Program.cs` — change the export call:

```csharp
    await exporter.ExportAsync(repository, startPackage, outputPath, reader);
```

- [ ] **Step 5: Build and verify**

```
dotnet build --nologo
```

Expected: Build succeeded, 0 errors.

- [ ] **Step 6: Commit**

```
git add src/EAxWiki.Export/MarkdownExporter.cs src/EAxWiki/Program.cs
git commit -m "feat: generate diagram pages with PNG and element listings"
```
