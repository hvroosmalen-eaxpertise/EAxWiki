param(
    [string]$RepoPath = "",
    [switch]$Force,
    [switch]$Verbose,
    [switch]$Json
)

if (-not $IsWindows) {
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

Write-Host "=== Exporting wiki from EA model ===" -ForegroundColor Cyan

# Build --repo argument: connection strings contain '=' and are passed as-is;
# file paths are resolved relative to the repo root; empty = omitted so the
# app prompts interactively.
$runArgs = @()
if ($RepoPath) {
    $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                    elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                    else { Join-Path $repoRoot $RepoPath }
    $runArgs += "--repo", $resolvedRepo
    $displayRepo = $resolvedRepo -replace '(?i)(Password|Pwd|User\s*Id|Uid|UserName|Username)\s*=[^;]*', '$1=***'
    Write-Host "Repository: $displayRepo"
}
if ($Force)   { $runArgs += "--force" }
if ($Verbose) { $runArgs += "--verbose" }
if ($Json)    { $runArgs += "--json" }

try {
    dotnet run --project src/EAxWiki -- $runArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Export failed (exit code $LASTEXITCODE)."
        Cleanup-EAProcesses
        Pop-Location
        exit $LASTEXITCODE
    }
    Write-Host "Export complete." -ForegroundColor Green
}
finally {
    Cleanup-EAProcesses
}

Pop-Location
