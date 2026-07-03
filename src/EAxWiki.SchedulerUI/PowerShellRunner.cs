using System.Diagnostics;

namespace EAxWiki.SchedulerUI;

internal record PowerShellResult(int ExitCode, string Output);

/// <summary>
/// Shells out to pwsh.exe rather than reimplementing Task Scheduler registration in C# — keeps
/// register-scheduled-task.ps1 (already built and tested, including the CIM-Repetition workaround
/// for Daily/Weekly triggers) as the single source of truth for how a task actually gets registered.
/// This GUI only ever constructs arguments and reads results; it never touches Task Scheduler
/// directly beyond querying it, and only via the same PowerShell cmdlets used elsewhere.
/// </summary>
internal static class PowerShellRunner
{
    public static async Task<PowerShellResult> RunScriptAsync(string scriptPath, IEnumerable<string> args, string workingDirectory)
    {
        var argLine = string.Join(' ', new[] { "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", Quote(scriptPath) }
            .Concat(args.Select(Quote)));
        return await RunAsync(argLine, workingDirectory);
    }

    public static async Task<PowerShellResult> RunCommandAsync(string command, string workingDirectory)
    {
        var argLine = $"-NoProfile -ExecutionPolicy Bypass -Command {Quote(command)}";
        return await RunAsync(argLine, workingDirectory);
    }

    private static async Task<PowerShellResult> RunAsync(string argLine, string workingDirectory)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pwsh.exe",
            Arguments = argLine,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = psi };
        var output = new System.Text.StringBuilder();
        process.OutputDataReceived += (_, e) => { if (e.Data != null) output.AppendLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data != null) output.AppendLine(e.Data); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        return new PowerShellResult(process.ExitCode, output.ToString());
    }

    private static string Quote(string value) => value.Contains(' ') ? $"\"{value}\"" : value;
}
