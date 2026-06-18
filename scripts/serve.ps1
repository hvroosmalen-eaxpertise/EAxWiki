param(
	[int]$Port = 8000
)

& $PSScriptRoot\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea" -Port $Port