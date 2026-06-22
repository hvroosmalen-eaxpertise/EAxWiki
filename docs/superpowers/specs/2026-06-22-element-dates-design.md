# Element Dates (Issue #8)

## Problem

Element pages show no timing information. Readers have no way to tell how fresh or stale a given element's definition is.

## Solution

Add `CreatedDate` (DateTime?) to `EaElement`, read from `eaElement.CreatedDate` in `MapElement`, and render both CreatedDate and the existing ModifiedDate on element pages.

## Architecture

3-layer pattern:
- **Model** (`EaElement.cs`): add `CreatedDate` property
- **Reader** (`EaReader.cs`): read COM property in `MapElement`
- **Exporter** (`MarkdownExporter.cs`): render in `WriteElementAsync`

## Model

```csharp
public DateTime? CreatedDate { get; set; }
```

ModifiedDate already exists as `DateTime`.

## Reader

```csharp
CreatedDate = eaElement.CreatedDate as DateTime?
```

## Exporter

Change the metadata block in `WriteElementAsync` from:

```
**Type:** {element.Type}
**Stereotype:** {element.Stereotype}
```

to:

```
**Type:** {element.Type}
**Stereotype:** {element.Stereotype}
**Created:** 2025-03-12  **Modified:** 2026-06-15
```

Only on individual element pages — not on package index or types pages.

## Edge Cases

- `CreatedDate` null → show `-`
- `ModifiedDate == DateTime.MinValue` → show `-`
- Date format: `yyyy-MM-dd`
