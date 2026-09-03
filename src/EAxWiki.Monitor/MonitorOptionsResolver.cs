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

        // Llama paths come only from the config file. There are no hardcoded defaults —
        // machine-specific paths as constants would silently activate a local LLM on any
        // machine that happens to have files at those locations. SchedulerUI / .eaxwiki
        // is the single source of truth.
        var llamaExe = file?.LlamaExePath is { Length: > 0 } le ? le : null;
        var llamaModel = file?.LlamaModelPath is { Length: > 0 } lm ? lm : null;

        var aiMode = file?.AiMode ?? "none";
        if ((aiMode == "none") && file?.AiEndpoint is { Length: > 0 } ep &&
            (ep.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase) ||
             ep.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase)) &&
            llamaExe is not null && llamaModel is not null &&
            File.Exists(llamaExe) && File.Exists(llamaModel))
        {
            aiMode = "local";
        }
        // Issue #94: if the user configured both LlamaExePath and LlamaModelPath in .eaxwiki
        // (paths present in the config AND resolving to real files on disk) and did NOT set
        // AiMode, treat that as intent to run the LLM locally. Otherwise the LLM watchdog
        // stays disarmed and llama-server never starts. Explicit AiMode="none" remains an
        // opt-out. Requires the paths in the config itself — no filesystem-based auto-enable
        // from defaults, since we no longer have any.
        if (aiMode == "none" && file?.AiMode is null &&
            file?.LlamaExePath is { Length: > 0 } fileLlamaExe &&
            file?.LlamaModelPath is { Length: > 0 } fileLlamaModel &&
            File.Exists(fileLlamaExe) && File.Exists(fileLlamaModel))
        {
            aiMode = "local";
        }

        var llmPort = cli.LlmPort ?? file?.LlmPort ?? DefaultLlmPort;

        // AiMode is authoritative for endpoint selection. In local mode the llama-server URL
        // is derived from LlmPort — any stale file?.AiEndpoint (e.g. a Reset-defaulted
        // "https://api.openai.com/v1" left in .eaxwiki) is ignored so the write-back API's
        // /api/ai-suggest doesn't send local-mode requests to a remote provider. In remote
        // mode the stored endpoint is used as-is.
        string? aiEndpoint;
        string? aiModel;
        if (aiMode == "local")
        {
            aiEndpoint = $"http://localhost:{llmPort}/v1";
            aiModel = file?.AiModel ?? "local";
        }
        else
        {
            aiEndpoint = file?.AiEndpoint;
            aiModel = file?.AiModel;
        }

        return new MonitorOptions
        {
            RepoPath = repoPath,
            WikiDir = Path.GetFullPath(wikiDir),
            WikiPort = wikiPort,
            ApiPort = file?.ApiPort ?? 0,
            LlmPort = llmPort,
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
            AiEndpoint = aiEndpoint,
            AiModel = aiModel,
            AiKey = aiMode == "local" ? null : file?.AiKey,
            LlamaExePath = llamaExe,
            LlamaModelPath = llamaModel,
        };
    }
}