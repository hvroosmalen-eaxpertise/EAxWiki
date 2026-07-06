# PowerShell Fallback (pwsh → powershell.exe) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add `powershell.exe` fallback for systems without `pwsh.exe` — shared bootstrap for scripts, PATH detection in C#.

**Architecture:** Single `_bootstrap.ps1` dot-sourced by all scripts provides `$PSExecutable` and `$IsWindowsOS` shims. `PowerShellRunner.cs` gets `FindPowerShellExecutable()` that searches PATH for pwsh.exe first, then powershell.exe.

**Tech Stack:** PowerShell (5.1 + 7+), .NET 10 C#.

## Global Constraints

- Scripts must work under both `powershell.exe` (5.1) and `pwsh.exe` (7+)
- `install.ps1` warns but does NOT block execution on PS 5.1 (remove `#Requires -Version 7`)
- `PowerShellRunner.cs` must detect the available PowerShell at runtime
- All 179 .NET tests must pass
- `-EncodedCommand` is PS 7+ only; fallback for PS 5.1: write command to temp file and use `-File`

---

### Task 1: Create `scripts/_bootstrap.ps1`

**Files:**
- Create: `scripts/_bootstrap.ps1`

- [ ] **Step 1: Write `scripts/_bootstrap.ps1`**

```powershell
# _bootstrap.ps1 — Shared bootstrap for EAxWiki scripts.
# Dot-source at the top of every script: . $PSScriptRoot\_bootstrap.ps1
#
# Provides:
#   $PSExecutable   — Full path to the running PowerShell executable
#   $IsWindowsOS    — $true on Windows (works in both PS 5.1 and PS 7+)

$PSExecutable = (Get-Process -Id $PID).Path

if ($PSVersionTable.PSVersion.Major -ge 6) {
    $IsWindowsOS = $IsWindows
} else {
    $IsWindowsOS = $env:OS -eq 'Windows_NT'
}

# $PSNativeCommandUseErrorActionPreference is PS 7.3+ only.
# Setting it as a plain variable in 5.1 is harmless (silently ignored).
if ($PSVersionTable.PSVersion.Major -ge 7 -and $PSVersionTable.PSVersion.Minor -ge 3) {
    $PSNativeCommandUseErrorActionPreference = $false
}
```

- [ ] **Step 2: Commit**

```bash
git add scripts/_bootstrap.ps1
git commit -m "feat(scripts): add shared _bootstrap.ps1 for PS edition detection"
```

---

### Task 2: Update all scripts in `scripts/` to use bootstrap

**Files:**
- Modify: `scripts/export.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`)
- Modify: `scripts/serve.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`)
- Modify: `scripts/writeback.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`)
- Modify: `scripts/serve-api.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`)
- Modify: `scripts/export-and-serve.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`)
- Modify: `scripts/start-scheduler-ui.ps1` (add bootstrap line only)
- Modify: `scripts/register-scheduled-task.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`, `(Get-Process -Id $PID).Path` → `$PSExecutable`)
- Modify: `scripts/monitor-export-and-serve.ps1` (add bootstrap line, `$IsWindows` → `$IsWindowsOS`, `(Get-Process -Id $PID).Path` → `$PSExecutable` in 2 places)

Each script follows the same pattern:
1. Add `. $PSScriptRoot\_bootstrap.ps1` as the first non-comment, non-whitespace line
2. Replace `$IsWindows` with `$IsWindowsOS`
3. Replace `(Get-Process -Id $PID).Path` with `$PSExecutable`

- [ ] **Step 1: Update `export.ps1`** — insert bootstrap line after shebang/comments, replace `$IsWindows`

```powershell
$PSNativeCommandUseErrorActionPreference = $false
```
Becomes after bootstrap line:
```powershell
. $PSScriptRoot\_bootstrap.ps1
```
And `if (-not $IsWindows)` → `if (-not $IsWindowsOS)`

- [ ] **Step 2: Update `serve.ps1`** — insert bootstrap line, replace `$IsWindows` in the activate path and `if ($IsWindows)`

- [ ] **Step 3: Update `writeback.ps1`** — insert bootstrap line, `$IsWindows` → `$IsWindowsOS`

- [ ] **Step 4: Update `serve-api.ps1`** — insert bootstrap line, `$IsWindows` → `$IsWindowsOS`

- [ ] **Step 5: Update `export-and-serve.ps1`** — insert bootstrap line, `$IsWindows` → `$IsWindowsOS`

- [ ] **Step 6: Update `start-scheduler-ui.ps1`** — insert bootstrap line only (no `$IsWindows` usage)

- [ ] **Step 7: Update `register-scheduled-task.ps1`** — insert bootstrap line, `$IsWindows` → `$IsWindowsOS`, `(Get-Process -Id $PID).Path` → `$PSExecutable`

Line 153 changes from:
```powershell
$psExe = (Get-Process -Id $PID).Path
```
To (after bootstrap provides `$PSExecutable`):
```powershell
$psExe = $PSExecutable
```

- [ ] **Step 8: Update `monitor-export-and-serve.ps1`** — insert bootstrap line, `$IsWindows` → `$IsWindowsOS`, `(Get-Process -Id $PID).Path` → `$PSExecutable`

Line 653 changes:
```powershell
$psExe = (Get-Process -Id $PID).Path
```
to:
```powershell
$psExe = $PSExecutable
```

Also check if there's a second `(Get-Process -Id $PID).Path` in this file.

- [ ] **Step 9: Commit**

```bash
git add scripts/
git commit -m "feat(scripts): dot-source _bootstrap.ps1 in all scripts for PS 5.1 compat"
```

---

### Task 3: Update `install.ps1`

**Files:**
- Modify: `install.ps1`

- [ ] **Step 1: Edit `install.ps1`**

Remove line 1 (`#Requires -Version 7`).

Add after the parameter block (after line 28's closing `)`):

```powershell
# Bootstrap PS edition detection
. $PSScriptRoot\scripts\_bootstrap.ps1

# Warn but don't block on PS 5.1
if ($PSVersionTable.PSVersion.Major -lt 7) {
    Write-Warning "EAxWiki is designed for PowerShell 7+. Some features may not work under Windows PowerShell 5.1."
    Write-Warning "Install PowerShell 7 from: https://github.com/PowerShell/PowerShell/releases"
}
```

Replace all `$IsWindows` with `$IsWindowsOS` throughout.

- [ ] **Step 2: Commit**

```bash
git add install.ps1
git commit -m "feat(install): remove #Requires -Version 7, add PS 5.1 compat via bootstrap"
```

---

### Task 4: Update `PowerShellRunner.cs`

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/PowerShellRunner.cs`

- [ ] **Step 1: Add `FindPowerShellExecutable()` and `RunCommandViaTempFileAsync()`**

Replace `FileName = "pwsh.exe"` with `FileName = FindPowerShellExecutable()`. Add the lookup helper methods:

```csharp
using System.Diagnostics;
using System.IO;

namespace EAxWiki.SchedulerUI;

internal record PowerShellResult(int ExitCode, string Output);

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
            var fullCommand = $"$ProgressPreference = 'SilentlyContinue'; {command}";
            var encoded = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(fullCommand));
            var argLine = $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encoded}";
            return await RunAsync(argLine, workingDirectory);
        }
        else
        {
            // PS 5.1 has no -EncodedCommand — write to temp file and use -File
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
```

- [ ] **Step 2: Build and run tests**

```bash
dotnet build src/EAxWiki.Tests/EAxWiki.Tests.csproj
dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --no-build
```

Expected: Build succeeded, 179 passed.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.SchedulerUI/PowerShellRunner.cs
git commit -m "feat(scheduler): add FindPowerShellExecutable() for pwsh/powershell.exe fallback"
```

---

### Task 5: Final verification

- [ ] **Step 1: Full build and test**

```bash
dotnet build src/EAxWiki.Tests/EAxWiki.Tests.csproj
dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --no-build
```

Expected: 179 passed, 0 failed.

- [ ] **Step 2: Close issue #63**

Comment on the issue with the final commit references.
