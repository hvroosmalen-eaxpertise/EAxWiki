using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorLockTests : IDisposable
{
    private readonly string _dir;

    public MonitorLockTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_mlock_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void TryAcquire_NoFile_Acquires()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        Assert.True(MonitorLock.TryAcquire(path, out var pid));
        Assert.Equal(Environment.ProcessId, pid);
        Assert.Equal(Environment.ProcessId.ToString(), File.ReadAllText(path).Trim());
    }

    [Fact]
    public void TryAcquire_OwnPidFile_Acquires()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        File.WriteAllText(path, Environment.ProcessId.ToString());
        Assert.True(MonitorLock.TryAcquire(path, out _));
    }

    [Fact]
    public void TryAcquire_LiveForeignPid_Rejects()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        // A live PID that isn't ours: spawn a fresh child.
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 10 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        File.WriteAllText(path, p!.Id.ToString());

        Assert.False(MonitorLock.TryAcquire(path, out _));
        p.Kill();
        p.WaitForExit();
    }

    [Fact]
    public void TryAcquire_DeadPidFile_RemovesAndAcquires()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        File.WriteAllText(path, "-9999");
        Assert.True(MonitorLock.TryAcquire(path, out _));
        Assert.Equal(Environment.ProcessId.ToString(), File.ReadAllText(path).Trim());
    }

    [Fact]
    public void Release_RemovesFile()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        MonitorLock.TryAcquire(path, out _);
        MonitorLock.Release(path);
        Assert.False(File.Exists(path));
    }
}
