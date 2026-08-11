# _bootstrap.ps1 — Shared bootstrap for EAxWiki scripts.
# Dot-source at the top of every script: . $PSScriptRoot\_bootstrap.ps1
#
# Provides:
#   $PSExecutable   — Full path to the running PowerShell executable
#   $IsWindowsOS    — $true on Windows (works in both PS 5.1 and PS 7+)

$PSExecutable = (Get-Process -Id $PID).Path

if ($PSVersionTable.PSVersion.Major -ge 6) {
    $IsWindowsOS = $IsWindows
} else {
    $IsWindowsOS = $env:OS -eq 'Windows_NT'
}

# $PSNativeCommandUseErrorActionPreference is PS 7.3+ only.
# Setting it as a plain variable in 5.1 is harmless (silently ignored).
if ($PSVersionTable.PSVersion.Major -ge 7 -and $PSVersionTable.PSVersion.Minor -ge 3) {
    $PSNativeCommandUseErrorActionPreference = $false
}

# Get-EAxWikiDllPath — resolve the pre-built EAxWiki.dll and verify it exists. Running the DLL
# via `dotnet exec` instead of `dotnet run --project src/EAxWiki` avoids rebuilding/overwriting
# the DLL that a running write-back API server has loaded, which is what lets the API stay up
# across export runs (see export.ps1 / writeback.ps1 / export-and-serve.ps1 / serve-api.ps1).
# RepoRoot is explicit (not derived from $PSScriptRoot) because this function is defined in a
# dot-sourced file, where those automatic variables do not reliably point at the repo.
function Get-EAxWikiDllPath {
    param([string]$RepoRoot)
    $dllPath = Join-Path $RepoRoot "src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll"
    if (-not (Test-Path $dllPath)) {
        throw "EAxWiki.dll not found at '$dllPath'. Run 'dotnet build src/EAxWiki' first."
    }
    return $dllPath
}
