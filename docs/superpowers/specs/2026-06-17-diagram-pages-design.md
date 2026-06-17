# Diagram Pages Design

## Summary

Add per-diagram pages to the wiki. Each diagram generates a dedicated `.md` page with an embedded PNG image and a list of all elements appearing on that diagram. Links from package index pages point to the diagram page instead of showing just the diagram name.

## Data Model

### New: `EaDiagramObject`

```
namespace EAxWiki.Core.Models;

public class EaDiagramObject
{
    public int DiagramId { get; set; }
    public int ElementId { get; set; }
    public int Sequence { get; set; }
}
```

### Modified: `EaDiagram`

Add `DiagramObjects` collection:

```
public List<EaDiagramObject> DiagramObjects { get; set; } = new();
```

## Reader Changes (EaReader.cs)

In `MapDiagram`, iterate `eaDiagram.DiagramObjects` (EA COM `EA.Collection` of `EA.DiagramObject`) and map to `EaDiagramObject`:

- `eaDO.DiagramID` → `DiagramId`
- `eaDO.ElementID` → `ElementId`
- `eaDO.Sequence` → `Sequence`

No other reader changes needed — elements are already loaded with their `Id`.

## Exporter Changes (MarkdownExporter.cs)

### Export flow

During `ExportAsync`, after the main package/element export loop and after `WriteRootIndexAsync`, add a diagram export pass:

1. Collect all diagrams from all exported packages (recursive walk)
2. Build a `Dictionary<int, EaElement>` from all collected elements for fast lookup
3. For each diagram:
   - Create `diagrams/` subfolder in the diagram's package directory
   - Export PNG via EA API: `Project.PutDiagramImageToFile(diagramId, filePath, 1)` (1 = PNG format)
   - Generate `{SanitizedName}.md` with:
     - `# {Name}` heading
     - Notes (if any)
     - Embedded image: `![{Name}]({SanitizedName}.png)`
     - `## Elements` section listing each element on the diagram (resolved via `EaDiagramObject.ElementId` → `EaElement.Name`), linking to the element page
4. Update each package's `index.md` to link diagrams as `[{Name}](diagrams/{SanitizedName}.md)` instead of plain text

### Package index diagram links

Change the `## Diagrams` section in `ExportPackageAsync` from:

```
- {Name} ({Type})
```

To:

```
- [{Name}](diagrams/{SanitizedName}.md) ({Type})
```

With notes still shown inline.

### EA Repository access

The PNG export requires the EA `Repository` object (or specifically `Project` interface). Pass a `Func<int, string, bool>?` delegate to the exporter for image export. The `Program.cs` entry point wires it to the actual EA COM call.

### EA COM image export code

```csharp
// In Program.cs or EaReader:
project.PutDiagramImageToFile(diagramId, filePath, 1); // 1 = PNG
```

The EA `Repository.RepositoryType` provides access to `Repository.GetProjectInterface()` which returns `EA.Project`. `PutDiagramImageToFile(diagramID, fileName, imageFormat)` where imageFormat 1 = PNG.

### Output structure example

For package `Metrics` with diagram "Metrics Overview":

```
wiki/Metrics/
  index.md
  {Element1}.md
  ...
  diagrams/
    Metrics Overview.md
    Metrics Overview.png
```

### Edge cases

- **Diagram with no elements** — Show "No elements on this diagram." message
- **Diagram export fails** — Skip PNG, generate page without image (log warning)
- **Element referenced by diagram but not in collected elements** — Show name as plain text
