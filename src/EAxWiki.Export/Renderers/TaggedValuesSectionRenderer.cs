using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class TaggedValuesSectionRenderer
{
    internal static IEnumerable<string> Render(EaElement element, ExportContext ctx, string wikiRelPathHtml)
    {
        if (element.TaggedValues.Count == 0)
            yield break;

        yield return "## Tagged Values";
        yield return string.Empty;

        if (ctx.ApiPort > 0)
        {
            yield return "<table>";
            yield return "<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>";
            yield return "<tbody>";
            var tvIdx = 0;
            foreach (var tv in element.TaggedValues)
            {
                var rowId = $"tag-{tvIdx++}";
                var (viewHtml, editRowHtml) = RowNotesWidgetRenderer.Render(
                    rowId, tv.Notes, "tagged-value", "table-row", element.Id, wikiRelPathHtml, ctx.ApiPort, ctx.ApiToken, 3,
                    ("tag-name", tv.Name), ("tag-value", tv.Value));
                yield return $"<tr><td>{HtmlHelpers.HtmlEscape(tv.Name)}</td><td>{HtmlHelpers.HtmlEscape(tv.Value)}</td><td>{viewHtml}</td></tr>";
                yield return editRowHtml;
            }
            yield return "</tbody>";
            yield return "</table>";
        }
        else
        {
            yield return "| Name | Value | Notes |";
            yield return "|------|-------|-------|";
            foreach (var tv in element.TaggedValues)
                yield return $"| {MarkdownHelpers.EscapeCell(tv.Name)} | {MarkdownHelpers.EscapeCell(tv.Value)} | {MarkdownHelpers.EscapeCell(tv.Notes ?? "")} |";
        }

        yield return string.Empty;
        yield return "[↑ Back to top](#)";
        yield return string.Empty;
    }
}
