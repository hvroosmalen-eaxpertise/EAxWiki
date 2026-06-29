# Status Dashboard v2 — Charts, Drill-Down, Type Filter

## Problem

The current Status Dashboard (`wiki/status/index.md`) is a flat table — one status column ("Proposed"), 34 package rows, and a note that 181 elements have no status. It shows *what* exists but makes it hard to grasp relative distribution or find individual elements by status.

## Goals

1. **Visual distribution** — proportional horizontal bar charts so the relative volume per status is immediately visible.
2. **Inline drill-down** — clicking a count reveals the list of element names (with links) on the same page, no extra files.
3. **Type breakdown** — add a "By Type" section showing status distribution per stereotype type, same structure as the package breakdown.

## Design

### Summary section (CSS bars)

Each status value gets a colored horizontal bar whose width is proportional to its count relative to the biggest status:

```
<div class="status-bar-container">
  <div class="status-bar status-proposed" style="width: 100%">
    Proposed 271
  </div>
</div>
```

- `.status-bar-container`: full-width, 8px border-radius, overflow hidden, ~1.8em height
- `.status-bar`: colored fill using the same existing `.status-proposed` etc. CSS classes (covers background)
- If no status elements exist, shown as a note below the bars (unchanged from current)
- Max-width bar = 100% (the status with highest count); others scale proportionally
- Multiple status bars stacked vertically

### By Package section (table + collapsible drill-down)

The current package table is preserved with one change — each count becomes a **same-page anchor link**:

```
| Package | Proposed | Total |
|---------|----------|-------|
| [Asset](..) | [6](#pkg-Asset-Proposed) | **6** |
```

Below the table, one `<details>` block per (package, status) combination that has elements:

```
<details id="pkg-Asset-Proposed">
  <summary>Asset — Proposed (6)</summary>

  - [Element A](../Asset/ElementA.md) <span class="status-badge status-proposed">Proposed</span>
  - [Element B](../Asset/ElementB.md) <span class="status-badge status-proposed">Proposed</span>
  ...
</details>
```

- Anchor IDs follow the pattern `pkg-{SanitizedPackageName}-{Status}`
- Sanitization: lowercase, non-alphanumeric → `_` (same `SanitizeName` used for files)
- The `<details>` content is a bullet list of `[Name](link)` with a status badge inline
- Elements with no status do NOT get a details block (they're already excluded from the table counts)
- Collapsed by default — the summary shows package name, status, and count

### By Type section (new table + collapsible drill-down)

A second section with identical structure but grouped by **stereotype type** (e.g. BusinessActor, ApplicationComponent, Purpose, Capability, DataObject, etc.):

```
## By Type

| Type | Proposed | Total |
|------|----------|-------|
| [Purpose](#type-Purpose-Proposed) | 2 | **2** |
| [Capability](#type-Capability-Proposed) | 1 | **1** |
...
```

Below the table, `<details>` blocks keyed by type instead of package:

```
<details id="type-Purpose-Proposed">
  <summary>Purpose — Proposed (2)</summary>

  - [Element A](../pkg/ElementA.md)
  ...
</details>
```

- Anchor IDs follow the pattern `type-{SanitizedTypeName}-{Status}`
- Type name comes from `ParseStereotype(stereotype).Type` (e.g. "BusinessActor", not the full `ArchiMate_BusinessActor`)
- Sorting: by type name alphabetically

### Elements with no status

The dashboard currently counts and reports elements with no status. This behavior is unchanged — the "X elements have no status set" note remains below the summary bars.

### New CSS (added to extra.css by InfrastructureWriter)

```css
/* Bar chart */
.status-bar-container {
  margin: 0.5em 0;
  border-radius: 4px;
  overflow: hidden;
}
.status-bar {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0.15em 0.6em;
  font-size: 0.85em;
  font-weight: 600;
  color: #fff;
  white-space: nowrap;
  border-radius: 4px;
  min-width: fit-content;
  margin-bottom: 0.25em;
}

/* Details/summary for drill-down */
details.status-details {
  margin: 0.5em 0;
  padding: 0.25em 0.75em;
  border: 1px solid #e0e0e0;
  border-radius: 4px;
  background: #fafafa;
}
details.status-details summary {
  cursor: pointer;
  font-weight: 600;
  padding: 0.25em 0;
}
details.status-details[open] summary {
  margin-bottom: 0.25em;
}
```

### Exporter changes (`StatusDashboardExporter.cs`)

The exporter already iterates `ctx.Elements` to build counts. The new version needs to also:

1. **Track element lists** for each (package, status) and (type, status) combination:
   - `Dictionary<(string PackageName, string Status), List<(string Name, string Link)>>` for drill-down
   - Same for types: `Dictionary<(string TypeName, string Status), List<(string Name, string Link)>>`

2. **Generate the Summary bars** section with proportional widths (needs max count first)

3. **Generate anchored `<details>` blocks** below each table

4. No new files or output directories — everything in `wiki/status/index.md`

The `PackageRow` record may need to be extended or replaced with a richer structure that holds both counts and element lists.

### No change to other files

- `MarkdownExporter.cs`: unchanged (already calls `StatusDashboardExporter`)
- `InfrastructureWriter.cs`: only CSS additions
- `ElementPageWriter.cs`: unchanged
- `ExportContext.cs`: unchanged
- Nav `.pages` entry is already in place

## Files touched

| File | Change |
|------|--------|
| `src/EAxWiki.Export/Exporters/StatusDashboardExporter.cs` | Add bar charts, drill-down tracking, `<details>` blocks, By Type section |
| `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` | Add `.status-bar-*` and `details.status-details` CSS to `extra.css` |

## Out of scope

- Interactive filtering (requires JS) — all filtering is pre-generated
- Separate drill-down pages — everything inline on the dashboard page
- Mermaid or other charting libraries — CSS bars only
- Diagram status — Diagrams table does not have a Status column in this EA schema
- History/trend data — not stored in the EA model
