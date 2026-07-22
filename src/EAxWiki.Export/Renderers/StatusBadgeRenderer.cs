using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class StatusBadgeRenderer
{
    internal static string Render(EaElement element, ExportContext ctx, string wikiRelPathHtml, string statusOptionsJson, string? kind = null)
    {
        var statusBadgeClass = string.IsNullOrEmpty(element.Status) ? "status-not-set" : $"status-{element.Status.ToLowerInvariant()}";
        var statusBadgeLabel = string.IsNullOrEmpty(element.Status) ? "Not Set" : MarkdownHelpers.EscapeCell(element.Status);
        var statusBadgeHtml = $"<span class=\"status-badge {statusBadgeClass}\">{statusBadgeLabel}</span>";

        if (ctx.ApiPort > 0)
        {
            var kindAttr = !string.IsNullOrEmpty(kind) ? $" data-kind=\"{kind}\"" : "";
            return $"<span id=\"ea-status-editor\" class=\"ea-status-editor\"" +
                   $" data-ea-id=\"{element.Id}\"" +
                   kindAttr +
                   $" data-status=\"{HtmlHelpers.HtmlEscape(element.Status)}\"" +
                   $" data-options='{statusOptionsJson}'" +
                   $" data-file-path=\"{wikiRelPathHtml}\"" +
                   $" data-api-port=\"{ctx.ApiPort}\"" +
                   $" data-api-token=\"{HtmlHelpers.HtmlEscape(ctx.ApiToken)}\">" +
                   statusBadgeHtml +
                   "<button class=\"ea-status-edit-btn\" type=\"button\" aria-label=\"Edit status\">&#9998;</button>" +
                   "</span>";
        }

        return statusBadgeHtml;
    }
}
