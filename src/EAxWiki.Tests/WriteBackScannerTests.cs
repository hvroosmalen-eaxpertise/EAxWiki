using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export;
using EAxWiki.Export.Exporters;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

/// <summary>
/// Fake IEaReader recording every write-back call instead of touching a real EA model.
/// </summary>
internal class FakeEaReader : IEaReader
{
    public List<(int Id, string Status)> StatusUpdates { get; } = [];
    public List<(int Id, string Notes)> ElementNotesUpdates { get; } = [];
    public List<(int Id, string Notes)> DiagramNotesUpdates { get; } = [];
    public List<(int ElementId, string Name, string Type, string Notes)> AttributeNotesUpdates { get; } = [];
    public List<(int ElementId, string Name, string ReturnType, bool IsStatic, string Notes)> MethodNotesUpdates { get; } = [];
    public List<(int ElementId, string Name, string Value, string Notes)> TaggedValueNotesUpdates { get; } = [];

    public EaRepository Open(string connectionString) => new();
    public void Close() { }
    public bool ExportDiagramImage(string diagramGuid, string filePath) => true;
    public string RepositoryPath => string.Empty;
    public IReadOnlyList<string> GetStatusTypes() => ["Approved", "Implemented", "Mandatory", "Proposed", "Validated"];
    public string GetElementStatus(int elementId) => "Proposed";

    public void UpdateElementStatus(int elementId, string newStatus) => StatusUpdates.Add((elementId, newStatus));
    public void UpdateElementNotes(int elementId, string newNotesHtml) => ElementNotesUpdates.Add((elementId, newNotesHtml));
    public void UpdateDiagramNotes(int diagramId, string newNotesHtml) => DiagramNotesUpdates.Add((diagramId, newNotesHtml));
    public void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml) =>
        AttributeNotesUpdates.Add((elementId, attributeName, attributeType, newNotesHtml));
    public void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml) =>
        MethodNotesUpdates.Add((elementId, methodName, returnType, isStatic, newNotesHtml));
    public void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml) =>
        TaggedValueNotesUpdates.Add((elementId, tagName, tagValue, newNotesHtml));
}

/// <summary>
/// Verifies WriteBackScanner routes element pages (ea_id) to UpdateElementNotes and diagram
/// pages (diagram_id) to UpdateDiagramNotes, based purely on which frontmatter key is present.
/// </summary>
public class WriteBackScannerTests : IDisposable
{
    private readonly string _wikiDir;

    public WriteBackScannerTests()
    {
        _wikiDir = Path.Combine(Path.GetTempPath(), "eaxwiki_writeback_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_wikiDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_wikiDir)) Directory.Delete(_wikiDir, recursive: true);
    }

    private const string ElementPage = """
        ---
        ea_id: 133
        status: Mandatory
        status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
        ea_hash: 1a666d0d
        notes_hash: deadbeef
        ---

        # Emission Sources

        <div class="ea-notes-content">
        <!--ea-notes-start-->
        <p>Edited directly in the .md file.</p>
        <!--ea-notes-end-->
        </div>
        """;

    private const string DiagramPage = """
        ---
        diagram_id: 55
        notes_hash: deadbeef
        ---

        # Architecture Overview

        <div class="ea-notes-content">
        <!--ea-notes-start-->
        <p>Edited diagram description directly in the .md file.</p>
        <!--ea-notes-end-->
        </div>
        """;

    [Fact]
    public void Scan_RoutesElementNotesToUpdateElementNotes()
    {
        File.WriteAllText(Path.Combine(_wikiDir, "Element.md"), ElementPage);
        var fakeReader = new FakeEaReader();
        var scanner = new WriteBackScanner(fakeReader, NullLogger<WriteBackScanner>.Instance);

        var result = scanner.Scan(_wikiDir);

        Assert.Single(fakeReader.ElementNotesUpdates);
        Assert.Equal(133, fakeReader.ElementNotesUpdates[0].Id);
        Assert.Contains("Edited directly", fakeReader.ElementNotesUpdates[0].Notes);
        Assert.Empty(fakeReader.DiagramNotesUpdates);
        Assert.Single(result.NotesChanges);
    }

    [Fact]
    public void Scan_RoutesDiagramNotesToUpdateDiagramNotes()
    {
        File.WriteAllText(Path.Combine(_wikiDir, "Diagram.md"), DiagramPage);
        var fakeReader = new FakeEaReader();
        var scanner = new WriteBackScanner(fakeReader, NullLogger<WriteBackScanner>.Instance);

        var result = scanner.Scan(_wikiDir);

        Assert.Single(fakeReader.DiagramNotesUpdates);
        Assert.Equal(55, fakeReader.DiagramNotesUpdates[0].Id);
        Assert.Contains("Edited diagram description", fakeReader.DiagramNotesUpdates[0].Notes);
        Assert.Empty(fakeReader.ElementNotesUpdates);
        Assert.Single(result.NotesChanges);
    }

    [Fact]
    public void Scan_SkipsFileWhenNotesHashMatches()
    {
        var normalized = "<p>Edited diagram description directly in the .md file.</p>";
        var hash = ElementPageWriter.ComputeNotesHash(normalized);
        var page = DiagramPage.Replace("notes_hash: deadbeef", $"notes_hash: {hash}");
        File.WriteAllText(Path.Combine(_wikiDir, "Diagram.md"), page);
        var fakeReader = new FakeEaReader();
        var scanner = new WriteBackScanner(fakeReader, NullLogger<WriteBackScanner>.Instance);

        var result = scanner.Scan(_wikiDir);

        Assert.Empty(fakeReader.DiagramNotesUpdates);
        Assert.Empty(result.NotesChanges);
    }

    private const string ElementPageWithRowWidgets = """
        ---
        ea_id: 42
        ---

        # Widget Container

        <table>
        <tbody>
        <tr><td>maxRetries</td><td>int</td><td>3</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0-->Edited attribute description directly in the .md file.<!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="deadbeef" data-kind="attribute" data-el-id="42" data-attr-name="maxRetries" data-attr-type="int" data-file-path="Widget Container.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
        <tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
        </tbody>
        </table>

        <div class="ea-row-notes-widget" data-row-id="method-0"><span class="ea-row-notes-text"><!--ea-row-notes-start:method-0-->UNCHANGED_HASH_MATCHES<!--ea-row-notes-end:method-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="inline" data-row-id="method-0" data-notes-hash="__METHOD_HASH__" data-kind="method" data-el-id="42" data-method-name="Retry" data-return-type="void" data-is-static="true" data-file-path="Widget Container.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></div>
        """;

    [Fact]
    public void Scan_RoutesAttributeRowNotesToUpdateAttributeNotes()
    {
        var methodHash = ElementPageWriter.ComputeNotesHash("UNCHANGED_HASH_MATCHES");
        var page = ElementPageWithRowWidgets.Replace("__METHOD_HASH__", methodHash);
        File.WriteAllText(Path.Combine(_wikiDir, "Widget Container.md"), page);
        var fakeReader = new FakeEaReader();
        var scanner = new WriteBackScanner(fakeReader, NullLogger<WriteBackScanner>.Instance);

        var result = scanner.Scan(_wikiDir);

        Assert.Single(fakeReader.AttributeNotesUpdates);
        Assert.Equal((42, "maxRetries", "int"), (fakeReader.AttributeNotesUpdates[0].ElementId, fakeReader.AttributeNotesUpdates[0].Name, fakeReader.AttributeNotesUpdates[0].Type));
        Assert.Contains("Edited attribute description", fakeReader.AttributeNotesUpdates[0].Notes);

        // Method row's hash already matches its content -> untouched
        Assert.Empty(fakeReader.MethodNotesUpdates);

        Assert.Single(result.NotesChanges);
    }
}
