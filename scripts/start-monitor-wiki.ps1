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

# Handle kill switch — delegate to kill-monitor-processes.ps1, which reads pid files from the
# state dir and cleans them up. Do NOT read a top-level .eaxwiki-monitor/monitor.pid — that
# file is not the real monitor pid (see comment below).
if ($Kill) {
    Write-Host "Killing any running monitor instance..." -ForegroundColor Magenta
    $killScript = Join-Path $PSScriptRoot "kill-monitor-processes.ps1"
    if (Test-Path $killScript) {
        & $killScript -RepoRoot $RepoRoot
    } else {
        Write-Host "kill-monitor-processes.ps1 not found; skipping kill step." -ForegroundColor Yellow
    }
}

# Duplicate-instance detection and pid-file writing are done inside the .NET monitor
# via MonitorLock (which uses the per-wiki state dir .eaxwiki-monitor/<hash>/monitor.pid).
# The wrapper does NOT write .eaxwiki-monitor/monitor.pid — that top-level file was a bug
# that wrote the wrapper shell's own PID and confused kill-monitor-processes.ps1.

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

# Start the monitor. The .NET Monitor's MonitorLock owns its own pid file lifecycle
# (creates on TryAcquire, deletes on Release) in the per-wiki state dir.
Write-Host "Launching EAxWiki.Monitor.exe..." -ForegroundColor Green
& "$monitorExe" $monitorArgs

Write-Host "Monitor process ended." -ForegroundColor Magenta