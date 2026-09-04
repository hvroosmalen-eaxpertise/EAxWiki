using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class ReferencedByTableRenderer
{
    internal static IEnumerable<string> Render(EaElement element, string dir, ExportContext ctx)
    {
        if (!ctx.IncomingIndex.TryGetValue(element.Id, out var incomingConns))
            yield break;

        // Collapsible section (issue #96). Default: closed. Promoted from ### to ##
        // for TOC parity with Tagged Values / Relationships / Appears on Diagrams.
        yield return "<details class=\"ea-section\" data-ea-section-id=\"referenced-by\" markdown=\"1\">";
        yield return "<summary><h2 id=\"referenced-by\">Referenced By</h2></summary>";
        yield return string.Empty;
        yield return "| Type | Stereotype | Source |";
        yield return "|------|------------|--------|";

        foreach (var (conn, sourceId) in incomingConns)
        {
            string source;
            if (ctx.ElementLookup.TryGetValue(sourceId, out var srcElem))
            {
                var srcName = MarkdownHelpers.SanitizeName(srcElem.Element.Name);
                var relativePath = Path.GetRelativePath(dir, Path.Combine(srcElem.PackageDir, $"{srcName}.html")).Replace('\\', '/');
                source = $"[{MarkdownHelpers.EscapeCell(srcElem.Element.Name)}]({relativePath})";
            }
            else
            {
                source = $"Element ID {sourceId} (not in export)";
            }

            yield return $"| {MarkdownHelpers.EscapeCell(conn.Type)} | {MarkdownHelpers.EscapeCell(conn.Stereotype)} | {source} |";
        }

        yield return string.Empty;
        yield return "</details>";
        yield return string.Empty;
    }
}
