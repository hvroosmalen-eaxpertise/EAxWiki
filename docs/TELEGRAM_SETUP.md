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
