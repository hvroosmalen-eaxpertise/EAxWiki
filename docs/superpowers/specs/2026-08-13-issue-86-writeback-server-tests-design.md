# Write-back Server HTTP & STA Reconnect Test Design

> Issue #86 part 2. Status: approved design.

**Goal:** Close the remaining test gaps around `WikiWritebackServer` and `EaReaderStaDispatcher`:
HTTP-level tests for the rate limiter (429) and graceful shutdown, plus a unit test for the
STA dispatcher's COMException-reconnect logic — all without a live EA repository.

**Existing coverage (commit `3012ecff3`, `WikiWritebackServerHttpTests.cs`, 11 tests):**
missing/wrong token → 401 (no token leak in body), valid status write → 200 + recorded,
unknown status → 400, missing file → 404, path traversal → 400, `/readyz` healthy → 200 /
unhealthy → 503, CORS header echoed on matching origin, CORS header suppressed on mismatched
port.

**Not covered today:** rate-limit 429, `/api/shutdown` graceful drain, STA reconnect.

## Global Constraints

- No change to public API: `IEaReader`, `EaReaderStaDispatcher` public surface, `WikiWritebackServer`
  public surface, `FakeEaReader`.
- No change to default production behavior: `Config.ApiRateLimitPerMinute` defaults to 60,
  identical to today's hardcoded limit. The `/api/shutdown` response-then-stop ordering is unchanged.
- No new dependencies. Tests use the existing `Microsoft.AspNetCore.TestHost` infrastructure.
- CRLF / file conventions preserved. New/changed files: LF, UTF-8 (no BOM).
- Every `dotnet test` command requires `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\';`
  because the EA test project conditionally references `Interop.EA.dll`.
- All existing tests stay green: 301 .NET + 162 Pester. New tests add to the .NET count.
- Working-tree hygiene: `model/`, `wiki/`, `.eaxwiki-monitor/audit.log`,
  `wiki/status/writeback.log` are runtime artifacts and must never be staged or committed.

## Scope

| Gap | Approach |
|-----|----------|
| Origin/port mismatch | Test current behavior only — no production change. Existing `CorsHeader_OnMismatchedPort_IsNotSet` covers header suppression; add one clarifying test that a mismatched-origin POST with a valid token still succeeds (token is the auth; CORS is browser-only). |
| Rate-limit 429 | Make the permit limit configurable via `Config.ApiRateLimitPerMinute` (default 60); test sets it low (e.g. 3) and fires 4 requests. |
| `/api/shutdown` | Verify 200 is returned first, then `IApplicationLifetime.ApplicationStopping` fires (TCS hook). |
| STA reconnect | Extract the retry loop into an internal static delegate-driven `ExecuteWithReconnect`, unit-test with delegates (no EA). |

---

## Component 1: Configurable rate limit

**`src/EAxWiki/Config.cs`** — add property:

```csharp
public int ApiRateLimitPerMinute { get; set; } = 60;
```

No CLI flag in this part (issue part 4's System.CommandLine work can expose it later). Tests
set the property directly on the `Config` passed to `Configure`.

**`src/EAxWiki/WikiWritebackServer.cs:376`** — replace the hardcoded limit:

```csharp
PermitLimit = config.ApiRateLimitPerMinute,
```

Default path (Config default 60) is byte-for-byte the current behavior.

## Component 2: HTTP tests (`WikiWritebackServerHttpTests.cs`)

The fixture currently builds its app in `InitializeAsync` with a fixed `Config { WikiPort = 8000, ApiPort = 8001 }`.
Refactor: extract the app-build into a private helper `BuildApp(Config config, ...)` (or add an
optional config parameter to `InitializeAsync`) so tests can pass an overridden `Config`. The
default initialization keeps the existing 11 tests unchanged.

New tests:

1. `RateLimit_Exceeded_Returns429` — build app with `ApiRateLimitPerMinute = 3`; fire 4 valid
   POSTs to `/api/status` with the real token; first 3 → 200, 4th → 429 with `Retry-After: 60`.
2. `MismatchedOrigin_PostWithValidToken_StillSucceeds` — POST with `Origin: http://localhost:9999`
   and a valid token → 200. Documents that origin matching is CORS-only, not authentication.
3. `Shutdown_Returns200_ThenApplicationStoppingFires` — register a `TaskCompletionSource` on
   `_app.Lifetime.ApplicationStopping`; POST `/api/shutdown`; assert 200; await the TCS with a
   5s timeout.

## Component 3: STA `ExecuteWithReconnect` extraction

**`src/EAxWiki.EA/EaReaderStaDispatcher.cs`** — extract the retry/reconnect loop from
`RunStaPump` (current lines ~80-121) into an internal static helper:

```csharp
internal static void ExecuteWithReconnect(
    Action execute,               // work.Execute(reader) — captured closure in RunStaPump
    Action reconnect,             // dispose + create + open new reader; throws on failure
    Func<Exception, bool> shouldRetry,   // ex is COMException && !_disposed
    Action<Exception, int, int> onRetry, // (ex, retryNumber, maxRetries) — _isHealthy = false + LogWarning
    Action<Exception> onFailure,  // work.SetException
    Action onHealthy,             // _isHealthy = true
    int maxRetries)
```

Semantics (identical to today's loop):

- Loop up to `maxRetries + 1` attempts.
- On `shouldRetry(ex)` with retries remaining → `onRetry(ex, retryNumber, maxRetries)` then
  `reconnect()`; if reconnect throws → `onFailure(reconnectEx)` and stop.
- On non-retryable exception or retries exhausted → `onFailure(ex)` and stop.
- On success → `onHealthy()` and stop.

`RunStaPump`'s `foreach` body becomes a single call to `ExecuteWithReconnect` with closures:

```csharp
ExecuteWithReconnect(
    execute: () => work.Execute(reader!),
    reconnect: () => { reader!.Dispose(); reader = OpenNewReader(repositoryPath); },
    shouldRetry: ex => ex is COMException && !_disposed,
    onRetry: (ex, retry, maxRetries) =>
    {
        _isHealthy = false;
        _logger.LogWarning(ex, "EA COM disconnected (retry {Retry}/{MaxRetries}); reconnecting.", retry, maxRetries);
    },
    onFailure: ex => work.SetException(ex),
    onHealthy: () => _isHealthy = true,
    maxRetries: 1);
```

The helper passes the 1-based `retryNumber` and `maxRetries` into `onRetry` so the caller can
reproduce the exact `"EA COM disconnected (retry {Retry}/{MaxRetries}); reconnecting."` log
message with the same values the loop used today.

`OpenNewReader` is a small private helper holding the current `new EaReader(...)` + `Open(...)`
logic, so the log messages and reader lifecycle are byte-identical to today. The captured
`reader` local keeps the shared-reader semantics: one reader reused across work items, replaced
only on reconnect. `finally { reader?.Dispose(); }` remains in `RunStaPump`.

**`src/EAxWiki.Tests/EaReaderStaDispatcherTests.cs`** (new) — five tests, no EA, no COM:

1. `Execute_SuccessOnFirstAttempt_CallsHealthyOnce_NoReconnect`
2. `Execute_ComExceptionFirstAttempt_ReconnectsOnce_ThenHealthy`
3. `Execute_ComExceptionEveryAttempt_FailsAfterMaxRetries_NoHealthy`
4. `Execute_NonComException_FailsWithoutReconnect`
5. `Execute_ReconnectThrows_FailsWithReconnectException`

The helper is `internal`; `EAxWiki.EA.csproj` already has `InternalsVisibleTo` for the test
assembly.

## Acceptance Criteria

- Build clean, no new warnings.
- New tests green: 3 HTTP + 5 STA (11 existing HTTP tests remain green).
- Full .NET suite: 301 → 311 passing (10 new tests: 1 Config default, 3 HTTP, 6 STA — the dispatch regression test was added per the approved "make the reconnect path live" decision).
- Pester suite: 162 passing, unchanged.
- No production behavior change for default config; no public API change.
- Optional live smoke gate (real EA): can run the existing writeback smoke test; if skipped,
  state so in the task report.

## Files Touched

- Modify: `src/EAxWiki/Config.cs` (+1 property)
- Modify: `src/EAxWiki/WikiWritebackServer.cs` (permit limit wiring; no other change)
- Modify: `src/EAxWiki.EA/EaReaderStaDispatcher.cs` (extract helper, add `OpenNewReader`)
- Modify: `src/EAxWiki.Tests/WikiWritebackServerHttpTests.cs` (+3 tests, fixture refactor)
- Add: `src/EAxWiki.Tests/EaReaderStaDispatcherTests.cs` (5 tests)

## Out of Scope

- Origin-mismatch 403 (issue text mentions it; the server's design deliberately has no 403 —
  CORS header suppression is the browser-scoped control, token is the auth).
- Real-Kestrel / in-flight-request drain verification (covered by Pester smoke tests).
- CLI flag for the rate limit (deferred to issue part 4).
- Anything from issue parts 3/4 (C# monitor, System.CommandLine).