using System.Linq;
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

    private static readonly Regex NotesMarkerPattern = new(@"<!--\s*ea-notes-(start|end)\s*-->", RegexOptions.Compiled);

    /// <summary>
    /// Strips the ea-notes-start/end markers (the widget's client-side innerHTML capture includes
    /// them as literal comment text, since they sit inside .ea-notes-content) and, if what remains
    /// contains no HTML tags at all, wraps blank-line-separated blocks in &lt;p&gt; so multiple
    /// paragraphs survive Markdown's block-HTML passthrough. Notes that already contain markup
    /// (beyond the stripped markers) are left untouched.
    /// </summary>
    public static string NormalizeNotesHtml(string? notes)
    {
        var cleaned = NotesMarkerPattern.Replace(notes ?? string.Empty, string.Empty).Trim();
        if (cleaned.Length == 0 || cleaned.Contains('<'))
            return cleaned;

        var paragraphs = Regex.Split(cleaned.Replace("\r\n", "\n"), @"\n\s*\n")
            .Where(p => !string.IsNullOrWhiteSpace(p));
        return string.Join("\n", paragraphs.Select(p => $"<p>{p.Trim()}</p>"));
    }

    /// <summary>
    /// Extracts the current notes body from between the ea-notes-start/end markers, or null if
    /// the page has no notes widget (exported without --api-port, or the element has no notes).
    /// </summary>
    public static string? ExtractNotesContent(string filePath)
    {
        string text;
        try { text = File.ReadAllText(filePath).Replace("\r\n", "\n"); }
        catch { return null; }

        var match = Regex.Match(text, @"<!--ea-notes-start-->\n(.*?)\n<!--ea-notes-end-->", RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// Updates the notes block (between the ea-notes-start/end markers) and the notes_hash
    /// frontmatter field. Uses an atomic temp-file swap, same as UpdateStatus.
    /// </summary>
    public static void UpdateNotes(string filePath, string newNotesHtml)
    {
        var original = File.ReadAllText(filePath);
        var usesCrlf = original.Contains("\r\n");
        var text = original.Replace("\r\n", "\n");

        var fmMatch = Regex.Match(text, @"\A---\n(.*?\n)---\n", RegexOptions.Singleline);
        if (!fmMatch.Success) return;

        var newHash = ElementPageWriter.ComputeNotesHash(newNotesHtml);
        var fmBody = fmMatch.Groups[1].Value;
        fmBody = Regex.IsMatch(fmBody, @"^notes_hash:.*$", RegexOptions.Multiline)
            ? Regex.Replace(fmBody, @"^notes_hash:.*$", $"notes_hash: {newHash}", RegexOptions.Multiline)
            : fmBody + $"notes_hash: {newHash}\n";

        text = $"---\n{fmBody}---\n" + text[fmMatch.Length..];

        var contentPattern = new Regex(@"(<!--ea-notes-start-->\n).*?(\n<!--ea-notes-end-->)", RegexOptions.Singleline);
        text = contentPattern.Replace(text, m => m.Groups[1].Value + newNotesHtml + m.Groups[2].Value, 1);

        if (usesCrlf) text = text.Replace("\n", "\r\n");

        var tmp = filePath + ".tmp";
        File.WriteAllText(tmp, text);
        File.Move(tmp, filePath, overwrite: true);
    }
}
