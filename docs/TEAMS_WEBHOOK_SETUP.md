# Microsoft Teams Webhook Setup for Monitoring Alerts

EAxWiki can send monitoring and alerting notifications to a Microsoft Teams channel when background export/serve operations start, encounter issues, or recover.

## Supported Webhook Type

Teams Incoming Webhooks are supported here. Slack is also supported — see [**Slack Webhook Setup**](SLACK_WEBHOOK_SETUP.md). The two are independent, not exclusive: configure either, neither, or both, and every alert goes to whichever channel(s) are set up.

## How to Create a Teams Webhook (Workflows)

Microsoft has been retiring the classic "Connectors" feature in favor of "Workflows" (built on Power Automate) — use Workflows unless your tenant genuinely doesn't offer it. These are the actual steps, verified end-to-end with a real alert landing in a real channel:

1. **Create or open a Team** — In Teams, go to **Teams and Channels** in the left sidebar, click **Create Team**, then select **Create team**.
2. **Add a channel** — Within the team, click **Add channel**, give it a name, and select a channel type (Standard/Private/Shared).
3. **Create a Workflow** — Open the new channel and, from its options, create a **Workflow**.
4. **Find the webhook template** — In the Workflows panel, search for **"webhook"** and select **"Send webhook alert to a channel"** (exact template wording can vary slightly by tenant/Teams version — look for anything mentioning a channel + webhook).
5. **Name the workflow** — Give it a name (e.g. `EAxWiki Alerts`) to finish creating it.
6. **Copy the webhook URL** — Power Automate shows the URL for this flow once it's created. It's long — something like:
   ```
   https://<region>.environment.api.powerplatform.com/powerautomate/automations/direct/workflows/<id>/triggers/manual/paths/invoke?api-version=1&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=<signature>
   ```

No further Teams-side configuration is needed — this URL is what EAxWiki posts a JSON payload to.

> **If your tenant only has classic "Connectors" and no "Workflows" option**, look for **"Incoming Webhook"** under **Connectors** instead (via the channel's **"…"** → **"Connectors"** menu). It produces a shorter `https://<tenant>.webhook.office.com/webhookb2/...` URL that works identically with EAxWiki, but Microsoft is phasing this path out.

## Configure EAxWiki to Use Your Webhook

### During Initial Setup

When you first run EAxWiki, it will prompt for both channels in sequence:
```
Configure Slack webhook for monitoring alerts? [y/N]: n
Configure Teams webhook for monitoring alerts? [y/N]: y
Teams webhook URL (from a Workflows "Send webhook alert to a channel" flow, or a classic Connector): <paste your URL here>
```

Answer `n` to the Slack prompt if you only want Teams — see [Slack Webhook Setup](SLACK_WEBHOOK_SETUP.md) if you want that too. Both webhook URLs are encrypted and saved to the `.eaxwiki` configuration file.

### Update Existing Configuration

To change the Teams webhook URL later:
1. Delete the `.eaxwiki` file in your repo root
2. Run EAxWiki again and it will prompt for both webhook URLs during interactive setup

## Testing Your Webhook

Send a one-off test message without running a real export:

```powershell
.\src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe --test-alert
```

This resolves each webhook URL the same way a real scheduled run does — `--webhook-url`/`--teams-webhook-url` argument, then `EAXWIKI_ALERT_WEBHOOK`/`EAXWIKI_ALERT_TEAMS_WEBHOOK` environment variable, then `.eaxwiki` — and posts a "Test" message to every channel that's configured. Check your Teams channel to confirm it arrived.

## When Alerts Are Sent

Alerts are sent by `EAxWiki.Monitor.exe` — the unattended monitor used for [scheduled runs](../README.md#scheduling-exports), not by `export.ps1` or `export-and-serve.ps1` directly. Each scheduled pass can send:

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
- If your tenant recently disabled classic Connectors, switch to the Workflows path above and update `.eaxwiki` with the new URL

**Payload format**

EAxWiki posts the classic Teams [MessageCard](https://learn.microsoft.com/en-us/microsoftteams/platform/webhooks-and-connectors/how-to/connectors-using) format (`@type: MessageCard`), which both the Workflows "webhook alert" trigger and the classic Connector accept.
