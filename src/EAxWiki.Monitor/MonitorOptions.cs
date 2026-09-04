namespace EAxWiki.Monitor;

/// <summary>
/// Fully-resolved monitor options (CLI arg → env var → .eaxwiki). Everything the MonitorLoop,
/// ExportRunner, AlertDispatcher and ProcessSupervisor need; immutable.
/// </summary>
public sealed record MonitorOptions
{
    public string? RepoPath { get; init; }
    public string WikiDir { get; init; } = string.Empty;
    public int WikiPort { get; init; } = 8000;
    public int ApiPort { get; init; }
    public int LlmPort { get; init; } = 8080;
    public int MaxRetries { get; init; } = 3;
    public int RetryDelaySeconds { get; init; } = 30;
    public double MinElementFraction { get; init; } = 0.5;
    public string? WebhookUrl { get; init; }
    public string? TeamsWebhookUrl { get; init; }
    public string? TelegramBotToken { get; init; }
    public string? TelegramChatId { get; init; }
    public bool TestAlert { get; init; }
    public bool NotifyOnStart { get; init; } = true;
    public bool Force { get; init; }
    public int ForceEveryNRuns { get; init; }
    public int ExportIntervalMinutes { get; init; } = 30;
    public int CheckIntervalSeconds { get; init; } = 30;
    public string AiMode { get; init; } = "none";
    public string? AiEndpoint { get; init; }
    public string? AiModel { get; init; }
    public string? AiKey { get; init; }
    public string? LlamaExePath { get; init; }
    public string? LlamaModelPath { get; init; }
}