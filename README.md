EAxWiki — Export EA model to Markdown wiki, served with MkDocs

This repository exports an Enterprise Architect `.qea` model to a `wiki/` folder of Markdown pages, then serves them locally with MkDocs. The site is also deployed to GitHub Pages on push.

**Live site:** https://hvroosmalen-eaxpertise.github.io/EAxWiki/

## Prerequisites

- **Python 3.x** (for MkDocs)
- **.NET 10 SDK** (for the C# exporter)
- **Enterprise Architect** (for COM-based model reading; Windows only)

## Quick start (MkDocs only)

```powershell
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
.\scripts\serve-mkdocs.ps1 -Port 8000
```

## Full export + serve

```powershell
.\scripts\serve.ps1
```

Or with verbose logging:
```powershell
.\scripts\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea" -Verbose
```

Or including JSON export:
```powershell
.\scripts\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea" -Json
```

Runs the .NET exporter against the `.qea` file, then serves the output. The `-Verbose` flag enables debug-level logging. The `export-and-serve.ps1` script also cleans up any orphaned EA.exe processes after the export finishes.

## Incremental vs full export

- `.\scripts\serve.ps1` — incremental (skip unchanged elements)
- `.\scripts\serve-full.ps1` — full regeneration (use after template changes)

## Scripts

| Script | Purpose |
|--------|---------|
| `scripts/serve.ps1` | Incremental export then MkDocs serve (default) |
| `scripts/serve-full.ps1` | Full regeneration then MkDocs serve |
| `scripts/export-and-serve.ps1` | Full pipeline with flags: `-Verbose`, `-Force`, `-Json`, `-RepoPath`, `-Port` |
| `scripts/serve-mkdocs.ps1` | Serve an already-exported wiki locally (no export) |

## Wiki navigation

The wiki has five navigation views:

- **Structure** — a top-down tree of packages and their elements, following the EA model hierarchy
- **Types** — elements grouped by modelling language and type (e.g. ArchiMate3 BusinessRole, UML Metric)
- **Diagrams** — an alphabetically sorted global index of all diagrams with modified date and description
- **Glossary** — terms extracted from "Definition"/"Glossary" tagged values and first sentences from element notes
- **Recent** — top 50 most recently modified elements and diagrams, sorted by date descending

## Element page features

- **Breadcrumb** — hierarchical path from root to the element's package
- **Dates** — shows CreatedDate and ModifiedDate beneath the breadcrumb
- **Status** — displays element Status (Proposed, Approved, Implemented, etc.) inline with Type and Stereotype
- **Relationships** — outgoing connectors with linked target element names
- **Referenced By** — incoming connectors from other elements with links
- **Appears on Diagrams** — list of diagrams containing this element with links
- **Attributes, Methods, Tagged Values** — detailed tabs where present

## Diagram page features

- **PNG image** — exported directly from EA
- **Interactive zoom** — click the image for a full-size overlay (via mkdocs-glightbox)
- **Elements list** — all elements on the diagram, alphabetically sorted, with links

All views are generated automatically by the exporter and configured via the awesome-pages MkDocs plugin.

## Design decisions

See [docs/design-decisions.md](docs/design-decisions.md) for a full summary of architecture, naming, navigation, export, error handling, and deployment decisions.

## Notes

- The URL shown by `mkdocs serve` (`http://0.0.0.0:8000`) is a listen address and not usable in a browser. Use `http://localhost:8000` or `http://127.0.0.1:8000` instead.
- If the browser can't connect, check Windows Firewall inbound rules for the port (8000 by default).
- The exporter uses EA COM Interop and only runs on Windows. It cannot run in CI/generic build environments.
- Element page export is parallelized for performance. Duplicate sanitized filenames (e.g. `unnamed.md`) are handled with per-file locking.
- View generation (Types, Glossary, Recent Changes, Diagrams index) runs in parallel after the structural export completes, reducing total export time on large models.
- All indexes (element lookup, diagram index, incoming connector index) are built once at the start of export and shared across all phases — no redundant traversals.
- The `--verbose` / `-v` flag enables per-element debug-level logging during export.
- The `--json` / `-j` flag writes `model.json` alongside the markdown pages with the full model as a machine-readable JSON document.

## CI / Deployment

A GitHub Actions workflow (`.github/workflows/mkdocs-deploy.yml`) builds the site and publishes it to the `gh-pages` branch on pushes to `master`. The repo must be public (or have a paid GitHub plan) for Pages to work.


