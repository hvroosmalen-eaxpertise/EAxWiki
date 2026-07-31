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
