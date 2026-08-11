BeforeAll {
    . "$PSScriptRoot\..\..\scripts\writeback.ps1"
}

Describe 'Get-WritebackArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-WritebackArgs
        $r.Verbose | Should -Be $false
        $r.RepoPath | Should -Be ""
    }

    It 'parses -Verbose flag' {
        $r = Get-WritebackArgs -Arguments @('-Verbose')
        $r.Verbose | Should -Be $true
    }

    It 'parses --verbose flag' {
        $r = Get-WritebackArgs -Arguments @('--verbose')
        $r.Verbose | Should -Be $true
    }

    It 'parses -v shorthand' {
        $r = Get-WritebackArgs -Arguments @('-v')
        $r.Verbose | Should -Be $true
    }

    It 'parses --repo with value' {
        $r = Get-WritebackArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'parses -RepoPath with value' {
        $r = Get-WritebackArgs -Arguments @('-RepoPath', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'parses -r shorthand' {
        $r = Get-WritebackArgs -Arguments @('-r', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'accepts bare repo path' {
        $r = Get-WritebackArgs -Arguments @('model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'accepts connection string as repo path' {
        $r = Get-WritebackArgs -Arguments @('DBType=postgresql;Database=foo')
        $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo'
    }

    It 'handles Unicode repo path' {
        $r = Get-WritebackArgs -Arguments @('--repo', 'héllo.qea')
        $r.RepoPath | Should -Be 'héllo.qea'
    }

    It 'handles empty repo path' {
        $r = Get-WritebackArgs -Arguments @('--repo', '')
        $r.RepoPath | Should -Be ''
    }

    It 'all flags combined' {
        $r = Get-WritebackArgs -Arguments @('--verbose', '--repo', 'model.qea')
        $r.Verbose | Should -Be $true
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'bare path overrides previous --repo' {
        $r = Get-WritebackArgs -Arguments @('--repo', 'model.qea', 'other.qea')
        $r.RepoPath | Should -Be 'other.qea'
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
