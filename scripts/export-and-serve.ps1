param(
    [string]$RepoPath = "model/EurSuRA.qea",
    [int]$Port = 8000,
    [switch]$Force,
    [switch]$Verbose,
    [switch]$Json
)

$exportArgs = @{ RepoPath = $RepoPath }
if ($Force)   { $exportArgs.Force   = $true }
if ($Verbose) { $exportArgs.Verbose = $true }
if ($Json)    { $exportArgs.Json    = $true }

& $PSScriptRoot\export.ps1 @exportArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

& $PSScriptRoot\serve.ps1 -Port $Port
