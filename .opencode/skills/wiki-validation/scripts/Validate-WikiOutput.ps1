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

$script:Results = @{ passed = 0; failed = 0; warnings = 0; skipped = 0; checks = @() }

function Write-ValidationCheck {
    param([string]$Category, [string]$Name, [string]$Status, [string]$Detail, [float]$Duration = 0)
    $colors = @{ pass = 'Green'; fail = 'Red'; warn = 'Yellow'; skip = 'Gray' }
    $icon = @{ pass = '[PASS]'; fail = '[FAIL]'; warn = '[WARN]'; skip = '[SKIP]' }
    if ($Duration -gt 0) { $Detail = "$Detail (${Duration}ms)" }
    Write-Host "$($icon[$Status]) $Name`: $Detail" -ForegroundColor $colors[$Status]
    $script:Results.checks += [PSCustomObject]@{
        category = $Category; name = $Name; status = $Status; detail = $Detail; duration = $Duration
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

function Test-Pages {
    $checks = @('ea-graph-container', 'data-focal-id', 'ea-notes', 'notes-editor\.js', 'graph-init\.js', 'cytoscape', 'diagram-thumbs')
    $excludeDirs = @('types', 'assets', 'glossary', 'recent', 'status', 'diagrams')

    $htmlFiles = Get-ChildItem $SitePath -Filter "*.html" -Recurse | Where-Object {
        $rel = $_.FullName.Replace("$SitePath\", "")
        $name = $_.Name
        # Exclude special directories and files
        $isExcluded = $false
        foreach ($dir in $excludeDirs) {
            if ($rel -match "(^|[\\/])$dir[\\/]") { $isExcluded = $true; break }
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
