. $PSScriptRoot\_bootstrap.ps1

# $PSNativeCommandUseErrorActionPreference (PowerShell 7.3+) defaults to $true in a fresh
# -NoProfile session (e.g. launched by monitor-export-and-serve.ps1 or Task Scheduler). When
# set, dotnet's own warn-level log lines on stderr are enough to corrupt the $LASTEXITCODE
# check below even on a fully successful run. Scoped to this script only.
$PSNativeCommandUseErrorActionPreference = $false

function Get-ExportArgs {
    param([string[]]$Arguments)
    $RepoPath  = ""
    $OutputDir = ""
    $Force     = $false
    $Verbose   = $false
    $Json      = $false
    $WriteBack = $false
    $ApiPort   = 0
    $Brand     = ""

    $i = 0
    while ($i -lt $Arguments.Count) {
        switch -Regex ($Arguments[$i]) {
            '^(-f|--force|-Force)$'              { $Force     = $true }
            '^(-v|--verbose|-Verbose)$'          { $Verbose   = $true }
            '^(-j|--json|-Json)$'                { $Json      = $true }
            '^(-w|--writeback|-WriteBack)$'      { $WriteBack = $true }
            '^(-r|--repo|-RepoPath)$'            { $i++; if ($i -lt $Arguments.Count) { $RepoPath  = $Arguments[$i] } }
            '^(-o|--output|-OutputDir)$'         { $i++; if ($i -lt $Arguments.Count) { $OutputDir = $Arguments[$i] } }
            '^(--api-port|-ApiPort)$'            { $i++; if ($i -lt $Arguments.Count) { $ApiPort   = [int]$Arguments[$i] } }
            '^(--brand|-Brand)$'                 { $i++; if ($i -lt $Arguments.Count) { $Brand     = $Arguments[$i] } }
            default                              { if (-not "$($Arguments[$i])".StartsWith('-')) { $RepoPath = $Arguments[$i] } }
        }
        $i++
    }
    return [PSCustomObject]@{
        RepoPath  = $RepoPath
        OutputDir = $OutputDir
        Force     = $Force
        Verbose   = $Verbose
        Json      = $Json
        WriteBack = $WriteBack
        ApiPort   = $ApiPort
        Brand     = $Brand
    }
}

$parsed = Get-ExportArgs -Arguments $args
$RepoPath  = $parsed.RepoPath
$OutputDir = $parsed.OutputDir
$Force     = $parsed.Force
$Verbose   = $parsed.Verbose
$Json      = $parsed.Json
$WriteBack = $parsed.WriteBack
$ApiPort   = $parsed.ApiPort
$Brand     = $parsed.Brand

if (-not $IsWindowsOS) {
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

# Resolve output directory to an absolute path so it is unambiguous regardless of the
# working directory the spawned process runs in.
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
if ($Brand)          { $runArgs += "--brand", $Brand }

try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $runArgs
    $code = if ($null -eq $LASTEXITCODE) { 0 } else { $LASTEXITCODE }
    Write-Output "EAXWIKI_EXIT_CODE=$code"
    if ($code -ne 0) {
        Write-Error "Export failed (exit code $code)."
        Cleanup-EAProcesses
        Pop-Location
        exit $code
    }
    Write-Host "Export complete." -ForegroundColor Green
}
finally {
    Cleanup-EAProcesses
}

Pop-Location
