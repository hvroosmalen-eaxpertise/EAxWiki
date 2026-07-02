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
┌─────────────────────────────┐   ┌─────────────────────────────┐   ┌─────────────────────────────┐
│           EXPORT            │   │            SERVE            │   │         WRITE-BACK          │
│                             │   │                             │   │                             │
│  EA Model (.qea / DB)       │   │  wiki/                      │   │  Wiki write-back server     │
│  Sparx Enterprise Architect │   │  Markdown + PNG files       │   │  EAxWiki.exe --api          │
│             │               │   │             │               │   │  scripts/serve-api.ps1      │
│          COM API            │   │             ▼               │   │                             │
│             │               │   │  MkDocs + Material          │   │  http://localhost:8001      │
│             ▼               │   │  scripts/serve.ps1          │   │                             │
│  EAxWiki.exe (.NET 10)      │   │             │               │   │  Browser: click pencil      │
│  scripts/export.ps1         │   │             ▼               │   │  POST /api/status, ...      │
│             │               │   │  Browser                    │   │                             │
│             ▼               │   │  http://localhost:8000      ├──►│  EA COM Update() +          │
│  wiki/                      ├──►│                             │   │  patch wiki/ frontmatter    │
│  Markdown + PNG files       │   │                             │   │                             │
│                             │   │                             │   │  export-and-serve.ps1       │
│  Incremental by default;    │   │  export-and-serve.ps1       │   │  --api-port starts this     │
│  use -Force to rebuild all  │   │  runs both steps at once    │   │                             │
└─────────────────────────────┘   └─────────────────────────────┘   └─────────────────────────────┘
                        All three run together on one Windows machine
```

MkDocs itself is cross-platform — only EXPORT and WRITE-BACK need EA COM, which is Windows-only. See the next diagram for running SERVE on a separate Linux/Mac machine instead.

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

> **This installs the tool, not a copy of the demo wiki.** Neither `EAxWiki-windows.zip` nor `EAxWiki-linux.zip` contains the `wiki/` or `model/` folders from this repository — those exist here purely so you can see what EAxWiki produces before installing. Once installed, run the exporter against your own `.qea` file or database connection (see [Configuration](#configuration)) and it writes a fresh `wiki/` folder next to wherever you run it, containing *your* elements, diagrams, and packages. The EurSuRA content you see in this repo and on the [live demo site](https://hvroosmalen-eaxpertise.github.io/EAxWiki/) has no bearing on what you'll get.

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

## Configuration

Every setting is passed as a command-line flag — there is no separate config file to edit, except the auto-saved `.eaxwiki` connection string (see [Saved connection config](#saved-connection-config)). All scripts accept both PowerShell (`-Flag`) and Unix-style (`--flag`) syntax, and forward unrecognized flags straight to the underlying `dotnet run -- ...` invocation.

A typical first-time setup is just:

```powershell
.\scripts\export-and-serve.ps1 --repo "path\to\YourModel.qea"
```

— which exports incrementally to `wiki/`, then serves it at `http://localhost:8000`. No `--repo`? You'll get an interactive prompt (file path or DB connection wizard) the first time, and the answer is saved to `.eaxwiki` so every later run just works with no flags at all.

| Flag | Short | Applies to | Description |
|---|---|---|---|
| `--repo <value>` | `-r` | export | Path to a `.qea` file, or a DB connection string. Omit to enter the interactive connection builder on first run. |
| `--name <name>` | `-n` | export | Display name for the repository, shown on the wiki home page. |
| `--output <dir>` | `-o` | export, serve | Output/input directory for the wiki (default: `wiki`). Give every instance its own to run several side by side — see [Running multiple wikis on one machine](#running-multiple-wikis-on-one-machine). |
| `--package <name>` | `-p` | export | Only export a specific package (by name), instead of the whole repository. |
| `--force` | `-f` | export | Full regeneration — rebuild every file instead of only changed ones. |
| `--verbose` | `-v` | export | Debug-level logging with per-element timing. |
| `--json` | `-j` | export | Also write `model.json` (the full model as JSON) alongside the Markdown. |
| `--writeback` | `-w` | export | Batch mode: scan `wiki/` for manual status/notes edits made while `--api` wasn't running, and push them to EA via COM before exporting. |
| `--api` | | write-back server | Start the wiki write-back server so the pencil-icon editors on the page work live. |
| `--api-port <port>` | | write-back server | Port the write-back server listens on (default: `8001`). |
| `--wiki-port <port>` | | write-back server | Port the *paired* `mkdocs serve` uses (default: `8000`). The write-back server only accepts requests whose `Origin` matches this port — see [Running multiple wikis on one machine](#running-multiple-wikis-on-one-machine). `export-and-serve.ps1` / `serve-api.ps1` set this automatically from `--port`; only pass it yourself if you call `dotnet run` directly. |
| `--port <port>` | `-p`, but only in `serve.ps1`/`export-and-serve.ps1`/`serve-api.ps1` | serve | Port `mkdocs serve` listens on (default: `8000`). Careful: this `-p` is those scripts' own shorthand — `-p` passed to the exporter itself (`export.ps1`) means `--package`, not port. |
| `--help` | `-h` | any | Show usage and exit. |

`--port` and `--api-port`/`--wiki-port` are independent of each other by design — one is MkDocs' own flag, the others belong to the EAxWiki write-back server — which is exactly what makes running several isolated instances on one machine possible.

## Scripts

| Script | Purpose |
|--------|---------|
| `scripts/export.ps1` | Export EA model to Markdown only |
| `scripts/serve.ps1` | Start MkDocs on an already-exported wiki |
| `scripts/export-and-serve.ps1` | Export then serve (calls the two above) |
| `scripts/writeback.ps1` | Scan wiki for status and notes changes and write them back to EA via COM (**Windows only**) |

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

### Live write-back — change status and notes directly from the wiki page

When the wiki runs locally on Windows with EA installed, users can edit an element's **Status** and **Notes**, and a diagram's **Description**, directly from the rendered wiki page — no need to open EA. All three use the same two-step pattern: a small pencil icon next to the value, click to edit, **Apply**/**Save** or **Cancel** to close.

- **Status** — sits on its own line. Clicking the pencil replaces the badge in place with a dropdown, Apply, and Cancel — no separate widget block elsewhere on the page. Elements with no status set show a "Not Set" badge and can be given one the same way.
- **Notes** — a pencil icon next to the notes text. Clicking it swaps the rendered notes for a raw-HTML `<textarea>` with Save / Cancel. Elements with no notes yet show "No description set." and can be given one the same way — the editor isn't gated on notes already existing.
- **Diagram description** — same pencil-and-textarea editor as Notes, on the diagram page. If the diagram has no description of its own, the page shows one auto-derived from an element on the diagram (marked "(derived)"); editing pre-fills with that clean text (no label) so Save just promotes it into the diagram's own stored description. Diagrams with neither show "No description set." and start from a blank box.
- **Attribute, method, and tagged value descriptions** — the same pencil pattern, one per row. For Attributes and Tagged Values (narrow table columns), clicking the pencil expands a full-width row below it for the textarea and Save/Cancel, instead of cramming them into the Description cell. Method descriptions have room to spare already, so the textarea swaps in inline where the description text was. Opening one editor closes any other that's open, anywhere on the page.

```
┌────────────────────────────────────────────────────────────┐
│  Browser (MkDocs :8000)                                    │
│                                                            │
│  Status: Approved [✎]                                     │
│  Notes:  Lorem ipsum...                            [✎]    │
│                │                                           │
│      POST /api/status, /api/notes,                          │
│      /api/diagram-notes, or /api/row-notes                  │
│                ▼                                           │
│  Wiki write-back server (:8001)                            │
│    ├─ EA COM → element.Status / .Notes = "..."              │
│    └─ Update .md frontmatter + page body in-place            │
└────────────────────────────────────────────────────────────┘
```

> **A caveat on attribute/method/tagged value editing:** EA's COM API exposes no ID for these child objects (unlike elements and diagrams), so write-back finds the right one within its parent element by matching name plus its other fields (type for attributes, return type and static-ness for methods, value for tagged values). EA does allow duplicate names — if a composite match is still ambiguous, the first match is used and a warning is logged, rather than the edit failing outright.

Use `export-and-serve.ps1` with `--api-port` to start everything in one command:

```powershell
.\scripts\export-and-serve.ps1 --repo "model/file.qea" --port 8000 --api-port 8001
.\scripts\export-and-serve.ps1 --repo "model/file.qea" --port 8000 --api-port 8001 --force
```

This exports the wiki (embedding the status, notes, diagram, and row-level editor widgets), starts the write-back server on port 8001 as a background job, and starts MkDocs on port 8000. When Apply/Save is clicked the EA model is updated immediately via COM, the page's `**Modified:**` date is bumped to today to match, and MkDocs hot-reloads the page within seconds.

Notes typed as plain text (no HTML tags) are automatically wrapped in `<p>` per blank-line-separated paragraph before being sent to EA, so multi-paragraph notes don't collapse into a single line. If you do want lists, bold text, or links, just type the HTML directly — ordinary rich text (`p`, lists, bold, links, ...) is preserved; `<script>`, event-handler attributes (`onerror=`, ...), and `javascript:`/embed/iframe content are stripped before it's saved, since this text is embedded directly into the wiki page as HTML.

> **Authentication:** each write-back server generates a random token on first use, saved to `<output>/.eaxwiki-token` (gitignored) and embedded into every exported page's editor widgets. The browser sends it back on every write request; requests without a matching token are rejected. If a widget shows "Not authenticated" after you upgrade EAxWiki, re-export with `--force` — incremental export skips unchanged pages, so pages exported before this existed won't have picked up the new token otherwise.

**Batch write-back** (for `.md` edits made while the server was not running):

```powershell
.\scripts\export.ps1 --writeback       # scan wiki/ for status, notes, diagram, and row-level description changes, write to EA
.\scripts\export.ps1                   # re-export to sync the wiki
```

**Status options** are read live from `t_statustypes` in the EA model — the dropdown always reflects the current valid set.

> **Linux / Mac:** Write-back is **not supported** on Linux or Mac. The EA COM API requires Sparx Enterprise Architect, which is Windows-only. A wiki served on Linux is read-only.

| Feature | Windows | Linux / Mac |
|---|---|---|
| Export (EA → wiki) | ✓ | ✗ requires EA |
| Serve wiki (MkDocs) | ✓ | ✓ |
| Live status write-back (wiki → EA) | ✓ | ✗ requires EA |
| Live notes write-back (wiki → EA) | ✓ | ✗ requires EA |
| Live diagram description write-back (wiki → EA) | ✓ | ✗ requires EA |
| Live attribute/method/tagged value write-back (wiki → EA) | ✓ | ✗ requires EA |
| Batch write-back (`--writeback`) | ✓ | ✗ requires EA |

### Running multiple wikis on one machine

Each export/serve/write-back triple is fully isolated by its `--output`, `--port`, and `--api-port` values, so you can run as many side by side as you like — for example, two different EA repositories served at once:

```powershell
.\scripts\export-and-serve.ps1 --repo "model/ProjectA.qea" --output "D:\wikis\A" --port 8000 --api-port 8001
.\scripts\export-and-serve.ps1 --repo "model/ProjectB.qea" --output "D:\wikis\B" --port 8100 --api-port 8101
```

Each write-back server only accepts requests from its own paired wiki: it checks that the request's `Origin` matches its own hostname on the `--wiki-port` it was started with. `export-and-serve.ps1` and `serve-api.ps1` infer `--wiki-port` from `--port` automatically, so nothing extra to configure above — instance A's wiki page simply can't reach instance B's write-back server, even though both run on the same machine. If you invoke `dotnet run --project src/EAxWiki -- --api ...` directly instead of through the scripts, pass `--wiki-port` yourself to match whichever port `mkdocs serve` uses for that instance.

The [auth token](#live-write-back--change-status-and-notes-directly-from-the-wiki-page) is isolated the same way, automatically: it's generated per `--output` directory (`<output>/.eaxwiki-token`), so instance A and instance B each get their own — A's token is never valid against B's server, even by accident.

## Saved connection config

On first run without `--repo`, the interactive prompt saves your connection string to a `.eaxwiki` file in the project root. Subsequent runs load it automatically — no re-entry needed.

```
First run:   interactive prompt → saved to .eaxwiki
Later runs:  loads .eaxwiki automatically
Override:    .\scripts\export.ps1 --repo "other_connection_string"
Reset:       delete .eaxwiki to re-enter interactively
```

> **Security:** `.eaxwiki` is encrypted at rest with Windows DPAPI, scoped to your Windows user account — only your account, on this machine, can decrypt it. It is also gitignored and never committed. A `.eaxwiki` file saved by an older version of EAxWiki (plaintext) is read transparently and re-encrypted automatically on next use.

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
- **Status badges** — element Status (Proposed, Approved, Implemented, "Not Set", etc.) shown as a coloured badge on its own line; used throughout element pages, type indices, and the Status Dashboard
- **Status and Notes edit icons** — when `--api-port` is set, a pencil icon next to Status and next to Notes opens a live write-back editor for each, in place, without a page reload (see [Live write-back](#live-write-back--change-status-and-notes-directly-from-the-wiki-page))
- **Relationships** — outgoing connectors with linked target element names
- **Referenced By** — incoming connectors from other elements with links
- **Appears on Diagrams** — inline thumbnail gallery of diagrams containing this element; each thumbnail links to the diagram page
- **Attributes, Methods, Tagged Values** — detailed tabs where present. Each description also gets a pencil edit icon when `--api-port` is set (see [Live write-back](#live-write-back--change-status-and-notes-directly-from-the-wiki-page))
- **Relationship Graph** — interactive force-directed graph showing the element's 2-hop neighbourhood (all directly connected elements, plus their neighbours). Nodes and edges are coloured by ArchiMate layer or EDGY facet, matching the stereotype label colours used throughout the wiki. The focal element is highlighted in orange. Unreachable 2-hop nodes (not in the export) appear at reduced opacity. Hover a node to see its full name and package in a tooltip. **Single-click** a node to expand it — its own neighbourhood is fetched and merged into the graph live. **Double-click** a node to navigate to its element page. Cross-package relationships are shown.

## Diagram page features

- **PNG image** — exported directly from EA
- **Interactive zoom** — click the image for a full-size overlay (via mkdocs-glightbox)
- **Description edit icon** — when `--api-port` is set, a pencil icon opens a live write-back editor for the diagram's description, same as the element Notes editor (see [Live write-back](#live-write-back--change-status-and-notes-directly-from-the-wiki-page))
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

