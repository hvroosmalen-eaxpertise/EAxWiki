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
    /// Scans <paramref name="wikiPath"/> for element and diagram pages whose frontmatter status or
    /// notes differ from their stored hash, then writes the changed values back to EA via COM.
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

            if (fm.TryGetValue("ea_id", out var elemIdStr) && int.TryParse(elemIdStr, out var elementId))
            {
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

                TryWriteBackNotes(fm, file, elementId, reader.UpdateElementNotes, notesChanges, "element");
            }
            else if (fm.TryGetValue("diagram_id", out var diagIdStr) && int.TryParse(diagIdStr, out var diagramId))
            {
                TryWriteBackNotes(fm, file, diagramId, reader.UpdateDiagramNotes, notesChanges, "diagram");
            }
        }

        return new ScanResult(statusChanges, notesChanges);
    }

    private void TryWriteBackNotes(
        Dictionary<string, string> fm, string file, int id,
        Action<int, string> updateInEa, List<NotesChangeResult> notesChanges, string kindLabel)
    {
        if (!fm.TryGetValue("notes_hash", out var storedNotesHash)) return;

        var currentNotes = FrontmatterParser.ExtractNotesContent(file);
        if (currentNotes == null) return;

        var expectedNotesHash = ElementPageWriter.ComputeNotesHash(currentNotes);
        if (string.Equals(expectedNotesHash, storedNotesHash, StringComparison.OrdinalIgnoreCase)) return;

        try
        {
            var normalized = FrontmatterParser.NormalizeNotesHtml(currentNotes);
            updateInEa(id, normalized);
            FrontmatterParser.UpdateNotes(file, normalized);
            notesChanges.Add(new NotesChangeResult(id, file));
            logger.LogInformation("Write-back: {Kind} {Id} notes updated ({File})", kindLabel, id, Path.GetFileName(file));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Notes write-back failed for {Kind} {Id} in {File}", kindLabel, id, file);
        }
    }
}
