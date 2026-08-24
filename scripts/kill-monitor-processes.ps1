<#
.SYNOPSIS
    Kills all monitor-started processes (serve, API, LLM) by reading PID files from .eaxwiki-monitor.

.DESCRIPTION
    This script resolves the repository root, then invokes the EAxWiki.ProcessKiller console app
    to kill any running processes that were started via the monitor (serve.ps1, write-back API server,
    LLM server). Uses PID files (.pid) in the .eaxwiki-monitor folder to identify processes.

.PARAMETER RepoRoot
    Optional path to the EAxWiki repository root. If omitted, the script resolves it relative to
    the script's own location (parent's parent directory).

.EXAMPLE
    .\scripts\kill-monitor-processes.ps1

    Kills all monitor-started processes in the current repository.

    .\scripts\kill-monitor-processes.ps1 -RepoRoot "C:\Path\To\Repo"

    Kills all monitor-started processes under the specified repository root.
#>
[CmdletBinding()]
param(
    [string]$RepoRoot
)

# Resolve repository root
if (-not $RepoRoot) {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
    $repoRoot = Split-Path -Parent $scriptDir
    $repoRoot = Split-Path -Parent $repoRoot
}

Write-Host "EAxWiki ProcessKiller" -ForegroundColor Cyan
Write-Host "Repository root: $RepoRoot" -ForegroundColor Yellow

# Find the ProcessKiller dll
$dllPath = Join-Path $RepoRoot "src\EAxWiki.ProcessKiller\bin\Debug\net10.0\EAxWiki.ProcessKiller.dll"

if (-not (Test-Path $dllPath)) {
    Write-Error "ProcessKiller.dll not found at '$dllPath'."
    Write-Error "Run: dotnet build src/EAxWiki.ProcessKiller first."
    exit 1
}

Write-Host "Using ProcessKiller: $dllPath" -ForegroundColor Green

# Invoke the ProcessKiller app
$exe = & dotnet "$dllPath" "$RepoRoot"

Write-Host "ProcessKiller completed:" -ForegroundColor Cyan
Write-Host $exe

# Provide guidance based on output
if ($exe -match "killed") {
    Write-Host "Monitor-started processes were killed." -ForegroundColor Green
}
if ($exe -match "not running") {
    Write-Host "No running monitor-started processes found." -ForegroundColor Yellow
}