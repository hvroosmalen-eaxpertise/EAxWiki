using EAxWiki.Export.Exporters;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Tests;

/// <summary>
/// Verifies that live/batch write-back (UpdateStatus, UpdateNotes) keeps the page's displayed
/// "Modified:" date in sync with EA. Without this, the exporter's incremental skip check
/// (file write-time >= element.ModifiedDate) would permanently skip re-rendering the page,
/// since the write-back patch's own file-write happens right after EA's ModifiedDate bumps.
/// </summary>
public class FrontmatterParserTests : IDisposable
{
    private readonly string _filePath;

    public FrontmatterParserTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), "eaxwiki_fm_" + Guid.NewGuid().ToString("N") + ".md");
    }

    public void Dispose()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }

    private const string SamplePage = """
        ---
        ea_id: 133
        status: Mandatory
        status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
        ea_hash: 1a666d0d
        notes_hash: c81852f4
        ---

        # Asset Emission Sources

        **Type:** Class  **Stereotype:** Asset
        **Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="133" data-status="Mandatory" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Asset/Emission Sources.md" data-api-port="8001"><span class="status-badge status-mandatory">Mandatory</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>
        **Created:** 2025-12-02  **Modified:** 2025-12-02

        <div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="133" data-file-path="Asset/Emission Sources.md" data-api-port="8001">
        <button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
        <div class="ea-notes-content">
        <!--ea-notes-start-->
        <p>Original notes.</p>
        <!--ea-notes-end-->
        </div>
        </div>
        """;

    [Fact]
    public void UpdateStatus_BumpsModifiedDateToToday()
    {
        File.WriteAllText(_filePath, SamplePage);
        var today = DateTime.Now.ToString("yyyy-MM-dd");

        FrontmatterParser.UpdateStatus(_filePath, "Approved");

        var text = File.ReadAllText(_filePath);
        Assert.Contains($"**Modified:** {today}", text);
        Assert.DoesNotContain("**Modified:** 2025-12-02", text);
        Assert.Contains("status: Approved", text);
        Assert.Contains("class=\"status-badge status-approved\">Approved</span>", text);
    }

    [Fact]
    public void UpdateNotes_BumpsModifiedDateToToday()
    {
        File.WriteAllText(_filePath, SamplePage);
        var today = DateTime.Now.ToString("yyyy-MM-dd");

        FrontmatterParser.UpdateNotes(_filePath, "<p>Edited notes.</p>");

        var text = File.ReadAllText(_filePath);
        Assert.Contains($"**Modified:** {today}", text);
        Assert.DoesNotContain("**Modified:** 2025-12-02", text);
        Assert.Contains("<p>Edited notes.</p>", text);
    }

    [Fact]
    public void UpdateStatus_LeavesCreatedDateUntouched()
    {
        File.WriteAllText(_filePath, SamplePage);

        FrontmatterParser.UpdateStatus(_filePath, "Approved");

        var text = File.ReadAllText(_filePath);
        Assert.Contains("**Created:** 2025-12-02", text);
    }

    private const string PageWithRowWidgets = """
        # Widget Container

        <table>
        <tbody>
        <tr><td>maxRetries</td><td>int</td><td>3</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0-->Number of retries.<!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="deadbeef" data-kind="attribute" data-el-id="42" data-attr-name="maxRetries" data-attr-type="int" data-file-path="Widget Container.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
        <tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
        <tr><td>timeoutMs</td><td>int</td><td>5000</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e3b0c442" data-kind="attribute" data-el-id="42" data-attr-name="timeoutMs" data-attr-type="int" data-file-path="Widget Container.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
        <tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
        </tbody>
        </table>

        <div class="ea-row-notes-widget" data-row-id="method-0"><span class="ea-row-notes-text"><!--ea-row-notes-start:method-0-->Runs the retry loop.<!--ea-row-notes-end:method-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="inline" data-row-id="method-0" data-notes-hash="cafef00d" data-kind="method" data-el-id="42" data-method-name="Retry" data-return-type="void" data-is-static="false" data-file-path="Widget Container.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></div>
        """;

    [Fact]
    public void ExtractRowNotesContent_ReturnsOnlyTheMatchingRowsContent()
    {
        File.WriteAllText(_filePath, PageWithRowWidgets);

        Assert.Equal("Number of retries.", FrontmatterParser.ExtractRowNotesContent(_filePath, "attr-0"));
        Assert.Equal("", FrontmatterParser.ExtractRowNotesContent(_filePath, "attr-1"));
        Assert.Equal("Runs the retry loop.", FrontmatterParser.ExtractRowNotesContent(_filePath, "method-0"));
        Assert.Null(FrontmatterParser.ExtractRowNotesContent(_filePath, "attr-99"));
    }

    [Fact]
    public void UpdateRowNotes_PatchesOnlyTheTargetRowsContentAndHash()
    {
        File.WriteAllText(_filePath, PageWithRowWidgets);
        var expectedNewHash = ElementPageWriter.ComputeNotesHash("Timeout in milliseconds before giving up.");

        FrontmatterParser.UpdateRowNotes(_filePath, "attr-1", "Timeout in milliseconds before giving up.");

        var text = File.ReadAllText(_filePath);
        Assert.Contains("Timeout in milliseconds before giving up.", text);
        Assert.Contains($"data-row-id=\"attr-1\" data-notes-hash=\"{expectedNewHash}\"", text);
        Assert.DoesNotContain("data-row-id=\"attr-1\" data-notes-hash=\"e3b0c442\"", text);

        // Other rows' content and hashes must be untouched
        Assert.Contains("Number of retries.", text);
        Assert.Contains("data-row-id=\"attr-0\" data-notes-hash=\"deadbeef\"", text);
        Assert.Contains("Runs the retry loop.", text);
        Assert.Contains("data-row-id=\"method-0\" data-notes-hash=\"cafef00d\"", text);
    }
}
