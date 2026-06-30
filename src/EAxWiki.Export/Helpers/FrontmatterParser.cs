using System.Text.RegularExpressions;
using EAxWiki.Export.Exporters;

namespace EAxWiki.Export.Helpers;

public static class FrontmatterParser
{
    /// <summary>
    /// Parses the YAML frontmatter block (between the first two --- delimiters) of a Markdown file.
    /// Returns a dictionary of key → raw string value, or an empty dictionary if no frontmatter is present.
    /// </summary>
    public static Dictionary<string, string> Parse(string filePath)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string[] lines;
        try { lines = File.ReadAllLines(filePath); }
        catch { return result; }

        if (lines.Length < 2 || lines[0].Trim() != "---")
            return result;

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Trim() == "---")
                break;

            var sep = lines[i].IndexOf(':');
            if (sep < 1) continue;

            var key = lines[i][..sep].Trim();
            var value = lines[i][(sep + 1)..].Trim();
            result[key] = value;
        }

        return result;
    }

    /// <summary>
    /// Updates status in a Markdown file: frontmatter fields, status badge, and widget div.
    /// Uses an atomic temp-file swap so MkDocs never reads a partially written file.
    /// </summary>
    public static void UpdateStatus(string filePath, string newStatus)
    {
        var lines = File.ReadAllLines(filePath).ToList();
        if (lines.Count < 2 || lines[0].Trim() != "---") return;

        int end = -1;
        for (int i = 1; i < lines.Count; i++)
        {
            if (lines[i].Trim() == "---") { end = i; break; }
        }
        if (end < 0) return;

        // 1. Update frontmatter
        var newHash = ElementPageWriter.ComputeStatusHash(newStatus);
        for (int i = 1; i < end; i++)
        {
            var sep = lines[i].IndexOf(':');
            if (sep < 1) continue;
            var key = lines[i][..sep].Trim();
            if (key.Equals("status", StringComparison.OrdinalIgnoreCase))
                lines[i] = $"status: {newStatus}";
            else if (key.Equals("ea_hash", StringComparison.OrdinalIgnoreCase))
                lines[i] = $"ea_hash: {newHash}";
        }

        // 2. Update page body: status badge and widget data-status attribute
        var badgePattern  = new Regex(@"class=""status-badge status-[^""]*"">[^<]*</span>");
        var widgetPattern = new Regex(@"data-status=""[^""]*""");
        var newClass = $"status-{newStatus.ToLowerInvariant()}";

        for (int i = end + 1; i < lines.Count; i++)
        {
            if (lines[i].Contains("status-badge"))
                lines[i] = badgePattern.Replace(lines[i],
                    $"class=\"status-badge {newClass}\">{newStatus}</span>");

            if (lines[i].Contains("id=\"ea-status-editor\""))
                lines[i] = widgetPattern.Replace(lines[i],
                    $"data-status=\"{newStatus}\"");
        }

        // 3. Atomic write — swap via temp file so MkDocs never sees a partial file
        var tmp = filePath + ".tmp";
        File.WriteAllLines(tmp, lines);
        File.Move(tmp, filePath, overwrite: true);
    }
}
