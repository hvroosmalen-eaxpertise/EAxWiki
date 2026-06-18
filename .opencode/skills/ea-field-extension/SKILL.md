---
name: ea-field-extension
description: Use when adding a new EA model field to the wiki output - covers the 3-layer pattern of model property, COM reader mapping, and exporter rendering
---

# EA Field Extension

## Overview

Adding a new EA model field to the wiki follows three layers: **Model -> Reader -> Exporter**. Each layer has a specific file and pattern.

## The 3-Layer Pattern

### Layer 1: Model (`src/EAxWiki.Core/Models/`)

Add a property to the relevant model class.

| Model Class | File |
|---|---|
| `EaPackage` | `EaPackage.cs` |
| `EaElement` | `EaElement.cs` |
| `EaDiagram` | `EaDiagram.cs` |
| `EaConnector` | embedded in `EaElement.cs` |
| `EaAttribute` | embedded in `EaElement.cs` |
| `EaMethod` | embedded in `EaElement.cs` |
| `EaTaggedValue` | embedded in `EaElement.cs` |
| `EaDiagramObject` | embedded in `EaDiagram.cs` |

### Layer 2: Reader (`src/EAxWiki.EA/EaReader.cs`)

Read the field in the corresponding `MapXxx()` method.

| EA COM type | Reader method |
|---|---|
| `EA.Package` | `MapPackage()` |
| `EA.Element` | `MapElement()` |
| `EA.Diagram` | `MapDiagram()` |
| `EA.Connector` | inside `MapElement()` |
| `EA.Attribute` | inside `MapElement()` |
| `EA.Method` | inside `MapElement()` |
| `EA.TaggedValue` | inside `MapElement()` |
| `EA.DiagramObject` | inside `MapDiagram()` |

**COM property conventions:**
- EA COM property names often differ from C# (`DiagramGUID`, `ElementID`, `PackageID`, `ConnectorID`)
- Check the EA COM API for the exact property name
- Cast with `(EA.Collection)` for child collections
- Use `for (short i = 0; ...)` for COM collection iteration (not `foreach`)
- Handle `DateTime` returns with `.ToString(format)` for string properties

### Layer 3: Exporter (`src/EAxWiki.Export/MarkdownExporter.cs`)

Render the field in the relevant exporter method.

| Output type | Exporter method |
|---|---|
| Package index page | `WritePackageIndexAsync()` |
| Element detail page | `WriteElementAsync()` |
| Diagram page | `WriteDiagramPageAsync()` |
| Root index page | `WriteRootIndexAsync()` |
| Types index | `WriteTypesIndexAsync()` |
| Diagrams index | `WriteDiagramsIndexAsync()` |

## Verification

After all three layers are updated:

1. Build: `dotnet build` on the user's machine
2. Export: `.\scripts\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea"` on the user's machine
3. Inspect the generated wiki output for the new field
4. Export requires EA COM and only runs on Windows

## Common Mistakes

- **Missing COM property**: Not all EA COM properties exist in every schema version. Verify with a SQL query against the EA database first, or wrap in try-catch.
- **Wrong type**: COM properties return `object`, `int`, `string`, or `DateTime`. Cast appropriately.
- **Collection iteration**: Use `GetAt(i)` with `short` counter, not LINQ or `foreach`.
- **Null notes**: `.Notes` can be null; handle in the exporter with `?? ""` or null-conditional access.
- **Schema differences**: Different EA schema versions have different columns. When adding a field like Status, verify the table exists (e.g. `t_diagram.Status` was absent in this project's schema).
