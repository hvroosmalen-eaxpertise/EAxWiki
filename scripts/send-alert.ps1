param(
    [string]$WebhookUrl = "",
    [string]$TeamsWebhookUrl = "",
    [string]$Message = "",
    [string]$Kind = "Test"
)

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
