using System.Diagnostics;
using System.Text;

namespace EAxWiki.Monitor;

public interface IScheduledTaskSnapshot
{
    ScheduledTaskInfo? Get();
}

/// <summary>
/// Queries the registered Task Scheduler task whose action -Execute matches the monitor's own
/// exe (Environment.ProcessPath), via pwsh Get-ScheduledTask serialized to JSON. Results are
/// cached (default 5 min) so the 30 s monitor loop doesn't shell out every cycle. Any query or
/// parse failure surfaces as null ("schedule unavailable"), never an exception.
/// </summary>
public sealed class ScheduledTaskSnapshot : IScheduledTaskSnapshot
{
    private readonly Func<string?> _queryJson;
    private readonly TimeSpan _cacheTtl;
    private ScheduledTaskInfo? _cached;
    private DateTime _cachedAt = DateTime.MinValue;

    public ScheduledTaskSnapshot(Func<string?>? queryJson = null, TimeSpan? cacheTtl = null)
    {
        _queryJson = queryJson ?? RunPwshQuery;
        _cacheTtl = cacheTtl ?? TimeSpan.FromMinutes(5);
    }

    public ScheduledTaskInfo? Get()
    {
        if (_cached != null && DateTime.Now - _cachedAt < _cacheTtl)
            return _cached;
        _cached = ScheduledTaskJsonParser.Parse(_queryJson());
        _cachedAt = DateTime.Now;
        return _cached;
    }

    private static string? RunPwshQuery()
    {
        var exePath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exePath)) return null;

        // $$""" lets single braces stay literal PowerShell syntax while {{ }} interpolates C#.
        // $ProgressPreference = 'SilentlyContinue' matches PowerShellRunner.cs in SchedulerUI:
        // the ScheduledTasks cmdlets emit progress records that get serialized as CLIXML onto
        // the error stream when no interactive host is attached, which would corrupt the JSON.
        var script = $$"""
            $ProgressPreference = 'SilentlyContinue'
            $exe = '{{exePath.Replace("'", "''")}}'
            $t = Get-ScheduledTask | Where-Object { $_.Actions | Where-Object { $_.Execute -eq $exe } } | Select-Object -First 1
            if (-not $t) { Write-Output 'null'; exit }
            $triggers = foreach ($tr in $t.Triggers) {
                [pscustomobject]@{
                    Kind = $tr.CimClass.CimClassName
                    StartBoundary = $tr.StartBoundary
                    RepetitionInterval = $tr.Repetition.Interval
                    RepetitionDuration = $tr.Repetition.Duration
                    DaysInterval = $tr.DaysInterval
                    DaysOfWeek = $tr.DaysOfWeek
                }
            }
            [pscustomobject]@{
                TaskName = $t.TaskName
                State = $t.State
                WakeToRun = $t.Settings.WakeToRun
                ExecutionTimeLimit = $t.Settings.ExecutionTimeLimit
                MultipleInstances = $t.Settings.MultipleInstances
                Triggers = @($triggers)
            } | ConvertTo-Json -Depth 6
            """;

        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(script));
        var psi = new ProcessStartInfo
        {
            FileName = MonitorPaths.FindPowerShell(),
            Arguments = $"-NoProfile -EncodedCommand {encoded}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var process = Process.Start(psi);
        if (process == null) return null;
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit(30_000);
        return string.IsNullOrWhiteSpace(output) ? null : output.Trim();
    }
}