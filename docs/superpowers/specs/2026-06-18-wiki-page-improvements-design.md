# Wiki Page Improvements

## Summary

Five improvements to generated wiki pages to show repository metadata, navigation context, and diagram state information.

## Changes

### 1. Home page — repository path

**File:** `MarkdownExporter.cs` — `WriteRootIndexAsync`

Add a `## Repository` section to `index.md` below the existing `## Structure` section, showing the path to the `.qea` file:

```markdown
## Repository

{connectionString}
```

**Implementation:**
- Add `string RepositoryPath { get; }` to `IEaReader` interface
- Implement in `EaReader` returning `repository.ConnectionString`
- Pass `_reader.RepositoryPath` to `WriteRootIndexAsync`
- If reader is null (headless mode), skip the section

### 2. Generation date footer

**Files:** `MarkdownExporter.cs` — `WriteRootIndexAsync`, `ExportPackageAsync`, `WriteElementAsync`, `ExportDiagramsAsync`, `GenerateTypesPagesAsync`

Append a footer to every generated markdown page:

```markdown
---

*Generated: YYYY-MM-dd HH:mm:ss*
```

**Implementation:**
- Add private helper `string FormatTimestamp() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")`
- Append to the `lines` list in each method before writing
- Use local time since this is a local export tool

### 3. Namespace breadcrumb

**File:** `MarkdownExporter.cs`

Add a clickable breadcrumb on every page (except root `index.md`) right after the H1 heading:

```markdown
EurSuRA / [Organisation](/Organisation/index.md) / [Departments](/Organisation/Departments/index.md)
```

**Implementation:**
- During `ExportAsync`, build `Dictionary<int, (string Name, int? ParentId)>` by walking all packages recursively
- Add a helper `string BuildBreadcrumb(int packageId, string outputDir)` that resolves the parent chain and generates markdown links
- Each segment links to the corresponding package's `index.md` using a relative path from the current page's directory
- Call the helper in:
  - `ExportPackageAsync` — breadcrumb for the package itself
  - `WriteElementAsync` — breadcrumb for the element's containing package
  - `ExportDiagramsAsync` — breadcrumb for the diagram's containing package
  - GenerateTypesPagesAsync — breadcrumb omitted (types pages are not packages)
- Root index page gets no breadcrumb

### 4. Package pages: Diagrams before Elements

**File:** `MarkdownExporter.cs` — `ExportPackageAsync`

Move the `## Diagrams` code block (lines 116-134) above the `## Elements` code block (lines 90-114). No logic changes, just reorder.

### 5. Diagram pages: description + status

**Files:**
- `EaDiagram.cs` — add `Status` property
- `EaReader.cs` — map `eaDiagram.Status`
- `MarkdownExporter.cs` — `ExportDiagramsAsync`

**Model change:**
Add `public string? Status { get; set; }` to `EaDiagram`.

**Reader change:**
Add `Status = eaDiagram.Status` in `MapDiagram`.

**Rendering change:**
Always render in diagram pages:

```markdown
**Description:** {Notes ?? "-"}
**Status:** {Status ?? "-"}
```

The description label is always shown. If `Notes` is null/empty, render `-`. If `Status` is null/empty, render `-`.

## Files Modified

| File | Changes |
|------|---------|
| `src/EAxWiki.Core/Interfaces/IEaReader.cs` | Add `string RepositoryPath` property |
| `src/EAxWiki.Core/Models/EaDiagram.cs` | Add `string? Status` property |
| `src/EAxWiki.EA/EaReader.cs` | Map `RepositoryPath` and `eaDiagram.Status` |
| `src/EAxWiki.Export/MarkdownExporter.cs` | All 5 items: repo path, footer, breadcrumb, reorder, description+status |
| `src/EAxWiki/Program.cs` | May need minor wiring if constructor signature changes |

## Testing

- Re-run export on user's machine (EA COM required)
- Verify `wiki/index.md` shows repository path
- Verify all pages have generation date footer
- Verify package pages show breadcrumb with correct links
- Verify package pages show Diagrams section before Elements
- Verify diagram pages show Description and Status
