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
    int PostStartDelaySeconds = 5);

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

        if (spec.ReadyFile is { } ready)
        {
            var deadline = DateTime.UtcNow.AddSeconds(spec.ReadyTimeoutSeconds);
            while (DateTime.UtcNow < deadline && !File.Exists(ready))
            {
                if (proc.HasExited) break;
                Thread.Sleep(1000);
            }
            return File.Exists(ready) ? proc : null;
        }

        Thread.Sleep(TimeSpan.FromSeconds(spec.PostStartDelaySeconds));
        return proc.HasExited ? null : proc;
    }
}