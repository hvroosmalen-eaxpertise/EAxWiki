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
        AssertContainsAll(content, "initNotesEditor", "suggestBtn", "ea-notes-suggest-btn", "/api/ai-suggest", "acquireEditLock");
    }

    [Fact]
    public async Task StatusEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "status-editor.js");
        AssertContainsAll(content, "initStatusEditor", "/api/status");
    }

    [Fact]
    public async Task RowNotesEditorScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "row-notes-editor.js");
        AssertContainsAll(content, "initRowNotesEditors", "openEditor", "/api/row-notes");
    }

    [Fact]
    public async Task GraphInitScript_ContainsCoreFunctions()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "graph-init.js");
        AssertContainsAll(content, "initEaGraph", "cytoscape");
    }

    [Fact]
    public async Task ExtraCss_ContainsCoreStyles()
    {
        var (writer, outPath) = await RunExportAsync();
        var content = ReadExportedFile(writer, outPath, "extra.css");
        AssertContainsAll(content, ".ea-notes-editor", ".ea-notes-suggest-btn", ".ea-status-editor");
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
        AssertContainsAll(content, "window.EAxIcons", "set: function", "aria-label", "spinner");
    }

    [Fact]
    public async Task AiSuggestJs_IsNotEmitted()
    {
        var (writer, outPath) = await RunExportAsync();
        var key = Normalize(Path.Combine(outPath, "ai-suggest.js"));
        Assert.False(writer.Files.ContainsKey(key), $"ai-suggest.js should no longer be produced. Keys: {string.Join(", ", writer.Files.Keys)}");
    }
}
