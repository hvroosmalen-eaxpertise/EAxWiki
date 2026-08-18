. $PSScriptRoot\_bootstrap.ps1

# Start the EAxWiki wiki write-back server alongside MkDocs.
#
# The write-back server listens on --api-port and handles status changes
# from the status-editor widget on element pages. MkDocs serves on --port.
#
# Usage:
#   .\scripts\serve-api.ps1
#   .\scripts\serve-api.ps1 --repo "path/to/model.qea" --output "wiki" --port 8000 --api-port 8001
#
# Only the orchestration values are parsed here (--port, --api-port, --repo, --output, bare repo).
# Everything else in $args is forwarded verbatim to export.ps1 and from there to EAxWiki.dll, so
# typo'd flags fail fast in the parser (exit 1) instead of being swallowed. Serve-only tokens
# (--port and a bare numeric port) are stripped; --api-port is stripped and re-appended as the
# parsed value; legacy wrapper aliases (-RepoPath, -OutputDir, -ApiPort) are normalized to the
# canonical flags the exe accepts.

function Get-ServeApiArgs {
    param([string[]]$Arguments)
    $RepoPath  = ""
    $OutputDir = ""
    $Port      = 8000
    $ApiPort   = 8001
    $Forward   = [System.Collections.Generic.List[string]]::new()

    $i = 0
    while ($i -lt $Arguments.Count) {
        $arg = $Arguments[$i]
        switch -Regex ($arg) {
            '^(-p|--port|-Port)$' {
                $i++
                if ($i -lt $Arguments.Count) { $Port = [int]$Arguments[$i] }
            }
            '^(--api-port|-ApiPort)$' {
                $i++
                if ($i -lt $Arguments.Count) { $ApiPort = [int]$Arguments[$i] }
            }
            '^(-r|--repo|-RepoPath)$' {
                $Forward.Add('--repo')
                $i++
                if ($i -lt $Arguments.Count) { $RepoPath = $Arguments[$i]; $Forward.Add($Arguments[$i]) }
            }
            '^(-o|--output|-OutputDir)$' {
                $Forward.Add('--output')
                $i++
                if ($i -lt $Arguments.Count) { $OutputDir = $Arguments[$i]; $Forward.Add($Arguments[$i]) }
            }
            default {
                if ($arg -match '^\d+$') { $Port = [int]$arg }
                else { $Forward.Add($arg) }
            }
        }
        $i++
    }
    return [PSCustomObject]@{
        RepoPath  = $RepoPath
        OutputDir = $OutputDir
        Port      = $Port
        ApiPort   = $ApiPort
        Forward   = $Forward.ToArray()
    }
}

$parsed = Get-ServeApiArgs -Arguments $args
$RepoPath  = $parsed.RepoPath
$OutputDir = $parsed.OutputDir
$Port      = $parsed.Port
$ApiPort   = $parsed.ApiPort

if (-not $IsWindowsOS) {
    Write-Error "The wiki write-back server requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

# Resolve wiki output directory to an absolute path once.
$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

Write-Host "=== EAxWiki - wiki write-back server + Wiki ===" -ForegroundColor Cyan
Write-Host "Write-back server : http://localhost:$ApiPort"
Write-Host "Wiki              : http://localhost:$Port"
Write-Host "Output            : $wikiDir"
Write-Host ""

# Export first so the widget is embedded with the correct API port.
Write-Host "Exporting wiki (--api-port $ApiPort embeds the status-editor widget)..." -ForegroundColor Cyan
$exportArgs = @($parsed.Forward)
$exportArgs += '--api-port', $ApiPort
& $PSScriptRoot\export.ps1 @exportArgs
if ($LASTEXITCODE -ne 0) {
    Write-Error "Export failed - cannot start write-back server + wiki."
    Pop-Location
    exit $LASTEXITCODE
}
Write-Host ""
Write-Host "Starting wiki write-back server in background..."

$apiArgs = @("--api", "--api-port", $ApiPort, "--wiki-port", $Port, "--output", $wikiDir)
if ($RepoPath) {
    $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                    elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                    else { Join-Path $repoRoot $RepoPath }
    $apiArgs += "--repo", $resolvedRepo
}

$dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
$apiJob = Start-Job -ScriptBlock {
    param($root, $dllPath, $argList)
    Set-Location $root
    dotnet exec $dllPath $argList
} -ArgumentList $repoRoot, $dll, $apiArgs

Start-Sleep -Seconds 3

if ($apiJob.State -eq 'Failed') {
    Write-Error "wiki write-back server failed to start."
    Receive-Job $apiJob
    Pop-Location
    exit 1
}

Write-Host "wiki write-back server started (job $($apiJob.Id))." -ForegroundColor Green
Write-Host ""

# Set up MkDocs environment
$mkdocsTemp = Join-Path $repoRoot ".mkdocs_temp"
if (-not (Test-Path $mkdocsTemp)) { New-Item -ItemType Directory -Path $mkdocsTemp | Out-Null }
$env:TEMP = $mkdocsTemp
$env:TMP  = $mkdocsTemp

$pipCache = Join-Path $repoRoot ".pip_cache"
if (-not (Test-Path $pipCache)) { New-Item -ItemType Directory -Path $pipCache | Out-Null }
$env:PIP_CACHE_DIR = $pipCache

$venvDir = Join-Path $repoRoot ".venv"
if (-not (Test-Path $venvDir)) {
    Write-Host "Creating virtual environment..."
    python3 -m venv $venvDir 2>$null
    if ($LASTEXITCODE -ne 0) { python -m venv $venvDir }
}

$activate = if ($IsWindowsOS) {
    Join-Path $venvDir "Scripts\Activate.ps1"
} else {
    Join-Path $venvDir "bin/Activate.ps1"
}

. $activate
python -m pip install --upgrade pip --quiet
python -m pip install -r (Join-Path $repoRoot "requirements.txt") --quiet

Write-Host "Starting MkDocs (Ctrl+C to stop both)..."
Write-Host ""

try {
    mkdocs serve --dev-addr "0.0.0.0:$Port" --dirty
}
finally {
    Write-Host ""
    Write-Host "Stopping wiki write-back server..." -ForegroundColor DarkYellow
    Stop-Job $apiJob -ErrorAction SilentlyContinue
    Remove-Job $apiJob -Force -ErrorAction SilentlyContinue
}

Pop-Location
