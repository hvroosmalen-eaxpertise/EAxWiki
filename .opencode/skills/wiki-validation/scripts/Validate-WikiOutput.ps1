[CmdletBinding()]
param(
    [string]$SitePath = "./site",
    [string]$WikiPath = "./wiki",
    [ValidateSet("Once","Watch")]
    [string]$Mode = "Once",
    [int]$WatchIntervalSec = 30,
    [string]$OutputJson = "",
    [int]$Threshold = 100,
    [string]$ApiBase = "http://127.0.0.1:8001",
    [int]$TestElementId = 0,
    [int]$TestDiagramId = 0,
    [string]$AiEndpoint = "",
    [switch]$SkipApi,
    [switch]$VerboseOutput
)

function Get-ValidateArgs {
    param([string[]]$Arguments = @())

    $result = [PSCustomObject]@{
        SitePath        = "./site"
        WikiPath        = "./wiki"
        Mode            = "Once"
        WatchIntervalSec = 30
        OutputJson      = ""
        Threshold       = 100
        ApiBase         = "http://127.0.0.1:8001"
        TestElementId   = 0
        TestDiagramId   = 0
        AiEndpoint      = ""
        SkipApi         = $false
        Verbose         = $false
    }

    $i = 0
    while ($i -lt $Arguments.Count) {
        switch ($Arguments[$i]) {
            '-SitePath'       { $result.SitePath = $Arguments[++$i] }
            '-WikiPath'       { $result.WikiPath = $Arguments[++$i] }
            '-Mode'           { $result.Mode = $Arguments[++$i] }
            '-WatchIntervalSec' { $result.WatchIntervalSec = [int]$Arguments[++$i] }
            '-OutputJson'     { $result.OutputJson = $Arguments[++$i] }
            '-Threshold'      { $result.Threshold = [int]$Arguments[++$i] }
            '-ApiBase'        { $result.ApiBase = $Arguments[++$i] }
            '-TestElementId'  { $result.TestElementId = [int]$Arguments[++$i] }
            '-TestDiagramId'  { $result.TestDiagramId = [int]$Arguments[++$i] }
            '-AiEndpoint'     { $result.AiEndpoint = $Arguments[++$i] }
            '-SkipApi'        { $result.SkipApi = $true }
            '-Verbose'        { $result.Verbose = $true }
        }
        $i++
    }
    return $result
}
