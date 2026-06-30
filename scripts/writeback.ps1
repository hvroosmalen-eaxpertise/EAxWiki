# Scan the local wiki for status changes made by users and write them back to the EA model via COM.
#
# Production workflow:
#   1. A user edits the 'status:' field in the YAML frontmatter of an element page (wiki/*.md).
#   2. Run this script to detect changes and push them back to the EA repository via the EA COM API.
#   3. Re-run export.ps1 to regenerate the wiki from the updated EA model.
#
# Requirements: Windows + Sparx Enterprise Architect installed (same as export).

$RepoPath = ""
$Verbose  = $false

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(-v|--verbose|-Verbose)$'     { $Verbose = $true }
        '^(-r|--repo|-RepoPath)$'       { $i++; if ($i -lt $args.Count) { $RepoPath = $args[$i] } }
        default                         { if (-not $args[$i].StartsWith('-')) { $RepoPath = $args[$i] } }
    }
    $i++
}

if (-not $IsWindows) {
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

$runArgs = @("--writeback")
if ($RepoPath) {
    $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                    elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                    else { Join-Path $repoRoot $RepoPath }
    $runArgs += "--repo", $resolvedRepo
    $displayRepo = $resolvedRepo -replace '(?i)(Password|Pwd|User\s*Id|Uid|UserName|Username)\s*=[^;]*', '$1=***'
    Write-Host "Repository: $displayRepo"
}
if ($Verbose) { $runArgs += "--verbose" }

try {
    dotnet run --project src/EAxWiki -- $runArgs
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
