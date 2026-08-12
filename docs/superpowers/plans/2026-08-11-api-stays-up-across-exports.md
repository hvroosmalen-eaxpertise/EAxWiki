# API Stays Up Across Exports Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Keep the EAxWiki write-back API server running continuously across export cycles — the export path never stops or restarts it, and a running API's loaded DLL is never overwritten by a rebuild.

**Architecture:** `dotnet run --project src/EAxWiki` rebuilds and overwrites the `EAxWiki.dll` that a running API server has loaded (that lock is why the monitor stopped the API before export). The fix removes the lock problem entirely: every `dotnet run --project src/EAxWiki` call site runs the **already-built** DLL via `dotnet exec`. A shared `Get-EAxWikiDllPath` helper in `scripts/_bootstrap.ps1` resolves the DLL (taking `-RepoRoot` explicitly, so it is independent of `$PSScriptRoot`/`$PSCommandPath` semantics inside dot-sourced functions) and fails with a clear "run `dotnet build` first" error when it is missing — no build fallback. The monitor drops its stop-before-export block and the now-dead `Stop-ApiServer` function; its API watchdog (restart-on-crash) is unchanged, so a crashed API is still restarted on the next pass. The monitor also gains a dot-source run-guard so Pester can load it without executing the monitoring loop (which previously made the harness hang or exit the test host).

**Tech Stack:** PowerShell 5.1+/7 (scripts), Pester 5, .NET 10 `dotnet` CLI (`dotnet exec`).

## Global Constraints

- **No build during export.** Export must run the already-built `src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll`; never `dotnet run --project src/EAxWiki`, never `dotnet build`.
- **Missing DLL ⇒ clear error, no fallback.** If the DLL is absent, `Get-EAxWikiDllPath` must `throw` with exactly `EAxWiki.dll not found at '<path>'. Run 'dotnet build src/EAxWiki' first.`
- **API lifecycle.** The API is stopped/restarted *only* by the monitor's existing API watchdog when it detects a crash (monitor:1161-1218, unchanged). The export path must never proactively stop it.
- **Skipping `dotnet run` is what removes the lock** — a loaded DLL can be overwritten only by a rebuild; `dotnet exec` of the existing DLL touches no binaries.
- **`.qea` concurrency is safe** — SQLite-backed, EA supports concurrent multi-process access, and the exporter is read-only on the model (only the API writes, on rate-limited user write-backs).
- **Run-guard scope is monitor-only.** The pre-existing fragility where dot-sourcing `export.ps1`/`writeback.ps1`/`serve-api.ps1`/`export-and-serve.ps1` runs their full bodies (including a real `dotnet exec` export) during Pester is **out of scope** — do not add run-guards to those scripts.
- **Out of scope:** `start-scheduler-ui.ps1`, `install`/`register-scheduled-task` scripts, the SchedulerUI's own `dotnet run` (`src/EAxWiki.SchedulerUI`), the monitor's hardcoded `$effectiveForce = $true` (monitor:848), any C# changes, and the pre-existing uncommitted wiki export output in `wiki/`.
- **Never stage generated dirs.** `git add` only the specific source/test files listed in each commit step — never `wiki/`, `.eaxwiki-monitor/`, `model/`, or `.eaxwiki`.
- **DLL must exist before running the Pester suite** (Tasks 2-4's dot-sourced bodies call `Get-EAxWikiDllPath`). Run `dotnet build src/EAxWiki` first; the repo currently has the artifacts at `src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll` (+ `.runtimeconfig.json`, `.deps.json`, `EAxWiki.exe`).
- **Encoding:** these files contain no emoji and should stay as-is (monitor + its test file are UTF-8 with BOM; `_bootstrap.ps1` and the other scripts are as committed). Verify BOM is preserved after edits to the monitor.
- **Commit convention** (mirroring repo style): `type(scope): subject`, no issue number (this task has none — the only open issues are #80/#75/#73/#71/#69/#43/#42).

---

### Task 1: `Get-EAxWikiDllPath` shared helper in `_bootstrap.ps1`

**Files:**
- Modify: `scripts/_bootstrap.ps1` (append function at end, after line 20)
- Test: `tests/scripts/_bootstrap.Tests.ps1` (new file)

**Interfaces:**
- Produces: `Get-EAxWikiDllPath` — `param([string]$RepoRoot)` returns the absolute path to `src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll`, or `throw`s `"EAxWiki.dll not found at '<path>'. Run 'dotnet build src/EAxWiki' first."`. Consumed by Tasks 2, 3, 4.
- The `-RepoRoot` parameter is intentional: the helper is defined inside `_bootstrap.ps1`, and `$PSScriptRoot`/`$PSCommandPath` inside a function defined in a dot-sourced file is not reliably the bootstrap's directory. Each caller already computes `$repoRoot` at top level (where `$MyInvocation.MyCommand.Definition` is reliable), so passing it in is explicit and testable.

- [ ] **Step 1: Write the failing test**

Create `tests/scripts/_bootstrap.Tests.ps1`:

```powershell
BeforeAll {
    . "$PSScriptRoot\..\..\scripts\_bootstrap.ps1"
}

Describe 'Get-EAxWikiDllPath' {
    It 'returns the built EAxWiki.dll path for a repo root' {
        $repoRoot = Split-Path -Parent $PSScriptRoot | Split-Path -Parent
        $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
        $dll | Should -EndWith 'src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll'
        Test-Path $dll | Should -BeTrue
    }

    It 'throws a clear error when the DLL has not been built' {
        Mock Test-Path { return $false }
        { Get-EAxWikiDllPath -RepoRoot 'C:\does-not-exist' } | Should -Throw -ExpectedMessage '*dotnet build src/EAxWiki*'
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/_bootstrap.Tests.ps1 -Output Detailed`
Expected: FAIL — `Get-EAxWikiDllPath` is not recognized (command not found).

- [ ] **Step 3: Write minimal implementation**

Append to the end of `scripts/_bootstrap.ps1`:

```powershell

# Get-EAxWikiDllPath — resolve the pre-built EAxWiki.dll and verify it exists. Running the DLL
# via `dotnet exec` instead of `dotnet run --project src/EAxWiki` avoids rebuilding/overwriting
# the DLL that a running write-back API server has loaded, which is what lets the API stay up
# across export runs (see export.ps1 / writeback.ps1 / export-and-serve.ps1 / serve-api.ps1).
# RepoRoot is explicit (not derived from $PSScriptRoot) because this function is defined in a
# dot-sourced file, where those automatic variables do not reliably point at the repo.
function Get-EAxWikiDllPath {
    param([string]$RepoRoot)
    $dllPath = Join-Path $RepoRoot "src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll"
    if (-not (Test-Path $dllPath)) {
        throw "EAxWiki.dll not found at '$dllPath'. Run 'dotnet build src/EAxWiki' first."
    }
    return $dllPath
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `Invoke-Pester tests/scripts/_bootstrap.Tests.ps1 -Output Detailed`
Expected: PASS (2 tests).

- [ ] **Step 5: Commit**

```bash
git add scripts/_bootstrap.ps1 tests/scripts/_bootstrap.Tests.ps1
git commit -m "feat(scripts): add Get-EAxWikiDllPath to resolve the pre-built EAxWiki.dll"
```

---

### Task 2: `export.ps1` runs the pre-built DLL

**Files:**
- Modify: `scripts/export.ps1:76-77` (stale comment), `:101-102` (the `dotnet run` invocation)
- Test: `tests/scripts/export.Tests.ps1` (append `Describe`)

**Interfaces:**
- Consumes: Task 1's `Get-EAxWikiDllPath`.
- Produces: `export.ps1` no longer contains `dotnet run --project src/EAxWiki`; its export invocation is `dotnet exec <dll> $runArgs`. The `EAXWIKI_EXIT_CODE=$code` protocol with the monitor (parsed at monitor:900-907) is unchanged.

- [ ] **Step 1: Write the failing test**

Append to `tests/scripts/export.Tests.ps1`:

```powershell
Describe 'export.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/export.Tests.ps1 -Output Detailed`
Expected: FAIL — `Should -Not -Match 'dotnet run --project'` fails because `scripts/export.ps1:102` still contains `dotnet run --project src/EAxWiki`.

- [ ] **Step 3: Write minimal implementation**

In `scripts/export.ps1`, replace the comment at lines 76-77:

```powershell
# Resolve output directory to an absolute path so it is unambiguous regardless of the
# working directory the spawned process runs in.
```

Replace the invocation at lines 101-102:

```powershell
try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $runArgs
    $code = if ($null -eq $LASTEXITCODE) { 0 } else { $LASTEXITCODE }
```

Everything after (the `EAXWIKI_EXIT_CODE` line, error handling, `Cleanup-EAProcesses`) is untouched.

- [ ] **Step 4: Run test to verify it passes**

Run: `Invoke-Pester tests/scripts/export.Tests.ps1 -Output Detailed`
Expected: PASS (26 tests — 25 existing `Get-ExportArgs` + 1 new). Note: the `BeforeAll` dot-source still runs the full export body (pre-existing fragility; now it runs `dotnet exec` of the built DLL — no build, so it is strictly lighter than before). The DLL must exist (see Global Constraints).

- [ ] **Step 5: Commit**

```bash
git add scripts/export.ps1 tests/scripts/export.Tests.ps1
git commit -m "fix(export): run pre-built EAxWiki.dll via dotnet exec to avoid API DLL lock"
```

---

### Task 3: `writeback.ps1` runs the pre-built DLL

**Files:**
- Modify: `scripts/writeback.ps1:72-73` (the `dotnet run` invocation)
- Test: `tests/scripts/writeback.Tests.ps1` (append `Describe`)

**Interfaces:**
- Consumes: Task 1's `Get-EAxWikiDllPath`.
- Produces: `writeback.ps1`'s invocation is `dotnet exec <dll> $runArgs`; exit-code handling unchanged.

- [ ] **Step 1: Write the failing test**

Append to `tests/scripts/writeback.Tests.ps1`:

```powershell
Describe 'writeback.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/writeback.Tests.ps1 -Output Detailed`
Expected: FAIL — `scripts/writeback.ps1:73` still contains `dotnet run --project src/EAxWiki`.

- [ ] **Step 3: Write minimal implementation**

In `scripts/writeback.ps1`, replace lines 72-73:

```powershell
try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $runArgs
    if ($LASTEXITCODE -ne 0) {
```

- [ ] **Step 4: Run test to verify it passes**

Run: `Invoke-Pester tests/scripts/writeback.Tests.ps1 -Output Detailed`
Expected: PASS (14 tests — 13 existing + 1 new).

- [ ] **Step 5: Commit**

```bash
git add scripts/writeback.ps1 tests/scripts/writeback.Tests.ps1
git commit -m "fix(writeback): run pre-built EAxWiki.dll via dotnet exec to avoid API DLL lock"
```

---

### Task 4: `serve-api.ps1` + `export-and-serve.ps1` run the pre-built DLL in their API jobs

**Files:**
- Modify: `scripts/serve-api.ps1:87-91` (the `Start-Job` scriptblock), `scripts/export-and-serve.ps1:101-105` (same)
- Test: `tests/scripts/serve-api.Tests.ps1` (append `Describe`), `tests/scripts/export-and-serve.Tests.ps1` (append `Describe`)

**Interfaces:**
- Consumes: Task 1's `Get-EAxWikiDllPath`.
- Produces: in each script, a parent-scope `$dll = Get-EAxWikiDllPath -RepoRoot $repoRoot` resolved **before** `Start-Job` (so the clear "run dotnet build first" error surfaces before any background job is created), and the job scriptblock becomes `param($root, $dllPath, $argList)` → `Set-Location $root` → `dotnet exec $dllPath $argList`. `$root` is still used for `Set-Location` so `.eaxwiki` discovery (Program.cs walks up from CWD) and relative repo paths keep working inside the job.

- [ ] **Step 1: Write the failing tests**

Append to `tests/scripts/serve-api.Tests.ps1`:

```powershell
Describe 'serve-api.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\serve-api.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

Append to `tests/scripts/export-and-serve.Tests.ps1`:

```powershell
Describe 'export-and-serve.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

- [ ] **Step 2: Run tests to verify they fail**

Run: `Invoke-Pester tests/scripts/serve-api.Tests.ps1,tests/scripts/export-and-serve.Tests.ps1 -Output Detailed`
Expected: FAIL in both — each script still contains `dotnet run --project src/EAxWiki` in its `Start-Job` scriptblock.

- [ ] **Step 3: Write minimal implementation**

In `scripts/serve-api.ps1`, replace lines 87-91:

```powershell
$dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
$apiJob = Start-Job -ScriptBlock {
    param($root, $dllPath, $argList)
    Set-Location $root
    dotnet exec $dllPath $argList
} -ArgumentList $repoRoot, $dll, $apiArgs
```

In `scripts/export-and-serve.ps1`, replace lines 101-105 (same shape):

```powershell
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    $apiJob = Start-Job -ScriptBlock {
        param($root, $dllPath, $argList)
        Set-Location $root
        dotnet exec $dllPath $argList
    } -ArgumentList $repoRoot, $dll, $apiArgs
```

(The `export-and-serve.ps1` version is inside `if ($ApiPort -gt 0) { ... }` and keeps its existing 8-space indentation. `Start-Job` binds three parameters with two array params intact — verified empirically.)

- [ ] **Step 4: Run tests to verify they pass**

Run: `Invoke-Pester tests/scripts/serve-api.Tests.ps1,tests/scripts/export-and-serve.Tests.ps1 -Output Detailed`
Expected: PASS (serve-api 13, export-and-serve 23).

- [ ] **Step 5: Commit**

```bash
git add scripts/serve-api.ps1 scripts/export-and-serve.ps1 tests/scripts/serve-api.Tests.ps1 tests/scripts/export-and-serve.Tests.ps1
git commit -m "fix(scripts): run pre-built EAxWiki.dll via dotnet exec in api jobs"
```

---

### Task 5: Monitor no longer stops the API before export

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1:873-878` (remove stop-before-export block), `:717-790` (remove now-dead `Stop-ApiServer` function)
- Test: `tests/scripts/monitor-export-and-serve.Tests.ps1` (append `Describe`)

**Interfaces:**
- Consumes: nothing new. The API watchdog at monitor:1161-1218 (restart-on-crash) and `Clear-Port` (monitor:792-801, still used at monitor:1170) are untouched.
- Produces: the monitor's export path never invokes `Stop-ApiServer`; `Stop-ApiServer` no longer exists anywhere (removing the call leaves it dead — grep confirms it is referenced only at its definition and the removed call).

- [ ] **Step 1: Write the failing test**

Append to `tests/scripts/monitor-export-and-serve.Tests.ps1`:

```powershell
Describe 'API stays up across exports' {
    It 'no longer stops the API server before export' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\monitor-export-and-serve.ps1" -Raw
        $content | Should -Not -Match 'Stop the API server before export'
        $content | Should -Not -Match 'Stop-ApiServer'
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: FAIL — both patterns are present in `scripts/monitor-export-and-serve.ps1` today.

- [ ] **Step 3: Write minimal implementation**

In `scripts/monitor-export-and-serve.ps1`:

**(a)** Remove the stop-before-export block (lines 872-878), leaving the retry loop's opening intact:

```powershell
        if (-not $skipPhase) {
```

(Remove exactly these lines — the comment and the guarded call — between `if (-not $skipPhase) {` and `$exportArgs = @("--output", $wikiDir)`:

```powershell
            # Stop the API server before export so its DLL locks don't prevent
            # dotnet-run from overwriting the binaries.  The API watchdog section
            # at the end of this pass will restart it automatically.
            if ($ApiPort -gt 0 -and (Test-ApiAlive)) {
                Stop-ApiServer
            }

```

**(b)** Remove the entire now-dead `Stop-ApiServer` function (lines 717-790), from `function Stop-ApiServer {` through its closing `}` (which ends with `Clear-Port -PortNumber $ApiPort`). `Clear-Port` and `Test-ApiAlive` (both used elsewhere) stay.

- [ ] **Step 4: Run test to verify it passes**

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: PASS (39 tests — 38 existing + 1 new). (If this run hangs or the test host exits, see Task 6 Step 2 note — the harness is not yet deterministic until the run-guard lands; the hang is the pre-existing dot-source behavior, not a regression from this task.)

- [ ] **Step 5: Commit**

```bash
git add scripts/monitor-export-and-serve.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1
git commit -m "fix(monitor): never stop the API server before export"
```

---

### Task 6: Monitor dot-source run-guard

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1` — relocate `Write-MonitorLog` (currently 337-342) to right after `ConvertTo-RedactedConnectionString` (line 193); insert run-guard immediately after it; refresh the stale comment inside `Send-TelegramMessage` (line 128)
- Test: `tests/scripts/monitor-export-and-serve.Tests.ps1` — new `Describe 'Dot-source run-guard'`; add `Mock Write-MonitorLog { }` to the `Send-TelegramMessage` `BeforeEach`

**Interfaces:**
- Consumes: Task 5 (monitor now testable from source; this task makes it testable by dot-source too).
- Produces: when Pester dot-sources the monitor, `Get-MonitorArgs`, `Send-TelegramMessage`, `Write-MonitorLog`, and `ConvertTo-RedactedConnectionString` are defined and control returns immediately — none of the setup body (`$parsed`, `.eaxwiki` load, PID-file write/`exit 0` on duplicate monitor, `while ($true)` loop) runs. Normal invocation (`.\monitor-export-and-serve.ps1`) is unaffected because `$MyInvocation.InvocationName` is the script path, not `.`.

- [ ] **Step 1: Write the failing test**

Append to `tests/scripts/monitor-export-and-serve.Tests.ps1`:

```powershell
Describe 'Dot-source run-guard' {
    It 'defines the testable functions but does not execute the monitor body' {
        (Get-Command Get-MonitorArgs -ErrorAction SilentlyContinue) | Should -Not -BeNullOrEmpty
        (Get-Command Send-TelegramMessage -ErrorAction SilentlyContinue) | Should -Not -BeNullOrEmpty
        (Get-Command Write-MonitorLog -ErrorAction SilentlyContinue) | Should -Not -BeNullOrEmpty
        Test-Path variable:parsed | Should -Be $false
        Test-Path variable:logDir | Should -Be $false
        Test-Path variable:state | Should -Be $false
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: FAIL. Two possible failure modes, both prove the body executes without the guard:
- If a monitor process is running: the dot-source hits the duplicate-monitor check (`exit 0`, monitor:357), which can kill the whole Pester host — the run aborts.
- If no monitor is running: the body runs past `$parsed = ...` (monitor:195), so `Test-Path variable:parsed` is `$true` and the assertion fails — or the suite hangs inside `while ($true)` (monitor:824); press Ctrl+C to abort.
Either way, interrupt if needed and proceed to Step 3.

- [ ] **Step 3: Write minimal implementation**

In `scripts/monitor-export-and-serve.ps1`:

**(a)** Move the `Write-MonitorLog` function (lines 337-342) verbatim to right after `ConvertTo-RedactedConnectionString`'s closing `}` (line 193), so it exists during dot-source:

```powershell
function Write-MonitorLog {
    param([string]$Phase, [string]$Message)
    $line = "{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}" -f (Get-Date), $Phase, $Message
    Add-Content -Path $logPath -Value $line
    Write-Host $line
}
```

(Runtime behavior is unchanged: `$logPath` is resolved at call time from the script scope, and the function is still defined before its first real call.)

**(b)** Insert the run-guard immediately after the relocated `Write-MonitorLog`, before `$parsed = Get-MonitorArgs -Arguments $args` (line 195):

```powershell
# Dot-source run-guard: when Pester dot-sources this file, only the function definitions
# above are loaded and control returns immediately. Without this, the parse/bind, .eaxwiki
# load, PID-file bookkeeping (including the duplicate-monitor `exit`), and the
# while ($true) monitoring loop would run a real monitoring pass (or hang) during tests.
# Normal invocation (.\monitor-export-and-serve.ps1) is unaffected because
# $MyInvocation.InvocationName is the script path, not '.'.
if ($MyInvocation.InvocationName -eq '.') {
    return
}
```

**(c)** Refresh the now-stale comment inside `Send-TelegramMessage` (line 128) to reference the run-guard instead of the duplicate-monitor exit:

```powershell
    # Issue #80: Telegram Bot API dispatch. Standalone (no dependency on the script's top-level
    # variables) so Pester can exercise it even when the monitor body is skipped by the
    # dot-source run-guard. Token goes in the URL (standard Telegram pattern); chat_id is a
    # *string* because group/supergroup IDs are negative numbers (-100...) and must survive
    # JSON round-tripping.
```

**(d)** In `tests/scripts/monitor-export-and-serve.Tests.ps1`, extend the `Send-TelegramMessage` `BeforeEach` (line 156-160) with a `Write-MonitorLog` mock so the dispatch tests don't attempt `Add-Content -Path $null` (`$logPath` is unset now that the body is skipped):

```powershell
    BeforeEach {
        Mock Write-MonitorLog { }
        $script:tgCalls = 0
        $global:tgUri = $null
        $global:tgBody = $null
    }
```

- [ ] **Step 4: Verify encoding + run test**

Verify the monitor file and its test file are still UTF-8 with BOM (PS 5.1 mojibakes no-BOM files):

```powershell
Get-ChildItem scripts\monitor-export-and-serve.ps1, tests\scripts\monitor-export-and-serve.Tests.ps1 | ForEach-Object { $b = [System.IO.File]::ReadAllBytes($_.FullName); "{0}: {1}" -f $_.Name, $(if ($b.Length -ge 3 -and $b[0] -eq 0xEF -and $b[1] -eq 0xBB -and $b[2] -eq 0xBF) { 'UTF8-BOM' } else { 'no-BOM' }) }
```

If either shows `no-BOM`, re-save with BOM (`$c = Get-Content $f -Raw -Encoding UTF8; [System.IO.File]::WriteAllText($f, $c, (New-Object System.Text.UTF8Encoding $true))`).

Run: `Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed`
Expected: PASS (40 tests — 39 from Task 5 plus the new run-guard test; the two existing `Send-TelegramMessage` tests now run against the `Write-MonitorLog` mock).

- [ ] **Step 5: Sanity-run the parser**

Run: `& 'E:\Users\Han\Repos\EAxWiki\scripts\monitor-export-and-serve.ps1' --test-alert 2>&1 | Select-Object -Last 5`
Expected: no parse error. (If a monitor is running, it will exit at the duplicate-monitor check — expected. If none is running, it will run one monitoring pass; that is normal runtime behavior, and it proves the guard did not break real invocation.)

- [ ] **Step 6: Commit**

```bash
git add scripts/monitor-export-and-serve.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1
git commit -m "test(monitor): add dot-source run-guard so pester skips the monitoring loop"
```

---

### Task 7: Full verification + README test counts

**Files:**
- Modify: `README.md` — Tests table (`:622-644`)

**Interfaces:**
- Consumes: all tasks.
- Produces: verified, accurate documentation of test totals.

- [ ] **Step 1: Build the DLL**

Run: `dotnet build src/EAxWiki`
Expected: build succeeds. (Required so the Pester dot-source bodies and `_bootstrap.Tests.ps1` find the DLL.)

- [ ] **Step 2: Run the full .NET suite**

Run: `dotnet test src/EAxWiki.Tests`
Expected: all pass (275 tests — unchanged by this plan; no C# changes).

- [ ] **Step 3: Run the full Pester suite**

Run: `Invoke-Pester tests/scripts/ -Output Detailed`
Expected: all pass. Record the per-file counts. Expected new totals: Bootstrap 2, Export 26, ExportAndServe 23, ServeApi 13, Writeback 14, MonitorExportAndServe 40 → Pester subtotal **155**.

- [ ] **Step 4: Update README Tests table**

In `README.md`, add a `Bootstrap` row and update the changed counts:

```markdown
| Bootstrap | 2 | `Get-EAxWikiDllPath` resolution + clear missing-DLL error |
```

Update: `| Export | 25 |` → `| Export | 26 |`; `| ExportAndServe | 22 |` → `| ExportAndServe | 23 |`; `| MonitorExportAndServe | 38 |` → `| MonitorExportAndServe | 40 |`; `| ServeApi | 12 |` → `| ServeApi | 13 |`; `| Writeback | 13 |` → `| Writeback | 14 |`. Then `| **Pester subtotal** | **147** |` → `| **Pester subtotal** | **155** |`, and the total line `**422 tests total** (275 .NET + 147 Pester), all pass.` → `**430 tests total** (275 .NET + 155 Pester), all pass.`

(Use the actual numbers from Step 3 if they differ.)

- [ ] **Step 5: Verify the diff is clean**

Run: `git diff --stat`
Expected: only the intended files (scripts, tests, README). Do not stage `wiki/`, `.eaxwiki-monitor/`, `model/`, or `.eaxwiki`.

- [ ] **Step 6: Commit + push**

```bash
git add README.md
git commit -m "docs: update pester test counts for pre-built-dll execution"
git push origin master
```

---

### Task 8: Manual E2E — API stays up across a real export

**Files:** none (manual verification only)

**Interfaces:**
- Consumes: everything.

- [ ] **Step 1: Start the API**

Run: `.\scripts\serve-api.ps1 --output wiki` (or start the monitor via `.\scripts\monitor-export-and-serve.ps1` if you prefer the unattended path). Wait for the write-back server to report ready, then confirm it is alive:

```powershell
Invoke-RestMethod -Uri "http://localhost:8001/healthz" -TimeoutSec 5
```

Record the API PID (from `.eaxwiki-monitor\<hash>\api.pid` if using the monitor, or the job's process).

- [ ] **Step 2: Run an export while the API is up**

Run: `.\scripts\export.ps1 --output wiki`
Expected: export completes (`EAXWIKI_EXIT_CODE=0`, "Export complete."). No build happens (no `dotnet run`/`MSBuild` output).

- [ ] **Step 3: Verify the API was never stopped**

Check **all** of the following:
1. The API PID from Step 1 still exists and is still the same process (`Get-Process -Id <pid>`).
2. `Invoke-RestMethod -Uri "http://localhost:8001/healthz"` still returns 200.
3. The monitor log (`.eaxwiki-monitor\<hash>\logs\monitor-*.log`) contains **no** `Stopping tracked API server` line for this export.
4. `.eaxwiki-monitor\<hash>\api.pid` was not rewritten (the watchdog only rewrites it after a crash/restart).

- [ ] **Step 4: Stop services**

Stop `serve-api.ps1` with Ctrl+C (its `finally` block stops the API job) or stop the monitor process, and confirm the API exits.

---

## Self-Review

**Spec coverage:** Goal (API never proactively stopped; export runs pre-built DLL) → Tasks 1-5 + E2E Task 8. DLL-missing clear error + no build fallback → Task 1 (Global Constraints). Monitor harness determinism → Task 6. README counts → Task 7. All four `dotnet run` call sites covered: export.ps1 (Task 2), writeback.ps1 (Task 3), serve-api.ps1 + export-and-serve.ps1 (Task 4). Monitor watchdog restart-on-crash untouched (Global Constraints; no task modifies monitor:1161-1218).

**Placeholder scan:** no TBD/TODO; every code step shows complete code; every run step has an exact command and expected output.

**Type consistency:** `Get-EAxWikiDllPath -RepoRoot $repoRoot` (param `[string]$RepoRoot`, returns DLL path or throws) is consumed identically in Tasks 2-4. `$dll` in the parent scope feeds `-ArgumentList $repoRoot, $dll, $apiArgs` and the job binds `param($root, $dllPath, $argList)` consistently in both scripts in Task 4. Run-guard introduces no new public surface.
