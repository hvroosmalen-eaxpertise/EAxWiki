# Export Output Validation Pass — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a post-export validation pass that checks each written `.md` file for content quality (frontmatter, HTML tags, links, image paths, non-empty), and includes the validation summary in the Finish webhook alert.

**Architecture:** A new `ExportValidator` class iterates over a `WrittenMdFiles` collection (populated by each writer as files are written), runs 5 validation checks, writes a `.validation-report.json` file to the output directory, and logs the summary via `ILogger`. The PowerShell monitor script reads the report and includes warnings/errors in the Finish Slack/Teams alert.

**Tech Stack:** .NET 10, YamlDotNet v16.3.0, PowerShell 7+, xUnit + Moq

## Global Constraints

- All existing tests must pass after each task
- Must compile with `dotnet build` on Windows
- `ExportContext` is an `internal record` — new members follow same convention
- `ExportValidator` is `internal` — only consumed by `MarkdownExporter`
- The validation report file is `.validation-report.json` in the output directory (dot-prefixed, gitignored, not served)
- Only `.md` files written during THIS export run are validated (not pre-existing files)
- PNG file validation checks only that the referenced path exists (no binary content checks)

---

### Task 1: Track Written .md Files in ExportContext

**Files:**
- Modify: `src/EAxWiki.Export/ExportContext.cs` (add collection)
- Modify: `src/EAxWiki.Export/Exporters/ElementPageWriter.cs` (track writes)
- Modify: `src/EAxWiki.Export/Exporters/DiagramExporter.cs` (track writes)
- Modify: `src/EAxWiki.Export/Exporters/PackageExporter.cs` (track index.md writes)
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (track root index.md write)

**Interfaces:**
- Consumes: `ExportContext` (existing record)
- Produces: `ConcurrentBag<string> WrittenMdFiles` on `ExportContext`

- [ ] **Step 1: Add `WrittenMdFiles` to ExportContext**

```csharp
// In ExportContext.cs, after RegisteredElementFiles (~line 30):
public ConcurrentBag<string> WrittenMdFiles { get; } = new();
```

- [ ] **Step 2: Track writes in ElementPageWriter**

After each `writer.WriteFileAsync(...)` call for a `.md` file (after the up-to-date skip check), add the file path. The element `.md` is written at the end of the method with a path like `mdPath` or derived from the element's directory. After the write completes:

```csharp
ctx.WrittenMdFiles.Add(mdPath);
```

- [ ] **Step 3: Track writes in DiagramExporter**

After each `writer.WriteFileAsync(mdPath, ...)` on line 123 (diagram page .md) and line 180 (diagrams index .md), add:

```csharp
ctx.WrittenMdFiles.Add(mdPath);
```

- [ ] **Step 4: Track writes in PackageExporter**

After each package `index.md` write (look for `WritePackageIndexFile` or inline write call that produces `indexPath`), add:

```csharp
ctx.WrittenMdFiles.Add(indexPath);
```

- [ ] **Step 5: Track root index.md in MarkdownExporter**

After `_writer.WriteFileAsync(Path.Combine(outputDir, "index.md"), ...)` on line 174, add:

```csharp
ctx.WrittenMdFiles.Add(Path.Combine(outputDir, "index.md"));
```

- [ ] **Step 6: Build and verify**

Run: `dotnet build src/EAxWiki.Export`
Expected: Build succeeds.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.Export/ExportContext.cs src/EAxWiki.Export/Exporters/ElementPageWriter.cs src/EAxWiki.Export/Exporters/DiagramExporter.cs src/EAxWiki.Export/Exporters/PackageExporter.cs src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: track written .md files in ExportContext.WrittenMdFiles"
```

---

### Task 2: ExportValidator Implementation

**Files:**
- Create: `src/EAxWiki.Export/ExportValidator.cs`
- Create: `src/EAxWiki.Tests/ExportValidatorTests.cs`

**Interfaces:**
- Consumes: `ConcurrentBag<string> WrittenMdFiles` from ExportContext, `ILogger`
- Produces: `ValidationReport` record with counts and per-file issues

- [ ] **Step 1: Define data types and class**

ExportValidator.cs:

```csharp
using System.Text.RegularExpressions;
using System.Text.Json;
using EAxWiki.Export.Helpers;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Export;

internal record ValidationIssue(string File, string Type, string Message);

internal record ValidationReport(
    int FilesValidated,
    int Passed,
    int Warnings,
    int Errors,
    List<ValidationIssue> Issues
);

internal static class ExportValidator
{
    public static ValidationReport Validate(IEnumerable<string> files, string outputPath)
    {
        var issues = new List<ValidationIssue>();
        var passed = 0;
        var validated = 0;

        foreach (var file in files)
        {
            if (!File.Exists(file)) continue;
            validated++;
            var fileIssues = ValidateFile(file, outputPath);
            if (fileIssues.Count == 0)
                passed++;
            else
                issues.AddRange(fileIssues);
        }

        var warnings = issues.Count(i => i.Type == "warning");
        var errors = issues.Count(i => i.Type == "error");
        return new ValidationReport(validated, passed, warnings, errors, issues);
    }

    private static List<ValidationIssue> ValidateFile(string filePath, string outputPath)
    {
        var issues = new List<ValidationIssue>();
        var relativePath = Path.GetRelativePath(outputPath, filePath);
        var content = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(content))
        {
            issues.Add(new ValidationIssue(relativePath, "error", "File is empty"));
            return issues;
        }

        var fm = FrontmatterParser.Parse(content);
        if (fm.Count == 0 && content.Contains("---"))
        {
            issues.Add(new ValidationIssue(relativePath, "error", "YAML frontmatter is present but could not be parsed"));
        }

        var body = StripFrontmatter(content);

        foreach (var tag in FindUnclosedTags(body))
            issues.Add(new ValidationIssue(relativePath, "error", $"Unclosed <{tag}> tag"));

        foreach (Match match in Regex.Matches(body, @"\]\(([^)]+)\)"))
        {
            var link = match.Groups[1].Value;
            if (IsExternalUrl(link) || link.StartsWith("#")) continue;
            var target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath)!, link));
            if (!File.Exists(target))
                issues.Add(new ValidationIssue(relativePath, "warning", $"Broken link: {link}"));
        }

        foreach (Match match in Regex.Matches(body, @"<img\s+[^>]*src=""([^""]+)"""))
        {
            var src = match.Groups[1].Value;
            if (IsExternalUrl(src)) continue;
            var imgPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath)!, src));
            if (!File.Exists(imgPath))
                issues.Add(new ValidationIssue(relativePath, "warning", $"Missing image: {src}"));
        }

        return issues;
    }

    private static bool IsExternalUrl(string value) =>
        value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

    private static string StripFrontmatter(string content)
    {
        var match = Regex.Match(content, @"\A---\s*\n.*?\n---\s*\n", RegexOptions.Singleline);
        return match.Success ? content[match.Length..] : content;
    }

    private static List<string> FindUnclosedTags(string html)
    {
        var tagsToCheck = new[] { "div", "span", "details", "summary", "table", "tr", "td", "th", "a", "p", "ul", "ol", "li", "pre", "code", "select", "option" };
        var unclosed = new List<string>();
        foreach (var tag in tagsToCheck)
        {
            var opens = Regex.Matches(html, $"<{tag}(\\s[^>]*)?>", RegexOptions.IgnoreCase).Count;
            var closes = Regex.Matches(html, $"</{tag}>", RegexOptions.IgnoreCase).Count;
            if (opens > closes)
                unclosed.Add(tag);
        }
        return unclosed;
    }

    public static string ToJson(ValidationReport report) =>
        JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
}
```

- [ ] **Step 2: Add unit tests**

ExportValidatorTests.cs:

```csharp
using EAxWiki.Export;

namespace EAxWiki.Tests;

public class ExportValidatorTests
{
    [Fact]
    public void Validate_EmptyFile_ReturnsError()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "empty.md");
        File.WriteAllText(file, "");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Errors);
        Assert.Equal(0, report.Passed);
        Assert.Contains(report.Issues, i => i.Message == "File is empty");
    }

    [Fact]
    public void Validate_ValidElementPage_ReturnsPassed()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "valid.md");
        File.WriteAllText(file, @"---
ea_id: 123
status: Approved
status_options: [Proposed, Approved]
ea_hash: abc
notes_hash: def
---
# Title
Some content.");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(0, report.Errors);
        Assert.Equal(0, report.Warnings);
        Assert.Equal(1, report.Passed);
    }

    [Fact]
    public void Validate_UnclosedDiv_ReturnsError()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "unclosed.md");
        File.WriteAllText(file, "# Title\n<div>unclosed content");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Errors);
        Assert.Contains(report.Issues, i => i.Message.Contains("div"));
    }

    [Fact]
    public void Validate_BrokenLink_ReturnsWarning()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "links.md");
        File.WriteAllText(file, "# Title\nSee [missing](nonexistent.md) for details.");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Warnings);
        Assert.Contains(report.Issues, i => i.Message.Contains("nonexistent.md"));
    }

    [Fact]
    public void Validate_MissingImage_ReturnsWarning()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "img.md");
        File.WriteAllText(file, @"# Title
<img src=""missing.png"" alt=""test"">");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Warnings);
        Assert.Contains(report.Issues, i => i.Message.Contains("missing.png"));
    }

    [Fact]
    public void Validate_ExistingImage_NoWarning()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "img.md");
        File.WriteAllText(file, @"# Title
<img src=""present.png"" alt=""test"">");
        File.WriteAllText(Path.Combine(dir.Path, "present.png"), "fake-png");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(0, report.Warnings);
        Assert.Equal(1, report.Passed);
    }

    [Fact]
    public void ToJson_ProducesValidJson()
    {
        var report = new ValidationReport(10, 8, 1, 1,
            new List<ValidationIssue> { new("test.md", "error", "Unclosed <div> tag") });

        var json = ExportValidator.ToJson(report);
        Assert.Contains("\"FilesValidated\": 10", json);
        Assert.Contains("\"Errors\": 1", json);
    }
}

/// <summary>Helper that creates a temp directory and cleans it up on Dispose.</summary>
internal sealed class TempDir : IDisposable
{
    public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
    public TempDir() => Directory.CreateDirectory(Path);
    public void Dispose()
    {
        try { Directory.Delete(Path, true); } catch { }
    }
}
```

- [ ] **Step 3: Build and run tests**

Run: `dotnet build src/EAxWiki.Export src/EAxWiki.Tests`
Expected: Build succeeds.

Run: `dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~ExportValidatorTests"`
Expected: All 7 tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.Export/ExportValidator.cs src/EAxWiki.Tests/ExportValidatorTests.cs
git commit -m "feat: add ExportValidator with 5 validation checks and tests"
```

---

### Task 3: Wire Validation into MarkdownExporter

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (hook after orphan cleanup)

**Interfaces:**
- Consumes: `ExportValidator`, `ExportContext.WrittenMdFiles`
- Produces: `.validation-report.json` file in output dir, validation summary in ILogger output

- [ ] **Step 1: Add validation call after orphan cleanup**

In `MarkdownExporter.cs`, around line 105, after `await InfrastructureWriter.CleanupOrphanedFilesAsync(ctx, cancellationToken);`, add:

```csharp
var report = ExportValidator.Validate(ctx.WrittenMdFiles, outputPath);
var reportPath = Path.Combine(outputPath, ".validation-report.json");
await File.WriteAllTextAsync(reportPath, ExportValidator.ToJson(report), cancellationToken);

if (report.Errors > 0 || report.Warnings > 0)
{
    _logger.LogWarning("Validation: {Validated} files checked, {Passed} passed, {Warnings} warnings, {Errors} errors",
        report.FilesValidated, report.Passed, report.Warnings, report.Errors);
    foreach (var issue in report.Issues)
    {
        _logger.LogWarning("  [{Type}] {File}: {Message}", issue.Type, issue.File, issue.Message);
    }
}
else
{
    _logger.LogInformation("Validation: {Validated} files checked, all passed", report.FilesValidated);
}
```

- [ ] **Step 2: Build and verify**

Run: `dotnet build src/EAxWiki.Export`
Expected: Build succeeds.

- [ ] **Step 3: Run all tests**

Run: `dotnet test src/EAxWiki.Tests`
Expected: All tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: wire ExportValidator into MarkdownExporter after orphan cleanup"
```

---

### Task 4: Monitor Script — Include Validation Summary in Finish Alert

**Files:**
- Modify: `scripts/monitor-export-and-serve.ps1` (Finish alert section, around line 567)

**Interfaces:**
- Consumes: `.validation-report.json` written by C# validator
- Produces: Enhanced Finish alert with validation summary

- [ ] **Step 1: Add validation report reading function**

After `Get-DiagramCount` (around line 371), add:

```powershell
function Get-ValidationReport {
    param([string]$WikiDir)
    $reportPath = Join-Path $WikiDir ".validation-report.json"
    if (-not (Test-Path $reportPath)) { return $null }
    try { return Get-Content $reportPath -Raw | ConvertFrom-Json } catch { return $null }
}
```

- [ ] **Step 2: Read and include validation data in Finish alert**

Around lines 567-575, modify:

```powershell
$diagramCount = Get-DiagramCount
$pageDelta = $elementCount - $previousCount
$deltaLabel = if ($pageDelta -ge 0) { "+$pageDelta" } else { "$pageDelta" }
$state.lastDiagramCount = $diagramCount

$validationReport = Get-ValidationReport -WikiDir $WikiDir
$validationSuffix = ""
if ($validationReport -and $validationReport.Errors -gt 0) {
    $validationSuffix = " | Validation: $($validationReport.Warnings) warning(s), $($validationReport.Errors) error(s)"
}
elseif ($validationReport -and $validationReport.Warnings -gt 0) {
    $validationSuffix = " | Validation: $($validationReport.Warnings) warning(s)"
}

if ($NotifyOnStart) {
    Send-Alert -Kind Finish -Message ("Export finished in {0} - {1} page(s) total ({2} diagram, {3} element), {4} vs previous run.{5}" -f `
        $exportStopwatch.Elapsed.ToString('mm\:ss'), $elementCount, $diagramCount,
        ($elementCount - $diagramCount), $deltaLabel, $validationSuffix)
}
```

- [ ] **Step 3: Verify PowerShell syntax**

Run: `pwsh -NoProfile -Command "& 'scripts/monitor-export-and-serve.ps1' --help"`
Expected: Script loads without syntax errors.

- [ ] **Step 4: Commit**

```bash
git add scripts/monitor-export-and-serve.ps1
git commit -m "feat: include validation summary in Finish webhook alert"
```

---

### Self-Review Checklist

1. **Spec coverage:** All items covered — written file tracking (Task 1), 5 validation checks (Task 2), post-export hook + report file (Task 3), webhook Finish alert integration (Task 4).
2. **Placeholder scan:** No TBD, TODO, or vague steps. All code is explicit with exact paths and line references.
3. **Type consistency:** `ExportContext.WrittenMdFiles` is `ConcurrentBag<string>`, `ExportValidator.Validate()` returns `ValidationReport`, `ToJson()` returns `string`. All names consistent across tasks.
