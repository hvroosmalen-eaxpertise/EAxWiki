using EAxWiki.Core.Monitoring;
using System.Diagnostics;
using System.Text.Json;

string? repoRoot = args.Length > 0 ? args[0] : null;

if (repoRoot == null)
{
    Console.Error.WriteLine("Usage: EAxWiki.ProcessKiller <repo-root>");
    Console.Error.WriteLine("  Kills all monitor-started processes (serve, API, LLM) under the given repo root.");
    return 1;
}

var pidDir = Path.Combine(repoRoot, ".eaxwiki-monitor");
if (!Directory.Exists(pidDir))
{
    Console.WriteLine("No .eaxwiki-monitor directory found under repo root.");
    Console.WriteLine("Processes may not have been started via the monitor, or the repo root is incorrect.");
    return 0;
}

var pidFiles = Directory.GetFiles(pidDir, "*.pid", SearchOption.AllDirectories);
if (pidFiles.Length == 0)
{
    Console.WriteLine("No .pid files found in .eaxwiki-monitor directory.");
    Console.WriteLine("No monitor-started processes to kill.");
    return 0;
}

Console.WriteLine($"Found {pidFiles.Length} PID file(s) in {pidDir}.");
Console.WriteLine("Attempting to kill running processes...");

int killed = 0;
int notRunning = 0;

foreach (var pidFile in pidFiles)
{
    int pid;
    var info = PidFile.Read(pidFile);
    if (info != null)
    {
        pid = info.Pid;
    }
    else if (string.Equals(Path.GetFileName(pidFile), "monitor.pid", StringComparison.OrdinalIgnoreCase)
             && int.TryParse(File.ReadAllText(pidFile).Split('\n', '\r').FirstOrDefault()?.Trim(), out var plainPid))
    {
        // monitor.pid is plain PID text (see MonitorLock), not JSON.
        pid = plainPid;
    }
    else
    {
        Console.WriteLine($"  SKIP: {Path.GetFileName(pidFile)} - could not read PID file.");
        continue;
    }

    if (pid == Environment.ProcessId)
    {
        Console.WriteLine($"  SKIPPED: PID {pid} ({Path.GetFileName(pidFile)}) - would kill this process; skipping.");
        continue;
    }

    bool deleteStale = false;
    try
    {
        using var proc = Process.GetProcessById(pid);
        try
        {
            proc.Kill(entireProcessTree: true);
        }
        catch (InvalidOperationException)
        {
            // "Cannot be used to terminate a process tree containing the calling process."
            // Happens when the target is our ancestor (e.g. the shell that launched dotnet).
            // Do NOT retry with Kill() — killing our ancestor would take us with it. Skip.
            Console.WriteLine($"  SKIPPED: PID {pid} ({Path.GetFileName(pidFile)}) - ancestor of this process; skipping.");
            continue;
        }
        Console.WriteLine($"  Killed PID {pid} ({Path.GetFileName(pidFile)})");
        killed++;
        deleteStale = true;
    }
    catch (ArgumentException)
    {
        Console.WriteLine($"  NOT RUNNING: PID {pid} ({Path.GetFileName(pidFile)}) - process no longer exists.");
        notRunning++;
        deleteStale = true;
    }
    catch (System.ComponentModel.Win32Exception)
    {
        Console.WriteLine($"  NOT ACCESSIBLE: PID {pid} ({Path.GetFileName(pidFile)}) - process inaccessible (different session/elevation).");
        notRunning++;
        // Do NOT delete: the process may still be running under another session/elevation.
    }

    if (deleteStale)
    {
        try { File.Delete(pidFile); }
        catch (IOException) { /* best-effort cleanup */ }
        catch (UnauthorizedAccessException) { /* best-effort cleanup */ }
    }
}

Console.WriteLine($"\nFinished: killed={killed}, not running or inaccessible={notRunning}");
return 0;