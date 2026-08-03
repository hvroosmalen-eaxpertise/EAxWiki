# EurSuRA Branding Implementation Plan (Issue #79)

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a configurable `--brand` option to the exporter, ship EurSuRA as the first brand (logo, palette, Geist fonts, branded graph/widget colors), and apply it to this repo's demo wiki.

**Architecture:** The exporter reads the brand from env `EAXWIKI_BRAND` (set by `Program.cs` from CLI → `.eaxwiki`), then `InfrastructureWriter` emits a branded `brand.css`, a `assets/eursura-logo.png`, and a parameterized `EA_LAYER_COLORS`/`EA_LAYER_DARK_TEXT` in `graph-init.js` when brand == `eursura`. The static `mkdocs.yml` references `brand.css` + the logo harmlessly (MkDocs skips missing files — verified exit 0). No brand → byte-identical output to today.

**Tech Stack:** .NET 10, C#, MkDocs Material 9.7, PowerShell 7 (Pester 5), embedded resources.

## Global Constraints

- No `--brand` / unknown brand → output byte-identical to the current neutral export.
- Brand value `eursura` is case-sensitive (exact lowercase string).
- `mkdocs.yml` stays neutral for other users — only gains two harmless references.
- All EurSuRA styling lives in `brand.css` + `graph-init.js` parameterization; `extra.css` is NOT modified.
- Brand colors (from issue #79): Light Cyan `#C4E5E7`, Jet Black `#103135`, Platinum `#F3F7F7`, Opal `#A8C6C7`, Lime Cream `#D0F391`.
- Fonts: Geist (body/headings), Geist Mono (code) — loaded via Google Fonts `@import` in `brand.css` only.
- Do not modify `SchedulerUI` or the wizard prompts (out of scope).
- Follow existing code style: no comments in code unless the surrounding code has them; DPAPI `.eaxwiki` handling must match `LocalConfigStore`.

---

### Task 1: `--brand` flag plumbing (Config → .eaxwiki → env)

**Files:**
- Modify: `src/EAxWiki/Config.cs:15-22` (property) and `:108` (help case region)
- Modify: `src/EAxWiki.Core/Configuration/LocalConfigStore.cs:27-42` (Config record)
- Modify: `src/EAxWiki/Program.cs:155-170` (savedConfig fallback) and `:216-219` (env set)
- Test: `src/EAxWiki.Tests/ConfigTests.cs`, `src/EAxWiki.Tests/LocalConfigStoreTests.cs`

**Interfaces:**
- Consumes: nothing new.
- Produces: `Config.Brand` (string, default `""`), `LocalConfigStore.Config.Brand` (string?, null), and env var `EAXWIKI_BRAND` set by Program.cs before export. Task 3 reads this env var.

- [ ] **Step 1: Write the failing tests**

Add to `src/EAxWiki.Tests/ConfigTests.cs` (after the existing `Load_ApiPortFlag_SetsApiPort` test, ~line 128):

```csharp
[Fact]
public void Load_BrandFlag_SetsBrand()
{
    var cfg = new Config();
    cfg.Load(["--brand", "eursura"]);
    Assert.Equal("eursura", cfg.Brand);
}

[Fact]
public void Load_NoBrand_DefaultsToEmpty()
{
    var cfg = new Config();
    cfg.Load([]);
    Assert.Equal("", cfg.Brand);
}
```

Add to `src/EAxWiki.Tests/LocalConfigStoreTests.cs` (mirroring the existing `ApiPort` round-trip test around line 32):

```csharp
[Fact]
public void Brand_RoundTrips()
{
    var dir = Path.Combine(Path.GetTempPath(), "eaxwiki_cfg_" + Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(dir);
    try
    {
        var path = Path.Combine(dir, ".eaxwiki");
        LocalConfigStore.Save(path, new LocalConfigStore.Config { RepoPath = "x.qea", Brand = "eursura" });
        var loaded = LocalConfigStore.Load(path, out _);
        Assert.Equal("eursura", loaded.Brand);
    }
    finally { Directory.Delete(dir, recursive: true); }
}
```

- [ ] **Step 2: Run tests to verify they fail**

Run: `dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~ConfigTests|FullyQualifiedName~LocalConfigStoreTests" --no-restore`
Expected: 2 ConfigTests + 1 LocalConfigStoreTests FAIL (no `Brand` member).

- [ ] **Step 3: Add `Brand` to `Config.cs`**

Add after the `AiKey` property (line 21):

```csharp
    public string Brand { get; set; } = "";
```

Add to the `Load` switch, after the `--ai-key` case (line 103-107):

```csharp
                case "--brand":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    Brand = args[++i];
                    break;
```

- [ ] **Step 4: Add `Brand` to `LocalConfigStore.Config`**

Add after `AiKey` (line 39):

```csharp
        public string? Brand { get; set; }
```

- [ ] **Step 5: Add fallback + env set in `Program.cs`**

In the `if (savedConfig != null)` block (lines 158-170), add as the last fallback:

```csharp
    if (string.IsNullOrEmpty(config.Brand) && !string.IsNullOrEmpty(savedConfig.Brand))
        config.Brand = savedConfig.Brand;
```

At the env-set site (after `Environment.SetEnvironmentVariable("EAXWIKI_AI_ENDPOINT", ...)` line 219), add:

```csharp
Environment.SetEnvironmentVariable("EAXWIKI_BRAND", config.Brand);
```

- [ ] **Step 6: Add `--brand` to help text**

In `Program.cs` around line 311 (the `--api-port` help line), add:

```csharp
    Console.WriteLine("  --brand <name>        Brand theme to emit (eursura); default: none");
```

- [ ] **Step 7: Run tests to verify they pass**

Run: `dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~ConfigTests|FullyQualifiedName~LocalConfigStoreTests" --no-restore`
Expected: PASS.

- [ ] **Step 8: Commit**

```bash
git add src/EAxWiki/Config.cs src/EAxWiki/Program.cs src/EAxWiki.Core/Configuration/LocalConfigStore.cs src/EAxWiki.Tests/ConfigTests.cs src/EAxWiki.Tests/LocalConfigStoreTests.cs
git add --renormalize src/EAxWiki/Config.cs src/EAxWiki/Program.cs src/EAxWiki.Core/Configuration/LocalConfigStore.cs src/EAxWiki.Tests/ConfigTests.cs src/EAxWiki.Tests/LocalConfigStoreTests.cs
git commit -m "feat(config): add --brand option (issue #79)"
```

---

### Task 2: Add brand embedded resources (brand-eursura.css + logo)

**Files:**
- Create: `src/EAxWiki.Export/Resources/brand-eursura.css`
- Create: `src/EAxWiki.Export/Resources/eursura-logo.png` (downloaded from the issue attachment)
- Modify: `src/EAxWiki.Export/EAxWiki.Export.csproj:19-22`

**Interfaces:**
- Consumes: Task 1's `EAXWIKI_BRAND` env var naming.
- Produces: embedded resources named `*.brand-eursura.css` and `*.eursura-logo.png` (manifest suffix matching), consumed by Task 3's `WriteBrandAssetsAsync`.

- [ ] **Step 1: Download the logo from the issue attachment**

```bash
curl.exe -sL -o src/EAxWiki.Export/Resources/eursura-logo.png "https://github.com/user-attachments/assets/be8af4cb-c385-4319-b72f-378d0bf39f79"
```

Verify it is a real PNG (expected 30674 bytes, `--binary`/PNG magic `\x89PNG`):

```bash
Get-Item src/EAxWiki.Export/Resources/eursura-logo.png | Select-Object Length
Format-Hex -Path src/EAxWiki.Export/Resources/eursura-logo.png -Count 4
```
Expected: Length 30674, first bytes `89 50 4E 47`.

- [ ] **Step 2: Create `brand-eursura.css`**

```css
@import url('https://fonts.googleapis.com/css2?family=Geist:wght@400;500;600;700&family=Geist+Mono&display=swap');

:root {
  --md-primary-fg-color: #103135;
  --md-primary-fg-color--light: #A8C6C7;
  --md-accent-fg-color: #D0F391;
  --md-typeset-a-color: #103135;
  --md-text-font: 'Geist', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  --md-code-font: 'Geist Mono', SFMono-Regular, Consolas, monospace;
}

.md-header {
  background-color: #103135;
}
.md-header__title {
  color: #F3F7F7;
}
.md-tabs {
  background-color: #103135;
}

.status-proposed { background: #F3F7F7; color: #103135; }
.status-approved { background: #D0F391; color: #103135; }
.status-implemented { background: #C4E5E7; color: #103135; }
.status-mandatory { background: #A8C6C7; color: #103135; }
.status-invalid { background: #E2E3E5; color: #103135; }
.status-not-set { background: #E2E3E5; color: #6C757D; font-style: italic; }

.sl[data-layer="business"] { background: #A8C6C7; color: #103135; }
.sl[data-layer="application"] { background: #103135; color: #F3F7F7; }
.sl[data-layer="technology"] { background: #C4E5E7; color: #103135; }
.sl[data-layer="physical"] { background: #6FB4B6; color: #103135; }
.sl[data-layer="motivation"] { background: #D0F391; color: #103135; }
.sl[data-layer="strategy"] { background: #7FA8A9; color: #103135; }
.sl[data-layer="implementation"] { background: #5C8A8B; color: #F3F7F7; }
.sl[data-layer="composite"] { background: #405B5C; color: #F3F7F7; }
.sl[data-layer="uml"] { background: #F3F7F7; color: #103135; }

.diagram-thumb:hover img {
  border-color: #103135;
}
```

- [ ] **Step 3: Register both as embedded resources**

Edit `src/EAxWiki.Export/EAxWiki.Export.csproj` (add under the existing `EmbeddedResource` group, lines 19-22):

```xml
    <EmbeddedResource Include="Resources\brand-eursura.css" />
    <EmbeddedResource Include="Resources\eursura-logo.png" />
```

- [ ] **Step 4: Verify the assembly embeds both**

```bash
dotnet build src/EAxWiki.Export/EAxWiki.Export.csproj --no-restore -v q 2>&1 | Select-Object -Last 3
```
Expected: build succeeds. (A failing-integrity proof comes in Task 4.)

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Resources/brand-eursura.css src/EAxWiki.Export/Resources/eursura-logo.png src/EAxWiki.Export/EAxWiki.Export.csproj
git add --renormalize src/EAxWiki.Export/Resources/brand-eursura.css src/EAxWiki.Export/Resources/eursura-logo.png src/EAxWiki.Export/EAxWiki.Export.csproj
git commit -m "feat(export): add eursura brand css + logo resources (issue #79)"
```

---

### Task 3: Emit brand assets + parameterize graph colors in the exporter

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs:67-106` (`WriteGraphScriptsAsync`), `:1057-1066` (`WriteExtraCssAsync` area), `:1068-1091` (`CleanupOrphanedFilesAsync`)
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:47-57` (env read) and `:98-99` (calls)

**Interfaces:**
- Consumes: `EAXWIKI_BRAND` env var (Task 1); embedded resources `*.brand-eursura.css`, `*.eursura-logo.png` (Task 2).
- Produces: when brand == `eursura` → `wiki/brand.css`, `wiki/assets/eursura-logo.png`, `graph-init.js` containing brand `EA_LAYER_COLORS`/`EA_LAYER_DARK_TEXT`. Neutral → no brand files, unchanged JS. `WriteGraphScriptsAsync` signature becomes `WriteGraphScriptsAsync(string outputDir, string brand, CancellationToken ct = default)`.

- [ ] **Step 1: Read brand in `MarkdownExporter`**

In `ExportAsync`, after the existing `Environment.GetEnvironmentVariable("EAXWIKI_AI_ENDPOINT")` read (line 49), add:

```csharp
            var brand = Environment.GetEnvironmentVariable("EAXWIKI_BRAND") ?? string.Empty;
```

At the view-tasks list (line 98-99), change:

```csharp
                infrastructure.WriteExtraCssAsync(outputPath, cancellationToken),
                infrastructure.WriteGraphScriptsAsync(outputPath, cancellationToken),
```

to:

```csharp
                infrastructure.WriteExtraCssAsync(outputPath, cancellationToken),
                infrastructure.WriteBrandAssetsAsync(outputPath, brand, cancellationToken),
                infrastructure.WriteGraphScriptsAsync(outputPath, brand, cancellationToken),
```

- [ ] **Step 2: Parameterize `WriteGraphScriptsAsync` in `InfrastructureWriter`**

Change the signature at line 67 to:

```csharp
    public async Task WriteGraphScriptsAsync(string outputDir, string brand, CancellationToken ct = default)
```

Inside the method, replace the literal `EA_LAYER_COLORS = { ... };` block (lines 79-95) — including the `EA_LAYER_DARK_TEXT` line (line 96) — with placeholders:

```csharp
    var EA_LAYER_COLORS = /*EA_LAYER_COLORS*/;
    var EA_LAYER_DARK_TEXT = /*EA_LAYER_DARK_TEXT*/;
```

Before the `await writer.WriteFileAsync(...)` call (line 423), insert the color-map builder:

```csharp
        var (layerColors, darkText) = brand == "eursura"
            ? (new Dictionary<string, string>
               {
                   ["business"] = "#A8C6C7",
                   ["application"] = "#103135",
                   ["technology"] = "#C4E5E7",
                   ["physical"] = "#6FB4B6",
                   ["motivation"] = "#D0F391",
                   ["strategy"] = "#7FA8A9",
                   ["implementation"] = "#5C8A8B",
                   ["composite"] = "#405B5C",
                   ["uml"] = "#F3F7F7",
               },
               new Dictionary<string, bool> { ["business"] = true, ["technology"] = true, ["physical"] = true, ["motivation"] = true, ["strategy"] = true, ["uml"] = true })
            : (new Dictionary<string, string>
               {
                   ["business"] = "#D4A017",
                   ["application"] = "#2E86C1",
                   ["technology"] = "#27AE60",
                   ["physical"] = "#17A589",
                   ["motivation"] = "#8E44AD",
                   ["strategy"] = "#A0682B",
                   ["implementation"] = "#D84B79",
                   ["composite"] = "#5D6D7E",
                   ["uml"] = "#7F8C8D",
               },
               new Dictionary<string, bool> { ["business"] = true });

        string SerializeColors(Dictionary<string, string> map) =>
            string.Join(",\n", map.Select(kv => $"    '{kv.Key}':       '{kv.Value}'"));

        string SerializeDarkText(Dictionary<string, bool> map) =>
            string.Join(", ", map.Select(kv => $"'{kv.Key}': {kv.Value.ToString().ToLowerInvariant()}"));

        var graphInitJs = graphInitJs.Replace("/*EA_LAYER_COLORS*/", "{\n" + SerializeColors(layerColors) + "\n}")
                                     .Replace("/*EA_LAYER_DARK_TEXT*/", "{ " + SerializeDarkText(darkText) + " }");
```

Rename the existing literal `const string graphInitJs = """` to `var graphInitJs = """` (it must be mutable for the `.Replace` calls). Keep `EA_DISTANCE_COLORS` and the EDGY entries inside the literal untouched. Write `graphInitJs` (the replaced variable) instead of the literal.

- [ ] **Step 3: Add `WriteBrandAssetsAsync`**

Add a new method next to `WriteExtraCssAsync` (after line 1066):

```csharp
    public async Task WriteBrandAssetsAsync(string outputDir, string brand, CancellationToken ct = default)
    {
        if (brand != "eursura") return;

        var assembly = Assembly.GetExecutingAssembly();

        var cssResource = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("brand-eursura.css", StringComparison.OrdinalIgnoreCase));
        using var cssStream = assembly.GetManifestResourceStream(cssResource)!;
        using var cssReader = new StreamReader(cssStream);
        var css = await cssReader.ReadToEndAsync(ct);
        await writer.WriteFileAsync(Path.Combine(outputDir, "brand.css"), css, ct);

        var pngResource = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("eursura-logo.png", StringComparison.OrdinalIgnoreCase));
        using var pngStream = assembly.GetManifestResourceStream(pngResource)!;
        var pngBytes = new byte[pngStream.Length];
        await pngStream.ReadAsync(pngBytes, ct);
        Directory.CreateDirectory(Path.Combine(outputDir, "assets"));
        await File.WriteAllBytesAsync(Path.Combine(outputDir, "assets", "eursura-logo.png"), pngBytes, ct);
    }
```

- [ ] **Step 4: Protect brand assets from orphan cleanup**

In `CleanupOrphanedFilesAsync` (line 1079-1086), add `assets` to `specialDirs`:

```csharp
        var specialDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine(ctx.OutputPath, "diagrams"),
            Path.Combine(ctx.OutputPath, "types"),
            Path.Combine(ctx.OutputPath, "glossary"),
            Path.Combine(ctx.OutputPath, "recent"),
            Path.Combine(ctx.OutputPath, "status"),
            Path.Combine(ctx.OutputPath, "assets"),
        };
```

(`brand.css` at the output root is never deleted by cleanup — root `.md` deletion is gated on `!isRoot`, and root files aren't enumerated for deletion.)

- [ ] **Step 5: Build**

Run: `dotnet build src/EAxWiki.slnx --no-restore -v q 2>&1 | Select-Object -Last 5`
Expected: build succeeds (6 pre-existing warnings acceptable).

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Export/MarkdownExporter.cs
git add --renormalize src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat(export): emit eursura brand css, logo, graph colors (issue #79)"
```

---

### Task 4: .NET tests for brand emission

**Files:**
- Modify: `src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs`

**Interfaces:**
- Consumes: `MarkdownExporter.ExportAsync` behavior from Task 3; `TestInMemoryWriter.Files`; `EAXWIKI_BRAND` env var.
- Produces: proof that neutral export is unchanged and branded export emits brand files + brand graph colors. The brand test uses the real temp dir to check the on-disk logo (written via `File.WriteAllBytesAsync`, bypassing the string-only `IOutputWriter`), consistent with diagram PNG precedent.

- [ ] **Step 1: Write the failing tests**

Append to `ScriptTemplateIntegrityTests.cs`:

```csharp
    [Fact]
    public async Task BrandEursura_EmitsBrandCssLogoAndBrandColors()
    {
        var writer = new TestInMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var outPath = Path.Combine(Path.GetTempPath(), "eaxwiki_brand_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(outPath);
            Environment.SetEnvironmentVariable("EAXWIKI_BRAND", "eursura");
            try
            {
                var result = await exporter.ExportAsync(MinimalRepository(), null, outPath);
                Assert.Equal(1, result.SucceededElements);
            }
            finally
            {
                Environment.SetEnvironmentVariable("EAXWIKI_BRAND", null);
            }

            var brandKey = Normalize(Path.Combine(outPath, "brand.css"));
            Assert.True(writer.Files.ContainsKey(brandKey), $"brand.css should be emitted for eursura. Keys: {string.Join(", ", writer.Files.Keys)}");

            var graph = ReadExportedFile(writer, outPath, "graph-init.js");
            Assert.Contains("#A8C6C7", graph);
            Assert.Contains("'technology':       '#C4E5E7'", graph);
            Assert.Contains("'uml':       '#F3F7F7'", graph);

            Assert.True(File.Exists(Path.Combine(outPath, "assets", "eursura-logo.png")), "logo should be written to disk");
        }
        finally
        {
            if (Directory.Exists(outPath))
                Directory.Delete(outPath, recursive: true);
        }
    }

    [Fact]
    public async Task BrandNeutral_DoesNotEmitBrandFiles()
    {
        var (writer, outPath) = await RunExportAsync();
        var brandKey = Normalize(Path.Combine(outPath, "brand.css"));
        Assert.False(writer.Files.ContainsKey(brandKey), $"brand.css should not exist without --brand. Keys: {string.Join(", ", writer.Files.Keys)}");
        Assert.False(Directory.Exists(Path.Combine(outPath, "assets")));

        var graph = ReadExportedFile(writer, outPath, "graph-init.js");
        Assert.Contains("#D4A017", graph);
        Assert.DoesNotContain("#A8C6C7", graph);
    }
```

Note: place `BrandNeutral_DoesNotEmitBrandFiles` in the same class (runs sequentially after any env-setting tests in this class; no other class sets `EAXWIKI_BRAND`).

- [ ] **Step 2: Run tests to verify they fail**

Run: `dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~ScriptTemplateIntegrityTests" --no-restore`
Expected: `BrandEursura_EmitsBrandCssLogoAndBrandColors` FAILS (brand.css absent), `BrandNeutral_DoesNotEmitBrandFiles` FAILS (brand colors present). Other integrity tests still pass.

- [ ] **Step 3: Run the full suite to verify they pass**

Run: `dotnet test src/EAxWiki.Tests --no-restore`
Expected: all pass (270 + 2 new = 272).

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs
git add --renormalize src/EAxWiki.Tests/ScriptTemplateIntegrityTests.cs
git commit -m "test(export): cover eursura brand emission (issue #79)"
```

---

### Task 5: Monitor + export scripts pass `--brand` through (with Pester tests)

**Files:**
- Modify: `scripts/export.ps1:9-42` (arg parse), `:83-94` (runArgs)
- Modify: `scripts/monitor-export-and-serve.ps1:61-119` (Get-MonitorArgs), `:251-265` (.eaxwiki fallback), `:826-831` (exportArgs)
- Test: `tests/scripts/export.Tests.ps1`, `tests/scripts/monitor-export-and-serve.Tests.ps1`

**Interfaces:**
- Consumes: `--brand` CLI flag / `.eaxwiki` `brand` field.
- Produces: `export.ps1` passes `--brand <name>` to `dotnet run`; monitor passes it to `export.ps1`. `Get-ExportArgs` returns `.Brand` (string, default `""`); `Get-MonitorArgs` returns `.Brand` (string, default `$null`).

- [ ] **Step 1: Write the failing Pester tests**

Append to `tests/scripts/export.Tests.ps1` inside the `Describe 'Get-ExportArgs'` block:

```powershell
    It 'parses --brand with value' {
        $r = Get-ExportArgs -Arguments @('--brand', 'eursura')
        $r.Brand | Should -Be 'eursura'
    }

    It 'parses -Brand with value' {
        $r = Get-ExportArgs -Arguments @('-Brand', 'eursura')
        $r.Brand | Should -Be 'eursura'
    }
```

Add to the `returns defaults` test assertion block in `export.Tests.ps1`:

```powershell
        $r.Brand | Should -Be ""
```

Append to `tests/scripts/monitor-export-and-serve.Tests.ps1` inside `Describe 'Get-MonitorArgs'`:

```powershell
    It 'parses --brand' {
        $r = Get-MonitorArgs -Arguments @('--brand', 'eursura')
        $r.Brand | Should -Be 'eursura'
    }

    It 'parses -Brand' {
        $r = Get-MonitorArgs -Arguments @('-Brand', 'eursura')
        $r.Brand | Should -Be 'eursura'
    }
```

Add to the monitor `returns defaults` assertion block:

```powershell
        $r.Brand | Should -Be $null
```

- [ ] **Step 2: Run tests to verify they fail**

Run: `Invoke-Pester tests/scripts/export.Tests.ps1 -Output Detailed`
Expected: the 2 new `--brand` tests FAIL (no Brand member).

- [ ] **Step 3: Add `--brand` to `export.ps1`**

In `Get-ExportArgs` (lines 11-17) add:

```powershell
    $Brand     = ""
```

In the switch (after the `--api-port` case, line 28) add:

```powershell
            '^(--brand|-Brand)$'                 { $i++; if ($i -lt $Arguments.Count) { $Brand    = $Arguments[$i] } }
```

In the returned object (lines 33-41) add:

```powershell
        Brand     = $Brand
```

After the parse (line 51) add:

```powershell
$Brand     = $parsed.Brand
```

In the `$runArgs` builder (after line 94) add:

```powershell
if ($Brand)          { $runArgs += "--brand", $Brand }
```

- [ ] **Step 4: Add `--brand` to `monitor-export-and-serve.ps1`**

In `Get-MonitorArgs` (after line 72) add:

```powershell
    $Brand                = $null
```

In the switch (after the `--telegram-chat-id` case, line 92) add:

```powershell
            '^(--brand|-Brand)$'                     { $i++; if ($i -lt $Arguments.Count) { $Brand             = $Arguments[$i] } }
```

In the returned object (after `TelegramChatId`, line 113) add:

```powershell
        Brand               = $Brand
```

After the Telegram `.eaxwiki` fallback block (after line 265), add:

```powershell
if ($null -eq $Brand -or "" -eq $Brand) {
    if ($env:EAXWIKI_BRAND) {
        $Brand = $env:EAXWIKI_BRAND
    } elseif ($eaxwikiConfig -and $eaxwikiConfig.brand) {
        $Brand = $eaxwikiConfig.brand
    }
}
```

In the `$exportArgs` builder (after line 831, the `--api-port` addition) add:

```powershell
            if ($Brand) { $exportArgs += "--brand", $Brand }
```

- [ ] **Step 5: Run Pester tests to verify they pass**

Run:
```powershell
Invoke-Pester tests/scripts/export.Tests.ps1 -Output Detailed
Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed
```
Expected: both PASS. (If a background monitor is running, stop it first — the monitor file dot-sources and the duplicate-monitor guard can make Pester exit early. PID file: `.eaxwiki-monitor\d88915cd6c0f\monitor.pid`.)

- [ ] **Step 6: Commit**

```bash
git add scripts/export.ps1 scripts/monitor-export-and-serve.ps1 tests/scripts/export.Tests.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1
git add --renormalize scripts/export.ps1 scripts/monitor-export-and-serve.ps1 tests/scripts/export.Tests.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1
git commit -m "feat(scripts): pass --brand through export + monitor (issue #79)"
```

---

### Task 6: Static `mkdocs.yml` references (safe for all users)

**Files:**
- Modify: `mkdocs.yml`

**Interfaces:**
- Consumes: Task 3 output paths `wiki/brand.css` and `wiki/assets/eursura-logo.png`.
- Produces: brand-active for the demo; harmless missing-file references for non-branded exports (verified: MkDocs build exit 0 with missing `extra_css` and missing `theme.logo`).

- [ ] **Step 1: Edit `mkdocs.yml`**

Change the `theme:` block (lines 4-9):

```yaml
theme:
  name: material
  logo: assets/eursura-logo.png
  features:
    - navigation.instant
    - navigation.tracking
    - navigation.indexes
```

Change `extra_css` (lines 18-20):

```yaml
extra_css:
  - extra.css
  - brand.css
```

- [ ] **Step 2: Verify the static config still builds against the current (neutral) wiki**

```bash
& ".venv\Scripts\python.exe" -m mkdocs build --strict 2>&1 | Select-Object -Last 3
```
Expected: builds with exit 0 (brand.css and logo are absent in the current neutral wiki — MkDocs tolerates this).

- [ ] **Step 3: Commit**

```bash
git add mkdocs.yml
git add --renormalize mkdocs.yml
git commit -m "chore(theme): reference eursura brand.css + logo (issue #79)"
```

---

### Task 7: Rebrand the demo wiki (this repo)

**Files:**
- Modify: `.eaxwiki` (add `brand` field — DO NOT commit)
- Modify: `wiki/*` (regenerated — commit)
- Modify: `docs/design-decisions.md`

**Interfaces:**
- Consumes: `--brand eursura` support from Tasks 1/3/5; `mkdocs.yml` refs from Task 6.
- Produces: `.eaxwiki` with `brand=eursura`; committed `wiki/` containing `brand.css`, `assets/eursura-logo.png`, branded `graph-init.js`.

- [ ] **Step 1: Add `brand` to `.eaxwiki` (encrypted, never committed)**

PowerShell 5.1 note: `.eaxwiki` is a single DPAPI-encrypted base64 blob; edit by decrypt → add `brand` → re-encrypt → write. Run from repo root:

```powershell
Add-Type -AssemblyName System.Security
$entropy = [System.Text.Encoding]::UTF8.GetBytes("EAxWiki.LocalConfig.v1")
$raw = (Get-Content .eaxwiki -Raw).Trim()
$encrypted = [Convert]::FromBase64String($raw)
$decrypted = [System.Security.Cryptography.ProtectedData]::Unprotect($encrypted, $entropy, [System.Security.Cryptography.DataProtectionScope]::CurrentUser)
$json = [System.Text.Encoding]::UTF8.GetString($decrypted)
$cfg = $json | ConvertFrom-Json
$cfg | Add-Member -NotePropertyName brand -NotePropertyValue "eursura" -Force
$newJson = $cfg | ConvertTo-Json -Compress
$plainBytes = [System.Text.Encoding]::UTF8.GetBytes($newJson)
$newEncrypted = [System.Security.Cryptography.ProtectedData]::Protect($plainBytes, $entropy, [System.Security.Cryptography.DataProtectionScope]::CurrentUser)
[System.IO.File]::WriteAllText((Resolve-Path .eaxwiki), [Convert]::ToBase64String($newEncrypted))
```

Verify the monitor resolves it (config line will now show the brand if the script logs it — at minimum confirm no parse error):

```powershell
& .\scripts\monitor-export-and-serve.ps1 --test-alert 2>&1 | Select-Object -Last 6
```
Expected: script parses `.eaxwiki` without error. (It may hit the duplicate-monitor guard or send the alert — that's fine, the goal is config resolution, not the alert.)

- [ ] **Step 2: Regenerate the wiki branded**

Ensure no monitor/serve is holding locks (kill any monitor pwsh and `EAxWiki.dll`/`dotnet run` processes; `Get-CimInstance Win32_Process -Filter "Name='pwsh.exe'"` filtered on `monitor-export-and-serve`). Then run a full export:

```bash
.\scripts\export.ps1 --repo "model/EurSuRA.qea" --force
```

Expected: export succeeds; `wiki/brand.css`, `wiki/assets/eursura-logo.png`, and `wiki/graph-init.js` (with `#A8C6C7`/`#C4E5E7`) now exist.

Verify:

```bash
Test-Path wiki/brand.css; Test-Path wiki/assets/eursura-logo.png; Select-String -Path wiki/graph-init.js -Pattern '#A8C6C7'
```

- [ ] **Step 3: Serve and eyeball**

```bash
.\scripts\serve.ps1 --port 8000
```
Expected: header shows the EurSuRA logo + Jet Black bar, Geist body font, Lime Cream accents; graph nodes and status badges use EurSuRA tones. Stop the server when done.

- [ ] **Step 4: Document the brand in `design-decisions.md`**

Append a dated section documenting: `--brand eursura` (CLI/env/`.eaxwiki`), that default stays neutral/byte-identical, brand assets are embedded resources written to `wiki/`, orphan-cleanup whitelists `assets/`, and the layer-color mapping table (from the spec).

- [ ] **Step 5: Commit (wiki + docs only — NOT `.eaxwiki`)**

```bash
git add wiki/ docs/design-decisions.md
git add --renormalize wiki/ docs/design-decisions.md
git commit -m "feat(wiki): regenerate demo with eursura branding (issue #79)"
```
Never stage `.eaxwiki`.

- [ ] **Step 6: Run the full test suites for a final gate**

```bash
dotnet test src/EAxWiki.Tests --no-restore
Invoke-Pester tests/scripts/export.Tests.ps1 -Output Detailed
Invoke-Pester tests/scripts/monitor-export-and-serve.Tests.ps1 -Output Detailed
```
Expected: all pass (272 .NET tests; export + monitor Pester suites pass).

---

### Task 8: Docs + close issue

**Files:**
- Modify: `README.md` (test counts, branding mention)
- GitHub: comment + close issue #79

**Interfaces:**
- Consumes: everything above.
- Produces: updated README test counts and a short branding note; issue #79 closed with a summary comment.

- [ ] **Step 1: Update README test counts**

Update the "Tests" section counts: .NET 272 (was 270), Pester 143 (unchanged), total 415 (was 413). Add a short "Branding" line under Features: "Optional `--brand eursura` emits EurSuRA logo/palette/fonts; default stays neutral."

- [ ] **Step 2: Commit**

```bash
git add README.md
git add --renormalize README.md
git commit -m "docs(readme): note --brand support and test counts (issue #79)"
```

- [ ] **Step 3: Post close comment and close the issue**

Write the close comment to a temp file and post it:

```bash
Write-Output "Implemented: configurable --brand (eursura) with logo, palette, Geist fonts, branded graph/widget colors. Neutral default unchanged; demo wiki regenerated branded. Commits: <range>." | Set-Content "C:\Users\hanva\AppData\Local\Temp\opencode\issue79-close.md"
gh issue comment 79 --body-file "C:\Users\hanva\AppData\Local\Temp\opencode\issue79-close.md"
gh issue close 79 --reason completed
```

- [ ] **Step 4: Push**

```bash
git push origin master
```
Expected: pushes all commits; the `mkdocs-deploy` workflow rebuilds the live demo site (branded wiki) on GitHub Pages.

---

## Self-Review Notes

- **Spec coverage:** Config flag + `.eaxwiki` field (Task 1), embedded resources (Task 2), brand CSS/logo emission + graph colors + orphan whitelist (Task 3), neutrality + brand + unknown-brand tests (Task 4 — unknown brand is implicitly neutral since only exact `"eursura"` triggers; add an assertion in Task 4 step 1 if desired), script pass-through (Task 5), static mkdocs.yml (Task 6), demo rebrand (Task 7), docs/close (Task 8).
- **Type consistency:** `Config.Brand` (string), `LocalConfigStore.Config.Brand` (string?), `Get-ExportArgs.Brand` (string `""`), `Get-MonitorArgs.Brand` (string `$null`), `WriteGraphScriptsAsync(string, string, CancellationToken)`, `WriteBrandAssetsAsync(string, string, CancellationToken)` — all consistent.
- **Deviation from spec:** `EA_LAYER_DARK_TEXT` is parameterized too (the spec's color table assigns per-layer text colors — the spec's "unchanged" line was contradicted by its own table). This is required for contrast on the light EurSuRA layer colors.
