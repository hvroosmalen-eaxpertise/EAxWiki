# Slack Webhook Setup for Monitoring Alerts

EAxWiki can send monitoring and alerting notifications to Slack when background export/serve operations encounter issues or complete.

## Supported Webhook Type

Currently, only **Slack Incoming Webhooks** are supported. Teams webhook support is tracked in [GitHub issue #??](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues).

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

After configuration, EAxWiki will automatically send alerts to your Slack channel when:
- Export/serve operations start or complete
- Errors occur during processing
- Monitoring thresholds are exceeded (if enabled)

Check your Slack channel to confirm messages are being delivered.

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

Teams webhook support is planned. Track progress in [GitHub issue #??](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues).
