using EAxWiki.Core.Models;

namespace EAxWiki.Export.Renderers;

internal static class NotesWidgetRenderer
{
    internal static IEnumerable<string> Render(EaElement element, ExportContext ctx, string normalizedNotes, string wikiRelPathHtml)
    {
        if (ctx.ApiPort > 0)
        {
            yield return
                $"<div id=\"ea-notes-editor\" class=\"ea-notes-editor\"" +
                $" data-ea-id=\"{element.Id}\"" +
                $" data-file-path=\"{wikiRelPathHtml}\"" +
                $" data-api-port=\"{ctx.ApiPort}\"" +
                $" data-api-token=\"{HtmlHelpers.HtmlEscape(ctx.ApiToken)}\">";
            yield return "<button id=\"ea-notes-edit-btn\" class=\"ea-notes-edit-btn\" type=\"button\" aria-label=\"Edit notes\">&#9998;</button>";
            yield return "<div class=\"ea-notes-content\">";
            yield return "<!--ea-notes-start-->";
            yield return normalizedNotes;
            yield return "<!--ea-notes-end-->";
            yield return "</div>";
            yield return "</div>";
            yield return string.Empty;
        }
        else if (!string.IsNullOrWhiteSpace(element.Notes))
        {
            yield return element.Notes;
            yield return string.Empty;
        }
    }
}
