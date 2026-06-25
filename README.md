EAxWiki — Export EA model to Markdown wiki, served with MkDocs

This repository exports an Enterprise Architect `.qea` model to a `wiki/` folder of Markdown pages, then serves them locally with MkDocs. The site is also deployed to GitHub Pages on push.

> **Note — test data only:** The `wiki/` folder, `model/` folder, and the live site in this repository contain the **EurSuRA** model, which is used exclusively for development and testing of EAxWiki itself. They are not part of the tool and have no relation to any installation. When you use EAxWiki with your own EA model, it will write to a `wiki/` folder in your own repository.

**Live site (test data):** https://hvroosmalen-eaxpertise.github.io/EAxWiki/

## How it works

EAxWiki is a two-step pipeline: **export** turns your EA model into Markdown, **serve** renders it as a website.

### Export

The exporter is a .NET 10 console application that connects to a Sparx Enterprise Architect model (`.qea` file) via COM Interop and writes a `wiki/` folder of Markdown files. Because it uses EA's COM API, it can only run on Windows with EA installed.

What the exporter produces:
- One Markdown page per element, organised into the same package hierarchy as the EA model
- PNG diagram images with a linked Markdown page per diagram
- Five cross-cutting index views: **Structure**, **Types**, **Glossary**, **Diagrams**, **Recent Changes**
- An `extra.css` and `.pages` navigation file for MkDocs

The exporter runs **incrementally** by default — it compares each element's `ModifiedDate` in EA against the file's last-write time and skips anything that has not changed. Pass `-Force` to regenerate everything.

### Serve

The serve step runs [MkDocs](https://www.mkdocs.org/) with the [Material theme](https://squidfunk.github.io/mkdocs-material/) against the `wiki/` folder produced by the exporter. MkDocs renders the Markdown into a fully navigable website and serves it locally on a port of your choice.

Because the serve step only needs Python and the `wiki/` folder, it works on any platform — including Linux and Mac. The `wiki/` folder just needs to be accessible on the serving machine, whether via a shared filesystem, git, or any other means.

### Windows — export and serve on the same machine

```
┌─────────────────────────────┐   ┌─────────────────────────────┐
│           EXPORT            │   │            SERVE            │
│                             │   │                             │
│  EA Model (.qea / DB)       │   │  wiki/                      │
│  Sparx Enterprise Architect │   │  Markdown + PNG files       │
│             │               │   │             │               │
│          COM API            │   │             ▼               │
│             │               │   │  MkDocs + Material          │
│             ▼               │   │  scripts/serve.ps1          │
│  EAxWiki.exe (.NET 10)      │   │             │               │
│  scripts/export.ps1         │   │             ▼               │
│             │               │   │  Browser                    │
│             ▼               │   │  http://localhost:8000      │
│  wiki/                      ├──►│                             │
│  Markdown + PNG files       │   │                             │
│                             │   │                             │
│  Incremental by default;    │   │  export-and-serve.ps1       │
│  use -Force to rebuild all  │   │  runs both steps at once    │
└─────────────────────────────┘   └─────────────────────────────┘
         Windows only                    Windows, Linux, Mac
```

### Windows + Linux — export on Windows, serve on Linux

```
┌──────────────────────────────────┐   ┌──────────────────────────────────┐
│         WINDOWS MACHINE          │   │         LINUX / MAC MACHINE      │
│                                  │   │                                  │
│  EA Model (.qea / DB)            │   │                                  │
│           │                      │   │                                  │
│        COM API                   │   │                                  │
│           │                      │   │                                  │
│           ▼                      │   │  wiki/                           │
│  EAxWiki.exe                     │   │  Markdown + PNG files            │
│  scripts/export.ps1              │   │           │                      │
│           │                      │   │           ▼                      │
│           ▼                      │   │  MkDocs + Material               │
│  wiki/  ──── shared filesystem ──┼──►│  scripts/serve.ps1               │
│           (or git, rsync, etc.)  │   │           │                      │
│                                  │   │           ▼                      │
│                                  │   │  Browser                         │
│                                  │   │  http://localhost:8000           │
└──────────────────────────────────┘   └──────────────────────────────────┘
```

## Installation

Installer packages are available on the [GitHub Releases page](https://github.com/hvroosmalen-eaxpertise/EAxWiki/releases/latest) and are updated automatically on every push to master.

### Windows (export + serve)

1. Download **`EAxWiki-windows.zip`** from the [latest release](https://github.com/hvroosmalen-eaxpertise/EAxWiki/releases/latest) and extract it.
2. Open PowerShell 7+ in the extracted folder and run:
```powershell
pwsh -ExecutionPolicy Bypass -File .\install.ps1
```

> **Note — execution policy:** Windows blocks unsigned scripts downloaded from the internet by default. The `-ExecutionPolicy Bypass` flag above overrides this for the installer only and does not change your system policy permanently. Alternatively, unblock the files first and then run normally:
> ```powershell
> Unblock-File -Path .\install.ps1
> Unblock-File -Path .\scripts\*.ps1
> pwsh .\install.ps1
> ```

The installer will:
- Check for .NET 10 SDK and Python
- Auto-detect your Sparx EA installation by scanning all drives for `Program Files (x86)\Sparx Systems\EA` and `Program Files\Sparx Systems\EA`
- Build the .NET exporter
- Set up the Python venv and install MkDocs

If EA is installed in a non-standard location, pass the path explicitly:
```powershell
pwsh -ExecutionPolicy Bypass -File .\install.ps1 -EAPath "D:\MyTools\Sparx\EA"
```

### Linux / Mac (serve only)

Download **`install.sh`** from the [latest release](https://github.com/hvroosmalen-eaxpertise/EAxWiki/releases/latest) and run:
```bash
bash ./install.sh
```
The script installs PowerShell Core (`pwsh`) if missing (via apt/dnf/Homebrew), then sets up MkDocs.

### How Windows and Linux work together

Sparx Enterprise Architect is Windows-only, so the export step can only run on Windows. The exported output — the `wiki/` folder of Markdown files — needs to be accessible on the machine running MkDocs. How you share it is up to you:

- **Shared filesystem** (NAS, network drive, mapped drive) — point `--output` at a shared path; the Linux machine reads the same folder directly.
- **Git** — commit `wiki/` and push; the Linux machine pulls to get the latest.
- **Any other file sync** (rsync, SCP, Dropbox, OneDrive, etc.)

```
Windows machine                         Linux / Mac machine
──────────────────────                  ──────────────────────────────
1. Open EA model
2. scripts/export.ps1
   └─ writes wiki/ ──── shared path ──► pwsh scripts/serve.ps1
                       (or sync/push)   └─ http://localhost:8000
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
.\scripts\export.ps1 -Force                    # full regeneration (skip nothing)
.\scripts\export.ps1 -Verbose                  # debug-level per-element logging
.\scripts\export.ps1 -RepoPath "path/to/model.qea"
```

`-RepoPath` also accepts a database connection string. If the value contains `=` it is passed directly to EA without any path resolution:

```powershell
# SQL Server (Windows auth)
.\scripts\export.ps1 -RepoPath "DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=MYSERVER;Initial Catalog=EA;Integrated Security=SSPI;"

# SQL Server (SQL auth)
.\scripts\export.ps1 -RepoPath "DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=MYSERVER;Initial Catalog=EA;User Id=sa;Password=secret;"

# MySQL / MariaDB
.\scripts\export.ps1 -RepoPath "DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;"

# Oracle
.\scripts\export.ps1 -RepoPath "DBType=2;Connect=Data Source=TNSNAME;User Id=user;Password=pass;"

# PostgreSQL
.\scripts\export.ps1 -RepoPath "DBType=7;Connect=Server=localhost;Database=EA;User Id=user;Password=pass;"
```

If `--repo` is omitted when running the app directly, an interactive prompt walks through DB type, server, database, and credentials.

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


