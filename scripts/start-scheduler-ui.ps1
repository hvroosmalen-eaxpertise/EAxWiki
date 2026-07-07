. $PSScriptRoot\_bootstrap.ps1

# Starts the EAxWiki Scheduler GUI (EAxWiki.SchedulerUI) from anywhere by resolving the repo root
# relative to this script's own location, rather than requiring you to `cd` into the repo first or
# remember the project path.
#
# Usage:
#   .\scripts\start-scheduler-ui.ps1

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
$project = Join-Path $repoRoot "src\EAxWiki.SchedulerUI"

dotnet run --project $project
