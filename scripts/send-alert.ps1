param(
    [string]$WebhookUrl = "",
    [string]$TeamsWebhookUrl = "",
    [string]$TelegramBotToken = "",
    [string]$TelegramChatId = "",
    [string]$Message = "",
    [string]$Kind = "Test"
)

# Kept in sync with EAxWiki.Monitor AlertDispatcher (issue #80 parity work).
function ConvertTo-HtmlEscaped {
    param([string]$Text)
    if ($null -eq $Text) { return "" }
    return (($Text -replace '&', '&amp;') -replace '<', '&lt;') -replace '>', '&gt;'
}

# Kept in sync with EAxWiki.Monitor TelegramAlertTextFormatter (issue #80 parity work).
function Format-TelegramAlertText {
    param(
        [string]$Kind,
        [string]$InstanceLabel,
        [string]$Message,
        [datetime]$Timestamp = (Get-Date)
    )
    $emoji = switch ($Kind) {
        'Start'         { '🔄' }
        'Finish'        { '🟢' }
        'Failure'       { '🔴' }
        'ServeFailure'  { '🔴' }
        'LlmFailure'    { '🔴' }
        'ApiFailure'    { '🔴' }
        'Recovery'      { '🟢' }
        'ServeRecovery' { '🟢' }
        'LlmRecovery'   { '🟢' }
        'ApiRecovery'   { '🟢' }
        'Test'          { '🔵' }
        'DailyDigest'   { '📊' }
        'UserStop'      { '✋' }
        default         { '🔵' }
    }
    $labelHtml = ConvertTo-HtmlEscaped $InstanceLabel
    $kindHtml  = ConvertTo-HtmlEscaped $Kind
    $stamp     = $Timestamp.ToString('yyyy-MM-dd HH:mm:ss zzz')
    $preBlocks = New-Object System.Collections.Generic.List[string]
    $withPlaceholders = [regex]::Replace($Message, '(?s)```(.*?)```', {
        param($m)
        $preBlocks.Add('<pre>' + (ConvertTo-HtmlEscaped $m.Groups[1].Value) + '</pre>')
        "`u{FFFD}PRE$($preBlocks.Count - 1)`u{FFFD}"
    })
    $escaped = ConvertTo-HtmlEscaped $withPlaceholders
    $bodyHtml = [regex]::Replace($escaped, "`u{FFFD}PRE(\d+)`u{FFFD}", {
        param($m)
        $preBlocks[[int]$m.Groups[1].Value]
    })
    $composed = "{0} <b>EAxWiki [{1}]</b> — {2}`n{3}`n`n<i>{2} • {4}</i>" -f `
        $emoji, $kindHtml, $labelHtml, $bodyHtml, $stamp
    if ($composed.Length -gt 4000) {
        $composed = $composed.Substring(0, 4000) + "`n... (truncated)"
    }
    return $composed
}

$instanceLabel = "$env:COMPUTERNAME - SchedulerUI"

$color = switch ($Kind) {
    'Start'         { '#3aa3e3' }
    'Finish'        { '#28a745' }
    'Failure'       { '#dc3545' }
    'ServeFailure'  { '#dc3545' }
    'Recovery'      { '#28a745' }
    'ServeRecovery' { '#28a745' }
    'Test'          { '#3aa3e3' }
    'DailyDigest'   { '#3aa3e3' }
    'UserStop'      { '#FF8C00' }
    default         { '#3aa3e3' }
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
    'UserStop'      { ':raised_hand:' }
    default         { ':large_blue_circle:' }
}

if ($WebhookUrl) {
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
    } catch {
        Write-Host "Slack webhook dispatch failed: $($_.Exception.Message)"
    }
}

if ($TeamsWebhookUrl) {
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
    } catch {
        Write-Host "Teams webhook dispatch failed: $($_.Exception.Message)"
    }
}

if ($TelegramBotToken -and $TelegramChatId) {
    $tgText = Format-TelegramAlertText -Kind $Kind -InstanceLabel $instanceLabel -Message $Message
    $tgUri = "https://api.telegram.org/bot{0}/sendMessage" -f $TelegramBotToken
    $tgBody = @{
        chat_id    = [string]$TelegramChatId
        text       = $tgText
        parse_mode = 'HTML'
    }

    $attempts = 0
    while ($true) {
        $attempts++
        try {
            Invoke-RestMethod -Uri $tgUri -Method Post -Body ($tgBody | ConvertTo-Json) -ContentType 'application/json; charset=utf-8' | Out-Null
            Write-Host "Telegram dispatched."
            break
        } catch {
            $status = $null
            if ($_.Exception.Response) { $status = $_.Exception.Response.StatusCode }
            elseif ($_.Exception.StatusCode) { $status = $_.Exception.StatusCode }
            if ($null -ne $status -and [int]$status -eq 400 -and $attempts -eq 1 -and $tgBody.ContainsKey('parse_mode')) {
                $tgBody.Remove('parse_mode')
                continue
            }
            Write-Host "Telegram dispatch failed: $($_.Exception.Message)"
            break
        }
    }
}
