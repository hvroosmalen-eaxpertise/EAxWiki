# Recent Changes Page (Issue #13)

## Problem

With hundreds of elements and diagrams, there is no generated view showing what has been modified recently in the EA model.

## Solution

Generate `recent/index.md` listing elements and diagrams sorted by `ModifiedDate` descending, top 50.

## Architecture

New `GenerateRecentChangesAsync` method in `MarkdownExporter.cs`, called from `ExportAsync` after all elements and diagrams are collected. Follows same pattern as glossary and types page generation.

## Data Sources

All data already collected:
- **Elements**: `EaElement.ModifiedDate` (DateTime)
- **Diagrams**: `EaDiagram.ModifiedDate` (string, parsed via `DateTime.TryParse`)

## Rendering

Merge both into a single list, sorted by `ModifiedDate` descending.

```markdown
# Recent Changes

| Name | Type | Modified | Path |
|------|------|----------|------|
| [Carbon Target](link) | Requirement | 2026-06-18 | Root / Package / Sub |
| [ESRS Report](link) | Diagram | 2026-06-17 | Root / Package |
```

Top 50 items. Items without valid ModifiedDate appear at the bottom with `-`.

### Columns

- **Name**: linked element/diagram page name
- **Type**: element type (e.g. Requirement, Class) or "Diagram"
- **Modified**: `yyyy-MM-dd` or `-`
- **Path**: breadcrumb path (build from `packageLookup`)

## Navigation

Add `- Recent: recent/` to root `.pages`.

## Edge Cases

- Element ModifiedDate is `DateTime.MinValue` → show `-`, sort to bottom
- Diagram ModifiedDate fails `DateTime.TryParse` → show `-`, sort to bottom
- Fewer than 50 items → show all
- No items → show "No recent changes found."
