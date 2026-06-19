# Language-Grouped Types Pages — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Regroup the `types/` pages by modelling language extracted from the element stereotype's `::` prefix.

**Architecture:** Add a `ParseStereotype` helper to `MarkdownExporter`, then rewrite `GenerateTypesPagesAsync` to write language subdirectories instead of flat files.

**Tech Stack:** C# .NET 10, no new dependencies

## Global Constraints

- All user-derived filenames sanitized via `SanitizeName()`
- Empty/whitespace stereotypes → language `"UML"`, type `"Uncategorized"`
- Relative element links use `../` from types root, `../../` from language subdirectories
- `.pages` files for collapsible nav sections per language
- Requires `--force` flag to regenerate (file structure changes)
- Target framework: `net10.0`

---

### Task 1: Add `ParseStereotype` helper method

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` — add private static method before `GenerateTypesPagesAsync`

**Interfaces:**
- Produces: `private static (string Language, string Type) ParseStereotype(string stereotype)`

- [ ] **Step 1: Add the helper method**

```csharp
private static (string Language, string Type) ParseStereotype(string stereotype)
{
    if (string.IsNullOrWhiteSpace(stereotype))
        return ("UML", "Uncategorized");

    string language, type;

    var separatorIndex = stereotype.IndexOf("::", StringComparison.Ordinal);
    if (separatorIndex >= 0)
    {
        language = stereotype[..separatorIndex];
        type = stereotype[(separatorIndex + 2)..];
    }
    else
    {
        language = "UML";
        type = stereotype;
    }

    var baseName = new string(language.TakeWhile(c => !char.IsDigit(c)).ToArray());
    if (!string.IsNullOrEmpty(baseName) && type.StartsWith(baseName + "_", StringComparison.OrdinalIgnoreCase))
    {
        type = type[(baseName.Length + 1)..];
    }

    return (language, type);
}
```

- [ ] **Step 2: Build to verify compilation**

```powershell
dotnet build src\EAxWiki.Export\EAxWiki.Export.csproj
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add ParseStereotype helper for language-prefixed stereotypes"
```

---

### Task 2: Rewrite `GenerateTypesPagesAsync` with language grouping

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` — replace entire `GenerateTypesPagesAsync` method (lines 215-279)

**Interfaces:**
- Consumes: `ParseStereotype(string)` from Task 1
- Produces: Language-subdirectory structure under `types/`, each with `.pages`, `index.md`, and type pages

- [ ] **Step 1: Replace the method body**

Replace the current `GenerateTypesPagesAsync` with:

```csharp
private async Task GenerateTypesPagesAsync(List<(EaElement Element, string PackageDir)> elements, string outputDir)
{
    var typesDir = Path.Combine(outputDir, "types");
    await _writer.CreateDirectoryAsync(typesDir);

    var typesStopwatch = Stopwatch.StartNew();

    var parsed = elements
        .Select(e =>
        {
            var (language, type) = ParseStereotype(e.Element.Stereotype);
            return (e.Element, e.PackageDir, Language: language, Type: type);
        })
        .ToList();

    var languages = parsed.Select(x => x.Language).Distinct().OrderBy(l => l).ToList();

    _logger.LogInformation("Generating type pages across {LanguageCount} languages", languages.Count);

    // Root types/index.md — list languages
    var indexLines = new List<string>
    {
        "# Types",
        string.Empty,
        "Elements grouped by modelling language:",
        string.Empty,
    };

    foreach (var lang in languages)
    {
        indexLines.Add($"- [{lang}]({SanitizeName(lang)}/index.md)");
    }

    indexLines.Add(string.Empty);
    indexLines.Add(FormatTimestamp());
    await _writer.WriteFileAsync(Path.Combine(typesDir, "index.md"), string.Join(Environment.NewLine, indexLines));

    // Per-language pages
    foreach (var lang in languages)
    {
        var langDir = Path.Combine(typesDir, SanitizeName(lang));
        await _writer.CreateDirectoryAsync(langDir);

        // .pages for this language
        var langPages = new List<string>
        {
            $"title: {lang}",
            string.Empty,
        };
        await _writer.WriteFileAsync(Path.Combine(langDir, ".pages"), string.Join(Environment.NewLine, langPages));

        var typeGroups = parsed
            .Where(x => x.Language == lang)
            .GroupBy(x => x.Type)
            .OrderBy(g => g.Key)
            .ToList();

        // Language index.md — list types
        var langIndexLines = new List<string>
        {
            $"# {lang}",
            string.Empty,
            $"{typeGroups.Sum(g => g.Count())} element(s) across {typeGroups.Count} type(s):",
            string.Empty,
        };

        foreach (var group in typeGroups)
        {
            langIndexLines.Add($"- [{group.Key}]({SanitizeName(group.Key)}.md)");
        }

        langIndexLines.Add(string.Empty);
        langIndexLines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(langDir, "index.md"), string.Join(Environment.NewLine, langIndexLines));

        // Per-type pages
        foreach (var group in typeGroups)
        {
            var lines = new List<string>
            {
                $"# {group.Key}",
                string.Empty,
                $"{group.Count()} element(s):",
                string.Empty,
            };

            foreach (var item in group)
            {
                var elemName = SanitizeName(item.Element.Name);
                var relativeDir = Path.GetRelativePath(outputDir, item.PackageDir);
                var relativePath = $"../../{relativeDir}/{elemName}.md".Replace('\\', '/');
                lines.Add($"- [{item.Element.Name}]({relativePath})");
            }

            lines.Add(string.Empty);
            lines.Add(FormatTimestamp());
            await _writer.WriteFileAsync(Path.Combine(langDir, $"{SanitizeName(group.Key)}.md"), string.Join(Environment.NewLine, lines));
        }
    }

    typesStopwatch.Stop();
    _logger.LogInformation("Generated type pages across {LanguageCount} languages in {ElapsedMs}ms",
        languages.Count, typesStopwatch.ElapsedMilliseconds);
}
```

- [ ] **Step 2: Build to verify compilation**

```powershell
dotnet build src\EAxWiki.Export\EAxWiki.Export.csproj
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: group types pages by modelling language (Issue #17)"
```

---

### Task 3: Final verification

- [ ] **Step 1: Push and close issue**

```bash
git push
```

- [ ] **Step 2: Close issue #17 with comment**

```bash
gh issue close 17 --comment "Implemented language-grouped types pages. Run with --force to regenerate the types/ directory structure."
```
