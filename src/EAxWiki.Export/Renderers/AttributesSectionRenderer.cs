using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class AttributesSectionRenderer
{
    internal static IEnumerable<string> Render(EaElement element, ExportContext ctx, string wikiRelPathHtml)
    {
        if (element.Attributes.Count == 0)
            yield break;

        yield return "## Attributes";
        yield return string.Empty;

        if (ctx.ApiPort > 0)
        {
            yield return "<table>";
            yield return "<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>";
            yield return "<tbody>";
            var attrIdx = 0;
            foreach (var attr in element.Attributes)
            {
                var rowId = $"attr-{attrIdx++}";
                var (viewHtml, editRowHtml) = RowNotesWidgetRenderer.Render(
                    rowId, attr.Notes, "attribute", "table-row", element.Id, wikiRelPathHtml, ctx.ApiPort, ctx.ApiToken, 4,
                    ("attr-name", attr.Name), ("attr-type", attr.Type));
                yield return $"<tr><td>{HtmlHelpers.HtmlEscape(attr.Name)}</td><td>{HtmlHelpers.HtmlEscape(attr.Type)}</td><td>{HtmlHelpers.HtmlEscape(attr.DefaultValue ?? "")}</td><td>{viewHtml}</td></tr>";
                yield return editRowHtml;
            }
            yield return "</tbody>";
            yield return "</table>";
        }
        else
        {
            yield return "| Name | Type | Default | Description |";
            yield return "|------|------|---------|-------------|";
            foreach (var attr in element.Attributes)
            {
                var desc = (attr.Notes ?? "").Replace("|", "\\|").Replace("\n", "<br/>");
                yield return $"| {MarkdownHelpers.EscapeCell(attr.Name)} | {MarkdownHelpers.EscapeCell(attr.Type)} | {MarkdownHelpers.EscapeCell(attr.DefaultValue ?? "")} | {desc} |";
            }
        }

        yield return string.Empty;
        yield return "[↑ Back to top](#)";
        yield return string.Empty;
    }
}
