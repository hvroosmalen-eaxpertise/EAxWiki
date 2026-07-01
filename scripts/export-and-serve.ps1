# Export the wiki and start MkDocs. If --api-port is given, also starts the
# local wiki write-back server so the status-editor widget can write back to EA.
#
# Usage:
#   .\scripts\export-and-serve.ps1
#   .\scripts\export-and-serve.ps1 --repo "model/file.qea" --output "wiki" --port 8000 --api-port 8001

# Support both PowerShell -Flag and Unix-style --flag syntax.
$RepoPath  = ""
$OutputDir = ""   # defaults to <repo-root>\wiki when not specified
$Port      = 8000
$Force     = $false
$Verbose   = $false
$Json      = $false
$WriteBack = $false
$ApiPort   = 0

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(-f|--force|-Force)$'         { $Force     = $true }
        '^(-v|--verbose|-Verbose)$'     { $Verbose   = $true }
        '^(-j|--json|-Json)$'           { $Json      = $true }
        '^(-w|--writeback|-WriteBack)$' { $WriteBack = $true }
        '^(-p|--port|-Port)$'           { $i++; if ($i -lt $args.Count) { $Port      = [int]$args[$i] } }
        '^(-r|--repo|-RepoPath)$'       { $i++; if ($i -lt $args.Count) { $RepoPath  = $args[$i] } }
        '^(-o|--output|-OutputDir)$'    { $i++; if ($i -lt $args.Count) { $OutputDir = $args[$i] } }
        '^(--api-port|-ApiPort)$'       { $i++; if ($i -lt $args.Count) { $ApiPort   = [int]$args[$i] } }
        default                         { if (-not "$($args[$i])".StartsWith('-')) { $RepoPath = $args[$i] } }
    }
    $i++
}

if ($ApiPort -gt 0 -and -not $IsWindows) {
    Write-Error "The wiki write-back server requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

# Resolve wiki output directory to an absolute path once so both the export and
# the write-back server refer to the same directory.
$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

# --- Export ---
$exportArgs = @("--output", $wikiDir)
if ($RepoPath)       { $exportArgs += "--repo", $RepoPath }
if ($Force)          { $exportArgs += "--force" }
if ($Verbose)        { $exportArgs += "--verbose" }
if ($Json)           { $exportArgs += "--json" }
if ($WriteBack)      { $exportArgs += "--writeback" }
if ($ApiPort -gt 0)  { $exportArgs += "--api-port", $ApiPort }

& $PSScriptRoot\export.ps1 @exportArgs
if ($LASTEXITCODE -ne 0) { Pop-Location; exit $LASTEXITCODE }

# --- wiki write-back server (optional background job) ---
$apiJob = $null
if ($ApiPort -gt 0) {
    Write-Host ""
    Write-Host "Starting wiki write-back server on port $ApiPort..." -ForegroundColor Cyan

    $apiArgs = @("--api", "--api-port", $ApiPort, "--wiki-port", $Port, "--output", $wikiDir)
    if ($RepoPath) {
        $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                        elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                        else { Join-Path $repoRoot $RepoPath }
        $apiArgs += "--repo", $resolvedRepo
    }

    $apiJob = Start-Job -ScriptBlock {
        param($root, $argList)
        Set-Location $root
        dotnet run --project src/EAxWiki -- $argList
    } -ArgumentList $repoRoot, $apiArgs

    Start-Sleep -Seconds 3

    if ($apiJob.State -eq 'Failed') {
        Write-Error "wiki write-back server failed to start."
        Receive-Job $apiJob
        Pop-Location
        exit 1
    }

    Write-Host "wiki write-back server started (job $($apiJob.Id))." -ForegroundColor Green
}

# --- MkDocs ---
try {
    & $PSScriptRoot\serve.ps1 --port $Port --wiki-dir $wikiDir
} finally {
    if ($apiJob) {
        Write-Host ""
        Write-Host "Stopping wiki write-back server..." -ForegroundColor DarkYellow
        Stop-Job  $apiJob -ErrorAction SilentlyContinue
        Remove-Job $apiJob -Force -ErrorAction SilentlyContinue
    }
}

Pop-Location
