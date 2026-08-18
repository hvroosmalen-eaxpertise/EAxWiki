using System.Diagnostics;

namespace EAxWiki.Monitor;

/// <summary>
/// Duplicate-instance guard. monitor.pid is plain PID text (unlike the JSON serve/api/llm pid
/// files). TryAcquire returns false when an existing live PID that isn't this process holds the
/// file (the monitor then exits 0); a stale/dead pid file is replaced.
/// </summary>
public static class MonitorLock
{
    public static bool TryAcquire(string monitorPidPath, out int pid)
    {
        pid = Environment.ProcessId;
        if (File.Exists(monitorPidPath))
        {
            var existing = File.ReadAllText(monitorPidPath).Trim();
            if (int.TryParse(existing, out var existingPid) && existingPid != pid && IsAlive(existingPid))
                return false;
            File.Delete(monitorPidPath);
        }
        Directory.CreateDirectory(Path.GetDirectoryName(monitorPidPath)!);
        File.WriteAllText(monitorPidPath, pid.ToString());
        return true;
    }

    public static void Release(string monitorPidPath)
    {
        if (File.Exists(monitorPidPath))
            File.Delete(monitorPidPath);
    }

    private static bool IsAlive(int pid)
    {
        try
        {
            using var proc = Process.GetProcessById(pid);
            return !proc.HasExited;
        }
        catch (ArgumentException)
        {
            return false; // no process with that id
        }
        catch (System.ComponentModel.Win32Exception)
        {
            return false; // process inaccessible (another session/elevation) - treat as not live
        }
    }
}
