using EAxWiki.Core.Models;

namespace EAxWiki.Export.Renderers;

internal static class NotesWidgetRenderer
{
    internal static IEnumerable<string> Render(EaPackage package, ExportContext ctx, string normalizedNotes, string wikiRelPathHtml, string? kind = null)
    {
        var element = new EaElement { Id = package.Id, Status = package.Status, Notes = package.Notes };
        return Render(element, ctx, normalizedNotes, wikiRelPathHtml, kind);
    }

    internal static IEnumerable<string> Render(EaElement element, ExportContext ctx, string normalizedNotes, string wikiRelPathHtml, string? kind = null)
    {
        if (ctx.ApiPort > 0)
        {
            var aiConfigured = ctx.ApiPort > 0 && ctx.AiConfigured;
            var kindAttr = kind == "package" ? " data-kind=\"package\"" : "";
            var notesStartMarker = kind == "package" ? "ea-package-notes-start" : "ea-notes-start";
            var notesEndMarker = kind == "package" ? "ea-package-notes-end" : "ea-notes-end";
            yield return
                $"<div id=\"ea-notes-editor\" class=\"ea-notes-editor\"" +
                $" data-ea-id=\"{element.Id}\"" +
                kindAttr +
                $" data-file-path=\"{wikiRelPathHtml}\"" +
                $" data-api-port=\"{ctx.ApiPort}\"" +
                $" data-api-token=\"{HtmlHelpers.HtmlEscape(ctx.ApiToken)}\"" +
                $" data-ai-configured=\"{aiConfigured.ToString().ToLowerInvariant()}\">";
            yield return "<button id=\"ea-notes-edit-btn\" class=\"ea-notes-edit-btn\" type=\"button\" aria-label=\"Edit notes\">&#9998;</button>";
            yield return "<div class=\"ea-notes-content\">";
            yield return $"<!--{notesStartMarker}-->";
            yield return normalizedNotes;
            yield return $"<!--{notesEndMarker}-->";
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
