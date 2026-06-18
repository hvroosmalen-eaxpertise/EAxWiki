# Logging & Performance Design

## Summary

Add structured logging via `Microsoft.Extensions.Logging` to `MarkdownExporter` to identify performance hot spots, plus targeted performance fixes for known slow paths.

## Approach

**Approach B** — structured logging with optional `--verbose` flag for granular diagnostics.

## Logging Wire-up

### Dependencies
- `Microsoft.Extensions.Logging.Abstractions` (v10.0.9)
- `Microsoft.Extensions.Logging` (v10.0.9)
- `Microsoft.Extensions.Logging.Console` (v10.0.9)

### Program.cs changes
- Create a `LoggerFactory` via `LoggerFactory.Create(builder => ...)` with `AddConsole()` and minimum log level based on `--verbose`
- Build an `ILogger<MarkdownExporter>` from the factory
- Pass it to `new MarkdownExporter(writer, logger)`

### Config.cs changes
- Add `bool Verbose { get; set; }` property
- Parse `--verbose` / `-v` flag

### MarkdownExporter changes
- Constructor takes `ILogger<MarkdownExporter> logger`
- Store as `_logger`

## Logging Plan

### Information level (always on)
| Location | Message | Data |
|----------|---------|------|
| `ExportPackageAsync` start | `"Exporting package {PackageName} ({ElementCount} elements, {DiagramCount} diagrams)"` | package name, element count, diagram count |
| `ExportPackageAsync` end | `"Exported package {PackageName} in {ElapsedMs}ms"` | elapsed ms |
| `GenerateTypesPagesAsync` start | `"Generating {StereotypeCount} type pages"` | count of stereotypes |
| `GenerateTypesPagesAsync` end | `"Generated {StereotypeCount} type pages in {ElapsedMs}ms"` | elapsed ms |
| `ExportDiagramsAsync` start | `"Exporting {DiagramCount} diagrams with PNG images"` | diagram count |
| Per-diagram | `"Exported diagram {DiagramName} in {ElapsedMs}ms"` | diagram name, elapsed ms |
| `ExportAsync` end | `"Export complete: {TotalElapsedMs}ms total"` | total ms |

### Debug level (--verbose only)
| Location | Message | Data |
|----------|---------|------|
| `WriteElementAsync` | `"Writing element {ElementName}"` | element name |

## Performance Fixes

### 1. FileOutputWriter async
Changed `File.WriteAllText` to `File.WriteAllTextAsync` with `await`.

### 2. SanitizeName cache
- `static readonly char[] _invalidChars` — created once, not per call
- `static ConcurrentDictionary<string, string> _sanitizeCache` — memoizes results

### 3. GenerateTypesPagesAsync — group by stereotype once
Replaced O(N×M) `.Where()` per stereotype with single `ToLookup()` pass (O(N)).

## Files Changed

| File | Change |
|------|--------|
| `EAxWiki.Export.csproj` | Added `Microsoft.Extensions.Logging.Abstractions` package ref |
| `EAxWiki.csproj` | Added `Microsoft.Extensions.Logging` + `.Console` package refs |
| `src/EAxWiki/Config.cs` | Added `Verbose` property + `--verbose`/`-v` parsing |
| `src/EAxWiki/Program.cs` | Build LoggerFactory, create logger, pass to exporter |
| `src/EAxWiki.Export/MarkdownExporter.cs` | Constructor injection of logger, add timing/logging calls, fix SanitizeName, fix types grouping |
| `src/EAxWiki.Export/FileOutputWriter.cs` | Changed `File.WriteAllText` to `File.WriteAllTextAsync` |
