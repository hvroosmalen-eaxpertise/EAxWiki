# Design: Script Template Integrity Verification

**Date:** 2026-07-31
**Status:** Proposed
**Scope:** Exporter script-template regression protection for EAxWiki

## Problem

The exporter embeds several browser-side scripts as C# raw-string literals in `InfrastructureWriter.cs` (notes editor, status editor, row-notes editor, graph init) plus a CSS resource (`extra.css`) and a vendored library (`cytoscape.min.js`). A previous regression removed the AI Suggest button from the source template and added it only to a generated file — the Suggest button silently disappeared from the wiki until it was manually noticed. There is no automated check that these core functions still exist in the source before an export runs.

## Solution

A new xUnit test class `ScriptTemplateIntegrityTests` in `EAxWiki.Tests` that runs the exporter via the existing `InMemoryWriter` + `MarkdownExporter` pattern (no EA/COM needed), reads each generated script from the in-memory output, and asserts it contains the key function markers that constitute its core behavior. Runs with plain `dotnet test`; a regression like the Suggest-button removal fails the suite immediately.

## Architecture

```
EAxWiki.Tests/
├── ExportIntegrationTests.cs            # Existing exporter integration tests
├── TestInMemoryWriter.cs                # NEW — shared in-memory IOutputWriter
└── ScriptTemplateIntegrityTests.cs      # NEW — source-template integrity checks
```

`InMemoryWriter` is currently a private nested class inside `ExportIntegrationTests`. Extract it into a shared `TestInMemoryWriter` class in `EAxWiki.Tests` (same namespace) so both test classes can construct the exporter the same way; update `ExportIntegrationTests` to use the shared class. Both test classes build a `MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance)` and run `ExportAsync` against a minimal in-memory repo, then inspect `writer.Files`.

## Checks

| Test method | File asserted | Required markers (all must be present) |
|---|---|---|
| `NotesEditorScript_ContainsCoreFunctions` | `notes-editor.js` | `initNotesEditor`, `suggestBtn`, `ea-notes-suggest-btn`, `/api/ai-suggest`, `acquireEditLock` |
| `StatusEditorScript_ContainsCoreFunctions` | `status-editor.js` | `initStatusEditor`, `/api/status` |
| `RowNotesEditorScript_ContainsCoreFunctions` | `row-notes-editor.js` | `initRowNotesEditors`, `openEditor`, `/api/row-notes` |
| `GraphInitScript_ContainsCoreFunctions` | `graph-init.js` | `initEaGraph`, `cytoscape` |
| `ExtraCss_ContainsCoreStyles` | `extra.css` | `.ea-notes-editor`, `.ea-notes-suggest-btn`, `.ea-status-editor` |
| `CytoscapeMinJs_IsEmitted` | `cytoscape.min.js` | file exists (existence only) |
| `AiSuggestJs_IsNotEmitted` | `ai-suggest.js` | file absent (pairs with stub removal below) |

### Marker semantics

- Markers are stable identifiers (function names, API endpoint paths, CSS selectors, button ids) rather than full-script equality — resilient to intentional formatting changes while catching removal of the underlying feature.
- Each marker is asserted individually with `Assert.Contains(marker, content)` so a failure names the exact missing marker.
- A missing file fails with a descriptive message listing which file was expected.

## Source changes (in service of the verification)

1. **Remove `WriteAiSuggestScriptAsync`** from `InfrastructureWriter.cs` (currently writes an empty `(function () { 'use strict'; })();` stub — dead code, not referenced by `mkdocs.yml`, and `mkdocs.yml`'s `extra_javascript` was already updated to drop `ai-suggest.js`).
2. **Remove its call** from `MarkdownExporter.cs`.
3. **Delete the stale git-tracked `wiki/ai-suggest.js`** from the output dir manually as part of this change. Note: `CleanupOrphanedFilesAsync` only removes orphaned `*.md` element files and package directories — it does not clean up root-level `.js` files, so the stale file must be removed by the plan, not by the next export.

### Behavior

- No runtime change: the AI Suggest functionality lives in `notes-editor.js` and remains fully covered by the new notes-editor test.
- `AiSuggestJs_IsNotEmitted` guards against the stub being re-introduced.

## Not in scope

- `Validate-WikiOutput.ps1` (wiki-validation skill) — already covers generated-output checks (page structure, API health, round-trips). This design targets the *source* side and is complementary.
- Golden-file snapshot comparisons — too brittle; marker-based checks chosen instead.
- Changes to the individual script implementations themselves.

## Testing

- The new test class is the verification; run via `dotnet test` alongside the existing suite (261 tests currently green).
- Manual sanity: run `dotnet test`, confirm new class passes; temporarily delete a marker (e.g. `suggestBtn`) in the source and confirm the corresponding test fails; restore it.

## Success Criteria

- `dotnet test` reports the new class green on unchanged source.
- Removing any marker from a source template makes exactly the corresponding test fail.
- `ai-suggest.js` no longer produced by export; `AiSuggestJs_IsNotEmitted` passes.
- Existing 261 tests remain green (268 total after this change).
