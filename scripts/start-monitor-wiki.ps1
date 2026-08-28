<#
.SYNOPSIS
    Starts the EAxWiki unattended monitor: export, serve, write-back API and LLM watchdogs.

.DESCRIPTION
    This script verifies the build, acquires the monitor lock (killing any stale monitor instance),
    and starts the EAxWiki.Monitor.exe which runs the unattended monitor loop.

    The monitor cycle:
    - Checks if export is due (based on ExportIntervalMinutes, default 30)
    - If due: captures EA PIDs before export, runs export, detects new EA processes spawned during export,
      kills them after export completes, then serves/serves-api/llm watchdogs
    - If not due: serves/serves-api/llm watchdogs only
    - Sleeps CheckIntervalSeconds (default 30) between cycles

    The monitor uses PID files in .eaxwiki-monitor/ to track state and prevent duplicate instances.
    Use --kill to force-kill a running monitor before starting a new one.

.PARAMETER RepoRoot
    Optional path to the EAxWiki repository root. If omitted, resolved relative to the script's location.

.PARAMETER Kill
    If specified, kills any currently running monitor instance before starting a new one.

.EXAMPLE
    .\scripts\start-monitor-wiki.ps1

    Starts the monitor using the default repository (relative to the scripts folder).

.EXAMPLE
    .\scripts\start-monitor-wiki.ps1 -RepoRoot "C:\Path\To\EAxWiki" -Kill

    Starts the monitor under the specified repo root, first killing any running instance.

.EXAMPLE
    .\scripts\start-monitor-wiki.ps1 --port 8080 --api-port 8081

    Starts the monitor with custom wiki and API ports.
#>
[CmdletBinding()]
param(
    [string]$RepoRoot,
    [switch]$Kill,
    [string]$AiEndpoint
)

# Resolve repository root
if (-not $RepoRoot) {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
    $RepoRoot = Split-Path -Parent $scriptDir
}

Write-Host "EAxWiki Monitor Startup" -ForegroundColor Cyan
Write-Host "Repository root: $RepoRoot" -ForegroundColor Yellow

# Verify build - check for the monitor exe
$monitorExe = Join-Path $RepoRoot "src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe"
if (-not (Test-Path $monitorExe)) {
    Write-Error "EAxWiki.Monitor.exe not found at '$monitorExe'."
    Write-Error "Run: dotnet build src/EAxWiki.Monitor first."
    exit 1
}

Write-Host "Using Monitor exe: $monitorExe" -ForegroundColor Green

# Handle kill switch
if ($Kill) {
    Write-Host "Killing any running monitor instance..." -ForegroundColor Magenta

    # Try to kill via PID file
    $pidFile = Join-Path $RepoRoot ".eaxwiki-monitor\monitor.pid"
    if (Test-Path $pidFile) {
        $savedPid = Get-Content $pidFile -ErrorAction SilentlyContinue
        if ($savedPid) {
            Write-Host "Found saved monitor PID: $savedPid" -ForegroundColor Yellow
            # Try taskkill
            $result = & taskkill /f /pid $savedPid 2>$null
            if ($result) {
                Write-Host "Killed monitor process (PID $savedPid)." -ForegroundColor Green
            } else {
                Write-Host "Could not kill PID $savedPid (may already be stopped)." -ForegroundColor Yellow
            }
        }
        Remove-Item $pidFile -Force -ErrorAction SilentlyContinue
    }

    # Also try to kill using the ProcessKiller
    $killerDll = Join-Path $RepoRoot "src\EAxWiki.ProcessKiller\bin\Debug\net10.0\EAxWiki.ProcessKiller.dll"
    if (Test-Path $killerDll) {
        Write-Host "Using ProcessKiller to terminate monitor-related processes..." -ForegroundColor Green
        & dotnet "$killerDll" "$RepoRoot" > $null 2>$null
    }
}

# Acquire monitor lock - check if another monitor is already running
$monitorPidPath = Join-Path $RepoRoot ".eaxwiki-monitor\monitor.pid"
if (Test-Path $monitorPidPath) {
    $existingPid = Get-Content $monitorPidPath -ErrorAction SilentlyContinue
    if ($existingPid) {
        Write-Host "WARNING: A monitor instance is already running (PID $existingPid)." -ForegroundColor Red
        Write-Host "Use -Kill flag to force-terminate it, or ensure the lock file is cleaned up." -ForegroundColor Red
        Write-Host "Alternatively, remove the lock file manually: Remove-Item $monitorPidPath" -ForegroundColor Yellow
        $confirm = Read-Host "Do you want to proceed anyway?"
        if ($confirm -ne 'y' -and $confirm -ne 'Y') {
            Write-Host "Aborted." -ForegroundColor Yellow
            exit 1
        }
    }
}

# Write our PID file
Add-Content -Path $monitorPidPath -Value $PID -Encoding UTF8

Write-Host "" -ForegroundColor Cyan
Write-Host "Starting EAxWiki monitor..." -ForegroundColor Cyan
Write-Host "  Monitor will run export cycles every 30 minutes (default export interval)." -ForegroundColor Yellow
Write-Host "  Monitor sleep interval: 30 seconds (default check interval)." -ForegroundColor Yellow
Write-Host "  Press Ctrl+C to stop." -ForegroundColor DarkYellow
Write-Host ""

# Build argument list for the monitor
$monitorArgs = @()
if ($PArgs.Count -gt 0) {
    Write-Host "Passing through monitor arguments: $($PArgs -join ' ')" -ForegroundColor DarkYellow
    $monitorArgs = $PArgs
}
if ($AiEndpoint) {
    $monitorArgs += '--ai-endpoint', $AiEndpoint
}

# Start the monitor
Write-Host "Launching EAxWiki.Monitor.exe..." -ForegroundColor Green
& "$monitorExe" $monitorArgs

# Clean up PID file on exit
Remove-Item $monitorPidPath -Force -ErrorAction SilentlyContinue

Write-Host "Monitor process ended." -ForegroundColor Magenta