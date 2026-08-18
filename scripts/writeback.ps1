. $PSScriptRoot\_bootstrap.ps1

# Scan the local wiki for status changes made by users and write them back to the EA model via COM.
#
# Production workflow:
#   1. A user edits the 'status:' field in the YAML frontmatter of an element page (wiki/*.md).
#   2. Run this script to detect changes and push them back to the EA repository via the EA COM API.
#   3. Re-run export.ps1 to regenerate the wiki from the updated EA model.
#
# Requirements: Windows + Sparx Enterprise Architect installed (same as export).
#
# Args (--verbose, --repo/-r, or a bare repo path) are forwarded verbatim to EAxWiki.dll, which
# always prints the (redacted) repository and applies --writeback for us.

# See export.ps1 for why this is needed: dotnet's own stderr log lines can otherwise corrupt
# $LASTEXITCODE under $PSNativeCommandUseErrorActionPreference's default in a -NoProfile session.
$PSNativeCommandUseErrorActionPreference = $false

if (-not $IsWindowsOS) {
    Write-Error "Write-back requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$eaPidsBefore = @(Get-Process EA -ErrorAction SilentlyContinue | ForEach-Object { $_.Id })

function Cleanup-EAProcesses {
    $eaProcesses = Get-Process EA -ErrorAction SilentlyContinue
    $orphans = $eaProcesses | Where-Object { $_.Id -notin $eaPidsBefore }
    if ($orphans) {
        $orphans | Stop-Process -Force -ErrorAction SilentlyContinue
        Write-Host "Cleaned up $($orphans.Count) orphaned EA process(es)." -ForegroundColor DarkYellow
    }
}

Write-Host "=== Writing wiki status changes back to EA model ===" -ForegroundColor Cyan

$runArgs = @('--writeback') + $args

try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $runArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Write-back failed (exit code $LASTEXITCODE)."
        Cleanup-EAProcesses
        Pop-Location
        exit $LASTEXITCODE
    }
    Write-Host "Write-back complete." -ForegroundColor Green
    Write-Host "Run export.ps1 to regenerate the wiki from the updated EA model." -ForegroundColor DarkCyan
}
finally {
    Cleanup-EAProcesses
}

Pop-Location
