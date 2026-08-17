namespace EAxWiki.Monitor;

/// <summary>
/// Parsed-but-unresolved command-line surface. Null means "not given on the command line";
/// resolution against env vars and .eaxwiki happens in <see cref="MonitorOptionsResolver"/>.
/// </summary>
public sealed record CliOptions
{
    public string? Repo { get; init; }
    public string? OutputDir { get; init; }
    public int? Port { get; init; }
    public int? MaxRetries { get; init; }
    public int? RetryDelaySeconds { get; init; }
    public double? MinElementFraction { get; init; }
    public string? WebhookUrl { get; init; }
    public string? TeamsWebhookUrl { get; init; }
    public string? TelegramBotToken { get; init; }
    public string? TelegramChatId { get; init; }
    public string? Brand { get; init; }
    public bool TestAlert { get; init; }
    public bool? NotifyOnStart { get; init; }
    public bool Force { get; init; }
    public int? ForceEveryNRuns { get; init; }
    public int? ExportIntervalMinutes { get; init; }
    public int? CheckIntervalSeconds { get; init; }
    public int? LlmPort { get; init; }
}