param(
	[int]$Port = 8000
)

# Run from repository root. Creates a .venv and starts mkdocs serve.
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$venvDir = Join-Path $repoRoot ".venv"
if (-not (Test-Path $venvDir)) {
	Write-Host "Creating virtual environment at $venvDir..."
	python -m venv $venvDir
}

$activate = Join-Path $venvDir "Scripts\Activate.ps1"
if (-Not (Test-Path $activate)) {
	Write-Error "Activation script not found. Ensure Python 3.x is installed and 'python' is available on PATH."
	Pop-Location
	exit 1
}

# Dot-source the activation script so the venv takes effect in this session
. $activate

Write-Host "Upgrading pip and installing requirements..."
python -m pip install --upgrade pip
python -m pip install -r (Join-Path $repoRoot "requirements.txt")

Write-Host "Preparing to start MkDocs on port $Port..."

# Print accessible URLs (localhost and local network addresses)
try {
	$hostName = [System.Net.Dns]::GetHostName()
	$addresses = [System.Net.Dns]::GetHostEntry($hostName).AddressList |
		Where-Object { $_.AddressFamily -eq 'InterNetwork' } | Select-Object -ExpandProperty IPAddressToString -Unique
} catch {
	$addresses = @()
}

Write-Host "You can open the site in a browser at:"
Write-Host "  http://localhost:$Port"
foreach ($ip in $addresses) {
	Write-Host "  http://$ip`:$Port"
}

Write-Host "Starting mkdocs (binding to 0.0.0.0 so other hosts can connect if firewall allows)..."
mkdocs serve --dev-addr 0.0.0.0:$Port

Pop-Location
