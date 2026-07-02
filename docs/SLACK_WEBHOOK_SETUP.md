# Slack Webhook Setup for Monitoring Alerts

EAxWiki can send monitoring and alerting notifications to Slack when background export/serve operations encounter issues or complete.

## Supported Webhook Type

Currently, only **Slack Incoming Webhooks** are supported. Teams webhook support is tracked in [GitHub issue #39](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues/39).

## How to Create a Slack Incoming Webhook

1. **Go to your Slack Workspace**
   - Navigate to https://api.slack.com/apps
   - Sign in with your workspace credentials

2. **Create a New App**
   - Click "Create New App"
   - Select "From scratch"
   - Name: `EAxWiki` (or your preferred name)
   - Select your workspace
   - Click "Create App"

3. **Enable Incoming Webhooks**
   - In the left sidebar, go to "Features" → "Incoming Webhooks"
   - Toggle "Activate Incoming Webhooks" to ON
   - Click "Add New Webhook to Workspace"
   - Select the channel where alerts should be posted (e.g., #alerts, #wiki-updates)
   - Click "Allow"

4. **Copy Your Webhook URL**
   - After authorization, you'll see a new webhook listed under "Webhook URLs for Your Workspace"
   - Copy the URL (looks like: `https://hooks.slack.com/services/T0BESDURCC9/B0BEJJYCJV9/...`)
   - **Keep this URL secret** — it allows anyone to post to your Slack channel

## Configure EAxWiki to Use Your Webhook

### During Initial Setup

When you first run EAxWiki, it will prompt:
```
Configure Slack webhook for monitoring alerts? [y/N]: y
Slack webhook URL (https://hooks.slack.com/services/...): <paste your URL here>
```

The webhook URL will be encrypted and saved to `.eaxwiki` configuration file.

### Update Existing Configuration

To change the webhook URL later:
1. Delete the `.eaxwiki` file in your repo root
2. Run EAxWiki again and it will prompt for a new webhook URL during interactive setup

## Testing Your Webhook

Send a one-off test message without running a real export:

```powershell
.\scripts\monitor-export-and-serve.ps1 --test-alert
```

This resolves the webhook URL the same way a real scheduled run does — `--webhook-url` argument, then `EAXWIKI_ALERT_WEBHOOK` environment variable, then `.eaxwiki` — and posts a blue "Test" message. Check your Slack channel to confirm it arrived.

## When Alerts Are Sent

Alerts are sent by `scripts/monitor-export-and-serve.ps1` — the unattended wrapper used for [scheduled runs](../README.md#scheduling-exports), not by `export.ps1` or `export-and-serve.ps1` directly. Each scheduled pass can send:

| Kind | When |
|---|---|
| Start | Beginning of every scheduled run (disable with `--no-notify-start`) |
| Failure | Export failed after all retries are exhausted |
| Recovery | Export succeeded after a prior run had failed |
| ServeFailure | `mkdocs serve` failed to (re)start after all retries |
| ServeRecovery | `mkdocs serve` came back up after a prior serve failure |

A transient failure that succeeds on retry (within the same scheduled pass) does **not** alert — only the final outcome of a pass does, so a blip that resolves itself doesn't page anyone.

## Security Notes

- **Keep your webhook URL secret** — do not commit `.eaxwiki` to git (it's gitignored)
- Webhook URLs are encrypted at rest in `.eaxwiki` using Windows DPAPI (encrypted for your user account only)
- If you suspect your webhook URL was exposed, revoke it immediately from https://api.slack.com/apps and create a new one

## Troubleshooting

**Messages not appearing in Slack?**
- Verify the webhook URL is correct (copy it again from https://api.slack.com/apps)
- Check that the selected channel still exists
- Ensure the app has permissions to post to the channel
- Review Slack workspace notifications settings

**"Error: Invalid webhook URL"?**
- Make sure you copied the complete URL including `https://`
- Webhook URLs are long strings starting with `https://hooks.slack.com/services/`

## Future: Teams Webhook Support

Teams webhook support is planned. Track progress in [GitHub issue #39](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues/39).
