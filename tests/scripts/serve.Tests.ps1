BeforeAll {
    . "$PSScriptRoot\..\..\scripts\serve.ps1"
}

Describe 'Get-ServeArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-ServeArgs
        $r.Port | Should -Be 8000
        $r.WikiDir | Should -Be ""
    }

    It 'parses -Port with value' {
        $r = Get-ServeArgs -Arguments @('-Port', '9000')
        $r.Port | Should -Be 9000
    }

    It 'parses --port with value' {
        $r = Get-ServeArgs -Arguments @('--port', '9000')
        $r.Port | Should -Be 9000
    }

    It 'parses -p shorthand' {
        $r = Get-ServeArgs -Arguments @('-p', '9000')
        $r.Port | Should -Be 9000
    }

    It 'parses --wiki-dir with value' {
        $r = Get-ServeArgs -Arguments @('--wiki-dir', 'custom-wiki')
        $r.WikiDir | Should -Be 'custom-wiki'
    }

    It 'parses -WikiDir with value' {
        $r = Get-ServeArgs -Arguments @('-WikiDir', 'custom-wiki')
        $r.WikiDir | Should -Be 'custom-wiki'
    }

    It 'parses -o shorthand' {
        $r = Get-ServeArgs -Arguments @('-o', 'custom-wiki')
        $r.WikiDir | Should -Be 'custom-wiki'
    }

    It 'accepts bare numeric port' {
        $r = Get-ServeArgs -Arguments @('9000')
        $r.Port | Should -Be 9000
    }

    It 'handles Unicode wiki dir' {
        $r = Get-ServeArgs -Arguments @('--wiki-dir', 'héllo-wörld')
        $r.WikiDir | Should -Be 'héllo-wörld'
    }

    It 'empty wiki-dir value' {
        $r = Get-ServeArgs -Arguments @('--wiki-dir', '')
        $r.WikiDir | Should -Be ''
    }

    It 'all flags combined' {
        $r = Get-ServeArgs -Arguments @('--port', '8080', '--wiki-dir', 'mywiki')
        $r.Port | Should -Be 8080
        $r.WikiDir | Should -Be 'mywiki'
    }

    It 'duplicate port uses last value' {
        $r = Get-ServeArgs -Arguments @('--port', '9000', '-Port', '9090')
        $r.Port | Should -Be 9090
    }
}
