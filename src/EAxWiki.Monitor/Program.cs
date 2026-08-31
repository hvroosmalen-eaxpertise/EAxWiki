using System.CommandLine;
using EAxWiki.Core.Configuration;
using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (!OperatingSystem.IsWindows())
        {
            Console.Error.WriteLine("Monitoring requires Sparx Enterprise Architect, which is only available on Windows.");
            return 1;
        }

        var root = MonitorCommandLine.BuildCommand();
        var parseResult = root.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            await parseResult.InvokeAsync(new InvocationConfiguration(), CancellationToken.None);
            return 1;
        }
        var cli = MonitorCommandLine.ToOptions(parseResult);

        var repoRoot = MonitorPaths.FindRepoRoot(AppContext.BaseDirectory);

        LocalConfigStore.Config? config = null;
        var eaxwikiPath = Path.Combine(repoRoot, ".eaxwiki");
        if (File.Exists(eaxwikiPath))
        {
            try { config = LocalConfigStore.Load(eaxwikiPath, out _); }
            catch { /* legacy/undecryptable — resolve with null config */ }
        }

        var options = MonitorOptionsResolver.Resolve(cli, repoRoot, Environment.GetEnvironmentVariable, config);
        var stateDir = MonitorPaths.StateDir(repoRoot, options.WikiDir);
        Directory.CreateDirectory(Path.Combine(stateDir, "logs"));

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole(options => options.TimestampFormat = "HH:mm:ss.fff ");
            builder.AddProvider(new MonitorFileLoggerProvider(stateDir));
        });
        var logger = loggerFactory.CreateLogger("monitor");

        var monitorPidPath = Path.Combine(stateDir, "monitor.pid");
        if (!MonitorLock.TryAcquire(monitorPidPath, out _))
        {
            logger.LogInformation("Duplicate monitor detected; exiting.");
            return 0;
        }
        try
        {
            logger.LogInformation("Repo: {Repo}", Redact(options.RepoPath));
            logger.LogInformation("ApiPort={ApiPort} WikiPort={WikiPort} AiEndpoint={AiEndpoint} LlamaExePath={LlamaExePath}",
                options.ApiPort, options.WikiPort, options.AiEndpoint, options.LlamaExePath);

            var alerts = new AlertDispatcher(
                new AlertOptions(options.WebhookUrl, options.TeamsWebhookUrl,
                    options.TelegramBotToken, options.TelegramChatId,
                    $"{Environment.MachineName} - {options.WikiDir}"),
                null,
                loggerFactory.CreateLogger("Alert"));

            if (options.TestAlert)
            {
                alerts.Dispatch("Test alert from EAxWiki.Monitor - if you can see this in Slack/Teams/Telegram, alerting is wired correctly.", AlertKind.Test);
                return 0;
            }

            var healthStore = new HealthStore();
            var state = healthStore.Load(Path.Combine(stateDir, "health.json"));
            var loop = MonitorApp.Build(options, state, healthStore, stateDir, loggerFactory);
            await loop.RunAsync(CancellationToken.None);
        }
        finally
        {
            MonitorLock.Release(monitorPidPath);
        }
        return 0;
    }

    private static string Redact(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (!value.Contains('=')) return value;
        return System.Text.RegularExpressions.Regex.Replace(value, "(?i)(Password|Pwd)\\s*=[^;]*", "$1=***");
    }
}