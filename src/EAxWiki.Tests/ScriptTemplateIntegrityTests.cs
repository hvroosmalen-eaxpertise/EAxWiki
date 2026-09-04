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

    private static string Normalize(string path) => path.Replace('\\', '/');

    private static async Task<(TestInMemoryWriter Writer, string OutPath)> RunExportAsync()
    {
        var writer = new TestInMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var outPath = Path.Combine(Path.GetTempPath(), "eaxwiki_integrity_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(outPath);
            var result = await exporter.ExportAsync(MinimalRepository(), null, outPath);
            Assert.Equal(1, result.SucceededElements);
        }
        finally
        {
            if (Directory.Exists(outPath))
                Directory.Delete(outPath, recursive: true);
        }
        return (writer, outPath);
    }

    private static string ReadExportedFile(TestInMemoryWriter writer, string outPath, string fileName)
    {
        var key = Normalize(Path.Combine(outPath, fileName));
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
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "notes-editor.js");
        AssertContainsAll(content, "initNotesEditor", "suggestBtn", "ea-notes-suggest-btn", "/api/ai-suggest", "acquireEditLock", "EAxIcons.set(saveBtn", "EAxIcons.set(suggestBtn, 'spinner'");
        Assert.DoesNotContain("saveBtn.textContent", content);
        Assert.DoesNotContain("suggestBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }

    [Fact]
    public async Task StatusEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "status-editor.js");
        AssertContainsAll(content, "initStatusEditor", "/api/status", "EAxIcons.set(applyBtn", "ea-status-btn", "ea-status-cancel-btn");
        Assert.DoesNotContain("applyBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }

    [Fact]
    public async Task RowNotesEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "row-notes-editor.js");
        AssertContainsAll(content, "initRowNotesEditors", "openEditor", "/api/row-notes", "EAxIcons.set(saveBtn", "ea-notes-save-btn", "ea-notes-cancel-btn");
        Assert.DoesNotContain("saveBtn.textContent", content);
        Assert.DoesNotContain("cancelBtn.textContent", content);
    }

    [Fact]
    public async Task GraphInitScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "graph-init.js");
        AssertContainsAll(content, "initEaGraph", "cytoscape", "wikiBase", "script[src$=\"cytoscape.min.js\"]");
    }

    [Fact]
    public async Task ExtraCss_ContainsCoreStyles()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "extra.css");
        AssertContainsAll(content, ".ea-notes-editor", ".ea-notes-suggest-btn", ".ea-status-editor", "ea-icon-spinner", "@keyframes ea-spin", "fill: currentColor");
    }

    [Fact]
    public async Task CytoscapeMinJs_IsEmitted()
    {
        var (writer, outPath) = await RunExportAsync();
        ReadExportedFile(writer, outPath, "cytoscape.min.js");
    }

    [Fact]
    public async Task EaIconsScript_IsEmitted()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "ea-icons.js");
        AssertContainsAll(content, "window.EAxIcons", "set: function", "aria-label", "spinner", "ea-icon-spinner", "viewBox=\"0 0 24 24\"", "save: '<svg", "cancel: '<svg", "apply: '<svg");
    }

    [Fact]
    public async Task ApiProbeScript_IsEmitted()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "api-probe.js");
        AssertContainsAll(content, "/readyz", "ea-api-ready", "ea-api-unavailable", "no-ea", "no-api", "ea-api-status");
    }

    [Fact]
    public async Task Editors_GateOnEaApiReady()
    {
        var (writer, outPath) = await RunExportAsync();
        foreach (var file in new[] { "status-editor.js", "notes-editor.js", "row-notes-editor.js" })
        {
            var content = ReadExportedFile(writer, outPath, file);
            Assert.Contains("ea-api-ready", content);
        }
    }

    [Fact]
    public async Task ExtraCss_GatesEditButtonsOnEaApiReady()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "extra.css");
        AssertContainsAll(content, "body.ea-api-ready", ".ea-status-edit-btn", ".ea-notes-edit-btn", ".ea-row-notes-edit-btn");
    }

    [Fact]
    public async Task AiSuggestJs_IsNotEmitted()
    {
        var (writer, outPath) = await RunExportAsync();
        var key = Normalize(Path.Combine(outPath, "ai-suggest.js"));
        Assert.False(writer.Files.ContainsKey(key), $"ai-suggest.js should no longer be produced. Keys: {string.Join(", ", writer.Files.Keys)}");
    }

    // Issue #97: brand.css is seeded on first export (when missing) with a
    // commented-out template, and NEVER overwritten by subsequent exports —
    // user-owned styling that survives every export cycle.
    [Fact]
    public async Task BrandCss_SeededOnFirstExport()
    {
        var (writer, outPath) = await RunExportAsync();
        var brandKey = Normalize(Path.Combine(outPath, "brand.css"));
        Assert.True(writer.Files.ContainsKey(brandKey),
            $"brand.css should be seeded on first export. Keys: {string.Join(", ", writer.Files.Keys)}");
        // Template ships as commented-out CSS variable examples.
        Assert.Contains("Your wiki's brand", writer.Files[brandKey]);
        Assert.Contains("--md-primary-fg-color", writer.Files[brandKey]);
    }

    [Fact]
    public async Task BrandCss_NotOverwrittenIfPresent()
    {
        var writer = new TestInMemoryWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var outPath = Path.Combine(Path.GetTempPath(), "eaxwiki_brandseed_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(outPath);
            var brandPath = Path.Combine(outPath, "brand.css");
            const string userCss = "/* my custom brand */\n:root { --md-primary-fg-color: #FF00FF; }\n";
            File.WriteAllText(brandPath, userCss);

            var result = await exporter.ExportAsync(MinimalRepository(), null, outPath);
            Assert.Equal(1, result.SucceededElements);

            // File on disk is unchanged — the seeder saw it existed and skipped.
            Assert.Equal(userCss, File.ReadAllText(brandPath));
        }
        finally
        {
            if (Directory.Exists(outPath))
                Directory.Delete(outPath, recursive: true);
        }
    }
}
