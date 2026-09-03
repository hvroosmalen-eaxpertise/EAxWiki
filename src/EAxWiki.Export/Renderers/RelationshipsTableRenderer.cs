using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class RelationshipsTableRenderer
{
    internal static IEnumerable<string> Render(EaElement element, string dir, ExportContext ctx)
    {
        if (element.Connectors.Count == 0)
            yield break;

        yield return "## Relationships";
        yield return string.Empty;
        yield return "| Type | Stereotype | Connected To |";
        yield return "|------|------------|-------------|";

        foreach (var conn in element.Connectors)
        {
            var otherId = conn.SourceId == element.Id ? conn.TargetId
                : conn.TargetId == element.Id ? conn.SourceId
                : -1;

            if (otherId <= 0) continue;

            string connectedTo;
            if (ctx.ElementLookup.TryGetValue(otherId, out var other))
            {
                var otherName = MarkdownHelpers.SanitizeName(other.Element.Name);
                var relativePath = Path.GetRelativePath(dir, Path.Combine(other.PackageDir, $"{otherName}.html")).Replace('\\', '/');
                connectedTo = $"[{MarkdownHelpers.EscapeCell(other.Element.Name)}]({relativePath})";
            }
            else
            {
                connectedTo = $"Element ID {otherId} (not in export)";
            }

            yield return $"| {MarkdownHelpers.EscapeCell(conn.Type)} | {MarkdownHelpers.EscapeCell(conn.Stereotype)} | {connectedTo} |";
        }

        yield return string.Empty;
        yield return "[↑ Back to top](#)";
        yield return string.Empty;
    }
}
