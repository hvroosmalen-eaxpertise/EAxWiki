using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class ProcessSupervisorTests : IDisposable
{
    private readonly string _dir;

    public ProcessSupervisorTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_super_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private string LogDir()
    {
        var dir = Path.Combine(_dir, "logs");
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static ServiceSpec Pinger(string name, string pidPath)
    {
        return new ServiceSpec(name, pidPath, "cmd.exe",
            new[] { "/c", "ping -n 30 127.0.0.1 >nul" }, Path.GetDirectoryName(pidPath)!,
            PostStartDelaySeconds: 0);
    }

    [Fact]
    public async Task EnsureRunning_StartsLongLivedChild_ReturnsTrue_WritesPid()
    {
        var pidPath = Path.Combine(_dir, "pinger.pid");
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());
        var spec = Pinger("pinger", pidPath);

        var ok = await supervisor.EnsureRunningAsync(spec, maxRetries: 1, retryDelaySeconds: 0, CancellationToken.None);

        Assert.True(ok);
        Assert.Equal(1, supervisor.AttemptsUsed);
        var info = PidFile.Read(pidPath);
        Assert.NotNull(info);
        Assert.True(PidFile.IsAlive(pidPath));
    }

    [Fact]
    public async Task EnsureRunning_ExeNotFound_FailsAfterRetries()
    {
        var pidPath = Path.Combine(_dir, "missing.pid");
        var spec = new ServiceSpec("missing", pidPath, @"Z:\does-not-exist.exe",
            Array.Empty<string>(), LogDir(), PostStartDelaySeconds: 0);
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

        var ok = await supervisor.EnsureRunningAsync(spec, maxRetries: 2, retryDelaySeconds: 0, CancellationToken.None);

        Assert.False(ok);
        Assert.Equal(2, supervisor.AttemptsUsed);
    }

    [Fact]
    public async Task EnsureRunning_ReadyFile_WaitsForIt()
    {
        var pidPath = Path.Combine(_dir, "ready.pid");
        var readyFile = Path.Combine(_dir, "status", "api-ready");
        Directory.CreateDirectory(Path.GetDirectoryName(readyFile)!);
        var spec = new ServiceSpec("ready", pidPath, "cmd.exe",
            new[] { "/c", $"echo ready > {readyFile}" }, LogDir(),
            ReadyFile: readyFile, ReadyTimeoutSeconds: 15, PostStartDelaySeconds: 0);
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

        var ok = await supervisor.EnsureRunningAsync(spec, maxRetries: 1, retryDelaySeconds: 0, CancellationToken.None);

        Assert.True(ok);
        Assert.True(File.Exists(readyFile));
    }

    [Fact]
    public void IsAlive_UntrackedListeningPort_TrueWithPortProbeFallback()
    {
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            var spec = new ServiceSpec("serve", Path.Combine(_dir, "serve.pid"), "cmd.exe",
                Array.Empty<string>(), LogDir(), Port: port, PortProbeFallback: true);
            var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

            Assert.True(supervisor.IsAlive(spec));
        }
        finally { listener.Stop(); }
    }

    [Fact]
    public void IsAlive_NoPidNoProbe_ReturnsFalse()
    {
        var spec = new ServiceSpec("serve", Path.Combine(_dir, "serve.pid"), "cmd.exe",
            Array.Empty<string>(), LogDir());
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

        Assert.False(supervisor.IsAlive(spec));
    }
}