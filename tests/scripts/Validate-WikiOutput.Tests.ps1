BeforeAll {
    . "$PSScriptRoot\..\..\.opencode\skills\wiki-validation\scripts\Validate-WikiOutput.ps1"
}

Describe 'Get-ValidateArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-ValidateArgs
        $r.SitePath | Should -Be "./site"
        $r.WikiPath | Should -Be "./wiki"
        $r.Mode | Should -Be "Once"
        $r.WatchIntervalSec | Should -Be 30
        $r.OutputJson | Should -Be ""
        $r.Threshold | Should -Be 100
        $r.ApiBase | Should -Be "http://127.0.0.1:8001"
        $r.TestElementId | Should -Be 0
        $r.TestDiagramId | Should -Be 0
        $r.AiEndpoint | Should -Be ""
        $r.SkipApi | Should -Be $false
        $r.Verbose | Should -Be $false
    }

    It 'parses -SitePath' {
        $r = Get-ValidateArgs -Arguments @('-SitePath', './my-site')
        $r.SitePath | Should -Be "./my-site"
    }

    It 'parses -Mode Watch' {
        $r = Get-ValidateArgs -Arguments @('-Mode', 'Watch')
        $r.Mode | Should -Be "Watch"
    }

    It 'parses -TestElementId' {
        $r = Get-ValidateArgs -Arguments @('-TestElementId', '462')
        $r.TestElementId | Should -Be 462
    }

    It 'parses -TestDiagramId' {
        $r = Get-ValidateArgs -Arguments @('-TestDiagramId', '100')
        $r.TestDiagramId | Should -Be 100
    }

    It 'parses -AiEndpoint' {
        $r = Get-ValidateArgs -Arguments @('-AiEndpoint', 'http://localhost:11434')
        $r.AiEndpoint | Should -Be "http://localhost:11434"
    }

    It 'parses -SkipApi switch' {
        $r = Get-ValidateArgs -Arguments @('-SkipApi')
        $r.SkipApi | Should -Be $true
    }

    It 'parses -OutputJson' {
        $r = Get-ValidateArgs -Arguments @('-OutputJson', 'report.json')
        $r.OutputJson | Should -Be "report.json"
    }

    It 'parses -Threshold' {
        $r = Get-ValidateArgs -Arguments @('-Threshold', '50')
        $r.Threshold | Should -Be 50
    }

    It 'parses -WatchIntervalSec' {
        $r = Get-ValidateArgs -Arguments @('-WatchIntervalSec', '60')
        $r.WatchIntervalSec | Should -Be 60
    }

    It 'parses -Verbose switch' {
        $r = Get-ValidateArgs -Arguments @('-Verbose')
        $r.Verbose | Should -Be $true
    }

    It 'parses combined flags' {
        $r = Get-ValidateArgs -Arguments @('-SkipApi', '-Verbose', '-Mode', 'Watch', '-TestElementId', '462')
        $r.SkipApi | Should -Be $true
        $r.Verbose | Should -Be $true
        $r.Mode | Should -Be "Watch"
        $r.TestElementId | Should -Be 462
    }
}
