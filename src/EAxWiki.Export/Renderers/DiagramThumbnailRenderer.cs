using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class DiagramThumbnailRenderer
{
    internal static IEnumerable<string> Render(EaElement element, string dir, ExportContext ctx)
    {
        if (!ctx.DiagramIndex.TryGetValue(element.Id, out var elementDiagrams))
            yield break;

        yield return "### Appears on Diagrams";
        yield return string.Empty;
        yield return "<div class=\"diagram-thumbs\">";

        foreach (var (diagram, pkgDir) in elementDiagrams)
        {
            var diagDir = Path.Combine(pkgDir, "diagrams");
            var sanitized = MarkdownHelpers.SanitizeName(diagram.Name);
            var diagLink = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{sanitized}.md")).Replace('\\', '/');
            var diagLinkHtml = diagLink.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                ? diagLink[..^3] + ".html" : diagLink;

            var pngRelPath = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{sanitized}.png")).Replace('\\', '/');

            yield return $"  <a href=\"{diagLinkHtml}\" class=\"diagram-thumb\"><img src=\"{pngRelPath}\" alt=\"{diagram.Name}\" loading=\"lazy\"><span>{MarkdownHelpers.EscapeCell(diagram.Name)}</span></a>";
        }

        yield return "</div>";
        yield return string.Empty;
        yield return "[↑ Back to top](#)";
        yield return string.Empty;
    }
}
