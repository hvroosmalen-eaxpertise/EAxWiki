#Requires -Version 7
<#
.SYNOPSIS
    EAxWiki installer — sets up all prerequisites and builds the project.

.DESCRIPTION
    Run this once on a new machine. On Windows it builds the .NET exporter and
    sets up MkDocs. On Linux/Mac it sets up MkDocs only (EA export requires Windows).

.PARAMETER EAPath
    Path to the Sparx EA installation folder (Windows only). Auto-detected if omitted.
    Example: -EAPath "C:\Program Files (x86)\Sparx Systems\EA"

.PARAMETER SkipDotnet
    Skip the .NET build step (Windows only).

.PARAMETER SkipPython
    Skip Python venv and MkDocs setup.

.EXAMPLE
    pwsh ./install.ps1
    pwsh ./install.ps1 -EAPath "D:\Tools\Sparx\EA"
#>
param(
    [string]$EAPath    = "",
    [switch]$SkipDotnet,
    [switch]$SkipPython
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = $PSScriptRoot
$ok   = "`e[32m[OK]`e[0m"
$err  = "`e[31m[ERROR]`e[0m"
$warn = "`e[33m[WARN]`e[0m"
$info = "`e[36m[INFO]`e[0m"

function Test-Command($name) { $null -ne (Get-Command $name -ErrorAction SilentlyContinue) }

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  EAxWiki Installer" -ForegroundColor Cyan
Write-Host "  Platform: $([System.Runtime.InteropServices.RuntimeInformation]::OSDescription)" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# ── Prerequisites check ────────────────────────────────────────────────────────

Write-Host "Checking prerequisites..." -ForegroundColor Yellow

# Python
$pythonCmd = if (Test-Command "python3") { "python3" } elseif (Test-Command "python") { "python" } else { $null }
if ($pythonCmd) {
    $pyVer = & $pythonCmd --version 2>&1
    Write-Host "$ok  Python: $pyVer"
} else {
    Write-Host "$err Python not found. Install Python 3.x from https://www.python.org/downloads/"
    if (-not $SkipPython) { exit 1 }
}

# .NET and EA (Windows only)
if ($IsWindows -and -not $SkipDotnet) {
    if (Test-Command "dotnet") {
        $dotnetVer = dotnet --version 2>&1
        $major = [int]($dotnetVer -split '\.')[0]
        if ($major -ge 10) {
            Write-Host "$ok  .NET SDK: $dotnetVer"
        } else {
            Write-Host "$warn .NET SDK $dotnetVer found but version 10+ is required."
            Write-Host "      Download from: https://dotnet.microsoft.com/download"
        }
    } else {
        Write-Host "$err .NET SDK not found. Download from: https://dotnet.microsoft.com/download"
        exit 1
    }

    # Locate Enterprise Architect
    $eaCandidates = @(
        $EAPath,
        $env:EA_PATH,
        "C:\Program Files (x86)\Sparx Systems\EA",
        "C:\Program Files\Sparx Systems\EA",
        "D:\Program Files (x86)\Sparx Systems\EA",
        "D:\Program Files\Sparx Systems\EA",
        "E:\Program Files (x86)\Sparx Systems\EA",
        "E:\Program Files\Sparx Systems\EA"
    ) | Where-Object { $_ -and (Test-Path (Join-Path $_ "EA.exe")) }

    if ($eaCandidates) {
        $resolvedEAPath = $eaCandidates[0].TrimEnd('\') + '\'
        Write-Host "$ok  Enterprise Architect: $resolvedEAPath"
        $env:EA_PATH = $resolvedEAPath
    } else {
        Write-Host "$warn Enterprise Architect not found in common locations."
        Write-Host "      Re-run with -EAPath to specify the folder containing EA.exe, or"
        Write-Host "      set the EA_PATH environment variable before building."
        Write-Host "      The .NET project will not build without it."
    }
} elseif (-not $IsWindows) {
    Write-Host "$info .NET / Enterprise Architect: skipped (Linux/Mac — export is Windows-only)"
}

Write-Host ""

# ── .NET build (Windows only) ──────────────────────────────────────────────────

if ($IsWindows -and -not $SkipDotnet) {
    $srcProject = Join-Path $repoRoot "src\EAxWiki\EAxWiki.csproj"
    if (-not (Test-Path $srcProject)) {
        Write-Host "$err Source not found at: $srcProject"
        Write-Host "      Download the Windows installer zip from GitHub Releases — it includes the source."
        exit 1
    }

    Write-Host "Building .NET project..." -ForegroundColor Yellow
    dotnet build $srcProject --configuration Release --nologo
    if ($LASTEXITCODE -ne 0) {
        Write-Host "$err Build failed. Check output above for details."
        exit $LASTEXITCODE
    }
    Write-Host "$ok  Build succeeded."
    Write-Host ""
}

# ── Python venv + MkDocs ───────────────────────────────────────────────────────

if (-not $SkipPython) {
    Write-Host "Setting up Python environment..." -ForegroundColor Yellow

    # If run as a standalone download (not from a repo clone), fetch requirements.txt
    $requirementsFile = Join-Path $repoRoot "requirements.txt"
    if (-not (Test-Path $requirementsFile)) {
        Write-Host "$info requirements.txt not found locally — downloading from GitHub..."
        $rawUrl = "https://raw.githubusercontent.com/hvroosmalen-eaxpertise/EAxWiki/master/requirements.txt"
        try {
            Invoke-WebRequest -Uri $rawUrl -OutFile $requirementsFile -UseBasicParsing
            Write-Host "$ok  Downloaded requirements.txt"
        } catch {
            Write-Host "$err Failed to download requirements.txt: $_"
            exit 1
        }
    }

    $venvDir  = Join-Path $repoRoot ".venv"
    $activate = if ($IsWindows) {
        Join-Path $venvDir "Scripts\Activate.ps1"
    } else {
        Join-Path $venvDir "bin/Activate.ps1"
    }

    if (-not (Test-Path $venvDir)) {
        Write-Host "Creating virtual environment..."
        & $pythonCmd -m venv $venvDir
        if ($LASTEXITCODE -ne 0) { Write-Host "$err Failed to create venv."; exit 1 }
    } else {
        Write-Host "$info Virtual environment already exists at .venv"
    }

    . $activate

    python -m pip install --upgrade pip --quiet
    python -m pip install -r $requirementsFile --quiet
    Write-Host "$ok  MkDocs and requirements installed."
    Write-Host ""
}

# ── Summary ────────────────────────────────────────────────────────────────────

Write-Host "=====================================" -ForegroundColor Green
Write-Host "  Installation complete!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""

if ($IsWindows) {
    Write-Host "Available commands:"
    Write-Host "  .\scripts\export.ps1                   Export EA model to wiki/"
    Write-Host "  .\scripts\serve.ps1                    Serve the wiki with MkDocs"
    Write-Host "  .\scripts\export-and-serve.ps1         Export then serve"
    Write-Host "  .\scripts\export-and-serve.ps1 -Force  Full regeneration then serve"
} else {
    Write-Host "Available commands:"
    Write-Host "  pwsh scripts/serve.ps1                 Serve an existing wiki/ with MkDocs"
    Write-Host ""
    Write-Host "$info Export requires Sparx Enterprise Architect (Windows only)."
    Write-Host "      To generate the wiki, run export on a Windows machine and commit wiki/ to git."
}

Write-Host ""
