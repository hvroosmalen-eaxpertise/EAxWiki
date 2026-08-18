. $PSScriptRoot\_bootstrap.ps1

# Export the wiki and start MkDocs. If --api-port is given, also starts the
# local wiki write-back server so the status-editor widget can write back to EA.
#
# Usage:
#   .\scripts\export-and-serve.ps1
#   .\scripts\export-and-serve.ps1 --repo "model/file.qea" --output "wiki" --port 8000 --api-port 8001
#
# Only the orchestration values are parsed here (--port, --api-port, --repo, --output, bare repo).
# Everything else in $args is forwarded verbatim to export.ps1 and from there to EAxWiki.dll, so
# typo'd flags fail fast in the parser (exit 1) instead of being swallowed. Serve-only tokens
# (--port and a bare numeric port) are stripped because EAxWiki.dll doesn't know --port; --api-port
# is stripped and re-appended as the parsed value so the status-editor widget is embedded with the
# correct port. Legacy wrapper aliases (-RepoPath, -OutputDir, -ApiPort) are normalized to the
# canonical flags the exe accepts.

function Get-ExportAndServeArgs {
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
                if (-not "$arg".StartsWith('-')) { $RepoPath = $arg }
                $Forward.Add($arg)
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

$parsed = Get-ExportAndServeArgs -Arguments $args
$RepoPath  = $parsed.RepoPath
$OutputDir = $parsed.OutputDir
$Port      = $parsed.Port
$ApiPort   = $parsed.ApiPort

if ($ApiPort -gt 0 -and -not $IsWindowsOS) {
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
# Forward the user's (serve-only-stripped) args, then inject the resolved API port so the
# status-editor widget embeds the correct URL. A relative --output resolves against $repoRoot
# inside EAxWiki.dll, so it matches $wikiDir by construction.
$exportArgs = @($parsed.Forward)
$exportArgs += '--api-port', $ApiPort

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
