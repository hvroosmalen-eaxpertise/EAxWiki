---
name: wiki-validation
description: >-
  Use when testing wiki output, validating exports, checking wiki pages,
  running wiki tests, verifying export completeness, or performing wiki
  health checks. Triggers on: "test wiki output", "validate wiki",
  "check wiki pages", "run wiki tests", "verify export", "wiki health check",
  "are the wiki pages working", "did the export succeed".
---

# Wiki Validation Skill

## Overview

Validates EAxWiki output: page structure, diagram thumbnails, relationship graphs,
service health, and API integration.

## Usage

Run the validation script:

```powershell
# Basic validation (static checks only)
\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1

# Full validation with API integration tests
\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -TestElementId 462 -AiEndpoint "http://localhost:11434"

# Watch mode (continuous monitoring)
\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -Mode Watch -WatchIntervalSec 30

# JSON output
\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -OutputJson "validation-report.json"
```

## Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `-SitePath` | `./site` | Path to built mkdocs site |
| `-WikiPath` | `./wiki` | Path to wiki source |
| `-Mode` | `Once` | `Once` or `Watch` |
| `-WatchIntervalSec` | `30` | Seconds between checks in watch mode |
| `-OutputJson` | (none) | Path to JSON report |
| `-Threshold` | `100` | Exit 1 if failures exceed this |
| `-ApiBase` | `http://127.0.0.1:8001` | Writeback API base URL |
| `-TestElementId` | (none) | Element ID for API tests |
| `-TestDiagramId` | (none) | Diagram ID for diagram suggest |
| `-AiEndpoint` | (none) | AI endpoint for suggest test |
| `-SkipApi` | false | Skip API integration checks |
| `-VerboseOutput` | false | Detailed per-file output |

## Remediation

| Failure | Fix |
|---------|-----|
| `graph-index-exists` fails | Re-export: `dotnet exec src/EAxWiki/bin/Debug/net10.0/EAxWiki.dll --force` |
| `diagram-png-existence` fails | Check EA connection, re-export |
| `ea-graph-container` missing | Check `ElementPageWriter.cs` template |
| `notes-editor` missing | Add `notes-editor.js` to `mkdocs.yml` extra_javascript |
| `mkdocs-serve-port` fails | Run `mkdocs serve --dev-addr 127.0.0.1:8000` |
| `api-healthz` fails | Check EA running, verify `--api-port` |
| `api-status-roundtrip` fails | Check `EaReader.UpdateElementStatus()`, `FrontmatterParser` |
| `api-ai-suggest` fails | Set `--ai-endpoint`, verify LLM running |
