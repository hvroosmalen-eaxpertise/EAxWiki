# Design Decisions

## Architecture

- **Export pipeline**: C# .NET 10 console app reads EA model via COM Interop, writes Markdown + PNG files, then MkDocs serves the result. Three separate scripts for flexibility.
- **No test project**: The project doesn't include automated tests. Verification is done by inspecting the generated wiki.

## File Naming

- **`SanitizeName()`** for all file/folder names derived from user content (package names, element names, diagram names). Caches sanitized names in a `ConcurrentDictionary` to avoid repeated work.
- **Leading/trailing whitespace** in element names is trimmed via `SanitizeName().Trim()` to prevent link-to-file mismatches in the EA model.
- **`#` in filenames** is replaced with `_` because MkDocs interprets `#` as an anchor delimiter.

## Navigation

- **Three views**: Structure (tree hierarchy), Types (grouped by stereotype), Diagrams (global alphabetical index).
- **awesome-pages MkDocs plugin** controls the nav. Root `.pages` uses `Structure: ''` + `Diagrams: diagrams/` + `Types: types/` format.
- **Breadcrumb** is shown on element pages using parent-package links.

## Export

- **Output directory is cleaned** before each export run (`Directory.Delete(recursive: true)`). No incremental/differential export.
- **Relative links** use `../` prefix and forward slashes, computed via `Path.GetRelativePath`.
- **Parallel element page export**: `Task.WhenAll` over the element loop, with sequential index line building (safe since `List.Add` on the same thread). Biggest performance gain with minimal code change.
- **Duplicate sanitized filenames** (e.g. `unnamed.md` when multiple elements have empty names) are handled by per-file `SemaphoreSlim` locking in `FileOutputWriter` to prevent `IOException`.

## Diagrams

- **Format**: PNG via EA COM `Project.PutDiagramImageToFile`.
- **Page location**: `diagrams/{Name}.md` per diagram, inside their package subfolder.
- **Global index**: `diagrams/index.md` with a table (Diagram | Modified | Description | Path), sorted by breadcrumb path then diagram name.
- **Layout on diagram pages**: description text below the diagram image.
- **Diagram Status was dropped** — the `t_diagram.Status` column does not exist in this EA schema version.
- **`SELECT *` instead of `SELECT Status`** to avoid EA schema validation errors.
- **RPC_E_SERVERFAULT** for some diagrams during PNG export; isolated by per-diagram try-catch.

## Error Handling

- **Three-layer COM isolation**: per-diagram try-catch → per-package `ExportDiagramsAsync` call → entire `ExportAsync` body.
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

## Deployment

- **GitHub Pages** at `https://hvroosmalen-eaxpertise.github.io/EAxWiki/` — requires a public repo (or paid GitHub plan).
- **CI workflow** (`.github/workflows/mkdocs-deploy.yml`) has `permissions: contents: write` on the GITHUB_TOKEN for pushing to `gh-pages`.
- **Export cannot run in CI** — EA COM Interop is Windows-only with a running EA instance. The CI workflow only builds MkDocs from the pre-generated `wiki/` folder.

## Infrastructure

- **`FileOutputWriter`** implements `IOutputWriter` interface, writes files asynchronously with per-file `SemaphoreSlim` locking.
- **`Config` class** exposes a `Verbose` property bound to the `--verbose` / `-v` command-line flag.
- **`EaReader`** reads the EA model via COM, exposing `MapDiagram`, `ExportDiagramImage`, and `RepositoryPath` methods.
