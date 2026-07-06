BeforeAll {
    . "$PSScriptRoot\..\..\scripts\export.ps1"
}

Describe 'Get-ExportArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-ExportArgs
        $r.Force | Should -Be $false
        $r.Verbose | Should -Be $false
        $r.Json | Should -Be $false
        $r.WriteBack | Should -Be $false
        $r.RepoPath | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.ApiPort | Should -Be 0
    }

    It 'parses -Force flag' {
        $r = Get-ExportArgs -Arguments @('-Force')
        $r.Force | Should -Be $true
    }

    It 'parses --force flag' {
        $r = Get-ExportArgs -Arguments @('--force')
        $r.Force | Should -Be $true
    }

    It 'parses -Verbose flag' {
        $r = Get-ExportArgs -Arguments @('-Verbose')
        $r.Verbose | Should -Be $true
    }

    It 'parses --verbose flag' {
        $r = Get-ExportArgs -Arguments @('--verbose')
        $r.Verbose | Should -Be $true
    }

    It 'parses -Json flag' {
        $r = Get-ExportArgs -Arguments @('-Json')
        $r.Json | Should -Be $true
    }

    It 'parses --json flag' {
        $r = Get-ExportArgs -Arguments @('--json')
        $r.Json | Should -Be $true
    }

    It 'parses -WriteBack flag' {
        $r = Get-ExportArgs -Arguments @('-WriteBack')
        $r.WriteBack | Should -Be $true
    }

    It 'parses --writeback flag' {
        $r = Get-ExportArgs -Arguments @('--writeback')
        $r.WriteBack | Should -Be $true
    }

    It 'parses --repo with value' {
        $r = Get-ExportArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'parses -RepoPath with value' {
        $r = Get-ExportArgs -Arguments @('-RepoPath', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'parses --output with value' {
        $r = Get-ExportArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
    }

    It 'parses -OutputDir with value' {
        $r = Get-ExportArgs -Arguments @('-OutputDir', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
    }

    It 'parses --api-port with numeric value' {
        $r = Get-ExportArgs -Arguments @('--api-port', '8001')
        $r.ApiPort | Should -Be 8001
    }

    It 'parses -ApiPort with numeric value' {
        $r = Get-ExportArgs -Arguments @('-ApiPort', '8001')
        $r.ApiPort | Should -Be 8001
    }

    It 'accepts bare repo path (no flag)' {
        $r = Get-ExportArgs -Arguments @('model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'accepts connection string as repo path' {
        $r = Get-ExportArgs -Arguments @('DBType=postgresql;Database=foo')
        $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo'
    }

    It 'treats --repo value with = as connection string (not relative path)' {
        $r = Get-ExportArgs -Arguments @('--repo', 'DBType=postgresql;Database=foo')
        $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo'
    }

    It 'handles Unicode path' {
        $r = Get-ExportArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
    }

    It 'handles empty string argument' {
        $r = Get-ExportArgs -Arguments @('--output', '')
        $r.OutputDir | Should -Be ''
    }

    It 'supports all flags combined' {
        $r = Get-ExportArgs -Arguments @('--force', '--verbose', '--json', '--writeback', '--repo', 'model.qea', '--output', 'wiki', '--api-port', '8080')
        $r.Force | Should -Be $true
        $r.Verbose | Should -Be $true
        $r.Json | Should -Be $true
        $r.WriteBack | Should -Be $true
        $r.RepoPath | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.ApiPort | Should -Be 8080
    }

    It 'last flag value wins for duplicate flags' {
        $r = Get-ExportArgs -Arguments @('--output', 'wiki1', '--output', 'wiki2')
        $r.OutputDir | Should -Be 'wiki2'
    }

    It 'bare path after flags overrides previous --repo' {
        $r = Get-ExportArgs -Arguments @('--repo', 'model.qea', 'other.qea')
        $r.RepoPath | Should -Be 'other.qea'
    }
}
