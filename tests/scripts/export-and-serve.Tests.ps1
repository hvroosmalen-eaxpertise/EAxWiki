BeforeAll {
    . "$PSScriptRoot\..\..\scripts\export-and-serve.ps1"
}

Describe 'Get-ExportAndServeArgs (reduced orchestration parser)' {
    It 'returns defaults with no arguments' {
        $r = Get-ExportAndServeArgs
        $r.RepoPath  | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port      | Should -Be 8000
        $r.ApiPort   | Should -Be 8001
        $r.Forward   | Should -BeNullOrEmpty
    }

    It 'parses --port with value' {
        $r = Get-ExportAndServeArgs -Arguments @('--port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -p shorthand' {
        $r = Get-ExportAndServeArgs -Arguments @('-p', '8080')
        $r.Port | Should -Be 8080
    }

    It 'strips serve-only --port from the forwarded args' {
        $r = Get-ExportAndServeArgs -Arguments @('--port', '8080', '--force')
        $r.Forward | Should -Be @('--force')
    }

    It 'parses --repo with value and forwards it as --repo' {
        $r = Get-ExportAndServeArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('--repo', 'model.qea')
    }

    It 'normalizes legacy -RepoPath to --repo when forwarding' {
        $r = Get-ExportAndServeArgs -Arguments @('-RepoPath', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('--repo', 'model.qea')
    }

    It 'parses --output with value and forwards it as --output' {
        $r = Get-ExportAndServeArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
        $r.Forward   | Should -Be @('--output', 'mywiki')
    }

    It 'parses --api-port with value and does not forward it (re-appended parsed)' {
        $r = Get-ExportAndServeArgs -Arguments @('--api-port', '9090')
        $r.ApiPort | Should -Be 9090
        $r.Forward  | Should -BeNullOrEmpty
    }

    It 'accepts bare repo path' {
        $r = Get-ExportAndServeArgs -Arguments @('model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('model.qea')
    }

    It 'accepts connection string as repo' {
        $r = Get-ExportAndServeArgs -Arguments @('DBType=postgresql;Database=foo')
        $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo'
        $r.Forward  | Should -Be @('DBType=postgresql;Database=foo')
    }

    It 'handles Unicode output dir' {
        $r = Get-ExportAndServeArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
        $r.Forward   | Should -Be @('--output', 'héllo-wörld')
    }

    It 'passes unknown flags through verbatim' {
        $r = Get-ExportAndServeArgs -Arguments @('--brand', 'eursura', '--force')
        $r.Forward | Should -Be @('--brand', 'eursura', '--force')
    }

    It 'all flags combined: orchestration parsed, pass-through forwarded' {
        $r = Get-ExportAndServeArgs -Arguments @('--force', '--verbose', '--json', '--writeback', '--port', '8080', '--repo', 'model.qea', '--output', 'wiki', '--api-port', '9090')
        $r.Port      | Should -Be 8080
        $r.ApiPort   | Should -Be 9090
        $r.RepoPath  | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.Forward   | Should -Be @('--force', '--verbose', '--json', '--writeback', '--repo', 'model.qea', '--output', 'wiki')
    }
}

Describe 'export-and-serve.ps1 forwarder' {
    It 'builds the export call from the reduced parser Forward list' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content.Contains('$exportArgs = @($parsed.Forward)') | Should -Be $true
        $content.Contains('$exportArgs += ''--api-port'', $ApiPort') | Should -Be $true
    }

    It 'no longer parses pass-through flags' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content.Contains('$Force = $true') | Should -Be $false
        $content.Contains('$Verbose = $true') | Should -Be $false
    }
}

Describe 'export-and-serve.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
