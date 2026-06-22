# Connector Source/Target Names Instead of Numeric IDs

## Problem

Element pages currently render connector relationships using raw EA element IDs (e.g., `125 → 145`), which are meaningless to human readers.

## Solution

Replace the numeric Source/Target IDs with the connected element's name as a clickable link to that element's page. Use the accumulated `elements` list to build a lookup dictionary at write time.

## Data Flow

### Before

```
ExportPackageAsync → WriteElementAsync(element, dir, outputDir, packageLookup)
    connector rendering: {conn.SourceId} → {conn.TargetId}
elements.Add((elem, dir))  ← after write
```

### After

```
ExportPackageAsync → WriteElementAsync(element, dir, outputDir, elements, packageLookup)
    build Dict<elementId, (Name, PackageDir)> from elements
    connector rendering: [Other Element Name](relative-page-path)
elements.Add((elem, dir))  ← before write
```

### Specific changes

1. **`ExportPackageAsync`** (`MarkdownExporter.cs:76`):
   - Move `elements.Add((elem, dir))` to execute before `WriteElementAsync`
   - Pass `elements` as new parameter to `WriteElementAsync`

2. **`WriteElementAsync`** (`MarkdownExporter.cs:424`):
   - Add `List<(EaElement Element, string PackageDir)> elements` parameter
   - Build `Dictionary<int, (string Name, string PackageDir)>` from `elements` once
   - Replace numeric ID rendering with lookup-based name + link

## Connector Rendering Logic

Replace the current `| Type | Stereotype | SourceId → TargetId |` table with:

```
| Type | Stereotype | Connected To |
|------|------------|-------------|
```

For each connector on the current element:

1. Determine the "other" side element ID:
   - `conn.SourceId == currentElement.Id` → other = `conn.TargetId`
   - `conn.TargetId == currentElement.Id` → other = `conn.SourceId`
   - Neither matches → skip (defensive)

2. Look up the other ID in the element dictionary:
   - **Found**: render `[Element Name](../relative/path.md)` where the relative path is computed via `Path.GetRelativePath(currentDir, otherElemPagePath)`
   - **Not found**: render plain text `Element Name (not in export)`

## Edge Cases

| Case | Behavior |
|------|----------|
| Self-referencing connector (Source == Target == current) | Link to the current element's own page |
| Cross-package connector to unprocessed package | Plain text with "(not in export)" suffix |
| Orphaned connector (ID not in EA model) | Plain text with "(not in export)" suffix |
| Self-referencing where other == current | Same as current page link (no navigation change) |

## Files Changed

- `src/EAxWiki.Export/MarkdownExporter.cs` — move `elements.Add`, add `elements` param, rewrite connector rendering

## Out of Scope

- Connector direction arrows or visual direction indicators
- Bidirectional connector listing
- Filtering/sorting the connector table
- Adding connector direction metadata to `EaConnector` model
