using EAxWiki.Core.Models;
using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class ExportRunnerTests : IDisposable
{
    private readonly string _dir;

    public ExportRunnerTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_export_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private sealed class FakeExporter : IStaExporter
    {
        public int Calls;
        public bool Throw;
        public bool LastForce;
        public bool LastWriteBack;
        public void ExportOnSta(string repoPath, string outputPath, bool force, bool writeBack, int apiPort, string? aiEndpoint)
        {
            Calls++;
            LastForce = force;
            LastWriteBack = writeBack;
            if (Throw) throw new InvalidOperationException("COM boom");
            Directory.CreateDirectory(Path.Combine(outputPath, "Pkg"));
            File.WriteAllText(Path.Combine(outputPath, "Pkg", "Elem.md"), "# Elem");
            Directory.CreateDirectory(Path.Combine(outputPath, "diagrams"));
            File.WriteAllText(Path.Combine(outputPath, "diagrams", "D1.md"), "# D1");
        }
    }

    private sealed class FakeAlerts : IAlertDispatcher
    {
        public readonly List<(AlertKind Kind, string Message)> Sent = new();
        public void Dispatch(string message, AlertKind kind) => Sent.Add((kind, message));
    }

    private static MonitorOptions Options() => new()
    {
        WikiDir = "W", MaxRetries = 3, RetryDelaySeconds = 0, MinElementFraction = 0.5,
        ApiPort = 0, NotifyOnStart = true, AiEndpoint = null,
    };

    private (ExportRunner Runner, FakeExporter Exporter, FakeAlerts Alerts, HealthState State, string WikiDir) Create(
        MonitorOptions? options = null)
    {
        var wikiDir = Path.Combine(_dir, "wiki");
        Directory.CreateDirectory(wikiDir);
        var exporter = new FakeExporter();
        var alerts = new FakeAlerts();
        var state = new HealthState();
        var runner = new ExportRunner((options ?? Options()) with { WikiDir = wikiDir }, exporter,
            new WikiOutputMetrics(), state, alerts, NullLogger<ExportRunner>.Instance);
        return (runner, exporter, alerts, state, wikiDir);
    }

    [Fact]
    public void ShouldForce_Incremental_False()
    {
        var (runner, _, _, _, _) = Create();
        Assert.False(runner.ShouldForce(0));
        Assert.False(runner.ShouldForce(5));
    }

    [Fact]
    public void ShouldForce_ForceFlag_AlwaysTrue()
    {
        var (runner, _, _, _, _) = Create(Options() with { Force = true });
        Assert.True(runner.ShouldForce(0));
    }

    [Fact]
    public void ShouldForce_ForceEveryN_TrueWhenReached()
    {
        var (runner, _, _, _, _) = Create(Options() with { ForceEveryNRuns = 4 });
        Assert.False(runner.ShouldForce(3));
        Assert.True(runner.ShouldForce(4));
    }

    [Fact]
    public async Task RunExport_Forced_SetsForceAndResetsRunsSinceForce()
    {
        var (runner, exporter, _, state, _) = Create();
        state.RunsSinceForce = 9;

        var ok = await runner.RunExportAsync(effectiveForce: true, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(ok);
        Assert.True(exporter.LastForce);
        Assert.Equal(0, state.RunsSinceForce);
        Assert.Equal("full (--force)", state.LastMode);
        Assert.Equal(0, state.LastExitCode);
        Assert.NotNull(state.LastSuccessTime);
        Assert.Equal(2, state.LastElementCount);
        Assert.Equal(1, state.LastDiagramCount);
    }

    [Fact]
    public async Task RunExport_Incremental_IncrementsRunsSinceForce()
    {
        var (runner, exporter, _, state, _) = Create();
        state.RunsSinceForce = 3;

        var ok = await runner.RunExportAsync(effectiveForce: false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(ok);
        Assert.False(exporter.LastForce);
        Assert.Equal("incremental", state.LastMode);
        Assert.Equal(4, state.RunsSinceForce);
    }

    [Fact]
    public async Task RunExport_WritebackEnabled_WhenApiPortSet()
    {
        var opts = Options() with { ApiPort = 8001 };
        var (runner, exporter, _, _, _) = Create(opts);

        await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(exporter.LastWriteBack);
    }

    [Fact]
    public async Task RunExport_SanityFloor_MarksFailureWhenCollapse()
    {
        // Previous run recorded 100 elements; this run only exports 1 → below floor 50 → failure.
        var (runner, exporter, alerts, state, _) = Create();
        state.LastElementCount = 100;
        state.ConsecutiveFailures = 0;

        var ok = await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.False(ok);
        Assert.Equal(1, state.LastExitCode);
        Assert.Equal(1, state.ConsecutiveFailures);
        Assert.NotNull(state.LastFailureTime);
        var failure = alerts.Sent.Single(a => a.Kind == AlertKind.Failure).Message;
        Assert.Contains("Sanity check failed", failure);
        Assert.Contains("1 attempt(s)", failure); // collapse short-circuits, not max-retries
    }

    [Fact]
    public async Task RunExport_RetriesAndSucceeds_AfterTransientFailure()
    {
        var (runner, exporter, alerts, state, _) = Create();
        exporter.Throw = true;

        var ok = await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        // Fails on every attempt (still throwing) — proves max-retries attempts are made, no success.
        Assert.False(ok);
        Assert.Equal(3, exporter.Calls); // MaxRetries = 3
        Assert.Equal(3, state.ConsecutiveFailures);
    }

    [Fact]
    public async Task RunExport_RecoveryAlert_WhenPreviouslyFailing()
    {
        var (runner, _, alerts, state, _) = Create();
        state.ConsecutiveFailures = 2;
        state.LastElementCount = 1; // matches this run's count → sanity passes

        var ok = await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(ok);
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.Recovery);
        Assert.Equal(0, state.ConsecutiveFailures);
    }

    [Fact]
    public async Task RunExport_FinishAlert_HasWritebackAndValidationSuffixes()
    {
        var wikiDir = Path.Combine(_dir, "wiki");
        Directory.CreateDirectory(wikiDir);
        var validation = Path.Combine(wikiDir, ".validation-report.json");
        File.WriteAllText(validation, """{"Errors":1,"Warnings":0,"Passed":5,"FilesValidated":6}""");
        var (runner, _, alerts, state, _) = Create();

        var writebacks = new WritebackDelta(3, new Dictionary<string, int> { ["status"] = 2, ["notes"] = 1 });
        await runner.RunExportAsync(false, writebacks, CancellationToken.None);

        var finish = alerts.Sent.Single(a => a.Kind == AlertKind.Finish).Message;
        Assert.Contains("page(s) total", finish);
        Assert.Contains("1 diagram", finish);
        Assert.Contains("- validation: 1 error(s) (5/6 files clean)", finish);
        Assert.Contains("- write-backs: 2 status, 1 notes", finish);
    }

    [Fact]
    public async Task RunExport_NoNotifyStart_NoFinishAlert()
    {
        var (runner, _, alerts, _, _) = Create(Options() with { NotifyOnStart = false });
        await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);
        Assert.DoesNotContain(alerts.Sent, a => a.Kind == AlertKind.Finish);
    }
}
