using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public static class MonitorApp
{
    public static MonitorLoop Build(
        MonitorOptions options,
        HealthState state,
        HealthStore healthStore,
        string stateDir,
        ILoggerFactory loggerFactory)
    {
        var templateDir = Path.GetDirectoryName(stateDir)!;

        var pageRenderer = new HealthPageRenderer(
            Path.Combine(templateDir, "health-template.md"),
            options.WikiDir);

        var errorsPageRenderer = new ErrorLogPageRenderer(
            Path.Combine(templateDir, "errors-template.md"),
            options.WikiDir,
            Path.Combine(stateDir, "logs"),
            new[] { options.WebhookUrl ?? "", options.TeamsWebhookUrl ?? "", options.TelegramBotToken ?? "" });
        var configPageRenderer = new ConfigPageRenderer(options.WikiDir, new ScheduledTaskSnapshot());

        var alerts = new AlertDispatcher(
            new AlertOptions(options.WebhookUrl, options.TeamsWebhookUrl,
                options.TelegramBotToken, options.TelegramChatId,
                $"{Environment.MachineName} - {options.WikiDir}"),
            null,
            loggerFactory.CreateLogger("Alert"));

        var digestTracker = new DigestTracker(state, options.WikiDir, Path.Combine(stateDir, "logs"),
            Path.Combine(templateDir, "digest-template.md"));

        var exporter = new StaMarkdownExporter(loggerFactory);
        var metrics = new WikiOutputMetrics();
        var exportRunner = new ExportRunner(options, exporter, metrics, state, alerts,
            loggerFactory.CreateLogger<ExportRunner>());

        var supervisor = new ProcessSupervisor(loggerFactory.CreateLogger("Supervisor"),
            new TcpPortProbe(), new NetstatPortKiller());

        var serveSpec = BuildServeSpec(options, stateDir);
        var apiSpec = BuildApiSpec(options, stateDir);
        var llmSpec = BuildLlmSpec(options, stateDir);

        return new MonitorLoop(options, state, healthStore, pageRenderer, errorsPageRenderer, configPageRenderer,
            stateDir, exportRunner, digestTracker, alerts, supervisor, serveSpec, apiSpec, llmSpec!,
            loggerFactory.CreateLogger("MonitorLoop"));
    }

    public static ServiceSpec BuildServeSpec(MonitorOptions options, string stateDir)
    {
        return new ServiceSpec(
            "serve",
            Path.Combine(stateDir, "serve.pid"),
            MonitorPaths.FindPowerShell(),
            new[] { "-NoProfile", "-File", "scripts\\serve.ps1", "--port", options.WikiPort.ToString(), "--wiki-dir", options.WikiDir },
            Path.Combine(stateDir, "logs"),
            Port: options.WikiPort,
            PortProbeFallback: true,
            WorkingDirectory: ResolveRepoRoot(stateDir),
            PostStartDelaySeconds: 5);
    }

    public static ServiceSpec BuildApiSpec(MonitorOptions options, string stateDir)
    {
        var repoRoot = ResolveRepoRoot(stateDir);
        var projDir = Path.Combine(repoRoot, "src", "EAxWiki", "bin", "Debug", "net10.0");
        var args = new List<string>
        {
            "exec",
            "--runtimeconfig", Path.Combine(projDir, "EAxWiki.runtimeconfig.json"),
            "--depsfile", Path.Combine(projDir, "EAxWiki.deps.json"),
            Path.Combine(projDir, "EAxWiki.dll"),
            "--api", "--api-port", options.ApiPort.ToString(),
            "--wiki-port", options.WikiPort.ToString(),
            "--output", options.WikiDir,
        };
        if (!string.IsNullOrEmpty(options.RepoPath))
        {
            args.Add("--repo");
            args.Add(options.RepoPath);
        }

        return new ServiceSpec(
            "api",
            Path.Combine(stateDir, "api.pid"),
            "dotnet",
            args,
            Path.Combine(stateDir, "logs"),
            Port: options.ApiPort,
            ReadyFile: Path.Combine(options.WikiDir, "status", "api-ready"),
            ClearPortBeforeStart: true,
            WorkingDirectory: repoRoot,
            ReadyTimeoutSeconds: 120);
    }

    public static ServiceSpec? BuildLlmSpec(MonitorOptions options, string stateDir)
    {
        if (options.AiMode != "local") return null;
        if (string.IsNullOrEmpty(options.LlamaExePath) || string.IsNullOrEmpty(options.LlamaModelPath)) return null;
        if (!File.Exists(options.LlamaExePath) || !File.Exists(options.LlamaModelPath)) return null;

        return new ServiceSpec(
            "llm",
            Path.Combine(stateDir, "llm.pid"),
            options.LlamaExePath,
            new[] { "-m", options.LlamaModelPath, "-c", "4096", "--port", options.LlmPort.ToString(), "--n-gpu-layers", "0" },
            Path.Combine(stateDir, "logs"),
            Port: options.LlmPort,
            ClearPortBeforeStart: true,
            PostStartDelaySeconds: 5);
    }

    private static string ResolveRepoRoot(string stateDir) =>
        MonitorPaths.FindRepoRoot(stateDir);
}