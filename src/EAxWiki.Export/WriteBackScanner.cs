using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Exporters;
using EAxWiki.Export.Helpers;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Export;

public class WriteBackScanner(IEaReader reader, ILogger logger)
{
    public record ChangeResult(int ElementId, string OldStatus, string NewStatus, string FilePath);

    /// <summary>
    /// Scans <paramref name="wikiPath"/> for element pages whose frontmatter status differs from
    /// the ea_hash, then writes the changed status back to EA via COM.
    /// </summary>
    public List<ChangeResult> Scan(string wikiPath)
    {
        var allowedStatuses = new HashSet<string>(
            reader.GetStatusTypes(), StringComparer.OrdinalIgnoreCase);

        var applied = new List<ChangeResult>();
        var conflicts = new List<string>();

        foreach (var file in Directory.EnumerateFiles(wikiPath, "*.md", SearchOption.AllDirectories))
        {
            var fm = FrontmatterParser.Parse(file);
            if (!fm.TryGetValue("ea_id", out var idStr) ||
                !int.TryParse(idStr, out var elementId))
                continue;

            if (!fm.TryGetValue("status", out var currentStatus) ||
                !fm.TryGetValue("ea_hash", out var storedHash))
                continue;

            var expectedHash = ElementPageWriter.ComputeStatusHash(currentStatus);
            if (string.Equals(expectedHash, storedHash, StringComparison.OrdinalIgnoreCase))
                continue;  // status matches hash → exporter wrote this, no human change

            if (!allowedStatuses.Contains(currentStatus))
            {
                logger.LogWarning("Skipping {File}: '{Status}' is not a valid status value", file, currentStatus);
                continue;
            }

            // Derive the old status from the stored hash — we can't reverse it, so just log "unknown"
            // and use it only for reporting.
            var oldStatus = fm.TryGetValue("ea_hash", out _) ? "(previous)" : string.Empty;

            try
            {
                reader.UpdateElementStatus(elementId, currentStatus);
                FrontmatterParser.UpdateStatus(file, currentStatus);
                applied.Add(new ChangeResult(elementId, oldStatus, currentStatus, file));
                logger.LogInformation("Write-back: element {Id} status set to '{Status}' ({File})",
                    elementId, currentStatus, Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Write-back failed for element {Id} in {File}", elementId, file);
            }
        }

        return applied;
    }
}
