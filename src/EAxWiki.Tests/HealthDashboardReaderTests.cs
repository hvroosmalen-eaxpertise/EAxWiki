using EAxWiki.Core.Monitoring;
using EAxWiki.SchedulerUI;

namespace EAxWiki.Tests;

public class HealthDashboardReaderTests : IDisposable
{
    private readonly string _dir;

    public HealthDashboardReaderTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_dash_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void ReadAll_NoStateDir_ReturnsFourUnconfiguredServices()
    {
        var snapshot = new HealthDashboardReader().ReadAll(_dir);

        Assert.Equal(4, snapshot.Services.Count);
        Assert.All(snapshot.Services, s => Assert.False(s.Running));
    }

    [Fact]
    public void ReadAll_HealthFile_PopulatesExportRow()
    {
        var hash = InstanceHash.Compute(Path.Combine(_dir, "wiki"));
        var stateDir = Path.Combine(_dir, ".eaxwiki-monitor", hash);
        Directory.CreateDirectory(stateDir);
        new HealthStore().Save(Path.Combine(stateDir, "health.json"), new HealthState
        {
            LastSuccessTime = DateTimeOffset.Parse("2026-08-01T10:00:00Z"),
            ConsecutiveFailures = 2,
            LastElementCount = 150,
            LastDiagramCount = 30,
            LastMode = "incremental",
            RunsSinceForce = 5,
            LastApiPort = 8001,
        });

        var snapshot = new HealthDashboardReader().ReadAll(_dir);
        var export = snapshot.Services.Single(s => s.Name == "Export");

        Assert.Contains("2026-08-01T10:00:00", export.LastSuccess);
        Assert.Equal(2, export.ConsecutiveFailures);
    }

    [Fact]
    public void ReadAll_PidFileAlive_ServiceRunning()
    {
        var hash = InstanceHash.Compute(Path.Combine(_dir, "wiki"));
        var stateDir = Path.Combine(_dir, ".eaxwiki-monitor", hash);
        Directory.CreateDirectory(stateDir);

        // Spawn a short-lived child, record its pid, and confirm the dashboard reads it as running.
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 30 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        PidFile.Write(Path.Combine(stateDir, "serve.pid"), p!.Id, p.StartTime.ToUniversalTime());

        var snapshot = new HealthDashboardReader().ReadAll(_dir);
        var serve = snapshot.Services.Single(s => s.Name == "Serve");

        Assert.True(serve.Running);
        p.Kill();
        p.WaitForExit();
    }

    [Fact]
    public void ReadAll_MissingPidFiles_NotRunning()
    {
        var hash = InstanceHash.Compute(Path.Combine(_dir, "wiki"));
        var stateDir = Path.Combine(_dir, ".eaxwiki-monitor", hash);
        Directory.CreateDirectory(stateDir);

        var snapshot = new HealthDashboardReader().ReadAll(_dir);

        Assert.All(snapshot.Services.Where(s => s.Name != "Export"), s => Assert.False(s.Running));
    }
}
