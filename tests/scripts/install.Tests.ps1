Describe 'install.ps1 parameter binding' {
    It 'does not require PS 7 (PS 5.1 compatible via bootstrap)' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Not -Match '#Requires\s*-Version\s*7'
        $content | Should -Match '\. \$PSScriptRoot\\scripts\\_bootstrap\.ps1'
        $content | Should -Match 'Write-Warning\s+"EAxWiki is designed for PowerShell 7\+'
    }

    It 'declares EAPath as a string parameter' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\[string\]\$EAPath'
    }

    It 'declares SkipDotnet as a switch parameter' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\[switch\]\$SkipDotnet'
    }

    It 'declares SkipPython as a switch parameter' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\[switch\]\$SkipPython'
    }

    It 'EAPath defaults to empty string in param block' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\[string\]\$EAPath\s*=\s*""'
    }

    It 'has .PARAMETER documentation comment for EAPath' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\.PARAMETER\s+EAPath'
    }

    It 'has .PARAMETER comment for EAPath describing EA path' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\.PARAMETER EAPath[\s\S]*?Path to the Sparx EA installation folder'
    }

    It 'has .PARAMETER documentation comment for SkipDotnet' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\.PARAMETER\s+SkipDotnet'
    }

    It 'has .PARAMETER comment for SkipDotnet describing skip' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\.PARAMETER SkipDotnet[\s\S]*?Skip the .NET build step'
    }

    It 'has .PARAMETER documentation comment for SkipPython' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\.PARAMETER\s+SkipPython'
    }

    It 'has .PARAMETER comment for SkipPython describing skip' {
        $content = Get-Content "$PSScriptRoot\..\..\install.ps1" -Raw
        $content | Should -Match '\.PARAMETER SkipPython[\s\S]*?Skip Python venv and MkDocs setup'
    }
}
