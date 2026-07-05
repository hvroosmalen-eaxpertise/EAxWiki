# Microsoft Teams Webhook Setup for Monitoring Alerts

EAxWiki can send monitoring and alerting notifications to a Microsoft Teams channel when background export/serve operations start, encounter issues, or recover.

## Supported Webhook Type

Teams Incoming Webhooks are supported here. Slack is also supported — see [**Slack Webhook Setup**](SLACK_WEBHOOK_SETUP.md). The two are independent, not exclusive: configure either, neither, or both, and every alert goes to whichever channel(s) are set up.

## How to Create a Teams Incoming Webhook

Microsoft has been moving Teams away from classic "Connectors" toward "Workflows" (Power Automate), so the exact menu can differ by tenant. Try the classic Connector path first (Option A); if your tenant has that disabled, use the Workflows path (Option B) instead — either produces a webhook URL that works identically with EAxWiki.

### Option A — Classic Incoming Webhook connector

1. **Open the Teams channel** where you want alerts posted.
2. Click the **"…" (More options)** next to the channel name, then **"Connectors"** (sometimes under **"Manage channel"** → **"Connectors"** depending on your Teams version).
3. Search for **"Incoming Webhook"** and click **"Configure"** (or **"Add"**).
4. Give it a name (e.g. `EAxWiki`) and optionally upload an icon.
5. Click **"Create"**.
6. **Copy the webhook URL** shown — it's long, looks like `https://<tenant>.webhook.office.com/webhookb2/...`.
7. Click **"Done"**.

### Option B — Workflows (Power Automate)

If "Connectors" isn't available in your tenant:

1. In the target channel, click **"…"** → **"Workflows"**.
2. Search for the **"Post to a channel when a webhook request is received"** template.
3. Follow the prompts to select the channel and create the workflow.
4. **Copy the webhook URL** shown at the end of setup.

Either option produces a URL EAxWiki can POST a JSON payload to — no further Teams-side configuration is needed.

## Configure EAxWiki to Use Your Webhook

### During Initial Setup

When you first run EAxWiki, it will prompt for both channels in sequence:
```
Configure Slack webhook for monitoring alerts? [y/N]: n
Configure Teams webhook for monitoring alerts? [y/N]: y
Teams webhook URL (https://.../IncomingWebhook/...): <paste your URL here>
```

Answer `n` to the Slack prompt if you only want Teams — see [Slack Webhook Setup](SLACK_WEBHOOK_SETUP.md) if you want that too. Both webhook URLs are encrypted and saved to the `.eaxwiki` configuration file.

### Update Existing Configuration

To change the Teams webhook URL later:
1. Delete the `.eaxwiki` file in your repo root
2. Run EAxWiki again and it will prompt for both webhook URLs during interactive setup

## Testing Your Webhook

Send a one-off test message without running a real export:

```powershell
.\scripts\monitor-export-and-serve.ps1 --test-alert
```

This resolves each webhook URL the same way a real scheduled run does — `--webhook-url`/`--teams-webhook-url` argument, then `EAXWIKI_ALERT_WEBHOOK`/`EAXWIKI_ALERT_TEAMS_WEBHOOK` environment variable, then `.eaxwiki` — and posts a "Test" message to every channel that's configured. Check your Teams channel to confirm it arrived.

## When Alerts Are Sent

Alerts are sent by `scripts/monitor-export-and-serve.ps1` — the unattended wrapper used for [scheduled runs](../README.md#scheduling-exports), not by `export.ps1` or `export-and-serve.ps1` directly. Each scheduled pass can send:

| Kind | When |
|---|---|
| Start | Beginning of every scheduled run (disable with `--no-notify-start`) |
| Failure | Export failed after all retries are exhausted |
| Recovery | Export succeeded after a prior run had failed |
| ServeFailure | `mkdocs serve` failed to (re)start after all retries |
| ServeRecovery | `mkdocs serve` came back up after a prior serve failure |

A transient failure that succeeds on retry (within the same scheduled pass) does **not** alert — only the final outcome of a pass does, so a blip that resolves itself doesn't page anyone. If both Slack and Teams are configured, every alert goes to both — one channel failing to deliver (e.g. a revoked webhook) doesn't block the other.

## Security Notes

- **Keep your webhook URL secret** — do not commit `.eaxwiki` to git (it's gitignored)
- Webhook URLs are encrypted at rest in `.eaxwiki` using Windows DPAPI (encrypted for your user account only)
- If you suspect your webhook URL was exposed, delete the connector/workflow from the Teams channel and create a new one

## Troubleshooting

**Messages not appearing in Teams?**
- Verify the webhook URL is correct and complete (they're long)
- Check that the connector/workflow still exists in the channel — it can be removed independently of EAxWiki's own config
- If your tenant recently disabled classic Connectors, switch to the Workflows path (Option B above) and update `.eaxwiki` with the new URL

**Payload format**

EAxWiki posts the classic Teams [MessageCard](https://learn.microsoft.com/en-us/microsoftteams/platform/webhooks-and-connectors/how-to/connectors-using) format (`@type: MessageCard`), which both the classic Connector and the Workflows "webhook received" trigger accept.
