BeforeAll {
    . "$PSScriptRoot\..\..\scripts\export.ps1"
}

Describe 'export.ps1 forwarder' {
    It 'no longer hand-rolls its own arg parser' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('function Get-ExportArgs') | Should -Be $false
    }

    It 'forwards the user args ($args) to dotnet exec verbatim' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('dotnet exec $dll $args') | Should -Be $true
    }

    It 'still emits the EAXWIKI_EXIT_CODE= protocol' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('EAXWIKI_EXIT_CODE') | Should -Be $true
    }

    It 'still cleans up orphaned EA processes' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('function Cleanup-EAProcesses') | Should -Be $true
    }

    It 'still sets $PSNativeCommandUseErrorActionPreference = $false' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('$PSNativeCommandUseErrorActionPreference = $false') | Should -Be $true
    }
}

Describe 'export.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
