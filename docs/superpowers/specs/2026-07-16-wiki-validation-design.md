# Design: Wiki Validation Skill

**Date:** 2026-07-16
**Status:** Approved
**Scope:** Full pipeline + services validation for EAxWiki

## Problem

After exporting the EA model to MkDocs wiki, there is no automated way to verify that all pages render correctly — that diagram thumbnails load, the relationship graph initializes, the notes editor is present, and services are healthy. Currently this is done manually by inspecting HTML files and checking ports, which is slow and error-prone.

## Solution

A PowerShell validation script (`Validate-WikiOutput.ps1`) paired with a skill (`wiki-validation`) that guides the AI agent through running it and interpreting results. The script handles all deterministic checks; the skill provides workflow context and remediation guidance.

## Architecture

```
wiki-validation/
├── SKILL.md                          # Skill definition
├── scripts/
│   └── Validate-WikiOutput.ps1       # Validation script
└── references/
    └── page-checks.md                # Detailed check documentation
```

## Validation Script: `Validate-WikiOutput.ps1`

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-SitePath` | String | `./site` | Path to built mkdocs site directory |
| `-WikiPath` | String | `./wiki` | Path to wiki source directory |
| `-Mode` | String | `Once` | `Once` for single run, `Watch` for continuous |
| `-WatchIntervalSec` | Int | 30 | Seconds between checks in watch mode |
| `-OutputJson` | String | (none) | Path to write JSON report file |
| `-Threshold` | Int | 100 | Exit code 1 if failures exceed this count |
| `-Verbose` | Switch | false | Show detailed per-file output |

### Check Categories

#### 1. Infrastructure Checks

| Check | Description |
|-------|-------------|
| `graph-index-exists` | `graph-index.json` exists in site root |
| `graph-index-valid` | JSON parses successfully, has `nodes` and `edges` arrays |
| `graph-index-population` | `nodes.Count > 0` and `edges.Count > 0` |
| `diagram-png-existence` | All `<img>` src attributes in `diagram-thumb` links resolve to existing PNG files |

#### 2. Page Validation

Iterate all `.html` files under `SitePath` excluding:
- `types/` subdirectories (metamodel type pages — different template)
- `assets/` (CSS/JS/images)
- `404.html`
- `index.html` files (index pages use different template)
- `glossary/`, `recent/`, `status/` (special pages)

For each remaining element page, verify:

| Check | Selector/Pattern | Description |
|-------|-----------------|-------------|
| `ea-graph-container` | `id="ea-graph-container"` | Graph container div present |
| `data-focal-id` | `data-focal-id="\d+"` | Focal element ID set on container |
| `ea-notes` | `ea-notes-editor` or `ea-notes-content` or `ea-notes` class | Notes section present (element pages use `#ea-notes-editor` widget; diagram pages use `.ea-notes-content`) |
| `notes-editor` | `notes-editor\.js` in script tags | Notes editor JS loaded |
| `graph-init` | `graph-init\.js` in script tags | Graph init JS loaded |
| `cytoscape` | `cytoscape` in script tags | Cytoscape library loaded |
| `diagram-thumbs` | `class="diagram-thumbs"` | Diagram thumbnail section present |

#### 3. Service Health Checks

| Check | Description |
|-------|-------------|
| `mkdocs-serve-port` | Port 8000 is in LISTENING state |
| `mkdocs-serve-http` | HTTP GET to `http://127.0.0.1:8000` returns 200 |
| `writeback-api-port` | Port 8001 is in LISTENING state |
| `ea-process-cleanup` | No `EA.exe` processes running (exit code warning, not failure) |

#### 4. Type Page Validation (informational)

Count type definition pages under `types/` and verify they do NOT have `ea-graph-container` (confirming different template is applied correctly). This is informational — mismatches are reported but not counted as failures.

### Output Format

#### Console (default)

```
[PASS] graph-index-exists: graph-index.json found (136,721 bytes)
[PASS] graph-index-valid: 451 nodes, 592 edges
[FAIL] diagram-png-existence: 2 missing PNGs
  MISSING: site/Some Path/diagrams/Some Diagram.png
  MISSING: site/Other Path/diagrams/Other.png
[PASS] Assessments/CO₂ Reduction %.html: 7/7 checks passed (3 thumbs)
[PASS] Asset/Emission Sources.html: 7/7 checks passed (2 thumbs)
...
[WARN] ea-process-cleanup: 1 EA.exe process still running

=== SUMMARY ===
Pages: 451 passed, 0 failed, 32 skipped (type pages)
Infrastructure: 2 passed, 0 failed
Services: 2 passed, 0 failed, 1 warning
Diagram PNGs: 43/43 exist
Total: 455 passed, 0 failed, 1 warning
```

Colors: `[PASS]` = green, `[FAIL]` = red, `[WARN]` = yellow, `[SKIP]` = gray

#### JSON (with `-OutputJson`)

```json
{
  "timestamp": "2026-07-16T18:05:48Z",
  "mode": "once",
  "summary": {
    "pages_passed": 451,
    "pages_failed": 0,
    "pages_skipped": 32,
    "infrastructure_passed": 2,
    "infrastructure_failed": 0,
    "services_passed": 2,
    "services_failed": 0,
    "services_warnings": 1,
    "diagram_pngs_total": 43,
    "diagram_pngs_missing": 0,
    "total_passed": 455,
    "total_failed": 0,
    "total_warnings": 1
  },
  "checks": [
    {
      "category": "infrastructure",
      "name": "graph-index-exists",
      "status": "pass",
      "detail": "graph-index.json found (136,721 bytes)"
    },
    {
      "category": "page",
      "file": "Assessments/CO₂ Reduction %.html",
      "checks_passed": 7,
      "checks_failed": 0,
      "diagram_thumbs": 3,
      "focal_id": 462,
      "details": []
    }
  ]
}
```

### Watch Mode

When `-Mode Watch` is specified:
1. Run all checks once
2. Print report
3. Sleep for `-WatchIntervalSec` seconds
4. Repeat from step 1
5. On state change (new failure or recovery), print a delta summary
6. Exit on Ctrl+C

## Skill: `wiki-validation`

### Trigger Phrases

- "test wiki output", "validate wiki", "check wiki pages"
- "run wiki tests", "verify export", "wiki health check"
- "are the wiki pages working", "did the export succeed"

### Workflow

1. Determine the site path (check for `./site` or ask user)
2. Run `Validate-WikiOutput.ps1` with appropriate parameters
3. If failures found, offer to diagnose and fix specific issues
4. If watch mode requested, start continuous monitoring
5. Report summary with pass/fail counts

### Remediation Guidance

The skill includes common failure patterns and fixes:

| Failure | Likely Cause | Fix |
|---------|-------------|-----|
| `graph-index-exists` fails | Export didn't run or GraphIndexExporter not wired | Run `dotnet run --project src/EAxWiki -- --force` |
| `diagram-png-existence` fails | EA diagram export failed | Check EA connection, re-export with `--force` |
| `ea-graph-container` missing on element page | ElementPageWriter not emitting container | Check `ElementPageWriter.cs` output template |
| `notes-editor` missing | mkdocs.yml missing `notes-editor.js` in extra_javascript | Add to `mkdocs.yml` |
| `mkdocs-serve-port` fails | mkdocs serve not running | Run `mkdocs serve --dev-addr 127.0.0.1:8000` |
| `ea-process-cleanup` warns | EA COM objects not released | Check `EaReader.cs` for `Marshal.ReleaseComObject` |

## Testing the Skill

### Test Cases

1. **Fresh export validation** — Run after a clean export, expect all checks pass
2. **Missing diagram PNG** — Delete a diagram PNG, expect `diagram-png-existence` failure
3. **Missing graph container** — Edit an HTML file to remove `ea-graph-container`, expect page failure
4. **Port down** — Stop mkdocs serve, expect service check failures
5. **Watch mode** — Start in watch mode, verify it re-runs on interval

### Success Criteria

- All 451 element pages validated in under 30 seconds
- JSON output matches schema
- Watch mode detects state changes within one interval
- Colored console output is readable and actionable
