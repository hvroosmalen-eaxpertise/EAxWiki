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
.\scripts\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea" -Verbose
```

Runs the .NET exporter against the `.qea` file, then serves the output. The `-Verbose` flag enables debug-level logging. The script also cleans up any orphaned EA.exe processes after the export finishes.

### Incremental vs full export

By default the exporter skips element pages whose `.md` file is newer than the element's last-modified date in EA. This makes repeated exports faster — only changed elements are regenerated.

Use the `-Force` flag when the Markdown template changes (new sections, reordered layout, etc.) to force full regeneration of all files:

```powershell
.\scripts\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea" -Force
```

## Scripts

| Script | Purpose |
|--------|---------|
| `scripts/export-and-serve.ps1` | Full pipeline: export + MkDocs serve, with EA.exe cleanup |
| `scripts/serve-mkdocs.ps1` | Serve an already-exported wiki locally |
| `scripts/export.ps1` | Export only (no server) |

## Wiki navigation

The wiki has three navigation views:

- **Structure** — a top-down tree of packages and their elements, following the EA model hierarchy
- **Types** — elements grouped by stereotype (e.g. Task, Process, Metric, ArchiMate_Goal)
- **Diagrams** — an alphabetically sorted global index of all diagrams with modified date and description

All views are generated automatically by the exporter and configured via the awesome-pages MkDocs plugin.

## Design decisions

See [docs/design-decisions.md](docs/design-decisions.md) for a full summary of architecture, naming, navigation, export, error handling, and deployment decisions.

## Notes

- The URL shown by `mkdocs serve` (`http://0.0.0.0:8000`) is a listen address and not usable in a browser. Use `http://localhost:8000` or `http://127.0.0.1:8000` instead.
- If the browser can't connect, check Windows Firewall inbound rules for the port (8000 by default).
- The exporter uses EA COM Interop and only runs on Windows. It cannot run in CI/generic build environments.
- Element page export is parallelized for performance. Duplicate sanitized filenames (e.g. `unnamed.md`) are handled with per-file locking.
- The `--verbose` / `-v` flag enables per-element debug-level logging during export.

## CI / Deployment

A GitHub Actions workflow (`.github/workflows/mkdocs-deploy.yml`) builds the site and publishes it to the `gh-pages` branch on pushes to `master`. The repo must be public (or have a paid GitHub plan) for Pages to work.


