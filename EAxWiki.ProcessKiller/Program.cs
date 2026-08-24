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
    var info = PidFile.Read(pidFile);
    if (info == null)
    {
        Console.WriteLine($"  SKIP: {Path.GetFileName(pidFile)} - could not read PID file.");
        continue;
    }

    try
    {
        using var proc = Process.GetProcessById(info.Pid);
        proc.Kill(entireProcessTree: true);
        Console.WriteLine($"  Killed PID {info.Pid} ({Path.GetFileName(pidFile)})");
        killed++;
    }
    catch (ArgumentException)
    {
        Console.WriteLine($"  NOT RUNNING: PID {info.Pid} ({Path.GetFileName(pidFile)}) - process no longer exists.");
        notRunning++;
    }
    catch (System.ComponentModel.Win32Exception)
    {
        Console.WriteLine($"  NOT ACCESSIBLE: PID {info.Pid} ({Path.GetFileName(pidFile)}) - process inaccessible (different session/elevation).");
        notRunning++;
    }
}

Console.WriteLine($"\nFinished: killed={killed}, not running or inaccessible={notRunning}");
return 0;