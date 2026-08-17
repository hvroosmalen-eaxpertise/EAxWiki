using EAxWiki.Core.Monitoring;

namespace EAxWiki.SchedulerUI;

public record ServiceSnapshot(
    string Name,
    bool Running,
    bool NotConfigured,
    string LastSuccess,
    string LastFailure,
    int ConsecutiveFailures);

public record DashboardSnapshot(
    string InstanceLabel,
    IReadOnlyList<ServiceSnapshot> Services);

/// <summary>
/// Read-only health dashboard source: reads .eaxwiki-monitor/&lt;hash&gt;/health.json plus the
/// serve/api/llm pid files (pure file reads + Process.GetProcessById — no HTTP surface).
/// The Export row always shows; Serve/API/LLM derive Running from their pid files.
/// </summary>
public class HealthDashboardReader
{
    private static readonly HealthStore Store = new();

    public DashboardSnapshot ReadAll(string repoRoot)
    {
        var monitorDir = Path.Combine(repoRoot, ".eaxwiki-monitor");
        var services = new List<ServiceSnapshot>();

        var healthPath = Directory.Exists(monitorDir)
            ? Directory.GetFiles(monitorDir, "health.json", SearchOption.AllDirectories).FirstOrDefault()
            : null;

        HealthState? state = null;
        string? stateDir = null;
        string? instanceLabel = null;
        if (healthPath != null)
        {
            state = Store.Load(healthPath);
            stateDir = Path.GetDirectoryName(healthPath);
            instanceLabel = $"{Environment.MachineName} - {Path.GetDirectoryName(Path.GetDirectoryName(stateDir))}";
        }

        // A monitor that hasn't produced a health.json yet may still have left pid files behind;
        // resolve the state dir from those so Running detection doesn't depend on health.json.
        if (stateDir == null && Directory.Exists(monitorDir))
        {
            stateDir = Directory.GetDirectories(monitorDir)
                .FirstOrDefault(d => File.Exists(Path.Combine(d, "serve.pid"))
                    || File.Exists(Path.Combine(d, "api.pid"))
                    || File.Exists(Path.Combine(d, "llm.pid")));
        }

        services.Add(new ServiceSnapshot(
            "Export",
            Running: false,
            NotConfigured: false,
            state?.LastSuccessTime?.ToString("O") ?? "-",
            state?.LastFailureTime?.ToString("O") ?? "-",
            state?.ConsecutiveFailures ?? 0));

        services.Add(new ServiceSnapshot(
            "Serve",
            Running: stateDir != null && PidFile.IsAlive(Path.Combine(stateDir!, "serve.pid")),
            NotConfigured: false,
            state?.LastServeSuccessTime?.ToString("O") ?? "-",
            state?.LastServeFailureTime?.ToString("O") ?? "-",
            state?.ServeConsecutiveFailures ?? 0));

        services.Add(new ServiceSnapshot(
            "API",
            Running: stateDir != null && PidFile.IsAlive(Path.Combine(stateDir!, "api.pid")),
            NotConfigured: (state?.LastApiPort ?? 0) == 0,
            state?.LastApiSuccessTime?.ToString("O") ?? "-",
            state?.LastApiFailureTime?.ToString("O") ?? "-",
            state?.ApiConsecutiveFailures ?? 0));

        services.Add(new ServiceSnapshot(
            "LLM",
            Running: stateDir != null && PidFile.IsAlive(Path.Combine(stateDir!, "llm.pid")),
            NotConfigured: stateDir == null || !File.Exists(Path.Combine(stateDir!, "llm.pid")),
            state?.LastLlmSuccessTime?.ToString("O") ?? "-",
            state?.LastLlmFailureTime?.ToString("O") ?? "-",
            state?.LlmConsecutiveFailures ?? 0));

        return new DashboardSnapshot(instanceLabel ?? "-", services);
    }
}
