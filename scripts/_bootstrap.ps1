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
