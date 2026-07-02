# Registers monitor-export-and-serve.ps1 as a Windows Task Scheduler task on a
# simple fixed interval. Interval is a script parameter (not hardcoded) since the
# "right" cadence depends on the deploying organization; see issue #38 for a
# future timezone/day-night-aware replacement of this trigger.
#
# Usage:
#   .\scripts\register-scheduled-task.ps1
#   .\scripts\register-scheduled-task.ps1 --interval-hours 4 --repo "model/file.qea" --output "wiki" --port 8000
#   .\scripts\register-scheduled-task.ps1 --task-name "EAxWiki Monitor" --webhook-url "https://..."
#
# Re-running with the same --task-name replaces the existing registration.

$TaskName        = "EAxWiki-Monitor"
$IntervalHours   = 4
$RepoPath        = ""
$OutputDir       = ""
$Port            = 8000
$MaxRetries      = 3
$RetryDelaySeconds = 30
$WebhookUrl      = ""

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(--task-name|-TaskName)$'            { $i++; if ($i -lt $args.Count) { $TaskName          = $args[$i] } }
        '^(--interval-hours|-IntervalHours)$'  { $i++; if ($i -lt $args.Count) { $IntervalHours     = [int]$args[$i] } }
        '^(-r|--repo|-RepoPath)$'              { $i++; if ($i -lt $args.Count) { $RepoPath          = $args[$i] } }
        '^(-o|--output|-OutputDir)$'           { $i++; if ($i -lt $args.Count) { $OutputDir         = $args[$i] } }
        '^(-p|--port|-Port)$'                  { $i++; if ($i -lt $args.Count) { $Port              = [int]$args[$i] } }
        '^(--max-retries|-MaxRetries)$'        { $i++; if ($i -lt $args.Count) { $MaxRetries         = [int]$args[$i] } }
        '^(--retry-delay|-RetryDelaySeconds)$' { $i++; if ($i -lt $args.Count) { $RetryDelaySeconds  = [int]$args[$i] } }
        '^(--webhook-url|-WebhookUrl)$'        { $i++; if ($i -lt $args.Count) { $WebhookUrl         = $args[$i] } }
    }
    $i++
}

if (-not $IsWindows) {
    Write-Error "Task Scheduler registration is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
$monitorScript = Join-Path $repoRoot "scripts\monitor-export-and-serve.ps1"

$scriptArgs = @("--max-retries", $MaxRetries, "--retry-delay", $RetryDelaySeconds)
if ($RepoPath)   { $scriptArgs += "--repo", $RepoPath }
if ($OutputDir)  { $scriptArgs += "--output", $OutputDir }
if ($Port)       { $scriptArgs += "--port", $Port }
if ($WebhookUrl) { $scriptArgs += "--webhook-url", $WebhookUrl }

$argLine = ($scriptArgs | ForEach-Object { if ($_ -match '\s') { "`"$_`"" } else { $_ } }) -join ' '
$psExe = (Get-Process -Id $PID).Path

$action  = New-ScheduledTaskAction -Execute $psExe `
    -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"
$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Hours $IntervalHours) -RepetitionDuration ([TimeSpan]::MaxValue)
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -DontStopOnIdleEnd -ExecutionTimeLimit (New-TimeSpan -Hours ($IntervalHours))

Register-ScheduledTask -TaskName $TaskName -Action $action -Trigger $trigger -Settings $settings -Force | Out-Null

Write-Host "Registered scheduled task '$TaskName' to run every $IntervalHours hour(s)." -ForegroundColor Green
Write-Host "Command: $psExe -NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"
Write-Host ""
Write-Host "Run 'Unregister-ScheduledTask -TaskName $TaskName' to remove it, or re-run this script to replace it."
