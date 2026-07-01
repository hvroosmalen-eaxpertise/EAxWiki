# Design Decisions

## Architecture

- **Export pipeline**: C# .NET 10 console app reads EA model via COM Interop, writes Markdown + PNG files, then MkDocs serves the result. Three scripts for flexibility: `export.ps1`, `serve.ps1`, `export-and-serve.ps1`.
- **Test project** (`EAxWiki.Tests`): 19 unit tests for `MarkdownHelpers` and 6 integration tests using an `InMemoryWriter` stub. Tests run with xUnit and access internal types via `InternalsVisibleTo`.

## File Naming

- **`SanitizeName()`** for all file/folder names derived from user content (package names, element names, diagram names). Caches sanitized names in a `ConcurrentDictionary` to avoid repeated work.
- **Leading/trailing whitespace** in element names is trimmed via `SanitizeName().Trim()` to prevent link-to-file mismatches in the EA model.
- **`#` in filenames** is replaced with `_` because MkDocs interprets `#` as an anchor delimiter.

## Navigation

- **Five views**: Structure (tree hierarchy), Types (grouped by stereotype), Diagrams (global alphabetical index), Glossary (terms from tagged values and element notes), Recent Changes (top 50 most recently modified elements and diagrams).
- **awesome-pages MkDocs plugin** controls the nav. Root `.pages` uses `Structure: ''` + `Diagrams: diagrams/` + `Types: types/` format.
- **Breadcrumb** is shown on element and diagram pages using parent-package links.

## Export

- **Incremental export** (default): elements and diagrams whose output file is newer than the EA `ModifiedDate` are skipped. Pass `--force` to regenerate everything — useful after template changes.
- **Full regeneration with `--force`**: deletes and recreates the output directory before writing, then bypasses all timestamp checks.
- **Write-test probe** before directory deletion: verifies the output path is writable before doing anything destructive.
- **Relative links** use `../` prefix and forward slashes, computed via `Path.GetRelativePath`.
- **Parallel view generation**: `Task.WhenAll` runs Types, Glossary, Recent Changes, Diagrams index, and infrastructure writes concurrently after the structural export completes.
- **Duplicate sanitized filenames** (e.g. `unnamed.md`) get a `_{id}` suffix (e.g. `unnamed_634.md`). The actual written path is registered in `ExportContext.RegisteredElementFiles` so the orphan cleanup step knows the real filename and does not delete it.
- **`ExportContext`**: built once at the start of export, holds all indexes (element lookup, diagram index, incoming connector index, package lookup) and the `Force` flag. Shared across all export phases — no redundant model traversals.

## Diagrams

- **Format**: PNG via EA COM `Project.PutDiagramImageToFile`.
- **Page location**: `diagrams/{Name}.md` per diagram, inside their package subfolder.
- **Global index**: `diagrams/index.md` with a table (Diagram | Modified | Description | Path), sorted by breadcrumb path then diagram name.
- **Layout on diagram pages**: description text below the diagram image.
- **Diagram Status was dropped** — the `t_diagram.Status` column does not exist in this EA schema version.
- **`SELECT *` instead of `SELECT Status`** to avoid EA schema validation errors.
- **RPC_E_SERVERFAULT** for some diagrams during PNG export; isolated by per-diagram try-catch.

## Error Handling

- **Per-diagram failure isolation**: each diagram export is wrapped in try-catch; failures are collected and summarised in a single warning at the end rather than aborting the run.
- **`Dispose` is wrapped in try-catch** to prevent COM teardown crashes from propagating at exit.
- **EA.exe orphan cleanup** is done in the PowerShell script (not C#) by recording PIDs before/after export and killing only new PIDs in a `finally` block. This prevents locking build DLLs after a crash or aborted run.

## Logging

- **`Microsoft.Extensions.Logging`** with Console provider, configured in `Program.cs`.
- **`--verbose` / `-v` flag** sets the log level to Debug; otherwise defaults to Information.
- **Timestamp format**: `HH:mm:ss.fff` via `AddSimpleConsole`.
- **Per-element debug logging** under `--verbose` for tracing which elements are being processed.

## Modeling

- **Empty/whitespace stereotypes** group under `"Uncategorized"` in the Types view.
- **Diagram model** (`EaDiagram`) includes `Guid`, `ModifiedDate` (string), and a `DiagramObjects` collection for elements shown on each diagram.
- **Types page generation** uses a single `ToLookup()` pass (O(N)) instead of per-type `.Where()` calls (O(N×M)).

## Infrastructure

- **`FileOutputWriter`** implements `IOutputWriter` interface, writes files asynchronously with per-file `SemaphoreSlim` locking.
- **`Config` class** parses command-line flags: `--verbose` / `-v`, `--force` / `-f`, `--json` / `-j`, `--repo`, `--output` / `-o`, `--writeback` / `-w`, `--api`, `--api-port`.
- **`EaReader`** reads the EA model via COM, exposing `MapDiagram`, `ExportDiagramImage`, `GetStatusTypes`, `UpdateElementStatus`, `UpdateElementNotes`, and `RepositoryPath` methods. Validates the repository path before opening and uses `is` pattern matching over all COM collection loops to avoid hard casts.
- **`FileOutputWriter`** implements `IDisposable` and disposes all `SemaphoreSlim` instances on cleanup.
- **Output path is always passed as an absolute path** from the PowerShell scripts to avoid ambiguity: `dotnet run --project` changes the app's working directory to the project folder, so relative paths would resolve incorrectly. All scripts resolve `--output` to an absolute path before passing it to `dotnet run`.

## Round-trip editing (wiki → EA)

- **Live status editor widget**: Status renders on its own markdown line (`**Status:** <span id="ea-status-editor">...</span>`), wrapping the badge and a pencil edit button. Clicking the pencil hides the badge/button and appends a dropdown + Apply + Cancel directly into that same `<span>` — in place of the badge, not as a separate block elsewhere on the page. The widget renders regardless of whether a status is currently set (so a "Not Set" element can be given one), gated only on `--api-port` being passed to the exporter.
- **Wiki write-back server** (`WikiWritebackServer`, `--api` mode): an ASP.NET Core minimal API (SDK: `Microsoft.NET.Sdk.Web`) running on a configurable port (default 8001). Handles `POST /api/status`, `POST /api/notes`, and `POST /api/diagram-notes` requests from the widgets. Validates the new status against live `t_statustypes` (notes has no such validation — any HTML is accepted), updates EA via `element.Update()`/`diagram.Update()` COM call, then patches the `.md` file in-place.
- **CORS**: `builder.Services.AddCors()` + `app.UseCors()` with `AllowAnyOrigin/Header/Method` — required because MkDocs (:8000) and the write-back server (:8001) are different origins. Listens on both IPv4 (`0.0.0.0`) and IPv6 (`[::]`) because Chrome may resolve `localhost` to `::1`.
- **`FrontmatterParser.UpdateStatus`**: patches the `.md` file atomically (write to `.tmp` then `File.Move` overwrite): (1) `status:` and `ea_hash:` in YAML frontmatter, (2) the status badge `<span>` class and text in the page body, (3) `data-status` attribute on the widget span, (4) the `**Modified:**` date (see below). All must be updated so MkDocs hot-reload rebuilds the page with the correct, current state.
- **Change detection (`ea_hash`)**: `ea_hash: <SHA256(status)[..8]>` is stored in frontmatter. The batch write-back scanner (`WriteBackScanner`) detects manual `.md` edits by comparing the stored hash to the current `status:` value — a mismatch means the user edited the status field directly.
- **Status options are dynamic**: `GetStatusTypes()` calls `Repository.SQLQuery("SELECT Status FROM t_statustypes ORDER BY Status")` — always reflects the current EA model's valid status values. Options are embedded in `data-options` at export time and re-validated server-side on each POST.
- **`--dirty` MkDocs flag**: `serve.ps1` and `serve-api.ps1` pass `--dirty` to `mkdocs serve` so only the changed `.md` file is rebuilt on hot-reload instead of the full site, reducing rebuild time after a status change.
- **Multiple output directories**: all scripts accept `--output` / `-o` to specify the wiki directory as an absolute or relative path. This allows multiple wiki instances on the same server (different EA models, ports, and output dirs).
- **Live notes editor widget**: a two-step editor (pencil icon → raw-HTML `<textarea>` → Save/Cancel), deliberately raw HTML rather than WYSIWYG — EA's own `Notes` COM property already returns/accepts raw HTML fragments (no RTF/Markdown conversion layer), so the widget stays a symmetric passthrough. Rendered whenever `--api-port` is set, regardless of whether the element already has notes (issue #35 follow-up) — mirrors the earlier "Not Set" status fix, since gating the widget on non-empty notes meant an element with none had no way to add any from the wiki page. The shared client-side placeholder/empty-content handling (built for diagrams) already covered this correctly; only `ElementPageWriter`'s render condition needed to drop the non-empty check.
- **`FrontmatterParser.NormalizeNotesHtml`**: the single choke point for all notes writes (live editor, batch scanner, and export). If the input contains no `<` at all, it wraps each blank-line-separated block in `<p>` so multi-paragraph plain text survives Markdown's block-HTML passthrough (which, unlike top-level Markdown, does not auto-wrap bare text in `<p>`). It also strips any embedded `<!--ea-notes-start/end-->` marker text — a defensive measure against the widget's `innerHTML` capture, which otherwise includes the markers as literal comment text since they sit inside `.ea-notes-content`.
- **`notes_hash`**: same pattern as `ea_hash`, computed over the *normalized* notes value (not the raw EA value) so the hash always matches what's actually embedded between the `<!--ea-notes-start-->`/`<!--ea-notes-end-->` markers in the page body.
- **Notes batch write-back requires `--api-port`**: `WriteBackScanner` can only detect and apply manual notes edits on pages that were exported with `--api-port` set, because the `<!--ea-notes-start/end-->` markers (needed to isolate the notes text from the rest of the page body) are only emitted in that mode. Pages exported without it still get a `notes_hash` in frontmatter but no markers, so the scanner correctly skips them.
- **`WriteBackScanner.Scan` returns `ScanResult(StatusChanges, NotesChanges)`**: a single pass over the wiki directory checks both `ea_hash` and `notes_hash` per file and applies whichever changed, rather than two separate directory walks.
- **`**Modified:**` date is bumped on every write-back**: `FrontmatterParser.UpdateStatus`/`UpdateNotes` patch this date to today in addition to the field itself. This isn't cosmetic — `ElementPageWriter`'s incremental skip check compares the `.md` file's own write-time against EA's `ModifiedDate`, and since the write-back patch's file write happens *after* EA's COM `Update()` call bumps that date, the file's write-time would otherwise permanently exceed it, causing the page to be skipped on every future export (not just until the next run) short of `--force`.
- **Diagram descriptions reuse the Notes machinery wholesale**: `DiagramExporter` now writes a small frontmatter block (`diagram_id`, `notes_hash`) — a first for that generator, which previously had none — and the same `ea-notes-editor` widget markup as elements, distinguished only by a `data-kind="diagram"` attribute that `notes-editor.js` reads to pick the endpoint (`/api/notes` vs `/api/diagram-notes`) and payload id field (`elementId` vs `diagramId`). `EaReader.UpdateDiagramNotes` mirrors `UpdateElementNotes` via `Repository.GetDiagramByID`.
- **Derived description vs. editable seed value are kept separate**: when a diagram has no notes of its own, the page falls back to an auto-derived sentence pulled from one of its elements' notes (`DiagramExporter.GetDerivedDescriptionText`). The content between the `<!--ea-notes-start/end-->` markers is always that raw derived sentence with no label — the `"(derived)"` provenance indicator lives in a separate `<span class="ea-notes-derived-hint">` *outside* `.ea-notes-content`, so it's never captured by the widget's `innerHTML`-based edit-seed logic and never gets written back to EA. Genuinely empty diagrams (no notes, nothing derivable) get an empty marker block; `notes-editor.js` detects the empty content client-side and injects a `.ea-notes-placeholder` ("No description set.") for display only, which is explicitly excluded from the edit-seed value so editing starts blank rather than seeding the placeholder text itself.
- **`WriteBackScanner` discriminates element vs. diagram pages by frontmatter key**: `ea_id` routes to `UpdateElementStatus`/`UpdateElementNotes`; `diagram_id` routes to `UpdateDiagramNotes`. Both share one `TryWriteBackNotes` helper parameterized by an `Action<int, string>` update delegate, since the hash-compare/normalize/patch logic is otherwise identical.

## Deployment

- **Production target is local server only** — GitHub Pages was considered but ruled out: the write-back server requires a running Windows machine with EA installed. The wiki is served locally via MkDocs. GitHub Pages may still be used for publishing a read-only snapshot.
- **GitHub Pages** at `https://hvroosmalen-eaxpertise.github.io/EAxWiki/` — used for the test/demo model only.
- **CI workflow** (`.github/workflows/mkdocs-deploy.yml`) has `permissions: contents: write` on the GITHUB_TOKEN for pushing to `gh-pages`.
- **Export cannot run in CI** — EA COM Interop is Windows-only with a running EA instance. The CI workflow only builds MkDocs from the pre-generated `wiki/` folder.
