# Parallel Element Export Design

## Summary
Parallelize element markdown file writes to reduce export time by overlapping I/O.

## Change
Replace sequential `await WriteElementAsync` inside the foreach loop with `Task.WhenAll` over collected tasks. `elements.Add` and index line building stay sequential on the original thread.

## Files modified
- `src/EAxWiki.Export/MarkdownExporter.cs` — element loop in `ExportPackageAsync`

## No changes needed
- `IOutputWriter` — concurrent writes to different files are safe
- `SanitizeName` — already thread-safe via `ConcurrentDictionary`
- `_logger.LogDebug` — MEL is thread-safe
- `elements` list — `Add` calls on the original thread, not concurrent
- `indexLines` — built sequentially as before

## Error handling
Wrap `Task.WhenAll` in try-catch. `AggregateException` rethrown, caught by `ExportAsync`'s outer catch. Fail-fast behavior preserved.

## Edge cases
- Package with 0 elements → `Task.WhenAll(empty)` returns immediately
- Elements with long notes or deep tagged values → still parallelized
