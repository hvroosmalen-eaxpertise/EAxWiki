using EAxWiki.Core.Configuration;

namespace EAxWiki.Monitor;

/// <summary>
/// Resolution order for every option: CLI arg → env var → .eaxwiki file (unchanged from the PS
/// monitor). Ports keep their quirk: --port defaults to 8000, and if it is still exactly 8000
/// while .eaxwiki has a different wikiPort, the file wins (so a scheduled task that omits --port
/// picks up a config-file port).
/// </summary>
public static class MonitorOptionsResolver
{
    private const int DefaultPort = 8000;
    private const int DefaultLlmPort = 8080;
    private const string DefaultLlamaExe = @"E:\llama-cpp\llama-server.exe";
    private const string DefaultLlamaModel = @"E:\models\llama-3.2-3b-q4.gguf";

    public static MonitorOptions Resolve(CliOptions cli, string repoRoot,
        Func<string, string?> getEnv, LocalConfigStore.Config? file)
    {
        var wikiPort = cli.Port ?? DefaultPort;
        if (wikiPort == DefaultPort && file?.WikiPort is { } fw && fw != DefaultPort)
            wikiPort = fw;

        var repoPath = cli.Repo ?? file?.RepoPath;

        var wikiDir = cli.OutputDir is { Length: > 0 } outDir
            ? (Path.IsPathRooted(outDir) ? outDir : Path.Combine(repoRoot, outDir))
            : Path.Combine(repoRoot, "wiki");

        var llamaExe = file?.LlamaExePath is { Length: > 0 } le ? le : DefaultLlamaExe;
        var llamaModel = file?.LlamaModelPath is { Length: > 0 } lm ? lm : DefaultLlamaModel;

        var aiMode = file?.AiMode ?? "none";
        if ((aiMode == "none") && file?.AiEndpoint is { Length: > 0 } ep &&
            (ep.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase) ||
             ep.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase)) &&
            File.Exists(llamaExe))
        {
            aiMode = "local";
        }
        // Issue #94: if the user explicitly configured llama paths in .eaxwiki
        // (both exe and model present on disk) and did NOT set AiMode, treat that
        // as intent to run the LLM locally — otherwise the LLM watchdog stays
        // disarmed and llama-server never starts. Explicit AiMode="none" is
        // respected as an opt-out (checked via `file?.AiMode is null`).
        if (aiMode == "none" && file?.AiMode is null &&
            file?.LlamaExePath is { Length: > 0 } fileLlamaExe &&
            file?.LlamaModelPath is { Length: > 0 } fileLlamaModel &&
            File.Exists(fileLlamaExe) && File.Exists(fileLlamaModel))
        {
            aiMode = "local";
        }

        return new MonitorOptions
        {
            RepoPath = repoPath,
            WikiDir = Path.GetFullPath(wikiDir),
            WikiPort = wikiPort,
            ApiPort = file?.ApiPort ?? 0,
            LlmPort = cli.LlmPort ?? file?.LlmPort ?? DefaultLlmPort,
            MaxRetries = cli.MaxRetries ?? 3,
            RetryDelaySeconds = cli.RetryDelaySeconds ?? 30,
            MinElementFraction = cli.MinElementFraction ?? 0.5,
            WebhookUrl = cli.WebhookUrl ?? getEnv("EAXWIKI_ALERT_WEBHOOK") ?? file?.WebhookUrl,
            TeamsWebhookUrl = cli.TeamsWebhookUrl ?? getEnv("EAXWIKI_ALERT_TEAMS_WEBHOOK") ?? file?.TeamsWebhookUrl,
            TelegramBotToken = cli.TelegramBotToken ?? getEnv("EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN") ?? file?.TelegramBotToken,
            TelegramChatId = cli.TelegramChatId ?? getEnv("EAXWIKI_ALERT_TELEGRAM_CHAT_ID") ?? file?.TelegramChatId,
            Brand = cli.Brand ?? getEnv("EAXWIKI_BRAND") ?? file?.Brand,
            TestAlert = cli.TestAlert,
            NotifyOnStart = cli.NotifyOnStart ?? true,
            Force = cli.Force,
            ForceEveryNRuns = cli.ForceEveryNRuns ?? 0,
            ExportIntervalMinutes = cli.ExportIntervalMinutes ?? 30,
            CheckIntervalSeconds = cli.CheckIntervalSeconds ?? 30,
            AiMode = aiMode,
            AiEndpoint = file?.AiEndpoint,
            AiModel = file?.AiModel,
            LlamaExePath = llamaExe,
            LlamaModelPath = llamaModel,
        };
    }
}