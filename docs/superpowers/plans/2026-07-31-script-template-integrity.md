# Script Template Integrity Verification Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add xUnit tests that fail the build if any core exporter script/template loses its key functions, and remove the dead `ai-suggest.js` stub.

**Architecture:** A new `ScriptTemplateIntegrityTests` class in `EAxWiki.Tests` runs the full exporter against an in-memory output (no EA/COM) and asserts each generated script contains stable marker strings (function names, API paths, CSS selectors). `InMemoryWriter` is extracted from `ExportIntegrationTests` into a shared `TestInMemoryWriter` so both classes reuse it. The dead `WriteAiSuggestScriptAsync` stub and its `MarkdownExporter` call are removed; `AiSuggestJs_IsNotEmitted` guards against re-introduction.

**Tech Stack:** C#, xUnit, `MarkdownExporter` + `IOutputWriter` in-memory pattern (no EA/COM needed), `dotnet test`.

## Global Constraints

- Test count baseline: 261 passing before this change; 268 after (261 + 7 new).
- No EA/COM access — exporter must run against an in-memory `IOutputWriter` with a minimal `EaRepository`.
- Markers are stable identifiers, not full-script equality (resilient to formatting changes).
- Source changes limited to removing the dead `ai-suggest.js` stub; no runtime script behavior changes.
- Follow existing repo conventions: file-scoped namespaces, `private static` helpers, no comments unless required.
- Commits only after green tests; conventional commit messages (`feat:`, `test:`, `refactor:`).

---

## File Structure

| File | Responsibility |
|------|---------------|
| `src/EAxWiki.Tests/TestInMemoryWriter.cs` | NEW — shared in-memory `IOutputWriter` (extracted from `ExportIntegrationTests`) |
| `src/EAxWiki.Tests/ExportIntegrationTests.cs` | MODIFY — remove private `InMemoryWriter` class, use shared `TestInMemoryWriter` |
| `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` | NEW — marker-based integrity checks for all exported scripts/templates |
| `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` | MODIFY — delete `WriteAiSuggestScriptAsync` (lines 1015-1021) |
| `src/EAxWiki.Export/MarkdownExporter.cs` | MODIFY — delete `WriteAiSuggestScriptAsync` call (line 103) |
| `wiki/ai-suggest.js` | DELETE — stale tracked file, no longer produced |

---

### Task 1: Extract shared `TestInMemoryWriter`

**Files:**
- Create: `src/EAxWiki.Tests/TestInMemoryWriter.cs`
- Modify: `src/EAxWiki.Tests/ExportIntegrationTests.cs` (remove nested class lines 11-20; drop now-unused `using EAxWiki.Core.Interfaces;` and `using System.Threading;`)

**Interfaces:**
- Produces: `internal sealed class TestInMemoryWriter : IOutputWriter` with `public readonly Dictionary<string, string> Files` (keyed by forward-slash-normalized path) and `public readonly HashSet<string> Directories`; `WriteFileAsync(string filePath, string content, CancellationToken)` stores into `Files`.

- [ ] **Step 1: Create `TestInMemoryWriter.cs`**

```csharp
using EAxWiki.Core.Interfaces;

namespace EAxWiki.Tests;

internal sealed class TestInMemoryWriter : IOutputWriter
{
    public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
    public readonly HashSet<string> Directories = new(StringComparer.OrdinalIgnoreCase);

    public Task CreateDirectoryAsync(string path, CancellationToken ct = default) { Directories.Add(path); return Task.CompletedTask; }
    public Task WriteFileAsync(string filePath, string content, CancellationToken ct = default) { Files[Normalize(filePath)] = content; return Task.CompletedTask; }

    private static string Normalize(string path) => path.Replace('\\', '/');
}
```

- [ ] **Step 2: Remove the nested `InMemoryWriter` from `ExportIntegrationTests.cs`**

Delete lines 11-20 (the `private sealed class InMemoryWriter : IOutputWriter { ... }` block). Replace all 7 occurrences of `new InMemoryWriter()` with `new TestInMemoryWriter()`. Remove `using EAxWiki.Core.Interfaces;` (line 1) and `using System.Threading;` (line 5) — both only existed for the nested class.

- [ ] **Step 3: Verify full suite still green**

Run: `dotnet test "src\EAxWiki.Tests"` from repo root
Expected: `Passed! - Failed: 0, Passed: 261, Skipped: 0, Total: 261`

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.Tests/TestInMemoryWriter.cs src/EAxWiki.Tests/ExportIntegrationTests.cs
git commit -m "refactor: extract shared TestInMemoryWriter for exporter tests"
```

---

### Task 2: Add `ScriptTemplateIntegrityTests` with positive marker checks

**Files:**
- Create: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`

**Interfaces:**
- Consumes: `TestInMemoryWriter` (Task 1), `MarkdownExporter`, `EaRepository`/`EaPackage`/`EaElement` from `EAxWiki.Core.Models`, `NullLogger<MarkdownExporter>`.
- Produces: `ScriptTemplateIntegrityTests` with 6 `[Fact]` tests asserting markers for `notes-editor.js`, `status-editor.js`, `row-notes-editor.js`, `graph-init.js`, `extra.css`, `cytoscape.min.js`.

- [ ] **Step 1: Write the test class**

```csharp
using EAxWiki.Core.Models;
using EAxWiki.Export;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class ScriptTemplateIntegrityTests
{
    private static EaRepository MinimalRepository()
    {
        var element = new EaElement { Id = 1, Name = "MyElement", Type = "Class", Stereotype = "ESRS::Disclosure" };
        var package = new EaPackage { Id = 10, Name = "MyPackage", Elements = { element } };
        return new EaRepository { RootPackages = { package } };
    }

    private static string OutputPath { get; } = Path.Combine(Path.GetTempPath(), "eaxwiki_integrity_" + Guid.NewGuid().ToString("N"));

    private static string Normalize(string path) => path.Replace('\\', '/');

    private static async Task<TestInMemoryWriter> RunExportAsync()
    {
        var writer = new TestInMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        await exporter.ExportAsync(MinimalRepository(), null, OutputPath);
        return writer;
    }

    private static string ReadExportedFile(TestInMemoryWriter writer, string fileName)
    {
        var key = Normalize(Path.Combine(OutputPath, fileName));
        Assert.True(writer.Files.ContainsKey(key), $"{fileName} should be created. Keys: {string.Join(", ", writer.Files.Keys)}");
        return writer.Files[key];
    }

    private static void AssertContainsAll(string content, params string[] markers)
    {
        foreach (var marker in markers)
            Assert.Contains(marker, content);
    }

    [Fact]
    public async Task NotesEditorScript_ContainsCoreFunctions()
    {
        var content = ReadExportedFile(await RunExportAsync(), "notes-editor.js");
        AssertContainsAll(content, "initNotesEditor", "suggestBtn", "ea-notes-suggest-btn", "/api/ai-suggest", "acquireEditLock");
    }

    [Fact]
    public async Task StatusEditorScript_ContainsCoreFunctions()
    {
        var content = ReadExportedFile(await RunExportAsync(), "status-editor.js");
        AssertContainsAll(content, "initStatusEditor", "/api/status");
    }

    [Fact]
    public async Task RowNotesEditorScript_ContainsCoreFunctions()
    {
        var content = ReadExportedFile(await RunExportAsync(), "row-notes-editor.js");
        AssertContainsAll(content, "initRowNotesEditors", "openEditor", "/api/row-notes");
    }

    [Fact]
    public async Task GraphInitScript_ContainsCoreFunctions()
    {
        var content = ReadExportedFile(await RunExportAsync(), "graph-init.js");
        AssertContainsAll(content, "initEaGraph", "cytoscape");
    }

    [Fact]
    public async Task ExtraCss_ContainsCoreStyles()
    {
        var content = ReadExportedFile(await RunExportAsync(), "extra.css");
        AssertContainsAll(content, ".ea-notes-editor", ".ea-notes-suggest-btn", ".ea-status-editor");
    }

    [Fact]
    public async Task CytoscapeMinJs_IsEmitted()
    {
        ReadExportedFile(await RunExportAsync(), "cytoscape.min.js");
    }
}
```

- [ ] **Step 2: Run the new tests — expect green**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~ScriptTemplateIntegrityTests"`
Expected: `Passed! - Failed: 0, ... Total: 6` (all six markers already exist in source)

- [ ] **Step 3: Prove the guard catches regressions (mutation test)**

Temporarily break a marker in `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs`: change the `suggestBtn.className = 'ea-notes-suggest-btn';` line (line 646) so the class string reads `'ea-notes-suggest-btnX'` instead. (Use this marker, not `suggestBtn`/`/api/ai-suggest` — those appear multiple times in the script output, and `/api/ai-suggest` is a substring of `/api/ai-suggest-diagram`, so they would not trip the assertion.) Re-run:
Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~NotesEditorScript_ContainsCoreFunctions"`
Expected: FAIL on `Assert.Contains` for `ea-notes-suggest-btn`. **Revert the temporary edit immediately.**

- [ ] **Step 4: Verify full suite green and commit**

Run: `dotnet test "src\EAxWiki.Tests"`
Expected: `Total: 267` (261 + 6 new)

```bash
git add src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs
git commit -m "test: add script template integrity checks"
```

---

### Task 3: Remove dead `ai-suggest.js` stub

**Files:**
- Create (test): `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` (add one test — modify existing file)
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (delete lines 1015-1021)
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (delete line 103)
- Delete: `wiki/ai-suggest.js`

**Interfaces:**
- Consumes: `RunExportAsync` helper from Task 2.
- Produces: no new interface; removes `WriteAiSuggestScriptAsync(string outputDir, CancellationToken)` from `InfrastructureWriter`.

- [ ] **Step 1: Write the failing test `AiSuggestJs_IsNotEmitted`**

Append to `ScriptTemplateIntegrityTests`:

```csharp
    [Fact]
    public async Task AiSuggestJs_IsNotEmitted()
    {
        var writer = await RunExportAsync();
        var key = Normalize(Path.Combine(OutputPath, "ai-suggest.js"));
        Assert.False(writer.Files.ContainsKey(key), $"ai-suggest.js should no longer be produced. Keys: {string.Join(", ", writer.Files.Keys)}");
    }
```

- [ ] **Step 2: Run test to verify it fails (RED)**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~AiSuggestJs_IsNotEmitted"`
Expected: FAIL — `WriteAiSuggestScriptAsync` still writes the stub.

- [ ] **Step 3: Remove `WriteAiSuggestScriptAsync` from `InfrastructureWriter.cs`**

Delete this method (lines 1015-1021 plus its surrounding blank line):

```csharp
    public async Task WriteAiSuggestScriptAsync(string outputDir, CancellationToken ct = default)
    {
        const string js = """
(function () { 'use strict'; })();
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "ai-suggest.js"), js, ct);
    }
```

- [ ] **Step 4: Remove the call from `MarkdownExporter.cs`**

Delete line 103 (`infrastructure.WriteAiSuggestScriptAsync(outputPath, cancellationToken),`).

- [ ] **Step 5: Delete the stale tracked file**

```bash
git rm wiki/ai-suggest.js
```

Note: do NOT rely on `CleanupOrphanedFilesAsync` — it only removes orphaned `*.md` files and package dirs, not root-level `.js` files.

- [ ] **Step 6: Run test to verify it passes (GREEN)**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~AiSuggestJs_IsNotEmitted"`
Expected: PASS.

- [ ] **Step 7: Full suite + commit**

Run: `dotnet test "src\EAxWiki.Tests"`
Expected: `Passed! - Failed: 0, Passed: 268, Skipped: 0, Total: 268`

```bash
git add src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "refactor: remove dead ai-suggest.js stub"
```

---

## Notes for the implementer

- The `MarkdownExporter.ExportAsync` signature used by tests is `ExportAsync(EaRepository repository, object? reader, string outputPath)` — pass `null` for `reader`.
- `extra.css` is an embedded resource; the export test confirms it lands in `writer.Files` under key `{outputPath}/extra.css`.
- The 6 positive tests intentionally pass immediately (markers already exist) — their value is guarding future regressions; Task 2 Step 3 proves they can catch one.
- Do not touch `WikiWritebackServer.cs` — the `/api/ai-suggest` endpoint there is a separate server-side feature that stays.
