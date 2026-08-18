using System.CommandLine;

namespace EAxWiki.Monitor;

/// <summary>
/// System.CommandLine root command for the monitor. Flag surface mirrors the PS monitor's
/// Get-MonitorArgs (plus the new --llm-port). A bare non-flag argument is accepted as the
/// repo path via UnmatchedTokens (PS accepted a bare connection string the same way).
/// </summary>
public static class MonitorCommandLine
{
    public static RootCommand BuildCommand()
    {
        var repo = new Option<string?>("--repo", "-r") { Description = "EA repository path or connection string (defaults to .eaxwiki repoPath)." };
        var output = new Option<string?>("--output", "-o") { Description = "Wiki output directory, absolute or relative to the repo root (default: wiki)." };
        var port = new Option<int?>("--port", "-p") { Description = "Wiki (mkdocs serve) port (default 8000, or .eaxwiki wikiPort)." };
        var maxRetries = new Option<int?>("--max-retries") { Description = "Max export/service start attempts (default 3)." };
        var retryDelay = new Option<int?>("--retry-delay") { Description = "Retry backoff base in seconds (default 30)." };
        var minElementFraction = new Option<double?>("--min-element-fraction") { Description = "Minimum element-count floor as a fraction of the previous run (default 0.5)." };
        var webhook = new Option<string?>("--webhook-url") { Description = "Slack webhook URL." };
        var teamsWebhook = new Option<string?>("--teams-webhook-url") { Description = "Microsoft Teams webhook URL." };
        var telegramToken = new Option<string?>("--telegram-bot-token") { Description = "Telegram bot token." };
        var telegramChatId = new Option<string?>("--telegram-chat-id") { Description = "Telegram chat id (string; group ids are negative)." };
        var brand = new Option<string?>("--brand") { Description = "Wiki brand (e.g. eursura)." };
        var testAlert = new Option<bool>("--test-alert") { Description = "Send a Test alert to every configured channel and exit." };
        var noNotifyStart = new Option<bool?>("--no-notify-start") { Description = "Suppress Start and Finish alerts." };
        var force = new Option<bool>("--force", "-f") { Description = "Full rebuild on every run." };
        var forceEvery = new Option<int?>("--force-every") { Description = "Full rebuild every Nth run (0 = incremental only)." };
        var exportInterval = new Option<int?>("--export-interval") { Description = "Export cadence in minutes (default 30)." };
        var checkInterval = new Option<int?>("--check-interval") { Description = "Monitor loop sleep in seconds (default 30)." };
        var llmPort = new Option<int?>("--llm-port") { Description = "LLM server port (default 8080, or .eaxwiki llmPort)." };

        var root = new RootCommand("EAxWiki unattended monitor: export, serve, write-back API and LLM watchdogs.")
        {
            repo, output, port, maxRetries, retryDelay, minElementFraction,
            webhook, teamsWebhook, telegramToken, telegramChatId, brand,
            testAlert, noNotifyStart, force, forceEvery, exportInterval, checkInterval, llmPort,
        };

        // A bare positional argument is a connection string / repo path. System.CommandLine
        // rejects unknown tokens by default; treat them as unmatched instead and read them
        // in ToOptions (a plain .qea path or "DBType=...;..." never starts with '-').
        root.TreatUnmatchedTokensAsErrors = false;
        root.SetAction(_ => Task.FromResult(0));
        return root;
    }

    public static CliOptions ToOptions(ParseResult r)
    {
        // Ignore unknown-looking flags in the unmatched set so a typo'd --flg is reported as
        // "unknown option" instead of being silently swallowed as the repo path.
        var bare = r.UnmatchedTokens.FirstOrDefault(t => !t.StartsWith("-", StringComparison.Ordinal));
        return new CliOptions
        {
            Repo = r.GetResult("--repo") is System.CommandLine.Parsing.OptionResult { Implicit: false } ? r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--repo")) : bare,
            OutputDir = r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--output")),
            Port = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--port")),
            MaxRetries = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--max-retries")),
            RetryDelaySeconds = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--retry-delay")),
            MinElementFraction = r.GetValue((Option<double?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--min-element-fraction")),
            WebhookUrl = r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--webhook-url")),
            TeamsWebhookUrl = r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--teams-webhook-url")),
            TelegramBotToken = r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--telegram-bot-token")),
            TelegramChatId = r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--telegram-chat-id")),
            Brand = r.GetValue((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--brand")),
            TestAlert = r.GetValue((Option<bool>)r.RootCommandResult.Command.Children.First(o => o.Name == "--test-alert")),
            NotifyOnStart = r.GetResult("--no-notify-start") is System.CommandLine.Parsing.OptionResult { Implicit: false } ? false : null,
            Force = r.GetValue((Option<bool>)r.RootCommandResult.Command.Children.First(o => o.Name == "--force")),
            ForceEveryNRuns = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--force-every")),
            ExportIntervalMinutes = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--export-interval")),
            CheckIntervalSeconds = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--check-interval")),
            LlmPort = r.GetValue((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "--llm-port")),
        };
    }
}