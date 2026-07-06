BeforeAll {
    . "$PSScriptRoot\..\..\scripts\serve-api.ps1"
}

Describe 'Get-ServeApiArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-ServeApiArgs
        $r.RepoPath | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port | Should -Be 8000
        $r.ApiPort | Should -Be 8001
    }

    It 'parses --port with value' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -Port with value' {
        $r = Get-ServeApiArgs -Arguments @('-Port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -p shorthand' {
        $r = Get-ServeApiArgs -Arguments @('-p', '8080')
        $r.Port | Should -Be 8080
    }

    It 'accepts bare numeric port' {
        $r = Get-ServeApiArgs -Arguments @('9000')
        $r.Port | Should -Be 9000
    }

    It 'parses --api-port with value' {
        $r = Get-ServeApiArgs -Arguments @('--api-port', '9090')
        $r.ApiPort | Should -Be 9090
    }

    It 'parses -ApiPort with value' {
        $r = Get-ServeApiArgs -Arguments @('-ApiPort', '9090')
        $r.ApiPort | Should -Be 9090
    }

    It 'parses --repo with value' {
        $r = Get-ServeApiArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
    }

    It 'parses --output with value' {
        $r = Get-ServeApiArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
    }

    It 'handles Unicode output dir' {
        $r = Get-ServeApiArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
    }

    It 'all flags combined' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080', '--api-port', '9090', '--repo', 'model.qea', '--output', 'wiki')
        $r.Port | Should -Be 8080
        $r.ApiPort | Should -Be 9090
        $r.RepoPath | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
    }

    It 'bare port does not override --port flag value' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080', '9000')
        $r.Port | Should -Be 9000
    }
}
