param(
    [string]$RepoPath = "model/EurSuRA.qea",
    [int]$Port = 8000
)

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

Write-Host "=== Step 1: Export wiki from EA model ==="
Write-Host "Repository: $RepoPath"
dotnet run --project src/EAxWiki -- --repo $RepoPath
if ($LASTEXITCODE -ne 0) {
    Write-Error "Export failed (exit code $LASTEXITCODE)."
    Pop-Location
    exit $LASTEXITCODE
}
Write-Host "Export complete." -ForegroundColor Green

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
