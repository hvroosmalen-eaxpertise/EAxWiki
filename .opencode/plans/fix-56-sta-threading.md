# Fix #56: STA threading for write-back COM access

## Problem

`WikiWritebackServer.cs` creates `EaReader` on the **main thread** but all EA COM calls (`GetStatusTypes`, `UpdateElementStatus`, etc.) happen on **ASP.NET thread-pool threads** (MTA by default) inside request handler lambdas. EA COM requires STA. This causes `RPC_E_WRONG_THREAD` crashes, silent state corruption, and intermittent hangs.

## Changes

### 1. New file: `src/EAxWiki.EA/EaReaderStaDispatcher.cs`

Wraps `EaReader` and routes all COM calls through a dedicated STA thread.

**Design:**
- Constructor takes `ILogger` + `repositoryPath`, creates a `Thread` with `SetApartmentState(ApartmentState.STA)`, starts it
- STA thread pump: initializes `EaReader` + opens repository, then uses `BlockingCollection<WorkItem>.GetConsumingEnumerable()` to process work items sequentially
- `Dispatch<T>(Func<EaReader, T>)` queues a `WorkItem` to the `BlockingCollection` and blocks the caller via `TaskCompletionSource<T>.Task.GetAwaiter().GetResult()` — safe because caller is on MTA thread-pool with no `SynchronizationContext`
- `DispatchVoid(Action<EaReader>)` same pattern but for void-returning methods
- Implements `IEaReader` — all methods delegate through `Dispatch`/`DispatchVoid`
- `Dispose()` calls `CompleteAdding()` to unblock the STA thread pump, joins with 5s timeout, then disposes the reader
- `Open()`, `Close()`, `RepositoryPath` throw `NotSupportedException` (dispatcher handles these internally)

**NuGet dependencies:** None (uses `System.Collections.Concurrent` in-box).

### 2. Modify: `src/EAxWiki/WikiWritebackServer.cs`

Lines 66-67:
```diff
- var reader = new EaReader(loggerFactory.CreateLogger<EaReader>());
- reader.Open(config.RepositoryPath);
+ var reader = new EaReaderStaDispatcher(loggerFactory.CreateLogger<EaReader>(), config.RepositoryPath);
```

Add `using EAxWiki.EA;` if not already present (it is — line 3).

All `reader.*` calls and `reader.Dispose()` in the `finally` block remain unchanged since the dispatcher implements `IEaReader`.

## Safety

- **No deadlock**: `Task.GetAwaiter().GetResult()` blocks an MTA thread-pool thread; the STA thread runs independently. No `SynchronizationContext` involved.
- **Serialized COM access**: `BlockingCollection` with `SingleReader`-equivalent semantics (the STA pump processes one item at a time).
- **Composability**: `TaskCreationOptions.RunContinuationsAsynchronously` ensures the STA thread never runs the continuation.
- **Export path unaffected**: `Program.cs` export mode still uses `EaReader` directly (it runs synchronously on one thread).
- **Interface unchanged**: `IEaReader` stays the same — no breaking changes.

## Verification

1. `dotnet build src\EAxWiki.Tests\EAxWiki.Tests.csproj` — must succeed
2. `dotnet test src\EAxWiki.Tests\EAxWiki.Tests.csproj` — all existing tests must pass (the dispatcher is not unit-testable without EA installed)
