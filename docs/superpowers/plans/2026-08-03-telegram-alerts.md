# Telegram Alerts Implementation Plan (Issue #80)

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add Telegram as a third independent monitoring-alert channel (bot token + chat ID via the Bot API), fully parallel to the existing Slack/Teams webhook support.

**Architecture:** Two config values (`TelegramBotToken`, `TelegramChatId`) flow through the existing `LocalConfigStore.Config` / DPAPI `.eaxwiki` / CLI-arg / env-var / SchedulerUI / interactive-wizard surfaces, mirroring the Teams webhook exactly. `Send-Alert` in `monitor-export-and-serve.ps1` and the standalone `send-alert.ps1` each gain a third independent dispatch block that POSTs `{chat_id, text, parse_mode}` to `https://api.telegram.org/bot{TOKEN}/sendMessage`, with a one-shot plain-text fallback on HTTP 400. A new standalone `Send-TelegramMessage` function in the monitor script holds the dispatch so Pester can test it even when the script's top-level body exits early.

**Tech Stack:** PowerShell 5.1+/7 (monitor/scheduler scripts), C# (.NET 10), System.Text.Json + DPAPI (`LocalConfigStore`), Pester 5, xUnit/Moq.

## Global Constraints

- Full parity with Slack/Teams — Telegram is a third independent channel; configure any/all/none; one channel failing never blocks the others.
- Exactly one channel: a single bot token + chat ID (mirrors single-webhook-per-channel).
- Message style: Unicode emoji per `$Kind` + `*bold*` labels via `parse_mode: "Markdown"`. No colors/cards.
- Config resolution order per value: CLI arg → env var → encrypted `.eaxwiki`. Env vars: `EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN`, `EAXWIKI_ALERT_TELEGRAM_CHAT_ID`.
- Token is a secret: never baked into a scheduled-task action argument (`register-scheduled-task.ps1` header-only change, no new CLI).
- Negative group chat IDs (e.g. `-1001234567890`) must be sent as JSON *strings*, not PowerShell numbers.
- Markdown-fallback retry must be exactly one retry (no loop): on HTTP 400, retry once with `parse_mode` omitted.
- Telegram emoji map (exact): Start 🔄, Finish 🟢, Failure/ServeFailure/LlmFailure/ApiFailure 🔴, Recovery/ServeRecovery/LlmRecovery/ApiRecovery 🟢, Test 🔵, DailyDigest 📊, UserStop ✋.
- Message text format (exact): `{emoji} *EAxWiki [{Kind}]* - {instanceLabel}` then a newline then `$Message`.
- Out of scope: multiple chat destinations, inline keyboards, message editing/dedup, anything beyond `sendMessage`.
- Monitor script top-level body must not be restructured; do not fix the pre-existing "dot-source runs body" test fragility.
- **Encoding (critical):** any edited PowerShell file containing literal Unicode emoji must be saved as **UTF-8 with BOM** (PS 5.1 reads no-BOM files as ANSI and will mojibake the emoji). Currently only `scripts/monitor-export-and-serve.ps1` is BOM. `scripts/send-alert.ps1`, `tests/scripts/monitor-export-and-serve.Tests.ps1`, and the new `tests/scripts/send-alert.Tests.ps1` must be re-saved as UTF-8 with BOM as part of Tasks 3/4. Verify after editing with the check in Task 3 Step 4.

---

### Task 1: LocalConfigStore fields + round-trip tests

**Files:**
- Modify: `src/EAxWiki.Core/Configuration/LocalConfigStore.cs:31` (insert after `TeamsWebhookUrl`)
- Test: `src/EAxWiki.Tests/LocalConfigStoreTests.cs:21-42` and `:44-55`

**Interfaces:**
- Produces: `LocalConfigStore.Config.TelegramBotToken` (`string?`), `LocalConfigStore.Config.TelegramChatId` (`string?`). Used by Tasks 2, 5, 6, 7.

- [ ] **Step 1: Write the failing test**

Edit `SaveAndLoad_RoundTrip_PreservesAllFields` (`src/EAxWiki.Tests/LocalConfigStoreTests.cs:22-42`) to set and assert the two new fields:

```csharp
    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesAllFields()
    {
        var config = new LocalConfigStore.Config
        {
            RepoPath = @"C:\Models\repo.qea",
            WebhookUrl = "https://hooks.slack.com/ABC",
            TeamsWebhookUrl = "https://outlook.office.com/DEF",
            TelegramBotToken = "123456789:AAbbCCddEeffGGhhIIjj",
            TelegramChatId = "-1001234567890",
            WikiPort = 8000,
            ApiPort = 8001
        };
        var path = Path.Combine(_dir, ".eaxwiki");
        LocalConfigStore.Save(path, config);

        var loaded = LocalConfigStore.Load(path, out var wasLegacy);
        Assert.False(wasLegacy);
        Assert.Equal(config.RepoPath, loaded.RepoPath);
        Assert.Equal(config.WebhookUrl, loaded.WebhookUrl);
        Assert.Equal(config.TeamsWebhookUrl, loaded.TeamsWebhookUrl);
        Assert.Equal(config.TelegramBotToken, loaded.TelegramBotToken);
        Assert.Equal(config.TelegramChatId, loaded.TelegramChatId);
        Assert.Equal(config.WikiPort, loaded.WikiPort);
        Assert.Equal(config.ApiPort, loaded.ApiPort);
    }
```

Also add null-default assertions to `SaveAndLoad_MinimalConfig_RoundTrips` (after line 54):

```csharp
        Assert.Null(loaded.TelegramBotToken);
        Assert.Null(loaded.TelegramChatId);
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~LocalConfigStoreTests"`
Expected: FAIL — `Config` has no `TelegramBotToken`/`TelegramChatId` members (compile error).

- [ ] **Step 3: Write minimal implementation**

In `src/EAxWiki.Core/Configuration/LocalConfigStore.cs`, after line 31 (`public string? TeamsWebhookUrl { get; set; }`), add:

```csharp
        public string? TelegramBotToken { get; set; }
        public string? TelegramChatId { get; set; }
```

No change to `Save`/`Load` — System.Text.Json round-trips new nullable properties automatically, and the existing DPAPI path is untouched.

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~LocalConfigStoreTests"`
Expected: PASS.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Core/Configuration/LocalConfigStore.cs src/EAxWiki.Tests/LocalConfigStoreTests.cs
git commit -m "feat(config): add telegram bot token + chat id config fields (issue #80)"
```

---

### Task 2: Monitor CLI args — `Get-MonitorArgs`

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1:69-70` (defaults), `:87-88` (parse cases), `:106-107` (return object)
- Test: `tests/scripts/monitor-export-and-serve.Tests.ps1`

**Interfaces:**
- Consumes: Task 1's `Config.TelegramBotToken`/`TelegramChatId` (not directly — `.eaxwiki` JSON keys must be `telegramBotToken`/`telegramChatId`, which System.Text.Json emits and PowerShell reads case-insensitively).
- Produces: `Get-MonitorArgs` returns `TelegramBotToken` and `TelegramChatId` (both `$null` by default); CLI flags `--telegram-bot-token`/`-TelegramBotToken` and `--telegram-chat-id`/`-TelegramChatId`. Consumed by Task 3's resolution blocks.

- [ ] **Step 1: Write the failing test**

In `tests/scripts/monitor-export-and-serve.Tests.ps1`:

Extend `'returns defaults with no arguments'` (after line 15):

```powershell
        $r.TelegramBotToken | Should -Be $null
        $r.TelegramChatId | Should -Be $null
```

Add after line 61 (the `-TeamsWebhookUrl` parse test):

```powershell
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
```

Extend `'all flags combined'` (args array, after line 93; assertions, after line 105):

```powershell
            '--telegram-bot-token', '123456789:AAbbCCddEeffGGhhIIjj',
            '--telegram-chat-id', '-1001234567890',
```
```powershell
        $r.TelegramBotToken | Should -Be '123456789:AAbbCCddEeffGGhhIIjj'
        $r.TelegramChatId | Should -Be '-1001234567890'
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: FAIL — `Get-MonitorArgs` return object has no `TelegramBotToken`/`TelegramChatId` members (`Should -Be $null` fails with property-not-found, new parse tests return `$null`).

- [ ] **Step 3: Write minimal implementation**

In `scripts/monitor-export-and-serve.ps1`:

After line 70 (`$TeamsWebhookUrl = $null`), add defaults:

```powershell
    $TelegramBotToken     = $null
    $TelegramChatId       = $null
```

After line 88 (the Teams parse case), add:

```powershell
            '^(--telegram-bot-token|-TelegramBotToken)$'  { $i++; if ($i -lt $Arguments.Count) { $TelegramBotToken = $Arguments[$i] } }
            '^(--telegram-chat-id|-TelegramChatId)$'      { $i++; if ($i -lt $Arguments.Count) { $TelegramChatId   = $Arguments[$i] } }
```

After line 107 (in the return object), add:

```powershell
        TelegramBotToken    = $TelegramBotToken
        TelegramChatId      = $TelegramChatId
```

- [ ] **Step 4: Run test to verify it passes**

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: PASS (33 tests).

- [ ] **Step 5: Commit**

```bash
git add scripts/monitor-export-and-serve.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1
git commit -m "feat(monitor): parse telegram bot token + chat id CLI args (issue #80)"
```

---

### Task 3: Monitor dispatch — resolution, `Send-TelegramMessage`, `Send-Alert` block

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1` — parsed-var bindings after `:132`; resolution blocks after `:179`; new `Send-TelegramMessage` function after `:115` (must be defined before the duplicate-monitor early exit at `:258` so Pester can reach it); `Send-Alert` Telegram block after `:432`; no-webhook guard at `:348-351`; `--test-alert` message at `:436`.
- Test: `tests/scripts/monitor-export-and-serve.Tests.ps1` (new `Describe 'Send-TelegramMessage'`)

**Interfaces:**
- Consumes: Task 2's `TelegramBotToken`/`TelegramChatId` script vars; env vars `EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN`/`EAXWIKI_ALERT_TELEGRAM_CHAT_ID`; `.eaxwiki` keys `telegramBotToken`/`telegramChatId`; Task 1's Config fields.
- Produces: `Send-TelegramMessage` — `param([string]$BotToken, [string]$ChatId, [string]$Message, [string]$Kind, [string]$InstanceLabel)`, returns `$true` on success, `$null` when no token, `$false` on failure after one 400 fallback. Calls `Write-MonitorLog` on dispatch success/failure. Consumed by Task 4 (pattern reference only) and `Send-Alert`.

- [ ] **Step 1: Write the failing test**

Append a new `Describe` block to `tests/scripts/monitor-export-and-serve.Tests.ps1` (after line 116):

```powershell
Describe 'Send-TelegramMessage' {
    BeforeEach {
        $script:tgCalls = 0
        $global:tgUri = $null
        $global:tgBody = $null
    }

    It 'is a no-op when no bot token is provided' {
        Mock Invoke-RestMethod { $script:tgCalls++ }
        $result = Send-TelegramMessage -BotToken '' -ChatId '1' -Message 'x' -Kind Test -InstanceLabel 'l'
        $result | Should -BeNullOrEmpty
        $script:tgCalls | Should -Be 0
    }

    It 'posts chat_id, text, parse_mode to bot{token}/sendMessage' {
        Mock Invoke-RestMethod { param($Uri, $Method, $Body, $ContentType) $global:tgUri = $Uri; $global:tgBody = $Body }
        Send-TelegramMessage -BotToken '123456:ABC' -ChatId '-1001234567890' -Message 'export failed' -Kind Failure -InstanceLabel 'PC1 - wiki'
        $global:tgUri | Should -Be 'https://api.telegram.org/bot123456:ABC/sendMessage'
        $json = $global:tgBody | ConvertFrom-Json
        $json.chat_id | Should -Be '-1001234567890'
        $json.text | Should -Match '🔴 \*EAxWiki \[Failure\]\* - PC1 - wiki'
        $json.text | Should -Match 'export failed'
        $json.parse_mode | Should -Be 'Markdown'
    }

    It 'retries once without parse_mode when Telegram rejects Markdown (400)' {
        Mock Invoke-RestMethod {
            param($Uri, $Method, $Body, $ContentType)
            $script:tgCalls++
            $script:tgFallbackBody = $Body
            if ($script:tgCalls -eq 1) {
                # Throw an object that looks like $_.Exception.Response.StatusCode = 400.
                # PS 5.1 (the Pester host here) can't construct System.Net.Http.HttpRequestException
                # with a status code, and WebException.Response is read-only, so a pscustomobject
                # is the version-safe way to exercise the one-shot Markdown-fallback branch.
                throw [pscustomobject]@{ Response = [pscustomobject]@{ StatusCode = [System.Net.HttpStatusCode]::BadRequest } }
            }
        }
        $ok = Send-TelegramMessage -BotToken '123456:ABC' -ChatId '42' -Message 'hello *world*' -Kind Test -InstanceLabel 'l'
        $script:tgCalls | Should -Be 2
        $script:tgFallbackBody | Should -Not -Match 'parse_mode'
        $ok | Should -BeTrue
    }
}
```

Note: this file's `BeforeAll` dot-sources the monitor script. The current environment exits at the duplicate-monitor check (`:258`), so `Send-TelegramMessage` MUST be defined before that line or these tests fail with "command not found". Task 3 Step 3 places it accordingly.

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: FAIL — `Send-TelegramMessage` is not recognized (command not found).

- [ ] **Step 3: Write minimal implementation**

**(a)** Add the standalone function in `scripts/monitor-export-and-serve.ps1` right after `Get-MonitorArgs` ends (after line 115, before `ConvertTo-RedactedConnectionString` at line 117):

```powershell
function Send-TelegramMessage {
    # Issue #80: Telegram Bot API dispatch. Standalone (no dependency on the script's top-level
    # variables) so Pester can exercise it even when the monitor body exits early on a duplicate
    # monitor. Token goes in the URL (standard Telegram pattern); chat_id is a *string* because
    # group/supergroup IDs are negative numbers (-100...) and must survive JSON round-tripping.
    param(
        [string]$BotToken,
        [string]$ChatId,
        [string]$Message,
        [string]$Kind,
        [string]$InstanceLabel
    )
    if (-not $BotToken -or -not $ChatId) { return $null }

    $tgEmoji = switch ($Kind) {
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
    $text = "{0} *EAxWiki [{1}]* - {2}`n{3}" -f $tgEmoji, $Kind, $InstanceLabel, $Message
    $uri = "https://api.telegram.org/bot{0}/sendMessage" -f $BotToken
    $body = @{
        chat_id    = [string]$ChatId
        text       = $text
        parse_mode = 'Markdown'
    }

    $attempts = 0
    while ($true) {
        $attempts++
        try {
            Invoke-RestMethod -Uri $uri -Method Post -Body ($body | ConvertTo-Json) -ContentType 'application/json; charset=utf-8' | Out-Null
            Write-MonitorLog -Phase "alert" -Message "Telegram dispatched."
            return $true
        } catch {
            # HTTP 400 usually means Telegram rejected our Markdown (e.g. an unmatched '*' in the
            # message body). Retry exactly once with parse_mode omitted; any other status just logs.
            $status = $null
            if ($_.Exception.Response) { $status = $_.Exception.Response.StatusCode }
            elseif ($_.Exception.StatusCode) { $status = $_.Exception.StatusCode }
            if ($null -ne $status -and [int]$status -eq 400 -and $attempts -eq 1 -and $body.ContainsKey('parse_mode')) {
                $body.Remove('parse_mode')
                continue
            }
            Write-MonitorLog -Phase "alert" -Message "Telegram dispatch failed: $($_.Exception.Message)"
            return $false
        }
    }
}
```

**(b)** Bind the parsed args to script variables. After line 132 (`$TeamsWebhookUrl = $parsed.TeamsWebhookUrl`), add:

```powershell
$TelegramBotToken    = $parsed.TelegramBotToken
$TelegramChatId      = $parsed.TelegramChatId
```

**(c)** Add resolution blocks after the Teams resolution (after line 179):

```powershell
if ($null -eq $TelegramBotToken -or "" -eq $TelegramBotToken) {
    if ($env:EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN) {
        $TelegramBotToken = $env:EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN
    } elseif ($eaxwikiConfig -and $eaxwikiConfig.telegramBotToken) {
        $TelegramBotToken = $eaxwikiConfig.telegramBotToken
    }
}

if ($null -eq $TelegramChatId -or "" -eq $TelegramChatId) {
    if ($env:EAXWIKI_ALERT_TELEGRAM_CHAT_ID) {
        $TelegramChatId = $env:EAXWIKI_ALERT_TELEGRAM_CHAT_ID
    } elseif ($eaxwikiConfig -and $eaxwikiConfig.telegramChatId) {
        $TelegramChatId = $eaxwikiConfig.telegramChatId
    }
}
```

**(d)** Update the no-webhook guard in `Send-Alert` (line 348-351) so Telegram alone is enough to dispatch:

```powershell
    if (-not $WebhookUrl -and -not $TeamsWebhookUrl -and -not $TelegramBotToken -and -not $TelegramChatId) {
        Write-MonitorLog -Phase "alert" -Message "No alert channel configured (Slack: --webhook-url/EAXWIKI_ALERT_WEBHOOK; Teams: --teams-webhook-url/EAXWIKI_ALERT_TEAMS_WEBHOOK; Telegram: --telegram-bot-token/--telegram-chat-id or EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN/EAXWIKI_ALERT_TELEGRAM_CHAT_ID); alert logged only."
        return
    }
```

**(e)** Add the Telegram block at the end of `Send-Alert`, right after the Teams block closes (after line 432, before the function's closing `}` at line 433):

```powershell
    if ($TelegramBotToken -and $TelegramChatId) {
        Send-TelegramMessage -BotToken $TelegramBotToken -ChatId $TelegramChatId -Message $Message -Kind $Kind -InstanceLabel $instanceLabel
    }
```

**(f)** Update the `--test-alert` copy (line 436):

```powershell
    Send-Alert -Kind Test -Message "Test alert from monitor-export-and-serve.ps1 - if you can see this in Slack/Teams/Telegram, alerting is wired correctly."
```

- [ ] **Step 4: Verify encoding + run test**

First verify the emoji-bearing files are UTF-8 with BOM (PS 5.1 mojibakes emoji in no-BOM files — see Global Constraints):

```powershell
Get-ChildItem scripts\monitor-export-and-serve.ps1, tests\scripts\monitor-export-and-serve.Tests.ps1 | ForEach-Object { $b = [System.IO.File]::ReadAllBytes($_.FullName); "{0}: {1}" -f $_.Name, $(if ($b.Length -ge 3 -and $b[0] -eq 0xEF -and $b[1] -eq 0xBB -and $b[2] -eq 0xBF) { 'UTF8-BOM' } else { 'no-BOM' }) }
```

If either shows `no-BOM`, re-save it with BOM (e.g. `$c = Get-Content $f -Raw -Encoding UTF8; [System.IO.File]::WriteAllText($f, $c, (New-Object System.Text.UTF8Encoding $true))`).

Then run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: PASS (36 tests — 33 from Task 2 plus 3 new `Send-TelegramMessage` tests).

- [ ] **Step 5: Sanity-run the script's parser**

Run: `& 'E:\Users\Han\Repos\EAxWiki\scripts\monitor-export-and-serve.ps1' --telegram-bot-token x --telegram-chat-id 42 --test-alert 2>&1 | Select-Object -Last 5`
Expected: no parse error (no "unrecognized option"). In this environment the script exits at the duplicate-monitor check (`Duplicate monitor detected (PID ... already running). Exiting.`) before reaching the `--test-alert` block — that early exit is expected and proves the new flags parsed cleanly through `Get-MonitorArgs`. Do not expect a real Telegram message — no live token here.

- [ ] **Step 6: Commit**

```bash
git add scripts/monitor-export-and-serve.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1
git commit -m "feat(monitor): dispatch alerts to telegram via bot api (issue #80)"
```

---

### Task 4: `send-alert.ps1` parity (SchedulerUI user-stop path)

**Files:**
- Modify: `scripts/send-alert.ps1` — params (lines 1-6), Telegram block after line 75
- Test: `tests/scripts/send-alert.Tests.ps1` (new file)

**Interfaces:**
- Consumes: same Telegram semantics as Task 3's `Send-TelegramMessage`, but inlined (this standalone script has no function seam and uses `Write-Host` for logging, matching its Slack/Teams blocks).
- Produces: new script params `-TelegramBotToken`, `-TelegramChatId` (both default `""`). Consumed by Task 6 (SchedulerForm invokes `send-alert.ps1` with these args).

- [ ] **Step 1: Write the failing test**

Create `tests/scripts/send-alert.Tests.ps1`:

```powershell
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
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/send-alert.Tests.ps1 -Output Detailed`
Expected: FAIL — `send-alert.ps1` doesn't accept `-TelegramBotToken`/`-TelegramChatId` (parameter binding error).

- [ ] **Step 3: Write minimal implementation**

In `scripts/send-alert.ps1`, extend the `param()` block (after line 2):

```powershell
    [string]$TelegramBotToken = "",
    [string]$TelegramChatId = "",
```

After the Teams block (after line 75), add the Telegram block (mirrors `Send-TelegramMessage`; uses `Write-Host` to match this script's existing logging):

```powershell
if ($TelegramBotToken -and $TelegramChatId) {
    $tgEmoji = switch ($Kind) {
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
    $tgText = "{0} *EAxWiki [{1}]* - {2}`n{3}" -f $tgEmoji, $Kind, $instanceLabel, $Message
    $tgUri = "https://api.telegram.org/bot{0}/sendMessage" -f $TelegramBotToken
    $tgBody = @{
        chat_id    = [string]$TelegramChatId
        text       = $tgText
        parse_mode = 'Markdown'
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
```

- [ ] **Step 4: Verify encoding + run test**

The emoji in this task's code blocks require UTF-8 with BOM in `scripts/send-alert.ps1` and `tests/scripts/send-alert.Tests.ps1` (both currently no-BOM). Verify/re-save with the same commands as Task 3 Step 4, then:

Run: `Invoke-Pester tests/scripts/send-alert.Tests.ps1 -Output Detailed`
Expected: PASS (2 tests).

- [ ] **Step 5: Commit**

```bash
git add scripts/send-alert.ps1 tests/scripts/send-alert.Tests.ps1
git commit -m "feat(send-alert): support telegram bot dispatch for user-stop alerts (issue #80)"
```

---

### Task 5: Interactive wizard — `Program.cs`

**Files:**
- Modify: `src/EAxWiki/Program.cs:111-118` (after the Teams prompt), `:120-127` (`newConfig`)

**Interfaces:**
- Consumes: Task 1's `Config.TelegramBotToken`/`TelegramChatId`.
- Produces: `.eaxwiki` gains `telegramBotToken`/`telegramChatId` when the user opts in. Consumed by Task 3's resolution (already reads those keys) and Task 7's docs.

- [ ] **Step 1: Write the implementation**

In `src/EAxWiki/Program.cs`, after the Teams prompt block (after line 118), add:

```csharp
            Console.Write("Configure Telegram monitoring alerts? [y/N]: ");
            var wantTelegram = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            var telegramBotToken = "";
            var telegramChatId = "";
            if (wantTelegram == "y" || wantTelegram == "yes")
            {
                Console.Write("Telegram bot token (from @BotFather): ");
                telegramBotToken = (Console.ReadLine() ?? "").Trim();
                Console.Write("Telegram chat ID (numeric, the destination chat): ");
                telegramChatId = (Console.ReadLine() ?? "").Trim();
            }
```

In the `newConfig` initializer (after line 124), add:

```csharp
                TelegramBotToken = telegramBotToken,
                TelegramChatId = telegramChatId,
```

- [ ] **Step 2: Verify build + full .NET test suite**

Run: `dotnet build src/EAxWiki` then `dotnet test src/EAxWiki.Tests`
Expected: build succeeds, all tests pass.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki/Program.cs
git commit -m "feat(wizard): prompt for telegram monitoring alerts (issue #80)"
```

---

### Task 6: SchedulerUI fields + validation + wiring

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — fields after `:45`; `BuildConfigTab` rows after `:226`; `LoadEaxwikiConfig` reset after `:673` and load after `:689`; `SaveEaxwikiConfig` validation after `:729` and config after `:737`; `StartMonitorAsync` args after `:1166`; `StopExportAsync`/`StopServeAsync`/`StopLlmAsync`/`StopAllAsync` (lines `:1197`, `:1218`, `:1243`, `:1264`) send-alert invocations.

**Interfaces:**
- Consumes: Task 1's Config fields; Task 4's `send-alert.ps1` params.
- Produces: `--telegram-bot-token`/`--telegram-chat-id` in the monitor launch args; `-TelegramBotToken`/`-TelegramChatId` in the stop-alert commands. Consumed by Tasks 3/4 (already implemented) and Task 7 (docs).

- [ ] **Step 1: Write the implementation**

**(a)** Field declarations, after line 45 (`_teamsWebhookBox`):

```csharp
    private readonly TextBox _telegramBotTokenBox = new() { Width = 400, UseSystemPasswordChar = true };
    private readonly TextBox _telegramChatIdBox = new() { Width = 400 };
```

**(b)** `BuildConfigTab`, after line 226:

```csharp
        AddRow(table, "Telegram Bot Token:", _telegramBotTokenBox);
        AddRow(table, "Telegram Chat ID:", _telegramChatIdBox);
```

**(c)** `LoadEaxwikiConfig` reset branch, after line 673:

```csharp
            _telegramBotTokenBox.Text = "";
            _telegramChatIdBox.Text = "";
```

**(d)** `LoadEaxwikiConfig` populated branch, after line 689:

```csharp
            _telegramBotTokenBox.Text = config.TelegramBotToken ?? "";
            _telegramChatIdBox.Text = config.TelegramChatId ?? "";
```

**(e)** `SaveEaxwikiConfig` validation, after the Teams check (after line 729):

```csharp
        if (_telegramChatIdBox.Text.Trim() is { Length: > 0 } chatId &&
            !long.TryParse(chatId, out _))
        {
            AppendOutput($"Invalid Telegram chat ID (must be numeric, e.g. 123456789 or -1001234567890): {chatId}");
            return;
        }
```

**(f)** `SaveEaxwikiConfig` config object, after line 737:

```csharp
            TelegramBotToken = _telegramBotTokenBox.Text.Trim() is { Length: > 0 } token ? token : null,
            TelegramChatId = _telegramChatIdBox.Text.Trim() is { Length: > 0 } id ? id : null,
```

**(g)** `StartMonitorAsync` (line ~1163-1166), after the Teams arg:

```csharp
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        if (tgBotToken.Length > 0) { args.Add("--telegram-bot-token"); args.Add(tgBotToken); }
        var tgChatId = _telegramChatIdBox.Text.Trim();
        if (tgChatId.Length > 0) { args.Add("--telegram-chat-id"); args.Add(tgChatId); }
```

**(h)** In each of the four stop methods (`StopExportAsync` line ~1201, `StopServeAsync` line ~1222, `StopLlmAsync` line ~1247, `StopAllAsync` line ~1268), after the `teamsUrl` declaration, add:

```csharp
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        var tgChatId = _telegramChatIdBox.Text.Trim();
```

and extend each `send-alert.ps1` command string (four occurrences) with:

```powershell
 -TelegramBotToken '{tgBotToken.Replace("'", "''")}' -TelegramChatId '{tgChatId.Replace("'", "''")}'
```

- [ ] **Step 2: Verify build**

Run: `dotnet build src/EAxWiki.SchedulerUI`
Expected: build succeeds. (No automated tests exist for the WinForms UI; verification is build + the Task 9 manual E2E.)

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(scheduler-ui): telegram alert fields + wiring (issue #80)"
```

---

### Task 7: Docs — `register-scheduled-task.ps1` header, README, `TELEGRAM_SETUP.md`

**Files:**
- Modify: `scripts/register-scheduled-task.ps1:11-21` (header comment only)
- Modify: `README.md` — Monitoring & Alerting (`:37`), scripts table row, scheduling section (`:422`), Scheduler GUI (`:487`)
- Create: `docs/TELEGRAM_SETUP.md`

**Interfaces:**
- Consumes: all finished behavior from Tasks 1-6.
- Produces: user-facing setup guide + accurate README documentation.

- [ ] **Step 1: Update `register-scheduled-task.ps1` header comment**

Replace lines 11-21 with:

```powershell
# Slack, Teams and/or Telegram alert destinations (issue #39 / #80 — all are independent, not
# exclusive; configure any subset) can be configured in one of three ways each (checked in this order):
#   1. Stored in .eaxwiki as encrypted JSON (recommended for per-instance setup)
#   2. Set as EAXWIKI_ALERT_WEBHOOK / EAXWIKI_ALERT_TEAMS_WEBHOOK (Slack/Teams) or
#      EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN / EAXWIKI_ALERT_TELEGRAM_CHAT_ID (Telegram) environment
#      variables (use when .eaxwiki is shared/unencrypted)
#   3. Not configured (alerting is disabled for that channel; still logs to wiki/status/health.md)
#
# This registration script does NOT bake --webhook-url/--teams-webhook-url/--telegram-bot-token/
# --telegram-chat-id into the scheduled task's command line, even though monitor-export-and-serve.ps1
# itself accepts them for manual/direct invocation — Task Scheduler stores action arguments in a
# readable way (any admin can read them back via Get-ScheduledTask), so scheduled runs always resolve
# via env var or .eaxwiki.
```

- [ ] **Step 2: Update README**

**(a)** Monitoring & Alerting paragraph (line 37) — replace with:

```markdown
EAxWiki can send monitoring alerts to **Slack, Microsoft Teams, and/or Telegram** when background export/serve operations start, encounter issues, or recover — see [Scheduling exports](#scheduling-exports) for the unattended monitor wrapper that sends these. The channels are independent, not exclusive: configure any subset, and every alert goes to whichever channel(s) are set up. See [**Slack Webhook Setup**](docs/SLACK_WEBHOOK_SETUP.md), [**Teams Webhook Setup**](docs/TEAMS_WEBHOOK_SETUP.md), or [**Telegram Setup**](docs/TELEGRAM_SETUP.md) for detailed instructions.
```

**(b)** Scripts table row for `monitor-export-and-serve.ps1` — change `Slack/Teams alerting` to `Slack/Teams/Telegram alerting`.

**(c)** Scheduling exports paragraph (line 422) — change `(if a Slack and/or Teams webhook is configured` to `(if a Slack, Teams, and/or Telegram alert destination is configured`.

**(d)** Scheduler GUI Configuration tab text (line 487) — change `repo path/ports/Slack+Teams webhooks` to `repo path/ports/Slack/Teams/Telegram alert settings`.

- [ ] **Step 3: Create `docs/TELEGRAM_SETUP.md`**

```markdown
# Telegram Setup for Monitoring Alerts

EAxWiki can send monitoring and alerting notifications to Telegram when background export/serve operations start, encounter issues, or recover.

## Supported Type

EAxWiki sends messages through the Telegram **Bot API** (`sendMessage`) using a **bot token** (from @BotFather) and a **chat ID** (the destination chat). Slack and Teams are also supported — see [**Slack Webhook Setup**](SLACK_WEBHOOK_SETUP.md) and [**Teams Webhook Setup**](TEAMS_WEBHOOK_SETUP.md). All channels are independent, not exclusive: configure any subset, and every alert goes to whichever channel(s) are set up.

## Step 1 — Create the bot

1. Open Telegram and message **@BotFather**.
2. Send `/newbot`, choose a display name and a username (must end in `bot`).
3. BotFather replies with a **token** that looks like `123456789:AA...` — copy it. This is the bot's secret API key.

## Step 2 — Add the bot to a destination

- **Private chat:** message the bot once (any text) so it has a chat with you.
- **Group:** add the bot as a member, then post a test message in the group.
- **Channel:** add the bot as an administrator.

## Step 3 — Resolve the numeric chat ID

Message the bot (or post in the group/channel) again, then open in a browser:

```
https://api.telegram.org/bot<TOKEN>/getUpdates
```

Find the `chat` object in the JSON. Its `id` is the numeric chat ID:

- Private chat: the user's numeric ID (positive, e.g. `123456789`).
- Group/channel: a negative number (e.g. `-1001234567890`).

The numeric ID is required — a `@username` cannot be used as `chat_id`.

## Step 4 — Configure EAxWiki

The monitor asks interactively during first-time setup:

```
Configure Telegram monitoring alerts? [y/N]: y
Telegram bot token (from @BotFather): <paste your token>
Telegram chat ID (numeric, the destination chat): <paste your chat id>
```

To change it later, delete the `.eaxwiki` file in the repo root and run EAxWiki again — it will re-prompt for all alert destinations.

Or set the environment variables (checked after CLI args, before `.eaxwiki`):

```powershell
$env:EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN = '<token>'
$env:EAXWIKI_ALERT_TELEGRAM_CHAT_ID = '<chat id>'
```

## Testing Your Alert

```powershell
.\scripts\monitor-export-and-serve.ps1 --test-alert
```

This resolves each channel the same way a real scheduled run does and posts a blue "Test" message to every configured channel. Check your Telegram chat to confirm it arrived.

## When Alerts Are Sent

| Kind | Emoji | When |
|------|-------|------|
| Start | 🔄 | Every run start |
| Finish | 🟢 | Successful run (page/diagram counts) |
| Failure / ServeFailure / LlmFailure / ApiFailure | 🔴 | A pass finally gave up |
| Recovery / ServeRecovery / LlmRecovery / ApiRecovery | 🟢 | A previously failing component recovered |
| DailyDigest | 📊 | Once per calendar day |
| UserStop | ✋ | Scheduler GUI "stop" buttons |
| Test | 🔵 | `--test-alert` |

A transient failure that succeeds on retry within the same pass does **not** alert — only the final outcome of a pass does. If multiple channels are configured, every alert goes to all of them; one channel failing doesn't block the others.

## Security

- **Keep your bot token secret** — do not commit `.eaxwiki` to git (it's gitignored).
- The token and chat ID are encrypted at rest in `.eaxwiki` using Windows DPAPI (your user account only).
- Telegram markdown in the message body is stripped on a one-shot plain-text fallback if the Bot API rejects it (HTTP 400).
- If you suspect the token was exposed, message **@BotFather** and use `/revoke` to invalidate it, then repeat Steps 1-4.

## Troubleshooting

**"bot can't be found"?** The token is wrong or was revoked — repeat Step 1 and copy the fresh token.

**"chat not found"?** The bot was never added to the destination chat, or the chat ID is stale (IDs change when a private chat's bot history is cleared). Repeat Steps 2-3.

**Message too long?** Telegram limits text to 4096 characters. EAxWiki alert messages are normally far shorter.

**Markdown fallback triggered?** A literal `*`, `_`, or `` ` `` in the alert text can break Telegram Markdown. EAxWiki retries the message once as plain text, so you should still receive it.

## Also Sending to Slack/Teams

Slack, Teams, and Telegram are independent — configuring one doesn't affect the others, and any combination can be active at once. See [**Slack Webhook Setup**](SLACK_WEBHOOK_SETUP.md) and [**Teams Webhook Setup**](TEAMS_WEBHOOK_SETUP.md).
```

- [ ] **Step 4: Verify docs are consistent**

Run: `git diff --stat` — confirm only the intended files changed (plus any monitor re-export noise; do not stage `wiki/`, `.eaxwiki-monitor/`, `model/`, `.eaxwiki`).
Run: `Invoke-Pester tests/scripts/ -Output Detailed` — full script suite green.

- [ ] **Step 5: Commit**

```bash
git add scripts/register-scheduled-task.ps1 README.md docs/TELEGRAM_SETUP.md
git commit -m "docs: telegram alert setup guide + register/README updates (issue #80)"
```

---

### Task 8: Full verification + README test counts

**Files:**
- Modify: `README.md` — Tests table (`:593-611`)

**Interfaces:**
- Consumes: all tasks.
- Produces: accurate, verified documentation of test totals.

- [ ] **Step 1: Run the full .NET suite**

Run: `dotnet test src/EAxWiki.Tests`
Expected: all pass (244 tests).

- [ ] **Step 2: Run the full Pester suite**

Run: `Invoke-Pester tests/scripts/ -Output Detailed`
Expected: all pass. Note: this environment exits the monitor dot-source early (duplicate monitor running) — that's the pre-existing, expected behavior; the count will have grown by the new tests.

- [ ] **Step 3: Update README Tests table**

In `README.md`, update the `MonitorExportAndServe` Pester row's test count and the **Pester subtotal** / **366 tests total** line to the actual numbers reported by the two runs above.

- [ ] **Step 4: Commit**

```bash
git add README.md
git commit -m "docs: update test counts for telegram alerting (issue #80)"
```

---

### Task 9: Manual E2E + issue close

**Files:** none (manual verification only)

**Interfaces:**
- Consumes: everything.

- [ ] **Step 1: Configure a real bot**

Follow `docs/TELEGRAM_SETUP.md`: create a bot via @BotFather, add it to a private chat or group, resolve the numeric chat ID via `getUpdates`, and configure via the interactive wizard (delete `.eaxwiki` first to re-prompt) or env vars.

- [ ] **Step 2: Send a test alert**

Run: `.\scripts\monitor-export-and-serve.ps1 --test-alert`
Expected: the blue 🔵 Test message appears in the configured chat.

- [ ] **Step 3: Verify SchedulerUI**

Open SchedulerUI, confirm the Telegram fields load/save in the Configuration tab, that an invalid chat ID is rejected on Save, and that Stop buttons pass the token/chat ID through to `send-alert.ps1`.

- [ ] **Step 4: Close the issue**

First capture the commit range: `git log --oneline -9 | Select-String -Pattern 'issue #80'` and note the earliest and latest `issue #80` commit hashes. Then write the comment to a temp file and post:

```powershell
Set-Content -Path "C:\Users\hanva\AppData\Local\Temp\opencode\issue80-close.md" -Value @"
Implemented: Telegram as a third independent alert channel (bot token + chat ID via Bot API), full config-surface parity with Slack/Teams (CLI args, env vars, .eaxwiki/DPAPI, SchedulerUI fields, interactive wizard), Markdown-fallback retry on HTTP 400, TELEGRAM_SETUP.md guide. Commits: <FIRST..LAST hashes from the command above>.
"@
gh issue close 80 --body-file "C:\Users\hanva\AppData\Local\Temp\opencode\issue80-close.md"
```

(Use `--body-file` with a temp file under `C:\Users\hanva\AppData\Local\Temp\opencode\` — inline multiline PowerShell strings fail on `gh`.)
