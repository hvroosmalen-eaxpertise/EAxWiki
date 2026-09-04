using System.Threading;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;
using EAxWiki.Export.Renderers;

namespace EAxWiki.Export.Exporters;

internal class ElementPageWriter(IOutputWriter writer, ILogger logger)
{
    public async Task WriteAsync(EaElement element, string dir, ExportContext ctx, string? fileNameOverride = null, CancellationToken ct = default)
    {
        var outputDir = ctx.OutputPath;
        var baseName = fileNameOverride ?? MarkdownHelpers.SanitizeName(element.Name);
        var filePath = Path.Combine(dir, $"{baseName}.md");

        var isNew = true;
        if (!ctx.Force && element.ModifiedDate != DateTime.MinValue)
        {
            try
            {
                var fileTime = File.GetLastWriteTimeUtc(filePath);
                if (fileTime > DateTime.UnixEpoch)
                {
                    isNew = false;
                    var elementTime = element.ModifiedDate.Kind == DateTimeKind.Utc
                        ? element.ModifiedDate
                        : element.ModifiedDate.ToUniversalTime();
                    if (fileTime >= elementTime
                        && IsAiAttributeCurrent(filePath, ctx.AiConfigured)
                        && IsTemplateCurrent(filePath))
                    {
                        logger.LogDebug("Skipped {ElementName}", element.Name);
                        return;
                    }
                }
            }
            catch (IOException)
            {
            }
        }
        logger.LogInformation("{Action} {ElementName}", isNew ? "Created" : "Updated", element.Name);

        var createdStr = element.CreatedDate?.ToString("yyyy-MM-dd") ?? "-";
        var modifiedStr = element.ModifiedDate == DateTime.MinValue ? "-" : element.ModifiedDate.ToString("yyyy-MM-dd");

        var statusOptionsList = ctx.StatusTypes.Count > 0
            ? ctx.StatusTypes
            : (IReadOnlyList<string>)["Approved", "Implemented", "Mandatory", "Proposed", "Validated"];
        var statusOptions = string.Join(", ", statusOptionsList);
        var statusHash = HtmlHelpers.ComputeStatusHash(element.Status);
        var normalizedNotes = FrontmatterParser.NormalizeNotesHtml(element.Notes);
        var notesHash = HtmlHelpers.ComputeNotesHash(normalizedNotes);
        var statusOptionsJson = HtmlHelpers.HtmlEscape("[" + string.Join(",", statusOptionsList.Select(s => $"\"{HtmlHelpers.JsonEscape(s)}\"")) + "]");
        var wikiRelPath = Path.GetRelativePath(outputDir, filePath).Replace('\\', '/');
        var wikiRelPathHtml = HtmlHelpers.HtmlEscape(wikiRelPath);

        var statusBadgeHtml = StatusBadgeRenderer.Render(element, ctx, wikiRelPathHtml, statusOptionsJson);

        var lines = new List<string>
        {
            "---",
            $"ea_id: {element.Id}",
            $"status: {element.Status}",
            $"status_options: [{statusOptions}]",
            $"ea_hash: {statusHash}",
            $"notes_hash: {notesHash}",
            "---",
            string.Empty,
            $"# {MarkdownHelpers.GetStereotypeLabel(element)} {element.Name}",
            string.Empty,
            $"**Type:** {element.Type}  " +
            $"**Stereotype:** {MarkdownHelpers.EscapeCell(element.Stereotype)}  " +
            (string.IsNullOrWhiteSpace(element.StereotypeEx) ? "" : $"**StereotypeEx:** {MarkdownHelpers.EscapeCell(element.StereotypeEx)}  ") +
            (string.IsNullOrWhiteSpace(element.FQStereotype) ? "" : $"**FQStereotype:** {MarkdownHelpers.EscapeCell(element.FQStereotype)}  "),
            $"**Status:** {statusBadgeHtml}  ",
            $"**Created:** {createdStr}  **Modified:** {modifiedStr}",
            string.Empty,
            string.Empty,
            MarkdownHelpers.BuildBreadcrumb(element.PackageId, dir, outputDir, ctx.PackageLookup,
                msg => logger.LogWarning("{Message} (element '{Name}')", msg, element.Name)),
            string.Empty,
        };

        lines.AddRange(NotesWidgetRenderer.Render(element, ctx, normalizedNotes, wikiRelPathHtml));
        lines.AddRange(AttributesSectionRenderer.Render(element, ctx, wikiRelPathHtml));
        lines.AddRange(MethodsSectionRenderer.Render(element, ctx, wikiRelPathHtml));
        lines.AddRange(TaggedValuesSectionRenderer.Render(element, ctx, wikiRelPathHtml));
        lines.AddRange(RelationshipsTableRenderer.Render(element, dir, ctx));
        lines.AddRange(DiagramThumbnailRenderer.Render(element, dir, ctx));
        lines.AddRange(ReferencedByTableRenderer.Render(element, dir, ctx));

        lines.AddRange(["---", string.Empty, "## Relationship Graph", string.Empty,
            $"<div id=\"ea-graph-container\" data-focal-id=\"{element.Id}\"></div>", string.Empty]);

        // Template-freshness marker (issue #96). IsTemplateCurrent looks for this on every
        // element page — pre-issue-96 pages don't have it and get regenerated. Bump the
        // suffix whenever the element-page template changes in a way old files should be
        // rewritten for.
        lines.Add(TemplateMarker);
        lines.Add(string.Empty);

        await writer.WriteFileAsync(filePath, string.Join(Environment.NewLine, lines), ct);
        ctx.WrittenMdFiles.Add(filePath);
    }

    // Regenerate the element page when the notes-editor widget's data-ai-configured attribute
    // in the file doesn't match the current AiConfigured state. Otherwise a page written
    // before AI was configured (or after AI was removed) keeps the wrong flag until EA bumps
    // the element's ModifiedDate. Cheap: reads only the first ~60 lines — the widget sits
    // near the top of the page.
    private static bool IsAiAttributeCurrent(string filePath, bool aiConfigured)
    {
        var expected = "data-ai-configured=\"" + (aiConfigured ? "true" : "false") + "\"";
        var wrong = "data-ai-configured=\"" + (aiConfigured ? "false" : "true") + "\"";
        try
        {
            foreach (var line in File.ReadLines(filePath).Take(60))
            {
                if (line.Contains(expected, StringComparison.Ordinal)) return true;
                if (line.Contains(wrong, StringComparison.Ordinal)) return false;
            }
        }
        catch (IOException) { return false; }
        // No widget on this page (e.g. API not enabled at export time) — nothing to force.
        return true;
    }

    // Element-page template version marker. Emitted at the bottom of every page and
    // looked for by IsTemplateCurrent. Bump the suffix whenever the element-page
    // template changes in a way pre-existing files should be regenerated for
    // (issue #96 introduced collapsible <details> sections, bumped to v2).
    private const string TemplateMarker = "<!-- ea-element-template:v3 -->";

    private static bool IsTemplateCurrent(string filePath)
    {
        try
        {
            foreach (var line in File.ReadLines(filePath))
                if (line.Contains(TemplateMarker, StringComparison.Ordinal)) return true;
        }
        catch (IOException) { return false; }
        return false;
    }
}
