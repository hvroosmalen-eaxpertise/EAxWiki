using System.Diagnostics;
using System.IO;

namespace EAxWiki.SchedulerUI;

internal record PowerShellResult(int ExitCode, string Output);

/// <summary>
/// Shells out to pwsh.exe (or powershell.exe as fallback) rather than reimplementing Task Scheduler
/// registration in C# — keeps register-scheduled-task.ps1 (already built and tested, including the
/// CIM-Repetition workaround for Daily/Weekly triggers) as the single source of truth for how a task
/// actually gets registered. This GUI only ever constructs arguments and reads results; it never
/// touches Task Scheduler directly beyond querying it, and only via the same PowerShell cmdlets used
/// elsewhere.
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
        var psExe = FindPowerShellExecutable();
        var isPwsh = Path.GetFileNameWithoutExtension(psExe).Equals("pwsh", StringComparison.OrdinalIgnoreCase);

        if (isPwsh)
        {
            // -EncodedCommand (Base64 UTF-16LE) sidesteps command-line quoting entirely. Wrapping the
            // raw script in one pair of quotes (like Quote() does for individual args below) breaks as
            // soon as the script itself contains an embedded double quote — e.g. the task-status query
            // below builds strings like "$($t.CimClass.CimClassName) at=...", and that literal `"`
            // closes the outer quote early from the OS argument parser's point of view, well before
            // PowerShell ever sees the script. That corrupts the command into unrelated argv entries,
            // which pwsh then fails on silently (exit 1, no output) rather than a parse error we'd see.
            //
            // $ProgressPreference = 'SilentlyContinue' matters too: the ScheduledTasks cmdlets
            // (Get-ScheduledTask, Enable/Disable-ScheduledTask, ...) write progress records, and with no
            // interactive host attached those get serialized as "#< CLIXML" text onto the error stream
            // instead of rendering a progress bar. RunAsync below merges stdout+stderr into one buffer,
            // so that CLIXML block lands right in the middle of the JSON we're about to parse.
            var fullCommand = $"$ProgressPreference = 'SilentlyContinue'; {command}";
            var encoded = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(fullCommand));
            var argLine = $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encoded}";
            return await RunAsync(argLine, workingDirectory);
        }
        else
        {
            // PS 5.1 has no -EncodedCommand — write command to a temp file and use -File
            return await RunCommandViaTempFileAsync(command, workingDirectory);
        }
    }

    private static async Task<PowerShellResult> RunCommandViaTempFileAsync(string command, string workingDirectory)
    {
        var tempFile = Path.GetTempFileName() + ".ps1";
        try
        {
            await File.WriteAllTextAsync(tempFile, $"$ProgressPreference = 'SilentlyContinue'; {command}");
            return await RunScriptAsync(tempFile, [], workingDirectory);
        }
        finally
        {
            try { File.Delete(tempFile); } catch { }
        }
    }

    private static async Task<PowerShellResult> RunAsync(string argLine, string workingDirectory)
    {
        var psi = new ProcessStartInfo
        {
            FileName = FindPowerShellExecutable(),
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

    private static string FindPowerShellExecutable()
    {
        var pwshPath = GetFullPathFromPathEnv("pwsh.exe");
        if (pwshPath != null) return pwshPath;
        var psPath = GetFullPathFromPathEnv("powershell.exe");
        if (psPath != null) return psPath;
        return "pwsh.exe";
    }

    private static string? GetFullPathFromPathEnv(string fileName)
    {
        var paths = (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator);
        foreach (var dir in paths)
        {
            var full = Path.Combine(dir, fileName);
            if (File.Exists(full)) return full;
        }
        return null;
    }

    private static string Quote(string value) => value.Contains(' ') ? $"\"{value}\"" : value;
}
