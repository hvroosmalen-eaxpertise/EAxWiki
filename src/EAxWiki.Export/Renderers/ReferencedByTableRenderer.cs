using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class ReferencedByTableRenderer
{
    internal static IEnumerable<string> Render(EaElement element, string dir, ExportContext ctx)
    {
        if (!ctx.IncomingIndex.TryGetValue(element.Id, out var incomingConns))
            yield break;

        yield return "### Referenced By";
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
        yield return "[↑ Back to top](#)";
        yield return string.Empty;
    }
}
