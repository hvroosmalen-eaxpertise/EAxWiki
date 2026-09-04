using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;
using EAxWiki.Export.Renderers;

namespace EAxWiki.Export.Exporters;

internal class DiagramExporter(IOutputWriter writer, ILogger logger)
{
    private const int MaxIndexDescriptionLength = 100;
    public async Task ExportPagesAsync(ExportContext ctx, IEaReader reader, CancellationToken ct = default)
    {
        logger.LogInformation("Exporting {DiagramCount} diagrams with PNG images", ctx.AllDiagrams.Count);

        var failures = new List<string>();

        foreach (var (diagram, pkgDir) in ctx.AllDiagrams)
        {
            try
            {
                var diagramsDir = Path.Combine(pkgDir, "diagrams");
                await writer.CreateDirectoryAsync(diagramsDir, ct);

                var fileName = MarkdownHelpers.SanitizeName(diagram.Name);
                var pngPath = Path.Combine(diagramsDir, $"{fileName}.png");
                var mdPath = Path.Combine(diagramsDir, $"{fileName}.md");

                if (!ctx.Force && IsDiagramUpToDate(mdPath, diagram.ModifiedDate) &&
                    IsAiAttributeCurrent(mdPath, ctx.AiConfigured))
                {
                    logger.LogDebug("Skipped diagram {DiagramName}", diagram.Name);
                    continue;
                }

                var sw = Stopwatch.StartNew();

                var pngSuccess = reader.ExportDiagramImage(diagram.Guid, pngPath);
                if (!pngSuccess)
                    logger.LogWarning("Failed to export PNG for diagram {DiagramName}", diagram.Name);

                var derivedText = GetDerivedDescriptionText(diagram, ctx);
                var hasOwnNotes = !string.IsNullOrWhiteSpace(diagram.Notes);
                var seedValue = hasOwnNotes
                    ? FrontmatterParser.NormalizeNotesHtml(diagram.Notes)
                    : FrontmatterParser.NormalizeNotesHtml(derivedText);
                var notesHash = HtmlHelpers.ComputeNotesHash(seedValue);
                var wikiRelPath = Path.GetRelativePath(ctx.OutputPath, mdPath).Replace('\\', '/');
                var wikiRelPathHtml = HtmlHelpers.HtmlEscape(wikiRelPath);

                var lines = new List<string>
                {
                    "---",
                    $"diagram_id: {diagram.Id}",
                    $"notes_hash: {notesHash}",
                    "---",
                    string.Empty,
                    $"# :material-map-outline: {diagram.Name}",
                    string.Empty,
                    string.Empty,
                    MarkdownHelpers.BuildBreadcrumb(diagram.PackageId, diagramsDir, ctx.OutputPath, ctx.PackageLookup,
                        msg => logger.LogWarning("{Message} (diagram '{Name}')", msg, diagram.Name)),
                    string.Empty,
                };

                if (File.Exists(pngPath))
                {
                    lines.Add($"![{diagram.Name}]({fileName}.png)");
                    lines.Add(string.Empty);
                }

                if (ctx.ApiPort > 0)
                {
                    var aiConfigured = ctx.AiConfigured;
                    lines.Add(
                        $"<div id=\"ea-notes-editor\" class=\"ea-notes-editor\"" +
                        $" data-ea-id=\"{diagram.Id}\"" +
                        $" data-kind=\"diagram\"" +
                        $" data-file-path=\"{wikiRelPathHtml}\"" +
                        $" data-api-port=\"{ctx.ApiPort}\"" +
                        $" data-api-token=\"{HtmlHelpers.HtmlEscape(ctx.ApiToken)}\"" +
                        $" data-ai-configured=\"{aiConfigured.ToString().ToLowerInvariant()}\">");
                    lines.Add("<button id=\"ea-notes-edit-btn\" class=\"ea-notes-edit-btn\" type=\"button\" aria-label=\"Edit description\">&#9998;</button>");
                    if (!hasOwnNotes && !string.IsNullOrEmpty(derivedText))
                        lines.Add("<span class=\"ea-notes-derived-hint\">(derived)</span>");
                    lines.Add("<div class=\"ea-notes-content\">");
                    lines.Add("<!--ea-notes-start-->");
                    lines.Add(seedValue);
                    lines.Add("<!--ea-notes-end-->");
                    lines.Add("</div>");
                    lines.Add("</div>");
                }
                else
                {
                    var desc = hasOwnNotes
                        ? $"**Description:** {diagram.Notes}"
                        : derivedText != null ? $"**Derived Description:** {derivedText}" : "**Description:** -";
                    lines.Add(desc);
                }
                lines.Add(string.Empty);

                var diagramElements = new List<(EaElement Element, string PackageDir)>();
                foreach (var dob in diagram.DiagramObjects)
                {
                    if (ctx.ElementLookup.TryGetValue(dob.ElementId, out var e))
                        diagramElements.Add(e);
                }

                if (diagramElements.Count > 0)
                {
                    lines.Add("## Elements");
                    lines.Add(string.Empty);

                    foreach (var (elemEa, elemDir) in diagramElements.OrderBy(e => e.Element.Name))
                    {
                        var elemLink = Path.GetRelativePath(diagramsDir, Path.Combine(elemDir, $"{MarkdownHelpers.SanitizeName(elemEa.Name)}.html")).Replace('\\', '/');
                        lines.Add($"- {MarkdownHelpers.GetStereotypeLabel(elemEa)} [{elemEa.Name}]({elemLink})");
                    }

                    lines.Add(string.Empty);
                }

                await writer.WriteFileAsync(mdPath, string.Join(Environment.NewLine, lines), ct);
                ctx.WrittenMdFiles.Add(mdPath);

                sw.Stop();
                logger.LogInformation("Exported diagram {DiagramName} in {ElapsedMs}ms", diagram.Name, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to export diagram {DiagramName}", diagram.Name);
                failures.Add(diagram.Name);
            }
        }

        if (failures.Count > 0)
            logger.LogWarning("[Export] {FailureCount} diagram(s) failed to export: {DiagramNames}",
                failures.Count, string.Join(", ", failures));
    }

    public async Task WriteIndexAsync(ExportContext ctx, CancellationToken ct = default)
    {
        var diagramsDir = Path.Combine(ctx.OutputPath, "diagrams");
        await writer.CreateDirectoryAsync(diagramsDir, ct);

        var sorted = ctx.AllDiagrams
            .Select(d => (d.Diagram, d.PackageDir, Path: MarkdownHelpers.BuildBreadcrumb(d.Diagram.PackageId, diagramsDir, ctx.OutputPath, ctx.PackageLookup)))
            .OrderBy(d => d.Path)
            .ThenBy(d => d.Diagram.Name)
            .ToList();

        var lines = new List<string> { "# :material-map-outline: Diagrams", string.Empty };

        if (sorted.Count == 0)
        {
            lines.Add("No diagrams found in model.");
        }
        else
        {
            lines.Add("| Diagram | Modified | Description | Path |");
            lines.Add("|---------|----------|-------------|------|");

            foreach (var (diagram, pkgDir, path) in sorted)
            {
                var diagramPage = Path.GetRelativePath(diagramsDir, Path.Combine(pkgDir, "diagrams", $"{MarkdownHelpers.SanitizeName(diagram.Name)}.html")).Replace('\\', '/');
                var link = $":material-map-outline: [{MarkdownHelpers.EscapeCell(diagram.Name)}]({diagramPage})";

                var modified = !string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt)
                    ? dt.ToString("yyyy-MM-dd")
                    : "-";

                var desc = string.IsNullOrWhiteSpace(diagram.Notes) ? "-" : MarkdownHelpers.EscapeCell(diagram.Notes.Trim());
                if (desc.Length > MaxIndexDescriptionLength) desc = desc[..MaxIndexDescriptionLength] + "...";

                lines.Add($"| {link} | {modified} | {desc} | {path} |");
            }
        }

        lines.Add(string.Empty);
        var indexMdPath = Path.Combine(diagramsDir, "index.md");
        await writer.WriteFileAsync(indexMdPath, string.Join(Environment.NewLine, lines), ct);
        ctx.WrittenMdFiles.Add(indexMdPath);
    }

    // Marker that appears in every diagram H1 emitted by the current template.
    // Bump this whenever the diagram-page template changes in a way pre-existing files
    // should be regenerated for, so IsDiagramUpToDate can detect stale template output
    // even when the EA-side ModifiedDate hasn't advanced.
    private const string CurrentH1Marker = "# :material-map-outline: ";

    private static bool IsDiagramUpToDate(string mdPath, string? modifiedDateStr)
    {
        if (string.IsNullOrWhiteSpace(modifiedDateStr)) return false;
        if (!DateTime.TryParse(modifiedDateStr, out var diagramTime)) return false;

        var fileTime = File.GetLastWriteTimeUtc(mdPath);
        if (fileTime <= DateTime.UnixEpoch) return false; // file does not exist

        var diagramTimeUtc = diagramTime.Kind == DateTimeKind.Utc ? diagramTime : diagramTime.ToUniversalTime();
        if (fileTime < diagramTimeUtc) return false;

        // Template-freshness check: if the file was generated before the current H1
        // template, force a rewrite. Reads only the first ~20 lines (the H1 sits near
        // the top of every diagram page).
        try
        {
            foreach (var line in File.ReadLines(mdPath).Take(20))
                if (line.StartsWith(CurrentH1Marker, StringComparison.Ordinal)) return true;
            return false;
        }
        catch (IOException) { return false; }
    }

    // Same as ElementPageWriter.IsAiAttributeCurrent — regenerate the diagram page when the
    // notes-editor widget's data-ai-configured attribute doesn't match the current
    // AiConfigured state, so pages exported before AI was configured pick up the flag flip.
    private static bool IsAiAttributeCurrent(string mdPath, bool aiConfigured)
    {
        var expected = "data-ai-configured=\"" + (aiConfigured ? "true" : "false") + "\"";
        var wrong = "data-ai-configured=\"" + (aiConfigured ? "false" : "true") + "\"";
        try
        {
            foreach (var line in File.ReadLines(mdPath).Take(60))
            {
                if (line.Contains(expected, StringComparison.Ordinal)) return true;
                if (line.Contains(wrong, StringComparison.Ordinal)) return false;
            }
        }
        catch (IOException) { return false; }
        return true;
    }

    /// <summary>
    /// Returns the raw first-sentence text derived from an element on the diagram, or null if none
    /// qualifies. Kept separate from display formatting so it can also seed the notes editor's
    /// textarea without a "**Derived Description:**" label baked into the editable text.
    /// </summary>
    private static string? GetDerivedDescriptionText(EaDiagram diagram, ExportContext ctx)
    {
        foreach (var dob in diagram.DiagramObjects)
        {
            if (ctx.ElementLookup.TryGetValue(dob.ElementId, out var e) &&
                !string.IsNullOrWhiteSpace(e.Element.Notes))
            {
                var firstDot = e.Element.Notes.IndexOf('.');
                var sentence = firstDot >= 0 ? e.Element.Notes[..firstDot].Trim() : e.Element.Notes.Trim();
                if (sentence.Length >= 20)
                    return sentence;
            }
        }
        return null;
    }
}
