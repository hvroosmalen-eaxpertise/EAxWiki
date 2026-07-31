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
└── ScriptTemplateIntegrityTests.cs      # NEW — source-template integrity checks
```

Shared helpers (`InMemoryWriter`, exporter fixture) already exist in `ExportIntegrationTests.cs`; the new class reuses them rather than duplicating setup.

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
3. The orphaned `wiki/ai-suggest.js` in the output dir is cleaned up by the existing `InfrastructureWriter.CleanupOrphanedFilesAsync` on the next export.

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
