using System.Diagnostics;
using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public sealed record ServiceSpec(
    string Name,
    string PidFilePath,
    string Executable,
    IReadOnlyList<string> Arguments,
    string LogDir,
    int? Port = null,
    string? ReadyFile = null,
    bool PortProbeFallback = false,
    bool ClearPortBeforeStart = false,
    string? WorkingDirectory = null,
    int ReadyTimeoutSeconds = 120,
    int PostStartDelaySeconds = 5,
    // Issue #93: when the executable is a wrapper (e.g. serve.ps1 running
    // mkdocs), record the leaf process's PID in the pid file instead of the
    // launcher's, so external tooling that reads the pid file can actually
    // kill the leaf. Time-based match: any process with this name started
    // at-or-after the launcher counts as the leaf.
    string? LeafProcessName = null,
    int LeafDiscoveryTimeoutSeconds = 60);

public interface IProcessSupervisor
{
    int AttemptsUsed { get; }
    bool IsAlive(ServiceSpec spec);
    Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct);
}

/// <summary>
/// Generic child-process watchdog (serve.ps1, llama-server, EAxWiki --api). Alive = pid-file
/// alive (with port-probe fallback for serve). Start = optional Clear-Port, optional stale
/// ready-file removal, redirected output logs, optional ready-file poll, pid file written on
/// success, retry/backoff. Recovery/give-up alerts are the MonitorLoop's job (PS parity).
/// </summary>
public class ProcessSupervisor : IProcessSupervisor
{
    private readonly ILogger _logger;
    private readonly IPortProbe _probe;
    private readonly IPortKiller _killer;

    public ProcessSupervisor(ILogger logger, IPortProbe probe, IPortKiller killer)
    {
        _logger = logger;
        _probe = probe;
        _killer = killer;
    }

    public int AttemptsUsed { get; private set; }

    public bool IsAlive(ServiceSpec spec)
    {
        if (PidFile.IsAlive(spec.PidFilePath)) return true;
        if (spec.PortProbeFallback && spec.Port is { } port && _probe.IsListening(port)) return true;
        return false;
    }

    public async Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct)
    {
        AttemptsUsed = 0;
        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            AttemptsUsed = attempt;
            ct.ThrowIfCancellationRequested();
            _logger.LogInformation("Start attempt {Attempt}/{MaxRetries} for {Name}.", attempt, maxRetries, spec.Name);

            try
            {
                if (spec.ClearPortBeforeStart && spec.Port is { } clearPort)
                    _killer.KillPortOwner(clearPort);

                if (spec.ReadyFile is { } ready && File.Exists(ready))
                    File.Delete(ready);

                var started = StartAndWait(spec);
                if (started is not null)
                {
                    PidFile.Write(spec.PidFilePath, started.Id, started.StartTime.ToUniversalTime());
                    _logger.LogInformation("{Name} started (PID {Pid}).", spec.Name, started.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Start attempt {Attempt} for {Name} failed: {Error}", attempt, spec.Name, ex.Message);
            }

            if (attempt < maxRetries)
            {
                var delay = retryDelaySeconds * attempt;
                _logger.LogInformation("Retrying {Name} start in {Delay} seconds.", spec.Name, delay);
                await Task.Delay(TimeSpan.FromSeconds(delay), ct);
            }
        }
        return false;
    }

    private Process? StartAndWait(ServiceSpec spec)
    {
        var stamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
        var outFile = Path.Combine(spec.LogDir, $"{spec.Name}-{stamp}.out.log");
        var errFile = Path.Combine(spec.LogDir, $"{spec.Name}-{stamp}.err.log");

        var psi = new ProcessStartInfo
        {
            FileName = spec.Executable,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        foreach (var arg in spec.Arguments) psi.ArgumentList.Add(arg);
        if (!string.IsNullOrEmpty(spec.WorkingDirectory)) psi.WorkingDirectory = spec.WorkingDirectory;

        var proc = Process.Start(psi);
        if (proc == null) return null;

        // Drain output to the per-run log files in the background so a full pipe buffer can't
        // stall the child.
        _ = Task.Run(async () =>
        {
            try
            {
                var outTask = proc.StandardOutput.ReadToEndAsync();
                var errTask = proc.StandardError.ReadToEndAsync();
                await Task.WhenAll(outTask, errTask);
                var outText = await outTask;
                var errText = await errTask;
                Directory.CreateDirectory(spec.LogDir);
                File.WriteAllText(outFile, outText);
                File.WriteAllText(errFile, errText);
            }
            catch { /* child already gone; ignore */ }
        });

        Process? ready;
        if (spec.ReadyFile is { } readyFile)
        {
            var deadline = DateTime.UtcNow.AddSeconds(spec.ReadyTimeoutSeconds);
            while (DateTime.UtcNow < deadline && !File.Exists(readyFile))
            {
                if (proc.HasExited) break;
                Thread.Sleep(1000);
            }
            ready = File.Exists(readyFile) ? proc : null;
        }
        else
        {
            Thread.Sleep(TimeSpan.FromSeconds(spec.PostStartDelaySeconds));
            ready = proc.HasExited ? null : proc;
        }

        if (ready == null) return null;

        if (spec.LeafProcessName is { } leafName)
        {
            var leaf = WaitForLeaf(leafName, ready, spec.LeafDiscoveryTimeoutSeconds);
            if (leaf != null)
            {
                _logger.LogInformation(
                    "{Name}: recording leaf process {LeafName} PID {LeafPid} instead of launcher PID {LauncherPid}.",
                    spec.Name, leafName, leaf.Id, ready.Id);
                return leaf;
            }
            _logger.LogWarning(
                "{Name}: leaf process {LeafName} not observed within {Timeout}s; falling back to launcher PID {LauncherPid}.",
                spec.Name, leafName, spec.LeafDiscoveryTimeoutSeconds, ready.Id);
        }

        return ready;
    }

    private static Process? WaitForLeaf(string leafName, Process launcher, int timeoutSeconds)
    {
        DateTime launcherStart;
        try { launcherStart = launcher.StartTime; }
        catch { return null; }

        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTime.UtcNow < deadline)
        {
            var leaf = Process.GetProcessesByName(leafName)
                .Where(p =>
                {
                    try { return p.StartTime >= launcherStart.AddSeconds(-1); }
                    catch { return false; }
                })
                .OrderByDescending(p =>
                {
                    try { return p.StartTime; }
                    catch { return DateTime.MinValue; }
                })
                .FirstOrDefault();
            if (leaf != null) return leaf;
            if (launcher.HasExited) return null;
            Thread.Sleep(500);
        }
        return null;
    }
}