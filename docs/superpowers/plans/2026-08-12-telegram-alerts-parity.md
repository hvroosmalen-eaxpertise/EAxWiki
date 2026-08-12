# Telegram Alerts — Parity With Slack (Issue #80 Follow-up)

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [x]`) syntax for tracking.

**Goal:** Bring Telegram alerts to visual parity with Slack. Today Telegram messages render sparser than Slack because Slack automatically renders `footer` + `ts` (local time) below `pretext` + `text`, plus a colored side-bar; Telegram gets only `"{emoji} *EAxWiki [{Kind}]* - {instanceLabel}\n{Message}"` with no timestamp, no footer, no color signaling, and a fragile Markdown v1 dialect that silently drops content when the `$Message` body contains stray `_`, `*`, `[`, or backticks.

**User trigger:** Comment on issue #80 (2026-08-12): *"The messages sent to Telegram are not similar to the Slack messages. The Telegram messages need more elaborate information to be sent."*

**Architecture:** Reshape the Telegram text to add a rendered timestamp line + footer line (compensating for the missing `ts`/`footer` chrome Slack renders for free), and switch `parse_mode` from `"Markdown"` (v1) to `"HTML"`. HTML mode is Telegram's most robust dialect — the escape set is exactly `< > &`, `<pre>` / `<code>` are supported for the failure code blocks that today rely on triple-backtick fences, and `<b>` renders bold reliably where `*bold*` in v1 breaks on any unmatched `*` in the surrounding text. Keeping the current one-shot plain-text fallback on HTTP 400 as a defense-in-depth safety net for anything the escaper misses. Message body is truncated at 4000 chars with an explicit "... (truncated)" suffix to stay under Telegram's 4096-char `sendMessage` cap. **No new fields** are added to `$Message` — the parity gap is presentation, not content.

**Tech Stack:** PowerShell 5.1+/7 (`scripts/monitor-export-and-serve.ps1`, `scripts/send-alert.ps1`), Pester 5. No .NET changes (SchedulerUI, `LocalConfigStore`, wizard all remain untouched — the config surface from the original telegram-alerts plan is unchanged).

## Global Constraints

- **Two dispatch sites, same shape:** `Send-TelegramMessage` in `scripts/monitor-export-and-serve.ps1:126` and the inline Telegram block in `scripts/send-alert.ps1:79-123` MUST produce byte-identical text for the same `(Kind, Message, InstanceLabel, Timestamp)` tuple. Extract a helper if you want, but a copy-paste with matching tests is also fine — do NOT let them drift.
- **HTML escape order matters:** escape `&` first, then `<`, then `>`. Reversed order double-escapes.
- **Timestamp format:** local time via `(Get-Date).ToString('yyyy-MM-dd HH:mm:ss zzz')` — mirrors Slack's `ts` rendering. UTC not used because Slack itself shows local time to the viewer, not UTC.
- **Footer text:** exactly `$instanceLabel` (matches Slack's `footer` field verbatim).
- **4096-char cap:** Telegram's `sendMessage` rejects text > 4096 chars with HTTP 400. Truncate the final composed text at 4000 chars (leaving headroom for the truncation suffix) and append `\n... (truncated)`.
- **Keep the one-shot plain-text fallback:** on HTTP 400, retry once with `parse_mode` omitted. Rationale: even properly-escaped HTML can 400 on unusual byte sequences; the fallback is cheap insurance and already tested.
- **Emoji map unchanged:** Start 🔄, Finish 🟢, Failure/ServeFailure/LlmFailure/ApiFailure 🔴, Recovery/ServeRecovery/LlmRecovery/ApiRecovery 🟢, Test 🔵, DailyDigest 📊, UserStop ✋.
- **Text shape (exact, HTML mode):**
  ```
  {emoji} <b>EAxWiki [{Kind}]</b> — {instanceLabelEscaped}
  {messageBodyEscaped-with-``` swapped for <pre>...</pre>}
  <i>{footerEscaped} • {timestamp}</i>
  ```
  Blank line before the footer line. En-dash (—) in the header separator to match a tightened Slack aesthetic; the footer uses a bullet (•) as separator.
- **Code fences:** the existing `Failure` message body includes triple-backtick fences (`monitor-export-and-serve.ps1:917`). In the HTML escaper, detect ` ``` ` blocks and convert to `<pre>...</pre>` with inner content HTML-escaped. This is the only fenced-block source in the message pipeline — keep the conversion narrow, don't try to be a general Markdown-to-HTML translator.
- **Do not restructure the monitor script's top-level body.** Do not touch `Send-Alert`'s Slack/Teams blocks. Do not touch `LocalConfigStore`, `SchedulerForm.cs`, `Program.cs` wizard, or `TELEGRAM_SETUP.md`.
- **Encoding (critical, carried over from the original plan):** any edited PS file containing literal Unicode emoji or en-dash (—) / bullet (•) must be saved as UTF-8 **with BOM**. Verify with `(Get-Content <path> -AsByteStream -TotalCount 3)` returning `239 187 191`.

---

### Task 1: Extract shared Telegram text-composer + HTML escaper

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1:126-187` (`Send-TelegramMessage`)
- Modify: `scripts/send-alert.ps1:79-123` (inline Telegram block)

**Interfaces:**
- Produces: `Format-TelegramAlertText -Kind -InstanceLabel -Message -Timestamp` returning the fully-composed HTML-mode text (single string, ≤ 4096 chars). Used by both dispatch sites in Task 2.

- [x] **Step 1: Add failing composer tests**

  In `tests/scripts/monitor-export-and-serve.Tests.ps1`, add a new `Describe 'Format-TelegramAlertText'` block covering:
  1. Header line uses `<b>` and en-dash: `"🔵 <b>EAxWiki [Test]</b> — host - C:\\wiki"`
  2. Footer line uses `<i>...</i>` and bullet separator, with timestamp in `yyyy-MM-dd HH:mm:ss zzz` format
  3. Message body HTML-escapes `<`, `>`, `&` (test payload: `"<script>&amp;"`) → contains `&lt;script&gt;&amp;amp;`
  4. Triple-backtick block in message body is converted to `<pre>...</pre>` with inner `<` / `>` / `&` escaped
  5. InstanceLabel containing `&` is escaped in both the header and the footer
  6. A 5000-char payload is truncated: result length ≤ 4096, ends with `\n... (truncated)`
  7. `Failure` kind uses 🔴 emoji; `DailyDigest` uses 📊; `UserStop` uses ✋

- [x] **Step 2: Run tests, expect failure**

  `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed` → the `Format-TelegramAlertText` describe fails because the function doesn't exist yet.

- [x] **Step 3: Implement `Format-TelegramAlertText`**

  Add a new function above `Send-TelegramMessage` in `scripts/monitor-export-and-serve.ps1`:

  ```powershell
  function ConvertTo-HtmlEscaped {
      param([string]$Text)
      if ($null -eq $Text) { return "" }
      return ($Text -replace '&', '&amp;') -replace '<', '&lt;' -replace '>', '&gt;'
  }

  function Format-TelegramAlertText {
      # Issue #80 follow-up: Telegram parity with Slack. Renders the same emoji/title Slack shows
      # in pretext, plus an explicit footer + timestamp line (Slack does this via `footer` + `ts`
      # automatically, Telegram has no equivalent chrome). HTML mode is used because Markdown v1
      # silently drops content on any unmatched `*` or `_` in the message body.
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

      # Convert ```...``` fenced blocks to <pre>...</pre>, HTML-escaping the inner content.
      # Only the failure body uses fences today; anything else passes through the plain escaper.
      $bodyHtml = [regex]::Replace(
          $Message,
          '(?s)```(.*?)```',
          { param($m) '<pre>' + (ConvertTo-HtmlEscaped $m.Groups[1].Value) + '</pre>' }
      )
      # Escape any remaining <, >, & OUTSIDE the <pre> blocks. Split on our just-emitted <pre>...</pre>
      # segments so we don't double-escape the inner content.
      $parts = [regex]::Split($bodyHtml, '(<pre>.*?</pre>)')
      for ($i = 0; $i -lt $parts.Length; $i++) {
          if (-not $parts[$i].StartsWith('<pre>')) {
              $parts[$i] = ConvertTo-HtmlEscaped $parts[$i]
          }
      }
      $bodyHtml = -join $parts

      $composed = "{0} <b>EAxWiki [{1}]</b> — {2}`n{3}`n`n<i>{2} • {4}</i>" -f `
          $emoji, $kindHtml, $labelHtml, $bodyHtml, $stamp

      if ($composed.Length -gt 4000) {
          $composed = $composed.Substring(0, 4000) + "`n... (truncated)"
      }
      return $composed
  }
  ```

- [x] **Step 4: Verify tests pass**

  `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`. All `Format-TelegramAlertText` tests green; no regressions in the pre-existing `Send-TelegramMessage` tests (they will fail on the exact-text assertion — that's expected and fixed in Task 2).

- [x] **Step 5: Verify BOM**

  `(Get-Content scripts/monitor-export-and-serve.ps1 -AsByteStream -TotalCount 3)` returns `239 187 191`.

---

### Task 2: Switch both dispatch sites to the new composer + HTML parse_mode

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1:157-163` (the `$text` composition and `$body.parse_mode`)
- Modify: `scripts/send-alert.ps1:79-123` (the inline Telegram block)
- Modify: `tests/scripts/monitor-export-and-serve.Tests.ps1` (`Send-TelegramMessage` tests — update the exact-text assertion to match the new HTML shape)
- Modify: `tests/scripts/send-alert.Tests.ps1` (matching updates for the send-alert dispatch tests)

**Interfaces:**
- Consumes: `Format-TelegramAlertText` from Task 1.
- Produces: HTTP POST body `{ chat_id, text, parse_mode: 'HTML' }` at both dispatch sites.

- [x] **Step 1: Update `Send-TelegramMessage`**

  Replace the `$tgEmoji` switch + `$text = "{0} *EAxWiki..." -f ...` composition with:
  ```powershell
  $text = Format-TelegramAlertText -Kind $Kind -InstanceLabel $InstanceLabel -Message $Message
  ```
  Change `parse_mode = 'Markdown'` → `parse_mode = 'HTML'`. The existing 400-fallback logic stays exactly as-is (it strips `parse_mode` and retries).

- [x] **Step 2: Update `scripts/send-alert.ps1`**

  Replace the entire `if ($TelegramBotToken -and $TelegramChatId)` block body with a call to `Format-TelegramAlertText`. Since `send-alert.ps1` today doesn't dot-source the monitor script, do one of:
  - **Preferred:** inline a copy of `Format-TelegramAlertText` + `ConvertTo-HtmlEscaped` at the top of `send-alert.ps1`. Keep it byte-identical to the monitor's copy. This preserves `send-alert.ps1`'s standalone character (no dependency on the 1000-line monitor script).
  - **Alternate:** dot-source `_bootstrap.ps1` and hoist the two helpers there. Only do this if there's an existing precedent for the bootstrap holding non-monitor-specific alert helpers (there isn't today — the bootstrap is small and focused).

  Go with the inline copy; add a `# Kept in sync with monitor-export-and-serve.ps1:Format-TelegramAlertText — see Task 1 constraint.` comment above each copy.

- [x] **Step 3: Update existing tests**

  In `tests/scripts/monitor-export-and-serve.Tests.ps1`, the 3 `Send-TelegramMessage` tests currently assert on the old Markdown text shape. Update the mocked `Invoke-RestMethod` capture assertions to match the new HTML text. Same for the 2 dispatch tests in `tests/scripts/send-alert.Tests.ps1`.

- [x] **Step 4: Run all Pester tests**

  `Invoke-Pester tests/scripts -Output Detailed`. All 143 (± the new composer tests from Task 1) green.

- [x] **Step 5: Run .NET tests**

  `dotnet test src/EAxWiki.Tests` → still 270 passing (no .NET changes in this plan; this is just a regression gate).

---

### Task 3: Update documentation

**Files:**
- Modify: `docs/TELEGRAM_SETUP.md` (a short "Message format" section)
- Modify: `README.md` (only if the Telegram section quotes a sample message body — if so, refresh to the new HTML shape; otherwise skip)

**Interfaces:** none.

- [x] **Step 1: Add "Message format" section to `docs/TELEGRAM_SETUP.md`**

  After the setup steps, describe the new shape: emoji + bold title, message body (with `<pre>` blocks for failure output), footer with instance label + timestamp. Note that Telegram HTML mode is used (not Markdown), so `<`, `>`, `&` in element names are shown escaped and don't break formatting.

- [x] **Step 2: Grep README for Telegram sample**

  `Grep -Path README.md -Pattern 'Telegram|telegram'`. If a sample message is quoted, update it. If only the setup / config bits are mentioned, no change needed.

---

### Task 4: Manual E2E verification

**Files:** none (verification only).

- [x] **Step 1: Fire a `--test-alert`**

  `.\scripts\monitor-export-and-serve.ps1 --test-alert` with the existing `.eaxwiki` Telegram credentials from the 2026-08-03 E2E. Confirm in the Telegram chat:
  1. Title line shows `🔵 EAxWiki [Test] — <computername> - <wikidir>` with **bold** on the bracketed part
  2. Message body renders on its own line
  3. Footer line shows the instance label + timestamp in italics, separated by `•`
  4. No literal `<b>`, `<i>`, `<pre>`, or `&amp;` visible — they render as formatting

- [x] **Step 2: Force a failure alert**

  Trigger a real Failure alert (e.g. rename the model file for one run so the export fails 3× and gives up). Confirm the code fence renders as a monospaced block in Telegram, not as literal triple-backticks. Rename the model back afterward.

- [x] **Step 3: Long-message truncation smoke**

  From an interactive PowerShell:
  ```powershell
  . .\scripts\monitor-export-and-serve.ps1  # dot-source, run-guard exits body
  $long = 'x' * 5000
  Format-TelegramAlertText -Kind Test -InstanceLabel 'host - wiki' -Message $long |
      ForEach-Object { $_.Length }
  ```
  Expect a number ≤ 4096 and a "... (truncated)" suffix.

- [x] **Step 4: Compare side-by-side with Slack**

  Fire one `--test-alert` while both Slack and Telegram are configured. Screenshot both. Confirm the presented information (title, body, instance, timestamp) matches in both channels — closing the parity gap the user reported.

---

## Rollback

Every change lives in `scripts/monitor-export-and-serve.ps1` + `scripts/send-alert.ps1` + their Pester tests + `docs/TELEGRAM_SETUP.md`. `git revert` the merge commit and Telegram falls back to the current Markdown v1 shape; no data migration, no config-file changes, no `LocalConfigStore` schema impact.

## Definition of Done

- All Pester tests green (existing 143 + new Format-TelegramAlertText tests from Task 1 Step 1).
- All 270 .NET tests green.
- Task 4 Steps 1–4 checked off in issue #80 with a screenshot of the new Telegram message next to the Slack message.
- Issue #80 closed.
