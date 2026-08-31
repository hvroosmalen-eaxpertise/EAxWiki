<#
.SYNOPSIS
    Kills all monitor-started processes (serve, API, LLM) by reading PID files from .eaxwiki-monitor.

.DESCRIPTION
    This script resolves the repository root, then invokes the EAxWiki.ProcessKiller console app
    to kill any running processes that were started via the monitor (serve.ps1, write-back API server,
    LLM server). Uses PID files (.pid) in the .eaxwiki-monitor folder to identify processes.

    With -Force parameter: Also kills EA.exe instances and other processes not tracked via PID files.
    Use with caution - this will close Sparx Enterprise Architect if running.

.PARAMETER RepoRoot
    Optional path to the EAxWiki repository root. If omitted, the script resolves it relative to
    the script's own location (parent's parent directory).

.PARAMETER Force
    If specified, also kills EA.exe instances and other processes not tracked via PID files.
    Use with caution - this will close Sparx Enterprise Architect if running.

.EXAMPLE
    .\scripts\kill-monitor-processes.ps1

    Kills all monitor-started processes in the current repository.

.EXAMPLE
    .\scripts\kill-monitor-processes.ps1 -RepoRoot "C:\Path\To\Repo"

    Kills all monitor-started processes under the specified repository root.

.EXAMPLE
    .\scripts\kill-monitor-processes.ps1 -Force

    Kills all monitor-started processes AND EA.exe instances under the current repo.

.EXAMPLE
    .\scripts\kill-monitor-processes.ps1 -Force -RepoRoot "C:\Path\To\Repo"

    Kills all monitor-started processes AND EA.exe instances under the specified repo root.
#>
[CmdletBinding()]
param(
    [string]$RepoRoot,
    [switch]$Force
)

# Resolve repository root
if (-not $RepoRoot) {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
    $repoRoot = Split-Path -Parent $scriptDir
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

# Load EAxWiki.Core so [EAxWiki.Core.Monitoring.PidFile] resolves in Phase 1.
# Load from a byte array (not LoadFrom / LoadFile) -- those memory-map and lock the DLL for the
# lifetime of the PowerShell process, which blocks the next `dotnet build` from overwriting it.
$coreDll = Join-Path $RepoRoot "src\EAxWiki.Core\bin\Debug\net10.0\EAxWiki.Core.dll"
if (Test-Path $coreDll) {
    try { [void][Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($coreDll)) } catch { Write-Host "  (warning) Could not load EAxWiki.Core.dll: $_" -ForegroundColor Yellow }
} else {
    Write-Host "  (warning) EAxWiki.Core.dll not found at $coreDll -- Phase 1 will only handle monitor.pid." -ForegroundColor Yellow
}

# --- Step 1: Kill monitor-started processes via PID files ---
Write-Host "Phase 1: Killing monitor-started processes via PID files..." -ForegroundColor Magenta

$pidDir = Join-Path $repoRoot ".eaxwiki-monitor"
if (Test-Path $pidDir) {
    $pidFiles = Get-ChildItem -Path $pidDir -Filter "*.pid" -Recurse -ErrorAction SilentlyContinue
    if ($pidFiles) {
        foreach ($pidFile in $pidFiles) {
            $pidToKill = $null
            try {
                $info = [EAxWiki.Core.Monitoring.PidFile]::Read($pidFile.FullName)
                if ($info -ne $null) {
                    $pidToKill = $info.Pid
                }
                elseif ($pidFile.Name -ieq "monitor.pid") {
                    # monitor.pid is plain PID text (see MonitorLock), not JSON.
                    $firstLine = (Get-Content -Path $pidFile.FullName -TotalCount 1 -ErrorAction SilentlyContinue)
                    $parsed = 0
                    if ([int]::TryParse(($firstLine -as [string]).Trim(), [ref]$parsed)) {
                        $pidToKill = $parsed
                    }
                }
            }
            catch {
                Write-Host "  Could not read PID file $($pidFile.Name)." -ForegroundColor Red
                continue
            }

            if ($pidToKill -ne $null) {
                $shouldDeleteStale = $false
                try {
                    $proc = Get-Process -Id $pidToKill -ErrorAction SilentlyContinue
                    if ($proc -ne $null) {
                        $proc.Kill($true) # entireProcessTree
                        Write-Host "  Killed PID $pidToKill from $($pidFile.Name)" -ForegroundColor Green
                        $shouldDeleteStale = $true
                    } else {
                        Write-Host "  PID $pidToKill from $($pidFile.Name) not running." -ForegroundColor Yellow
                        $shouldDeleteStale = $true
                    }
                }
                catch {
                    Write-Host "  Could not kill PID $pidToKill from $($pidFile.Name) (access denied)." -ForegroundColor Red
                }

                if ($shouldDeleteStale) {
                    Remove-Item -Path $pidFile.FullName -Force -ErrorAction SilentlyContinue
                }
            }
        }
    }
}

# --- Step 2: With -Force, also kill EA.exe and other processes ---
if ($Force) {
    Write-Host "" -ForegroundColor Magenta
    Write-Host "Phase 2: -Force enabled - also killing EA.exe and other processes..." -ForegroundColor Magenta

    # Kill EA.exe instances
    Write-Host "  Looking for EA.exe instances..." -ForegroundColor Cyan
    $eaProcs = Get-Process -Name "EA" -ErrorAction SilentlyContinue
    foreach ($proc in $eaProcs) {
        Write-Host "  Killing EA.exe PID $($proc.Id) ..." -ForegroundColor Red
        try {
            $proc.Kill($true) | Out-Null
            Write-Host "  Successfully killed EA.exe PID $($proc.Id)" -ForegroundColor Green
        }
        catch {
            Write-Host "  Could not kill EA.exe PID $($proc.Id) (may need admin rights)." -ForegroundColor Red
        }
    }

    # Kill mkdocs.exe (serve process)
    Write-Host "  Looking for mkdocs.exe instances..." -ForegroundColor Cyan
    $mkdocsProcs = Get-Process -Name "mkdocs" -ErrorAction SilentlyContinue
    foreach ($proc in $mkdocsProcs) {
        Write-Host "  Killing mkdocs.exe PID $($proc.Id) ..." -ForegroundColor Red
        try {
            $proc.Kill($true) | Out-Null
            Write-Host "  Successfully killed mkdocs.exe PID $($proc.Id)" -ForegroundColor Green
        }
        catch {
            Write-Host "  Could not kill mkdocs.exe PID $($proc.Id)." -ForegroundColor Red
        }
    }

    # Kill EAxWiki.Monitor.exe
    Write-Host "  Looking for EAxWiki.Monitor.exe instances..." -ForegroundColor Cyan
    $monitorProcs = Get-Process -Name "EAxWiki.Monitor" -ErrorAction SilentlyContinue
    foreach ($proc in $monitorProcs) {
        Write-Host "  Killing EAxWiki.Monitor.exe PID $($proc.Id) ..." -ForegroundColor Red
        try {
            $proc.Kill($true) | Out-Null
            Write-Host "  Successfully killed EAxWiki.Monitor.exe PID $($proc.Id)" -ForegroundColor Green
        }
        catch {
            Write-Host "  Could not kill EAxWiki.Monitor.exe PID $($proc.Id)." -ForegroundColor Red
        }
    }

    # Kill python processes (venv for mkdocs)
    Write-Host "  Looking for python.exe instances (venv)..." -ForegroundColor Cyan
    $pythonProcs = Get-Process -Name "python" -ErrorAction SilentlyContinue
    foreach ($proc in $pythonProcs) {
        # Don't kill the script host itself
        if ($proc.Id -ne $pid) {
            Write-Host "  Killing python.exe PID $($proc.Id) ..." -ForegroundColor Red
            try {
                $proc.Kill($true) | Out-Null
                Write-Host "  Successfully killed python.exe PID $($proc.Id)" -ForegroundColor Green
            }
            catch {
                Write-Host "  Could not kill python.exe PID $($proc.Id)." -ForegroundColor Red
            }
        }
    }
}

# --- Step 3: Invoke the .NET ProcessKiller as backup ---
Write-Host "" -ForegroundColor Magenta
Write-Host "Phase 3: Invoking .NET ProcessKiller as backup..." -ForegroundColor Magenta
$killerResult = & dotnet "$dllPath" "$RepoRoot"
Write-Host "ProcessKiller output: $killerResult" -ForegroundColor Cyan

# Summary
Write-Host "" -ForegroundColor Cyan
Write-Host "Kill operation complete." -ForegroundColor Cyan
if ($Force) {
    Write-Host "  - Monitor-started processes killed via PID files" -ForegroundColor Green
    Write-Host "  - EA.exe, mkdocs, Monitor, python processes attempted" -ForegroundColor Green
} else {
    Write-Host "  - Only monitor-started processes killed (use -Force for more)" -ForegroundColor Yellow
}