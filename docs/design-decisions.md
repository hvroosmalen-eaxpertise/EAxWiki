# Design Decisions

## Architecture

- **Export pipeline**: C# .NET 10 console app reads EA model via COM Interop, writes Markdown + PNG files, then MkDocs serves the result. Three scripts for flexibility: `export.ps1`, `serve.ps1`, `export-and-serve.ps1`.

    ```mermaid
    sequenceDiagram
        participant Script as Launch script
        participant Exporter
        participant MkDocs as mkdocs
        participant Browser

        Note over Script,Browser: Export — runs once, before serving
        Script->>Exporter: dotnet run EAxWiki -- --output wiki [--api-port]
        Exporter->>Exporter: EaReader.Open(repo) — read via COM
        Exporter->>Exporter: write .md/.png/.css/.js + token (parallel)
        Exporter-->>Script: exit 0 — "Done. Wiki generated"

        Note over Script,Browser: Start services
        Script->>MkDocs: mkdocs serve --dev-addr 0.0.0.0:PORT --dirty
        MkDocs->>MkDocs: pip install requirements, watch wiki/ for changes

        Note over Script,Browser: Every page load — runtime
        Browser->>MkDocs: GET /
        MkDocs-->>Browser: 200 OK — rendered page + widgets
    ```
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
- **`ExportContext`**: built once at the start of export, holds all indexes (element lookup, diagram index, incoming connector index, package lookup) and the `Force` flag. Shared across all export phases — no redundant model traversals. Built by `ContextBuilder.Build()`, which delegates to five focused sub-builders:

    ```mermaid
    flowchart LR
        A[ContextBuilder.Build] --> B[ElementCollector]
        A --> C[DiagramIndexBuilder]
        A --> D[LookupBuilder]
        A --> E[ConnectorIndexBuilder]
        A --> F[PackageDirCollector]
        B -->|List<Element,Dir>| G[ExportContext]
        C -->|List<Diagram> + Index| G
        D -->|ElementLookup + PackageLookup| G
        E -->|Incoming connector index| G
        F -->|All package dirs| G
    ```

    Each sub-builder is `internal static`, independently testable, and was extracted from the original monolithic `Build()` method (see issue #46). `ConnectorIndexBuilder` includes the `HashSet<int>` dedup guard that prevents dual-side EA COM connector duplication from inflating the incoming index.

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
- **Saved connection string (`.eaxwiki`) is encrypted at rest with Windows DPAPI**: `LocalConfigStore` wraps `ProtectedData.Protect`/`Unprotect` with `DataProtectionScope.CurrentUser`, so only the Windows account that saved it can decrypt it — a MS SQL/MySQL/Oracle/PostgreSQL connection string entered interactively can embed a plaintext password (see the connection string examples under `--repo`), and `.eaxwiki` previously stored that as-is. A fixed, non-secret entropy value is mixed in purely to scope decryption to blobs this app wrote, not defense against a compromised account. `Load` transparently falls back to reading a pre-existing plaintext file (either not base64, or base64 that fails `Unprotect` — e.g. written under a different Windows account) and immediately re-saves it encrypted, so upgrading is a no-op for the user. DPAPI is Windows-only, consistent with the rest of the project's [EA COM-only constraint](#deployment) requiring Windows.

## Round-trip editing (wiki → EA)

- **Live status editor widget**: Status renders on its own markdown line (`**Status:** <span id="ea-status-editor">...</span>`), wrapping the badge and a pencil edit button. Clicking the pencil hides the badge/button and appends a dropdown + Apply + Cancel directly into that same `<span>` — in place of the badge, not as a separate block elsewhere on the page. The widget renders regardless of whether a status is currently set (so a "Not Set" element can be given one), gated only on `--api-port` being passed to the exporter.
- **Wiki write-back server** (`WikiWritebackServer`, `--api` mode): an ASP.NET Core minimal API (SDK: `Microsoft.NET.Sdk.Web`) running on a configurable port (default 8001). Handles `POST /api/status`, `POST /api/notes`, `POST /api/diagram-notes`, and `POST /api/row-notes` requests from the widgets. Validates the new status against live `t_statustypes`, updates EA via `element.Update()`/`diagram.Update()`/etc. COM calls, then patches the `.md` file in-place.
- **CORS is scoped per-instance, not `AllowAnyOrigin`**: custom middleware (not the built-in CORS services) accepts a request only if its `Origin` hostname matches the request's own `Host` header (works under any LAN name/IP the server is reached by) *and* `Origin`'s port matches a `--wiki-port` flag (default 8000) naming the one `mkdocs serve` instance this server is paired with. This exists because the project explicitly supports running several exporter/serve/write-back triples on one machine (`--output`/`--port`/`--api-port` per instance) — a blanket "same host, any port" rule would let one instance's wiki page talk to a sibling instance's write-back server. `export-and-serve.ps1`/`serve-api.ps1` set `--wiki-port` automatically from `--port`. Listens on both IPv4 (`0.0.0.0`) and IPv6 (`[::]`) because Chrome may resolve `localhost` to `::1`.
- **Write-back API requires a per-instance shared-secret token**: CORS/origin matching above only restricts *browser-mediated* cross-origin calls — a raw HTTP client (`curl`, a LAN port scan) can set any `Origin` header it likes, so it provides no real authentication. `ApiTokenStore.GetOrCreate` generates a random token once per wiki output directory, persisted to `<output>/.eaxwiki-token` (gitignored) so a later export and a later `--api` run agree on the same value without any manual config. The exporter embeds it into every widget as `data-api-token`; the client JS sends it back as an `X-EAxWiki-Token` header; the server checks it (via `CryptographicOperations.FixedTimeEquals`) on every `/api/*` request before doing anything else. Because the token is embedded in the exported HTML, anyone with legitimate view access to that wiki instance can read it from page source — it stops everyone *else* (LAN scanning, unrelated sites), not a viewer turning malicious. A page exported before this token existed has no `data-api-token`; re-export with `--force` to refresh it (incremental export won't touch unchanged pages).

    ```mermaid
    sequenceDiagram
        participant Exporter
        participant Token as .eaxwiki-token
        participant Server as Write-back server
        participant Browser as Browser (widget JS)

        Note over Exporter,Browser: Setup — once per output directory
        Exporter->>Token: GetOrCreate(outputPath)
        Token-->>Exporter: token (generated once, reused after)
        Exporter->>Browser: embed as data-api-token in exported page
        Server->>Token: GetOrCreate(outputPath)
        Token-->>Server: same token

        Note over Browser,Server: Every edit — runtime
        Browser->>Server: POST /api/status (X-EAxWiki-Token: token)
        Server->>Server: FixedTimeEquals(token, stored)
        alt match
            Server-->>Browser: 200 OK — EA + wiki page updated
        else mismatch
            Server-->>Browser: 401 — "Not authenticated"
        end
    ```
- **Write-back file-path resolution is centralized in `TryResolveWikiFilePath`**: rejects any `req.FilePath` that doesn't resolve to somewhere strictly inside the output directory (compared with a trailing separator, so a sibling directory sharing the same prefix — e.g. `wiki` vs `wiki-archive` — can't pass) and doesn't end in `.md`. Previously each of the four endpoints repeated a raw `StartsWith` check with no trailing separator, which is exactly the kind of prefix bug that lets `..\` traversal into a same-prefixed sibling slip through.
- **Notes HTML is sanitized, not passed through verbatim**: `FrontmatterParser.NormalizeNotesHtml` runs any notes value containing a `<` through `Ganss.Xss.HtmlSanitizer` (AngleSharp-backed allowlist sanitizer) before it's persisted to EA via COM or embedded into the generated page. Without this, typed `<script>`/`onerror=`/`javascript:` content would round-trip forever (re-exported from EA on every future run) as stored XSS on the wiki page. Ordinary rich text (`p`, lists, bold, links, ...) passes through unchanged.
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
- **Attribute/method/tagged-value descriptions have no ID to key on** (issue #35 follow-up): reflection on the embedded `EAxWiki.EA.dll` interop metadata (`IDualAttribute`, `IDualMethod`, `IDualTaggedValue`) confirmed these COM interfaces expose only `Name` plus their type-specific fields — no `AttributeID`/`MethodID`/equivalent, unlike `IDualElement` (`ElementID`) and diagrams. Write-back therefore fetches the parent element via `Repository.GetElementByID` and searches its `.Attributes`/`.Methods`/`.TaggedValues` collection by a composite key: Attribute on Name+Type, Method on Name+ReturnType+IsStatic, TaggedValue on Name+Value. EA permits duplicate names (method overloads, repeated tag names), so a residual tie after the composite match takes the first hit and logs a warning rather than failing the write.
- **Per-row change detection lives on the row itself, not in frontmatter**: unlike `ea_hash`/`notes_hash` (one value per file), an element can have an arbitrary number of attributes/methods/tagged values, so each row's `data-notes-hash` sits directly on its `<button class="ea-row-notes-edit-btn">`, alongside a `data-row-id` used to pair it with its `<!--ea-row-notes-start:{rowId}-->...<!--ea-row-notes-end:{rowId}-->` marker block. `FrontmatterParser.ExtractRowNotesContent`/`UpdateRowNotes` are parameterized by `rowId` so a single file can hold many independent marker pairs; `UpdateRowNotes`'s hash-patch regex requires `data-row-id` to precede `data-notes-hash` on the same tag.
- **Row markers sit inline, not on their own lines**: element/diagram notes markers are each written as separate list entries (their own source lines); row widgets are built as one concatenated HTML string per table cell / inline `<div>`, so the markers and content share a line. `ExtractRowNotesContent`/`UpdateRowNotes`'s regexes match content between the markers directly (no assumed `\n` on either side) to work either way — a mismatch here was the one bug the row-notes test suite caught before it reached a live model.
- **Two edit-UI surfaces sharing one script**: Attributes and Tagged Values render as real HTML `<table>`s (not markdown pipe-tables) when `--api-port` is set, specifically so each row can have a hidden sibling `<tr class="ea-row-edit">` for the edit textarea — expanding a full-width row below a narrow Description cell rather than cramming Save/Cancel into it. Methods already render with a full-width paragraph per entry (heading + Returns + notes), so their widget uses `data-surface="inline"`: the textarea replaces the notes text in place, no sibling row needed. `row-notes-editor.js` branches on `data-surface` but is otherwise one shared, multi-instance script (`querySelectorAll`, not `getElementById` like the singular status/notes widgets) that closes any other open row editor before opening a new one.
- **`ElementPageWriter.HtmlEscape`** gained quote-escaping (`"` → `&quot;`) to safely embed attribute/method/tag names as HTML attribute values in the row widgets' `data-*` attributes; verified harmless for its one prior use (the relationship graph's embedded JSON `<div>`, since HTML entities decode back through `.textContent`).

## AI-Suggested Descriptions (issue #77)

- **Context sent to the LLM includes related-element information, not just the element's own properties**: `RelationshipInfo` stores `TargetStereotype`, `TargetNotes`, and `ConnectorStereotype` so the prompt can describe what an element relates to and through what kind of connector. `EaElementSummary.Notes` (the element's existing description) is also included as "Existing Description" in the prompt.
- **The prompt explicitly forbids restating visible metadata**: the system instruction tells the LLM not to mention the element's type, stereotype, status, package, or relationship-type names — information already shown on the wiki page's own badges and labels. The focus is purpose and business significance.
- **Phase 1 (element suggestions) complete; Phase 2 (diagram suggestions) deferred**: Diagram-level suggestions will follow in a later change.
- **All context is sourced from local EA COM access, not an external system**: `EaReader.GetElementSummary` fetches element notes, relationships, and connector metadata through the same COM interop used by the rest of the exporter. `MapRelationshipsForSummary` in `EaReader.cs` extends the relationship summary with richer target and connector data.
- **Prompt builder is in `WikiWritebackServer.BuildSuggestPrompt`**: constructs a "Related Elements" section (each entry includes element name, stereotype, and notes) plus the element's own existing description and tagged values. The system prompt is intentionally concise — a single paragraph forbidding specific categories of response rather than a long list of rules.

## Icon Action Buttons (issue #81)

- **The 7 runtime-created action buttons are icon-only, no visible text**: the status editor's **Apply**/**Cancel**, the notes editor's **Save**/**Suggest**/**Cancel**, and the row-notes editor's **Save**/**Cancel**. Text labels were noisy and consumed horizontal space in the inline editing controls. Meaning comes from the icon plus `aria-label`/`title` (matching the pre-existing pencil-edit-button convention), so screen readers and hover/focus tooltips still convey the action. Every button is also `type='button'` to prevent accidental form submission.
- **Inline SVG, not an icon font or image files**: each icon is a 24×24 `viewBox` Material-style path with `fill="currentColor"`, sized ~1em via a shared `svg { width: 1em; height: 1em; fill: currentColor }` rule in `extra.css` — so an icon inherits the button's text color (e.g. white on the purple Suggest button) with no per-icon color rules, and there are no extra network requests.
- **Shared external helper `wiki/ea-icons.js`**: a new file generated by `InfrastructureWriter.WriteIconsScriptAsync` exposing `window.EAxIcons` with an `ICONS` map (`save`, `cancel`, `suggest`, `apply`, `spinner`) and `set(btn, name, label)` which sets `innerHTML` to the icon SVG plus `aria-label`/`title`. It is loaded first in `mkdocs.yml` `extra_javascript` because the three editor scripts call `EAxIcons.set` when they build buttons at runtime.
- **Defensive guard against a missing helper**: every `EAxIcons.set` call is wrapped in `if (typeof EAxIcons !== 'undefined')`, and each editor script logs a one-time `console.error` (via `window.__eaIconsWarned`) if `EAxIcons` is undefined — so a page whose script failed to load still builds its buttons instead of throwing mid-render.
- **Suggest in-flight spinner**: while the AI request is running, the notes editor swaps the sparkle (`suggest`) icon for a `spinner` icon carrying the `.ea-icon-spinner` class, which `@keyframes ea-spin` (0.8s linear infinite) rotates. The sparkle is restored on both the success and the error paths, so the button never gets stuck on the spinner.
- **Compact square styling**: the action buttons are 1.8em × 1.8em `inline-flex` centered squares, `padding: 0`, 4px radius, coloured from the theme's CSS variables (`--md-*`) so they follow Material's light/dark scheme; the Suggest button keeps its distinct purple accent.

## Unattended monitoring & scheduling (issues #37, #38)

- **Monitoring logic lives entirely in the PowerShell wrapper layer, not the C# exporter**: a broken `MarkdownExporter` can't reliably report its own breakage, so logging, retry/backoff, the health page, and alert dispatch all live in `scripts/monitor-export-and-serve.ps1`, wrapping `dotnet run` from the outside rather than inside `EAxWiki.Export`. Bounded retry (`--max-retries`, default 3) with backoff, plus a sanity check (`--min-element-fraction`, default 0.5) that catches an export exiting 0 while producing near-empty output.
- **Alert dispatch is a small, channel-agnostic function** (`Send-Alert -Kind Start|Failure|Recovery|ServeFailure|ServeRecovery|Test`): only Slack is implemented today, but the call site doesn't know that, so email/Teams can slot in later without touching the retry/detection logic. A transient failure that resolves on retry within the same pass does not alert — only the final outcome of a pass does. Webhook URL resolves in order: `--webhook-url` CLI arg → `EAXWIKI_ALERT_WEBHOOK` env var → `.eaxwiki`; deliberately not accepted as a Task Scheduler action argument, since those are readable by any admin via `Get-ScheduledTask`.
- **`wiki/status/health.md`** (pipeline health) is a separate page from the element Status Dashboard (`wiki/status/index.md`) — different subject (export/serve pipeline health vs. EA model element status), generated by the PowerShell wrapper, not the exporter, since its entire job is to report when export *didn't* run.
- **Serve watchdog checks the port before the PID**: `Test-ServeAlive` first checks whether its own tracked PID (stored with a start-time alongside it, not just the bare PID, to survive a reboot without a false positive from PID reuse) is still the process it launched; if not, it checks whether the wiki port itself is already listening before concluding serve is down. Without the port check, restarting serve after this monitor's own state was lost would start a second, colliding `mkdocs serve` on top of one started manually (e.g. via `export-and-serve.ps1`) outside its tracking.
- **Day/night scheduling is two native Task Scheduler triggers on one task, not a config file** (`register-scheduled-task.ps1 --work-start/--work-end/--work-interval-minutes/--off-hours-interval-minutes`): a slow all-day/every-day baseline (so a real failure at night/on a weekend isn't silently indistinguishable from "just paused") plus a fast weekday work-hours boost layered on top. `MultipleInstances IgnoreNew` (already needed for basic overlap protection) makes the two triggers' deliberate overlap during weekday daytime harmless. `monitor-export-and-serve.ps1` needs zero changes — it has no idea day/night scheduling exists. Explicitly not timezone-aware: EA COM only runs on one Windows machine in one timezone, so "day vs night" can only mean that machine's own clock, not per-reader adaptation for a distributed team (see `docs/superpowers/specs/2026-07-03-issue-38-scheduling-design.md`).
- **`--force-every N`** (distinct from the interval): forces a full rebuild only on every Nth scheduled run, tracked via `runsSinceForce` in the health state file, so a short-interval schedule can stay incremental most of the time while still self-correcting for drift periodically.
- **Two PowerShell/.NET gotchas hit building the above, now guarded against**: (1) `$PSNativeCommandUseErrorActionPreference` (PowerShell 7.3+, defaults `$true` in a `-NoProfile` session — exactly how Task Scheduler launches this script) corrupts `$LASTEXITCODE` when a native command's stderr is merged via `2>&1`, which was enough to make dotnet's own warn-level log lines register a fully successful export as a failure; set to `$false` at the top of `export.ps1`, `writeback.ps1`, and `monitor-export-and-serve.ps1`. (2) `New-ScheduledTaskTrigger`'s `-Daily`/`-Weekly` parameter sets don't expose `-RepetitionInterval`/`-RepetitionDuration` at all (only `-Once` does, confirmed via `Get-Command`'s own `ParameterSets`) — worked around by building a throwaway `-Once` trigger purely to get a correctly-populated `.Repetition` CIM object, then assigning that object onto the real Daily/Weekly trigger.
- **`EAxWiki.SchedulerUI`** (WinForms) shells out to `register-scheduled-task.ps1` and plain `Get-ScheduledTask`/`Get-ScheduledTaskInfo` calls via `pwsh.exe` rather than reimplementing Task Scheduler registration in C# — one source of truth for the CIM-Repetition workaround above, not two places for that bug class to reappear. Scoped to scheduling plus basic config editing: its Configuration tab reads and writes `.eaxwiki` (repo path/ports/Slack+Teams webhooks) through plain text/numeric fields and a Save button — a simple edit form, not the console wizard's full repo-type/connection-string flow (SQL Server/MySQL/Oracle/PostgreSQL), which stays console-only. `LocalConfigStore` was extracted from the `EAxWiki` console/API project into `EAxWiki.Core` (`Configuration/LocalConfigStore.cs`) so this GUI can read `.eaxwiki` without duplicating the DPAPI decrypt logic.
- **Teams webhook support (issue #39) is additive to Slack, not a replacement or a choice between the two**: `TeamsWebhookUrl` sits alongside `WebhookUrl` in `LocalConfigStore.Config` and resolves through the identical three-tier order (CLI arg → env var → `.eaxwiki`). `Send-Alert` dispatches to each configured channel independently — one webhook failing (revoked, wrong URL) doesn't suppress the other, and having neither configured just means alerts are logged locally only, same as before this issue. Teams uses the classic Incoming Webhook `MessageCard` payload (`@type: MessageCard`), not Slack's `attachments` structure; the two payloads are built and posted separately inside the same `Send-Alert` call, sharing only the `$Kind`-derived color/message, not a common serialization path — the schemas don't overlap enough to make a shared builder worth it for two formats.

## Model Health (issue #68)

- **Model content quality, not export mechanics or pipeline health**: three different "health" concepts live in `status/`, deliberately kept separate — `status/health.md` (pipeline health: did export/serve actually run, generated by the PowerShell monitor wrapper), the `ExportValidator` report (did the export render correctly: broken links, unclosed HTML, missing images — issue #49), and `status/model-health.md` (does the *model itself* have content problems: orphan elements, missing descriptions, stale elements, duplicate names — generated by `ModelHealthExporter` in the C# exporter, alongside the Status Dashboard).
- **Orphan = zero connectors AND no diagram appearance, not either alone**: an element with a relationship but no diagram, or vice versa, is not flagged. This is also the noise-reduction mechanism — a model like EurSuRA has plenty of legitimately-standalone descriptive/reference elements that would otherwise flood the report. Implementation only needs `elem.Connectors.Count == 0` — EA's `Element.Connectors` COM collection already returns connectors in both directions (client or supplier), which is also why `ConnectorIndexBuilder` needs its `seenConnectors` dedup when building the global incoming-connector index; no separate cross-reference against `ctx.IncomingIndex` is needed for a single element's orphan check.
- **"Untouched N+ days", not "stale status"**: `ModifiedDate` bumps on *any* field change (Notes, tagged values, relationships, etc.), not specifically a Status change — EA COM exposes no cheaper way to track how long the Status value itself has been unchanged without the (usually disabled) audit-trail feature. The report is honest about this in its own copy rather than implying it tracks Status-field history specifically.
- **Duplicate names are scoped to the same package only, keyed by `PackageId`, not the sanitized folder path**: the same name recurring in different packages is expected and intentional in this model, so cross-package matches are never flagged. Grouping by the sanitized `PackageDir` string instead of the real `PackageId` would risk false positives, since two structurally different packages can sanitize to the same folder name at different points in the hierarchy.
- **Every flagged entry links to the element's own page**, same convention as every other cross-cutting view — and it closes the loop with round-trip write-back specifically: clicking through from a "missing description" entry lands directly on a page that already has the `notes-editor` widget in place to fix it.

## Table formatting

- **Tables are rendered as raw HTML `<table>` elements** (not Markdown pipe-tables) so the exporter can control `width`, `word-break`, and `th`/`td` attributes directly per-column — see `InfrastructureWriter.WriteExtraCssAsync`. All EA data tables (element metadata, attribute/method/tagged value tables) use this approach.

- **MkDocs Material JS wraps every classless `<table>` at runtime**: the `content` plugin in `bundle.*.min.js` calls `M("table:not([class]")` on DOM-ready, which wraps each matching table in `<div.md-typeset__scrollwrap><div.md-typeset__table>{{table}}</div></div>`. This is Material's mechanism for scrollable overflow on wide markdown tables.

- **The `md-typeset__table` wrapper uses `display: inline-block` by default**, causing the wrapper to shrink-wrap to the table's intrinsic content width. A `width: 100% !important` rule on the `<table>` becomes circular — 100% of an inline-block parent that is itself sized to the table's content — so the table falls back to its intrinsic (content-based) width, appearing to "flash full-width then shrink" after the JS runs.

- **Fix: override `md-typeset__table` to `display: block !important`** in `extra.css`. A block-level wrapper fills the content area, making the table's `width: 100%` meaningful. The selector deliberately drops `:not([class])` so the rule survives any class that Material JS may add to the table dynamically.

    ```css
    .md-typeset table { width: 100% !important; }
    .md-typeset__table { display: block !important; }
    ```

    Applied in two places: `wiki/extra.css` (used by MkDocs directly) and `src/EAxWiki.Export/Resources/extra.css` (embedded as a managed resource, written on every export). Both must be kept in sync.

## Package Notes Editing (issue #78)

- **Package pages get Notes editing only, not Status**: EA 17.1's COM `IPackage` interface has no `Status` property (confirmed via reflection on `Interop.EA.dll`; `t_package` table also lacks a `Status` column). Rather than use Tagged Values as a workaround, package status editing was dropped from the scope. Element status editing is unaffected — `EA.IElement.Status` works.
- **Separate HTML markers for package notes**: Package pages use `<!--ea-package-notes-start/end-->` markers, distinct from element `<!--ea-notes-start/end-->`, so the notes-editor.js widget can correctly identify which kind of page it's on via the `data-kind="package"` attribute.
- **Same frontmatter pattern as elements**: Package pages get `package_id`, `notes_hash` frontmatter when `--api-port` is set. No `status`, `status_options`, or `ea_hash` fields — those are element-only.
- **Batch write-back supports package notes**: The `WriteBackScanner` detects `package_id` in frontmatter and routes notes changes to `reader.UpdatePackageNotes()`, separate from the element `ea_id` path.

## Deployment

- **Production target is local server only** — GitHub Pages was considered but ruled out: the write-back server requires a running Windows machine with EA installed. The wiki is served locally via MkDocs. GitHub Pages may still be used for publishing a read-only snapshot.
- **GitHub Pages** at `https://hvroosmalen-eaxpertise.github.io/EAxWiki/` — used for the test/demo model only.
- **CI workflow** (`.github/workflows/mkdocs-deploy.yml`) has `permissions: contents: write` on the GITHUB_TOKEN for pushing to `gh-pages`.
- **Export cannot run in CI** — EA COM Interop is Windows-only with a running EA instance. The CI workflow only builds MkDocs from the pre-generated `wiki/` folder.
