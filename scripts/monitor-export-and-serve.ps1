# Unattended wrapper around export.ps1 / serve.ps1 for issue #37: log to a file,
# keep a health state file and wiki/status/health.md up to date, retry transient
# failures with backoff, and alert (webhook) on final give-up / recovery.
#
# Intended to be triggered periodically (see register-scheduled-task.ps1). Each
# invocation does one pass: pre-flight EA.exe cleanup, export with bounded retry,
# health-page update, and a serve health check (restarting mkdocs if it died since
# the last pass). It does not block on `mkdocs serve` itself.
#
# Alert content (issue #41): the Start alert says whether the run is forced or incremental; a
# Finish alert (gated by the same $NotifyOnStart / --no-notify-start flag as Start) reports
# duration and page counts (total/diagram/element, with delta vs the previous run); and once a
# calendar day boundary is crossed, a DailyDigest alert reports the previous day's approximate
# wiki page-read count (from mkdocs' own dev-server log - see Get-NewPageReadCount for why this
# is inherently approximate) and write-back count (from wiki/status/writeback.log, written by
# WikiWritebackServer.cs).
#
# Usage:
#   .\scripts\monitor-export-and-serve.ps1
#   .\scripts\monitor-export-and-serve.ps1 --repo "model/file.qea" --output "wiki" --port 8000 `
#       --max-retries 3 --retry-delay 30 --min-element-fraction 0.5
#   .\scripts\monitor-export-and-serve.ps1 --test-alert
#
# Export mode, same as export.ps1: incremental (default) or --force for a full rebuild every
# run. On a short (e.g. 30-minute) cadence, --force every run is needlessly slow against a
# large model - use --force-every N instead to force a full rebuild only on every Nth run,
# correcting for any drift a single incremental diff might miss while staying incremental
# the rest of the time (tracked in the health state file as runsSinceForce):
#   .\scripts\monitor-export-and-serve.ps1 --force-every 48   # e.g. once/day on a 30-min cadence
#
# Slack webhook URL resolution (in order):
#   1. --webhook-url CLI argument (if passed)
#   2. EAXWIKI_ALERT_WEBHOOK environment variable (if set)
#   3. .eaxwiki config file (webhookUrl field in encrypted JSON)
#
# Teams webhook URL resolution follows the identical pattern (issue #39):
#   1. --teams-webhook-url CLI argument
#   2. EAXWIKI_ALERT_TEAMS_WEBHOOK environment variable
#   3. .eaxwiki config file (teamsWebhookUrl field)
#
# Slack and Teams are independent, not exclusive - if both are configured, every alert is sent
# to both. Neither is required; a missing/unresolved webhook for one channel just means alerts
# aren't dispatched there (still logged locally either way).
#
# Prefer storing webhook URLs in .eaxwiki via interactive setup or direct configuration.
# For scheduled/unattended use without .eaxwiki, set the env vars above in the scheduled task's
# "Run as" credentials - this keeps the credential out of Task Scheduler's stored action
# arguments (which any admin on the machine can read back).
#
# $PSNativeCommandUseErrorActionPreference (PowerShell 7.3+, defaults to $true in a fresh
# -NoProfile session - exactly how Task Scheduler launches this script) makes stderr output
# from a native command interfere with $LASTEXITCODE / $? once that stderr is merged via
# `2>&1`, as happens below when capturing export.ps1's output. dotnet's own logger writes
# warn-level lines (e.g. "Duplicate sanitized name...") to stderr even on a fully successful
# run, which was enough to make export.ps1's own `dotnet run` exit-code check misfire and
# report a false failure. Disabling it here scopes the fix to this script only.
$PSNativeCommandUseErrorActionPreference = $false

function Get-MonitorArgs {
    param([string[]]$Arguments)
    $RepoPath            = ""
    $OutputDir           = ""
    $Port                = 8000
    $MaxRetries          = 3
    $RetryDelaySeconds   = 30
    $MinElementFraction  = 0.5
    $WebhookUrl          = $null
    $TeamsWebhookUrl     = $null
    $TestAlert           = $false
    $NotifyOnStart       = $true
    $Force               = $false
    $ForceEveryNRuns     = 0

    $i = 0
    while ($i -lt $Arguments.Count) {
        switch -Regex ($Arguments[$i]) {
            '^(-r|--repo|-RepoPath)$'                { $i++; if ($i -lt $Arguments.Count) { $RepoPath           = $Arguments[$i] } }
            '^(-o|--output|-OutputDir)$'             { $i++; if ($i -lt $Arguments.Count) { $OutputDir          = $Arguments[$i] } }
            '^(-p|--port|-Port)$'                    { $i++; if ($i -lt $Arguments.Count) { $Port               = [int]$Arguments[$i] } }
            '^(--max-retries|-MaxRetries)$'          { $i++; if ($i -lt $Arguments.Count) { $MaxRetries          = [int]$Arguments[$i] } }
            '^(--retry-delay|-RetryDelaySeconds)$'   { $i++; if ($i -lt $Arguments.Count) { $RetryDelaySeconds   = [int]$Arguments[$i] } }
            '^(--min-element-fraction)$'             { $i++; if ($i -lt $Arguments.Count) { $MinElementFraction = [double]$Arguments[$i] } }
            '^(--webhook-url|-WebhookUrl)$'          { $i++; if ($i -lt $Arguments.Count) { $WebhookUrl          = $Arguments[$i] } }
            '^(--teams-webhook-url|-TeamsWebhookUrl)$' { $i++; if ($i -lt $Arguments.Count) { $TeamsWebhookUrl   = $Arguments[$i] } }
            '^(--test-alert|-TestAlert)$'            { $TestAlert = $true }
            '^(--no-notify-start)$'                  { $NotifyOnStart = $false }
            '^(-f|--force|-Force)$'                  { $Force = $true }
            '^(--force-every|-ForceEveryNRuns)$'     { $i++; if ($i -lt $Arguments.Count) { $ForceEveryNRuns = [int]$Arguments[$i] } }
            default                                  { if (-not "$($Arguments[$i])".StartsWith('-')) { $RepoPath = $Arguments[$i] } }
        }
        $i++
    }
    return [PSCustomObject]@{
        RepoPath            = $RepoPath
        OutputDir           = $OutputDir
        Port                = $Port
        MaxRetries          = $MaxRetries
        RetryDelaySeconds   = $RetryDelaySeconds
        MinElementFraction  = $MinElementFraction
        WebhookUrl          = $WebhookUrl
        TeamsWebhookUrl     = $TeamsWebhookUrl
        TestAlert           = $TestAlert
        NotifyOnStart       = $NotifyOnStart
        Force               = $Force
        ForceEveryNRuns     = $ForceEveryNRuns
    }
}

$parsed = Get-MonitorArgs -Arguments $args
$RepoPath            = $parsed.RepoPath
$OutputDir           = $parsed.OutputDir
$Port                = $parsed.Port
$MaxRetries          = $parsed.MaxRetries
$RetryDelaySeconds   = $parsed.RetryDelaySeconds
$MinElementFraction  = $parsed.MinElementFraction
$WebhookUrl          = $parsed.WebhookUrl
$TeamsWebhookUrl     = $parsed.TeamsWebhookUrl
$TestAlert           = $parsed.TestAlert
$NotifyOnStart       = $parsed.NotifyOnStart
$Force               = $parsed.Force
$ForceEveryNRuns     = $parsed.ForceEveryNRuns

if (-not $IsWindows) {
    Write-Error "Monitoring requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

# Resolve both webhook URLs from CLI arg â†’ env var â†’ .eaxwiki file. .eaxwiki is decrypted at
# most once (not once per channel) and shared between the two lookups below.
$needsEaxwikiConfig = ($null -eq $WebhookUrl -or "" -eq $WebhookUrl) -or ($null -eq $TeamsWebhookUrl -or "" -eq $TeamsWebhookUrl)
$eaxwikiConfig = $null
if ($needsEaxwikiConfig -and (Test-Path ".eaxwiki")) {
    try {
        $entropy = [System.Text.Encoding]::UTF8.GetBytes("EAxWiki.LocalConfig.v1")
        $base64 = Get-Content ".eaxwiki" -Raw | ForEach-Object { $_.Trim() }
        $encrypted = [Convert]::FromBase64String($base64)
        $decrypted = [System.Security.Cryptography.ProtectedData]::Unprotect($encrypted, $entropy, [System.Security.Cryptography.DataProtectionScope]::CurrentUser)
        $json = [System.Text.Encoding]::UTF8.GetString($decrypted)
        $eaxwikiConfig = $json | ConvertFrom-Json -ErrorAction SilentlyContinue
    } catch {
        # Silent - .eaxwiki may not exist in a decryptable form, or may be in an old format.
    }
}

if ($null -eq $WebhookUrl -or "" -eq $WebhookUrl) {
    if ($env:EAXWIKI_ALERT_WEBHOOK) {
        $WebhookUrl = $env:EAXWIKI_ALERT_WEBHOOK
    } elseif ($eaxwikiConfig -and $eaxwikiConfig.webhookUrl) {
        $WebhookUrl = $eaxwikiConfig.webhookUrl
    }
}

if ($null -eq $TeamsWebhookUrl -or "" -eq $TeamsWebhookUrl) {
    if ($env:EAXWIKI_ALERT_TEAMS_WEBHOOK) {
        $TeamsWebhookUrl = $env:EAXWIKI_ALERT_TEAMS_WEBHOOK
    } elseif ($eaxwikiConfig -and $eaxwikiConfig.teamsWebhookUrl) {
        $TeamsWebhookUrl = $eaxwikiConfig.teamsWebhookUrl
    }
}

$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

# Per-instance state, keyed by a hash of the resolved wiki output dir. This lives OUTSIDE
# $wikiDir on purpose: the exporter's orphan cleanup (InfrastructureWriter.CleanupOrphanedFilesAsync)
# deletes any top-level directory in the output dir that isn't a recognized package or one of its
# special dirs (diagrams/types/glossary/recent/status) on every run - a log directory placed inside
# $wikiDir gets silently wiped on the very next export.
$md5 = [System.Security.Cryptography.MD5]::Create()
$instanceHash = [Convert]::ToHexString($md5.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($wikiDir.ToLowerInvariant()))).Substring(0, 12).ToLowerInvariant()
$stateDir = Join-Path $repoRoot ".eaxwiki-monitor\$instanceHash"
$healthPath  = Join-Path $stateDir "health.json"
$servePidPath = Join-Path $stateDir "serve.pid"
$logDir      = Join-Path $stateDir "logs"
if (-not (Test-Path $logDir)) { New-Item -ItemType Directory -Path $logDir -Force | Out-Null }
$logPath     = Join-Path $logDir ("monitor-{0:yyyy-MM-dd}.log" -f (Get-Date))

function Write-MonitorLog {
    param([string]$Phase, [string]$Message)
    $line = "{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}" -f (Get-Date), $Phase, $Message
    Add-Content -Path $logPath -Value $line
    Write-Host $line
}

# Identifies which instance an alert is about - matters once more than one exporter/serve/monitor
# triple runs on the same machine (the project explicitly supports that via --output/--port).
$instanceLabel = "$env:COMPUTERNAME - $wikiDir"

function Get-HealthState {
    # PSCustomObject (both a [pscustomobject] literal and one returned by ConvertFrom-Json)
    # throws on assigning a property that doesn't already exist - silently, since it's a
    # non-terminating error under the default $ErrorActionPreference. So a health.json written
    # by an older version of this script (missing a field added since) would silently drop any
    # later `$state.newField = ...` assignment. Build the full-shape default first, then for an
    # existing file, backfill any fields the on-disk JSON doesn't have via Add-Member -Force,
    # so every field is guaranteed assignable regardless of when the state file was created.
    $default = [pscustomobject]@{
        lastSuccessTime       = $null
        lastFailureTime       = $null
        consecutiveFailures   = 0
        lastExitCode          = $null
        lastElementCount      = $null
        lastDiagramCount      = $null
        serveConsecutiveFailures = 0
        lastServeFailureTime  = $null
        lastServeSuccessTime  = $null
        runsSinceForce        = 0
        lastMode               = $null
        # Issue #41 daily activity digest: counters accumulate across runs, get reported and reset
        # once a calendar day boundary is crossed (see the digest block near the end of this script).
        # The *LogFile/*LogOffset pairs track how far each source log has already been scanned, so a
        # frequently-run monitor pass never re-counts a page read or write-back it already tallied.
        pageReadsToday        = 0
        writebacksToday       = 0
        lastDigestDate        = $null
        pageReadLogFile       = $null
        pageReadLogOffset     = 0
        writebackLogFile      = $null
        writebackLogOffset    = 0
    }

    if (Test-Path $healthPath) {
        try {
            $loaded = Get-Content $healthPath -Raw | ConvertFrom-Json
            foreach ($prop in $default.PSObject.Properties) {
                if (-not (Get-Member -InputObject $loaded -Name $prop.Name -ErrorAction SilentlyContinue)) {
                    $loaded | Add-Member -NotePropertyName $prop.Name -NotePropertyValue $prop.Value -Force
                }
            }
            return $loaded
        } catch {}
    }
    return $default
}

function Save-HealthState {
    param($State)
    $State | ConvertTo-Json | Set-Content -Path $healthPath
}

function Send-Alert {
    param(
        [string]$Message,
        [ValidateSet('Start', 'Finish', 'Failure', 'Recovery', 'ServeFailure', 'ServeRecovery', 'Test', 'DailyDigest')]
        [string]$Kind
    )
    Write-MonitorLog -Phase "alert" -Message "[$Kind] $Message"
    if (-not $WebhookUrl -and -not $TeamsWebhookUrl) {
        Write-MonitorLog -Phase "alert" -Message "No webhook URL configured (Slack: --webhook-url/EAXWIKI_ALERT_WEBHOOK; Teams: --teams-webhook-url/EAXWIKI_ALERT_TEAMS_WEBHOOK); alert logged only."
        return
    }

    # Slack and Teams are independent, not exclusive (issue #39) - send to whichever channel(s)
    # are configured, not "the first one found."
    $color = switch ($Kind) {
        'Start'         { '#3aa3e3' } # blue
        'Finish'        { '#28a745' } # green
        'Failure'       { '#dc3545' } # red
        'ServeFailure'  { '#dc3545' }
        'Recovery'      { '#28a745' } # green
        'ServeRecovery' { '#28a745' }
        'Test'          { '#3aa3e3' } # blue
        'DailyDigest'   { '#3aa3e3' } # blue
    }
    $emoji = switch ($Kind) {
        'Start'         { ':arrows_counterclockwise:' }
        'Finish'        { ':large_green_circle:' }
        'Failure'       { ':red_circle:' }
        'ServeFailure'  { ':red_circle:' }
        'Recovery'      { ':large_green_circle:' }
        'ServeRecovery' { ':large_green_circle:' }
        'Test'          { ':large_blue_circle:' }
        'DailyDigest'   { ':bar_chart:' }
    }

    if ($WebhookUrl) {
        # Slack's mrkdwn dialect: single asterisks for bold (not Markdown's **), triple-backtick
        # fences work the same as Markdown.
        $slackPayload = @{
            attachments = @(
                @{
                    color      = $color
                    mrkdwn_in  = @('text', 'pretext')
                    pretext    = "$emoji *EAxWiki [$Kind]* - $instanceLabel"
                    text       = $Message
                    footer     = $instanceLabel
                    ts         = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
                }
            )
        } | ConvertTo-Json -Depth 6

        try {
            Invoke-RestMethod -Uri $WebhookUrl -Method Post -Body $slackPayload -ContentType 'application/json; charset=utf-8' | Out-Null
            Write-MonitorLog -Phase "alert" -Message "Slack webhook dispatched."
        } catch {
            Write-MonitorLog -Phase "alert" -Message "Slack webhook dispatch failed: $($_.Exception.Message)"
        }
    }

    if ($TeamsWebhookUrl) {
        # Classic Teams "Incoming Webhook" MessageCard format. themeColor is hex without the
        # leading '#' (unlike Slack's attachment color, which requires it).
        $teamsPayload = @{
            '@type'    = 'MessageCard'
            '@context' = 'http://schema.org/extensions'
            themeColor = $color.TrimStart('#')
            summary    = "EAxWiki [$Kind] - $instanceLabel"
            sections   = @(
                @{
                    activityTitle = "EAxWiki [$Kind] - $instanceLabel"
                    text          = $Message
                }
            )
        } | ConvertTo-Json -Depth 6

        try {
            Invoke-RestMethod -Uri $TeamsWebhookUrl -Method Post -Body $teamsPayload -ContentType 'application/json; charset=utf-8' | Out-Null
            Write-MonitorLog -Phase "alert" -Message "Teams webhook dispatched."
        } catch {
            Write-MonitorLog -Phase "alert" -Message "Teams webhook dispatch failed: $($_.Exception.Message)"
        }
    }
}

if ($TestAlert) {
    Send-Alert -Kind Test -Message "Test alert from monitor-export-and-serve.ps1 - if you can see this in Slack/Teams, the webhook is wired correctly."
    exit 0
}

function Get-ElementCount {
    # Basic output-size sanity signal: count of generated markdown pages (elements + diagrams
    # together, deliberately - this feeds the sanity-check floor below, which cares about total
    # output size, not the element/diagram split).
    if (-not (Test-Path $wikiDir)) { return 0 }
    return @(Get-ChildItem -Path $wikiDir -Filter '*.md' -Recurse -File -ErrorAction SilentlyContinue).Count
}

function Get-DiagramCount {
    # Diagram pages live under any 'diagrams' subfolder. Kept separate from Get-ElementCount
    # (used only for the Finish alert's breakdown, issue #41) so the existing sanity-check floor
    # above - which intentionally counts *all* generated pages - stays untouched.
    if (-not (Test-Path $wikiDir)) { return 0 }
    return @(Get-ChildItem -Path $wikiDir -Filter '*.md' -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object { $_.DirectoryName -match '[\\/]diagrams([\\/]|$)' }).Count
}

# --- Issue #41: daily activity digest (approximate page reads + write-back count). ---
# Both counters use offset-tracked incremental scans of an append-only log rather than parsing
# dates out of individual log lines, because the mkdocs serve log has no date on each line (only
# a time-of-day) and can span multiple calendar days if mkdocs never restarts - there'd be no
# reliable way to tell which day an isolated "[14:32:10] ..." line belongs to after the fact.
# Scanning only new bytes since the last pass and accumulating into pageReadsToday/writebacksToday
# sidesteps that entirely.
function Read-NewLogText {
    param([string]$Path, [string]$OffsetProperty)
    if (-not (Test-Path $Path)) { return $null }
    $length = (Get-Item $Path).Length
    $offset = [long]$state.$OffsetProperty
    if ($length -lt $offset) { $offset = 0 } # file was rotated/truncated since the last scan
    if ($length -eq $offset) { return $null }

    $stream = [System.IO.File]::Open($Path, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read, [System.IO.FileShare]::ReadWrite)
    try {
        $stream.Seek($offset, [System.IO.SeekOrigin]::Begin) | Out-Null
        $reader = New-Object System.IO.StreamReader($stream)
        $text = $reader.ReadToEnd()
        $state.$OffsetProperty = $stream.Position
        return $text
    } finally {
        $stream.Dispose()
    }
}

function Get-NewPageReadCount {
    # "Browser connected: <url>" in mkdocs' dev-server log (stderr) marks a page load - but the
    # same line also fires when livereload auto-reconnects an already-open tab after a rebuild,
    # which happens on every export run that changed content. Counting those as reads would mostly
    # measure "how many scheduled exports ran while a tab was open," not real visits, so any
    # "Browser connected" within 10 seconds of a "Reloading browsers" line is excluded as a
    # reconnect. Still approximate - mkdocs' dev server was never built for analytics - but it's
    # the only signal available without adding a reverse proxy or new logging layer in front of it.
    $files = @(Get-ChildItem -Path $logDir -Filter "serve-*.err.log" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime)
    if ($files.Count -eq 0) { return 0 }

    $currentFile = $files[-1].FullName
    if ($state.pageReadLogFile -ne $currentFile) {
        # mkdocs (re)started since the last scan - new log file, count from its beginning.
        $state.pageReadLogFile = $currentFile
        $state.pageReadLogOffset = 0
    }

    $newText = Read-NewLogText -Path $currentFile -OffsetProperty 'pageReadLogOffset'
    if (-not $newText) { return 0 }

    $lastReloadSeconds = $null
    $count = 0
    foreach ($line in ($newText -split "`r?`n")) {
        if ($line -match '\[(\d{2}):(\d{2}):(\d{2})\]\s+Reloading browsers') {
            $lastReloadSeconds = [int]$matches[1] * 3600 + [int]$matches[2] * 60 + [int]$matches[3]
            continue
        }
        if ($line -match '\[(\d{2}):(\d{2}):(\d{2})\]\s+Browser connected:') {
            $seconds = [int]$matches[1] * 3600 + [int]$matches[2] * 60 + [int]$matches[3]
            if ($null -ne $lastReloadSeconds -and ($seconds - $lastReloadSeconds) -ge 0 -and ($seconds - $lastReloadSeconds) -le 10) {
                continue
            }
            $count++
        }
    }
    return $count
}

function Get-NewWritebackCount {
    # WikiWritebackServer.cs (LogWriteback) appends one line per successful write-back to
    # status/writeback.log - see that file for why it lives under status/ specifically.
    $writebackLogPath = Join-Path $wikiDir "status\writeback.log"
    if ($state.writebackLogFile -ne $writebackLogPath) {
        $state.writebackLogFile = $writebackLogPath
        $state.writebackLogOffset = 0
    }
    $newText = Read-NewLogText -Path $writebackLogPath -OffsetProperty 'writebackLogOffset'
    if (-not $newText) { return 0 }
    return @($newText -split "`r?`n" | Where-Object { $_.Trim().Length -gt 0 }).Count
}

function Update-HealthPage {
    param($State)
    $statusDir = Join-Path $wikiDir "status"
    if (-not (Test-Path $statusDir)) { New-Item -ItemType Directory -Path $statusDir -Force | Out-Null }

    $overall = if ($State.consecutiveFailures -eq 0 -and $State.serveConsecutiveFailures -eq 0) { "Healthy" }
               else { "Degraded" }

    $lines = @(
        "# Pipeline Health"
        ""
        "*Generated by monitor-export-and-serve.ps1 - reports export/serve pipeline status, not EA model element status.*"
        ""
        "**Overall:** $overall"
        ""
        "## Export"
        ""
        "| Field | Value |"
        "|---|---|"
        "| Last success | $($State.lastSuccessTime) |"
        "| Last failure | $($State.lastFailureTime) |"
        "| Consecutive failures | $($State.consecutiveFailures) |"
        "| Last exit code | $($State.lastExitCode) |"
        "| Last page count (total) | $($State.lastElementCount) |"
        "| Last page count (diagrams) | $($State.lastDiagramCount) |"
        "| Last mode | $($State.lastMode) |"
        "| Runs since full rebuild | $($State.runsSinceForce) |"
        ""
        "## Serve"
        ""
        "| Field | Value |"
        "|---|---|"
        "| Last success | $($State.lastServeSuccessTime) |"
        "| Last failure | $($State.lastServeFailureTime) |"
        "| Consecutive failures | $($State.serveConsecutiveFailures) |"
    )
    Set-Content -Path (Join-Path $statusDir "health.md") -Value $lines
}

$state = Get-HealthState

# --force is off by default here, same as export.ps1 itself - a 30-minute-cadence schedule
# doing a full rebuild every run would be needlessly slow against a large model. --force-every
# lets an otherwise-incremental schedule periodically self-correct any drift a single run's
# incremental diff might miss, without forcing every run.
$runsSinceForce = if ($state.runsSinceForce) { [int]$state.runsSinceForce } else { 0 }
$effectiveForce = $Force
if (-not $effectiveForce -and $ForceEveryNRuns -gt 0 -and $runsSinceForce -ge ($ForceEveryNRuns - 1)) {
    $effectiveForce = $true
}

if ($NotifyOnStart) {
    $startModeLabel = if ($effectiveForce) { "forced full rebuild" } else { "incremental" }
    Send-Alert -Kind Start -Message "Scheduled run starting ($startModeLabel)."
}

# --- Pre-flight: clean up orphaned EA.exe processes left by a prior crashed run. ---
$leftoverEA = @(Get-Process EA -ErrorAction SilentlyContinue)
if ($leftoverEA.Count -gt 0) {
    Write-MonitorLog -Phase "preflight" -Message "Found $($leftoverEA.Count) leftover EA.exe process(es) from a prior run; terminating."
    $leftoverEA | Stop-Process -Force -ErrorAction SilentlyContinue
}

$eaPidsBefore = @(Get-Process EA -ErrorAction SilentlyContinue | ForEach-Object { $_.Id })
function Cleanup-EAProcesses {
    $orphans = @(Get-Process EA -ErrorAction SilentlyContinue | Where-Object { $_.Id -notin $eaPidsBefore })
    if ($orphans.Count -gt 0) {
        $orphans | Stop-Process -Force -ErrorAction SilentlyContinue
        Write-MonitorLog -Phase "preflight" -Message "Cleaned up $($orphans.Count) orphaned EA process(es) from this run."
    }
}

# --- Export with bounded retry + backoff. ---
$exportArgs = @("--output", $wikiDir)
if ($RepoPath) { $exportArgs += "--repo", $RepoPath }
if ($effectiveForce) { $exportArgs += "--force" }
$state.lastMode = if ($effectiveForce) { "full (--force)" } else { "incremental" }
Write-MonitorLog -Phase "export" -Message "Mode: $($state.lastMode)."

$attempt = 0
$succeeded = $false
$lastExitCode = $null
$outputTail = ""
$exportStopwatch = [System.Diagnostics.Stopwatch]::StartNew()

while ($attempt -lt $MaxRetries -and -not $succeeded) {
    $attempt++
    Write-MonitorLog -Phase "export" -Message "Attempt $attempt/$MaxRetries starting."

    $output = & $PSScriptRoot\export.ps1 @exportArgs 2>&1 | Tee-Object -Variable capturedOutput
    $lastExitCode = $LASTEXITCODE
    $outputTail = ($capturedOutput | Select-Object -Last 20) -join "`n"
    Cleanup-EAProcesses

    if ($lastExitCode -eq 0) {
        $elementCount = Get-ElementCount
        $previousCount = if ($state.lastElementCount) { [int]$state.lastElementCount } else { 0 }
        $floor = [math]::Floor($previousCount * $MinElementFraction)

        if ($previousCount -gt 0 -and $elementCount -lt $floor) {
            Write-MonitorLog -Phase "export" -Message "Sanity check failed: element count $elementCount is below floor $floor (previous $previousCount)."
            $lastExitCode = 1
        } else {
            $succeeded = $true
            $state.lastElementCount = $elementCount
        }
    } else {
        Write-MonitorLog -Phase "export" -Message "Attempt $attempt failed with exit code $lastExitCode."
    }

    if (-not $succeeded -and $attempt -lt $MaxRetries) {
        $delay = $RetryDelaySeconds * $attempt
        Write-MonitorLog -Phase "export" -Message "Retrying in $delay seconds."
        Start-Sleep -Seconds $delay
    }
}

$exportStopwatch.Stop()
$state.lastExitCode = $lastExitCode

if ($succeeded) {
    $wasFailing = $state.consecutiveFailures -gt 0
    $state.lastSuccessTime = (Get-Date).ToString("o")
    $state.consecutiveFailures = 0
    $state.runsSinceForce = if ($effectiveForce) { 0 } else { $runsSinceForce + 1 }
    if ($wasFailing) {
        Send-Alert -Kind Recovery -Message "Export succeeded after $($attempt) attempt(s), recovering from a prior failure."
    }

    $diagramCount = Get-DiagramCount
    $pageDelta = $elementCount - $previousCount
    $deltaLabel = if ($pageDelta -ge 0) { "+$pageDelta" } else { "$pageDelta" }
    $state.lastDiagramCount = $diagramCount

    if ($NotifyOnStart) {
        Send-Alert -Kind Finish -Message ("Export finished in {0} - {1} page(s) total ({2} diagram, {3} element), {4} vs previous run." -f `
            $exportStopwatch.Elapsed.ToString('mm\:ss'), $elementCount, $diagramCount, ($elementCount - $diagramCount), $deltaLabel)
    }

    Write-MonitorLog -Phase "export" -Message "Succeeded on attempt $attempt in $($exportStopwatch.Elapsed.ToString('mm\:ss'))."
} else {
    $state.lastFailureTime = (Get-Date).ToString("o")
    $state.consecutiveFailures = [int]$state.consecutiveFailures + 1
    Write-MonitorLog -Phase "export" -Message "Gave up after $MaxRetries attempt(s)."
    $fence = [string][char]0x60 * 3
    $failureBody = "Export failed after $MaxRetries attempt(s) (exit code $lastExitCode).`n$fence`n$outputTail`n$fence"
    Send-Alert -Kind Failure -Message $failureBody
}

$state.pageReadsToday = [int]$state.pageReadsToday + (Get-NewPageReadCount)
$state.writebacksToday = [int]$state.writebacksToday + (Get-NewWritebackCount)

$today = (Get-Date).ToString("yyyy-MM-dd")
if ($state.lastDigestDate -and $state.lastDigestDate -ne $today) {
    Send-Alert -Kind DailyDigest -Message "Activity for $($state.lastDigestDate): ~$($state.pageReadsToday) wiki page read(s) (approximate), $($state.writebacksToday) write-back(s)."
    $state.pageReadsToday = 0
    $state.writebacksToday = 0
}
$state.lastDigestDate = $today

Update-HealthPage -State $state
Save-HealthState -State $state

# --- Serve watchdog: verify mkdocs is still up, restart if it died since the last pass. ---
# The pid file stores PID + process start time (not just PID) so a stale file surviving
# a machine reboot can't produce a false "still alive" if the OS has since reused that PID
# for an unrelated process.
function Test-PortListening {
    param([int]$PortNumber)
    try {
        $conn = Get-NetTCPConnection -LocalPort $PortNumber -State Listen -ErrorAction Stop
        return $null -ne $conn
    } catch {
        # Get-NetTCPConnection may be unavailable; fall back to a direct TCP probe.
        try {
            $client = New-Object System.Net.Sockets.TcpClient
            $iar = $client.BeginConnect('127.0.0.1', $PortNumber, $null, $null)
            $success = $iar.AsyncWaitHandle.WaitOne(500)
            $client.Close()
            return $success
        } catch {
            return $false
        }
    }
}

function Test-ServeAlive {
    # First, does this monitor instance have a tracked, still-running serve process?
    if (Test-Path $servePidPath) {
        try {
            $info = Get-Content $servePidPath -Raw | ConvertFrom-Json
            if ($info.pid) {
                $proc = Get-Process -Id ([int]$info.pid) -ErrorAction SilentlyContinue
                if ($proc -and $info.startTime) {
                    $recordedStart = [DateTimeOffset]::Parse($info.startTime)
                    $actualStart = [DateTimeOffset]$proc.StartTime
                    if ([Math]::Abs(($recordedStart - $actualStart).TotalSeconds) -le 2) {
                        return $true
                    }
                }
            }
        } catch {
            # Corrupt or unreadable pid file - fall through to the port check below.
        }
    }

    # No (valid) tracked process - but something else may already be serving this port,
    # e.g. a manually started export-and-serve.ps1 that this monitor instance didn't launch.
    # Don't start a second mkdocs and collide on the port; leave an already-listening port alone.
    if (Test-PortListening -PortNumber $Port) {
        Write-MonitorLog -Phase "serve" -Message "Port $Port is already listening (untracked by this monitor instance); leaving it alone."
        return $true
    }
    return $false
}

function Start-Serve {
    $stamp = Get-Date -Format "yyyy-MM-dd_HHmmss"
    $outFile = Join-Path $logDir "serve-$stamp.out.log"
    $errFile = Join-Path $logDir "serve-$stamp.err.log"
    $psExe = (Get-Process -Id $PID).Path
    $serveScript = Join-Path $PSScriptRoot "serve.ps1"
    $proc = Start-Process -FilePath $psExe `
        -ArgumentList @("-NoProfile", "-File", $serveScript, "--port", $Port, "--wiki-dir", $wikiDir) `
        -RedirectStandardOutput $outFile -RedirectStandardError $errFile `
        -WindowStyle Hidden -PassThru -ErrorAction Stop
    [pscustomobject]@{ pid = $proc.Id; startTime = $proc.StartTime.ToString("o") } |
        ConvertTo-Json | Set-Content -Path $servePidPath
    return $proc
}

if (-not (Test-ServeAlive)) {
    Write-MonitorLog -Phase "serve" -Message "mkdocs serve is not running; attempting to (re)start."
    $serveAttempt = 0
    $serveUp = $false
    while ($serveAttempt -lt $MaxRetries -and -not $serveUp) {
        $serveAttempt++
        try {
            $proc = Start-Serve
            Start-Sleep -Seconds 5
            $serveUp = ($null -ne $proc) -and ($null -ne (Get-Process -Id $proc.Id -ErrorAction SilentlyContinue))
        } catch {
            Write-MonitorLog -Phase "serve" -Message "Start attempt $serveAttempt failed: $($_.Exception.Message)"
        }
        if (-not $serveUp -and $serveAttempt -lt $MaxRetries) {
            $delay = $RetryDelaySeconds * $serveAttempt
            Write-MonitorLog -Phase "serve" -Message "Retrying serve start in $delay seconds."
            Start-Sleep -Seconds $delay
        }
    }

    if ($serveUp) {
        $wasFailing = $state.serveConsecutiveFailures -gt 0
        $state.lastServeSuccessTime = (Get-Date).ToString("o")
        $state.serveConsecutiveFailures = 0
        Write-MonitorLog -Phase "serve" -Message "mkdocs serve started on attempt $serveAttempt."
        if ($wasFailing) {
            Send-Alert -Kind ServeRecovery -Message "mkdocs serve restarted successfully after $serveAttempt attempt(s)."
        }
    } else {
        $state.lastServeFailureTime = (Get-Date).ToString("o")
        $state.serveConsecutiveFailures = [int]$state.serveConsecutiveFailures + 1
        Write-MonitorLog -Phase "serve" -Message "Gave up starting mkdocs serve after $MaxRetries attempt(s)."
        Send-Alert -Kind ServeFailure -Message "mkdocs serve failed to start after $MaxRetries attempt(s)."
    }

    Update-HealthPage -State $state
    Save-HealthState -State $state
} else {
    Write-MonitorLog -Phase "serve" -Message "mkdocs serve already running."
}

Pop-Location

if (-not $succeeded) { exit 1 }
exit 0
