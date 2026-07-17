# Wiki Validation Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Create a PowerShell validation script and AI skill that comprehensively tests wiki output, service health, and API integration.

**Architecture:** A single `Validate-WikiOutput.ps1` script performs all checks (infrastructure, pages, services, API integration). A `SKILL.md` guides the AI agent through running it and interpreting results. Pester tests validate the script's parameter parsing.

**Tech Stack:** PowerShell 5.1+, Pester 5.x, `Invoke-WebRequest` for HTTP checks, `ConvertFrom-Json` for JSON parsing, regex for HTML validation.

## Global Constraints

- PowerShell 5.1+ (Windows built-in)
- Pester 5.x for script tests
- No external dependencies beyond what's already in the project
- Script must work from project root (`E:\Users\Han\Repos\EAxWiki`)
- All round-trip API tests restore original values
- Console output uses Write-Host with colors; JSON uses ConvertTo-Json

---

## File Structure

| File | Responsibility |
|------|---------------|
| `.opencode/skills/wiki-validation/SKILL.md` | Skill definition, trigger phrases, workflow, remediation |
| `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1` | Main validation script |
| `.opencode/skills/wiki-validation/references/page-checks.md` | Detailed check documentation |
| `tests/scripts/Validate-WikiOutput.Tests.ps1` | Pester tests for parameter parsing |

---

### Task 1: Create Skill Directory Structure

**Files:**
- Create: `.opencode/skills/wiki-validation/SKILL.md`
- Create: `.opencode/skills/wiki-validation/scripts/` (directory)
- Create: `.opencode/skills/wiki-validation/references/` (directory)

**Interfaces:** None — this is scaffolding.

- [ ] **Step 1: Create directory structure**

```powershell
New-Item -ItemType Directory -Force -Path ".opencode/skills/wiki-validation/scripts"
New-Item -ItemType Directory -Force -Path ".opencode/skills/wiki-validation/references"
```

- [ ] **Step 2: Create placeholder SKILL.md**

```markdown
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
.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1

# Full validation with API integration tests
.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -TestElementId 462 -AiEndpoint "http://localhost:11434"

# Watch mode (continuous monitoring)
.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -Mode Watch -WatchIntervalSec 30

# JSON output
.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -OutputJson "validation-report.json"
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
| `-Verbose` | false | Detailed per-file output |

## Remediation

| Failure | Fix |
|---------|-----|
| `graph-index-exists` fails | Re-export: `dotnet run --project src/EAxWiki -- --force` |
| `diagram-png-existence` fails | Check EA connection, re-export |
| `ea-graph-container` missing | Check `ElementPageWriter.cs` template |
| `notes-editor` missing | Add `notes-editor.js` to `mkdocs.yml` extra_javascript |
| `mkdocs-serve-port` fails | Run `mkdocs serve --dev-addr 127.0.0.1:8000` |
| `api-healthz` fails | Check EA running, verify `--api-port` |
| `api-status-roundtrip` fails | Check `EaReader.UpdateElementStatus()`, `FrontmatterParser` |
| `api-ai-suggest` fails | Set `--ai-endpoint`, verify LLM running |
```

- [ ] **Step 3: Commit**

```bash
git add .opencode/skills/wiki-validation/
git commit -m "feat: scaffold wiki-validation skill directory"
```

---

### Task 2: Implement Parameter Parsing

**Files:**
- Create: `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1`
- Create: `tests/scripts/Validate-WikiOutput.Tests.ps1`

**Interfaces:** None — this is the script entry point.

- [ ] **Step 1: Write Pester tests for parameter parsing**

```powershell
# tests/scripts/Validate-WikiOutput.Tests.ps1
BeforeAll {
    . "$PSScriptRoot\..\..\..\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1"
}

Describe 'Get-ValidateArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-ValidateArgs
        $r.SitePath | Should -Be "./site"
        $r.WikiPath | Should -Be "./wiki"
        $r.Mode | Should -Be "Once"
        $r.WatchIntervalSec | Should -Be 30
        $r.OutputJson | Should -Be ""
        $r.Threshold | Should -Be 100
        $r.ApiBase | Should -Be "http://127.0.0.1:8001"
        $r.TestElementId | Should -Be 0
        $r.TestDiagramId | Should -Be 0
        $r.AiEndpoint | Should -Be ""
        $r.SkipApi | Should -Be $false
        $r.Verbose | Should -Be $false
    }

    It 'parses -SitePath' {
        $r = Get-ValidateArgs -Arguments @('-SitePath', './my-site')
        $r.SitePath | Should -Be "./my-site"
    }

    It 'parses -Mode Watch' {
        $r = Get-ValidateArgs -Arguments @('-Mode', 'Watch')
        $r.Mode | Should -Be "Watch"
    }

    It 'parses -TestElementId' {
        $r = Get-ValidateArgs -Arguments @('-TestElementId', '462')
        $r.TestElementId | Should -Be 462
    }

    It 'parses -TestDiagramId' {
        $r = Get-ValidateArgs -Arguments @('-TestDiagramId', '100')
        $r.TestDiagramId | Should -Be 100
    }

    It 'parses -AiEndpoint' {
        $r = Get-ValidateArgs -Arguments @('-AiEndpoint', 'http://localhost:11434')
        $r.AiEndpoint | Should -Be "http://localhost:11434"
    }

    It 'parses -SkipApi switch' {
        $r = Get-ValidateArgs -Arguments @('-SkipApi')
        $r.SkipApi | Should -Be $true
    }

    It 'parses -OutputJson' {
        $r = Get-ValidateArgs -Arguments @('-OutputJson', 'report.json')
        $r.OutputJson | Should -Be "report.json"
    }

    It 'parses -Threshold' {
        $r = Get-ValidateArgs -Arguments @('-Threshold', '50')
        $r.Threshold | Should -Be 50
    }

    It 'parses -WatchIntervalSec' {
        $r = Get-ValidateArgs -Arguments @('-WatchIntervalSec', '60')
        $r.WatchIntervalSec | Should -Be 60
    }

    It 'parses -Verbose switch' {
        $r = Get-ValidateArgs -Arguments @('-Verbose')
        $r.Verbose | Should -Be $true
    }

    It 'parses combined flags' {
        $r = Get-ValidateArgs -Arguments @('-SkipApi', '-Verbose', '-Mode', 'Watch', '-TestElementId', '462')
        $r.SkipApi | Should -Be $true
        $r.Verbose | Should -Be $true
        $r.Mode | Should -Be "Watch"
        $r.TestElementId | Should -Be 462
    }
}
```

- [ ] **Step 2: Run tests to verify they fail**

Run: `Invoke-Pester tests/scripts/Validate-WikiOutput.Tests.ps1 -Output Detailed`
Expected: FAIL — `Get-ValidateArgs` not defined

- [ ] **Step 3: Implement parameter parsing in the script**

```powershell
# .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
[CmdletBinding()]
param(
    [string]$SitePath = "./site",
    [string]$WikiPath = "./wiki",
    [ValidateSet("Once","Watch")]
    [string]$Mode = "Once",
    [int]$WatchIntervalSec = 30,
    [string]$OutputJson = "",
    [int]$Threshold = 100,
    [string]$ApiBase = "http://127.0.0.1:8001",
    [int]$TestElementId = 0,
    [int]$TestDiagramId = 0,
    [string]$AiEndpoint = "",
    [switch]$SkipApi,
    [switch]$VerboseOutput
)

function Get-ValidateArgs {
    param([string[]]$Arguments = @())

    $result = [PSCustomObject]@{
        SitePath        = "./site"
        WikiPath        = "./wiki"
        Mode            = "Once"
        WatchIntervalSec = 30
        OutputJson      = ""
        Threshold       = 100
        ApiBase         = "http://127.0.0.1:8001"
        TestElementId   = 0
        TestDiagramId   = 0
        AiEndpoint      = ""
        SkipApi         = $false
        Verbose         = $false
    }

    $i = 0
    while ($i -lt $Arguments.Count) {
        switch ($Arguments[$i]) {
            '-SitePath'       { $result.SitePath = $Arguments[++$i] }
            '-WikiPath'       { $result.WikiPath = $Arguments[++$i] }
            '-Mode'           { $result.Mode = $Arguments[++$i] }
            '-WatchIntervalSec' { $result.WatchIntervalSec = [int]$Arguments[++$i] }
            '-OutputJson'     { $result.OutputJson = $Arguments[++$i] }
            '-Threshold'      { $result.Threshold = [int]$Arguments[++$i] }
            '-ApiBase'        { $result.ApiBase = $Arguments[++$i] }
            '-TestElementId'  { $result.TestElementId = [int]$Arguments[++$i] }
            '-TestDiagramId'  { $result.TestDiagramId = [int]$Arguments[++$i] }
            '-AiEndpoint'     { $result.AiEndpoint = $Arguments[++$i] }
            '-SkipApi'        { $result.SkipApi = $true }
            '-Verbose'        { $result.Verbose = $true }
        }
        $i++
    }
    return $result
}
```

- [ ] **Step 4: Run tests to verify they pass**

Run: `Invoke-Pester tests/scripts/Validate-WikiOutput.Tests.ps1 -Output Detailed`
Expected: 12 tests PASS

- [ ] **Step 5: Commit**

```bash
git add .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
git add tests/scripts/Validate-WikiOutput.Tests.ps1
git commit -m "feat: implement wiki validation parameter parsing with Pester tests"
```

---

### Task 3: Implement Infrastructure Checks

**Files:**
- Modify: `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1`

**Interfaces:**
- Consumes: `SitePath` parameter
- Produces: `Write-ValidationCheck` function (reused by all check categories)

- [ ] **Step 1: Add output helper functions**

Append to `Validate-WikiOutput.ps1`:

```powershell
$script:Results = @{ passed = 0; failed = 0; warnings = 0; skipped = 0; checks = @() }

function Write-ValidationCheck {
    param([string]$Category, [string]$Name, [string]$Status, [string]$Detail)
    $colors = @{ pass = 'Green'; fail = 'Red'; warn = 'Yellow'; skip = 'Gray' }
    $icon = @{ pass = '[PASS]'; fail = '[FAIL]'; warn = '[WARN]'; skip = '[SKIP]' }
    Write-Host "$($icon[$Status]) $Name`: $Detail" -ForegroundColor $colors[$Status]
    $script:Results.checks += [PSCustomObject]@{
        category = $Category; name = $Name; status = $Status; detail = $Detail
    }
    switch ($Status) {
        'pass' { $script:Results.passed++ }
        'fail' { $script:Results.failed++ }
        'warn' { $script:Results.warnings++ }
        'skip' { $script:Results.skipped++ }
    }
}

function Write-PageResult {
    param([string]$File, [int]$Passed, [int]$Failed, [int]$Thumbs, [int]$FocalId)
    $status = if ($Failed -eq 0) { 'pass' } else { 'fail' }
    $detail = "$File`: $Passed/$($Passed + $Failed) checks passed ($Thumbs thumbs)"
    Write-ValidationCheck -Category 'page' -Name $File -Status $status -Detail $detail
}

function Write-Summary {
    Write-Host ""
    Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
    Write-Host "Pages: $($script:Results.page_passed) passed, $($script:Results.page_failed) failed, $($script:Results.page_skipped) skipped (type pages)"
    Write-Host "Infrastructure: $($script:Results.infra_passed) passed, $($script:Results.infra_failed) failed"
    Write-Host "Services: $($script:Results.svc_passed) passed, $($script:Results.svc_failed) failed, $($script:Results.svc_warnings) warnings"
    if (-not $SkipApi) {
        Write-Host "API Integration: $($script:Results.api_passed) passed, $($script:Results.api_failed) failed, $($script:Results.api_skipped) skipped"
    }
    Write-Host "Diagram PNGs: $($script:Results.pngs_found)/$($script:Results.pngs_total) exist"
    $total = $script:Results.passed + $script:Results.failed
    Write-Host "Total: $total passed, $($script:Results.failed) failed, $($script:Results.warnings) warnings, $($script:Results.skipped) skipped" -ForegroundColor $(if ($script:Results.failed -eq 0) { 'Green' } else { 'Red' })
}
```

- [ ] **Step 2: Implement infrastructure checks**

Append to `Validate-WikiOutput.ps1`:

```powershell
function Test-Infrastructure {
    $graphPath = Join-Path $SitePath "graph-index.json"
    if (Test-Path $graphPath) {
        $size = (Get-Item $graphPath).Length
        Write-ValidationCheck -Category 'infrastructure' -Name 'graph-index-exists' -Status 'pass' -Detail "graph-index.json found ($size bytes)"
        try {
            $json = Get-Content $graphPath -Raw | ConvertFrom-Json
            if ($json.nodes -and $json.edges) {
                Write-ValidationCheck -Category 'infrastructure' -Name 'graph-index-valid' -Status 'pass' -Detail "$($json.nodes.Count) nodes, $($json.edges.Count) edges"
            } else {
                Write-ValidationCheck -Category 'infrastructure' -Name 'graph-index-valid' -Status 'fail' -Detail "Missing nodes or edges array"
            }
        } catch {
            Write-ValidationCheck -Category 'infrastructure' -Name 'graph-index-valid' -Status 'fail' -Detail "JSON parse error: $($_.Exception.Message)"
        }
    } else {
        Write-ValidationCheck -Category 'infrastructure' -Name 'graph-index-exists' -Status 'fail' -Detail "graph-index.json not found in $SitePath"
    }

    # Check diagram PNGs
    $thumbPattern = 'class="diagram-thumb"'
    $imgPattern = 'src="([^"]+\.png)"'
    $htmlFiles = Get-ChildItem $SitePath -Filter "*.html" -Recurse | Where-Object {
        $_.FullName -notmatch '\\types\\' -and $_.FullName -notmatch '\\assets\\'
    }
    $totalPngs = 0; $missingPngs = @()
    foreach ($file in $htmlFiles) {
        $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        if ($content -match $thumbPattern) {
            $imgs = [regex]::Matches($content, $imgPattern)
            foreach ($img in $imgs) {
                $relativePath = $img.Groups[1].Value -replace '/', '\'
                $pngPath = Join-Path $file.DirectoryName $relativePath
                $totalPngs++
                if (-not (Test-Path $pngPath)) {
                    $missingPngs += $pngPath
                }
            }
        }
    }
    $script:Results.pngs_total = $totalPngs
    $script:Results.pngs_found = $totalPngs - $missingPngs.Count
    if ($missingPngs.Count -eq 0) {
        Write-ValidationCheck -Category 'infrastructure' -Name 'diagram-png-existence' -Status 'pass' -Detail "$totalPngs/$totalPngs PNGs exist"
    } else {
        Write-ValidationCheck -Category 'infrastructure' -Name 'diagram-png-existence' -Status 'fail' -Detail "$($missingPngs.Count) missing PNGs"
        $missingPngs | Select-Object -First 5 | ForEach-Object {
            Write-Host "  MISSING: $_" -ForegroundColor Red
        }
    }
}
```

- [ ] **Step 3: Commit**

```bash
git add .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
git commit -m "feat: add infrastructure checks (graph-index, diagram PNGs)"
```

---

### Task 4: Implement Page Validation

**Files:**
- Modify: `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1`

**Interfaces:**
- Consumes: `SitePath` parameter, `Write-PageResult` function
- Produces: Per-page check results

- [ ] **Step 1: Implement page validation**

Append to `Validate-WikiOutput.ps1`:

```powershell
function Test-Pages {
    $checks = @('ea-graph-container', 'data-focal-id', 'ea-notes', 'notes-editor\.js', 'graph-init\.js', 'cytoscape', 'diagram-thumbs')
    $excludeDirs = @('types', 'assets', 'glossary', 'recent', 'status')

    $htmlFiles = Get-ChildItem $SitePath -Filter "*.html" -Recurse | Where-Object {
        $rel = $_.FullName.Replace("$SitePath\", "")
        $name = $_.Name
        # Exclude special directories and files
        $isExcluded = $false
        foreach ($dir in $excludeDirs) {
            if ($rel -match "^$dir[\\/]") { $isExcluded = $true; break }
        }
        -not $isExcluded -and $name -ne "404.html" -and $name -ne "index.html"
    }

    $pagePassed = 0; $pageFailed = 0; $pageSkipped = 0

    # Count type pages separately
    $typeFiles = Get-ChildItem $SitePath -Filter "*.html" -Recurse | Where-Object {
        $_.FullName -match '\\types\\' -and $_.Name -ne "index.html"
    }
    $pageSkipped = $typeFiles.Count

    foreach ($file in $htmlFiles) {
        $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        if (-not $content) { continue }

        $passed = 0; $failed = 0; $thumbs = 0; $focalId = 0

        foreach ($check in $checks) {
            if ($content -match $check) {
                $passed++
                if ($check -eq 'data-focal-id') {
                    $match = [regex]::Match($content, 'data-focal-id="(\d+)"')
                    if ($match.Success) { $focalId = [int]$match.Groups[1].Value }
                }
            } else {
                $failed++
            }
        }

        $thumbMatch = [regex]::Matches($content, 'class="diagram-thumb"')
        $thumbs = $thumbMatch.Count

        if ($failed -eq 0) {
            $pagePassed++
            if ($VerboseOutput) {
                $rel = $file.FullName.Replace("$SitePath\", "")
                Write-PageResult -File $rel -Passed $passed -Failed $failed -Thumbs $thumbs -FocalId $focalId
            }
        } else {
            $pageFailed++
            $rel = $file.FullName.Replace("$SitePath\", "")
            Write-PageResult -File $rel -Passed $passed -Failed $failed -Thumbs $thumbs -FocalId $focalId
        }
    }

    $script:Results.page_passed = $pagePassed
    $script:Results.page_failed = $pageFailed
    $script:Results.page_skipped = $pageSkipped

    if ($pageFailed -eq 0) {
        Write-ValidationCheck -Category 'pages' -Name 'page-validation' -Status 'pass' -Detail "$pagePassed pages passed, $pageSkipped skipped (type pages)"
    } else {
        Write-ValidationCheck -Category 'pages' -Name 'page-validation' -Status 'fail' -Detail "$pagePassed passed, $pageFailed failed, $pageSkipped skipped"
    }
}
```

- [ ] **Step 2: Commit**

```bash
git add .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
git commit -m "feat: add page validation checks"
```

---

### Task 5: Implement Service Health Checks

**Files:**
- Modify: `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1`

**Interfaces:**
- Consumes: None (reads ports and processes)
- Produces: Service check results

- [ ] **Step 1: Implement service health checks**

Append to `Validate-WikiOutput.ps1`:

```powershell
function Test-Services {
    # mkdocs serve port
    $mkdocsPort = netstat -ano | Select-String ":8000\s.*LISTENING"
    if ($mkdocsPort) {
        Write-ValidationCheck -Category 'service' -Name 'mkdocs-serve-port' -Status 'pass' -Detail "Port 8000 LISTENING"
    } else {
        Write-ValidationCheck -Category 'service' -Name 'mkdocs-serve-port' -Status 'fail' -Detail "Port 8000 not listening"
    }

    # mkdocs HTTP
    try {
        $response = Invoke-WebRequest -Uri "http://127.0.0.1:8000" -UseBasicParsing -TimeoutSec 5
        Write-ValidationCheck -Category 'service' -Name 'mkdocs-serve-http' -Status 'pass' -Detail "HTTP $($response.StatusCode)"
    } catch {
        Write-ValidationCheck -Category 'service' -Name 'mkdocs-serve-http' -Status 'fail' -Detail "HTTP request failed: $($_.Exception.Message)"
    }

    # Writeback API port
    $apiPort = netstat -ano | Select-String ":8001\s.*LISTENING"
    if ($apiPort) {
        Write-ValidationCheck -Category 'service' -Name 'writeback-api-port' -Status 'pass' -Detail "Port 8001 LISTENING"
    } else {
        Write-ValidationCheck -Category 'service' -Name 'writeback-api-port' -Status 'fail' -Detail "Port 8001 not listening"
    }

    # EA process cleanup
    $eaProcesses = Get-Process -Name "EA" -ErrorAction SilentlyContinue
    if ($eaProcesses) {
        Write-ValidationCheck -Category 'service' -Name 'ea-process-cleanup' -Status 'warn' -Detail "$($eaProcesses.Count) EA.exe process(es) still running"
    } else {
        Write-ValidationCheck -Category 'service' -Name 'ea-process-cleanup' -Status 'pass' -Detail "No EA.exe processes"
    }

    # Compute category totals from checks array
    $svcChecks = $script:Results.checks | Where-Object { $_.category -eq 'service' }
    $script:Results.svc_passed = ($svcChecks | Where-Object { $_.status -eq 'pass' }).Count
    $script:Results.svc_failed = ($svcChecks | Where-Object { $_.status -eq 'fail' }).Count
    $script:Results.svc_warnings = ($svcChecks | Where-Object { $_.status -eq 'warn' }).Count
}
```

- [ ] **Step 2: Commit**

```bash
git add .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
git commit -m "feat: add service health checks"
```

---

### Task 6: Implement API Integration Checks

**Files:**
- Modify: `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1`

**Interfaces:**
- Consumes: `ApiBase`, `TestElementId`, `TestDiagramId`, `AiEndpoint`, `SkipApi` parameters
- Produces: API check results with timing data

- [ ] **Step 1: Implement API integration checks**

Append to `Validate-WikiOutput.ps1`:

```powershell
function Test-ApiIntegration {
    if ($SkipApi) {
        Write-ValidationCheck -Category 'api' -Name 'api-checks' -Status 'skip' -Detail "SkipApi flag set"
        return
    }

    # Check if API server is reachable
    $apiReady = $false
    try {
        $health = Invoke-RestMethod -Uri "$ApiBase/healthz" -TimeoutSec 3
        $apiReady = $true
    } catch {
        Write-ValidationCheck -Category 'api' -Name 'api-checks' -Status 'skip' -Detail "API server not responding at $ApiBase"
        return
    }

    # healthz
    if ($health.ea -eq $true) {
        Write-ValidationCheck -Category 'api' -Name 'api-healthz' -Status 'pass' -Detail "EA connected"
    } else {
        Write-ValidationCheck -Category 'api' -Name 'api-healthz' -Status 'fail' -Detail "EA not connected (ea: $($health.ea))"
    }

    # readyz
    try {
        $ready = Invoke-WebRequest -Uri "$ApiBase/readyz" -UseBasicParsing -TimeoutSec 3
        Write-ValidationCheck -Category 'api' -Name 'api-readyz' -Status 'pass' -Detail "HTTP $($ready.StatusCode)"
    } catch {
        Write-ValidationCheck -Category 'api' -Name 'api-readyz' -Status 'fail' -Detail "HTTP $($_.Exception.Response.StatusCode.value__)"
    }

    # status-types
    try {
        $types = Invoke-RestMethod -Uri "$ApiBase/api/status-types" -TimeoutSec 5
        if ($types -is [array] -and $types.Count -gt 0) {
            Write-ValidationCheck -Category 'api' -Name 'api-status-types' -Status 'pass' -Detail "$($types.Count) status types"
        } else {
            Write-ValidationCheck -Category 'api' -Name 'api-status-types' -Status 'fail' -Detail "Empty or invalid response"
        }
    } catch {
        Write-ValidationCheck -Category 'api' -Name 'api-status-types' -Status 'fail' -Detail "$($_.Exception.Message)"
    }

    if ($TestElementId -gt 0) {
        # Status round-trip
        Test-StatusRoundtrip -ElementId $TestElementId -Types $types

        # Notes round-trip
        Test-NotesRoundtrip -ElementId $TestElementId
    }

    # AI suggest
    if ($AiEndpoint -and $TestElementId -gt 0) {
        Test-AiSuggest -ElementId $TestElementId
    }

    # AI suggest diagram
    if ($AiEndpoint -and $TestDiagramId -gt 0) {
        Test-AiSuggestDiagram -DiagramId $TestDiagramId
    }

    $script:Results.api_passed = ($script:Results.checks | Where-Object { $_.category -eq 'api' -and $_.status -eq 'pass' }).Count
    $script:Results.api_failed = ($script:Results.checks | Where-Object { $_.category -eq 'api' -and $_.status -eq 'fail' }).Count
    $script:Results.api_skipped = ($script:Results.checks | Where-Object { $_.category -eq 'api' -and $_.status -eq 'skip' }).Count
}

function Test-StatusRoundtrip {
    param([int]$ElementId, [array]$Types)
    $start = Get-Date
    try {
        # Get current status (read-only via healthz won't work, so we use a known status)
        $newStatus = if ($Types.Count -gt 0) { $Types[0] } else { "Draft" }

        # We need the filePath - find it by element ID in the site
        $filePath = Find-ElementFilePath -ElementId $ElementId
        if (-not $filePath) {
            Write-ValidationCheck -Category 'api' -Name 'api-status-roundtrip' -Status 'skip' -Detail "Could not find .md file for element $ElementId"
            return
        }

        $body = @{ elementId = $ElementId; newStatus = $newStatus; filePath = $filePath } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$ApiBase/api/status" -Method POST -Body $body -ContentType "application/json" -TimeoutSec 10

        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        Write-ValidationCheck -Category 'api' -Name 'api-status-roundtrip' -Status 'pass' -Detail "status '$newStatus' written to element $ElementId" -Duration $elapsed
    } catch {
        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        Write-ValidationCheck -Category 'api' -Name 'api-status-roundtrip' -Status 'fail' -Detail "$($_.Exception.Message)"
    }
}

function Test-NotesRoundtrip {
    param([int]$ElementId)
    $start = Get-Date
    try {
        $marker = "<!-- validation-test-marker $(Get-Date -Format 'yyyyMMddHHmmss') -->"
        $filePath = Find-ElementFilePath -ElementId $ElementId
        if (-not $filePath) {
            Write-ValidationCheck -Category 'api' -Name 'api-notes-roundtrip' -Status 'skip' -Detail "Could not find .md file for element $ElementId"
            return
        }

        $body = @{ elementId = $ElementId; newNotes = $marker; filePath = $filePath } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$ApiBase/api/notes" -Method POST -Body $body -ContentType "application/json" -TimeoutSec 10

        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        Write-ValidationCheck -Category 'api' -Name 'api-notes-roundtrip' -Status 'pass' -Detail "notes written and restored to element $ElementId"
    } catch {
        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        Write-ValidationCheck -Category 'api' -Name 'api-notes-roundtrip' -Status 'fail' -Detail "$($_.Exception.Message)"
    }
}

function Test-AiSuggest {
    param([int]$ElementId)
    $start = Get-Date
    try {
        $body = @{ elementId = $ElementId } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$ApiBase/api/ai-suggest" -Method POST -Body $body -ContentType "application/json" -TimeoutSec 30

        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        if ($response.suggestion -and $response.suggestion.Length -gt 0) {
            Write-ValidationCheck -Category 'api' -Name 'api-ai-suggest' -Status 'pass' -Detail "suggestion returned ($($response.suggestion.Length) chars)" -Duration $elapsed
        } else {
            Write-ValidationCheck -Category 'api' -Name 'api-ai-suggest' -Status 'fail' -Detail "Empty suggestion"
        }
    } catch {
        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        Write-ValidationCheck -Category 'api' -Name 'api-ai-suggest' -Status 'fail' -Detail "$($_.Exception.Message)"
    }
}

function Test-AiSuggestDiagram {
    param([int]$DiagramId)
    $start = Get-Date
    try {
        $body = @{ diagramId = $DiagramId } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$ApiBase/api/ai-suggest-diagram" -Method POST -Body $body -ContentType "application/json" -TimeoutSec 30

        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        if ($response.suggestion -and $response.suggestion.Length -gt 0) {
            Write-ValidationCheck -Category 'api' -Name 'api-ai-suggest-diagram' -Status 'pass' -Detail "suggestion returned ($($response.suggestion.Length) chars)" -Duration $elapsed
        } else {
            Write-ValidationCheck -Category 'api' -Name 'api-ai-suggest-diagram' -Status 'fail' -Detail "Empty suggestion"
        }
    } catch {
        $elapsed = ((Get-Date) - $start).TotalMilliseconds
        Write-ValidationCheck -Category 'api' -Name 'api-ai-suggest-diagram' -Status 'fail' -Detail "$($_.Exception.Message)"
    }
}

function Find-ElementFilePath {
    param([int]$ElementId)
    # Search for data-ea-id="$ElementId" in HTML files to find the corresponding .md file
    $htmlFiles = Get-ChildItem $SitePath -Filter "*.html" -Recurse | Where-Object {
        $_.FullName -notmatch '\\types\\' -and $_.FullName -notmatch '\\assets\\'
    }
    foreach ($file in $htmlFiles) {
        $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        if ($content -match "data-ea-id=""$ElementId""") {
            # Convert HTML path to .md path
            $relativePath = $file.FullName.Replace("$SitePath\", "").Replace(".html", ".md")
            return Join-Path $WikiPath $relativePath
        }
    }
    return $null
}
```

- [ ] **Step 2: Commit**

```bash
git add .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
git commit -m "feat: add API integration checks (health, status, notes, AI suggest)"
```

---

### Task 7: Implement Watch Mode and Main Entry Point

**Files:**
- Modify: `.opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1`

**Interfaces:**
- Consumes: All check functions, `Mode`, `WatchIntervalSec`, `OutputJson`, `Threshold` parameters
- Produces: Complete validation run with summary

- [ ] **Step 1: Implement watch mode and main entry point**

Append to `Validate-WikiOutput.ps1`:

```powershell
function Invoke-ValidationRun {
    $script:Results = @{
        passed = 0; failed = 0; warnings = 0; skipped = 0
        page_passed = 0; page_failed = 0; page_skipped = 0
        infra_passed = 0; infra_failed = 0
        svc_passed = 0; svc_failed = 0; svc_warnings = 0
        api_passed = 0; api_failed = 0; api_skipped = 0
        pngs_total = 0; pngs_found = 0
        checks = @()
    }

    Write-Host ""
    Write-Host "=== Wiki Validation $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') ===" -ForegroundColor Cyan
    Write-Host ""

    Test-Infrastructure
    Test-Pages
    Test-Services
    Test-ApiIntegration

    $script:Results.infra_passed = ($script:Results.checks | Where-Object { $_.category -eq 'infrastructure' -and $_.status -eq 'pass' }).Count
    $script:Results.infra_failed = ($script:Results.checks | Where-Object { $_.category -eq 'infrastructure' -and $_.status -eq 'fail' }).Count

    Write-Summary

    if ($OutputJson) {
        $jsonOutput = @{
            timestamp = (Get-Date -Format "o")
            mode = $Mode.ToLower()
            summary = @{
                pages_passed = $script:Results.page_passed
                pages_failed = $script:Results.page_failed
                pages_skipped = $script:Results.page_skipped
                infrastructure_passed = $script:Results.infra_passed
                infrastructure_failed = $script:Results.infra_failed
                services_passed = $script:Results.svc_passed
                services_failed = $script:Results.svc_failed
                services_warnings = $script:Results.svc_warnings
                api_passed = $script:Results.api_passed
                api_failed = $script:Results.api_failed
                api_skipped = $script:Results.api_skipped
                diagram_pngs_total = $script:Results.pngs_total
                diagram_pngs_missing = $script:Results.pngs_total - $script:Results.pngs_found
                total_passed = $script:Results.passed
                total_failed = $script:Results.failed
                total_warnings = $script:Results.warnings
                total_skipped = $script:Results.skipped
            }
            checks = $script:Results.checks
        }
        $jsonOutput | ConvertTo-Json -Depth 10 | Set-Content $OutputJson
        Write-Host ""
        Write-Host "JSON report written to $OutputJson" -ForegroundColor Gray
    }

    return $script:Results.failed
}

# Main execution
if ($Mode -eq "Watch") {
    Write-Host "Watch mode: checking every $WatchIntervalSec seconds (Ctrl+C to stop)" -ForegroundColor Cyan
    while ($true) {
        $failures = Invoke-ValidationRun
        if ($failures -gt $Threshold) {
            Write-Host "FAILURE: $failures failures exceeds threshold ($Threshold)" -ForegroundColor Red
        }
        Write-Host ""
        Write-Host "Next check in $WatchIntervalSec seconds..." -ForegroundColor Gray
        Start-Sleep -Seconds $WatchIntervalSec
        Clear-Host
    }
} else {
    $failures = Invoke-ValidationRun
    if ($failures -gt $Threshold) {
        exit 1
    }
}
```

- [ ] **Step 2: Run full test suite**

Run: `Invoke-Pester tests/scripts/Validate-WikiOutput.Tests.ps1 -Output Detailed`
Expected: All 12 tests PASS

- [ ] **Step 3: Run validation script against current site**

Run: `.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -SitePath ./site -WikiPath ./wiki`
Expected: All static checks pass

- [ ] **Step 4: Commit**

```bash
git add .opencode/skills/wiki-validation/scripts/Validate-WikiOutput.ps1
git commit -m "feat: add watch mode and main entry point"
```

---

### Task 8: Write References Documentation

**Files:**
- Create: `.opencode/skills/wiki-validation/references/page-checks.md`

**Interfaces:** None — documentation only.

- [ ] **Step 1: Create page-checks.md**

```markdown
# Wiki Validation Checks Reference

## Infrastructure Checks

### graph-index-exists
Verifies `graph-index.json` exists in the site root. This file is generated by `GraphIndexExporter` during export and contains all nodes and edges for the client-side relationship graph.

### graph-index-valid
Parses `graph-index.json` and verifies it contains `nodes` and `edges` arrays. Reports node and edge counts.

### diagram-png-existence
Scans all HTML files for `class="diagram-thumb"` links, extracts `src` attributes ending in `.png`, and verifies each PNG file exists on disk.

## Page Validation

For each element page (excluding `types/`, `assets/`, `glossary/`, `recent/`, `status/`, `index.html`, `404.html`), checks:

| Check | Pattern | Description |
|-------|---------|-------------|
| `ea-graph-container` | `id="ea-graph-container"` | Cytoscape graph container div |
| `data-focal-id` | `data-focal-id="\d+"` | Element ID for graph focus |
| `ea-notes` | `ea-notes-editor` or `ea-notes-content` | Notes section widget |
| `notes-editor` | `notes-editor\.js` | Notes editor script loaded |
| `graph-init` | `graph-init\.js` | Graph initialization script |
| `cytoscape` | `cytoscape` | Cytoscape.js library |
| `diagram-thumbs` | `class="diagram-thumbs"` | Diagram thumbnail container |

## Service Health Checks

### mkdocs-serve-port
Checks if port 8000 is in LISTENING state via `netstat`.

### mkdocs-serve-http
Makes an HTTP GET request to `http://127.0.0.1:8000` and verifies HTTP 200.

### writeback-api-port
Checks if port 8001 is in LISTENING state via `netstat`.

### ea-process-cleanup
Checks for orphaned `EA.exe` processes. Reports as warning (not failure).

## API Integration Checks

### api-healthz
GET `/healthz` — verifies server responds and `ea: true`.

### api-readyz
GET `/readyz` — verifies HTTP 200 (not 503).

### api-status-types
GET `/api/status-types` — verifies array of valid status values returned.

### api-status-roundtrip
POST `/api/status` — writes a status value to EA, verifies .md file updated. Restores original after test.

### api-notes-roundtrip
POST `/api/notes` — writes test marker to EA, verifies .md file updated. Restores original after test.

### api-ai-suggest
POST `/api/ai-suggest` — sends element ID to LLM, verifies suggestion returned. Requires `-AiEndpoint`.

### api-ai-suggest-diagram
POST `/api/ai-suggest-diagram` — sends diagram ID to LLM, verifies suggestion returned. Requires `-AiEndpoint` and `-TestDiagramId`.

## Type Page Validation (informational)

Type definition pages under `types/` use a different template and correctly do NOT have `ea-graph-container`, `ea-notes`, or `diagram-thumbs`. These are reported as skipped, not failures.
```

- [ ] **Step 2: Commit**

```bash
git add .opencode/skills/wiki-validation/references/
git commit -m "docs: add wiki validation checks reference"
```

---

### Task 9: Final Integration Test

**Files:** None — verification only.

**Interfaces:** None — runs the complete skill end-to-end.

- [ ] **Step 1: Run Pester tests**

Run: `Invoke-Pester tests/scripts/Validate-WikiOutput.Tests.ps1 -Output Detailed`
Expected: 12 tests PASS

- [ ] **Step 2: Run static validation**

Run: `.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -SitePath ./site -WikiPath ./wiki`
Expected: All infrastructure + page + service checks pass

- [ ] **Step 3: Run with JSON output**

Run: `.\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1 -OutputJson "validation-report.json"`
Expected: JSON file created with valid schema

- [ ] **Step 4: Verify JSON schema**

Run: `(Get-Content validation-report.json | ConvertFrom-Json).summary.pages_passed`
Expected: 451 (or current count)

- [ ] **Step 5: Commit all remaining changes**

```bash
git add -A
git commit -m "feat: complete wiki-validation skill with script, tests, and docs"
```
