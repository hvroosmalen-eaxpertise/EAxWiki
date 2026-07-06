# Pester Tests for PowerShell Scripts

## Problem

All 8 PowerShell scripts (`export.ps1`, `serve.ps1`, `writeback.ps1`, `start-scheduler-ui.ps1`, `export-and-serve.ps1`, `serve-api.ps1`, `monitor-export-and-serve.ps1`, `install.ps1`) have zero automated test coverage.

## Approach

Refactor each manually-parsing script to extract flag parsing into a named function (`Get-<Name>Args`), then write Pester 5 tests that dot-source the script and test parsing independently. No behavioral changes — just a function boundary.

### Scripts and their handling

| Script | Flag parsing | Refactor needed? | Test file |
|---|---|---|---|
| `export.ps1` | Manual | Yes — extract `Get-ExportArgs` | `export.Tests.ps1` |
| `serve.ps1` | Manual | Yes — extract `Get-ServeArgs` | `serve.Tests.ps1` |
| `writeback.ps1` | Manual | Yes — extract `Get-WritebackArgs` | `writeback.Tests.ps1` |
| `start-scheduler-ui.ps1` | None | No (trivial) | — |
| `export-and-serve.ps1` | Manual | Yes — extract `Get-ExportAndServeArgs` | `export-and-serve.Tests.ps1` |
| `serve-api.ps1` | Manual | Yes — extract `Get-ServeApiArgs` | `serve-api.Tests.ps1` |
| `monitor-export-and-serve.ps1` | Manual | Yes — extract `Get-MonitorArgs` | `monitor-export-and-serve.Tests.ps1` |
| `install.ps1` | `param()` | No (already testable) | `install.Tests.ps1` |

### Refactoring pattern

Each script with manual flag parsing adds a function near the top:

```powershell
function Get-ExportArgs {
    param([string[]]$Arguments)
    # ... same while/switch logic as current script body ...
    return [PSCustomObject]@{
        Force     = $false
        Verbose   = $false
        Json      = $false
        WriteBack = $false
        RepoPath  = ""
        OutputDir = ""
        ApiPort   = 0
    }
}
```

The script body after the function becomes:
```powershell
$parsed = Get-ExportArgs -Arguments $args
$Force     = $parsed.Force
$Verbose   = $parsed.Verbose
# ... etc
```

The parsing logic is byte-for-byte identical, just moved inside the function and reading from `$Arguments` instead of `$args`.

### Test scope per script

All test files cover:

1. **Flag parsing equivalence** — both `-Flag` and `--flag` produce identical `[PSCustomObject]`
2. **Defaults** — no arguments returns documented defaults
3. **Flag values** — flags with values (`--repo`, `--port`, `--output` etc.) correctly capture the next argument
4. **Connection string vs file path** — repo paths containing `=` are treated as connection strings (not resolved relative to repo root)
5. **Error paths** — invalid port numbers, missing values after flag, empty strings
6. **Unicode paths** — paths containing non-ASCII characters
7. **`install.ps1`** — verifies `param()` defaults and switch binding (no refactoring needed)

### Prerequisites

- Pester 5: `Install-Module Pester -Force -SkipPublisherCheck`
- Tests run with `Invoke-Pester tests/scripts/` from repo root
- All tests run on any OS (no COM dependency)

### Files

- `tests/scripts/` — new directory
- `tests/scripts/export.Tests.ps1`
- `tests/scripts/serve.Tests.ps1`
- `tests/scripts/writeback.Tests.ps1`
- `tests/scripts/export-and-serve.Tests.ps1`
- `tests/scripts/serve-api.Tests.ps1`
- `tests/scripts/monitor-export-and-serve.Tests.ps1`
- `tests/scripts/install.Tests.ps1`

Modified:
- `scripts/export.ps1` — add `Get-ExportArgs`, call it
- `scripts/serve.ps1` — add `Get-ServeArgs`, call it
- `scripts/writeback.ps1` — add `Get-WritebackArgs`, call it
- `scripts/export-and-serve.ps1` — add `Get-ExportAndServeArgs`, call it
- `scripts/serve-api.ps1` — add `Get-ServeApiArgs`, call it
- `scripts/monitor-export-and-serve.ps1` — add `Get-MonitorArgs`, call it
