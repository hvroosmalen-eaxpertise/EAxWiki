using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Exporters;
using EAxWiki.Export.Helpers;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Export;

public class WriteBackScanner(IEaReader reader, ILogger logger)
{
    public record ChangeResult(int ElementId, string OldStatus, string NewStatus, string FilePath);
    public record NotesChangeResult(int ElementId, string FilePath);
    public record ScanResult(List<ChangeResult> StatusChanges, List<NotesChangeResult> NotesChanges);

    /// <summary>
    /// Scans <paramref name="wikiPath"/> for element pages whose frontmatter status or notes
    /// differ from their stored hash, then writes the changed values back to EA via COM.
    /// </summary>
    public ScanResult Scan(string wikiPath)
    {
        var allowedStatuses = new HashSet<string>(
            reader.GetStatusTypes(), StringComparer.OrdinalIgnoreCase);

        var statusChanges = new List<ChangeResult>();
        var notesChanges = new List<NotesChangeResult>();

        foreach (var file in Directory.EnumerateFiles(wikiPath, "*.md", SearchOption.AllDirectories))
        {
            var fm = FrontmatterParser.Parse(file);
            if (!fm.TryGetValue("ea_id", out var idStr) ||
                !int.TryParse(idStr, out var elementId))
                continue;

            if (fm.TryGetValue("status", out var currentStatus) &&
                fm.TryGetValue("ea_hash", out var storedStatusHash))
            {
                var expectedStatusHash = ElementPageWriter.ComputeStatusHash(currentStatus);
                if (!string.Equals(expectedStatusHash, storedStatusHash, StringComparison.OrdinalIgnoreCase))
                {
                    if (!allowedStatuses.Contains(currentStatus))
                    {
                        logger.LogWarning("Skipping {File}: '{Status}' is not a valid status value", file, currentStatus);
                    }
                    else
                    {
                        try
                        {
                            reader.UpdateElementStatus(elementId, currentStatus);
                            FrontmatterParser.UpdateStatus(file, currentStatus);
                            statusChanges.Add(new ChangeResult(elementId, "(previous)", currentStatus, file));
                            logger.LogInformation("Write-back: element {Id} status set to '{Status}' ({File})",
                                elementId, currentStatus, Path.GetFileName(file));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Status write-back failed for element {Id} in {File}", elementId, file);
                        }
                    }
                }
            }

            if (fm.TryGetValue("notes_hash", out var storedNotesHash))
            {
                var currentNotes = FrontmatterParser.ExtractNotesContent(file);
                if (currentNotes != null)
                {
                    var expectedNotesHash = ElementPageWriter.ComputeNotesHash(currentNotes);
                    if (!string.Equals(expectedNotesHash, storedNotesHash, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var normalized = FrontmatterParser.NormalizeNotesHtml(currentNotes);
                            reader.UpdateElementNotes(elementId, normalized);
                            FrontmatterParser.UpdateNotes(file, normalized);
                            notesChanges.Add(new NotesChangeResult(elementId, file));
                            logger.LogInformation("Write-back: element {Id} notes updated ({File})",
                                elementId, Path.GetFileName(file));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Notes write-back failed for element {Id} in {File}", elementId, file);
                        }
                    }
                }
            }
        }

        return new ScanResult(statusChanges, notesChanges);
    }
}
