# Write-back pattern consolidation (issue #86, part 1) — design

_2026-08-13_

## Goal

Remove the duplicated write-back logic in two places without changing any observable behavior:

1. **`EaReader.cs`** — seven `Update…` methods all repeat the same six-step shape (null-check repository → locate entity → null-check → set field → `Update()` → `RefreshModelView(0)` → re-read → `VerifyWrite` → log). Four of them are ID-based; three additionally search a parent element's COM collection by a composite key.
2. **`FrontmatterParser.cs`** — four `Update…` methods all repeat the same read → detect EOL → normalize to `\n` → regex-transform → bump `Modified:` → restore EOL → atomic temp-file swap. `UpdateNotes` and `UpdatePackageNotes` are near-identical (only the marker names differ).

This is a **pure internal refactor**: the public contracts (`IEaReader`, the four `FrontmatterParser.Update*` methods) and every caller (`EaReaderStaDispatcher`, `WikiWritebackServer`, `WriteBackScanner`, `FakeEaReader`) stay untouched.

## EaReader: one skeleton helper + one collection-search helper

### `Write<TEntity>`

A single private delegate-driven helper. Its signature deliberately contains **no EA COM types**, so the shared skeleton is plain C#:

```csharp
private void Write<TEntity>(
    Func<TEntity> locate,       // throws InvalidOperationException with a descriptive message when not found
    Action<TEntity> apply,      // sets the field(s), then calls entity.Update()
    Func<string?> readBack,     // re-reads the written value for VerifyWrite
    string expected,            // the value that was written
    string entityDescription,   // VerifyWrite label, e.g. "element 42 Status"
    string successTemplate,     // LogInformation message template
    params object[] successArgs)
{
    if (_repository == null) throw new InvalidOperationException("Repository is not open.");
    var entity = locate();
    apply(entity);
    _repository.RefreshModelView(0);
    var actual = readBack() ?? string.Empty;
    VerifyWrite(_logger, entityDescription, expected, actual);
    _logger?.LogInformation(successTemplate, successArgs);
}
```

### `Find<T>`

A generic first-match collection search for the three composite-key methods:

```csharp
private static (T? match, int count) Find<T>(EA.Collection collection, Func<T, bool> predicate) where T : class
{
    T? match = null; var count = 0;
    for (short i = 0; i < collection.Count; i++)
        if (collection.GetAt(i) is T item && predicate(item)) { count++; match ??= item; }
    return (match, count);
}
```

### Per-method shape

- **ID-based** (`UpdateElementStatus`, `UpdateElementNotes`, `UpdatePackageNotes`, `UpdateDiagramNotes`) become ~8 lines: a `locate` closure that does `GetXByID(id) ?? throw …`, an `apply` closure that sets the field and calls `Update()`, and a `readBack` closure that re-reads the value.
- **Collection-based** (`UpdateAttributeNotes`, `UpdateMethodNotes`, `UpdateTaggedValueNotes`) keep only their key-matching predicate. `locate` does: get parent element (throw if missing) → get collection (throw `"… has no … collection"` if absent) → `Find<T>` with the predicate → log the duplicate warning when `count > 1` → throw `"… not found …"` when no match. `readBack` re-runs `Find<T>` on the refreshed element.

All exception messages, the `VerifyWrite` labels, the duplicate-matching `LogWarning` text, and the `LogInformation` templates are byte-identical to today.

## FrontmatterParser: one rewrite helper + one notes-like helper

### `RewriteMarkdown`

Shared tail with an explicit no-op contract so today's early-return behavior is preserved exactly:

```csharp
// Reads the file, normalizes to \n, applies the transform, restores the original EOL,
// and atomically swaps via a temp file. A null return from the transform aborts without writing —
// preserving today's "no frontmatter → file untouched" behavior exactly.
private static void RewriteMarkdown(string filePath, Func<string, string?> transform)
{
    var original = File.ReadAllText(filePath);
    var usesCrlf = original.Contains("\r\n");
    var text = transform(original.Replace("\r\n", "\n"));
    if (text is null) return;
    if (usesCrlf) text = text.Replace("\n", "\r\n");
    var tmp = filePath + ".tmp";
    File.WriteAllText(tmp, text);
    File.Move(tmp, filePath, overwrite: true);
}
```

### All four `Update*` methods route through it

- **`UpdateStatus`** — keeps its line-based frontmatter/badge/widget transform verbatim, wrapped as the transform; returns `null` when the leading `---` block is missing or has no closing delimiter (the current early-returns).
- **`UpdateNotes` / `UpdatePackageNotes`** — genuinely identical modulo markers, so they share one further private helper:
  `UpdateNotesLike(filePath, newNotesHtml, startMarker, endMarker)` — does the `notes_hash` swap-or-append, the content-marker swap, and the `Modified:` bump. The two public methods become one-liners.
- **`UpdateRowNotes`** — uses `RewriteMarkdown` directly; its hash lives in `data-notes-hash` on the row tag, not in frontmatter, so it does not fit the notes-like shape.

Every regex, the first-match-only `Replace(…, 1)` semantics, the `notes_hash` append-when-missing branch, and the atomic swap are unchanged — only the surrounding boilerplate is centralized.

## Behavior-preservation guarantees

- No change to the `IEaReader` interface, `EaReaderStaDispatcher`, `WikiWritebackServer`, `WriteBackScanner`, or `FakeEaReader`.
- No change to the `FrontmatterParser` public surface.
- No change to exception types/messages, warning text, or log messages.
- No change to CRLF preservation, atomicity, or the `Modified:` bump behavior.
- `UpdateStatus` no-op paths (no/malformed frontmatter) still write nothing.

## Acceptance criteria

- `dotnet build src/EAxWiki` clean, no new warnings.
- All existing tests pass: .NET 298 (incl. `WikiWritebackServerHttpTests` via `FakeEaReader`) and Pester 162.
- One new `FrontmatterParserTests` case for `UpdatePackageNotes` (the only `Update*` method currently without a direct unit test), asserting hash + content + `Modified:` swap and no-op on missing frontmatter.
- Final gate (optional, needs EA): the write-back smoke test still round-trips a status, notes, and row-notes change.

## Out of scope

- Issue #86 parts 2–4 (remaining HTTP test gaps, C# `--monitor` mode, System.CommandLine).
- Adding happy-path unit tests for the `EaReader` COM write path (delegates are the seam; not required this round — the smoke suite stays the safety net for EA behavior).
- Refactoring the `Extract*` read methods (no duplication).
