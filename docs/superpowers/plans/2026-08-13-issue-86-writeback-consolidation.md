# Write-back Pattern Consolidation Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Remove the duplicated write-back logic in `EaReader.cs` (seven `Update…` methods sharing a six-step skeleton) and `FrontmatterParser.cs` (four `Update…` methods sharing a read/EOL/atomic-write tail) without changing any observable behavior.

**Architecture:** `EaReader` gains one private delegate-driven `Write<TEntity>` skeleton plus a generic `Find<T>` collection scan; each public `Update…` method keeps only its entity-specific locate/apply/read-back logic. `FrontmatterParser` gains a private `RewriteMarkdown(path, transform)` tail (with a `null`-abort no-op contract) and a shared `UpdateNotesLike` for the two near-identical notes methods; all four public `Update…` methods route through it. Public contracts (`IEaReader`, `FrontmatterParser`, `WikiWritebackServer`, `WriteBackScanner`, `FakeEaReader`) are untouched.

**Tech Stack:** C# / .NET 10, xUnit, Moq, Pester 5 (verification only). Spec: `docs/superpowers/specs/2026-08-13-issue-86-writeback-consolidation-design.md`.

## Global Constraints

- Behavior-preserving: exception types/messages, `VerifyWrite` labels, duplicate-match `LogWarning` text, and `LogInformation` templates must be byte-identical to the pre-refactor code.
- No change to public API: `IEaReader`, the `FrontmatterParser` public methods, `EaReaderStaDispatcher`, `WikiWritebackServer`, `WriteBackScanner`, `FakeEaReader`.
- CRLF preservation, atomic temp-file swaps, and the `Modified:` date bump must be unchanged.
- `UpdateStatus`/`UpdateNotes`/`UpdatePackageNotes` must still leave the file untouched (no write at all) when frontmatter is missing or malformed.
- All existing tests must stay green: 298 .NET + 162 Pester. Two new `FrontmatterParserTests` cases are added for `UpdatePackageNotes`.
- No new dependencies.
- The test project only compiles with EAPath set; every `dotnet test` command below must be prefixed with `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\';`.

---

### Task 1: EaReader — add `Write<TEntity>` and convert the four ID-based write-backs

**Files:**
- Modify: `src/EAxWiki.EA/EaReader.cs:273-331` (bodies of `UpdateElementStatus`, `UpdateElementNotes`, `UpdatePackageNotes`, `UpdateDiagramNotes`)

**Interfaces:**
- Produces: private helper
  `Write<TEntity>(Func<TEntity> locate, Action<TEntity> apply, Func<string?> readBack, string expected, string entityDescription, string successTemplate, params object[] successArgs)` — runs the shared skeleton; `locate` throws `InvalidOperationException` when the entity cannot be found. Task 2 reuses this helper unchanged.

- [ ] **Step 1: Insert the `Write<TEntity>` helper** directly after `UpdateTaggedValueNotes` (after the closing brace at `src/EAxWiki.EA/EaReader.cs:461`, before `VerifyWrite` at line 463):

```csharp
    /// <summary>
    /// Shared write-back skeleton for every Update* method: null-check the open repository, locate the
    /// target entity (locate throws a descriptive exception when it cannot be found), apply the field
    /// change and COM Update, refresh the model view, re-read the value and VerifyWrite it, then log.
    /// </summary>
    private void Write<TEntity>(
        Func<TEntity> locate,
        Action<TEntity> apply,
        Func<string?> readBack,
        string expected,
        string entityDescription,
        string successTemplate,
        params object[] successArgs)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var entity = locate();
        apply(entity);
        _repository.RefreshModelView(0);
        var actual = readBack() ?? string.Empty;
        VerifyWrite(_logger, entityDescription, expected, actual);
        _logger?.LogInformation(successTemplate, successArgs);
    }
```

- [ ] **Step 2: Convert `UpdateElementStatus`** (replace lines 273-286) — exception message and log template unchanged:

```csharp
    public void UpdateElementStatus(int elementId, string newStatus)
    {
        Write(
            () => _repository!.GetElementByID(elementId)
                ?? throw new InvalidOperationException($"Element {elementId} not found in repository."),
            element => { element.Status = newStatus; element.Update(); },
            () => _repository!.GetElementByID(elementId)?.Status,
            newStatus,
            $"element {elementId} Status",
            "Updated element {ElementId} status to '{Status}'",
            elementId, newStatus);
    }
```

- [ ] **Step 3: Convert `UpdateElementNotes`** (replace lines 288-301):

```csharp
    public void UpdateElementNotes(int elementId, string newNotesHtml)
    {
        Write(
            () => _repository!.GetElementByID(elementId)
                ?? throw new InvalidOperationException($"Element {elementId} not found in repository."),
            element => { element.Notes = newNotesHtml; element.Update(); },
            () => _repository!.GetElementByID(elementId)?.Notes,
            newNotesHtml,
            $"element {elementId} Notes",
            "Updated element {ElementId} notes",
            elementId);
    }
```

- [ ] **Step 4: Convert `UpdatePackageNotes`** (replace lines 303-316):

```csharp
    public void UpdatePackageNotes(int packageId, string newNotesHtml)
    {
        Write(
            () => _repository!.GetPackageByID(packageId)
                ?? throw new InvalidOperationException($"Package {packageId} not found in repository."),
            package => { package.Notes = newNotesHtml; package.Update(); },
            () => _repository!.GetPackageByID(packageId)?.Notes,
            newNotesHtml,
            $"package {packageId} Notes",
            "Updated package {PackageId} notes",
            packageId);
    }
```

- [ ] **Step 5: Convert `UpdateDiagramNotes`** (replace lines 318-331):

```csharp
    public void UpdateDiagramNotes(int diagramId, string newNotesHtml)
    {
        Write(
            () => _repository!.GetDiagramByID(diagramId)
                ?? throw new InvalidOperationException($"Diagram {diagramId} not found in repository."),
            diagram => { diagram.Notes = newNotesHtml; diagram.Update(); },
            () => _repository!.GetDiagramByID(diagramId)?.Notes,
            newNotesHtml,
            $"diagram {diagramId} Notes",
            "Updated diagram {DiagramId} notes",
            diagramId);
    }
```

- [ ] **Step 6: Build and run the EaReader-related tests**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build src/EAxWiki 2>&1 | Select-Object -Last 2
```
Expected: `0 Error(s)` and no new warnings.

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests --filter "FullyQualifiedName~EaReaderTests|FullyQualifiedName~WriteBackScannerTests" 2>&1 | Select-Object -Last 2
```
Expected: `Passed!  - Failed: 0, Passed: 42` (EaReaderTests 38 + WriteBackScannerTests 4) — the seven "WhenNotOpen_Throws" tests now exercise the `_repository == null` branch inside the new helper.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.EA/EaReader.cs
git commit -m "refactor(ea): dedup ID-based write-back skeleton via Write helper (issue #86)"
```

---

### Task 2: EaReader — add `Find<T>` and convert the three collection write-backs

**Files:**
- Modify: `src/EAxWiki.EA/EaReader.cs:333-461` (bodies of `UpdateAttributeNotes`, `UpdateMethodNotes`, `UpdateTaggedValueNotes`)

**Interfaces:**
- Consumes: `Write<TEntity>` from Task 1.
- Produces: private helper `Find<T>(EA.Collection collection, Func<T, bool> predicate) where T : class` returning `(T? match, int count)` — first-match scan preserving the existing composite-key search and duplicate count.

- [ ] **Step 1: Insert the `Find<T>` helper** directly after the `Write<TEntity>` helper (before `VerifyWrite`):

```csharp
    /// <summary>
    /// First-match scan of an EA COM collection, counting how many entries satisfy the predicate so the
    /// caller can log the "multiple matches, updating the first" warning. EA.Attribute/Method/TaggedValue
    /// expose no ID property, so the parent element's collection must be searched by a composite key.
    /// </summary>
    private static (T? match, int count) Find<T>(EA.Collection collection, Func<T, bool> predicate) where T : class
    {
        T? match = null;
        var count = 0;
        for (short i = 0; i < collection.Count; i++)
            if (collection.GetAt(i) is T item && predicate(item))
            {
                count++;
                match ??= item;
            }
        return (match, count);
    }
```

- [ ] **Step 2: Convert `UpdateAttributeNotes`** (replace lines 338-377) — same exception messages, duplicate warning text, and log template:

```csharp
    public void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml)
    {
        Write(
            () =>
            {
                var element = _repository!.GetElementByID(elementId)
                    ?? throw new InvalidOperationException($"Element {elementId} not found in repository.");
                if (element.Attributes is not EA.Collection attrs)
                    throw new InvalidOperationException($"Element {elementId} has no attributes collection.");
                var (match, matchCount) = Find<EA.Attribute>(attrs, a =>
                    string.Equals(a.Name, attributeName, StringComparison.Ordinal) &&
                    string.Equals(a.Type, attributeType, StringComparison.Ordinal));
                if (matchCount > 1)
                    _logger?.LogWarning("Multiple attributes named '{Name}' of type '{Type}' found on element {ElementId}; updating the first match.", attributeName, attributeType, elementId);
                return match
                    ?? throw new InvalidOperationException($"Attribute '{attributeName}' ({attributeType}) not found on element {elementId}.");
            },
            attr => { attr.Notes = newNotesHtml; attr.Update(); },
            () =>
            {
                var reElement = _repository!.GetElementByID(elementId);
                if (reElement?.Attributes is not EA.Collection reAttrs) return null;
                return Find<EA.Attribute>(reAttrs, a =>
                    string.Equals(a.Name, attributeName, StringComparison.Ordinal) &&
                    string.Equals(a.Type, attributeType, StringComparison.Ordinal)).match?.Notes;
            },
            newNotesHtml,
            $"attribute '{attributeName}' ({attributeType}) on element {elementId}",
            "Updated attribute '{Name}' notes on element {ElementId}",
            attributeName, elementId);
    }
```

- [ ] **Step 3: Convert `UpdateMethodNotes`** (replace lines 379-420):

```csharp
    public void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml)
    {
        Write(
            () =>
            {
                var element = _repository!.GetElementByID(elementId)
                    ?? throw new InvalidOperationException($"Element {elementId} not found in repository.");
                if (element.Methods is not EA.Collection methods)
                    throw new InvalidOperationException($"Element {elementId} has no methods collection.");
                var (match, matchCount) = Find<EA.Method>(methods, m =>
                    string.Equals(m.Name, methodName, StringComparison.Ordinal) &&
                    string.Equals(m.ReturnType, returnType, StringComparison.Ordinal) &&
                    m.IsStatic == isStatic);
                if (matchCount > 1)
                    _logger?.LogWarning("Multiple methods named '{Name}' ({ReturnType}) found on element {ElementId}; updating the first match.", methodName, returnType, elementId);
                return match
                    ?? throw new InvalidOperationException($"Method '{methodName}' ({returnType}) not found on element {elementId}.");
            },
            method => { method.Notes = newNotesHtml; method.Update(); },
            () =>
            {
                var reElement = _repository!.GetElementByID(elementId);
                if (reElement?.Methods is not EA.Collection reMethods) return null;
                return Find<EA.Method>(reMethods, m =>
                    string.Equals(m.Name, methodName, StringComparison.Ordinal) &&
                    string.Equals(m.ReturnType, returnType, StringComparison.Ordinal) &&
                    m.IsStatic == isStatic).match?.Notes;
            },
            newNotesHtml,
            $"method '{methodName}' ({returnType}) on element {elementId}",
            "Updated method '{Name}' notes on element {ElementId}",
            methodName, elementId);
    }
```

- [ ] **Step 4: Convert `UpdateTaggedValueNotes`** (replace lines 422-461):

```csharp
    public void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml)
    {
        Write(
            () =>
            {
                var element = _repository!.GetElementByID(elementId)
                    ?? throw new InvalidOperationException($"Element {elementId} not found in repository.");
                if (element.TaggedValues is not EA.Collection taggedValues)
                    throw new InvalidOperationException($"Element {elementId} has no tagged values collection.");
                var (match, matchCount) = Find<EA.TaggedValue>(taggedValues, tv =>
                    string.Equals(tv.Name, tagName, StringComparison.Ordinal) &&
                    string.Equals(tv.Value, tagValue, StringComparison.Ordinal));
                if (matchCount > 1)
                    _logger?.LogWarning("Multiple tagged values named '{Name}' with value '{Value}' found on element {ElementId}; updating the first match.", tagName, tagValue, elementId);
                return match
                    ?? throw new InvalidOperationException($"Tagged value '{tagName}' ({tagValue}) not found on element {elementId}.");
            },
            tv => { tv.Notes = newNotesHtml; tv.Update(); },
            () =>
            {
                var reElement = _repository!.GetElementByID(elementId);
                if (reElement?.TaggedValues is not EA.Collection reTvs) return null;
                return Find<EA.TaggedValue>(reTvs, tv =>
                    string.Equals(tv.Name, tagName, StringComparison.Ordinal) &&
                    string.Equals(tv.Value, tagValue, StringComparison.Ordinal)).match?.Notes;
            },
            newNotesHtml,
            $"tagged value '{tagName}' ({tagValue}) on element {elementId}",
            "Updated tagged value '{Name}' notes on element {ElementId}",
            tagName, elementId);
    }
```

- [ ] **Step 5: Build and run the EaReader-related tests**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests --filter "FullyQualifiedName~EaReaderTests|FullyQualifiedName~WriteBackScannerTests" 2>&1 | Select-Object -Last 2
```
Expected: `Passed!  - Failed: 0, Passed: 42`.

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.EA/EaReader.cs
git commit -m "refactor(ea): dedup collection write-backs via Find helper (issue #86)"
```

---

### Task 3: FrontmatterParser — `RewriteMarkdown` + `UpdateNotesLike`, convert the three regex-based `Update*` methods, add `UpdatePackageNotes` tests

**Files:**
- Modify: `src/EAxWiki.Export/Helpers/FrontmatterParser.cs:183-210` (`UpdateNotes`), `236-259` (`UpdateRowNotes`), `279-306` (`UpdatePackageNotes`)
- Test: `src/EAxWiki.Tests/FrontmatterParserTests.cs` (add two `[Fact]`s + one page constant)

**Interfaces:**
- Produces: private helpers
  `RewriteMarkdown(string filePath, Func<string, string?> transform)` — read → normalize `\n` → transform → restore original EOL → atomic temp-file swap; a `null` transform result aborts without writing.
  `UpdateNotesLike(string filePath, string newNotesHtml, string startMarker, string endMarker)` — frontmatter `notes_hash` swap-or-append + content-marker swap + `Modified:` bump.
  Task 4 consumes `RewriteMarkdown`.

- [ ] **Step 1: Write the two new `UpdatePackageNotes` tests** (append to `src/EAxWiki.Tests/FrontmatterParserTests.cs`, after the `UpdateRowNotes_PatchesOnlyTheTargetRowsContentAndHash` test at line 180):

```csharp
    private const string SamplePackagePage = """
        ---
        package_id: 46
        notes_hash: e3b0c442
        ---

        # Assessments

        <div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="46" data-kind="package" data-file-path="Assessments/index.md" data-api-port="8001">
        <button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
        <div class="ea-notes-content">
        <!--ea-package-notes-start-->
        <p>Original package notes.</p>
        <!--ea-package-notes-end-->
        </div>
        </div>
        """;

    [Fact]
    public void UpdatePackageNotes_UpdatesHashAndContent()
    {
        File.WriteAllText(_filePath, SamplePackagePage);
        var expectedNewHash = HtmlHelpers.ComputeNotesHash("<p>Edited package notes.</p>");

        FrontmatterParser.UpdatePackageNotes(_filePath, "<p>Edited package notes.</p>");

        var text = File.ReadAllText(_filePath);
        Assert.Contains($"notes_hash: {expectedNewHash}", text);
        Assert.DoesNotContain("notes_hash: e3b0c442", text);
        Assert.Contains("<!--ea-package-notes-start-->\n<p>Edited package notes.</p>\n<!--ea-package-notes-end-->", text);
    }

    [Fact]
    public void UpdatePackageNotes_MissingFrontmatter_LeavesFileUntouched()
    {
        File.WriteAllText(_filePath, "# Just a heading\n\nSome content.");
        var before = File.ReadAllText(_filePath);

        FrontmatterParser.UpdatePackageNotes(_filePath, "<p>x</p>");

        Assert.Equal(before, File.ReadAllText(_filePath));
    }
```

- [ ] **Step 2: Run the two new tests to confirm they pass against the current implementation**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests --filter "FullyQualifiedName~UpdatePackageNotes" 2>&1 | Select-Object -Last 2
```
Expected: `Passed!  - Failed: 0, Passed: 2` (characterization tests; the current code already satisfies them).

- [ ] **Step 3: Add `RewriteMarkdown` and `UpdateNotesLike`** at the end of the class (after `UpdatePackageNotes`, before the closing brace at line 307):

```csharp
    /// <summary>
    /// Shared rewrite tail for every Update* method: read the file, normalize to \n, apply the transform,
    /// restore the file's original EOL, and atomically swap via a temp file so MkDocs never sees a partial
    /// file. A transform returning null means "nothing to change" and leaves the file untouched (preserves
    /// the pre-refactor early-return behavior).
    /// </summary>
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

    /// <summary>
    /// Shared implementation of UpdateNotes/UpdatePackageNotes: swap the notes_hash frontmatter field
    /// (appending it if absent), replace the content between the start/end markers, and bump the page's
    /// Modified date. The two public methods differ only in the marker names.
    /// </summary>
    private static void UpdateNotesLike(string filePath, string newNotesHtml, string startMarker, string endMarker)
    {
        RewriteMarkdown(filePath, text =>
        {
            var fmMatch = Regex.Match(text, @"\A---\n(.*?\n)---\n", RegexOptions.Singleline);
            if (!fmMatch.Success) return null;

            var newHash = HtmlHelpers.ComputeNotesHash(newNotesHtml);
            var fmBody = fmMatch.Groups[1].Value;
            fmBody = Regex.IsMatch(fmBody, @"^notes_hash:.*$", RegexOptions.Multiline)
                ? Regex.Replace(fmBody, @"^notes_hash:.*$", $"notes_hash: {newHash}", RegexOptions.Multiline)
                : fmBody + $"notes_hash: {newHash}\n";

            text = $"---\n{fmBody}---\n" + text[fmMatch.Length..];

            var contentPattern = new Regex(
                $@"({Regex.Escape(startMarker)}\n).*?(\n{Regex.Escape(endMarker)})", RegexOptions.Singleline);
            text = contentPattern.Replace(text, m => m.Groups[1].Value + newNotesHtml + m.Groups[2].Value, 1);

            return ModifiedDatePattern.Replace(text, $"${{1}}{DateTime.Now:yyyy-MM-dd}");
        });
    }
```

- [ ] **Step 4: Convert `UpdateNotes`** (replace lines 183-210):

```csharp
    public static void UpdateNotes(string filePath, string newNotesHtml) =>
        UpdateNotesLike(filePath, newNotesHtml, "<!--ea-notes-start-->", "<!--ea-notes-end-->");
```

- [ ] **Step 5: Convert `UpdateRowNotes`** (replace lines 236-259):

```csharp
    public static void UpdateRowNotes(string filePath, string rowId, string newNotesHtml) =>
        RewriteMarkdown(filePath, text =>
        {
            var newHash = HtmlHelpers.ComputeNotesHash(newNotesHtml);

            var hashPattern = new Regex($"(data-row-id=\"{Regex.Escape(rowId)}\"[^>]*?data-notes-hash=\")[^\"]*(\")");
            text = hashPattern.Replace(text, $"${{1}}{newHash}$2", 1);

            var contentPattern = new Regex(
                $@"(<!--ea-row-notes-start:{Regex.Escape(rowId)}-->).*?(<!--ea-row-notes-end:{Regex.Escape(rowId)}-->)",
                RegexOptions.Singleline);
            text = contentPattern.Replace(text, m => m.Groups[1].Value + newNotesHtml + m.Groups[2].Value, 1);

            return ModifiedDatePattern.Replace(text, $"${{1}}{DateTime.Now:yyyy-MM-dd}");
        });
```

- [ ] **Step 6: Convert `UpdatePackageNotes`** (replace lines 279-306):

```csharp
    public static void UpdatePackageNotes(string filePath, string newNotesHtml) =>
        UpdateNotesLike(filePath, newNotesHtml, "<!--ea-package-notes-start-->", "<!--ea-package-notes-end-->");
```

- [ ] **Step 7: Build and run the FrontmatterParser tests**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests --filter "FullyQualifiedName~FrontmatterParserTests" 2>&1 | Select-Object -Last 2
```
Expected: `Passed!  - Failed: 0, Passed: 17` (15 existing + 2 new).

- [ ] **Step 8: Commit**

```bash
git add src/EAxWiki.Export/Helpers/FrontmatterParser.cs src/EAxWiki.Tests/FrontmatterParserTests.cs
git commit -m "refactor(export): dedup markdown rewrite tail via RewriteMarkdown (issue #86)"
```

---

### Task 4: FrontmatterParser — route `UpdateStatus` through `RewriteMarkdown`

**Files:**
- Modify: `src/EAxWiki.Export/Helpers/FrontmatterParser.cs:66-118` (body of `UpdateStatus`)

**Interfaces:**
- Consumes: `RewriteMarkdown` from Task 3. The transform returns `null` when the leading `---` block is missing or has no closing delimiter, preserving the current early-returns.

- [ ] **Step 1: Convert `UpdateStatus`** (replace lines 66-118; keep the existing XML doc comment and the `StatusBadgePattern`/`StatusWidgetPattern`/`ModifiedDatePattern`/`PatchModifiedDate` members unchanged):

```csharp
    public static void UpdateStatus(string filePath, string newStatus) =>
        RewriteMarkdown(filePath, text =>
        {
            var lines = text.Split('\n').ToList();
            if (lines.Count < 2 || lines[0].Trim() != "---") return null;

            int end = -1;
            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i].Trim() == "---") { end = i; break; }
            }
            if (end < 0) return null;

            // 1. Update frontmatter
            var newHash = HtmlHelpers.ComputeStatusHash(newStatus);
            for (int i = 1; i < end; i++)
            {
                var sep = lines[i].IndexOf(':');
                if (sep < 1) continue;
                var key = lines[i][..sep].Trim();
                if (key.Equals("status", StringComparison.OrdinalIgnoreCase))
                    lines[i] = $"status: {newStatus}";
                else if (key.Equals("ea_hash", StringComparison.OrdinalIgnoreCase))
                    lines[i] = $"ea_hash: {newHash}";
            }

            // 2. Update page body: status badge and widget data-status attribute
            var newClass = $"status-{newStatus.ToLowerInvariant()}";

            for (int i = end + 1; i < lines.Count; i++)
            {
                if (lines[i].Contains("status-badge"))
                    lines[i] = StatusBadgePattern.Replace(lines[i],
                        $"class=\"status-badge {newClass}\">{newStatus}</span>");

                if (lines[i].Contains("id=\"ea-status-editor\""))
                    lines[i] = StatusWidgetPattern.Replace(lines[i],
                        $"data-status=\"{newStatus}\"");

                if (lines[i].Contains("**Modified:**"))
                    lines[i] = PatchModifiedDate(lines[i]);
            }

            return string.Join("\n", lines);
        });
```

- [ ] **Step 2: Build and run the FrontmatterParser tests**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests --filter "FullyQualifiedName~FrontmatterParserTests" 2>&1 | Select-Object -Last 2
```
Expected: `Passed!  - Failed: 0, Passed: 17`.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/Helpers/FrontmatterParser.cs
git commit -m "refactor(export): route UpdateStatus through RewriteMarkdown (issue #86)"
```

---

### Task 5: Full verification

**Files:** none (verification only).

- [ ] **Step 1: Full .NET suite**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests 2>&1 | Select-Object -Last 2
```
Expected: `Passed!  - Failed: 0, Passed: 300` (298 baseline + 2 new `UpdatePackageNotes` tests).

- [ ] **Step 2: Full Pester suite** (hold port 8000 so the serve tests behave, mirroring the established recipe; port 8001 must be free):

```powershell
$l = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, 8000); $l.Start();
try { Invoke-Pester tests/scripts/ -PassThru 2>&1 | Select-Object -Last 1 }
finally { $l.Stop() }
```
Expected: `Passed: 162` (unchanged baseline).

- [ ] **Step 3: Optional live gate (needs EA)** — run the write-back smoke test (start `serve-api.ps1 --output wiki`, round-trip a status + notes + row-notes change against a real exported page, then restore). If skipped, state that in the task report.

- [ ] **Step 4: Confirm a clean tree and push** (the whole plan is part of issue #86; pushing is the final step):

```bash
git status --porcelain
git log origin/master..master --oneline
git push origin master
```
Expected: only the four plan commits listed; push succeeds.

---

## Self-Review

**Spec coverage:**
- `Write<TEntity>` + `Find<T>` + conversion of all seven `Update…` methods → Tasks 1-2. ✓
- `RewriteMarkdown` + `UpdateNotesLike` + all four `Update…` methods → Tasks 3-4. ✓
- Behavior-preservation guarantees (exception messages, warnings, log templates, CRLF, atomicity, `Modified:` bump, no-op writes) → enforced by verbatim code in each task + existing tests. ✓
- New `UpdatePackageNotes` unit test → Task 3 Step 1-2. ✓
- Acceptance criteria (300 .NET, 162 Pester, optional smoke gate, clean push) → Task 5. ✓

**Placeholder scan:** every code step contains full code; no TBD/TODO/lazy instructions. ✓

**Type consistency:** `Write<TEntity>`/`Find<T>`/`RewriteMarkdown`/`UpdateNotesLike` signatures are identical in the task that defines them and the tasks that consume them; the `Find<T>` calls use explicit type arguments (`Find<EA.Attribute>` etc.) because the predicate lambda cannot infer `T`. ✓
