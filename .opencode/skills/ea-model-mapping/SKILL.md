---
name: ea-model-mapping
description: Use when mapping EA COM collection types (Elements, Diagrams, Packages, Attributes, Methods, TaggedValues, Connectors, DiagramObjects) to C# model objects in EaReader
---

# EA Model Mapping

## Overview

`EaReader.MapElement()`, `MapDiagram()`, and `MapPackage()` each follow the same pattern for child collections: cast to `EA.Collection`, iterate with `for (short)`, cast `GetAt(i)` to the target type, map to a C# model.

## Pattern

```csharp
var items = (EA.Collection)eaParent.SomeCollection;
for (short i = 0; i < items.Count; i++)
{
    var eaItem = (EA.TargetType)items.GetAt(i);
    parent.Items.Add(new MappedType
    {
        PropA = eaItem.SomeProp,
        PropB = eaItem.OtherProp,
    });
}
```

## COM Type to C# Model Mapping

| `EA.Collection` of `...` | C# Model | Added to | Location in EaReader |
|---|---|---|---|
| `EA.Element` | `EaElement` | `pkg.Elements` | `MapPackage()` line ~60 |
| `EA.Diagram` | `EaDiagram` | `pkg.Diagrams` | `MapPackage()` line ~66 |
| `EA.Package` | `EaPackage` (recursive) | `pkg.Children` | `MapPackage()` line ~72 |
| `EA.Attribute` | `EaAttribute` | `elem.Attributes` | `MapElement()` line ~92 |
| `EA.Method` | `EaMethod` | `elem.Methods` | `MapElement()` line ~105 |
| `EA.TaggedValue` | `EaTaggedValue` | `elem.TaggedValues` | `MapElement()` line ~118 |
| `EA.Connector` | `EaConnector` | `elem.Connectors` | `MapElement()` line ~130 |
| `EA.DiagramObject` | `EaDiagramObject` | `diagram.DiagramObjects` | `MapDiagram()` line ~162 |

## Variable Name Convention

| COM type | Variable name |
|---|---|
| `EA.Element` | `eaElement` / `eaElem` |
| `EA.Diagram` | `eaDiagram` |
| `EA.Package` | `eaPkg` |
| `EA.Attribute` | `eaAttr` |
| `EA.Method` | `eaMethod` |
| `EA.TaggedValue` | `eaTv` |
| `EA.Connector` | `eaConn` |
| `EA.DiagramObject` | `eaDO` |

Mapping functions use prefix `Map` + type name: `MapElement`, `MapDiagram`, `MapPackage`.

## Common Mistakes

- **`foreach` doesn't work** — COM collections only support `GetAt(i)` with integer index. Use a `for` loop.
- **`int` vs `short`** — COM collection indexers expect `short`. Declare with `short i = 0`.
- **Zero-based** — COM collections are 0-indexed despite some EA dialogs showing 1-based.
- **Missing collection** — Some EA objects have optional collections (e.g., `EA.Connector` without a standard child collection pattern). Verify the COM API.
- **Property casing** — EA COM properties are PascalCase but occasionally diverge from C# conventions. Use the Object Browser in EA or check the EA automation docs.
