using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class MethodsSectionRenderer
{
    internal static IEnumerable<string> Render(EaElement element, ExportContext ctx, string wikiRelPathHtml)
    {
        if (element.Methods.Count == 0)
            yield break;

        yield return "## Methods";
        yield return string.Empty;

        var methodIdx = 0;
        foreach (var method in element.Methods)
        {
            var staticTag = method.IsStatic ? " *(static)*" : "";
            yield return $"### {method.Name}{staticTag}";
            yield return string.Empty;
            yield return $"**Returns:** `{method.Type}`";
            yield return string.Empty;

            if (ctx.ApiPort > 0)
            {
                var rowId = $"method-{methodIdx++}";
                var (viewHtml, _) = RowNotesWidgetRenderer.Render(
                    rowId, method.Notes, "method", "inline", element.Id, wikiRelPathHtml, ctx.ApiPort, ctx.ApiToken, 0,
                    ("method-name", method.Name), ("return-type", method.Type), ("is-static", method.IsStatic ? "true" : "false"));
                yield return $"<div class=\"ea-row-notes-widget\" data-row-id=\"{rowId}\">{viewHtml}</div>";
                yield return string.Empty;
            }
            else if (!string.IsNullOrWhiteSpace(method.Notes))
            {
                yield return method.Notes;
                yield return string.Empty;
            }
        }
    }
}
