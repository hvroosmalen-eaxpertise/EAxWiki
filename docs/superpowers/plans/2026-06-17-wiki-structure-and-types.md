# Wiki Structure & Types Views Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use subagent-driven-development (recommended) or executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add two navigation views to the MkDocs wiki: a "Structure" section (existing package hierarchy) and a "Types" section (elements grouped by Stereotype), via a `.pages` file and generated `types/` pages.

**Architecture:** Modify `MarkdownExporter` to collect all elements during the recursive package export, then generate a `types/` directory with stereotype-grouped index pages and a root `.pages` file. No existing file paths change.

**Tech Stack:** .NET 10, C#, MkDocs + Material theme + awesome-pages plugin

## Global Constraints

- Use `SanitizeName()` for all file/folder names derived from user content
- Stereotype `""` or whitespace groups under `"Uncategorized"`
- Relative links between `types/` pages and element files use `../` prefix
- Target framework: `net10.0`

---

### Task 1: Collect elements during export and generate Types pages

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

**Interfaces:**
- Consumes: `IOutputWriter.CreateDirectoryAsync(path)`, `IOutputWriter.WriteFileAsync(path, content)`, `EaElement.Stereotype`, `EaElement.Name`, `SanitizeName(name)`
- Produces: `types/index.md` and `types/{Stereotype}.md` files in the output directory

- [ ] **Step 1: Modify `ExportAsync` to collect elements and generate types**

Replace the `ExportAsync` and `ExportPackageAsync` signatures to pass a shared element list, and add a `GenerateTypesPagesAsync` call at the end.

Edit `MarkdownExporter.cs`:

Replace lines 15-25:
```csharp
    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath)
    {
        var packages = startPackage != null
            ? new List<EaPackage> { startPackage }
            : repository.RootPackages;

        foreach (var pkg in packages)
        {
            await ExportPackageAsync(pkg, outputPath);
        }
    }
```

With:
```csharp
    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath)
    {
        var packages = startPackage != null
            ? new List<EaPackage> { startPackage }
            : repository.RootPackages;

        var elements = new List<(EaElement Element, string PackageDir)>();

        foreach (var pkg in packages)
        {
            await ExportPackageAsync(pkg, outputPath, elements);
        }

        await GenerateTypesPagesAsync(elements, outputPath);
    }
```

- [ ] **Step 2: Update `ExportPackageAsync` signature and element tracking**

Replace the method signature (line 27) and add element tracking inside the elements loop (line 52 area):

Change `private async Task ExportPackageAsync(EaPackage package, string outputDir)` to:
```csharp
    private async Task ExportPackageAsync(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements)
```

After line 52 (`await WriteElementAsync(elem, dir);`), add:
```csharp
        elements.Add((elem, dir));
```

Update the recursive call on line 93 from:
```csharp
        await ExportPackageAsync(child, outputDir);
```
To:
```csharp
        await ExportPackageAsync(child, outputDir, elements);
```

- [ ] **Step 3: Add `GenerateTypesPagesAsync` method**

Add after the `ExportPackageAsync` method (after line 95), before `WriteElementAsync`:

```csharp
    private async Task GenerateTypesPagesAsync(List<(EaElement Element, string PackageDir)> elements, string outputDir)
    {
        var typesDir = Path.Combine(outputDir, "types");
        await _writer.CreateDirectoryAsync(typesDir);

        var stereotypes = elements
            .Select(e => string.IsNullOrWhiteSpace(e.Element.Stereotype) ? "Uncategorized" : e.Element.Stereotype)
            .Distinct()
            .OrderBy(s => s)
            .ToList();

        var indexLines = new List<string>
        {
            "# Types",
            string.Empty,
            "Elements grouped by stereotype:",
            string.Empty,
        };

        foreach (var stereo in stereotypes)
        {
            indexLines.Add($"- [{stereo}]({SanitizeName(stereo)}.md)");
        }

        indexLines.Add(string.Empty);
        await _writer.WriteFileAsync(Path.Combine(typesDir, "index.md"), string.Join(Environment.NewLine, indexLines));

        foreach (var stereo in stereotypes)
        {
            var matching = elements
                .Where(e => (string.IsNullOrWhiteSpace(e.Element.Stereotype) ? "Uncategorized" : e.Element.Stereotype) == stereo)
                .ToList();

            var lines = new List<string>
            {
                $"# {stereo}",
                string.Empty,
                $"{matching.Count} element(s) with this stereotype:",
                string.Empty,
            };

            foreach (var (elem, pkgDir) in matching)
            {
                var elemName = SanitizeName(elem.Name);
                var relativeDir = Path.GetRelativePath(outputDir, pkgDir);
                var relativePath = $"../{relativeDir}/{elemName}.md".Replace('\\', '/');
                lines.Add($"- [{elem.Name}]({relativePath})");
            }

            lines.Add(string.Empty);
            await _writer.WriteFileAsync(Path.Combine(typesDir, $"{SanitizeName(stereo)}.md"), string.Join(Environment.NewLine, lines));
        }
    }
```

- [ ] **Step 4: Build and verify no errors**

Run:
```
dotnet build --nologo
```

Expected: Build succeeded, 0 warnings, 0 errors.

- [ ] **Step 5: Commit**

```
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: generate types pages grouped by stereotype during export"
```

---

### Task 2: Generate the .pages navigation file

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

**Interfaces:**
- Consumes: `outputPath` from `ExportAsync`
- Produces: `{outputPath}/.pages` YAML file

- [ ] **Step 1: Add `WritePagesFileAsync` method**

Add after `GenerateTypesPagesAsync` method:

```csharp
    private async Task WritePagesFileAsync(string outputDir)
    {
        var pagesPath = Path.Combine(outputDir, ".pages");
        var content = new List<string>
        {
            "nav:",
            "  - Structure: ''",
            "  - Types: types/",
            string.Empty,
        };
        await _writer.WriteFileAsync(pagesPath, string.Join(Environment.NewLine, content));
    }
```

- [ ] **Step 2: Call it from `ExportAsync`**

In `ExportAsync`, after the `GenerateTypesPagesAsync` call (and still inside the method), add:

```csharp
        await WritePagesFileAsync(outputPath);
```

- [ ] **Step 3: Build and verify**

```
dotnet build --nologo
```

Expected: Build succeeded.

- [ ] **Step 4: Commit**

```
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: generate .pages nav file with Structure and Types sections"
```

---

### Task 3: Verify full export produces correct output

**Files:** (read-only)
- `wiki/.pages`
- `wiki/types/index.md`
- `wiki/types/{Stereotype}.md` (one or more files)

- [ ] **Step 1: Run the exporter**

If the EA model file exists at the default path, run:
```
dotnet run --project src/EAxWiki
```

Otherwise run with the correct repo path:
```
dotnet run --project src/EAxWiki -- --repo "path/to/EurSuRA.qea"
```

- [ ] **Step 2: Inspect generated files**

Check that these exist:
- `wiki/.pages` — contains `nav:` with Structure and Types entries
- `wiki/types/index.md` — lists all stereotypes
- `wiki/types/*.md` — a file per stereotype with correct relative links (`../PackageName/ElementName.md`)

```
Get-ChildItem -Path wiki/types -Filter *.md
Get-Content wiki/.pages
```

- [ ] **Step 3: Serve and verify navigation**

```
.\scripts\serve-mkdocs.ps1 -Port 8000
```

Open http://localhost:8000 and confirm:
- Sidebar shows "Structure" (with the package hierarchy expandable)
- Sidebar shows "Types" as a second section
- Clicking a stereotype page lists elements with working links
- Element pages still render correctly with no broken links

- [ ] **Step 4: Commit**

```
git add wiki/.pages wiki/types/
git commit -m "feat: add initial wiki navigation structure and types pages"
```
