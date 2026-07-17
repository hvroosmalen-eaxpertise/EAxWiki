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
