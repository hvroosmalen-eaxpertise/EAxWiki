using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class PidFileTests : IDisposable
{
    private readonly string _dir;

    public PidFileTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_pid_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void WriteAndRead_RoundTrip()
    {
        var path = Path.Combine(_dir, "serve.pid");
        var start = DateTimeOffset.UtcNow;
        PidFile.Write(path, 1234, start);

        var info = PidFile.Read(path);
        Assert.NotNull(info);
        Assert.Equal(1234, info!.Pid);
        Assert.Equal(start.ToString("O"), info.StartTime.ToString("O"));
    }

    [Fact]
    public void Read_MissingFile_ReturnsNull()
    {
        Assert.Null(PidFile.Read(Path.Combine(_dir, "serve.pid")));
    }

    [Fact]
    public void Read_CorruptFile_ReturnsNull()
    {
        var path = Path.Combine(_dir, "serve.pid");
        File.WriteAllText(path, "not json");
        Assert.Null(PidFile.Read(path));
    }

    [Fact]
    public void IsAlive_LiveShortLivedChild_True()
    {
        // The current process's own PID fails IsAlive because its start time is far older than
        // the 2s window — so spawn a genuinely fresh process (cmd /c ping -n 3 127.0.0.1).
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 3 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        var path = Path.Combine(_dir, "serve.pid");
        PidFile.Write(path, p!.Id, p.StartTime.ToUniversalTime());

        Assert.True(PidFile.IsAlive(path));
        p.Kill();
        p.WaitForExit();
    }

    [Fact]
    public void IsAlive_DeadPid_ReturnsFalse()
    {
        var path = Path.Combine(_dir, "serve.pid");
        PidFile.Write(path, -1, DateTimeOffset.UtcNow); // never a real process
        Assert.False(PidFile.IsAlive(path));
    }

    [Fact]
    public void IsAlive_StaleStartTime_ReturnsFalse()
    {
        // Same PID as a live child, but a start time recorded 5 minutes ago — must read stale.
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 3 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        var path = Path.Combine(_dir, "serve.pid");
        PidFile.Write(path, p!.Id, DateTimeOffset.UtcNow.AddMinutes(-5));

        Assert.False(PidFile.IsAlive(path));
        p.Kill();
        p.WaitForExit();
    }
}