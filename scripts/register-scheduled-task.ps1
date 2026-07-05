# Registers monitor-export-and-serve.ps1 as a Windows Task Scheduler task, either on a
# simple fixed interval or (see issue #38) on a day/night-aware schedule: a fast interval
# during a weekday work-hours window, layered on top of a slower all-day, every-day
# baseline. See docs/superpowers/specs/2026-07-03-issue-38-scheduling-design.md for the
# design behind the day/night mode — in short: Task Scheduler natively supports multiple
# triggers per task, so this is two native triggers, not a config file read by the wrapper.
# monitor-export-and-serve.ps1 itself is completely unaware this exists.
#
# Slack and/or Teams webhook URLs (issue #39 — both are independent, not exclusive; configure
# either, neither, or both) can be configured in one of three ways each (checked in this order):
#   1. Stored in .eaxwiki as encrypted JSON (recommended for per-instance setup)
#   2. Set as EAXWIKI_ALERT_WEBHOOK / EAXWIKI_ALERT_TEAMS_WEBHOOK environment variable (use when
#      .eaxwiki is shared/unencrypted)
#   3. Not configured (alerting is disabled for that channel; still logs to wiki/status/health.md)
#
# This registration script does NOT bake --webhook-url/--teams-webhook-url into the scheduled
# task's command line, even though monitor-export-and-serve.ps1 itself accepts them for manual/
# direct invocation — Task Scheduler stores action arguments in a readable way (any admin can
# read them back via Get-ScheduledTask), so scheduled runs always resolve via env var or .eaxwiki.
#
# Overlap protection: MultipleInstances is set to IgnoreNew, so if a run is still in
# progress when the next trigger fires (e.g. a slow EA export overruns a 30-minute
# interval), Task Scheduler skips the new trigger rather than stacking runs. As a
# backstop, ExecutionTimeLimit kills a genuinely hung run before the interval repeats.
# In day/night mode, the two triggers legitimately overlap during the weekday work-hours
# window (baseline + boost both active) — IgnoreNew is what makes that overlap harmless.
#
# Export mode: every scheduled run is incremental by default, same as export.ps1 itself —
# --force on every run of a short-interval schedule would be needlessly slow. Use --force
# to bake --force into every run instead, or --force-every N to force only every Nth run
# (periodic drift correction while staying incremental the rest of the time).
#
# Usage — simple fixed interval (unchanged from before #38):
#   .\scripts\register-scheduled-task.ps1
#   .\scripts\register-scheduled-task.ps1 --interval-minutes 30 --repo "model/file.qea" --output "wiki" --port 8000
#   .\scripts\register-scheduled-task.ps1 --interval-hours 4 --task-name "EAxWiki Monitor"
#
# Usage — day/night mode (issue #38): fast during weekday work hours, slow otherwise.
# All four flags are required together; there are no baked-in defaults for "work hours"
# since a wrong silent default is worse than requiring an explicit choice.
#   .\scripts\register-scheduled-task.ps1 --work-start "08:00" --work-end "18:00" `
#       --work-interval-minutes 10 --off-hours-interval-minutes 240
#
# Re-running with the same --task-name replaces the existing registration. Changing the
# schedule later (either mode) means re-running this script with new flags — there is no
# live-reloaded config; see the design doc for why that's an accepted tradeoff.

$TaskName                 = "EAxWiki-Monitor"
$IntervalMinutes          = 0     # if set (via --interval-minutes), takes precedence over --interval-hours
$IntervalHours            = 4     # used only when --interval-minutes is not given, and day/night mode is off
$WorkStart                = ""    # e.g. "08:00" — presence of this flag switches to day/night mode
$WorkEnd                  = ""    # e.g. "18:00"
$WorkIntervalMinutes      = 0     # fast cadence during the weekday work-hours window
$OffHoursIntervalMinutes  = 0     # slow baseline cadence, every day, all 24 hours
$RepoPath                 = ""
$OutputDir                = ""
$Port                     = 8000
$MaxRetries               = 3
$RetryDelaySeconds        = 30
$ForceExport              = $false # bake --force into every scheduled run (see monitor-export-and-serve.ps1)
$ForceEveryNRuns          = 0      # bake --force-every N into the scheduled run instead of forcing every time

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(--task-name|-TaskName)$'                    { $i++; if ($i -lt $args.Count) { $TaskName                = $args[$i] } }
        '^(--interval-minutes|-IntervalMinutes)$'       { $i++; if ($i -lt $args.Count) { $IntervalMinutes         = [int]$args[$i] } }
        '^(--interval-hours|-IntervalHours)$'           { $i++; if ($i -lt $args.Count) { $IntervalHours           = [int]$args[$i] } }
        '^(--work-start)$'                              { $i++; if ($i -lt $args.Count) { $WorkStart               = $args[$i] } }
        '^(--work-end)$'                                { $i++; if ($i -lt $args.Count) { $WorkEnd                 = $args[$i] } }
        '^(--work-interval-minutes)$'                   { $i++; if ($i -lt $args.Count) { $WorkIntervalMinutes     = [int]$args[$i] } }
        '^(--off-hours-interval-minutes)$'              { $i++; if ($i -lt $args.Count) { $OffHoursIntervalMinutes = [int]$args[$i] } }
        '^(-r|--repo|-RepoPath)$'                       { $i++; if ($i -lt $args.Count) { $RepoPath                = $args[$i] } }
        '^(-o|--output|-OutputDir)$'                    { $i++; if ($i -lt $args.Count) { $OutputDir               = $args[$i] } }
        '^(-p|--port|-Port)$'                           { $i++; if ($i -lt $args.Count) { $Port                    = [int]$args[$i] } }
        '^(--max-retries|-MaxRetries)$'                 { $i++; if ($i -lt $args.Count) { $MaxRetries               = [int]$args[$i] } }
        '^(--retry-delay|-RetryDelaySeconds)$'          { $i++; if ($i -lt $args.Count) { $RetryDelaySeconds        = [int]$args[$i] } }
        '^(-f|--force)$'                                { $ForceExport = $true }
        '^(--force-every)$'                             { $i++; if ($i -lt $args.Count) { $ForceEveryNRuns         = [int]$args[$i] } }
    }
    $i++
}

if (-not $IsWindows) {
    Write-Error "Task Scheduler registration is only available on Windows."
    exit 1
}

$dayNightMode = -not [string]::IsNullOrWhiteSpace($WorkStart)

function ConvertTo-TimeToday {
    param([string]$TimeString, [string]$FlagName)
    $parsed = [DateTime]::MinValue
    if (-not [DateTime]::TryParseExact($TimeString, "HH:mm", [System.Globalization.CultureInfo]::InvariantCulture, [System.Globalization.DateTimeStyles]::None, [ref]$parsed)) {
        Write-Error "Invalid $FlagName '$TimeString' — expected 24-hour HH:mm, e.g. 08:00 or 18:30."
        exit 1
    }
    $today = Get-Date
    return Get-Date -Year $today.Year -Month $today.Month -Day $today.Day -Hour $parsed.Hour -Minute $parsed.Minute -Second 0
}

if ($dayNightMode) {
    # Day/night mode: all four flags are required together — no partial combination makes sense.
    if ([string]::IsNullOrWhiteSpace($WorkEnd) -or $WorkIntervalMinutes -le 0 -or $OffHoursIntervalMinutes -le 0) {
        Write-Error "Day/night mode requires all four flags together: --work-start, --work-end, --work-interval-minutes, --off-hours-interval-minutes."
        exit 1
    }
    if ($WorkIntervalMinutes -lt 5) {
        Write-Error "--work-interval-minutes must be at least 5 (got $WorkIntervalMinutes) — a shorter cadence risks overlapping runs against a slow EA export."
        exit 1
    }
    if ($OffHoursIntervalMinutes -lt 5) {
        Write-Error "--off-hours-interval-minutes must be at least 5 (got $OffHoursIntervalMinutes)."
        exit 1
    }

    $workStartTime = ConvertTo-TimeToday -TimeString $WorkStart -FlagName "--work-start"
    $workEndTime   = ConvertTo-TimeToday -TimeString $WorkEnd   -FlagName "--work-end"
    if ($workEndTime -le $workStartTime) {
        Write-Error "--work-end ($WorkEnd) must be after --work-start ($WorkStart) on the same day — an overnight-spanning work window isn't supported."
        exit 1
    }
    $workDuration = $workEndTime - $workStartTime
} else {
    $effectiveMinutes = if ($IntervalMinutes -gt 0) { $IntervalMinutes } else { $IntervalHours * 60 }
    if ($effectiveMinutes -lt 5) {
        Write-Error "Interval must be at least 5 minutes (got $effectiveMinutes) — a shorter cadence risks overlapping runs against a slow EA export."
        exit 1
    }
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
$monitorScript = Join-Path $repoRoot "scripts\monitor-export-and-serve.ps1"

$scriptArgs = @("--max-retries", $MaxRetries, "--retry-delay", $RetryDelaySeconds)
if ($RepoPath)   { $scriptArgs += "--repo", $RepoPath }
if ($OutputDir)  { $scriptArgs += "--output", $OutputDir }
if ($Port)       { $scriptArgs += "--port", $Port }
if ($ForceExport) { $scriptArgs += "--force" }
elseif ($ForceEveryNRuns -gt 0) { $scriptArgs += "--force-every", $ForceEveryNRuns }

$argLine = ($scriptArgs | ForEach-Object { if ($_ -match '\s') { "`"$_`"" } else { $_ } }) -join ' '
$psExe = (Get-Process -Id $PID).Path

$action  = New-ScheduledTaskAction -Execute $psExe `
    -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"

if ($dayNightMode) {
    # New-ScheduledTaskTrigger's -Daily and -Weekly parameter sets don't expose
    # -RepetitionInterval/-RepetitionDuration at all (only the -Once set does) — confirmed via
    # Get-Command's own ParameterSets, not an assumption. But the underlying CIM trigger object
    # supports a Repetition sub-object regardless of which parameter set created it; the cmdlet
    # just doesn't surface it for Daily/Weekly. Building a throwaway -Once trigger purely to get
    # a correctly-populated Repetition CimInstance, then assigning that object onto the real
    # Daily/Weekly triggers, works around the cmdlet's parameter-set limitation without touching
    # the CIM/XML layer directly.
    function New-RepetitionPattern {
        param([TimeSpan]$Interval, [TimeSpan]$Duration)
        return (New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval $Interval -RepetitionDuration $Duration).Repetition
    }

    # Baseline: every day, all 24 hours, slow interval — the "always-alive" heartbeat so a
    # real failure at night/on a weekend isn't silently indistinguishable from "just paused."
    $midnightToday = Get-Date -Hour 0 -Minute 0 -Second 0
    $baselineTrigger = New-ScheduledTaskTrigger -Daily -At $midnightToday
    $baselineTrigger.Repetition = New-RepetitionPattern -Interval (New-TimeSpan -Minutes $OffHoursIntervalMinutes) -Duration (New-TimeSpan -Hours 24)

    # Boost: weekdays only, within the work-hours window, fast interval — layered on top of
    # the baseline. Deliberately overlaps with it during weekday daytime; IgnoreNew (below)
    # makes that overlap harmless rather than something this script needs to carve around.
    $boostTrigger = New-ScheduledTaskTrigger -Weekly -DaysOfWeek Monday,Tuesday,Wednesday,Thursday,Friday -At $workStartTime
    $boostTrigger.Repetition = New-RepetitionPattern -Interval (New-TimeSpan -Minutes $WorkIntervalMinutes) -Duration $workDuration

    $triggers = @($baselineTrigger, $boostTrigger)
    # Execution time limit is sized to the shorter (faster) of the two cadences, same
    # "buffer below the interval" rationale as the single-interval mode below.
    $timeLimitMinutes = [Math]::Max(1, $WorkIntervalMinutes - 1)
} else {
    $intervalSpan = New-TimeSpan -Minutes $effectiveMinutes
    # Leave a small buffer below the interval so a hung run is killed before the next
    # trigger is due, rather than racing it (IgnoreNew below is the primary guard either way).
    $timeLimitMinutes = [Math]::Max(1, $effectiveMinutes - 1)
    # [TimeSpan]::MaxValue serializes to an ISO8601 duration ("P99999999DT23H59M59S") that Task
    # Scheduler's XML schema rejects outright (Register-ScheduledTask fails with "value ... is
    # incorrectly formatted or out of range"). 10 years is effectively indefinite for this task's
    # purpose and stays well within the range Task Scheduler accepts.
    $triggers = @(New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval $intervalSpan -RepetitionDuration (New-TimeSpan -Days 3650))
}

$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -DontStopOnIdleEnd `
    -ExecutionTimeLimit (New-TimeSpan -Minutes $timeLimitMinutes) -MultipleInstances IgnoreNew

try {
    Register-ScheduledTask -TaskName $TaskName -Action $action -Trigger $triggers -Settings $settings -Force -ErrorAction Stop | Out-Null
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
$expectedTriggerCount = $triggers.Count
if ($registered.Triggers.Count -ne $expectedTriggerCount) {
    Write-Error "Registration reported no error, but task '$TaskName' has $($registered.Triggers.Count) trigger(s), expected $expectedTriggerCount. Aborting."
    exit 1
}

if ($dayNightMode) {
    Write-Host "Registered scheduled task '$TaskName':" -ForegroundColor Green
    Write-Host "  Weekdays $WorkStart-$WorkEnd : every $WorkIntervalMinutes minute(s)"
    Write-Host "  All other times (nights + weekends): every $OffHoursIntervalMinutes minute(s)"
} else {
    $intervalLabel = if ($effectiveMinutes % 60 -eq 0) { "$($effectiveMinutes / 60) hour(s)" } else { "$effectiveMinutes minute(s)" }
    Write-Host "Registered scheduled task '$TaskName' to run every $intervalLabel." -ForegroundColor Green
}
Write-Host "Command: $psExe -NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"
Write-Host ""
Write-Host "Run 'Unregister-ScheduledTask -TaskName $TaskName' to remove it, or re-run this script to replace it."
