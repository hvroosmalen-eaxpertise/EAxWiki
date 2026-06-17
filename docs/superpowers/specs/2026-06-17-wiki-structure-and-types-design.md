# Wiki Structure and Types Views

## Summary

Add two navigation views to the EAxWiki MkDocs site: a **Structure** view (the existing package hierarchy, surfaced as the default nav) and a **Types** view (elements grouped by Stereotype, as a second top-level nav section).

## Approach

**Approach B** — Keep the existing export output flat (unchanged), add a `.pages` nav file and generate a `types/` folder with stereotype-grouped index pages. No cross-reference breakage.

## Design

### Navigation (via `.pages`)

A `wiki/.pages` file will define two top-level nav sections:

```yaml
nav:
  - Structure: ''
  - Types: types/
```

- `Structure` shows the root content — the existing package hierarchy (folders + elements), navigable via expandable sidebar.
- `Types` adds a second entry in the sidebar pointing to `types/`.

### Types pages (generated at export time)

After the main package-based export completes, the exporter will generate:

- **`wiki/types/index.md`** — Lists every unique `Stereotype` found across all elements, each linking to its dedicated page.
- **`wiki/types/{Stereotype}.md`** — One file per stereotype, containing a table/list of all elements with that stereotype, linking to their existing element pages (e.g., `../{Package}/{ElementName}.md`).

### Exporter changes (`MarkdownExporter.cs`)

Modify the export pipeline in this order:

1. **First pass** — Recursively walk all packages and collect element metadata (name, stereotype, parent package path). This already happens during the recursive `ExportPackageAsync` traversal.

2. **Stereotype catalog** — After the walk, group all elements by `Stereotype`. Handle empty/missing stereotypes as a generic `"Uncategorized"` group.

3. **Generate types output** — Write `types/index.md` and `types/{Stereotype}.md` files using the `IOutputWriter`.

4. **Generate `.pages` file** — Write `wiki/.pages` with the nav definition above. Only overwrite if it doesn't already exist (or always regenerate — user preference).

### No changes to existing files

Existing element `.md` files remain in their package-based folders with the same names and internal content. No cross-reference paths change.

### Out of scope

- No multi-version/dual-build setup
- No dynamic JS-based tree view on the client side
- No changes to the MkDocs theme or plugins beyond adding the `.pages` file
