BeforeAll {
    . "$PSScriptRoot\..\..\scripts\serve-api.ps1"
}

Describe 'Get-ServeApiArgs (reduced orchestration parser)' {
    It 'returns defaults with no arguments' {
        $r = Get-ServeApiArgs
        $r.RepoPath  | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port      | Should -Be 8000
        $r.ApiPort   | Should -Be 8001
        $r.Forward   | Should -BeNullOrEmpty
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

    It 'accepts bare numeric port and strips it from Forward' {
        $r = Get-ServeApiArgs -Arguments @('9000')
        $r.Port | Should -Be 9000
        $r.Forward | Should -BeNullOrEmpty
    }

    It 'bare port overrides --port flag value (last wins)' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080', '9000')
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

    It 'parses --repo with value and forwards it as --repo' {
        $r = Get-ServeApiArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('--repo', 'model.qea')
    }

    It 'parses --output with value and forwards it as --output' {
        $r = Get-ServeApiArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
        $r.Forward   | Should -Be @('--output', 'mywiki')
    }

    It 'handles Unicode output dir' {
        $r = Get-ServeApiArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
    }

    It 'forwards a bare non-numeric token (becomes the exe bare positional repo)' {
        $r = Get-ServeApiArgs -Arguments @('model.qea')
        $r.Forward | Should -Be @('model.qea')
    }

    It 'all flags combined: orchestration parsed, pass-through forwarded' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080', '--api-port', '9090', '--repo', 'model.qea', '--output', 'wiki', '--force')
        $r.Port      | Should -Be 8080
        $r.ApiPort   | Should -Be 9090
        $r.RepoPath  | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.Forward   | Should -Be @('--repo', 'model.qea', '--output', 'wiki', '--force')
    }
}

Describe 'serve-api.ps1 forwarder' {
    It 'builds the export call from the reduced parser Forward list' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\serve-api.ps1" -Raw
        $content.Contains('$exportArgs = @($parsed.Forward)') | Should -Be $true
        $content.Contains('$exportArgs += ''--api-port'', $ApiPort') | Should -Be $true
    }
}

Describe 'serve-api.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\serve-api.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
