BeforeAll {
    . "$PSScriptRoot\..\..\scripts\writeback.ps1"
}

Describe 'writeback.ps1 forwarder' {
    It 'no longer hand-rolls its own arg parser' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('function Get-WritebackArgs') | Should -Be $false
    }

    It 'prepends --writeback to the raw user args' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('$runArgs = @(''--writeback'') + $args') | Should -Be $true
    }

    It 'still cleans up orphaned EA processes' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('function Cleanup-EAProcesses') | Should -Be $true
    }

    It 'still sets $PSNativeCommandUseErrorActionPreference = $false' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('$PSNativeCommandUseErrorActionPreference = $false') | Should -Be $true
    }
}

Describe 'writeback.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
