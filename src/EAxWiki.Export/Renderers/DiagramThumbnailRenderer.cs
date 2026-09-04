using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class DiagramThumbnailRenderer
{
    internal static IEnumerable<string> Render(EaElement element, string dir, ExportContext ctx)
    {
        if (!ctx.DiagramIndex.TryGetValue(element.Id, out var elementDiagrams))
            yield break;

        // Not collapsible (issue #96) — the diagram thumbnails are the primary visual
        // context for an element. Promoted from ### to ## for TOC parity with the
        // other element-page sections.
        yield return "## Appears on Diagrams";
        yield return string.Empty;
        yield return "<div class=\"diagram-thumbs\">";

        foreach (var (diagram, pkgDir) in elementDiagrams)
        {
            var diagDir = Path.Combine(pkgDir, "diagrams");
            var sanitized = MarkdownHelpers.SanitizeName(diagram.Name);
            var diagLink = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{sanitized}.md")).Replace('\\', '/');
            // Raw HTML <a href="…"> is not touched by mkdocs; the .md source path must be
            // rewritten to .html or clicking the thumbnail 404s (regressed by 9336ae4bb).
            var diagLinkHtml = diagLink.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                ? diagLink[..^3] + ".html" : diagLink;
            var pngRelPath = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{sanitized}.png")).Replace('\\', '/');

            yield return $"  <a href=\"{diagLinkHtml}\" class=\"diagram-thumb\"><img src=\"{pngRelPath}\" alt=\"{diagram.Name}\" loading=\"lazy\"><span>{MarkdownHelpers.EscapeCell(diagram.Name)}</span></a>";
        }

        yield return "</div>";
        yield return string.Empty;
    }
}