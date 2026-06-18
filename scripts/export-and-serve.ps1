param(
    [string]$RepoPath = "model/EurSuRA.qea",
    [int]$Port = 8000,
    [switch]$Verbose
)

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$resolvedRepo = if ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath } else { Join-Path $repoRoot $RepoPath }

# Track EA processes to kill orphans after export
$eaPidsBefore = @(Get-Process EA -ErrorAction SilentlyContinue | ForEach-Object { $_.Id })

function Cleanup-EAProcesses {
    $eaProcesses = Get-Process EA -ErrorAction SilentlyContinue
    $orphans = $eaProcesses | Where-Object { $_.Id -notin $eaPidsBefore }
    if ($orphans) {
        $orphans | Stop-Process -Force -ErrorAction SilentlyContinue
        Write-Host "Cleaned up $($orphans.Count) orphaned EA process(es)." -ForegroundColor DarkYellow
    }
}

Write-Host "=== Step 1: Export wiki from EA model ==="
Write-Host "Repository: $resolvedRepo"
$runArgs = @("--repo", $resolvedRepo)
if ($Verbose) { $runArgs += "--verbose" }
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

Write-Host "=== Step 2: Serve MkDocs ==="

$mkdocsTemp = Join-Path $repoRoot ".mkdocs_temp"
if (-not (Test-Path $mkdocsTemp)) { New-Item -ItemType Directory -Path $mkdocsTemp | Out-Null }
$env:TEMP = $mkdocsTemp
$env:TMP = $mkdocsTemp

$pipCache = Join-Path $repoRoot ".pip_cache"
if (-not (Test-Path $pipCache)) { New-Item -ItemType Directory -Path $pipCache | Out-Null }
$env:PIP_CACHE_DIR = $pipCache

$venvDir = Join-Path $repoRoot ".venv"
if (-not (Test-Path $venvDir)) {
    Write-Host "Creating virtual environment at $venvDir..."
    python -m venv $venvDir
}

$activate = Join-Path $venvDir "Scripts\Activate.ps1"
if (-Not (Test-Path $activate)) {
    Write-Error "Python venv activation script not found."
    Pop-Location
    exit 1
}

. $activate

Write-Host "Upgrading pip and installing requirements..."
python -m pip install --upgrade pip | Out-Null
python -m pip install -r (Join-Path $repoRoot "requirements.txt") | Out-Null

Write-Host ""
Write-Host "You can open the site in a browser at:"
Write-Host "  http://localhost:$Port"

Write-Host "Starting mkdocs..."
mkdocs serve --dev-addr 0.0.0.0:$Port

Pop-Location
