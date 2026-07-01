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
}
