using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class MonitorLoopTests
{
    private sealed class StubExportRunner : IExportRunner
    {
        public int Runs;
        public bool ShouldForce(int runsSinceForce) => false;
        public Task<bool> RunExportAsync(bool effectiveForce, WritebackDelta writebacks, CancellationToken ct)
        {
            Runs++;
            return Task.FromResult(true);
        }
    }

    private sealed class StubDigest : IDigestTracker
    {
        public int PageReads;
        public int CountNewPageReads() => PageReads;
        public WritebackDelta CountNewWritebacks() => new(0, new Dictionary<string, int>());
        public string? MaybeComposeDailyDigest(DateTime now) => null;
    }

    private sealed class StubAlerts : IAlertDispatcher
    {
        public readonly List<(AlertKind Kind, string Message)> Sent = new();
        public void Dispatch(string message, AlertKind kind) => Sent.Add((kind, message));
    }

    private sealed class StubSupervisor : IProcessSupervisor
    {
        public int AttemptsUsed { get; set; }
        public int StartCount;
        public bool IsAlive(ServiceSpec spec) => false;
        public Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct)
        {
            StartCount++;
            return Task.FromResult(true);
        }
    }

    private sealed class FakeHealthStore : HealthStore
    {
        public int Saves;
        public override void Save(string path, HealthState state) => Saves++;
    }

    private static MonitorLoop Build(
        out StubExportRunner exportRunner, out StubDigest digest, out StubAlerts alerts,
        out StubSupervisor supervisor, out HealthState state, out FakeHealthStore store,
        string? wikiDir = null, int checkInterval = 0, bool local = false)
    {
        var dir = Path.Combine(Path.GetTempPath(), "eaxwiki_loop_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        if (local)
        {
            // The loop only starts the LLM watchdog when both llama paths exist.
            File.WriteAllText(Path.Combine(dir, "llama-server.exe"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "model.gguf"), string.Empty);
        }
        var options = new MonitorOptions
        {
            RepoPath = @"C:\models\repo.qea",
            WikiDir = wikiDir ?? Path.Combine(dir, "wiki"),
            ApiPort = 8001,
            ExportIntervalMinutes = 30,
            CheckIntervalSeconds = checkInterval,
            MaxRetries = 1,
            RetryDelaySeconds = 0,
            AiMode = local ? "local" : "none",
            LlamaExePath = Path.Combine(dir, "llama-server.exe"),
            LlamaModelPath = Path.Combine(dir, "model.gguf"),
            NotifyOnStart = true,
        };
        exportRunner = new StubExportRunner();
        digest = new StubDigest();
        alerts = new StubAlerts();
        supervisor = new StubSupervisor();
        state = new HealthState();
        store = new FakeHealthStore();

        var loop = new MonitorLoop(
            options, state, store,
            new HealthPageRenderer(Path.Combine(dir, "health-template.md"), options.WikiDir),
            dir,
            exportRunner, digest, alerts, supervisor,
            new ServiceSpec("serve", Path.Combine(dir, "serve.pid"), "cmd.exe", Array.Empty<string>(), dir),
            new ServiceSpec("api", Path.Combine(dir, "api.pid"), "cmd.exe", Array.Empty<string>(), dir),
            new ServiceSpec("llm", Path.Combine(dir, "llm.pid"), "cmd.exe", Array.Empty<string>(), dir),
            NullLogger.Instance);
        return loop;
    }

    [Fact]
    public void RunOnce_FirstCycle_ExportsAndStartsServices()
    {
        var loop = Build(out var exportRunner, out _, out var alerts, out var supervisor, out var state, out _);
        loop.RunOnce();

        Assert.Equal(1, exportRunner.Runs);
        Assert.True(supervisor.StartCount >= 2); // serve + api (llm skipped: AiMode=none)
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.Start);
        Assert.False(state.SkipExport);
        Assert.False(state.SkipServe);
    }

    [Fact]
    public void RunOnce_SecondCycleWithinInterval_DoesNotExport()
    {
        var loop = Build(out var exportRunner, out _, out _, out _, out _, out _);
        loop.RunOnce(); // first: export
        loop.RunOnce(); // second: within 30-min interval → no export

        Assert.Equal(1, exportRunner.Runs);
    }

    [Fact]
    public void RunOnce_SkipExport_SetByStop_AlertsUserStop()
    {
        var loop = Build(out var exportRunner, out _, out var alerts, out _, out var state, out _);
        state.SkipExport = true;

        loop.RunOnce();

        Assert.Equal(0, exportRunner.Runs);
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.UserStop);
    }

    [Fact]
    public void RunOnce_LocalMode_StartsLlm()
    {
        var loop = Build(out _, out _, out _, out var supervisor, out _, out _, local: true);
        loop.RunOnce();

        Assert.True(supervisor.StartCount >= 3); // serve + api + llm
    }

    [Fact]
    public void RunOnce_SaveHealth_Invoked()
    {
        var loop = Build(out _, out _, out _, out _, out _, out var store);
        loop.RunOnce();
        // At least the mid-cycle save; watchdogs that (re)start a service also re-render + re-save.
        Assert.True(store.Saves >= 1);
    }
}
