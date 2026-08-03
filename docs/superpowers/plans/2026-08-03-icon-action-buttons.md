# Replace Action Labels with Icons Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the 7 runtime-created text-labeled action buttons (Save, Cancel, Suggest, Apply) across the three generated editor scripts with icon-only buttons backed by a shared `wiki/ea-icons.js` helper, per design `docs/superpowers/specs/2026-08-03-icon-action-buttons-design.md`.

**Architecture:** A new generated `wiki/ea-icons.js` exposes `window.EAxIcons = { ICONS: {...}, set(btn, name, label) }`. Each editor template in `InfrastructureWriter.cs` calls `EAxIcons.set(...)` (guarded by a `typeof` check with a one-time `console.error`) instead of assigning `textContent`, and sets `type = 'button'` on every button. The Suggest in-flight path swaps the sparkle for a CSS-animated spinner and restores it on completion/error. `mkdocs.yml` loads `ea-icons.js` first in `extra_javascript`. `Resources/extra.css` restyles the five button classes to the compact square pattern used by the edit buttons and adds the spinner keyframes.

**Tech Stack:** C# (net10.0), xUnit, `MarkdownExporter` + `IOutputWriter` in-memory pattern (no EA/COM in tests), raw-string JS templates embedded in `InfrastructureWriter.cs`, embedded CSS resource `Resources/extra.css`, `mkdocs.yml` `extra_javascript`, `scripts/export.ps1` for the forced export.

## Global Constraints

- Generated `wiki/*.js` files must stay byte-identical to their templates (integrity contract from the script-template-integrity feature). Never edit the generated `wiki/*.js` files — edit the templates in `InfrastructureWriter.cs` only, then re-export.
- `mkdocs.yml` `extra_javascript` order matters: `ea-icons.js` must precede all three editor scripts.
- Scope is exactly the 7 runtime-created buttons. Static pencil edit buttons (`&#9998;`) and message text (`msg.textContent`, e.g. `'Saving…'`, `'Retrying…'`, error strings) stay untouched.
- All icon buttons: `type = 'button'`, plus `aria-label` and `title` set via `EAxIcons.set`.
- Status-editor must NOT gain package-status dispatch (dropped for EA 17.1 compat) — icon work must not reintroduce it.
- Icons are inline SVG (`viewBox="0 0 24 24"`, `fill="currentColor"`, `aria-hidden="true"`), sized by CSS. No unicode glyphs, no icon-font.
- Suggest in-flight label is `'Generating…'` (U+2026 ellipsis) to match the existing `'Generating...'` intent; aria-label/title `'Generating…'`.
- Test count baseline: 268 passing before this change; 270 after (268 + 2 new test methods; existing methods extended in place).
- No EA/COM access in tests — exporter runs against `TestInMemoryWriter` with a minimal `EaRepository` (same pattern as `ScriptTemplateIntegrityTests`).
- Follow existing repo conventions: file-scoped namespaces, `private static` helpers, no comments unless required.
- Commits only after green tests; conventional commit messages (`feat:`, `docs:`, `test:`).

---

## File Structure

| File | Responsibility |
|------|---------------|
| `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` | MODIFY — new `WriteIconsScriptAsync` method; edit the three editor JS templates (status ~484-490, notes ~639-711, row-notes ~898-904) |
| `src/EAxWiki.Export/MarkdownExporter.cs` | MODIFY — call `WriteIconsScriptAsync` in the `viewTasks` list (lines 89-104) |
| `mkdocs.yml` | MODIFY — add `ea-icons.js` as first `extra_javascript` entry |
| `src/EAxWiki.Export/Resources/extra.css` | MODIFY — compact square button styles (5 classes), shared svg sizing rule, `@keyframes ea-spin`, `.ea-icon-spinner` |
| `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` | MODIFY — new `EaIconsScript_IsEmitted`; extend the 4 editor/css tests with `EAxIcons.set` positive + `*.textContent` negative assertions |
| `src/EAxWiki.Tests/ExportIntegrationTests.cs` | MODIFY — extend `Export_NotesEditorScript_IncludesAiSuggestButton`; new `Export_StatusEditorScript_UsesIconsNotLabels` |
| `wiki/ea-icons.js` | GENERATED — produced by `WriteIconsScriptAsync` on forced export (root-level `.js`, skipped by orphan cleanup) |
| `wiki/status-editor.js`, `wiki/notes-editor.js`, `wiki/row-notes-editor.js` | GENERATED — regenerated on forced export |

---

### Task 1: Emit `wiki/ea-icons.js` via `WriteIconsScriptAsync`

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (add method before `WriteExtraCssAsync` at line 1015)
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (viewTasks list, after line 99 `WriteGraphScriptsAsync`)
- Test: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` (new test method)

**Interfaces:**
- Produces: `public async Task WriteIconsScriptAsync(string outputDir, CancellationToken ct = default)` on `InfrastructureWriter`, writing file `ea-icons.js` (path `Path.Combine(outputDir, "ea-icons.js")`). Emits `window.EAxIcons` with `ICONS` keys `save`, `cancel`, `suggest`, `apply`, `spinner` and `set(btn, name, label)`. Later tasks consume `EAxIcons.set(btn, name, label)` with those five icon keys.

- [ ] **Step 1: Write the failing test**

In `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`, add after `CytoscapeMinJs_IsEmitted` (line 96):

```csharp
    [Fact]
    public async Task EaIconsScript_IsEmitted()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "ea-icons.js");
        AssertContainsAll(content, "window.EAxIcons", "EAxIcons.set", "aria-label", "spinner");
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~EaIconsScript_IsEmitted"`
Expected: FAIL — `ea-icons.js should be created. Keys: ...` (file not produced yet)

- [ ] **Step 3: Add `WriteIconsScriptAsync` to `InfrastructureWriter.cs`**

Insert this method directly before `public async Task WriteExtraCssAsync` (line 1015):

```csharp
    public async Task WriteIconsScriptAsync(string outputDir, CancellationToken ct = default)
    {
        const string js = """
window.EAxIcons = {
  ICONS: {
    save: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>',
    cancel: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>',
    suggest: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 0L14.6 9.4 24 12 14.6 14.6 12 24 9.4 14.6 0 12 9.4 9.4z"/></svg>',
    apply: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>',
    spinner: '<svg viewBox="0 0 24 24" class="ea-icon-spinner" fill="currentColor" aria-hidden="true"><path d="M12 6v3l4-4-4-4v3c-4.42 0-8 3.58-8 8 0 1.57.46 3.03 1.24 4.26L6.7 14.8c-.45-.83-.7-1.79-.7-2.8 0-3.31 2.69-6 6-6zm6.76 1.74L17.3 9.2c.44.84.7 1.79.7 2.8 0 3.31-2.69 6-6 6v-3l-4 4 4 4v-3c4.42 0 8-3.58 8-8 0-1.57-.46-3.03-1.24-4.26z"/></svg>'
  },
  set: function (btn, name, label) {
    btn.innerHTML = this.ICONS[name] || '';
    btn.setAttribute('aria-label', label);
    btn.setAttribute('title', label);
  }
};
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "ea-icons.js"), js, ct);
    }
```

- [ ] **Step 4: Wire the call into `MarkdownExporter.cs`**

In `src/EAxWiki.Export/MarkdownExporter.cs`, in the `viewTasks` list (lines 89-104), add a line for the icons script immediately after the `infrastructure.WriteGraphScriptsAsync` entry (line 99):

```csharp
                infrastructure.WriteGraphScriptsAsync(outputPath, cancellationToken),
                infrastructure.WriteIconsScriptAsync(outputPath, cancellationToken),
                infrastructure.WriteStatusEditorScriptAsync(outputPath, cancellationToken),
```

- [ ] **Step 5: Run test to verify it passes**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~EaIconsScript_IsEmitted"`
Expected: PASS

- [ ] **Step 6: Run full suite**

Run: `dotnet test "src\EAxWiki.Tests"`
Expected: `Passed! - Failed: 0, Passed: 269, Skipped: 0, Total: 269`

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Export/MarkdownExporter.cs src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs
git commit -m "feat(export): emit shared ea-icons.js icon helper"
```

---

### Task 2: Load `ea-icons.js` first in `mkdocs.yml`

**Files:**
- Modify: `mkdocs.yml` (lines 21-26)

**Interfaces:**
- Consumes: file `wiki/ea-icons.js` produced by Task 1.
- Produces: browser load order `ea-icons.js` → `cytoscape.min.js` → `graph-init.js` → `status-editor.js` → `notes-editor.js` → `row-notes-editor.js`. Tasks 3-5 rely on `EAxIcons` being defined before their editor scripts run.

- [ ] **Step 1: Edit `extra_javascript`**

In `mkdocs.yml`, change lines 21-26 from:

```yaml
extra_javascript:
  - cytoscape.min.js
  - graph-init.js
  - status-editor.js
  - notes-editor.js
  - row-notes-editor.js
```

to:

```yaml
extra_javascript:
  - ea-icons.js
  - cytoscape.min.js
  - graph-init.js
  - status-editor.js
  - notes-editor.js
  - row-notes-editor.js
```

- [ ] **Step 2: Verify**

Run: `Select-String -Path "mkdocs.yml" -Pattern "extra_javascript" -Context 0,7`
Expected: `ea-icons.js` is the first entry under `extra_javascript`.

- [ ] **Step 3: Commit**

```bash
git add mkdocs.yml
git commit -m "docs: load ea-icons.js before editor scripts in mkdocs"
```

---

### Task 3: Icon buttons in the status-editor template

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (status template, lines 428-587)
- Test: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` (`StatusEditorScript_ContainsCoreFunctions`)
- Test: `src/EAxWiki.Tests/ExportIntegrationTests.cs` (new `Export_StatusEditorScript_UsesIconsNotLabels`)

**Interfaces:**
- Consumes: `EAxIcons.set(btn, 'apply'|'cancel', label)` from Task 1.
- Produces: status-editor buttons `ea-status-btn` (Apply, icon key `apply`) and `ea-status-cancel-btn` (Cancel, icon key `cancel`), each with `type = 'button'`.

- [ ] **Step 1: Write the failing tests**

In `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`, replace `StatusEditorScript_ContainsCoreFunctions` (lines 58-64) with:

```csharp
    [Fact]
    public async Task StatusEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "status-editor.js");
        AssertContainsAll(content, "initStatusEditor", "/api/status", "EAxIcons.set(applyBtn", "ea-status-btn", "ea-status-cancel-btn");
        Assert.DoesNotContain("applyBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }
```

In `src/EAxWiki.Tests/ExportIntegrationTests.cs`, add after `Export_NotesEditorScript_IncludesAiSuggestButton` (after line 119):

```csharp
    [Fact]
    public async Task Export_StatusEditorScript_UsesIconsNotLabels()
    {
        var writer = new TestInMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository();

        await exporter.ExportAsync(repo, null, OutputPath);

        var key = Normalize(Path.Combine(OutputPath, "status-editor.js"));
        Assert.True(writer.Files.ContainsKey(key), $"status-editor.js should be created. Keys: {string.Join(", ", writer.Files.Keys)}");
        var content = writer.Files[key];
        Assert.Contains("EAxIcons.set(applyBtn", content);
        Assert.DoesNotContain("applyBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }
```

- [ ] **Step 2: Run tests to verify they fail**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~StatusEditorScript_ContainsCoreFunctions|FullyQualifiedName~Export_StatusEditorScript_UsesIconsNotLabels"`
Expected: FAIL — `Assert.Contains` for `"EAxIcons.set(applyBtn"` (template still uses `textContent`)

- [ ] **Step 3: Edit the status template**

In the status-editor JS template (`InfrastructureWriter.cs` lines 428-587), after the line `'use strict';` (line 430) insert the one-time guard:

```js
  if (typeof EAxIcons === 'undefined' && !window.__eaIconsWarned) {
    window.__eaIconsWarned = true;
    console.error('EAxIcons helper not loaded');
  }
```

Replace lines 484-490 (the `applyBtn`/`cancelBtn` creation):

```js
      applyBtn = document.createElement('button');
      applyBtn.className = 'ea-status-btn';
      applyBtn.textContent = 'Apply';

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-status-cancel-btn';
      cancelBtn.textContent = 'Cancel';
```

with:

```js
      applyBtn = document.createElement('button');
      applyBtn.className = 'ea-status-btn';
      applyBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(applyBtn, 'apply', 'Apply');

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-status-cancel-btn';
      cancelBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(cancelBtn, 'cancel', 'Cancel');
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~StatusEditorScript_ContainsCoreFunctions"`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs src/EAxWiki.Tests/ExportIntegrationTests.cs
git commit -m "feat(export): icon-only status editor buttons"
```

---

### Task 4: Icon buttons + spinner in the notes-editor template

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (notes template, lines 589-821)
- Test: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` (`NotesEditorScript_ContainsCoreFunctions`)
- Test: `src/EAxWiki.Tests/ExportIntegrationTests.cs` (`Export_NotesEditorScript_IncludesAiSuggestButton`)

**Interfaces:**
- Consumes: `EAxIcons.set(btn, 'save'|'suggest'|'cancel'|'spinner', label)` from Task 1.
- Produces: notes-editor buttons `ea-notes-save-btn` (Save, `save`), `ea-notes-suggest-btn` (Suggest, `suggest`, swapped to `spinner` in-flight), `ea-notes-cancel-btn` (Cancel, `cancel`), each `type = 'button'`. On Suggest completion and error, the sparkle (`suggest`) is restored.

- [ ] **Step 1: Write the failing test**

In `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`, replace `NotesEditorScript_ContainsCoreFunctions` (lines 50-56) with:

```csharp
    [Fact]
    public async Task NotesEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "notes-editor.js");
        AssertContainsAll(content, "initNotesEditor", "suggestBtn", "ea-notes-suggest-btn", "/api/ai-suggest", "acquireEditLock", "EAxIcons.set(saveBtn", "EAxIcons.set(suggestBtn, 'spinner'");
        Assert.DoesNotContain("saveBtn.textContent", content);
        Assert.DoesNotContain("suggestBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }
```

In `src/EAxWiki.Tests/ExportIntegrationTests.cs`, replace `Export_NotesEditorScript_IncludesAiSuggestButton` (lines 104-119) with:

```csharp
    [Fact]
    public async Task Export_NotesEditorScript_IncludesAiSuggestButton()
    {
        var writer = new TestInMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = MinimalRepository();

        await exporter.ExportAsync(repo, null, OutputPath);

        var key = Normalize(Path.Combine(OutputPath, "notes-editor.js"));
        Assert.True(writer.Files.ContainsKey(key), $"notes-editor.js should be created. Keys: {string.Join(", ", writer.Files.Keys)}");
        var content = writer.Files[key];
        Assert.Contains("suggestBtn", content);
        Assert.Contains("ea-notes-suggest-btn", content);
        Assert.Contains("/api/ai-suggest", content);
        Assert.Contains("EAxIcons.set(saveBtn", content);
        Assert.DoesNotContain("saveBtn.textContent", content);
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~NotesEditorScript_ContainsCoreFunctions|FullyQualifiedName~Export_NotesEditorScript_IncludesAiSuggestButton"`
Expected: FAIL — `"EAxIcons.set(saveBtn"` not found (template still uses `textContent`)

- [ ] **Step 3: Edit the notes template**

In the notes-editor JS template (`InfrastructureWriter.cs` lines 589-821), after `'use strict';` (line 593) insert the one-time guard:

```js
  if (typeof EAxIcons === 'undefined' && !window.__eaIconsWarned) {
    window.__eaIconsWarned = true;
    console.error('EAxIcons helper not loaded');
  }
```

Replace lines 639-653 (saveBtn/suggestBtn/cancelBtn creation):

```js
      saveBtn = document.createElement('button');
      saveBtn.className = 'ea-notes-save-btn';
      saveBtn.textContent = 'Save';

      suggestBtn = null;
      if (widget.dataset.aiConfigured === 'true') {
        suggestBtn = document.createElement('button');
        suggestBtn.className = 'ea-notes-suggest-btn';
        suggestBtn.textContent = 'Suggest';
        suggestBtn.type = 'button';
      }

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-notes-cancel-btn';
      cancelBtn.textContent = 'Cancel';
```

with:

```js
      saveBtn = document.createElement('button');
      saveBtn.className = 'ea-notes-save-btn';
      saveBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(saveBtn, 'save', 'Save');

      suggestBtn = null;
      if (widget.dataset.aiConfigured === 'true') {
        suggestBtn = document.createElement('button');
        suggestBtn.className = 'ea-notes-suggest-btn';
        suggestBtn.type = 'button';
        if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'suggest', 'Suggest');
      }

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-notes-cancel-btn';
      cancelBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(cancelBtn, 'cancel', 'Cancel');
```

Replace the Suggest in-flight start (lines 674-676):

```js
        suggestBtn.addEventListener('click', function () {
          suggestBtn.disabled = true;
          suggestBtn.textContent = 'Generating...';
```

with:

```js
        suggestBtn.addEventListener('click', function () {
          suggestBtn.disabled = true;
          if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'spinner', 'Generating…');
```

Replace both Suggest restore paths (line 701-702 inside the success `if`, and line 707-708 inside the `catch`):

```js
            suggestBtn.disabled = false;
            suggestBtn.textContent = 'Suggest';
```

with:

```js
            suggestBtn.disabled = false;
            if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'suggest', 'Suggest');
```

(There are two occurrences of this pair — the success branch at ~701-702 and the error branch at ~707-708 — both must be replaced.)

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~NotesEditorScript_ContainsCoreFunctions|FullyQualifiedName~Export_NotesEditorScript_IncludesAiSuggestButton"`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs src/EAxWiki.Tests/ExportIntegrationTests.cs
git commit -m "feat(export): icon-only notes editor buttons with spinner"
```

---

### Task 5: Icon buttons in the row-notes-editor template

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (row-notes template, lines 823-1013)
- Test: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` (`RowNotesEditorScript_ContainsCoreFunctions`)

**Interfaces:**
- Consumes: `EAxIcons.set(btn, 'save'|'cancel', label)` from Task 1.
- Produces: row-notes editor buttons `ea-notes-save-btn` (Save, `save`) and `ea-notes-cancel-btn` (Cancel, `cancel`), each `type = 'button'`.

- [ ] **Step 1: Write the failing test**

In `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`, replace `RowNotesEditorScript_ContainsCoreFunctions` (lines 66-72) with:

```csharp
    [Fact]
    public async Task RowNotesEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "row-notes-editor.js");
        AssertContainsAll(content, "initRowNotesEditors", "openEditor", "/api/row-notes", "EAxIcons.set(saveBtn", "ea-notes-save-btn", "ea-notes-cancel-btn");
        Assert.DoesNotContain("saveBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~RowNotesEditorScript_ContainsCoreFunctions"`
Expected: FAIL — `"EAxIcons.set(saveBtn"` not found (template still uses `textContent`)

- [ ] **Step 3: Edit the row-notes template**

In the row-notes-editor JS template (`InfrastructureWriter.cs` lines 823-1013), after `'use strict';` (line 827) insert the one-time guard:

```js
  if (typeof EAxIcons === 'undefined' && !window.__eaIconsWarned) {
    window.__eaIconsWarned = true;
    console.error('EAxIcons helper not loaded');
  }
```

Replace lines 898-904 (saveBtn/cancelBtn creation):

```js
    var saveBtn = document.createElement('button');
    saveBtn.className = 'ea-notes-save-btn';
    saveBtn.textContent = 'Save';

    var cancelBtn = document.createElement('button');
    cancelBtn.className = 'ea-notes-cancel-btn';
    cancelBtn.textContent = 'Cancel';
```

with:

```js
    var saveBtn = document.createElement('button');
    saveBtn.className = 'ea-notes-save-btn';
    saveBtn.type = 'button';
    if (typeof EAxIcons !== 'undefined') EAxIcons.set(saveBtn, 'save', 'Save');

    var cancelBtn = document.createElement('button');
    cancelBtn.className = 'ea-notes-cancel-btn';
    cancelBtn.type = 'button';
    if (typeof EAxIcons !== 'undefined') EAxIcons.set(cancelBtn, 'cancel', 'Cancel');
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~RowNotesEditorScript_ContainsCoreFunctions"`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs
git commit -m "feat(export): icon-only row notes editor buttons"
```

---

### Task 6: Compact icon button CSS + spinner in `Resources/extra.css`

**Files:**
- Modify: `src/EAxWiki.Export/Resources/extra.css`
- Test: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs` (`ExtraCss_ContainsCoreStyles`)

**Interfaces:**
- Consumes: `.ea-icon-spinner` class on the spinner SVG (Task 1) — animation must be defined here.
- Produces: compact square buttons (5 classes), shared svg sizing rule, `@keyframes ea-spin`, `.ea-icon-spinner`. `WriteExtraCssAsync` (InfrastructureWriter line 1015) reads this embedded resource verbatim, so no C# change is needed.

- [ ] **Step 1: Write the failing test**

In `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`, replace `ExtraCss_ContainsCoreStyles` (lines 82-88) with:

```csharp
    [Fact]
    public async Task ExtraCss_ContainsCoreStyles()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "extra.css");
        AssertContainsAll(content, ".ea-notes-editor", ".ea-notes-suggest-btn", ".ea-status-editor", "ea-icon-spinner", "@keyframes ea-spin", "fill: currentColor");
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~ExtraCss_ContainsCoreStyles"`
Expected: FAIL — `"ea-icon-spinner"` not found

- [ ] **Step 3: Edit `Resources/extra.css`**

Replace `.ea-status-btn` (lines 199-208):

```css
.ea-status-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-primary-fg-color);
  border-radius: 4px;
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
  cursor: pointer;
  font-size: inherit;
  font-weight: 600;
}
```

with:

```css
.ea-status-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.8em;
  height: 1.8em;
  padding: 0;
  border: 1px solid var(--md-primary-fg-color);
  border-radius: 4px;
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
  cursor: pointer;
  font-size: 0.9em;
  line-height: 1;
}
```

Replace `.ea-status-cancel-btn` (lines 209-217):

```css
.ea-status-cancel-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  cursor: pointer;
  font-size: inherit;
}
```

with:

```css
.ea-status-cancel-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.8em;
  height: 1.8em;
  padding: 0;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  cursor: pointer;
  font-size: 0.9em;
  line-height: 1;
}
```

Replace `.ea-notes-save-btn` (lines 276-285):

```css
.ea-notes-save-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-primary-fg-color);
  border-radius: 4px;
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
  cursor: pointer;
  font-size: 0.85em;
  font-weight: 600;
}
```

with:

```css
.ea-notes-save-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.8em;
  height: 1.8em;
  padding: 0;
  border: 1px solid var(--md-primary-fg-color);
  border-radius: 4px;
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
  cursor: pointer;
  font-size: 0.9em;
  line-height: 1;
}
```

Replace `.ea-notes-cancel-btn` (lines 286-294):

```css
.ea-notes-cancel-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  cursor: pointer;
  font-size: 0.85em;
}
```

with:

```css
.ea-notes-cancel-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.8em;
  height: 1.8em;
  padding: 0;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  cursor: pointer;
  font-size: 0.9em;
  line-height: 1;
}
```

Replace `.ea-notes-suggest-btn` (lines 295-304):

```css
.ea-notes-suggest-btn {
  padding: 0.2em 0.8em;
  border: 1px solid #9c27b0;
  border-radius: 4px;
  background: #9c27b0;
  color: #fff;
  cursor: pointer;
  font-size: 0.85em;
  font-weight: 600;
}
```

with:

```css
.ea-notes-suggest-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.8em;
  height: 1.8em;
  padding: 0;
  border: 1px solid #9c27b0;
  border-radius: 4px;
  background: #9c27b0;
  color: #fff;
  cursor: pointer;
  font-size: 0.9em;
  line-height: 1;
}
```

Insert after the `.ea-notes-suggest-btn:hover` rule (after line 308), before the disabled rules at line 309:

```css
.ea-status-btn svg, .ea-notes-save-btn svg, .ea-notes-cancel-btn svg, .ea-notes-suggest-btn svg {
  width: 1em;
  height: 1em;
  fill: currentColor;
}
@keyframes ea-spin {
  to { transform: rotate(360deg); }
}
.ea-icon-spinner {
  animation: ea-spin 0.8s linear infinite;
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test "src\EAxWiki.Tests" --filter "FullyQualifiedName~ExtraCss_ContainsCoreStyles"`
Expected: PASS

- [ ] **Step 5: Run full suite**

Run: `dotnet test "src\EAxWiki.Tests"`
Expected: `Passed! - Failed: 0, Passed: 270, Skipped: 0, Total: 270`

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Export/Resources/extra.css src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs
git commit -m "feat(export): compact icon button styles and spinner CSS"
```

---

### Task 7: Forced export, verification, and commit of generated wiki output

**Files:**
- Generated by export: `wiki/ea-icons.js` (NEW), `wiki/status-editor.js`, `wiki/notes-editor.js`, `wiki/row-notes-editor.js`, `wiki/extra.css`, plus the full wiki regeneration and `model/EurSuRA.qea`.
- No source edits in this task unless verification fails (then loop back to the owning task).

**Interfaces:**
- Consumes: all Tasks 1-6 (templates, mkdocs.yml, CSS, tests green).

- [ ] **Step 1: Stop the monitor task and free the EA model**

The scheduled `EAxWiki-Monitor` task may be running an export that holds the EA model lock. Stop it and kill any lingering EA/export processes:

```powershell
schtasks /end /tn "EAxWiki-Monitor"
Get-Process -Name EA -ErrorAction SilentlyContinue | Where-Object { $_.Id -ne 6444 } | Stop-Process -Force
```

(`6444` is the long-running elevated EA instance that cannot be killed from this shell; leave it. If another elevated EA holds the model, retry the export below after confirming the lock is gone.)

- [ ] **Step 2: Run the forced export**

Run (from repo root):

```powershell
.\scripts\export.ps1 --repo "model/EurSuRA.qea" --output "wiki" --force --writeback --api-port 8001
```

Expected: `Export complete: ... succeeded, 0 failed` and validation `... passed, ... warnings, 0 errors`.

- [ ] **Step 3: Verify generated output**

Verify the helper file exists and survived orphan cleanup:

```powershell
Test-Path "wiki\ea-icons.js"
```

Verify the generated editor scripts contain the icon calls and no label assignments:

```powershell
Select-String -Path "wiki\status-editor.js","wiki\notes-editor.js","wiki\row-notes-editor.js" -Pattern "EAxIcons.set"
Select-String -Path "wiki\status-editor.js","wiki\notes-editor.js","wiki\row-notes-editor.js" -Pattern "\.textContent = 'Save'|\.textContent = 'Cancel'|\.textContent = 'Apply'|\.textContent = 'Suggest'"
```

Expected: `EAxIcons.set` appears in all three files; the second command finds no matches.

Verify `mkdocs.yml` still lists `ea-icons.js` first and the CSS shipped the spinner:

```powershell
Select-String -Path "wiki\extra.css" -Pattern "ea-icon-spinner|@keyframes ea-spin"
```

- [ ] **Step 4: Commit the regenerated wiki + model**

Use `git status -sb` first to review what changed (the working tree already contains monitor-generated wiki diffs from before this feature; the forced export supersedes them). Stage everything except the monitor logs, then commit:

```powershell
git add wiki model/EurSuRA.qea
git status -sb
```

Then:

```powershell
git commit -m "wiki: regenerate after icon action buttons feature"
```

Do NOT stage `.eaxwiki-monitor/` (monitor stdout/stderr logs) or `api-ready` unless the validation report marks it required — follow the existing repo convention for `api-ready` (`M` commit only when the API server state changed; delete/re-create only if validation demands it).

- [ ] **Step 5: Push**

```powershell
git push origin master
```

Expected: `master -> master` in sync (`git status -sb` shows `## master...origin/master` with no ahead/behind).

---

## Self-Review

**Spec coverage:**
- New `wiki/ea-icons.js` with `window.EAxIcons` + `set()` → Task 1.
- `mkdocs.yml` loads `ea-icons.js` first → Task 2.
- 7 buttons across 3 editors icon-only → Tasks 3, 4, 5.
- `type='button'` on all buttons → Tasks 3, 4, 5.
- `aria-label` + `title` → `EAxIcons.set` (Task 1) — applies to all buttons.
- Suggest in-flight spinner swap + restore on success/error → Task 4.
- CSS compact square buttons, svg `fill: currentColor`, `@keyframes ea-spin`, `.ea-icon-spinner` → Task 6.
- Guard `typeof EAxIcons !== 'undefined'` with one-time `console.error` → added in Tasks 3, 4, 5.
- Tests: `ScriptTemplateIntegrityTests` (positive + negative) and `ExportIntegrationTests` → Tasks 1, 3, 4, 5, 6.
- Forced export + verification (ea-icons.js produced, no orphan-cleanup removal, byte-identical contract) → Task 7.
- Status-editor no package-status dispatch reintroduced → the template edit in Task 3 only touches the guard + two button blocks.

**Placeholder scan:** No TBD/TODO; every code step shows complete code; exact commands with expected output given.

**Type consistency:** `EAxIcons.set(btn, name, label)` is produced once in Task 1 and consumed identically in Tasks 3-5 with icon keys `save`, `cancel`, `suggest`, `apply`, `spinner` — all five defined in the `ICONS` map. The `.ea-icon-spinner` class referenced by the spinner SVG (Task 1) is styled in Task 6. `WriteIconsScriptAsync(string outputDir, CancellationToken ct = default)` matches the other writer methods' signature.
