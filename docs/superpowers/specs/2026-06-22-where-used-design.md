# Where Used Cross-References (Issue #11)

## Problem

Given an element, there is no way to see which diagrams it appears on or which other elements reference it.

## Solution

Add a "Where Used" section to element pages showing:
1. **Appears on diagrams** — list of diagrams containing this element
2. **Referenced By** — elements that have a connector pointing to this element

The existing "Relationships" table (from Issue #15) showing outgoing connectors is kept unchanged.

## Architecture

Two reverse indices built once in `ExportAsync` after element collection, before any page writing. Passed to `WriteElementAsync` as new parameters.

### Index 1: Diagram Lookup

`Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>>`

Build by walking the package tree via `CollectDiagrams` / `CollectDiagramsRecursive`. For each diagram's `DiagramObjects`, map `ElementId` → (diagram, packageDir) tuple. Multiple diagrams can contain the same element, so each element ID maps to a list.

### Index 2: Incoming Connector Lookup

`Dictionary<int, List<(EaConnector Connector, int SourceId)>>`

Build by scanning every element's `Connectors` list across all packages. For each connector, map `TargetId` → (connector, sourceId). Multiple incoming connectors can reference the same element.

Note: `SourceId` is stored (not the source element object) because the element may not be in the export set. Rendering resolves via the existing `elements` lookup.

## Page Rendering

All three sections live under a single `## Relationships` heading:

```
## Relationships

### Appears on Diagrams
- [Metrics Overview](diagrams/Metrics Overview.md)
- [ESRS Data Model](diagrams/ESRS Data Model.md)

### References
| Type | Stereotype | Connected To |
|------|------------|-------------|
| ...existing outgoing connector table... |

### Referenced By
| Type | Stereotype | Source |
|------|------------|--------|
| Association | | [Governance Model](...) |
```

Individual sections are omitted if there is no data for that category.

### Diagram Links

Link format: `{diagramPkgDir}/diagrams/{SanitizeName(diagram.Name)}.md`, relative to the element's directory via `Path.GetRelativePath`.

### Incoming Connector Links

Same pattern as existing outgoing connector rendering: resolve `SourceId` via the elements lookup, link to the source element's page. If the source element is not in the export set, show `Element ID {id} (not in export)`.

## Edge Cases

- Element appears on no diagrams → omit "Appears on Diagrams" section entirely
- Element has no incoming connectors → omit "Referenced By" section entirely
- Incoming connector source not in export → show name as plain text fallback
- Diagram not in export (should not happen — diagrams are collected from same walk) → skip silently
