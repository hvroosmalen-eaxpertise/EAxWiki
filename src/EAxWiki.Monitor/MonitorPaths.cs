using EAxWiki.Core.Monitoring;

namespace EAxWiki.Monitor;

/// <summary>
/// Repo-root and state-dir resolution. FindRepoRoot walks up from the executable directory
/// (Task Scheduler actions have no WorkingDirectory) until it finds scripts/register-scheduled-task.ps1
/// or a .git directory. StateDir replicates the PS monitor's per-instance .eaxwiki-monitor/&lt;hash&gt;
/// folder keyed on the wiki dir.
/// </summary>
public static class MonitorPaths
{
    public static string FindRepoRoot(string startDir)
    {
        var dir = new DirectoryInfo(startDir);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "scripts", "register-scheduled-task.ps1")) ||
                Directory.Exists(Path.Combine(dir.FullName, ".git")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Could not locate the EAxWiki repo root.");
    }

    public static string StateDir(string repoRoot, string wikiDir) =>
        Path.Combine(repoRoot, ".eaxwiki-monitor", InstanceHash.Compute(wikiDir));

    public static string FindPowerShell()
    {
        var pshome = Environment.GetEnvironmentVariable("PSHOME");
        if (!string.IsNullOrEmpty(pshome))
            return Path.Combine(pshome, "pwsh");
        return "pwsh";
    }
}
