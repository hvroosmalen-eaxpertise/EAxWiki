# Element Status (Issue #9)

## Problem

EA tracks a `Status` field on every element (Proposed, Approved, Implemented, Mandatory, etc.) but this is not exported. This is valuable context for governance and maturity tracking.

## Solution

Add `Status` (string) to `EaElement`, read from `eaElement.Status` in `MapElement`, and display on element pages inline with Type and Stereotype.

## Architecture

3-layer pattern:
- **Model** (`EaElement.cs`): add `Status` property
- **Reader** (`EaReader.cs`): read COM property in `MapElement`
- **Exporter** (`MarkdownExporter.cs`): render in `WriteElementAsync`

## Model

```csharp
public string Status { get; set; } = string.Empty;
```

## Reader

```csharp
Status = eaElement.Status ?? string.Empty
```

## Exporter

Change the metadata block in `WriteElementAsync` from:

```
**Type:** {element.Type}
**Stereotype:** {element.Stereotype}
```

to:

```
**Type:** {element.Type}  **Stereotype:** {element.Stereotype}  **Status:** Approved
```

(All three on one markdown line, separated by two spaces for markdown line break)

Only on individual element pages — not on package index or types pages.

## Edge Cases

- Status null/empty → omit the field entirely (don't show `-`)
- EA enum values: Proposed, Approved, Implemented, Mandatory, Validated
