# Write-back Server HTTP & STA Reconnect Tests Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add HTTP-level tests for the write-back server's rate limiter (429), origin-bypass behavior, and graceful shutdown, plus make the STA dispatcher's COMException-reconnect logic live and unit-test it — all without a live EA repository.

**Architecture:** Three independent components. (1) `Config.ApiRateLimitPerMinute` (default 60) becomes the rate limiter's permit limit; an HTTP test fires 4 requests against a limit of 3 and asserts the 4th gets 429. (2) The existing `WikiWritebackServerHttpTests` fixture is refactored to build apps from a caller-supplied `Config`, then gains three tests. (3) The retry/reconnect loop inside `EaReaderStaDispatcher.RunStaPump` is extracted into an internal static delegate-driven `ExecuteWithReconnect`, the swallow-exceptions bug in `WorkItem.Execute` is fixed so `COMException` reaches the pump, and five delegate-only unit tests (plus one dispatch regression test) verify the loop.

**Tech Stack:** .NET 10 (C#), xUnit 2.9, `Microsoft.AspNetCore.TestHost` 10.0.11, ASP.NET Core minimal hosting (`WebApplication`), `System.Threading.RateLimiting`, `System.Runtime.InteropServices` (`COMException`), Pester (untouched).

## Global Constraints

- No change to **public** API: `IEaReader`, `EaReaderStaDispatcher` public surface, `WikiWritebackServer` public surface, `FakeEaReader`. New members are `internal` or `private` (`Config.ApiRateLimitPerMinute` is a new public property — the only public addition).
- No change to default production behavior: `Config.ApiRateLimitPerMinute` defaults to 60, identical to today's hardcoded limit. `/api/shutdown` response-then-stop ordering unchanged.
- Exception (approved in brainstorming): making the STA reconnect path live is a **deliberate production behavior fix** — today `WorkItem.Execute` swallows all exceptions so the reconnect loop never fires and `_isHealthy` is always true; after this change `COMException` triggers one retry+reconnect and `IsHealthy` reflects real state.
- No new NuGet dependencies. Tests use the existing `Microsoft.AspNetCore.TestHost` infrastructure.
- CRLF / file conventions preserved. New/changed files: LF, UTF-8 (no BOM).
- Every `dotnet test` command requires `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\';` because `EAxWiki.EA` unconditionally references `Interop.EA.dll` via `$(EAPath)`. Set it inline per command (see commands below).
- All existing tests stay green: 301 .NET + 162 Pester. New tests add to the .NET count.
- Working-tree hygiene: `model/`, `wiki/`, `.eaxwiki-monitor/audit.log`, `wiki/status/writeback.log` are runtime artifacts and must never be staged or committed.
- Do not commit or push unless explicitly asked. Commits use lowercase conventional style matching repo history (e.g. `feat(config): ...`, `test(api): ...`).

## File Structure

- Modify `src/EAxWiki/Config.cs` — add `public int ApiRateLimitPerMinute { get; set; } = 60;` next to `ApiPort`/`WikiPort`. No CLI flag (deferred to issue part 4).
- Modify `src/EAxWiki/WikiWritebackServer.cs:378` — `PermitLimit = config.ApiRateLimitPerMinute` (was hardcoded `60`).
- Modify `src/EAxWiki.Tests/ConfigTests.cs` — one new test asserting the default value.
- Modify `src/EAxWiki.Tests/WikiWritebackServerHttpTests.cs` — extract `BuildAppAsync(Config)` helper; add 3 tests (429, origin-bypass, shutdown).
- Modify `src/EAxWiki.EA/EaReaderStaDispatcher.cs` — add `internal static ExecuteWithReconnect`, add `private EaReader OpenNewReader(string)`, make `WorkItem.Execute` not swallow exceptions, make `WorkItem` `internal`, rewrite `RunStaPump`'s work-item loop to call the helper.
- Add `src/EAxWiki.Tests/EaReaderStaDispatcherTests.cs` — 6 tests (5 helper-loop + 1 dispatch regression).

---

### Task 1: `Config.ApiRateLimitPerMinute` option

**Files:**
- Modify: `src/EAxWiki/Config.cs:16` (insert property after `WikiPort`)
- Test: `src/EAxWiki.Tests/ConfigTests.cs`

**Interfaces:**
- Produces: `public int Config.ApiRateLimitPerMinute { get; set; }` with initializer `= 60;`

- [ ] **Step 1: Write the failing test**

Add to the end of `src/EAxWiki.Tests/ConfigTests.cs` (before closing `}` of the class):

```csharp
    [Fact]
    public void Load_NoArgs_ApiRateLimitDefaultsTo60()
    {
        var cfg = new Config();
        cfg.Load([]);
        Assert.Equal(60, cfg.ApiRateLimitPerMinute);
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~ConfigTests"
```

Expected: compile error `Config` does not contain a definition for `ApiRateLimitPerMinute` (the test project references the `EAxWiki` project, which will be rebuilt).

- [ ] **Step 3: Implement the property**

In `src/EAxWiki/Config.cs`, after the `WikiPort` line (`public int WikiPort { get; set; } = 0;`), add:

```csharp
    public int ApiRateLimitPerMinute { get; set; } = 60;
```

- [ ] **Step 4: Run test to verify it passes**

Run the same command as Step 2. Expected: `Passed! - Failed: 0, Passed: <N>` (all ConfigTests green, including the new default-60 test).

- [ ] **Step 5: Run the existing HTTP tests to confirm no regression**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~WikiWritebackServerHttpTests"
```

Expected: `Passed! - Failed: 0, Passed: 11`.

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki/Config.cs src/EAxWiki.Tests/ConfigTests.cs
git commit -m "feat(config): add ApiRateLimitPerMinute option (issue #86)"
```

---

### Task 2: HTTP tests — 429, origin-bypass, graceful shutdown

**Files:**
- Modify: `src/EAxWiki.Tests/WikiWritebackServerHttpTests.cs`
- Modify: `src/EAxWiki/WikiWritebackServer.cs:378`

**Interfaces:**
- Consumes: `Config.ApiRateLimitPerMinute` (Task 1).
- Produces: private fixture helper `private async Task<(WebApplication App, HttpClient Client)> BuildAppAsync(Config config)`; tests `RateLimit_Exceeded_Returns429`, `MismatchedOrigin_PostWithValidToken_StillSucceeds`, `Shutdown_Returns200_ThenApplicationStoppingFires`.

- [ ] **Step 1: Refactor the fixture to accept a caller-supplied Config**

In `src/EAxWiki.Tests/WikiWritebackServerHttpTests.cs`, replace the app-building block inside `InitializeAsync` (currently lines 67-76):

```csharp
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.UseTestServer();
        _app = builder.Build();

        var config = new Config { WikiPort = WikiPort, ApiPort = 8001 };
        WikiWritebackServer.Configure(_app, _reader, config, _outputDir, NullLogger.Instance);

        await _app.StartAsync();
        _client = _app.GetTestClient();
```

with:

```csharp
        (_app, _client) = await BuildAppAsync(new Config { WikiPort = WikiPort, ApiPort = 8001 });
```

Then add this private method to the class (e.g. just above the `Post` helper):

```csharp
    private async Task<(WebApplication App, HttpClient Client)> BuildAppAsync(Config config)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.UseTestServer();
        var app = builder.Build();
        WikiWritebackServer.Configure(app, _reader, config, _outputDir, NullLogger.Instance);
        await app.StartAsync();
        return (app, app.GetTestClient());
    }
```

- [ ] **Step 2: Run existing tests to verify the refactor is behavior-neutral**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~WikiWritebackServerHttpTests"
```

Expected: `Passed! - Failed: 0, Passed: 11`.

- [ ] **Step 3: Write the failing 429 test**

Add to the end of `src/EAxWiki.Tests/WikiWritebackServerHttpTests.cs` (before the closing `}` of the class):

```csharp
    [Fact]
    public async Task RateLimit_Exceeded_Returns429()
    {
        // Dispose the default (limit-60) app; build a fresh one with a tiny limit so the test
        // stays fast. The fixture's DisposeAsync later disposes these again — idempotent.
        _client.Dispose();
        await _app.DisposeAsync();
        var (app, client) = await BuildAppAsync(new Config { WikiPort = WikiPort, ApiPort = 8001, ApiRateLimitPerMinute = 3 });
        try
        {
            var body = new { elementId = 42, newStatus = "Approved", filePath = "test-element.md" };
            for (var i = 0; i < 3; i++)
            {
                var ok = await client.SendAsync(Post("/api/status", body));
                Assert.Equal(HttpStatusCode.OK, ok.StatusCode);
            }

            var limited = await client.SendAsync(Post("/api/status", body));
            Assert.Equal(HttpStatusCode.TooManyRequests, limited.StatusCode);
            Assert.Equal("60", limited.Headers.GetValues("Retry-After").Single());
        }
        finally
        {
            client.Dispose();
            await app.DisposeAsync();
        }
    }
```

- [ ] **Step 4: Run test to verify it fails**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~WikiWritebackServerHttpTests.RateLimit_Exceeded_Returns429"
```

Expected: FAIL — the 4th request returns `OK` (200) because the permit limit is still hardcoded to 60, so `Assert.Equal(HttpStatusCode.TooManyRequests, ...)` fails.

- [ ] **Step 5: Wire the config into the rate limiter**

In `src/EAxWiki/WikiWritebackServer.cs`, line 378, change:

```csharp
                PermitLimit = 60,
```

to:

```csharp
                PermitLimit = config.ApiRateLimitPerMinute,
```

(`config` is already a parameter of `Configure`.) Comment on line 374 — `// Per-token rate limiter (60 requests/minute, sliding window)` — stays; it describes the default.

- [ ] **Step 6: Run the 429 test to verify it passes**

Same command as Step 4. Expected: `Passed! - Failed: 0, Passed: 1`.

- [ ] **Step 7: Write the origin-bypass test**

Add to the end of the test class:

```csharp
    [Fact]
    public async Task MismatchedOrigin_PostWithValidToken_StillSucceeds()
    {
        // Origin/port matching only suppresses the CORS headers (browser-scoped). Token auth runs
        // regardless, so a mismatched-origin POST with a valid token succeeds — documents that
        // origin is not authentication.
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "Approved", filePath = "test-element.md" }, origin: "http://localhost:9999"));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(_reader.StatusUpdates);
    }
```

- [ ] **Step 8: Write the graceful-shutdown test**

Add to the end of the test class:

```csharp
    [Fact]
    public async Task Shutdown_Returns200_ThenApplicationStoppingFires()
    {
        var stopping = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var registration = _app.Lifetime.ApplicationStopping.Register(() => stopping.TrySetResult());

        var response = await _client.SendAsync(Post("/api/shutdown", new { }));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // The endpoint responds 200 first, then (after ~500ms) calls lifetime.StopApplication().
        // Prove the graceful-drain signal the monitor relies on actually fired.
        await stopping.Task.WaitAsync(TimeSpan.FromSeconds(5));
    }
```

- [ ] **Step 9: Run the full HTTP test class**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~WikiWritebackServerHttpTests"
```

Expected: `Passed! - Failed: 0, Passed: 14` (11 existing + 3 new).

- [ ] **Step 10: Commit**

```bash
git add src/EAxWiki/WikiWritebackServer.cs src/EAxWiki.Tests/WikiWritebackServerHttpTests.cs
git commit -m "test(api): cover rate limit, origin bypass and graceful shutdown (issue #86)"
```

---

### Task 3: STA `ExecuteWithReconnect` extraction + make reconnect live

**Files:**
- Modify: `src/EAxWiki.EA/EaReaderStaDispatcher.cs`
- Add: `src/EAxWiki.Tests/EaReaderStaDispatcherTests.cs`

**Interfaces:**
- Produces: `internal static void EaReaderStaDispatcher.ExecuteWithReconnect(Action execute, Action reconnect, Func<Exception, bool> shouldRetry, Action<Exception, int, int> onRetry, Action<Exception> onFailure, Action onHealthy, int maxRetries)`; `internal sealed class EaReaderStaDispatcher.WorkItem` with `Execute(EaReader)` that **propagates** exceptions and `SetException(Exception)`.

Background: today `WorkItem.Execute` swallows every exception (routes it to `_onError`), so the `catch (COMException …)` reconnect block in `RunStaPump` is unreachable and `_isHealthy` is always set true. This task makes the reconnect path live per the approved design.

- [ ] **Step 1: Write the failing unit tests**

Create `src/EAxWiki.Tests/EaReaderStaDispatcherTests.cs`:

```csharp
using System.Runtime.InteropServices;
using EAxWiki.EA;

namespace EAxWiki.Tests;

public class EaReaderStaDispatcherTests
{
    private sealed record RunResult(
        List<int> RetryNumbers,
        List<Exception> RetryExceptions,
        Exception? Failure,
        int HealthyCount,
        int ReconnectCount);

    private static RunResult Run(
        Action execute,
        Action? reconnect = null,
        Func<Exception, bool>? shouldRetry = null,
        int maxRetries = 1)
    {
        var retryNumbers = new List<int>();
        var retryExceptions = new List<Exception>();
        Exception? failure = null;
        var healthyCount = 0;
        var reconnectCount = 0;

        EaReaderStaDispatcher.ExecuteWithReconnect(
            execute: execute,
            reconnect: () => { reconnectCount++; reconnect?.Invoke(); },
            shouldRetry: shouldRetry ?? (ex => ex is COMException),
            onRetry: (ex, retry, _) => { retryNumbers.Add(retry); retryExceptions.Add(ex); },
            onFailure: ex => failure = ex,
            onHealthy: () => healthyCount++,
            maxRetries: maxRetries);

        return new RunResult(retryNumbers, retryExceptions, failure, healthyCount, reconnectCount);
    }

    [Fact]
    public void Execute_SuccessOnFirstAttempt_CallsHealthyOnce_NoReconnect()
    {
        var result = Run(execute: () => { });

        Assert.Empty(result.RetryNumbers);
        Assert.Null(result.Failure);
        Assert.Equal(1, result.HealthyCount);
        Assert.Equal(0, result.ReconnectCount);
    }

    [Fact]
    public void Execute_ComExceptionFirstAttempt_ReconnectsOnce_ThenHealthy()
    {
        var attempts = 0;
        var result = Run(execute: () =>
        {
            attempts++;
            if (attempts == 1) throw new COMException("EA gone");
        });

        Assert.Equal([1], result.RetryNumbers);
        Assert.Single(result.RetryExceptions);
        Assert.IsType<COMException>(result.RetryExceptions[0]);
        Assert.Null(result.Failure);
        Assert.Equal(1, result.HealthyCount);
        Assert.Equal(1, result.ReconnectCount);
        Assert.Equal(2, attempts);
    }

    [Fact]
    public void Execute_ComExceptionEveryAttempt_FailsAfterMaxRetries_NoHealthy()
    {
        var result = Run(execute: () => throw new COMException("EA gone"));

        Assert.Equal([1], result.RetryNumbers);
        Assert.IsType<COMException>(result.Failure);
        Assert.Equal(0, result.HealthyCount);
        Assert.Equal(1, result.ReconnectCount);
    }

    [Fact]
    public void Execute_NonComException_FailsWithoutReconnect()
    {
        var result = Run(execute: () => throw new InvalidOperationException("boom"));

        Assert.Empty(result.RetryNumbers);
        Assert.IsType<InvalidOperationException>(result.Failure);
        Assert.Equal(0, result.HealthyCount);
        Assert.Equal(0, result.ReconnectCount);
    }

    [Fact]
    public void Execute_ReconnectThrows_FailsWithReconnectException()
    {
        var reconnectError = new InvalidOperationException("reconnect failed");
        var result = Run(
            execute: () => throw new COMException("EA gone"),
            reconnect: () => throw reconnectError);

        Assert.Equal([1], result.RetryNumbers);
        Assert.Same(reconnectError, result.Failure);
        Assert.Equal(0, result.HealthyCount);
        Assert.Equal(1, result.ReconnectCount);
    }

    [Fact]
    public void Dispatch_ComException_IsRetriedThenSucceeds()
    {
        // Regression test for the dead-code bug: WorkItem.Execute must PROPAGATE COMException to
        // the pump's ExecuteWithReconnect (so the reconnect path can run) instead of swallowing it
        // and routing straight to the caller's _onError. Models the exact production wiring:
        //   execute: () => work.Execute(reader)
        //   onFailure: ex => work.SetException(ex)
        var attempts = 0;
        var routedToCaller = new List<Exception>();
        var work = new EaReaderStaDispatcher.WorkItem(
            _ => { attempts++; if (attempts == 1) throw new COMException("EA gone"); },
            ex => routedToCaller.Add(ex));

        var reconnectCount = 0;
        var healthyCount = 0;

        EaReaderStaDispatcher.ExecuteWithReconnect(
            execute: () => work.Execute(null!),
            reconnect: () => reconnectCount++,
            shouldRetry: ex => ex is COMException,
            onRetry: (_, _, _) => { },
            onFailure: ex => throw new Xunit.Sdk.XunitException($"unexpected failure: {ex}"),
            onHealthy: () => healthyCount++,
            maxRetries: 1);

        Assert.Equal(2, attempts);
        Assert.Equal(1, reconnectCount);
        Assert.Equal(1, healthyCount);
        Assert.Empty(routedToCaller);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~EaReaderStaDispatcherTests"
```

Expected: compile error — `EaReaderStaDispatcher` does not contain a definition for `ExecuteWithReconnect`, and `EaReaderStaDispatcher.WorkItem` is inaccessible due to its protection level.

- [ ] **Step 3: Fix `WorkItem` so it propagates exceptions, and make it `internal`**

In `src/EAxWiki.EA/EaReaderStaDispatcher.cs`, replace the whole `WorkItem` class (current lines 249-267):

```csharp
    private sealed class WorkItem
    {
        private readonly Action<EaReader> _execute;
        private readonly Action<Exception> _onError;

        public WorkItem(Action<EaReader> execute, Action<Exception> onError)
        {
            _execute = execute;
            _onError = onError;
        }

        public void Execute(EaReader reader)
        {
            try { _execute(reader); }
            catch (Exception ex) { _onError(ex); }
        }

        public void SetException(Exception ex) => _onError(ex);
    }
```

with:

```csharp
    internal sealed class WorkItem
    {
        private readonly Action<EaReader> _execute;
        private readonly Action<Exception> _onError;

        public WorkItem(Action<EaReader> execute, Action<Exception> onError)
        {
            _execute = execute;
            _onError = onError;
        }

        // Deliberately does NOT swallow exceptions: RunStaPump's ExecuteWithReconnect needs to
        // observe COMException to trigger a reconnect. The pump routes non-retryable failures via
        // SetException.
        public void Execute(EaReader reader) => _execute(reader);

        public void SetException(Exception ex) => _onError(ex);
    }
```

- [ ] **Step 4: Add `OpenNewReader` and `ExecuteWithReconnect`**

In `src/EAxWiki.EA/EaReaderStaDispatcher.cs`, immediately after the `RunStaPump` method (after its closing brace, currently line 128), insert:

```csharp
    private EaReader OpenNewReader(string repositoryPath)
    {
        var newReader = new EaReader(_logger as ILogger<EaReader>);
        newReader.Open(repositoryPath);
        return newReader;
    }

    // The work-item retry loop from RunStaPump, extracted so the reconnect semantics are
    // unit-testable without a live EA repository. Semantics identical to the original loop:
    //   * up to maxRetries + 1 attempts total
    //   * on shouldRetry(ex) with retries remaining -> onRetry(ex, retryNumber, maxRetries),
    //     then reconnect(); if reconnect throws -> onFailure(reconnectEx) and stop
    //   * on non-retryable exception or retries exhausted -> onFailure(ex) and stop
    //   * on success -> onHealthy() and stop
    internal static void ExecuteWithReconnect(
        Action execute,
        Action reconnect,
        Func<Exception, bool> shouldRetry,
        Action<Exception, int, int> onRetry,
        Action<Exception> onFailure,
        Action onHealthy,
        int maxRetries)
    {
        var retries = 0;
        var executed = false;
        while (!executed && retries <= maxRetries)
        {
            try
            {
                execute();
                onHealthy();
                executed = true;
            }
            catch (Exception ex) when (shouldRetry(ex) && retries < maxRetries)
            {
                retries++;
                onRetry(ex, retries, maxRetries);
                try
                {
                    reconnect();
                }
                catch (Exception reconnectEx)
                {
                    onFailure(reconnectEx);
                    executed = true;
                }
            }
            catch (Exception ex)
            {
                onFailure(ex);
                executed = true;
            }
        }
    }
```

- [ ] **Step 5: Rewrite `RunStaPump` to use the helper**

In `src/EAxWiki.EA/EaReaderStaDispatcher.cs`, replace the current `RunStaPump` body (lines 57-128). Change the initial open to use `OpenNewReader`:

```csharp
    private void RunStaPump(string repositoryPath)
    {
        EaReader? reader = null;
        try
        {
            reader = OpenNewReader(repositoryPath);
            _isHealthy = true;
        }
        catch (Exception ex)
        {
            _initException = ex;
            return;
        }
        finally
        {
            _initComplete.Set();
        }

        if (_initException != null) return;

        try
        {
            foreach (var work in _workQueue.GetConsumingEnumerable())
            {
                ExecuteWithReconnect(
                    execute: () => work.Execute(reader!),
                    reconnect: () =>
                    {
                        reader!.Dispose();
                        reader = OpenNewReader(repositoryPath);
                        _logger.LogInformation("EA reconnection succeeded.");
                    },
                    shouldRetry: ex => ex is COMException && !_disposed,
                    onRetry: (ex, retries, maxRetries) =>
                    {
                        _isHealthy = false;
                        _logger.LogWarning(ex, "EA COM disconnected (retry {Retry}/{MaxRetries}); reconnecting.", retries, maxRetries);
                    },
                    onFailure: ex => work.SetException(ex),
                    onHealthy: () => _isHealthy = true,
                    maxRetries: 1);
            }
        }
        finally
        {
            reader?.Dispose();
        }
    }
```

Notes:
- `reconnect` runs only while `reader` is non-null (it is assigned before the loop starts), so `reader!` is safe.
- `_isHealthy` starts false on a retryable COMException (`onRetry`) and flips true only after a successful work item (`onHealthy`) — matching the original intent (reconnect does not itself prove queryability).
- `_disposed` is read from the dispatcher instance inside the `shouldRetry` closure — same guard as the original `catch (... ) when (!_disposed && retries < maxRetries)`.

- [ ] **Step 6: Run the STA tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~EaReaderStaDispatcherTests"
```

Expected: `Passed! - Failed: 0, Passed: 6`.

- [ ] **Step 7: Run the full .NET suite**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet
```

Expected: `Passed! - Failed: 0, Passed: 311` (301 existing + 1 Config + 3 HTTP + 6 STA). If the actual count differs from 311, record the real number in the report; the requirement is all pass with zero failures.

- [ ] **Step 8: Commit**

```bash
git add src/EAxWiki.EA/EaReaderStaDispatcher.cs src/EAxWiki.Tests/EaReaderStaDispatcherTests.cs
git commit -m "fix(ea): make STA COM reconnect live via ExecuteWithReconnect (issue #86)"
```

---

### Task 4: Final verification (both suites)

**Files:** none (verification only).

- [ ] **Step 1: Run the full .NET suite**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet
```

Expected: `Passed! - Failed: 0` (see count note in Task 3 Step 7).

- [ ] **Step 2: Run the full Pester suite**

Pester suite must be run from a shell where port 8000 is held by a `TcpListener` and 8001 is free (same recipe used for issue #86 part 1):

```powershell
$l = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, 8000); $l.Start()
try {
    Invoke-Pester -Path tests -Output Detailed
}
finally { $l.Stop() }
```

Run from repo root (`E:\Users\Han\Repos\EAxWiki`). Expected: `Passed: 162, Failed: 0, Skipped: 0`.

- [ ] **Step 3: Confirm working tree is clean of runtime artifacts**

```powershell
git status --short
```

Expected: only the three expected commits and no `model/`, `wiki/`, `.eaxwiki-monitor/`, or `wiki/status/` entries. If any runtime artifact shows as modified/untracked, restore or delete it (see repo conventions) — do NOT commit it.

---

## Self-Review Notes

- Spec coverage: rate-limit 429 → Task 2 (config-driven permit limit, TDD); origin mismatch → Task 2 `MismatchedOrigin_PostWithValidToken_StillSucceeds`; shutdown 200-then-stop → Task 2 `Shutdown_Returns200_ThenApplicationStoppingFires`; STA reconnect extraction + no-EA tests → Task 3; config default 60 + no CLI flag → Task 1; no public API change except `Config.ApiRateLimitPerMinute`; no behavior change for default config (permit limit 60 identical).
- `onRetry` carries `(ex, retryNumber, maxRetries)` so the caller reproduces the original log message values.
- Test counts: 301 existing .NET tests + 10 new = 311. Pester unchanged at 162. (Task 3 Step 7 and Task 4 flag this as a verify-in-report item rather than an exact assertion.)
- Existing `EaReaderStaDispatcherTests` namespace is `EAxWiki.Tests`; `using EAxWiki.EA;` gives access to the `internal` members via the existing `InternalsVisibleTo("EAxWiki.Tests")` in `EAxWiki.EA.csproj`.

## Execution Handoff

Plan complete and saved to `docs/superpowers/plans/2026-08-13-issue-86-writeback-server-tests.md`. Two execution options:

1. **Subagent-Driven (recommended)** — I dispatch a fresh subagent per task, review between tasks, fast iteration.

2. **Inline Execution** — execute tasks in this session using executing-plans, batch execution with checkpoints.

Which approach?
