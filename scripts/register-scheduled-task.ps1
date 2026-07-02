# Registers monitor-export-and-serve.ps1 as a Windows Task Scheduler task on a
# simple fixed interval. Interval is a script parameter (not hardcoded) since the
# "right" cadence depends on the deploying organization; see issue #38 for a
# future timezone/day-night-aware replacement of this trigger. --interval-minutes
# gives sub-hourly granularity (e.g. every 30 minutes) as a first step toward #38
# without needing the full day/night/timezone design.
#
# Slack webhook URL can be configured in one of three ways (checked in this order):
#   1. Stored in .eaxwiki as encrypted JSON (recommended for per-instance setup)
#   2. Set as EAXWIKI_ALERT_WEBHOOK environment variable (use when .eaxwiki is shared/unencrypted)
#   3. Not configured (alerting is disabled; only logging to wiki/status/health.md)
#
# The monitor script does NOT accept --webhook-url on the command line because Task Scheduler
# stores action arguments in a readable way (any admin can read them back via Get-ScheduledTask).
#
# Overlap protection: MultipleInstances is set to IgnoreNew, so if a run is still in
# progress when the next trigger fires (e.g. a slow EA export overruns a 30-minute
# interval), Task Scheduler skips the new trigger rather than stacking runs. As a
# backstop, ExecutionTimeLimit kills a genuinely hung run before the interval repeats.
#
# Usage:
#   .\scripts\register-scheduled-task.ps1
#   .\scripts\register-scheduled-task.ps1 --interval-minutes 30 --repo "model/file.qea" --output "wiki" --port 8000
#   .\scripts\register-scheduled-task.ps1 --interval-hours 4 --task-name "EAxWiki Monitor"
#
# Re-running with the same --task-name replaces the existing registration.

$TaskName          = "EAxWiki-Monitor"
$IntervalMinutes   = 0     # if set (via --interval-minutes), takes precedence over --interval-hours
$IntervalHours     = 4     # used only when --interval-minutes is not given
$RepoPath          = ""
$OutputDir         = ""
$Port              = 8000
$MaxRetries        = 3
$RetryDelaySeconds = 30

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(--task-name|-TaskName)$'             { $i++; if ($i -lt $args.Count) { $TaskName          = $args[$i] } }
        '^(--interval-minutes|-IntervalMinutes)$' { $i++; if ($i -lt $args.Count) { $IntervalMinutes = [int]$args[$i] } }
        '^(--interval-hours|-IntervalHours)$'   { $i++; if ($i -lt $args.Count) { $IntervalHours     = [int]$args[$i] } }
        '^(-r|--repo|-RepoPath)$'               { $i++; if ($i -lt $args.Count) { $RepoPath          = $args[$i] } }
        '^(-o|--output|-OutputDir)$'            { $i++; if ($i -lt $args.Count) { $OutputDir         = $args[$i] } }
        '^(-p|--port|-Port)$'                   { $i++; if ($i -lt $args.Count) { $Port              = [int]$args[$i] } }
        '^(--max-retries|-MaxRetries)$'         { $i++; if ($i -lt $args.Count) { $MaxRetries         = [int]$args[$i] } }
        '^(--retry-delay|-RetryDelaySeconds)$'  { $i++; if ($i -lt $args.Count) { $RetryDelaySeconds  = [int]$args[$i] } }
    }
    $i++
}

if (-not $IsWindows) {
    Write-Error "Task Scheduler registration is only available on Windows."
    exit 1
}

$effectiveMinutes = if ($IntervalMinutes -gt 0) { $IntervalMinutes } else { $IntervalHours * 60 }
if ($effectiveMinutes -lt 5) {
    Write-Error "Interval must be at least 5 minutes (got $effectiveMinutes) — a shorter cadence risks overlapping runs against a slow EA export."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
$monitorScript = Join-Path $repoRoot "scripts\monitor-export-and-serve.ps1"

$scriptArgs = @("--max-retries", $MaxRetries, "--retry-delay", $RetryDelaySeconds)
if ($RepoPath)   { $scriptArgs += "--repo", $RepoPath }
if ($OutputDir)  { $scriptArgs += "--output", $OutputDir }
if ($Port)       { $scriptArgs += "--port", $Port }

$argLine = ($scriptArgs | ForEach-Object { if ($_ -match '\s') { "`"$_`"" } else { $_ } }) -join ' '
$psExe = (Get-Process -Id $PID).Path

$intervalSpan = New-TimeSpan -Minutes $effectiveMinutes
# Leave a small buffer below the interval so a hung run is killed before the next
# trigger is due, rather than racing it (IgnoreNew below is the primary guard either way).
$timeLimitMinutes = [Math]::Max(1, $effectiveMinutes - 1)

$action  = New-ScheduledTaskAction -Execute $psExe `
    -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"
# [TimeSpan]::MaxValue serializes to an ISO8601 duration ("P99999999DT23H59M59S") that Task
# Scheduler's XML schema rejects outright (Register-ScheduledTask fails with "value ... is
# incorrectly formatted or out of range"). 10 years is effectively indefinite for this task's
# purpose and stays well within the range Task Scheduler accepts.
$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval $intervalSpan -RepetitionDuration (New-TimeSpan -Days 3650)
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -DontStopOnIdleEnd `
    -ExecutionTimeLimit (New-TimeSpan -Minutes $timeLimitMinutes) -MultipleInstances IgnoreNew

try {
    Register-ScheduledTask -TaskName $TaskName -Action $action -Trigger $trigger -Settings $settings -Force -ErrorAction Stop | Out-Null
} catch {
    Write-Error "Failed to register scheduled task '$TaskName': $($_.Exception.Message)"
    exit 1
}

# Confirm the task actually exists post-registration rather than trusting a non-terminating
# success — Register-ScheduledTask has been observed to fail non-terminating on bad trigger
# XML while script execution continued past it.
$registered = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
if (-not $registered) {
    Write-Error "Registration reported no error, but task '$TaskName' does not exist in Task Scheduler. Aborting."
    exit 1
}

$intervalLabel = if ($effectiveMinutes % 60 -eq 0) { "$($effectiveMinutes / 60) hour(s)" } else { "$effectiveMinutes minute(s)" }
Write-Host "Registered scheduled task '$TaskName' to run every $intervalLabel." -ForegroundColor Green
Write-Host "Command: $psExe -NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"
Write-Host ""
Write-Host "Run 'Unregister-ScheduledTask -TaskName $TaskName' to remove it, or re-run this script to replace it."
