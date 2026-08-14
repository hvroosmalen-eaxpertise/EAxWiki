# EAxWiki — Export EA model to Markdown wiki, served with MkDocs

This repository exports an Enterprise Architect `.qea`, or any database stored model repository to a `wiki/` folder of Markdown pages, then serves them locally with MkDocs. The wiki is fully navigable, with diagrams, cross-cutting indexes, and in the right configuration enables live editing of **Status** and **Notes** directly from the page. It can also suggest AI-generated descriptions for elements via a local or cloud LLM.

> **Note — test data only:** The `wiki/` folder, `model/` folder, and the live site in this repository contain the **EurSuRA** model, which is used exclusively for development and testing of EAxWiki itself. They are not part of the tool and have no relation to your installation. When you use EAxWiki with your own EA model, it will write to a `wiki/` folder in your own environment.

**Live site (test data):** https://hvroosmalen-eaxpertise.github.io/EAxWiki/ this is a read-only demo of what EAxWiki produces, using the EurSuRA model. Again: It is not your own model.

### EAxWiki is developed and maintained by EAxpertise (The Netherlands) — see [www.eaxpertise.nl](https://www.eaxpertise.nl) for more information. Contact us at sales@eaxpertise.nl.


## How it works

EAxWiki is a two-step pipeline: **export** turns your EA model into Markdown, **serve** renders it as a website.

### Export

The exporter is a .NET 10 console application that connects to a Sparx Enterprise Architect model (`.qea` file) via COM Interop and writes a `wiki/` folder of Markdown files. Because it uses EA's COM API, it can only run on Windows with EA installed.

What the exporter produces:
- One Markdown page per element, organised into the same package hierarchy as the EA model
- PNG diagram images with a linked Markdown page per diagram (clickable for zoom via glightbox)
- Seven cross-cutting index views: **Structure**, **Types**, **Glossary**, **Diagrams**, **Recent Changes**, **Status Dashboard**, **Model Health**
- A `model.json` file with the full model serialised as JSON (opt-in via `--json` / `-j`)
- An `extra.css`, `.pages` navigation file, and an `ea-icons.js` SVG icon helper for MkDocs

The exporter runs **incrementally** by default — it compares each element's `ModifiedDate` in EA against the file's last-write time and skips anything that has not changed. Pass `-Force` to regenerate everything.

### Serve

The serve step runs [MkDocs](https://www.mkdocs.org/) with the [Material theme](https://squidfunk.github.io/mkdocs-material/) against the `wiki/` folder produced by the exporter. MkDocs renders the Markdown into a fully navigable website and serves it locally on a port of your choice.

Because the serve step only needs Python and the `wiki/` folder, it works on any platform — including Linux and Mac. The `wiki/` folder just needs to be accessible on the serving machine, whether via a shared filesystem, git, or any other means.

### Monitoring & Alerting

EAxWiki can send monitoring alerts to **Slack, Microsoft Teams, and/or Telegram** when background export/serve operations start, encounter issues, or recover — see [Scheduling exports](#scheduling-exports) for the unattended monitor wrapper that sends these. The channels are independent, not exclusive: configure any subset, and every alert goes to whichever channel(s) are set up.

Each channel needs a destination you create once in its own service, then give EAxWiki:

- **Slack** — an *Incoming Webhook* URL. In your workspace at https://api.slack.com/apps → *Create New App* → *From scratch* → enable **Incoming Webhooks** → *Add New Webhook to Workspace* and pick a channel. Copy the URL (`https://hooks.slack.com/services/...`).
- **Microsoft Teams** — a webhook URL from a Workflows flow (*Add a workflow* → *Webhook* → *When a webhook request is received* → *Send webhook alert to a channel*) or a classic Connector. Copy the URL.
- **Telegram** — a *bot token* plus a *chat ID*. Message **@BotFather** → `/newbot` to create a bot and get its token. Add the bot to a private chat, group, or channel, then open `https://api.telegram.org/bot<TOKEN>/getUpdates` in a browser and read the numeric `chat.id` from the JSON (positive for private chats, negative for groups/channels).

The easiest way to configure EAxWiki is to answer the interactive prompts when you first run it (`Configure Slack webhook... [y/N]`, `Configure Teams webhook... [y/N]`, `Configure Telegram monitoring alerts? [y/N]` — then paste each URL/token/chat ID). The values are encrypted into `.eaxwiki` with Windows DPAPI. To change or add a channel later, delete the `.eaxwiki` file in the repo root and run EAxWiki again — it will re-prompt for all destinations.

Alternatively, set environment variables (these are checked after CLI args and before `.eaxwiki`, and are the way scheduled runs pick them up when `.eaxwiki` is not used):

```powershell
$env:EAXWIKI_ALERT_WEBHOOK                 = '<slack webhook url>'
$env:EAXWIKI_ALERT_TEAMS_WEBHOOK           = '<teams webhook url>'
$env:EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN      = '<telegram bot token>'
$env:EAXWIKI_ALERT_TELEGRAM_CHAT_ID        = '<telegram chat id>'
```

Test a channel without running a real export:

```powershell
.\scripts\monitor-export-and-serve.ps1 --test-alert
```

This resolves each channel the same way a real scheduled run does (CLI flag → env var → `.eaxwiki`) and posts a blue "Test" message to every configured channel. Detailed walkthroughs, alert kinds/emojis, security notes, and troubleshooting live in [**Slack Webhook Setup**](docs/SLACK_WEBHOOK_SETUP.md), [**Teams Webhook Setup**](docs/TEAMS_WEBHOOK_SETUP.md), and [**Telegram Setup**](docs/TELEGRAM_SETUP.md).

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

**Runtime state** (`.data/`): The exporter and write-back server store runtime files — such as the edit-lock (`edit-lock.json`) and the audit log — in `.data/` alongside the `wiki/` directory. This keeps them outside mkdocs' file watcher, so they never trigger livereload when updated.

MkDocs itself is cross-platform — only EXPORT and WRITE-BACK need the Sparx EA COM interface (EAInterop.dll), which is Windows-only. See the next diagram for running SERVE on a separate Linux/Mac machine instead.
**Note:** On Linux systems it is possible to run an EA Instance through Wine, but this is not officially supported and may not work reliably.

### Windows + Linux — export on Windows, serve on Linux

```
┌──────────────────────────────────────────┐   ┌──────────────────────────────────┐
│            WINDOWS MACHINE               │   │         LINUX / MAC MACHINE      │
│                                          │   │                                  │
│  EA Model (.qea / DB)                    │   │  wiki/                           │
│           │                              │   │  Markdown + PNG files            │
│        COM API                           │   │           │                      │
│           │                              │   │           ▼                      │
│           ▼                              │   │  MkDocs + Material               │
│  EAxWiki.exe / export.ps1                │   │  scripts/serve.ps1               │
│           │                              │   │           │                      │
│           ▼                              │   │           ▼                      │
│  wiki/  ──── shared filesystem ──────────┼──►│  Browser                         │
│           (or git, rsync, etc.)          │   │  http://localhost:8000           │
│                                          │   │           │                      │
│  Wiki write-back server                  │ ◄─┼───────────┤ POST /api/status      │
│  EAxWiki.exe --api (port 8001)            │   │           │ POST /api/notes       │
│           │                              │   │           │ POST /api/ai-suggest  │
│           ├── EA COM → Update()          │   │           │ ... (live edits)     │
│           │                              │   │                                  │
│           └── LLM prompt ──────┐        │   │                                  │
│                                ▼        │   │                                  │
│  LLM endpoint (optional)                │   │                                  │
│  llama-server / OpenAI / Claude / ...    │   │                                  │
│  POST /chat/completions                  │   │                                  │
│  (OpenAI-compatible API)                 │   │                                  │
│                                          │   │                                  │
└──────────────────────────────────────────┘   └──────────────────────────────────┘
```

The browser talks to two servers on separate ports: **MkDocs** (`:8000`) for reading, and the **write-back server** (`:8001` on the Windows machine's IP) for live edits and AI suggestions. The write-back server calls EA COM for status/notes updates and the configured LLM endpoint for AI-generated descriptions.

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

> **Building with an older .NET SDK:** All projects target `net10.0` by default. The SchedulerUI (`src/EAxWiki.SchedulerUI`) targets `$(WindowsTargetFramework)`, which defaults to `net10.0-windows`. To build with .NET 8 or .NET 9 SDK instead, pass `-p:WindowsTargetFramework=net8.0-windows` (or `net9.0-windows`) when running `dotnet build`. The non-Windows projects (`EAxWiki.Core`, `EAxWiki.Export`, `EAxWiki.EA`) must be retargeted manually by changing their `.csproj` `<TargetFramework>` values — they have no override property.

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
| `--api` | | write-back server | Start the wiki write-back server so the pencil-icon editors on the page work live. Combined with `--cert` for HTTPS — see [Write-back server security](#write-back-server-security). |
| `--api-port <port>` | | write-back server | Port the write-back server listens on (default: `8001`). |
| `--wiki-port <port>` | | write-back server | Port the *paired* `mkdocs serve` uses (default: `8000`). The write-back server only accepts requests whose `Origin` matches this port — see [Running multiple wikis on one machine](#running-multiple-wikis-on-one-machine). `export-and-serve.ps1` / `serve-api.ps1` set this automatically from `--port`; only pass it yourself if you call `dotnet run` directly. |
| `--cert <path>` | | write-back server | Path to a PFX certificate for HTTPS. When set, the write-back server binds to `https://` instead of `http://` — see [Write-back server security](#write-back-server-security). |
| `--cert-password <pw>` | | write-back server | PFX certificate password. Only used with `--cert`. |
| `--port <port>` | `-p`, but only in `serve.ps1`/`export-and-serve.ps1`/`serve-api.ps1` | serve | Port `mkdocs serve` listens on (default: `8000`). Careful: this `-p` is those scripts' own shorthand — `-p` passed to the exporter itself (`export.ps1`) means `--package`, not port. |
| `--help` | `-h` | any | Show usage and exit. |

`--port` and `--api-port`/`--wiki-port` are independent of each other by design — one is MkDocs' own flag, the others belong to the EAxWiki write-back server — which is exactly what makes running several isolated instances on one machine possible.

## Scripts

| Script | Purpose |
|--------|---------|
| `scripts/export.ps1` | Export EA model to Markdown only |
| `scripts/serve.ps1` | Start MkDocs on an already-exported wiki |
| `scripts/export-and-serve.ps1` | Export then serve (calls the two above) |
| `scripts/serve-api.ps1` | Start MkDocs *and* the wiki write-back server together, without re-exporting |
| `scripts/writeback.ps1` | Scan wiki for status and notes changes and write them back to EA via COM (**Windows only**) |
| `scripts/monitor-export-and-serve.ps1` | Unattended wrapper for scheduled runs: retry with backoff, Slack/Teams/Telegram alerting, health page, serve watchdog (see [Scheduling exports](#scheduling-exports), **Windows only**) |
| `scripts/register-scheduled-task.ps1` | Register `monitor-export-and-serve.ps1` as a Windows Task Scheduler task — fixed interval or day/night mode (see [Scheduling exports](#scheduling-exports), **Windows only**) |
| `src/EAxWiki.SchedulerUI` | WinForms GUI front end for the script above — see [Scheduler GUI](#scheduler-gui), **Windows only** |

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

When the wiki runs locally on Windows with EA installed, users can edit an element's **Status** and **Notes**, a diagram's **Description**, and a package's **Notes**, directly from the rendered wiki page — no need to open EA. All use the same two-step pattern: a small pencil icon next to the value, click to edit, then confirm with icon-only buttons — a checkmark to **Save**/**Apply**, an **X** to **Cancel** (labels shown on hover/focus via `aria-label`/`title`). The action buttons carry no visible text; their meaning comes from the icon, loaded from the shared `ea-icons.js` helper written by the exporter.

- **Status** — sits on its own line (elements only). Clicking the pencil replaces the badge in place with a dropdown, Apply, and Cancel — no separate widget block elsewhere on the page. Elements with no status set show a "Not Set" badge and can be given one the same way.
- **Notes** — a pencil icon next to the notes text (elements and packages). Clicking it swaps the rendered notes for a raw-HTML `<textarea>` with Save / Cancel. Pages with no notes yet show "No description set." and can be given one the same way — the editor isn't gated on notes already existing. Package notes use separate HTML markers (`<!--ea-package-notes-start/end-->`) so they don't collide with element notes markers.
- **Diagram description** — same pencil-and-textarea editor as Notes, on the diagram page. If the diagram has no description of its own, the page shows one auto-derived from an element on the diagram (marked "(derived)"); editing pre-fills with that clean text (no label) so Save just promotes it into the diagram's own stored description. Diagrams with neither show "No description set." and start from a blank box.
- **Attribute, method, and tagged value descriptions** — the same pencil pattern, one per row. For Attributes and Tagged Values (narrow table columns), clicking the pencil expands a full-width row below it for the textarea and Save/Cancel, instead of cramming them into the Description cell. Method descriptions have room to spare already, so the textarea swaps in inline where the description text was. Opening one editor closes any other that's open, anywhere on the page.

Every pencil is gated on a runtime probe of the write-back server's `/readyz` endpoint, run once per page load by `api-probe.js`. If the API is unreachable, or the API is up but has lost its EA COM connection, the pencils render greyed out (`cursor: not-allowed`) with a tooltip explaining why (`Read-only: EAxWiki API not reachable.` / `Read-only: API is up but cannot reach the EA model.`), and clicks are ignored. Reloading the page after the API and EA come back re-enables editing without a re-export. This means a wiki served to a reader on a machine without EA — or served on a laptop while the API job isn't running — never offers an edit button the reader can't actually use.

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

> **Authentication (auth token):** each write-back server generates a cryptographically random 24-byte hex token on first use, saved to `<output>/.eaxwiki-token` (gitignored) and embedded into every exported page's editor widgets as `data-api-token`. The browser sends it back on every write request via the `X-EAxWiki-Token` header; requests without a matching token are rejected with HTTP 401. Token comparison uses `CryptographicOperations.FixedTimeEquals` (constant-time) to prevent timing side-channel attacks.
>
> The token is scoped to one `--output` directory, so multiple wiki instances on the same machine each get their own independent token — instance A's token is never valid against instance B's server.
>
> **Two-layer access control:** the auth token is complemented by a CORS-style origin check — the server only accepts cross-origin requests from the same hostname and the `--wiki-port` it was started with. This prevents a wiki page (or any script running in it) from accidentally or maliciously reaching a write-back server that belongs to a different EAxWiki instance on the same machine. The auth token itself protects against raw HTTP clients (curl, LAN scanning) that can set any `Origin` header they like — it is visible to anyone who can view the wiki page source, but is never transmitted over the network unencrypted when `--cert` is used (see [Write-back server security](#write-back-server-security)).
>
> **Fallback:** if `.eaxwiki-token` does not exist when the server starts, it is created automatically. Pages exported without a token (from an older version or incremental skip) will show "Not authenticated" when editing — re-export with `--force` to embed the current token.
>
> **Edit-lock — preventing export cycles from interrupting editors:** When a wiki page editor (status, notes, or diagram/row description) is opened, the widget acquires an edit-lock via `POST /api/edit-lock`; closing the editor releases it. The lock is persisted as `<repo>/.data/edit-lock.json` — intentionally outside `wiki/` so mkdocs' file watcher never sees it and the page never reloads mid-edit. The monitor's `Test-EditLock` function checks this file before each export — if a lock is active and not expired (5-minute timeout), the export cycle is deferred until the next loop iteration. This keeps the page stable while someone is actively editing.

**Batch write-back** (for `.md` edits made while the server was not running):

```powershell
.\scripts\export.ps1 --writeback       # scan wiki/ for status, notes, diagram, row-level description, and package notes changes, write to EA
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

### AI-suggested descriptions

When `--ai-endpoint` is configured (default `http://localhost:8080/v1`), a sparkle-icon **Suggest** button appears next to the Notes pencil icon. Clicking it sends the element's context to a local or cloud LLM and fills the textarea with a first-draft description focused on what the element does and why it matters — deliberately omitting information the viewer can already see (type, stereotype, status, package). While the request is in flight, the sparkle swaps to an animated spinner icon on the button; it reverts to the sparkle when the suggestion arrives or on error.

| Flag | Env | Default | Description |
|---|---|---|---|
| `--ai-endpoint <url>` | `AI_ENDPOINT` | `http://localhost:8080/v1` | OpenAI-compatible API base URL. Set empty to disable AI. |
| `--ai-model <name>` | `AI_MODEL` | `llama-3.2-3b` | Model name sent in API requests |
| `--ai-key <key>` | `AI_KEY` | `""` | API key (empty = no auth header) |

The default endpoint expects a local `llama-server` instance; any OpenAI-compatible provider works (OpenAI, Claude via LiteLLM, Azure OpenAI, etc.). Related-element context (target notes, stereotypes) is fetched from EA COM to enrich the prompt — see `docs/superpowers/specs/2026-07-08-ai-suggested-descriptions-design.md` for the full design.

### Branding

Optional `--brand eursura` emits the EurSuRA logo, palette, fonts, and graph colors; the default (no `--brand`) stays neutral. The brand can also be set via the `EAXWIKI_BRAND` env var or the `brand` field in `.eaxwiki`.

### Write-back server security

The write-back server is a Kestrel HTTP server that runs alongside `mkdocs serve` and accepts write requests from the browser. Since it can modify the live EA repository via COM, the following security measures are in place:

| Measure | What it does | Why |
|---|---|---|
| **Auth token** (`X-EAxWiki-Token`) | Random 24-byte hex token per output directory, validated with constant-time comparison | Prevents unauthorized access from LAN scanning or unrelated sites |
| **Origin/port CORS check** | Accepts cross-origin requests only from the same host on the configured `--wiki-port` | Prevents one wiki instance from reaching another's write-back server on the same machine |
| **HTTPS** (`--cert <pfx>` / `--cert-password <pw>`) | When a PFX certificate is provided, Kestrel binds to `https://` instead of `http://` | Protects the auth token and notes/status content from network eavesdropping (see below) |
| **Request body size limit** (1 MB) | `KestrelServerOptions.Limits.MaxRequestBodySize = 1_048_576` | Prevents OOM / disk-fill from arbitrarily large notes payloads |
| **Rate limiting** (60 / min / token) | In-memory sliding window per `X-EAxWiki-Token` value, returns `429 Too Many Requests` with `Retry-After: 60` | Prevents a compromised or misbehaving client from hammering the EA COM API |
| **Audit log** | JSON-lines file at `.eaxwiki-monitor/audit.log` with timestamp, token prefix, endpoint, element ID, field name, status code | Provides a structured trail of all write-back activity for forensic review |
| **Health endpoints** | `GET /healthz` → `{"status":"healthy", "ea":true\|false}`, `GET /readyz` → `200 {"status":"ready","ea":true}` or `503 {"status":"not ready","ea":false}` when EA COM is unreachable | Allow monitoring probes (and `api-probe.js` on every wiki page — see [Live write-back](#live-write-back--change-status-and-notes-directly-from-the-wiki-page)) to distinguish "API up + EA reachable" from "API up, EA gone" and "API down". `/readyz` reflects the current dispatcher state, not a stale startup snapshot |

**HTTPS in detail:** Without a certificate, the auth token and all write-back payloads travel in cleartext over HTTP. While the origin/port CORS check limits which pages can talk to the server, it does nothing against passive network eavesdropping on the same LAN (or, if the port is exposed, on the broader network). Providing a PFX certificate upgrades the connection to HTTPS, protecting all traffic using TLS. The server listens on HTTP or HTTPS depending on whether `--cert` is given — never both, since the mixed case offers no security advantage and adds an unauthenticated fallback path.

```powershell
# Start the write-back server with HTTPS
.\scripts\export-and-serve.ps1 --repo "model.qea" --api-port 8001 --cert "C:\certs\wiki.pfx" --cert-password "secret"
```

When HTTPS is active, the editor widget JavaScript constructs `https://` API URLs automatically (the server passes the protocol to the page at export time).

**Audit log in detail:** Each successful or failed write-back request produces one JSON line in `.eaxwiki-monitor/audit.log`:

```json
{"timestamp":"2026-07-07T14:22:00.0000000Z","tokenPrefix":"a1b2c3d4","endpoint":"POST /api/status","elementId":123,"field":"status","statusCode":200,"message":"Write-back completed"}
```

The log is written synchronously with `AutoFlush` semantics (append + flush per line) so no entry is lost on a crash. It lives in `.eaxwiki-monitor/` alongside the scheduler's health state, outside the wiki output directory, to avoid being cleaned up by `InfrastructureWriter.CleanupOrphanedFilesAsync`. The token prefix is truncated to its first 8 hex characters — enough to distinguish tokens in logs without exposing the full secret.

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

### Windows Task Scheduler — unattended monitoring (recommended)

`scripts/register-scheduled-task.ps1` registers `scripts/monitor-export-and-serve.ps1` on a fixed interval. Unlike calling `export.ps1` directly from Task Scheduler, the monitor wrapper is built for running unattended with nobody watching: it retries transient failures with backoff, restarts `mkdocs serve` if it dies, writes a `wiki/status/health.md` page, and (if a Slack, Teams, and/or Telegram alert destination is configured — see [Monitoring & Alerting](#monitoring--alerting)) posts an alert on every run start, on final failure, and on recovery.

```powershell
# Register a task that runs every 30 minutes
.\scripts\register-scheduled-task.ps1 --interval-minutes 30 --port 8000

# Or every N hours instead
.\scripts\register-scheduled-task.ps1 --interval-hours 4 --port 8000
```

Re-running `register-scheduled-task.ps1` with the same `--task-name` (default `EAxWiki-Monitor`) replaces the existing registration. Remove it with `Unregister-ScheduledTask -TaskName EAxWiki-Monitor`.

What the monitor wrapper does on each scheduled run:
- Pre-flight: kills any orphaned `EA.exe` left over from a prior crashed run
- Exports with bounded retry + backoff (`--max-retries`, default 3; `--retry-delay`, default 30s) and a sanity check that alerts if the element count collapses versus the previous successful run (`--min-element-fraction`, default 0.5)
- Checks whether `mkdocs serve` is still up — including a check on whether the wiki port itself is already listening, so it won't start a second, colliding `mkdocs serve` on top of one you started manually outside the monitor's tracking — and restarts it if it's down
- Sends "run starting" and "run finished" notifications for every pass — Start reports forced vs. incremental, Finish reports duration and page counts (total/diagram/element, delta vs. the previous run) — disable both with `--no-notify-start`; plus Failure/Recovery alerts for export and serve independently, and a once-per-day digest of approximate wiki page reads and write-back counts

Export mode on the schedule: incremental by default, same as `export.ps1` itself — forcing a full rebuild on every run of a short interval would be needlessly slow against a large model.

```powershell
# Force a full rebuild on every scheduled run
.\scripts\register-scheduled-task.ps1 --interval-minutes 30 --force

# Or force only every Nth run (e.g. once/day on a 30-minute cadence) for periodic
# drift correction, staying incremental the rest of the time
.\scripts\register-scheduled-task.ps1 --interval-minutes 30 --force-every 48
```

Overlap protection: the registered task uses `MultipleInstances IgnoreNew`, so if a run is still in progress when the next trigger fires (e.g. a slow EA export overruns a 30-minute interval), Task Scheduler skips the new trigger instead of stacking runs; an `ExecutionTimeLimit` just under the interval kills a genuinely hung run as a backstop.

The task also sets `WakeToRun` by default, so Task Scheduler holds the machine awake for the run's duration if something else wakes it while asleep — without this, a run can freeze for hours if the machine falls back asleep right after an unrelated wake event. Pass `--no-wake-to-run` to opt out (e.g. on a laptop where unexpected wake behavior is itself the bigger annoyance, or hardware with known-flaky wake timers).

You can also run `monitor-export-and-serve.ps1` directly (e.g. to test alerting) without registering a task:

```powershell
.\scripts\monitor-export-and-serve.ps1 --port 8000          # one pass
.\scripts\monitor-export-and-serve.ps1 --test-alert          # send a test message to every configured channel and exit
```

#### Day/night scheduling

By default a schedule runs at one fixed cadence 24/7. `--work-start`/`--work-end` switch to a
day/night mode instead: a fast interval during a weekday work-hours window, layered on top of a
slower all-day, every-day baseline (the "always-alive" heartbeat, so a real failure at night or on
a weekend isn't silently indistinguishable from "just paused"). This is two native Task Scheduler
triggers on one task, not a config file — `monitor-export-and-serve.ps1` has no idea day/night
scheduling exists.

```powershell
.\scripts\register-scheduled-task.ps1 --work-start "08:00" --work-end "18:00" `
    --work-interval-minutes 10 --off-hours-interval-minutes 240
```

All four flags are required together — there are no baked-in defaults for "work hours," since a
wrong silent default is worse than requiring an explicit choice. Changing the window later means
re-running this script with new flags; there is no live-reloaded config. See
`docs/superpowers/specs/2026-07-03-issue-38-scheduling-design.md` for the full design, including
why this is deliberately *not* timezone-aware scheduling for a global team (EA COM only runs on one
machine in one timezone — "day vs night" can only mean that machine's own clock).

#### Scheduler GUI

`src/EAxWiki.SchedulerUI` is a small WinForms app that builds and runs the same
`register-scheduled-task.ps1` calls above from a form instead of the command line — a Configuration
tab (view and edit the current `.eaxwiki` repo path/ports/Slack/Teams/Telegram alert settings, with a Save button;
repository type can be a `.qea` file or a SQL Server/MySQL-MariaDB/Oracle/PostgreSQL connection,
same as the console wizard), a Task Status tab (current state, next run time, registered triggers,
with Enable/Disable/Unregister buttons), and a Schedule Settings tab (simple interval or day/night
mode, export force mode, wake-to-run, a Register/Apply button). Opening the app — or clicking
Refresh Status — reads back whatever is actually registered in Task Scheduler and reflects it on
the Schedule Settings tab, so it never silently shows stale defaults in place of a real schedule.
It shells out to the same scripts and plain `Get-ScheduledTask` queries described above rather than
reimplementing any Task Scheduler logic itself — one source of truth either way.

The Configuration tab also includes a **Test Connection** button that validates the configured
repository path by opening it through EA's COM API (`EA.Repository.OpenFile`), the same mechanism
the exporter uses — so you know the connection works before scheduling. At startup, the app checks
for Administrator privileges and disables all Task Scheduler operations (Register, Enable, Disable,
Unregister, Refresh Status) with a clear message if the user is not elevated, since all Task
Scheduler cmdlets require admin rights.

```powershell
.\scripts\start-scheduler-ui.ps1
```

The script resolves the repo root from its own location, so it works from any working directory.
Equivalent to running `dotnet run --project src/EAxWiki.SchedulerUI` from the repo root directly.

### Windows Task Scheduler — simple export only

If you don't need retry/alerting/serve-watchdog behaviour, you can still call `export.ps1` directly:

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

The wiki has seven navigation views:

- **Structure** — a top-down tree of packages and their elements, following the EA model hierarchy
- **Types** — elements grouped by modelling language and type (e.g. ArchiMate3 BusinessRole, UML Metric)
- **Diagrams** — an alphabetically sorted global index of all diagrams with modified date and description
- **Glossary** — terms extracted from "Definition"/"Glossary" tagged values and first sentences from element notes
- **Recent** — top 50 most recently modified elements and diagrams, sorted by date descending
- **Status Dashboard** — a dashboard at `/status/` with summary bar charts, a **By Package** table with collapsible drill-down (clickable element links with status badges), and a **By Type** breakdown section
- **Model Health** — a report at `/status/model-health.html` flagging content-quality issues in the model itself (not export/serve pipeline health): orphan elements with no connectors and no diagram appearance, elements with an empty Notes field, elements with a Status set that haven't been touched in 90+ days, and duplicate element names within the same package. Every flagged entry links to the element's own page for one-click editing via the write-back widgets.

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

## Tests

EAxWiki uses **xUnit** with **Moq** for unit tests of the C# codebase. The test project is at `src/EAxWiki.Tests/`.

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests
```

PowerShell scripts are tested with **Pester 5**. Test files are in `tests/`.

```powershell
.\tests\run-tests.ps1
```

| Group | Tests | What's covered |
|-------|-------|---------------|
| Export integration | 11 | End-to-end Markdown output via `InMemoryWriter` |
| Write-back scanner | 4 | Notes round-trip routing, hash skip, attribute row notes |
| EaReader + ModelMapper | 60 | COM model mapping (Element/Package/Diagram), logger warning paths, null handling, guard clauses, Open validation, dispose, STA dispatcher COM-reconnect loop (`ExecuteWithReconnect`) |
| ContextBuilder | 9 | Sub-builder decomposition (ElementCollector, DiagramIndexBuilder, ConnectorIndexBuilder, LookupBuilder, PackageDirCollector) |
| Frontmatter parser | 18 | YamlDotNet-based YAML frontmatter parsing, notes/status hash+content rewrite, CRLF round-trip |
| ElementPageWriter renderers | 34 | All 11 widget renderers (rich HTML + plain Markdown modes), edge cases, 2-hop graph, missing references |
| Other | ~149 | Cleanup, Markdown helpers, hash helpers, config store, repository/health/validation writers, resilience, script template integrity, write-back server HTTP tests (auth/rate-limit/shutdown/CORS), config defaults, etc. |
| Property-based (FsCheck) | 26 | SanitizeName, EscapeCell, ParseStereotype, GetStereotypeLabel, SanitizeForAnchor, ComputeNotesHash, ComputeStatusHash invariants |
| **.NET subtotal** | **311** | |
| Bootstrap | 2 | `Get-EAxWikiDllPath` resolution + clear missing-DLL error |
| Export | 26 | `-Branch`, `-WhatIf`, `-Force`, overrides, cleanup guard, error paths, `--brand` |
| ExportAndServe | 23 | Port/root/API-port flags, retry/force args, combined pipeline args |
| Install | 11 | PS 5.1 compat via bootstrap, parameter binding |
| MonitorExportAndServe | 47 | Schedule parsing, task registration/update, state file, health check, alerting (Slack/Teams/Telegram), CLI flags, `--brand` |
| SendAlert | 2 | Telegram dispatch guard + dispatch |
| Serve | 12 | Port/root flags, file server config, cert modes, default page, path normalization |
| ServeApi | 13 | Port/root, CORS headers, routing, JSON endpoints, static fallback |
| ValidateWikiOutput | 12 | Validation CLI args (repo/output/tolerance, defaults, error paths) |
| Writeback | 14 | Token validation, CORS, note/DLNote/diagram/row-note endpoints, error paths |
| **Pester subtotal** | **162** | |

**473 tests total** (311 .NET + 162 Pester), all pass.

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

