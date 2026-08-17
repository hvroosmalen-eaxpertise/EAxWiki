BeforeAll {
    . "$PSScriptRoot\..\..\scripts\_bootstrap.ps1"
}

Describe 'Get-EAxWikiDllPath' {
    It 'returns the built EAxWiki.dll path for a repo root' {
        $repoRoot = Split-Path -Parent $PSScriptRoot | Split-Path -Parent
        $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
        $dll | Should -BeLike '*src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll'
        Test-Path $dll | Should -BeTrue
    }

    It 'throws a clear error when the DLL has not been built' {
        Mock Test-Path { return $false }
        { Get-EAxWikiDllPath -RepoRoot 'C:\does-not-exist' } | Should -Throw -ExpectedMessage '*dotnet build src/EAxWiki*'
    }
}

Describe 'Get-EAxWikiMonitorExePath' {
    It 'resolves the built EAxWiki.Monitor.exe' {
        $repoRoot = (Split-Path -Parent $PSScriptRoot) | Split-Path -Parent
        $exe = Get-EAxWikiMonitorExePath -RepoRoot $repoRoot
        $exe | Should -Be (Join-Path $repoRoot 'src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe')
        Test-Path $exe | Should -BeTrue
    }

    It 'throws a clear error when the exe is missing' {
        $repoRoot = (Split-Path -Parent $PSScriptRoot) | Split-Path -Parent
        { Get-EAxWikiMonitorExePath -RepoRoot "$repoRoot\does-not-exist" } | Should -Throw
    }
}
