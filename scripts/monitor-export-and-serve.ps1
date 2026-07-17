. $PSScriptRoot\_bootstrap.ps1

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
    $ExportIntervalMinutes = 30
    $MonitorCheckIntervalSeconds = 30

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
            '^(--export-interval|-ExportIntervalMinutes)$' { $i++; if ($i -lt $Arguments.Count) { $ExportIntervalMinutes = [int]$Arguments[$i] } }
            '^(--check-interval|-MonitorCheckIntervalSeconds)$' { $i++; if ($i -lt $Arguments.Count) { $MonitorCheckIntervalSeconds = [int]$Arguments[$i] } }
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
        ExportIntervalMinutes = $ExportIntervalMinutes
        MonitorCheckIntervalSeconds = $MonitorCheckIntervalSeconds
    }
}

function ConvertTo-RedactedConnectionString {
    param([string]$ConnectionString)
    if ([string]::IsNullOrEmpty($ConnectionString)) { return "" }
    if (-not $ConnectionString.Contains('=')) { return $ConnectionString }
    return $ConnectionString -replace '(?i)(Password|Pwd)\s*=[^;]*', '$1=***'
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
$ExportIntervalMinutes = $parsed.ExportIntervalMinutes
$MonitorCheckIntervalSeconds = $parsed.MonitorCheckIntervalSeconds

if (-not $IsWindowsOS) {
    Write-Error "Monitoring requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

# Resolve both webhook URLs from CLI arg â†’ env var â†’ .eaxwiki file. .eaxwiki is decrypted at
# most once (not once per channel) and shared between the two lookups below.
$eaxwikiConfig = $null
if (Test-Path ".eaxwiki") {
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

if (($null -eq $RepoPath -or "" -eq $RepoPath) -and $eaxwikiConfig -and $eaxwikiConfig.repoPath) {
    $RepoPath = $eaxwikiConfig.repoPath
}

# Resolve all service settings from .eaxwiki.
$LlamaExePath     = if ($eaxwikiConfig -and $eaxwikiConfig.llamaExePath)     { $eaxwikiConfig.llamaExePath }     else { $null }
$LlamaModelPath   = if ($eaxwikiConfig -and $eaxwikiConfig.llamaModelPath)   { $eaxwikiConfig.llamaModelPath }   else { $null }
$AiMode           = if ($eaxwikiConfig -and $eaxwikiConfig.aiMode)           { $eaxwikiConfig.aiMode }           else { "none" }
$AiEndpoint       = if ($eaxwikiConfig -and $eaxwikiConfig.aiEndpoint)       { $eaxwikiConfig.aiEndpoint }       else { $null }
$AiModel          = if ($eaxwikiConfig -and $eaxwikiConfig.aiModel)          { $eaxwikiConfig.aiModel }          else { $null }
$ApiPort          = if ($eaxwikiConfig -and $eaxwikiConfig.apiPort)          { $eaxwikiConfig.apiPort }          else { 0 }
$WikiPort         = if ($eaxwikiConfig -and $eaxwikiConfig.wikiPort)         { $eaxwikiConfig.wikiPort }         else { 8000 }
if ($Port -eq 8000 -and $WikiPort -ne 8000) { $Port = $WikiPort }

# Default LLM paths (matching SchedulerForm's defaults).
$LlamaExePathDefault   = "E:\llama-cpp\llama-server.exe"
$LlamaModelPathDefault = "E:\models\llama-3.2-3b-q4.gguf"
if (-not $LlamaExePath)   { $LlamaExePath   = $LlamaExePathDefault }
if (-not $LlamaModelPath) { $LlamaModelPath = $LlamaModelPathDefault }

# Infer AiMode if not explicitly saved in .eaxwiki.
if (($null -eq $AiMode -or $AiMode -eq "none") -and $AiEndpoint) {
    if ($AiEndpoint -like "http://localhost*" -or $AiEndpoint -like "http://127.0.0.1*") {
        if ($LlamaExePath -and (Test-Path $LlamaExePath -ErrorAction SilentlyContinue)) {
            $AiMode = "local"
        }
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
$instanceHash = ($md5.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($wikiDir.ToLowerInvariant())) | ForEach-Object { $_.ToString("x2") }) -join ''
$instanceHash = $instanceHash.Substring(0, 12)
$stateDir = Join-Path $repoRoot ".eaxwiki-monitor\$instanceHash"
$healthPath  = Join-Path $stateDir "health.json"
$servePidPath    = Join-Path $stateDir "serve.pid"
$llmPidPath      = Join-Path $stateDir "llm.pid"
$apiPidPath      = Join-Path $stateDir "api.pid"
$monitorPidPath  = Join-Path $stateDir "monitor.pid"
$logDir      = Join-Path $stateDir "logs"
if (-not (Test-Path $logDir)) { New-Item -ItemType Directory -Path $logDir -Force | Out-Null }
$logPath     = Join-Path $logDir ("monitor-{0:yyyy-MM-dd}.log" -f (Get-Date))

$templateDir     = Join-Path $repoRoot ".eaxwiki-monitor"
$healthTemplate  = Join-Path $templateDir "health-template.md"
$digestTemplate  = Join-Path $templateDir "digest-template.md"

function Write-MonitorLog {
    param([string]$Phase, [string]$Message)
    $line = "{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}" -f (Get-Date), $Phase, $Message
    Add-Content -Path $logPath -Value $line
    Write-Host $line
}

Write-MonitorLog -Phase "config" -Message "Repo: $(ConvertTo-RedactedConnectionString $RepoPath)"
Write-MonitorLog -Phase "config" -Message "ApiPort=$ApiPort WikiPort=$Port AiEndpoint=$AiEndpoint LlamaExePath=$LlamaExePath"
if ($AiMode -eq "local" -and $null -eq $eaxwikiConfig.aiMode) {
    Write-MonitorLog -Phase "config" -Message "Inferred AiMode=local from AiEndpoint=$AiEndpoint and existing LlamaExePath."
}

# Prevent duplicate monitor instances via PID file.
if (Test-Path $monitorPidPath) {
    $existingPid = Get-Content $monitorPidPath -Raw -ErrorAction SilentlyContinue
    if ($existingPid -match '^\d+$') {
        $existingProc = Get-Process -Id $existingPid -ErrorAction SilentlyContinue
        if ($existingProc -and $existingProc.Id -ne $PID) {
            Write-MonitorLog -Phase "monitor" -Message "Duplicate monitor detected (PID $existingPid already running). Exiting."
            exit 0
        }
    }
    Remove-Item $monitorPidPath -Force -ErrorAction SilentlyContinue
}
try {
    $PID | Out-File -FilePath $monitorPidPath -NoNewline -ErrorAction Stop
} catch {
    Write-MonitorLog -Phase "monitor" -Message "Failed to write monitor PID file: $($_.Exception.Message)"
    exit 1
}
Register-EngineEvent -SourceIdentifier PowerShell.Exiting -Action { Remove-Item $monitorPidPath -Force -ErrorAction SilentlyContinue } | Out-Null

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
        llmConsecutiveFailures = 0
        lastLlmFailureTime    = $null
        lastLlmSuccessTime    = $null
        apiConsecutiveFailures = 0
        lastApiFailureTime    = $null
        lastApiSuccessTime    = $null
        # Tracks the ApiPort used during the last export; a change triggers a forced full rebuild
        # so inline editor widgets are embedded in pages that weren't regenerated incrementally.
        lastApiPort           = 0
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
        # User-initiated stop flags (issue #XX): set by SchedulerUI's stop buttons,
        # cleared by Enable or by the run itself (skipExport is one-shot, skipServe persists).
        skipExport            = $false
        skipServe             = $false
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
        [ValidateSet('Start', 'Finish', 'Failure', 'Recovery', 'ServeFailure', 'ServeRecovery', 'LlmFailure', 'LlmRecovery', 'ApiFailure', 'ApiRecovery', 'Test', 'DailyDigest', 'UserStop')]
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
        'LlmFailure'    { '#dc3545' }
        'ApiFailure'    { '#dc3545' }
        'Recovery'      { '#28a745' } # green
        'ServeRecovery' { '#28a745' }
        'LlmRecovery'   { '#28a745' }
        'ApiRecovery'   { '#28a745' }
        'Test'          { '#3aa3e3' } # blue
        'DailyDigest'   { '#3aa3e3' } # blue
        'UserStop'      { '#FF8C00' } # orange
    }
    $emoji = switch ($Kind) {
        'Start'         { ':arrows_counterclockwise:' }
        'Finish'        { ':large_green_circle:' }
        'Failure'       { ':red_circle:' }
        'ServeFailure'  { ':red_circle:' }
        'LlmFailure'    { ':red_circle:' }
        'ApiFailure'    { ':red_circle:' }
        'Recovery'      { ':large_green_circle:' }
        'ServeRecovery' { ':large_green_circle:' }
        'LlmRecovery'   { ':large_green_circle:' }
        'ApiRecovery'   { ':large_green_circle:' }
        'Test'          { ':large_blue_circle:' }
        'DailyDigest'   { ':bar_chart:' }
        'UserStop'      { ':raised_hand:' }
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

function Get-NewWritebackSummary {
    # WikiWritebackServer.cs (LogWriteback) appends one line per successful write-back to
    # status/writeback.log with format: "{timestamp} {kind}" where kind is one of:
    # status, notes, diagram-notes, or row-notes:{kind}.
    # Returns a hashtable with total count and per-kind breakdown.
    $writebackLogPath = Join-Path $wikiDir "status\writeback.log"
    if ($state.writebackLogFile -ne $writebackLogPath) {
        $state.writebackLogFile = $writebackLogPath
        $state.writebackLogOffset = 0
    }
    $newText = Read-NewLogText -Path $writebackLogPath -OffsetProperty 'writebackLogOffset'
    if (-not $newText) { return @{ Total = 0; Kinds = @{} } }

    $kinds = @{}
    $total = 0
    foreach ($line in ($newText -split "`r?`n")) {
        $trimmed = $line.Trim()
        if ($trimmed.Length -eq 0) { continue }
        $total++
        # Format: "2026-07-08 15:30:00 status"
        $parts = $trimmed -split '\s+', 3
        if ($parts.Count -ge 3) {
            $kind = $parts[2]
            $kinds[$kind] = [int]$kinds[$kind] + 1
        }
    }
    return @{ Total = $total; Kinds = $kinds }
}

function Get-NewWritebackCount {
    $summary = Get-NewWritebackSummary
    return $summary.Total
}

function Update-HealthPage {
    param($State)
    $monitorDir = Join-Path (Join-Path $PSScriptRoot "..") ".eaxwiki-monitor"
    $statusDir = Join-Path $monitorDir "status"
    if (-not (Test-Path $statusDir)) { New-Item -ItemType Directory -Path $statusDir -Force | Out-Null }

    $overall = if ($State.consecutiveFailures -eq 0 -and $State.serveConsecutiveFailures -eq 0 -and $State.llmConsecutiveFailures -eq 0 -and $State.apiConsecutiveFailures -eq 0) { "Healthy" }
               else { "Degraded" }

    $template = Get-Content $healthTemplate -Raw
    $lines = $template `
        -replace '@@OVERALL@@', $overall `
        -replace '@@LAST_SUCCESS_TIME@@', "$($State.lastSuccessTime)" `
        -replace '@@LAST_FAILURE_TIME@@', "$($State.lastFailureTime)" `
        -replace '@@CONSECUTIVE_FAILURES@@', "$($State.consecutiveFailures)" `
        -replace '@@LAST_EXIT_CODE@@', "$($State.lastExitCode)" `
        -replace '@@LAST_ELEMENT_COUNT@@', "$($State.lastElementCount)" `
        -replace '@@LAST_DIAGRAM_COUNT@@', "$($State.lastDiagramCount)" `
        -replace '@@LAST_MODE@@', "$($State.lastMode)" `
        -replace '@@RUNS_SINCE_FORCE@@', "$($State.runsSinceForce)" `
        -replace '@@LAST_SERVE_SUCCESS_TIME@@', "$($State.lastServeSuccessTime)" `
        -replace '@@LAST_SERVE_FAILURE_TIME@@', "$($State.lastServeFailureTime)" `
        -replace '@@SERVE_CONSECUTIVE_FAILURES@@', "$($State.serveConsecutiveFailures)" `
        -replace '@@LAST_LLM_SUCCESS_TIME@@', "$($State.lastLlmSuccessTime)" `
        -replace '@@LAST_LLM_FAILURE_TIME@@', "$($State.lastLlmFailureTime)" `
        -replace '@@LLM_CONSECUTIVE_FAILURES@@', "$($State.llmConsecutiveFailures)" `
        -replace '@@LAST_API_SUCCESS_TIME@@', "$($State.lastApiSuccessTime)" `
        -replace '@@LAST_API_FAILURE_TIME@@', "$($State.lastApiFailureTime)" `
        -replace '@@API_CONSECUTIVE_FAILURES@@', "$($State.apiConsecutiveFailures)"
    Set-Content -Path (Join-Path $statusDir "health.md") -Value $lines
}
$state = Get-HealthState

$lastExportTime = [DateTime]::MinValue

function Test-EditLock {
    param([string]$WikiDir)
    $lockPath = Join-Path (Join-Path (Split-Path $WikiDir -Parent) ".data") "edit-lock.json"
    if (-not (Test-Path $lockPath)) { return $false }

    try {
        $lock = Get-Content $lockPath -Raw | ConvertFrom-Json
        if (-not $lock.Active) { return $false }

        $expires = [DateTime]::Parse($lock.ExpiresAt, [CultureInfo]::InvariantCulture, [System.Globalization.DateTimeStyles]::AssumeUniversal)
        if ([DateTime]::UtcNow -gt $expires) {
            Write-MonitorLog -Phase "export" -Message "Edit-lock expired; clearing stale lock."
            Remove-Item $lockPath -Force -ErrorAction SilentlyContinue
            return $false
        }

        return $true
    } catch {
        Write-MonitorLog -Phase "export" -Message "Failed to read edit-lock: $($_.Exception.Message)"
        return $false
    }
}

function Stop-ApiServer {
    # Gracefully stop the tracked API server process so its DLL locks are released
    # before a dotnet-run export can overwrite them.  Falls back to Clear-Port if
    # the tracked process is gone but something else still holds the port.
    if (Test-Path $apiPidPath) {
        try {
            $info = Get-Content $apiPidPath -Raw | ConvertFrom-Json
            if ($info.pid) {
                $proc = Get-Process -Id ([int]$info.pid) -ErrorAction SilentlyContinue
                if ($proc) {
                    Write-MonitorLog -Phase "api" -Message "Stopping tracked API server (PID $($info.pid)) to release DLL locks."
                    $proc.CloseMainWindow() | Out-Null
                    # Give it up to 5 seconds to exit gracefully.
                    $deadline = [DateTime]::UtcNow.AddSeconds(5)
                    while ([DateTime]::UtcNow -lt $deadline) {
                        if ($proc.HasExited) { break }
                        Start-Sleep -Milliseconds 200
                    }
                    if (-not $proc.HasExited) {
                        Write-MonitorLog -Phase "api" -Message "API server did not exit gracefully; force-killing."
                        $proc | Stop-Process -Force -ErrorAction SilentlyContinue
                        Start-Sleep -Seconds 1
                    }
                }
            }
        } catch { }
        # Always clear the pid file so the watchdog doesn't think it's still alive.
        Remove-Item $apiPidPath -Force -ErrorAction SilentlyContinue
    }
    # Release the port if anything still holds it.
    Clear-Port -PortNumber $ApiPort
}

function Clear-Port {
    param([int]$PortNumber)
    $owner = Get-NetTCPConnection -LocalPort $PortNumber -ErrorAction SilentlyContinue |
        Select-Object -First 1 -ExpandProperty OwningProcess
    if ($owner) {
        Write-MonitorLog -Phase "api" -Message "Killing process $owner on port $PortNumber."
        Stop-Process -Id $owner -Force -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 2
    }
}

function Test-ApiAlive {
    if (Test-Path $apiPidPath) {
        try {
            $info = Get-Content $apiPidPath -Raw | ConvertFrom-Json
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
        } catch { }
    }
    # Port is occupied by an untracked or stale process — don't report alive; the start loop
    # below will kill the occupant and replace it.
    return $false
}

while ($true) {
    $exportDue = $lastExportTime -eq [DateTime]::MinValue -or (([DateTime]::UtcNow - $lastExportTime).TotalMinutes -ge $ExportIntervalMinutes)

    # Clear user-initiated skip flags at the start of every run so a prior Stop action
    # doesn't permanently disable services (skipExport is one-shot per design, skipServe
    # is service-lifecycle scoped - both get reset here).
    $state.skipExport = $false
    $state.skipServe = $false

    # --force is off by default here, same as export.ps1 itself - a 30-minute-cadence schedule
    # doing a full rebuild every run would be needlessly slow against a large model. --force-every
    # lets an otherwise-incremental schedule periodically self-correct any drift a single run's
    # incremental diff might miss, without forcing every run.
    $runsSinceForce = if ($state.runsSinceForce) { [int]$state.runsSinceForce } else { 0 }

    # --- Edit-lock check: defer export if a user is actively editing. ---
    if ($exportDue) {
        if (Test-EditLock -WikiDir $wikiDir) {
            Write-MonitorLog -Phase "export" -Message "Deferring export - edit in progress, retry next cycle."
            $exportDue = $false
        }
    }

    if ($exportDue) {
        $effectiveForce = $true
        Write-MonitorLog -Phase "config" -Message "Full export (monitor always rebuilds)."

        if ($NotifyOnStart) {
            Send-Alert -Kind Start -Message "Scheduled run starting (forced full rebuild)."
        }

        $eaPidsBefore = @(Get-Process EA -ErrorAction SilentlyContinue | ForEach-Object { $_.Id })

        # --- Export with bounded retry + backoff. ---
        $skipPhase = $false
        if ($state.skipExport) {
            Write-MonitorLog -Phase "export" -Message "Skipped by user request (skipExport flag)."
            Send-Alert -Kind UserStop -Message "Export skipped by user request."
            $state.skipExport = $false
            $skipPhase = $true
            $succeeded = $true
            $lastExitCode = 0
            $elementCount = if ($state.lastElementCount) { [int]$state.lastElementCount } else { 0 }
            $previousCount = $elementCount
        }

        $writebackSummary = Get-NewWritebackSummary

        if (-not $skipPhase) {
            # Stop the API server before export so its DLL locks don't prevent
            # dotnet-run from overwriting the binaries.  The API watchdog section
            # at the end of this pass will restart it automatically.
            if ($ApiPort -gt 0 -and (Test-ApiAlive)) {
                Stop-ApiServer
            }

            $exportArgs = @("--output", $wikiDir)
            if ($RepoPath) { $exportArgs += "--repo", $RepoPath }
            if ($effectiveForce) { $exportArgs += "--force" }
            if ($ApiPort -gt 0) {
                $exportArgs += "--writeback", "--api-port", "$ApiPort"
            }
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

                $output = & $PSScriptRoot\export.ps1 @exportArgs
                $lastExitCode = 1
                foreach ($line in $output) {
                    if ($line -match '^EAXWIKI_EXIT_CODE=(\d+)$') {
                        $lastExitCode = [int]$Matches[1]
                    }
                }
                $outputTail = ($output | Where-Object { $_ -notmatch '^EAXWIKI_EXIT_CODE=' } | Select-Object -Last 20) -join "`n"

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
                $state.lastApiPort = $ApiPort
                if ($wasFailing) {
                    Send-Alert -Kind Recovery -Message "Export succeeded after $($attempt) attempt(s), recovering from a prior failure."
                }

                $diagramCount = Get-DiagramCount
                $pageDelta = $elementCount - $previousCount
                $deltaLabel = if ($pageDelta -ge 0) { "+$pageDelta" } else { "$pageDelta" }
                $state.lastDiagramCount = $diagramCount

                $validationSuffix = ""
                $validationReportPath = Join-Path $wikiDir ".validation-report.json"
                if (Test-Path $validationReportPath) {
                    try {
                        $vr = Get-Content $validationReportPath -Raw | ConvertFrom-Json
                        $parts = @()
                        if ([int]$vr.Errors -gt 0) { $parts += "$($vr.Errors) error(s)" }
                        if ([int]$vr.Warnings -gt 0) { $parts += "$($vr.Warnings) warning(s)" }
                        if ($parts.Count -gt 0) {
                            $validationSuffix = " - validation: $($parts -join ', ') ($($vr.Passed)/$($vr.FilesValidated) files clean)"
                        } else {
                            $validationSuffix = " - all $($vr.FilesValidated) files validated clean"
                        }
                    } catch {}
                }

                $writebackSuffix = ""
                if ($NotifyOnStart) {
                    if ($writebackSummary -and $writebackSummary.Total -gt 0) {
                        $parts = @()
                        foreach ($kv in $writebackSummary.Kinds.GetEnumerator() | Sort-Object { $_.Value } -Descending) {
                            $parts += "$($kv.Value) $($kv.Key)"
                        }
                        if ($parts.Count -gt 0) {
                            $writebackSuffix = " - write-backs: $($parts -join ', ')"
                        }
                    }
                    Send-Alert -Kind Finish -Message ("Export finished in {0} - {1} page(s) total ({2} diagram, {3} element), {4} vs previous run.{5}{6}" -f `
                        $exportStopwatch.Elapsed.ToString('mm\:ss'), $elementCount, $diagramCount, ($elementCount - $diagramCount), $deltaLabel, $validationSuffix, $writebackSuffix)
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
        } # end if (-not $skipPhase)

        $lastExportTime = [DateTime]::UtcNow
    } else {
        Write-MonitorLog -Phase "config" -Message "Skipping export (next due in $ExportIntervalMinutes min)."
        $writebackSummary = $null
        $succeeded = $true
    }

    # --- Daily activity digest ---
    $state.pageReadsToday = [int]$state.pageReadsToday + (Get-NewPageReadCount)
    $state.writebacksToday = [int]$state.writebacksToday + $writebackSummary.Total

    $today = (Get-Date).ToString("yyyy-MM-dd")
    if ($state.lastDigestDate -and $state.lastDigestDate -ne $today) {
        $digestContent = Get-Content $digestTemplate -Raw
        $digestMessage = $digestContent `
            -replace '@@DIGEST_DATE@@', "$($state.lastDigestDate)" `
            -replace '@@PAGE_READS_TODAY@@', "$($state.pageReadsToday)" `
            -replace '@@WRITEBACKS_TODAY@@', "$($state.writebacksToday)"
        Send-Alert -Kind DailyDigest -Message $digestMessage
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
    # Ensure status dir and placeholder files exist before mkdocs serve starts.
    # The API server writes api-ready on startup, but mkdocs serve copies all files
    # in wiki/ on its first build — if api-ready doesn't exist yet, mkdocs crashes
    # with FileNotFoundError.
    $statusDir = Join-Path $wikiDir "status"
    if (-not (Test-Path $statusDir)) { New-Item -ItemType Directory -Path $statusDir -Force | Out-Null }
    $apiReadyPath = Join-Path $statusDir "api-ready"
    if (-not (Test-Path $apiReadyPath)) { "placeholder" | Set-Content -Path $apiReadyPath -NoNewline }

    $stamp = Get-Date -Format "yyyy-MM-dd_HHmmss"
    $outFile = Join-Path $logDir "serve-$stamp.out.log"
    $errFile = Join-Path $logDir "serve-$stamp.err.log"
    $psExe = $PSExecutable
    $serveScript = Join-Path $PSScriptRoot "serve.ps1"
    $proc = Start-Process -FilePath $psExe `
        -ArgumentList @("-NoProfile", "-File", $serveScript, "--port", $Port, "--wiki-dir", $wikiDir) `
        -RedirectStandardOutput $outFile -RedirectStandardError $errFile `
        -WindowStyle Hidden -PassThru -ErrorAction Stop
    [pscustomobject]@{ pid = $proc.Id; startTime = $proc.StartTime.ToString("o") } |
        ConvertTo-Json | Set-Content -Path $servePidPath
    return $proc
}

if ($state.skipServe) {
    Write-MonitorLog -Phase "serve" -Message "Serve restart blocked by user (skipServe flag)."
} elseif (-not (Test-ServeAlive)) {
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

# --- API server watchdog: start the write-back API server if configured in .eaxwiki. ---
# The API server (EAxWiki.exe --api) runs persistently to handle wiki-editor write-backs
# and AI suggestion requests between export runs.
function Start-ApiServer {
    $stamp = Get-Date -Format "yyyy-MM-dd_HHmmss"
    $outFile = Join-Path $logDir "api-$stamp.out.log"
    $errFile = Join-Path $logDir "api-$stamp.err.log"
    $psExe = $PSExecutable
    $dllPath = Join-Path $repoRoot "src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll"
    $projDir = Join-Path $repoRoot "src\EAxWiki"
    $execArgs = @("exec", "--runtimeconfig", (Join-Path $projDir "bin\Debug\net10.0\EAxWiki.runtimeconfig.json"),
        "--depsfile", (Join-Path $projDir "bin\Debug\net10.0\EAxWiki.deps.json"),
        $dllPath, "--api", "--api-port", $ApiPort, "--wiki-port", $Port, "--output", $wikiDir)
    if ($RepoPath) { $execArgs += "--repo"; $execArgs += $RepoPath }
    $proc = Start-Process -FilePath $psExe `
        -ArgumentList @("-NoProfile", "-ExecutionPolicy", "Bypass", "-Command", "dotnet $execArgs") `
        -RedirectStandardOutput $outFile -RedirectStandardError $errFile `
        -WindowStyle Hidden -PassThru -ErrorAction Stop
    return $proc
}

if ($ApiPort -gt 0) {
    if (-not (Test-ApiAlive)) {
        Write-MonitorLog -Phase "api" -Message "API server not running; attempting to start on port $ApiPort."
        $apiAttempt = 0
        $apiUp = $false
        while ($apiAttempt -lt $MaxRetries -and -not $apiUp) {
            $apiAttempt++
            try {
                # Kill any stale occupant before each attempt.
                Clear-Port -PortNumber $ApiPort
                # Remove any stale ready-file from a prior crash, so we
                # can reliably detect the fresh write.
                $readyFile = Join-Path $wikiDir "status\api-ready"
                if (Test-Path $readyFile) { Remove-Item $readyFile -Force }

                $proc = Start-ApiServer

                # Poll for the ready file (written by the API server via
                # app.Lifetime.ApplicationStarted) with 1-second intervals.
                $ready = $false
                $timeout = [DateTime]::UtcNow.AddSeconds(120)
                while ([DateTime]::UtcNow -lt $timeout -and -not $ready) {
                    $ready = Test-Path $readyFile
                    if (-not $ready) { Start-Sleep -Seconds 1 }
                }
                $apiUp = $ready -and ($null -ne $proc) -and ($null -ne (Get-Process -Id $proc.Id -ErrorAction SilentlyContinue))
                if ($apiUp) {
                    [pscustomobject]@{ pid = $proc.Id; startTime = $proc.StartTime.ToString("o") } |
                        ConvertTo-Json | Set-Content -Path $apiPidPath -Force
                }
            } catch {
                Write-MonitorLog -Phase "api" -Message "Start attempt $apiAttempt failed: $($_.Exception.Message)"
            }
            if (-not $apiUp -and $apiAttempt -lt $MaxRetries) {
                $delay = $RetryDelaySeconds * $apiAttempt
                Write-MonitorLog -Phase "api" -Message "Retrying API start in $delay seconds."
                Start-Sleep -Seconds $delay
            }
        }
        if ($apiUp) {
            $wasFailing = $state.apiConsecutiveFailures -gt 0
            $state.lastApiSuccessTime = (Get-Date).ToString("o")
            $state.apiConsecutiveFailures = 0
            Write-MonitorLog -Phase "api" -Message "API server started on attempt $apiAttempt."
            if ($wasFailing) {
                Send-Alert -Kind ApiRecovery -Message "Write-back API server restarted successfully on port $ApiPort."
            }
        } else {
            $state.lastApiFailureTime = (Get-Date).ToString("o")
            $state.apiConsecutiveFailures = [int]$state.apiConsecutiveFailures + 1
            Write-MonitorLog -Phase "api" -Message "Gave up starting API server after $MaxRetries attempt(s)."
            Send-Alert -Kind ApiFailure -Message "Write-back API server failed to start after $MaxRetries attempt(s)."
        }
        Update-HealthPage -State $state
        Save-HealthState -State $state
    } else {
        Write-MonitorLog -Phase "api" -Message "API server already running on port $ApiPort."
    }
} else {
    Write-MonitorLog -Phase "api" -Message "API server not configured (ApiPort not set)."
}

# --- LLM server watchdog: start llama-server if AiMode is "local" in .eaxwiki. ---
# The LLM server provides AI suggestions via the wiki-editor's "Suggest" button.
$llmPort = 8080

function Test-LlmAlive {
    if (Test-Path $llmPidPath) {
        try {
            $info = Get-Content $llmPidPath -Raw | ConvertFrom-Json
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
        } catch { }
    }
    # Port occupied by untracked process — don't report alive; Clear-Port will replace it.
    return $false
}

function Start-Llm {
    $stamp = Get-Date -Format "yyyy-MM-dd_HHmmss"
    $outFile = Join-Path $logDir "llm-$stamp.out.log"
    $errFile = Join-Path $logDir "llm-$stamp.err.log"
    $proc = Start-Process -FilePath $LlamaExePath `
        -ArgumentList @("-m", $LlamaModelPath, "-c", "4096", "--port", $llmPort, "--n-gpu-layers", "0") `
        -RedirectStandardOutput $outFile -RedirectStandardError $errFile `
        -WindowStyle Hidden -PassThru -ErrorAction Stop
    [pscustomobject]@{ pid = $proc.Id; startTime = $proc.StartTime.ToString("o") } |
        ConvertTo-Json | Set-Content -Path $llmPidPath
    return $proc
}

if ($AiMode -eq "local") {
    if ($null -eq $LlamaExePath -or "" -eq $LlamaExePath) {
        Write-MonitorLog -Phase "llm" -Message "AiMode is 'local' but llamaExePath not set in .eaxwiki; skipping."
    } elseif ($null -eq $LlamaModelPath -or "" -eq $LlamaModelPath) {
        Write-MonitorLog -Phase "llm" -Message "AiMode is 'local' but llamaModelPath not set in .eaxwiki; skipping."
    } elseif (-not (Test-Path $LlamaExePath)) {
        Write-MonitorLog -Phase "llm" -Message "LLM server not found at $LlamaExePath; skipping."
    } elseif (-not (Test-Path $LlamaModelPath)) {
        Write-MonitorLog -Phase "llm" -Message "LLM model not found at $LlamaModelPath; skipping."
    } else {
        if (-not (Test-LlmAlive)) {
            Write-MonitorLog -Phase "llm" -Message "LLM server not running; attempting to start."
            Clear-Port -PortNumber $llmPort
            $llmAttempt = 0
            $llmUp = $false
            while ($llmAttempt -lt $MaxRetries -and -not $llmUp) {
                $llmAttempt++
                try {
                    $proc = Start-Llm
                    Start-Sleep -Seconds 5
                    $llmUp = ($null -ne $proc) -and ($null -ne (Get-Process -Id $proc.Id -ErrorAction SilentlyContinue))
                } catch {
                    Write-MonitorLog -Phase "llm" -Message "Start attempt $llmAttempt failed: $($_.Exception.Message)"
                }
                if (-not $llmUp -and $llmAttempt -lt $MaxRetries) {
                    $delay = $RetryDelaySeconds * $llmAttempt
                    Write-MonitorLog -Phase "llm" -Message "Retrying LLM start in $delay seconds."
                    Start-Sleep -Seconds $delay
                }
            }
            if ($llmUp) {
                $wasFailing = $state.llmConsecutiveFailures -gt 0
                $state.lastLlmSuccessTime = (Get-Date).ToString("o")
                $state.llmConsecutiveFailures = 0
                Write-MonitorLog -Phase "llm" -Message "LLM server started (PID $($proc.Id)) on port $llmPort."
                if ($wasFailing) {
                    Send-Alert -Kind LlmRecovery -Message "LLM server restarted successfully on port $llmPort."
                }
            } else {
                $state.lastLlmFailureTime = (Get-Date).ToString("o")
                $state.llmConsecutiveFailures = [int]$state.llmConsecutiveFailures + 1
                Write-MonitorLog -Phase "llm" -Message "Gave up starting LLM server after $MaxRetries attempt(s)."
                Send-Alert -Kind LlmFailure -Message "LLM server failed to start after $MaxRetries attempt(s)."
            }
            Update-HealthPage -State $state
            Save-HealthState -State $state
        } else {
            Write-MonitorLog -Phase "llm" -Message "LLM server already running on port $llmPort."
        }
    }
    } else {
        Write-MonitorLog -Phase "llm" -Message "LLM not configured (AiMode=$AiMode)."
    }

    Write-MonitorLog -Phase "monitor" -Message "Sleeping for $MonitorCheckIntervalSeconds seconds."
    Start-Sleep -Seconds $MonitorCheckIntervalSeconds
} # end while ($true)

Pop-Location

Write-Host "Monitor stopped."
