# Support both PowerShell -Flag and Unix-style --flag syntax.
$RepoPath  = ""
$Port      = 8000
$Force     = $false
$Verbose   = $false
$Json      = $false
$WriteBack = $false

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(-f|--force|-Force)$'         { $Force     = $true }
        '^(-v|--verbose|-Verbose)$'     { $Verbose   = $true }
        '^(-j|--json|-Json)$'           { $Json      = $true }
        '^(-w|--writeback|-WriteBack)$' { $WriteBack = $true }
        '^(-p|--port|-Port)$'           { $i++; if ($i -lt $args.Count) { $Port = [int]$args[$i] } }
        '^(-r|--repo|-RepoPath)$'       { $i++; if ($i -lt $args.Count) { $RepoPath = $args[$i] } }
        default                         { if (-not $args[$i].StartsWith('-')) { $RepoPath = $args[$i] } }
    }
    $i++
}

$exportArgs = @()
if ($RepoPath)  { $exportArgs += "--repo", $RepoPath }
if ($Force)     { $exportArgs += "--force" }
if ($Verbose)   { $exportArgs += "--verbose" }
if ($Json)      { $exportArgs += "--json" }
if ($WriteBack) { $exportArgs += "--writeback" }

& $PSScriptRoot\export.ps1 @exportArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

& $PSScriptRoot\serve.ps1 --port $Port
