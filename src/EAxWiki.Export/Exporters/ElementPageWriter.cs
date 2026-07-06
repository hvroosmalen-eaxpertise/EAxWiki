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
                    if (fileTime >= elementTime)
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

        var graphHtml = RelationshipGraphRenderer.Render(element, dir, ctx);
        if (graphHtml.Length > 0)
            lines.AddRange(["---", string.Empty, "## Relationship Graph", string.Empty, graphHtml, string.Empty]);

        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(filePath, string.Join(Environment.NewLine, lines), ct);
    }
}
