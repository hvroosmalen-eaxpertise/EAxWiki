BeforeAll {
    . "$PSScriptRoot\..\..\scripts\monitor-export-and-serve.ps1"
}

Describe 'Get-MonitorArgs' {
    It 'returns defaults with no arguments' {
        $r = Get-MonitorArgs
        $r.RepoPath | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port | Should -Be 8000
        $r.MaxRetries | Should -Be 3
        $r.RetryDelaySeconds | Should -Be 30
        $r.MinElementFraction | Should -Be 0.5
        $r.WebhookUrl | Should -Be $null
        $r.TeamsWebhookUrl | Should -Be $null
        $r.TelegramBotToken | Should -Be $null
        $r.TelegramChatId | Should -Be $null
        $r.TestAlert | Should -Be $false
        $r.NotifyOnStart | Should -Be $true
        $r.Force | Should -Be $false
        $r.ForceEveryNRuns | Should -Be 0
    }

    It 'parses -r shorthand' { $r = Get-MonitorArgs -Arguments @('-r', 'model.qea'); $r.RepoPath | Should -Be 'model.qea' }
    It 'parses --repo' { $r = Get-MonitorArgs -Arguments @('--repo', 'model.qea'); $r.RepoPath | Should -Be 'model.qea' }
    It 'parses -RepoPath' { $r = Get-MonitorArgs -Arguments @('-RepoPath', 'model.qea'); $r.RepoPath | Should -Be 'model.qea' }
    It 'accepts connection string as repo' { $r = Get-MonitorArgs -Arguments @('DBType=postgresql;Database=foo'); $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo' }

    It 'parses -o shorthand' { $r = Get-MonitorArgs -Arguments @('-o', 'wiki'); $r.OutputDir | Should -Be 'wiki' }
    It 'parses --output' { $r = Get-MonitorArgs -Arguments @('--output', 'wiki'); $r.OutputDir | Should -Be 'wiki' }
    It 'parses --port' { $r = Get-MonitorArgs -Arguments @('--port', '8080'); $r.Port | Should -Be 8080 }
    It 'parses -p shorthand' { $r = Get-MonitorArgs -Arguments @('-p', '8080'); $r.Port | Should -Be 8080 }

    It 'parses --max-retries' { $r = Get-MonitorArgs -Arguments @('--max-retries', '5'); $r.MaxRetries | Should -Be 5 }
    It 'parses -MaxRetries' { $r = Get-MonitorArgs -Arguments @('-MaxRetries', '5'); $r.MaxRetries | Should -Be 5 }

    It 'parses --retry-delay' { $r = Get-MonitorArgs -Arguments @('--retry-delay', '60'); $r.RetryDelaySeconds | Should -Be 60 }
    It 'parses -RetryDelaySeconds' { $r = Get-MonitorArgs -Arguments @('-RetryDelaySeconds', '60'); $r.RetryDelaySeconds | Should -Be 60 }

    It 'parses --min-element-fraction' {
        $r = Get-MonitorArgs -Arguments @('--min-element-fraction', '0.75')
        $r.MinElementFraction | Should -Be 0.75
    }

    It 'parses --webhook-url' {
        $r = Get-MonitorArgs -Arguments @('--webhook-url', 'https://hooks.slack.com/abc')
        $r.WebhookUrl | Should -Be 'https://hooks.slack.com/abc'
    }

    It 'parses -WebhookUrl' {
        $r = Get-MonitorArgs -Arguments @('-WebhookUrl', 'https://hooks.slack.com/abc')
        $r.WebhookUrl | Should -Be 'https://hooks.slack.com/abc'
    }

    It 'parses --teams-webhook-url' {
        $r = Get-MonitorArgs -Arguments @('--teams-webhook-url', 'https://outlook.office.com/webhook/abc')
        $r.TeamsWebhookUrl | Should -Be 'https://outlook.office.com/webhook/abc'
    }

    It 'parses -TeamsWebhookUrl' {
        $r = Get-MonitorArgs -Arguments @('-TeamsWebhookUrl', 'https://outlook.office.com/webhook/abc')
        $r.TeamsWebhookUrl | Should -Be 'https://outlook.office.com/webhook/abc'
    }

    It 'parses --telegram-bot-token' {
        $r = Get-MonitorArgs -Arguments @('--telegram-bot-token', '123456789:AAbbCCddEeffGGhhIIjj')
        $r.TelegramBotToken | Should -Be '123456789:AAbbCCddEeffGGhhIIjj'
    }

    It 'parses -TelegramBotToken' {
        $r = Get-MonitorArgs -Arguments @('-TelegramBotToken', '123456789:AAbbCCddEeffGGhhIIjj')
        $r.TelegramBotToken | Should -Be '123456789:AAbbCCddEeffGGhhIIjj'
    }

    It 'parses --telegram-chat-id' {
        $r = Get-MonitorArgs -Arguments @('--telegram-chat-id', '-1001234567890')
        $r.TelegramChatId | Should -Be '-1001234567890'
    }

    It 'parses -TelegramChatId' {
        $r = Get-MonitorArgs -Arguments @('-TelegramChatId', '-1001234567890')
        $r.TelegramChatId | Should -Be '-1001234567890'
    }

    It 'parses --test-alert flag' { $r = Get-MonitorArgs -Arguments @('--test-alert'); $r.TestAlert | Should -Be $true }
    It 'parses -TestAlert flag' { $r = Get-MonitorArgs -Arguments @('-TestAlert'); $r.TestAlert | Should -Be $true }

    It 'parses --no-notify-start flag' { $r = Get-MonitorArgs -Arguments @('--no-notify-start'); $r.NotifyOnStart | Should -Be $false }

    It 'parses -f shorthand' { $r = Get-MonitorArgs -Arguments @('-f'); $r.Force | Should -Be $true }
    It 'parses --force' { $r = Get-MonitorArgs -Arguments @('--force'); $r.Force | Should -Be $true }
    It 'parses -Force' { $r = Get-MonitorArgs -Arguments @('-Force'); $r.Force | Should -Be $true }

    It 'parses --force-every' { $r = Get-MonitorArgs -Arguments @('--force-every', '48'); $r.ForceEveryNRuns | Should -Be 48 }

    It 'handles Unicode output dir' {
        $r = Get-MonitorArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
    }

    It 'handles empty webhook url' {
        $r = Get-MonitorArgs -Arguments @('--webhook-url', '')
        $r.WebhookUrl | Should -Be ''
    }

    It 'all flags combined' {
        $r = Get-MonitorArgs -Arguments @(
            '--repo', 'model.qea',
            '--output', 'wiki',
            '--port', '8080',
            '--max-retries', '5',
            '--retry-delay', '60',
            '--min-element-fraction', '0.75',
            '--webhook-url', 'https://hooks.slack.com/abc',
            '--teams-webhook-url', 'https://outlook.office.com/webhook/abc',
            '--telegram-bot-token', '123456789:AAbbCCddEeffGGhhIIjj',
            '--telegram-chat-id', '-1001234567890',
            '--test-alert',
            '--force',
            '--force-every', '48'
        )
        $r.RepoPath | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.Port | Should -Be 8080
        $r.MaxRetries | Should -Be 5
        $r.RetryDelaySeconds | Should -Be 60
        $r.MinElementFraction | Should -Be 0.75
        $r.WebhookUrl | Should -Be 'https://hooks.slack.com/abc'
        $r.TeamsWebhookUrl | Should -Be 'https://outlook.office.com/webhook/abc'
        $r.TelegramBotToken | Should -Be '123456789:AAbbCCddEeffGGhhIIjj'
        $r.TelegramChatId | Should -Be '-1001234567890'
        $r.TestAlert | Should -Be $true
        $r.NotifyOnStart | Should -Be $true
        $r.Force | Should -Be $true
        $r.ForceEveryNRuns | Should -Be 48
    }

    It 'handles whitespace in url' {
        $r = Get-MonitorArgs -Arguments @('--webhook-url', 'https://hooks.example.com/path with spaces')
        $r.WebhookUrl | Should -Be 'https://hooks.example.com/path with spaces'
    }
}
