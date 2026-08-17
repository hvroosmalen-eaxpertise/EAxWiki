namespace EAxWiki.Core.Monitoring;

/// <summary>
/// Monitor health/state persisted as <c>.eaxwiki-monitor/&lt;hash&gt;/health.json</c>.
/// Field names serialize camelCase via <c>JsonSerializerDefaults.Web</c> — identical to the
/// PowerShell monitor's JSON (lastSuccessTime, skipExport, ...). The SchedulerUI reads this
/// file read-only for its health dashboard.
/// </summary>
public class HealthState
{
    public DateTimeOffset? LastSuccessTime { get; set; }
    public DateTimeOffset? LastFailureTime { get; set; }
    public int ConsecutiveFailures { get; set; }

    public int? LastExitCode { get; set; }
    public int? LastElementCount { get; set; }
    public int? LastDiagramCount { get; set; }

    public int ServeConsecutiveFailures { get; set; }
    public DateTimeOffset? LastServeFailureTime { get; set; }
    public DateTimeOffset? LastServeSuccessTime { get; set; }

    public int LlmConsecutiveFailures { get; set; }
    public DateTimeOffset? LastLlmFailureTime { get; set; }
    public DateTimeOffset? LastLlmSuccessTime { get; set; }

    public int ApiConsecutiveFailures { get; set; }
    public DateTimeOffset? LastApiFailureTime { get; set; }
    public DateTimeOffset? LastApiSuccessTime { get; set; }

    // Tracks the ApiPort used during the last export; the SchedulerUI reads it to show
    // whether the write-back API was enabled for the last run.
    public int LastApiPort { get; set; }

    public int RunsSinceForce { get; set; }
    public string? LastMode { get; set; }

    public int PageReadsToday { get; set; }
    public int WritebacksToday { get; set; }
    public string? LastDigestDate { get; set; }
    public string? PageReadLogFile { get; set; }
    public long PageReadLogOffset { get; set; }
    public string? WritebackLogFile { get; set; }
    public long WritebackLogOffset { get; set; }

    public bool SkipExport { get; set; }
    public bool SkipServe { get; set; }
}
