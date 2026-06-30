# Support both PowerShell -Flag and Unix-style --flag syntax.
$Port    = 8000
$WikiDir = ""   # defaults to <repo-root>\wiki when not specified

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(-p|--port|-Port)$'           { $i++; if ($i -lt $args.Count) { $Port    = [int]$args[$i] } }
        '^(-o|--wiki-dir|-WikiDir)$'    { $i++; if ($i -lt $args.Count) { $WikiDir = $args[$i] } }
        default                         { if ($args[$i] -match '^\d+$') { $Port    = [int]$args[$i] } }
    }
    $i++
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$resolvedWikiDir = if ($WikiDir) {
    if ([System.IO.Path]::IsPathRooted($WikiDir)) { $WikiDir }
    else { Join-Path $repoRoot $WikiDir }
} else {
    Join-Path $repoRoot "wiki"
}

$mkdocsTemp = Join-Path $repoRoot ".mkdocs_temp"
if (-not (Test-Path $mkdocsTemp)) { New-Item -ItemType Directory -Path $mkdocsTemp | Out-Null }
$env:TEMP = $mkdocsTemp
$env:TMP  = $mkdocsTemp

$pipCache = Join-Path $repoRoot ".pip_cache"
if (-not (Test-Path $pipCache)) { New-Item -ItemType Directory -Path $pipCache | Out-Null }
$env:PIP_CACHE_DIR = $pipCache

$venvDir = Join-Path $repoRoot ".venv"
if (-not (Test-Path $venvDir)) {
    Write-Host "Creating virtual environment at $venvDir..."
    python3 -m venv $venvDir 2>$null
    if ($LASTEXITCODE -ne 0) { python -m venv $venvDir }
}

$activate = if ($IsWindows) {
    Join-Path $venvDir "Scripts\Activate.ps1"
} else {
    Join-Path $venvDir "bin/Activate.ps1"
}

if (-Not (Test-Path $activate)) {
    Write-Error "Python venv activation script not found at: $activate`nEnsure Python 3.x is installed and on PATH."
    Pop-Location
    exit 1
}

. $activate

Write-Host "Installing MkDocs requirements..."
python -m pip install --upgrade pip --quiet
python -m pip install -r (Join-Path $repoRoot "requirements.txt") --quiet

Write-Host ""
Write-Host "Wiki available at:"
Write-Host "  http://localhost:$Port"

try {
    $hostName  = [System.Net.Dns]::GetHostName()
    $addresses = [System.Net.Dns]::GetHostEntry($hostName).AddressList |
        Where-Object { $_.AddressFamily -eq 'InterNetwork' } |
        Select-Object -ExpandProperty IPAddressToString -Unique
    foreach ($ip in $addresses) { Write-Host "  http://$ip`:$Port" }
} catch {}

Write-Host ""
Write-Host "Starting mkdocs (docs: $resolvedWikiDir)..."
mkdocs serve --dev-addr "0.0.0.0:$Port" --dirty

Pop-Location
