using EAxWiki.Export.Exporters;

namespace EAxWiki.Export.Helpers;

internal static class FrontmatterParser
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
    /// Updates a single frontmatter key in a Markdown file in place.
    /// Also updates ea_hash to the new hash of the written value.
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

        File.WriteAllLines(filePath, lines);
    }
}
