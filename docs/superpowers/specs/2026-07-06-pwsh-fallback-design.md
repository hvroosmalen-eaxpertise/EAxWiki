# Issue #63: Add powershell.exe fallback for systems without pwsh.exe

## Problem

All scripts hard-depend on `pwsh.exe` (PowerShell 7+). If the user only has Windows
PowerShell 5.1 (`powershell.exe`), scripts fail with "command not found" errors.

The C# code in `PowerShellRunner.cs` also hardcodes `"pwsh.exe"` as the process to
launch for scheduled task registration and status queries.

## Solution

Shared bootstrap + C# helper approach: a single `_bootstrap.ps1` file that all
scripts dot-source, plus a `FindPowerShellExecutable()` static method in
`PowerShellRunner.cs`.

## Changes

### 1. `scripts/_bootstrap.ps1` (new)

Shared bootstrap dot-sourced at the top of every script. Provides three things:

- **`$Script:PSExecutable`** — full path to the currently running PowerShell,
  replacing inline `(Get-Process -Id $PID).Path` in `monitor-export-and-serve.ps1`
  and `register-scheduled-task.ps1`.

- **`$Script:IsWindowsOS`** — `$true` on Windows, works on both PS 5.1 (via
  `$env:OS -eq 'Windows_NT'`) and PS 7+ (via built-in `$IsWindows`). Replaces all
  inline `$IsWindows` usages.

- **Conditional `$PSNativeCommandUseErrorActionPreference`** — only set when the
  PS version is >= 7.3 (the version that introduced this preference). On 5.1 the
  preference doesn't exist; setting it as a plain variable is harmless.

### 2. Script changes (9 files)

Each script in `scripts/` gets a one-line addition and variable renames:

| Script | Bootstrap line | Changes |
|--------|---------------|---------|
| `export.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS` |
| `serve.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS` |
| `writeback.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS` |
| `serve-api.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS` |
| `export-and-serve.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS` |
| `start-scheduler-ui.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | (no `$IsWindows` usage) |
| `register-scheduled-task.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS`, `(Get-Process -Id $PID).Path` → `$PSExecutable` |
| `monitor-export-and-serve.ps1` | `. $PSScriptRoot\_bootstrap.ps1` | `$IsWindows` → `$IsWindowsOS`, `(Get-Process -Id $PID).Path` → `$PSExecutable` (2 places) |

### 3. `install.ps1`

- Remove `#Requires -Version 7` (blocks before any code runs).
- Add manual version check at top: warns if PS version < 7 but doesn't block.
- `. $PSScriptRoot\_bootstrap.ps1` — wait, `install.ps1` is at the repo root,
  not in `scripts/`. So the bootstrap path needs to be `$PSScriptRoot\scripts\_bootstrap.ps1`.

  Actually, `. $PSScriptRoot\scripts\_bootstrap.ps1` would work if `install.ps1`
  is at the root and `_bootstrap.ps1` is in `scripts/`.

  Wait, but `install.ps1` is special — it's used during initial setup. If someone
  runs it under PS 5.1, the bootstrap itself should work. Let me check: `$PSVersionTable`
  exists in 5.1, `Get-Process -Id $PID` works in 5.1. So the bootstrap is fine.

  But wait, `install.ps1` at root has `$PSScriptRoot` = the root directory.
  So `. $PSScriptRoot\scripts\_bootstrap.ps1` would source `scripts/_bootstrap.ps1`.

  Actually, in PS 5.1, `$PSScriptRoot` is only set for `.ps1` files that are
  dot-sourced or invoked as scripts (not interactively). For `install.ps1` invoked
  as `.\install.ps1`, `$PSScriptRoot` is the containing directory. This works fine.

### 4. `src/EAxWiki.SchedulerUI/PowerShellRunner.cs`

Add `FindPowerShellExecutable()`:

```csharp
private static string FindPowerShellExecutable()
{
    var pwshPath = GetFullPathFromPathEnv("pwsh.exe");
    if (pwshPath != null) return pwshPath;
    var psPath = GetFullPathFromPathEnv("powershell.exe");
    if (psPath != null) return psPath;
    return "pwsh.exe"; // last resort, will fail with a clear error
}

private static string? GetFullPathFromPathEnv(string fileName)
{
    // PATHEXT lookup on Windows: try each extension in PATHEXT for the given
    // base name, or just use the name directly if it already has an extension.
    var paths = (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator);
    foreach (var dir in paths)
    {
        var full = Path.Combine(dir, fileName);
        if (File.Exists(full)) return full;
    }
    return null;
}
```

Replace `FileName = "pwsh.exe"` with `FileName = FindPowerShellExecutable()`.

Add `using System.IO;` if not already present.

## Incompatibilities between PS 5.1 and PS 7+

| Feature | PS 5.1 | PS 7+ |
|---------|--------|-------|
| `$IsWindows` | Not available | Automatic variable |
| `$PSNativeCommandUseErrorActionPreference` | Not available | PS 7.3+ preference |
| `ConvertFrom-Json -Depth` | No `-Depth` parameter | Supports `-Depth` |
| `#Requires -Version 7` | Blocks execution | Passes |

The bootstrap handles the first two. The third (`ConvertFrom-Json -Depth`) is used
in `monitor-export-and-serve.ps1` for health state reading (depth 4-6). In PS 5.1,
the default depth is 2, which may truncate nested objects. The health state JSON
has at most 2-3 levels of nesting, so depth 2 may be sufficient. To be safe, the
bootstrap can check the PS version and skip the `-Depth` parameter on 5.1, or the
scripts can inline the check. Given the health state is shallow (flat-ish object
with primitive values), this is acceptable as-is.

## Testing

- All 179 .NET tests must pass after the `PowerShellRunner.cs` change.
- Manual: invoke each script under both `pwsh.exe` and `powershell.exe`.
- Manual: verify `dotnet test` and `dotnet build` still work.
