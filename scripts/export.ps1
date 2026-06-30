# Support both PowerShell -Flag and Unix-style --flag syntax.
$RepoPath  = ""
$OutputDir = ""   # defaults to <repo-root>\wiki when not specified
$Force     = $false
$Verbose   = $false
$Json      = $false
$WriteBack = $false
$ApiPort   = 0

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(-f|--force|-Force)$'              { $Force     = $true }
        '^(-v|--verbose|-Verbose)$'          { $Verbose   = $true }
        '^(-j|--json|-Json)$'                { $Json      = $true }
        '^(-w|--writeback|-WriteBack)$'      { $WriteBack = $true }
        '^(-r|--repo|-RepoPath)$'            { $i++; if ($i -lt $args.Count) { $RepoPath  = $args[$i] } }
        '^(-o|--output|-OutputDir)$'         { $i++; if ($i -lt $args.Count) { $OutputDir = $args[$i] } }
        '^(--api-port|-ApiPort)$'            { $i++; if ($i -lt $args.Count) { $ApiPort   = [int]$args[$i] } }
        default                              { if (-not "$($args[$i])".StartsWith('-')) { $RepoPath = $args[$i] } }
    }
    $i++
}

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

# Resolve output directory to an absolute path so it is unambiguous regardless
# of the working directory that dotnet run assigns to the spawned process.
$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

Write-Host "=== Exporting wiki from EA model ===" -ForegroundColor Cyan

$runArgs = @("--output", $wikiDir)
if ($RepoPath) {
    $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                    elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                    else { Join-Path $repoRoot $RepoPath }
    $runArgs += "--repo", $resolvedRepo
}
if ($Force)          { $runArgs += "--force" }
if ($Verbose)        { $runArgs += "--verbose" }
if ($Json)           { $runArgs += "--json" }
if ($WriteBack)      { $runArgs += "--writeback" }
if ($ApiPort -gt 0)  { $runArgs += "--api-port", $ApiPort }

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
