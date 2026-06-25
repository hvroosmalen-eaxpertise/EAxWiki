EAxWiki — Export EA model to Markdown wiki, served with MkDocs

This repository exports an Enterprise Architect `.qea` model to a `wiki/` folder of Markdown pages, then serves them locally with MkDocs. The site is also deployed to GitHub Pages on push.

**Live site:** https://hvroosmalen-eaxpertise.github.io/EAxWiki/

## Installation

Installer packages are available on the [GitHub Releases page](https://github.com/hvroosmalen-eaxpertise/EAxWiki/releases/latest) and are updated automatically on every push to master.

### Windows (export + serve)

1. Download **`EAxWiki-windows.zip`** from the [latest release](https://github.com/hvroosmalen-eaxpertise/EAxWiki/releases/latest) and extract it.
2. Open PowerShell 7+ in the extracted folder and run:
```powershell
pwsh ./install.ps1
```

The installer will:
- Check for .NET 10 SDK and Python
- Auto-detect your Sparx EA installation by scanning all drives for `Program Files (x86)\Sparx Systems\EA` and `Program Files\Sparx Systems\EA`
- Build the .NET exporter
- Set up the Python venv and install MkDocs

If EA is installed in a non-standard location, pass the path explicitly:
```powershell
pwsh ./install.ps1 -EAPath "D:\MyTools\Sparx\EA"
```

### Linux / Mac (serve only)

Download **`install.sh`** from the [latest release](https://github.com/hvroosmalen-eaxpertise/EAxWiki/releases/latest) and run:
```bash
bash ./install.sh
```
The script installs PowerShell Core (`pwsh`) if missing (via apt/dnf/Homebrew), then sets up MkDocs.

### How Windows and Linux work together

Sparx Enterprise Architect is Windows-only, so the export step can only run on Windows. The exported output — the `wiki/` folder of Markdown files — is committed to git and shared with Linux machines, which can then serve it with MkDocs.

```
Windows machine                       Linux / Mac machine
─────────────────────                 ──────────────────────────────
1. Open EA model                      1. git pull  (gets latest wiki/)
2. scripts/export.ps1                 2. pwsh scripts/serve.ps1
   └─ writes wiki/ to git ──push──►  3. open http://localhost:8000
3. git push
```

| Step | Windows | Linux / Mac |
|------|---------|-------------|
| Export (EA → Markdown) | ✓ | ✗ requires EA |
| Serve (MkDocs)         | ✓ | ✓ |

Linux only needs Python and PowerShell Core (`pwsh`). The `install.sh` script installs `pwsh` automatically if it is not present.

## Prerequisites

| Prerequisite | Windows | Linux/Mac |
|---|---|---|
| Python 3.x | required | required |
| .NET 10 SDK | required | not needed |
| Enterprise Architect | required for export | not needed |
| PowerShell 7+ (`pwsh`) | recommended | required (`install.sh` installs it) |

## Scripts

| Script | Purpose |
|--------|---------|
| `scripts/export.ps1` | Export EA model to Markdown only |
| `scripts/serve.ps1` | Start MkDocs on an already-exported wiki |
| `scripts/export-and-serve.ps1` | Export then serve (calls the two above) |

### Export only

```powershell
.\scripts\export.ps1
.\scripts\export.ps1 -Force          # full regeneration (skip nothing)
.\scripts\export.ps1 -Verbose        # debug-level per-element logging
.\scripts\export.ps1 -RepoPath "path/to/model.qea"
```

### Serve only (wiki already exported)

```powershell
.\scripts\serve.ps1
.\scripts\serve.ps1 -Port 8001
```

The serve script creates a `.venv` if needed, installs MkDocs requirements, and starts `mkdocs serve`.

### Export + serve

```powershell
.\scripts\export-and-serve.ps1                    # incremental export, then serve
.\scripts\export-and-serve.ps1 -Force             # full regeneration, then serve
.\scripts\export-and-serve.ps1 -Verbose -Force    # full regeneration with verbose logging
.\scripts\export-and-serve.ps1 -RepoPath "path/to/model.qea" -Port 8001
```

The export step cleans up any orphaned EA.exe processes when it finishes.

## Incremental vs full export

By default the exporter skips elements and diagrams whose output file is newer than the source's `ModifiedDate` in EA. Pass `-Force` to regenerate everything — useful after template changes or when timestamps are unreliable.

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


