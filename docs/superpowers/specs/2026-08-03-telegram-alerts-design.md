# Message to Telegram (Issue #80)

## Problem

Monitoring alerts today go to Slack and/or Teams via incoming webhook URLs. Telegram is a third channel the user wants. But Telegram has no "incoming webhook URL" for sending messages — a bot sends messages through the Bot API, which needs a **bot token** (from @BotFather) and a **chat ID** (the destination). This shapes the whole design: two config values instead of one, token embedded in the API URL, and no colored cards (plain text + emoji + Markdown only).

## Decisions

- **Full parity with Slack/Teams**: Telegram is a third independent channel, not a replacement. Configure any/all/none; every alert goes to whichever channels are set up, and one channel failing never blocks the others (same as the existing issue #39 behavior).
- **One channel**: a single bot token + chat ID, matching the single-webhook-per-channel pattern.
- **Message style**: Unicode emoji per alert kind + `*bold*` labels via `parse_mode: "Markdown"`. Telegram's plain Bot API messages have no colors/cards, so the Slack `color` / Teams `themeColor` mechanism does not apply.
- **Full parity config surface**: CLI args → env vars → encrypted `.eaxwiki` → SchedulerUI field → interactive setup prompt, plus a `TELEGRAM_SETUP.md` doc with the BotFather flow.
- **Token is a secret**: never baked into a scheduled-task command line (same rule as the webhooks today — `register-scheduled-task.ps1` won't accept it as an action argument, only env var or `.eaxwiki`).
- **Resilience**: per-channel try/catch with a one-shot plain-text fallback if Telegram rejects the Markdown payload (HTTP 400), so one `*` in a log line can't kill an alert.

## Configuration surface

New `.eaxwiki` fields (both optional, DPAPI-encrypted like the webhook URLs):

| Field | CLI arg | Env var |
|---|---|---|
| `TelegramBotToken: string?` | `--telegram-bot-token` / `-TelegramBotToken` | `EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN` |
| `TelegramChatId: string?` | `--telegram-chat-id` / `-TelegramChatId` | `EAXWIKI_ALERT_TELEGRAM_CHAT_ID` |

Added to `LocalConfigStore.Config` (src/EAxWiki.Core/Configuration/LocalConfigStore.cs). Resolution order per value: CLI arg → env var → `.eaxwiki`.

Interactive wizard (src/EAxWiki/Program.cs), after the Teams prompt:

```
Configure Telegram monitoring alerts? [y/N]:
Telegram bot token (from @BotFather):
Telegram chat ID (numeric, the destination chat):
```

SchedulerUI Configuration tab (SchedulerForm.cs): a "Telegram bot token" and "Telegram chat ID" text pair, loaded/saved alongside the existing webhook boxes; validation: token non-empty → try parse as URI-free string, chat ID must parse as an integer.

## Dispatch

`Send-Alert` in `scripts/monitor-export-and-serve.ps1` gains a third independent block (`if ($TelegramBotToken -and $TelegramChatId)`) after the Slack and Teams blocks. `scripts/send-alert.ps1` gets the same block for the SchedulerUI "stopped by user" path.

New `$tgEmoji` switch (Unicode, replacing the Slack-style `:name:` map):

| Kind | Emoji |
|---|---|
| Start | 🔄 |
| Finish | 🟢 |
| Failure / ServeFailure / LlmFailure / ApiFailure | 🔴 |
| Recovery / ServeRecovery / LlmRecovery / ApiRecovery | 🟢 |
| Test | 🔵 |
| DailyDigest | 📊 |
| UserStop | ✋ |

Message text: `{emoji} *EAxWiki [{Kind}]* - {instanceLabel}` then newline + `$Message`.

Payload to `https://api.telegram.org/bot{TOKEN}/sendMessage`:

```json
{ "chat_id": "<chatId>", "text": "<formatted>", "parse_mode": "Markdown" }
```

Token embedded in the URL (standard Telegram pattern); `chat_id` in the body.

Resilience:
- Own try/catch — Telegram failure logs and never blocks Slack/Teams.
- `Write-MonitorLog` on success ("Telegram dispatched.") and failure ("Telegram dispatch failed: ...").
- On HTTP 400 (likely malformed Markdown in `$Message`), retry **once** with `parse_mode` omitted (plain-text fallback). No loop.
- `--test-alert` copy and the monitor's no-webhook guard message updated to mention Telegram.

## Docs

New `docs/TELEGRAM_SETUP.md` (mirroring SLACK/TEAMS setup docs):
- Step 1 — Create the bot: message @BotFather → `/newbot` → choose name/username → copy the token (`123456789:AA...`).
- Step 2 — Add the bot to a destination: private chat (message the bot once), group (add member + post a test message), or channel (add as admin).
- Step 3 — Resolve the chat ID (numeric): call `https://api.telegram.org/bot<TOKEN>/getUpdates` after messaging the bot; the `chat.id` for a group/channel is a negative number (e.g. `-1001234567890`), for a private chat it's the numeric user ID. The numeric ID is required — a `@username` cannot be used.
- Step 4 — Configure EAxWiki: interactive prompt, `.eaxwiki` fields, or env vars (with the "delete `.eaxwiki` to re-prompt" note).
- Testing: `.\scripts\monitor-export-and-serve.ps1 --test-alert`.
- When alerts are sent: reuse the existing kind table.
- Security notes: token is a secret, keep out of git, DPAPI-encrypted at rest, revoke via BotFather `/revoke` if leaked.
- Troubleshooting: "bot can't be found" (wrong token), "chat not found" (bot never added / stale chat ID), message-length limits, Markdown fallback.

README.md updates:
- Monitoring & Alerting section: add Telegram to the channel list.
- `--test-alert` / webhook resolution text: add the two Telegram flags + two env vars.
- Scheduler GUI Configuration tab description: mention the Telegram fields.
- `register-scheduled-task.ps1` header comment listing webhook resolution: add Telegram.

## Testing

- Pester (`tests/`): Telegram block skipped when no token; payload shape (`chat_id`, `text`, `parse_mode`); Markdown-fallback retry without `parse_mode` on a 400; `send-alert.ps1` forwards the Telegram args.
- `LocalConfigStoreTests`: round-trip for `TelegramBotToken` and `TelegramChatId`.
- Manual E2E: real BotFather bot → `--test-alert` lands in the chat.

## Out of scope

- Multiple chat destinations
- Inline keyboards
- Message editing / dedup of repeated alerts
- Anything beyond `sendMessage`

## Risks

- Negative group chat IDs (`-100...`) in JSON — pass as a string in the payload, not a PowerShell number.
- Markdown-fallback retry must be exactly one attempt, no loop.
- `Send-Alert`'s existing per-channel try/catch seam means Telegram slots in without touching Slack/Teams behavior.

## New/affected files

- `src/EAxWiki.Core/Configuration/LocalConfigStore.cs` — two new `Config` fields
- `src/EAxWiki/Program.cs` — wizard prompts + persist
- `src/EAxWiki.SchedulerUI/SchedulerForm.cs` — Telegram fields + validation + send-alert args
- `scripts/monitor-export-and-serve.ps1` — args, resolution, `$tgEmoji`, dispatch block, guard/test copy
- `scripts/send-alert.ps1` — params + dispatch block
- `scripts/register-scheduled-task.ps1` — header comment only
- `docs/TELEGRAM_SETUP.md` — new
- `README.md` — channel/resolution docs
- `src/EAxWiki.Tests/LocalConfigStoreTests.cs` — round-trip tests
- `tests/` (Pester) — Telegram dispatch tests

## Related

- Issue #80 (this issue)
- Existing channel behavior documented for issue #39 (Slack + Teams independence) in `docs/design-decisions.md`
