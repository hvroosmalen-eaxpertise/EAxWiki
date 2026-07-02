# Unattended wrapper around export.ps1 / serve.ps1 for issue #37: log to a file,
# keep a health state file and wiki/status/health.md up to date, retry transient
# failures with backoff, and alert (webhook) on final give-up / recovery.
#
# Intended to be triggered periodically (see register-scheduled-task.ps1). Each
# invocation does one pass: pre-flight EA.exe cleanup, export with bounded retry,
# health-page update, and a serve health check (restarting mkdocs if it died since
# the last pass). It does not block on `mkdocs serve` itself.
#
# Usage:
#   .\scripts\monitor-export-and-serve.ps1
#   .\scripts\monitor-export-and-serve.ps1 --repo "model/file.qea" --output "wiki" --port 8000 `
#       --max-retries 3 --retry-delay 30 --min-element-fraction 0.5 --webhook-url "https://..."

$RepoPath            = ""
$OutputDir           = ""     # defaults to <repo-root>\wiki when not specified
$Port                = 8000
$MaxRetries          = 3
$RetryDelaySeconds   = 30
$MinElementFraction  = 0.5    # sanity floor: alert if output shrinks below this fraction of the previous successful run
$WebhookUrl          = $env:EAXWIKI_ALERT_WEBHOOK

$i = 0
while ($i -lt $args.Count) {
    switch -Regex ($args[$i]) {
        '^(-r|--repo|-RepoPath)$'                { $i++; if ($i -lt $args.Count) { $RepoPath           = $args[$i] } }
        '^(-o|--output|-OutputDir)$'             { $i++; if ($i -lt $args.Count) { $OutputDir          = $args[$i] } }
        '^(-p|--port|-Port)$'                    { $i++; if ($i -lt $args.Count) { $Port               = [int]$args[$i] } }
        '^(--max-retries|-MaxRetries)$'          { $i++; if ($i -lt $args.Count) { $MaxRetries          = [int]$args[$i] } }
        '^(--retry-delay|-RetryDelaySeconds)$'   { $i++; if ($i -lt $args.Count) { $RetryDelaySeconds   = [int]$args[$i] } }
        '^(--min-element-fraction)$'             { $i++; if ($i -lt $args.Count) { $MinElementFraction = [double]$args[$i] } }
        '^(--webhook-url|-WebhookUrl)$'          { $i++; if ($i -lt $args.Count) { $WebhookUrl          = $args[$i] } }
        default                                  { if (-not "$($args[$i])".StartsWith('-')) { $RepoPath = $args[$i] } }
    }
    $i++
}

if (-not $IsWindows) {
    Write-Error "Monitoring requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

# Per-instance state, keyed by a hash of the resolved wiki output dir. This lives OUTSIDE
# $wikiDir on purpose: the exporter's orphan cleanup (InfrastructureWriter.CleanupOrphanedFilesAsync)
# deletes any top-level directory in the output dir that isn't a recognized package or one of its
# special dirs (diagrams/types/glossary/recent/status) on every run — a log directory placed inside
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

function Get-HealthState {
    if (Test-Path $healthPath) {
        try { return Get-Content $healthPath -Raw | ConvertFrom-Json } catch {}
    }
    return [pscustomobject]@{
        lastSuccessTime       = $null
        lastFailureTime       = $null
        consecutiveFailures   = 0
        lastExitCode          = $null
        lastElementCount      = $null
        serveConsecutiveFailures = 0
        lastServeFailureTime  = $null
        lastServeSuccessTime  = $null
    }
}

function Save-HealthState {
    param($State)
    $State | ConvertTo-Json | Set-Content -Path $healthPath
}

function Send-Alert {
    param(
        [string]$Message,
        [ValidateSet('Failure', 'Recovery', 'ServeFailure', 'ServeRecovery')]
        [string]$Kind
    )
    Write-MonitorLog -Phase "alert" -Message "[$Kind] $Message"
    if (-not $WebhookUrl) {
        Write-MonitorLog -Phase "alert" -Message "No webhook URL configured (--webhook-url or EAXWIKI_ALERT_WEBHOOK); alert logged only."
        return
    }
    $payload = @{ text = "**EAxWiki [$Kind]** — $Message" } | ConvertTo-Json
    try {
        Invoke-RestMethod -Uri $WebhookUrl -Method Post -Body $payload -ContentType 'application/json' | Out-Null
    } catch {
        Write-MonitorLog -Phase "alert" -Message "Webhook dispatch failed: $($_.Exception.Message)"
    }
}

function Get-ElementCount {
    # Basic output-size sanity signal: count of generated markdown pages.
    if (-not (Test-Path $wikiDir)) { return 0 }
    return @(Get-ChildItem -Path $wikiDir -Filter '*.md' -Recurse -File -ErrorAction SilentlyContinue).Count
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
        "*Generated by monitor-export-and-serve.ps1 — reports export/serve pipeline status, not EA model element status.*"
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
        "| Last element count | $($State.lastElementCount) |"
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

$state = Get-HealthState

# --- Export with bounded retry + backoff. ---
$exportArgs = @("--output", $wikiDir)
if ($RepoPath) { $exportArgs += "--repo", $RepoPath }

$attempt = 0
$succeeded = $false
$lastExitCode = $null
$outputTail = ""

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

$state.lastExitCode = $lastExitCode

if ($succeeded) {
    $wasFailing = $state.consecutiveFailures -gt 0
    $state.lastSuccessTime = (Get-Date).ToString("o")
    $state.consecutiveFailures = 0
    if ($wasFailing) {
        Send-Alert -Kind Recovery -Message "Export succeeded after $($attempt) attempt(s), recovering from a prior failure."
    }
    Write-MonitorLog -Phase "export" -Message "Succeeded on attempt $attempt."
} else {
    $state.lastFailureTime = (Get-Date).ToString("o")
    $state.consecutiveFailures = [int]$state.consecutiveFailures + 1
    Write-MonitorLog -Phase "export" -Message "Gave up after $MaxRetries attempt(s)."
    Send-Alert -Kind Failure -Message "Export failed after $MaxRetries attempt(s) (exit code $lastExitCode).`n``````n$outputTail`n``````"
}

Update-HealthPage -State $state
Save-HealthState -State $state

# --- Serve watchdog: verify mkdocs is still up, restart if it died since the last pass. ---
function Test-ServeAlive {
    if (-not (Test-Path $servePidPath)) { return $false }
    $pid_ = Get-Content $servePidPath -Raw -ErrorAction SilentlyContinue
    if (-not $pid_) { return $false }
    $proc = Get-Process -Id ([int]$pid_.Trim()) -ErrorAction SilentlyContinue
    return $null -ne $proc
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
    Set-Content -Path $servePidPath -Value $proc.Id
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
