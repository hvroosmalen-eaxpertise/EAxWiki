using EAxWiki.Core.Models;
using EAxWiki.Export;
using EAxWiki.Export.Renderers;

namespace EAxWiki.Tests;

public class ElementPageWriterTests
{
    private static EaElement MakeElement(Action<EaElement>? configure = null)
    {
        var e = new EaElement
        {
            Id = 1,
            Name = "TestElement",
            Type = "Class",
            Status = "Proposed",
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 6, 1),
        };
        configure?.Invoke(e);
        return e;
    }

    private static ExportContext PlainContext(string outputPath = @"C:\out") =>
        new(outputPath, [], [], [], [], [], [])
        {
            ApiPort = 0,
            ApiToken = ""
        };

    private static ExportContext RichContext(string outputPath = @"C:\out") =>
        new(outputPath, [], [], [], [], [], [])
        {
            ApiPort = 8080,
            ApiToken = "secret"
        };

    #region HtmlHelpers

    [Fact]
    public void HtmlEscape_EncodesSpecialChars()
    {
        Assert.Equal("&amp;&lt;&gt;&quot;&#39;", HtmlHelpers.HtmlEscape("&<>\"'"));
    }

    [Fact]
    public void HtmlEscape_Null_ReturnsEmpty()
    {
        Assert.Equal("", HtmlHelpers.HtmlEscape(null!));
    }

    [Fact]
    public void JsonEscape_EncodesQuotesAndBackslashes()
    {
        Assert.Equal("a\\\"b\\\\c", HtmlHelpers.JsonEscape("a\"b\\c"));
    }

    [Fact]
    public void ComputeStatusHash_Consistent()
    {
        var hash = HtmlHelpers.ComputeStatusHash("Proposed");
        Assert.Equal(8, hash.Length);
        Assert.All(hash, c => Assert.True(char.IsAsciiHexDigit(c)));
    }

    [Fact]
    public void ComputeNotesHash_Empty_ReturnsSameLength()
    {
        var hash = HtmlHelpers.ComputeNotesHash("");
        Assert.Equal(8, hash.Length);
    }

    [Fact]
    public void ComputeNotesHash_Null_ReturnsSameAsEmpty()
    {
        Assert.Equal(HtmlHelpers.ComputeNotesHash(""), HtmlHelpers.ComputeNotesHash(null));
    }

    #endregion

    #region StatusBadgeRenderer

    [Fact]
    public void StatusBadgeRenderer_PlainMode_ReturnsSimpleBadge()
    {
        var element = MakeElement();
        var result = StatusBadgeRenderer.Render(element, PlainContext(), "/path", "[]");
        Assert.Contains("status-proposed", result);
        Assert.Contains("Proposed", result);
        Assert.DoesNotContain("ea-status-editor", result);
    }

    [Fact]
    public void StatusBadgeRenderer_PlainMode_EmptyStatus_ShowsNotSet()
    {
        var element = MakeElement(e => e.Status = "");
        var result = StatusBadgeRenderer.Render(element, PlainContext(), "/path", "[]");
        Assert.Contains("status-not-set", result);
        Assert.Contains("Not Set", result);
    }

    [Fact]
    public void StatusBadgeRenderer_RichMode_IncludesEditorAttributes()
    {
        var element = MakeElement();
        var result = StatusBadgeRenderer.Render(element, RichContext(), "/path", "[\"Proposed\"]");
        Assert.Contains("ea-status-editor", result);
        Assert.Contains("data-api-port=\"8080\"", result);
        Assert.Contains("data-api-token=\"secret\"", result);
        Assert.Contains("data-ea-id=\"1\"", result);
    }

    #endregion

    #region NotesWidgetRenderer

    [Fact]
    public void NotesWidgetRenderer_PlainMode_WithNotes_ReturnsNotes()
    {
        var element = MakeElement(e => e.Notes = "Some notes content");
        var result = string.Join("\n", NotesWidgetRenderer.Render(element, PlainContext(), "Some notes content", "/path"));
        Assert.Contains("Some notes content", result);
    }

    [Fact]
    public void NotesWidgetRenderer_PlainMode_EmptyNotes_ReturnsEmpty()
    {
        var element = MakeElement(e => e.Notes = "");
        var result = NotesWidgetRenderer.Render(element, PlainContext(), "", "/path");
        Assert.Empty(result);
    }

    [Fact]
    public void NotesWidgetRenderer_RichMode_IncludesEditorDiv()
    {
        var element = MakeElement();
        var result = string.Join("\n", NotesWidgetRenderer.Render(element, RichContext(), "notes", "/path"));
        Assert.Contains("ea-notes-editor", result);
        Assert.Contains("data-api-port=\"8080\"", result);
        Assert.Contains("notes", result);
        Assert.Contains("ea-notes-edit-btn", result);
        Assert.Contains("ea-notes-content", result);
    }

    #endregion

    #region RowNotesWidgetRenderer

    [Fact]
    public void RowNotesWidgetRenderer_TableRowSurface_IncludesEditRow()
    {
        var (viewHtml, editSurfaceHtml) = RowNotesWidgetRenderer.Render(
            "row1", "desc", "attribute", "table-row", 1, "/p", 8080, "tok", 4,
            ("attr-name", "MyAttr"), ("attr-type", "string"));

        Assert.Contains("data-row-id=\"row1\"", viewHtml);
        Assert.Contains("data-kind=\"attribute\"", viewHtml);
        Assert.Contains("data-surface=\"table-row\"", viewHtml);
        Assert.Contains("data-el-id=\"1\"", viewHtml);
        Assert.Contains("data-attr-name=\"MyAttr\"", viewHtml);
        Assert.Contains("row-notes-edit-btn", viewHtml);
        Assert.Contains("desc", viewHtml);
        Assert.Contains("ea-row-edit", editSurfaceHtml);
        Assert.Contains("colspan=\"4\"", editSurfaceHtml);
    }

    [Fact]
    public void RowNotesWidgetRenderer_InlineSurface_NoEditRow()
    {
        var (viewHtml, editSurfaceHtml) = RowNotesWidgetRenderer.Render(
            "m1", "notes", "method", "inline", 1, "/p", 8080, "tok", 0,
            ("method-name", "Foo"), ("return-type", "void"));

        Assert.Contains("data-surface=\"inline\"", viewHtml);
        Assert.Empty(editSurfaceHtml);
    }

    [Fact]
    public void RowNotesWidgetRenderer_NullNotes_ProducesNormalizedEmpty()
    {
        var (viewHtml, _) = RowNotesWidgetRenderer.Render(
            "r1", null, "tagged-value", "inline", 1, "/p", 8080, "tok", 0);

        Assert.Contains("<!--ea-row-notes-start:r1-->", viewHtml);
    }

    #endregion

    #region AttributesSectionRenderer

    [Fact]
    public void AttributesSectionRenderer_Empty_ReturnsNothing()
    {
        var element = MakeElement();
        var result = AttributesSectionRenderer.Render(element, PlainContext(), "/p");
        Assert.Empty(result);
    }

    [Fact]
    public void AttributesSectionRenderer_PlainMode_ReturnsMarkdownTable()
    {
        var element = MakeElement(e => e.Attributes = [new EaAttribute { Name = "Attr1", Type = "int", DefaultValue = "42", Notes = "desc" }]);
        var result = string.Join("\n", AttributesSectionRenderer.Render(element, PlainContext(), "/p"));
        Assert.Contains("## Attributes", result);
        Assert.Contains("| Name | Type | Default | Description |", result);
        Assert.Contains("| Attr1 | int | 42 | desc |", result);
    }

    [Fact]
    public void AttributesSectionRenderer_RichMode_ReturnsHtmlTable()
    {
        var element = MakeElement(e => e.Attributes = [new EaAttribute { Name = "A1", Type = "int", Notes = "desc" }]);
        var result = string.Join("\n", AttributesSectionRenderer.Render(element, RichContext(), "/p"));
        Assert.Contains("<table>", result);
        Assert.Contains("<th>Name</th>", result);
        Assert.Contains("data-kind=\"attribute\"", result);
        Assert.Contains("row-notes-edit-btn", result);
    }

    #endregion

    #region MethodsSectionRenderer

    [Fact]
    public void MethodsSectionRenderer_Empty_ReturnsNothing()
    {
        var element = MakeElement();
        var result = MethodsSectionRenderer.Render(element, PlainContext(), "/p");
        Assert.Empty(result);
    }

    [Fact]
    public void MethodsSectionRenderer_PlainMode_ReturnsMarkdownSections()
    {
        var element = MakeElement(e => e.Methods = [new EaMethod { Name = "DoWork", Type = "void", IsStatic = true, Notes = "does work" }]);
        var result = string.Join("\n", MethodsSectionRenderer.Render(element, PlainContext(), "/p"));
        Assert.Contains("## Methods", result);
        Assert.Contains("### DoWork *(static)*", result);
        Assert.Contains("**Returns:** `void`", result);
        Assert.Contains("does work", result);
    }

    [Fact]
    public void MethodsSectionRenderer_RichMode_IncludesWidget()
    {
        var element = MakeElement(e => e.Methods = [new EaMethod { Name = "Foo", Type = "int", IsStatic = false, Notes = "foo notes" }]);
        var result = string.Join("\n", MethodsSectionRenderer.Render(element, RichContext(), "/p"));
        Assert.Contains("ea-row-notes-widget", result);
        Assert.Contains("data-kind=\"method\"", result);
    }

    [Fact]
    public void MethodsSectionRenderer_RichMode_NoNotes_OmitsWidget()
    {
        var element = MakeElement(e => e.Methods = [new EaMethod { Name = "Bar", Type = "void", IsStatic = false, Notes = "" }]);
        var result = string.Join("\n", MethodsSectionRenderer.Render(element, RichContext(), "/p"));
        Assert.Contains("data-kind=\"method\"", result);
    }

    #endregion

    #region TaggedValuesSectionRenderer

    [Fact]
    public void TaggedValuesSectionRenderer_Empty_ReturnsNothing()
    {
        var element = MakeElement();
        var result = TaggedValuesSectionRenderer.Render(element, PlainContext(), "/p");
        Assert.Empty(result);
    }

    [Fact]
    public void TaggedValuesSectionRenderer_PlainMode_ReturnsMarkdownTable()
    {
        var element = MakeElement(e => e.TaggedValues = [new EaTaggedValue { Name = "Tag1", Value = "Val1", Notes = "notes" }]);
        var result = string.Join("\n", TaggedValuesSectionRenderer.Render(element, PlainContext(), "/p"));
        Assert.Contains("data-ea-section-id=\"tagged-values\"", result);
        Assert.Contains("<h2 id=\"tagged-values\">Tagged Values</h2>", result);
        Assert.Contains("| Tag1 | Val1 | notes |", result);
    }

    [Fact]
    public void TaggedValuesSectionRenderer_RichMode_ReturnsHtmlTable()
    {
        var element = MakeElement(e => e.TaggedValues = [new EaTaggedValue { Name = "T1", Value = "V1", Notes = "n1" }]);
        var result = string.Join("\n", TaggedValuesSectionRenderer.Render(element, RichContext(), "/p"));
        Assert.Contains("<table>", result);
        Assert.Contains("data-kind=\"tagged-value\"", result);
    }

    #endregion

    #region RelationshipsTableRenderer

    [Fact]
    public void RelationshipsTableRenderer_Empty_ReturnsNothing()
    {
        var element = MakeElement();
        var ctx = PlainContext();
        var result = RelationshipsTableRenderer.Render(element, @"C:\pkg", ctx);
        Assert.Empty(result);
    }

    [Fact]
    public void RelationshipsTableRenderer_WithConnectorToKnownElement()
    {
        var target = new EaElement { Id = 2, Name = "Target" };
        var element = MakeElement(e => e.Connectors = [new EaConnector { Id = 1, Name = "conn", Type = "Association", Stereotype = "uses", SourceId = 1, TargetId = 2 }]);
        var ctx = new ExportContext(@"C:\out",
            [(target, @"C:\pkg")],
            new Dictionary<int, (EaElement, string)> { [2] = (target, @"C:\pkg") },
            [], [], [],
            new Dictionary<int, (string, int?)>()) { ApiPort = 0 };
        var result = string.Join("\n", RelationshipsTableRenderer.Render(element, @"C:\pkg", ctx));
        Assert.Contains("data-ea-section-id=\"relationships\"", result);
        Assert.Contains("<h2 id=\"relationships\">Relationships</h2>", result);
        Assert.Contains("Association", result);
        Assert.Contains("Target", result);
    }

    [Fact]
    public void RelationshipsTableRenderer_WithConnectorToUnknownElement()
    {
        var element = MakeElement(e => e.Connectors = [new EaConnector { Id = 1, Name = "c", Type = "Dependency", SourceId = 1, TargetId = 999 }]);
        var result = string.Join("\n", RelationshipsTableRenderer.Render(element, @"C:\pkg", PlainContext()));
        Assert.Contains("Element ID 999 (not in export)", result);
    }

    [Fact]
    public void RelationshipsTableRenderer_SelfReferencingConnector_LinksToSelf()
    {
        var element = MakeElement(e => e.Connectors = [new EaConnector { Id = 1, Name = "self", Type = "Self", SourceId = 1, TargetId = 1 }]);
        var ctx = new ExportContext(@"C:\out", [],
            new Dictionary<int, (EaElement, string)> { [1] = (element, @"C:\pkg") },
            [], [], [],
            new Dictionary<int, (string, int?)>()) { ApiPort = 0 };
        var result = string.Join("\n", RelationshipsTableRenderer.Render(element, @"C:\pkg", ctx));
        Assert.Contains("Self", result);
        Assert.Contains("TestElement", result);
    }

    #endregion

    #region ReferencedByTableRenderer

    [Fact]
    public void ReferencedByTableRenderer_NoIncoming_ReturnsNothing()
    {
        var ctx = PlainContext();
        var result = ReferencedByTableRenderer.Render(MakeElement(), @"C:\pkg", ctx);
        Assert.Empty(result);
    }

    [Fact]
    public void ReferencedByTableRenderer_WithIncomingConnectorMissingSource_ShowsNotInExport()
    {
        var ctx = new ExportContext(@"C:\out", [],
            new Dictionary<int, (EaElement, string)>(),
            [], [],
            new Dictionary<int, List<(EaConnector, int)>> { [1] = [(new EaConnector { Id = 10, Type = "Flow", Stereotype = "triggers" }, 999)] },
            new Dictionary<int, (string, int?)>()) { ApiPort = 0 };
        var result = string.Join("\n", ReferencedByTableRenderer.Render(MakeElement(), @"C:\pkg", ctx));
        Assert.Contains("data-ea-section-id=\"referenced-by\"", result);
        Assert.Contains("<h2 id=\"referenced-by\">Referenced By</h2>", result);
        Assert.Contains("Element ID 999 (not in export)", result);
    }

    [Fact]
    public void ReferencedByTableRenderer_WithIncomingConnector()
    {
        var source = new EaElement { Id = 2, Name = "Source" };
        var ctx = new ExportContext(@"C:\out",
            [(source, @"C:\srcPkg")],
            new Dictionary<int, (EaElement, string)> { [2] = (source, @"C:\srcPkg") },
            [], [],
            new Dictionary<int, List<(EaConnector, int)>> { [1] = [(new EaConnector { Id = 10, Type = "Flow", Stereotype = "triggers" }, 2)] },
            new Dictionary<int, (string, int?)>()) { ApiPort = 0 };
        var result = string.Join("\n", ReferencedByTableRenderer.Render(MakeElement(), @"C:\pkg", ctx));
        Assert.Contains("data-ea-section-id=\"referenced-by\"", result);
        Assert.Contains("<h2 id=\"referenced-by\">Referenced By</h2>", result);
        Assert.Contains("Flow", result);
        Assert.Contains("Source", result);
    }

    #endregion

    #region DiagramThumbnailRenderer

    [Fact]
    public void DiagramThumbnailRenderer_NoDiagrams_ReturnsNothing()
    {
        var ctx = PlainContext();
        var result = DiagramThumbnailRenderer.Render(MakeElement(), @"C:\pkg", ctx);
        Assert.Empty(result);
    }

    [Fact]
    public void DiagramThumbnailRenderer_WithDiagrams_ReturnsThumbnailGrid()
    {
        var diag = new EaDiagram { Id = 55, Name = "My Diagram", PackageId = 10 };
        var ctx = new ExportContext(@"C:\out", [], [], [],
            new Dictionary<int, List<(EaDiagram, string)>> { [1] = [(diag, @"C:\pkg")] }, [],
            new Dictionary<int, (string, int?)>()) { ApiPort = 0 };
        var result = string.Join("\n", DiagramThumbnailRenderer.Render(MakeElement(), @"C:\pkg", ctx));
        Assert.Contains("diagram-thumbs", result);
        Assert.Contains("My Diagram", result);
        Assert.Contains("diagrams/My Diagram.png", result);
        Assert.Contains("diagrams/My Diagram.html", result);
    }

    #endregion


}
