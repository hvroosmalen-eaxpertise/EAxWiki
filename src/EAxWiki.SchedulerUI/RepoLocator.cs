namespace EAxWiki.SchedulerUI;

/// <summary>
/// Locates the EAxWiki repo root relative to wherever this GUI's exe happens to be built/installed,
/// by walking up from the exe's own directory looking for scripts/register-scheduled-task.ps1 —
/// the same "search upward for a marker" approach Program.cs (the console app) uses to find .eaxwiki,
/// rather than a hardcoded relative path count that would break if the build output layout changes.
/// </summary>
internal static class RepoLocator
{
    public static string? FindRepoRoot() => FindRepoRoot(null);

    internal static string? FindRepoRoot(string? startDir)
    {
        var dir = new DirectoryInfo(startDir ?? AppContext.BaseDirectory);
        for (var i = 0; i < 10 && dir != null; i++)
        {
            var candidate = Path.Combine(dir.FullName, "scripts", "register-scheduled-task.ps1");
            if (File.Exists(candidate))
                return dir.FullName;
            dir = dir.Parent;
        }
        return null;
    }
}
