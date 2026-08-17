using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public enum AlertKind
{
    Start, Finish, Failure, Recovery, ServeFailure, ServeRecovery,
    LlmFailure, LlmRecovery, ApiFailure, ApiRecovery, Test, DailyDigest, UserStop,
}

public interface IAlertDispatcher
{
    void Dispatch(string message, AlertKind kind);
}

public record AlertOptions(
    string? WebhookUrl,
    string? TeamsWebhookUrl,
    string? TelegramBotToken,
    string? TelegramChatId,
    string InstanceLabel);

/// <summary>
/// Port of the PS Send-Alert + Send-TelegramMessage: Slack attachments, Teams MessageCard, and
/// Telegram HTML messages. Channels are independent, not exclusive — an alert goes to every
/// configured channel. Injectable <see cref="HttpMessageHandler"/> for unit tests.
/// </summary>
public class AlertDispatcher : IAlertDispatcher
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private readonly AlertOptions _options;
    private readonly HttpClient _http;
    private readonly ILogger _logger;

    public AlertDispatcher(AlertOptions options, HttpMessageHandler? handler, ILogger logger)
    {
        _options = options;
        _http = handler is null ? new HttpClient() : new HttpClient(handler);
        _logger = logger;
    }

    public void Dispatch(string message, AlertKind kind)
    {
        _logger.LogInformation("[{Kind}] {Message}", kind, message);
        if (string.IsNullOrEmpty(_options.WebhookUrl) &&
            string.IsNullOrEmpty(_options.TeamsWebhookUrl) &&
            string.IsNullOrEmpty(_options.TelegramBotToken) &&
            string.IsNullOrEmpty(_options.TelegramChatId))
        {
            _logger.LogInformation("No alert channel configured; alert logged only.");
            return;
        }

        var color = TelegramAlertTextFormatter.ColorFor(kind);
        var emoji = SlackEmojiFor(kind);

        if (!string.IsNullOrEmpty(_options.WebhookUrl))
            SendSlackAsync(_options.WebhookUrl, kind, emoji, color, message).GetAwaiter().GetResult();

        if (!string.IsNullOrEmpty(_options.TeamsWebhookUrl))
            SendTeamsAsync(_options.TeamsWebhookUrl, kind, color, message).GetAwaiter().GetResult();

        if (!string.IsNullOrEmpty(_options.TelegramBotToken) && !string.IsNullOrEmpty(_options.TelegramChatId))
            SendTelegramAsync(kind, message).GetAwaiter().GetResult();
    }

    private static string SlackEmojiFor(AlertKind kind) => kind switch
    {
        AlertKind.Start => ":arrows_counterclockwise:",
        AlertKind.Finish => ":large_green_circle:",
        AlertKind.Failure => ":red_circle:",
        AlertKind.ServeFailure => ":red_circle:",
        AlertKind.LlmFailure => ":red_circle:",
        AlertKind.ApiFailure => ":red_circle:",
        AlertKind.Recovery => ":large_green_circle:",
        AlertKind.ServeRecovery => ":large_green_circle:",
        AlertKind.LlmRecovery => ":large_green_circle:",
        AlertKind.ApiRecovery => ":large_green_circle:",
        AlertKind.Test => ":large_blue_circle:",
        AlertKind.DailyDigest => ":bar_chart:",
        AlertKind.UserStop => ":raised_hand:",
        _ => ":large_blue_circle:",
    };

    private async Task SendSlackAsync(string url, AlertKind kind, string emoji, string color, string message)
    {
        var payload = new
        {
            attachments = new[]
            {
                new
                {
                    color,
                    mrkdwn_in = new[] { "text", "pretext" },
                    pretext = $"{emoji} *EAxWiki [{kind}]* - {_options.InstanceLabel}",
                    text = message,
                    footer = _options.InstanceLabel,
                    ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                }
            }
        };
        try
        {
            await _http.PostAsJsonAsync(url, payload, Json);
            _logger.LogInformation("Slack webhook dispatched.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Slack webhook dispatch failed: {Error}", ex.Message);
        }
    }

    private async Task SendTeamsAsync(string url, AlertKind kind, string color, string message)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@type"] = "MessageCard",
            ["@context"] = "http://schema.org/extensions",
            ["themeColor"] = color.TrimStart('#'),
            ["summary"] = $"EAxWiki [{kind}] - {_options.InstanceLabel}",
            ["sections"] = new[]
            {
                new
                {
                    activityTitle = $"EAxWiki [{kind}] - {_options.InstanceLabel}",
                    text = message,
                }
            }
        };
        try
        {
            await _http.PostAsJsonAsync(url, payload, Json);
            _logger.LogInformation("Teams webhook dispatched.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Teams webhook dispatch failed: {Error}", ex.Message);
        }
    }

    private async Task SendTelegramAsync(AlertKind kind, string message)
    {
        var uri = $"https://api.telegram.org/bot{_options.TelegramBotToken}/sendMessage";
        var text = TelegramAlertTextFormatter.Format(kind, _options.InstanceLabel, message, DateTimeOffset.Now);
        var body = new Dictionary<string, object?>
        {
            ["chat_id"] = _options.TelegramChatId,
            ["text"] = text,
            ["parse_mode"] = "HTML",
        };

        var attempts = 0;
        while (true)
        {
            attempts++;
            try
            {
                var response = await _http.PostAsJsonAsync(uri, body, Json);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Telegram dispatched.");
                    return;
                }
                var status = (int)response.StatusCode;
                if (status == 400 && attempts == 1 && body.ContainsKey("parse_mode"))
                {
                    body.Remove("parse_mode");
                    continue;
                }
                _logger.LogWarning("Telegram dispatch failed: HTTP {Status}", status);
                return;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Telegram dispatch failed: {Error}", ex.Message);
                return;
            }
        }
    }
}