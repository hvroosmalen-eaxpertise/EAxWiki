Describe 'send-alert.ps1 Telegram dispatch' {
    It 'does not dispatch when no Telegram token/chat ID is given' {
        Mock Invoke-RestMethod { $script:calls++ }
        $script:calls = 0
        & "$PSScriptRoot\..\..\scripts\send-alert.ps1" -TelegramBotToken '' -TelegramChatId '' -Message 'x' -Kind Test
        $script:calls | Should -Be 0
    }

    It 'forwards bot token + chat id to bot{token}/sendMessage' {
        Mock Invoke-RestMethod { param($Uri, $Method, $Body, $ContentType) $global:tgUri = $Uri; $global:tgBody = $Body }
        & "$PSScriptRoot\..\..\scripts\send-alert.ps1" -TelegramBotToken '123456:ABC' -TelegramChatId '-1001234567890' -Message 'Export stopped by user.' -Kind UserStop
        $global:tgUri | Should -Be 'https://api.telegram.org/bot123456:ABC/sendMessage'
        $json = $global:tgBody | ConvertFrom-Json
        $json.chat_id | Should -Be '-1001234567890'
        $json.text | Should -Match '✋ \*EAxWiki \[UserStop\]\*'
        $json.text | Should -Match 'Export stopped by user.'
        $json.parse_mode | Should -Be 'Markdown'
    }
}
