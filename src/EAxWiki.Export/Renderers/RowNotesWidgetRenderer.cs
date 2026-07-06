using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class RowNotesWidgetRenderer
{
    internal static (string ViewHtml, string EditSurfaceHtml) Render(
        string rowId, string? notesValue, string kind, string surface, int elementId,
        string wikiRelPathHtml, int apiPort, string apiToken, int colspan,
        params (string Attr, string Value)[] matchAttrs)
    {
        var normalized = FrontmatterParser.NormalizeNotesHtml(notesValue);
        var hash = HtmlHelpers.ComputeNotesHash(normalized);
        var matchAttrsHtml = string.Join(" ", matchAttrs.Select(a => $"data-{a.Attr}=\"{HtmlHelpers.HtmlEscape(a.Value)}\""));

        var viewHtml =
            $"<span class=\"ea-row-notes-text\"><!--ea-row-notes-start:{rowId}-->{normalized}<!--ea-row-notes-end:{rowId}--></span>" +
            $"<button class=\"ea-row-notes-edit-btn\" type=\"button\" data-surface=\"{surface}\" data-row-id=\"{rowId}\" data-notes-hash=\"{hash}\"" +
            $" data-kind=\"{kind}\" data-el-id=\"{elementId}\" {matchAttrsHtml}" +
            $" data-file-path=\"{wikiRelPathHtml}\" data-api-port=\"{apiPort}\" data-api-token=\"{HtmlHelpers.HtmlEscape(apiToken)}\" aria-label=\"Edit description\">&#9998;</button>";

        var editSurfaceHtml = surface == "table-row"
            ? $"<tr class=\"ea-row-edit\" data-row-id=\"{rowId}\" style=\"display:none\"><td colspan=\"{colspan}\"></td></tr>"
            : string.Empty;

        return (viewHtml, editSurfaceHtml);
    }
}
