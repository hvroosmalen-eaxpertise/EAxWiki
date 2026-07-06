BeforeAll {
    . "$PSScriptRoot\..\..\scripts\export-and-serve.ps1"
}

Describe 'Get-ExportAndServeArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-ExportAndServeArgs
        $r.RepoPath | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port | Should -Be 8000
        $r.Force | Should -Be $false
        $r.Verbose | Should -Be $false
        $r.Json | Should -Be $false
        $r.WriteBack | Should -Be $false
        $r.ApiPort | Should -Be 8001
    }

    It 'parses -Force flag' { $r = Get-ExportAndServeArgs -Arguments @('-Force'); $r.Force | Should -Be $true }
    It 'parses --force flag' { $r = Get-ExportAndServeArgs -Arguments @('--force'); $r.Force | Should -Be $true }
    It 'parses -Verbose flag' { $r = Get-ExportAndServeArgs -Arguments @('-Verbose'); $r.Verbose | Should -Be $true }
    It 'parses --verbose flag' { $r = Get-ExportAndServeArgs -Arguments @('--verbose'); $r.Verbose | Should -Be $true }
    It 'parses -Json flag' { $r = Get-ExportAndServeArgs -Arguments @('-Json'); $r.Json | Should -Be $true }
    It 'parses --json flag' { $r = Get-ExportAndServeArgs -Arguments @('--json'); $r.Json | Should -Be $true }
    It 'parses -WriteBack flag' { $r = Get-ExportAndServeArgs -Arguments @('-WriteBack'); $r.WriteBack | Should -Be $true }
    It 'parses --writeback flag' { $r = Get-ExportAndServeArgs -Arguments @('--writeback'); $r.WriteBack | Should -Be $true }
    It 'parses -f shorthand' { $r = Get-ExportAndServeArgs -Arguments @('-f'); $r.Force | Should -Be $true }
    It 'parses -v shorthand' { $r = Get-ExportAndServeArgs -Arguments @('-v'); $r.Verbose | Should -Be $true }
    It 'parses -j shorthand' { $r = Get-ExportAndServeArgs -Arguments @('-j'); $r.Json | Should -Be $true }
    It 'parses -w shorthand' { $r = Get-ExportAndServeArgs -Arguments @('-w'); $r.WriteBack | Should -Be $true }

    It 'parses --port with value' {
        $r = Get-ExportAndServeArgs -Arguments @('--port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -Port with value' {
        $r = Get-ExportAndServeArgs -Arguments @('-Port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -p shorthand' {
        $r = Get-ExportAndServeArgs -Arguments @('-p', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses --repo with value' {
        $r = Get-ExportAndServeArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'accepts connection string as repo' {
        $r = Get-ExportAndServeArgs -Arguments @('DBType=postgresql;Database=foo')
        $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo'
    }

    It 'parses --output with value' {
        $r = Get-ExportAndServeArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
    }

    It 'parses --api-port with value' {
        $r = Get-ExportAndServeArgs -Arguments @('--api-port', '9090')
        $r.ApiPort | Should -Be 9090
    }

    It 'handles Unicode output dir' {
        $r = Get-ExportAndServeArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
    }

    It 'all flags combined' {
        $r = Get-ExportAndServeArgs -Arguments @('--force', '--verbose', '--json', '--writeback', '--port', '8080', '--repo', 'model.qea', '--output', 'wiki', '--api-port', '9090')
        $r.Force | Should -Be $true
        $r.Verbose | Should -Be $true
        $r.Json | Should -Be $true
        $r.WriteBack | Should -Be $true
        $r.Port | Should -Be 8080
        $r.RepoPath | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.ApiPort | Should -Be 9090
    }
}
