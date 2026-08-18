. $PSScriptRoot\_bootstrap.ps1

# $PSNativeCommandUseErrorActionPreference (PowerShell 7.3+) defaults to $true in a fresh
# -NoProfile session (e.g. launched by EAxWiki.Monitor.exe or Task Scheduler). When
# set, dotnet's own warn-level log lines on stderr are enough to corrupt the $LASTEXITCODE
# check below even on a fully successful run. Scoped to this script only.
$PSNativeCommandUseErrorActionPreference = $false

if (-not $IsWindowsOS) {
    Write-Error "Export requires Sparx Enterprise Architect, which is only available on Windows."
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

# User args are forwarded verbatim: relative --repo / --output / bare repo resolve against
# $repoRoot because EAxWiki.dll resolves them against its working directory (we Push-Location'd).
Write-Host "=== Exporting wiki from EA model ===" -ForegroundColor Cyan

try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $args
    $code = if ($null -eq $LASTEXITCODE) { 0 } else { $LASTEXITCODE }
    Write-Output "EAXWIKI_EXIT_CODE=$code"
    if ($code -ne 0) {
        Write-Error "Export failed (exit code $code)."
        Cleanup-EAProcesses
        Pop-Location
        exit $code
    }
    Write-Host "Export complete." -ForegroundColor Green
}
finally {
    Cleanup-EAProcesses
}

Pop-Location
