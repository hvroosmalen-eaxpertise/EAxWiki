EAxWiki — Export EA model to Markdown wiki, served with MkDocs

This repository exports an Enterprise Architect `.qea` model to a `wiki/` folder of Markdown pages, then serves them locally with MkDocs.

> **Note — test data only:** The `wiki/` folder, `model/` folder, and the live site in this repository contain the **EurSuRA** model, which is used exclusively for development and testing of EAxWiki itself. They are not part of the tool and have no relation to any installation. When you use EAxWiki with your own EA model, it will write to a `wiki/` folder in your own repository.

**Live site (test data):** https://hvroosmalen-eaxpertise.github.io/EAxWiki/

## How it works

EAxWiki is a two-step pipeline: **export** turns your EA model into Markdown, **serve** renders it as a website.

### Export

The exporter is a .NET 10 console application that connects to a Sparx Enterprise Architect model (`.qea` file) via COM Interop and writes a `wiki/` folder of Markdown files. Because it uses EA's COM API, it can only run on Windows with EA installed.

What the exporter produces:
- One Markdown page per element, organised into the same package hierarchy as the EA model
- PNG diagram images with a linked Markdown page per diagram (clickable for zoom via glightbox)
- Six cross-cutting index views: **Structure**, **Types**, **Glossary**, **Diagrams**, **Recent Changes**, **Status Dashboard**
- A `model.json` file with the full model serialised as JSON (opt-in via `--json` / `-j`)
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
| Write-back (wiki → EA) | ✓ | ✗ requires EA |

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
| `scripts/writeback.ps1` | Scan wiki for status changes and write them back to EA via COM (**Windows only**) |

All scripts accept both PowerShell (`-Flag`) and Unix-style (`--flag`) syntax interchangeably, e.g. `--force`, `--verbose`, `--repo`.

### Export only

```powershell
.\scripts\export.ps1
.\scripts\export.ps1 --force                   # full regeneration (skip nothing)
.\scripts\export.ps1 --verbose                 # debug-level per-element logging
.\scripts\export.ps1 --repo "path/to/model.qea"
```

`--repo` also accepts a database connection string. If the value contains `=` it is passed directly to EA without any path resolution:

```powershell
# SQL Server (Windows auth)
.\scripts\export.ps1 --repo "DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=MYSERVER;Initial Catalog=EA;Integrated Security=SSPI;"

# SQL Server (SQL auth)
.\scripts\export.ps1 --repo "DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=MYSERVER;Initial Catalog=EA;User Id=sa;Password=secret;"

# MySQL / MariaDB
.\scripts\export.ps1 --repo "DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;"

# Oracle
.\scripts\export.ps1 --repo "DBType=2;Connect=Data Source=TNSNAME;User Id=user;Password=pass;"

# PostgreSQL
.\scripts\export.ps1 --repo "DBType=7;Connect=Server=localhost;Database=EA;User Id=user;Password=pass;"
```

If `--repo` is omitted, an interactive prompt walks through DB type, server, optional port, database, and credentials (password is masked).

### Serve only (wiki already exported)

```powershell
.\scripts\serve.ps1
.\scripts\serve.ps1 --port 8001
```

The serve script creates a `.venv` if needed, installs MkDocs requirements, and starts `mkdocs serve`.

### Export + serve

```powershell
.\scripts\export-and-serve.ps1                                           # incremental export, then serve
.\scripts\export-and-serve.ps1 --force                                   # full regeneration, then serve
.\scripts\export-and-serve.ps1 --verbose --force                         # full regeneration with verbose logging
.\scripts\export-and-serve.ps1 --repo "path/to/model.qea" --port 8000
.\scripts\export-and-serve.ps1 --repo "path/to/model.qea" --port 8000 --api-port 8001   # with live write-back
.\scripts\export-and-serve.ps1 --output "D:\wikis\projectA" --port 8000 --api-port 8001  # custom output dir
```

The export step cleans up any orphaned EA.exe processes when it finishes.

### Live write-back — change status directly from the wiki page

When the wiki runs locally on Windows with EA installed, users can change an element's **Status** directly from the rendered wiki page. A dropdown and **Apply** button appear on every element page that has a status value set.

```
┌────────────────────────────────────────────────────────────┐
│  Browser (MkDocs :8000)                                    │
│                                                            │
│  Status: [ Approved ▼ ]  [ Apply ]                        │
│                │                                           │
│         POST /api/status                                   │
│                ▼                                           │
│  Wiki write-back server (:8001)                            │
│    ├─ EA COM → element.Status = "Approved"                 │
│    └─ Update .md frontmatter + status badge in-place       │
└────────────────────────────────────────────────────────────┘
```

Use `export-and-serve.ps1` with `--api-port` to start everything in one command:

```powershell
.\scripts\export-and-serve.ps1 --repo "model/file.qea" --port 8000 --api-port 8001
.\scripts\export-and-serve.ps1 --repo "model/file.qea" --port 8000 --api-port 8001 --force
```

This exports the wiki (embedding the status-editor widget), starts the write-back server on port 8001 as a background job, and starts MkDocs on port 8000. When Apply is clicked the EA model is updated immediately via COM. MkDocs detects the `.md` change and hot-reloads the page within seconds.

**Batch write-back** (for `.md` edits made while the server was not running):

```powershell
.\scripts\export.ps1 --writeback       # scan wiki/ for status changes, write to EA
.\scripts\export.ps1                   # re-export to sync the wiki
```

**Status options** are read live from `t_statustypes` in the EA model — the dropdown always reflects the current valid set.

> **Linux / Mac:** Write-back is **not supported** on Linux or Mac. The EA COM API requires Sparx Enterprise Architect, which is Windows-only. A wiki served on Linux is read-only.

| Feature | Windows | Linux / Mac |
|---|---|---|
| Export (EA → wiki) | ✓ | ✗ requires EA |
| Serve wiki (MkDocs) | ✓ | ✓ |
| Live status write-back (wiki → EA) | ✓ | ✗ requires EA |
| Batch write-back (`--writeback`) | ✓ | ✗ requires EA |

## Saved connection config

On first run without `--repo`, the interactive prompt saves your connection string to a `.eaxwiki` file in the project root. Subsequent runs load it automatically — no re-entry needed.

```
First run:   interactive prompt → saved to .eaxwiki
Later runs:  loads .eaxwiki automatically
Override:    .\scripts\export.ps1 --repo "other_connection_string"
Reset:       delete .eaxwiki to re-enter interactively
```

> **Security:** `.eaxwiki` contains your credentials in plaintext. It is gitignored and never committed. Keep it on a private or access-controlled machine.

## Scheduling exports

Because the connection is saved in `.eaxwiki`, the scripts run unattended and are suitable for scheduling.

### Windows Task Scheduler

```powershell
$action  = New-ScheduledTaskAction -Execute "pwsh" -Argument "-ExecutionPolicy Bypass -File C:\EAxWiki\scripts\export.ps1"
$trigger = New-ScheduledTaskTrigger -Daily -At "02:00"
Register-ScheduledTask -TaskName "EAxWiki Export" -Action $action -Trigger $trigger -RunLevel Highest
```

### Linux / Mac cron (serve only — export requires Windows)

```cron
# Restart the wiki server daily at 03:00
0 3 * * * pwsh /opt/EAxWiki/scripts/serve.ps1 >> /var/log/eaxwiki.log 2>&1
```

## Changes to the model repository

EAxWiki handles structural changes in EA automatically on the next export run:

| Change in EA | Incremental run | `--force` run |
|---|---|---|
| Edit element content | ✓ updated (ModifiedDate changes) | ✓ updated |
| Move element to another package | ✓ written to new location; old file removed | ✓ |
| Rename package | ✓ new folder created; old folder deleted | ✓ |
| Delete element or package | ✓ file/folder removed | ✓ |

**Note on moved elements:** When you move an element without editing it, EA does not update its `ModifiedDate`. The element page is recreated at the new location with current content. On subsequent incremental runs it will be skipped (no changes) until you next edit the element in EA — which is normal incremental behaviour.

## Incremental vs full export

By default the exporter skips elements and diagrams whose output file is newer than the source's `ModifiedDate` in EA. Pass `-Force` to regenerate everything — useful after template changes or when timestamps are unreliable.

## Wiki navigation

The wiki has six navigation views:

- **Structure** — a top-down tree of packages and their elements, following the EA model hierarchy
- **Types** — elements grouped by modelling language and type (e.g. ArchiMate3 BusinessRole, UML Metric)
- **Diagrams** — an alphabetically sorted global index of all diagrams with modified date and description
- **Glossary** — terms extracted from "Definition"/"Glossary" tagged values and first sentences from element notes
- **Recent** — top 50 most recently modified elements and diagrams, sorted by date descending
- **Status Dashboard** — a dashboard at `/status/` with summary bar charts, a **By Package** table with collapsible drill-down (clickable element links with status badges), and a **By Type** breakdown section

## Element page features

- **Breadcrumb** — hierarchical path from root to the element's package
- **Dates** — shows CreatedDate and ModifiedDate beneath the breadcrumb
- **Stereotype labels** — each element heading is prefixed with a coloured label showing the full stereotype type name. ArchiMate elements use layer colours (Business=Yellow, Application=Blue, Technology=Green, Motivation=Purple, Strategy=Brown, Implementation=Pink). EDGY elements use facet colours. UML elements display in gray.
- **Status badges** — element Status (Proposed, Approved, Implemented, etc.) shown as a coloured badge next to the element title; used throughout element pages, type indices, and the Status Dashboard
- **Relationships** — outgoing connectors with linked target element names
- **Referenced By** — incoming connectors from other elements with links
- **Appears on Diagrams** — inline thumbnail gallery of diagrams containing this element; each thumbnail links to the diagram page
- **Attributes, Methods, Tagged Values** — detailed tabs where present
- **Relationship Graph** — interactive force-directed graph showing the element's 2-hop neighbourhood (all directly connected elements, plus their neighbours). Nodes and edges are coloured by ArchiMate layer or EDGY facet, matching the stereotype label colours used throughout the wiki. The focal element is highlighted in orange. Unreachable 2-hop nodes (not in the export) appear at reduced opacity. Hover a node to see its full name and package in a tooltip. **Single-click** a node to expand it — its own neighbourhood is fetched and merged into the graph live. **Double-click** a node to navigate to its element page. Cross-package relationships are shown.

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
- The `--verbose` / `-v` flag enables per-element debug-level logging during export. When used with a DB connection string it also logs the full (unredacted) connection string sent to EA, which is useful for diagnosing connection errors.
- The `--json` / `-j` flag writes `model.json` alongside the markdown pages with the full model as a machine-readable JSON document.
- The wiki home page title shows the database name (from `Database=` / `Initial Catalog=`) for DB connections, or the filename for `.qea` files. Credentials are never shown in the wiki output.

