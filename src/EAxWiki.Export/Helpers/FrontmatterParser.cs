using System.Text.RegularExpressions;
using EAxWiki.Export.Exporters;
using EAxWiki.Export.Renderers;
using Ganss.Xss;
using YamlDotNet.Serialization;

namespace EAxWiki.Export.Helpers;

public static class FrontmatterParser
{
    // Shared instance per the library's own usage docs — Sanitize() does not mutate shared state,
    // so one instance safely serves concurrent calls from the parallel export pipeline.
    private static readonly HtmlSanitizer NotesSanitizer = new();

    // Shared deserializer per the library's thread-safety docs — Deserialize() does not mutate
    // shared state, so one instance safely serves concurrent calls from the parallel export pipeline.
    private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder().Build();

    /// <summary>
    /// Parses the YAML frontmatter block (between the first two --- delimiters) of a Markdown file.
    /// Returns a dictionary of key → raw string value, or an empty dictionary if no frontmatter is present.
    /// </summary>
    public static Dictionary<string, string> Parse(string filePath)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string text;
        try { text = File.ReadAllText(filePath); }
        catch { return result; }

        var match = Regex.Match(text, @"\A---\s*\n(.*?\n)---", RegexOptions.Singleline);
        if (!match.Success) return result;

        var yamlBlock = match.Groups[1].Value;

        try
        {
            var yamlObject = YamlDeserializer.Deserialize<Dictionary<object, object>>(yamlBlock);
            if (yamlObject == null) return result;

            foreach (var kvp in yamlObject)
            {
                var key = kvp.Key?.ToString() ?? "";
                result[key] = kvp.Value switch
                {
                    string s => s,
                    List<object> list => "[" + string.Join(", ", list.Select(o => o?.ToString() ?? "")) + "]",
                    int i => i.ToString(),
                    long l => l.ToString(),
                    null => "",
                    _ => kvp.Value.ToString() ?? ""
                };
            }
        }
        catch
        {
            // YAML parse error — caller treats missing keys gracefully
        }

        return result;
    }

    /// <summary>
    /// Updates status in a Markdown file: frontmatter fields, status badge, and widget div.
    /// Uses an atomic temp-file swap so MkDocs never reads a partially written file.
    /// </summary>
    public static void UpdateStatus(string filePath, string newStatus) =>
        RewriteMarkdown(filePath, text =>
        {
            var lines = text.Split('\n').ToList();
            if (lines.Count < 2 || lines[0].Trim() != "---") return null;

            int end = -1;
            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i].Trim() == "---") { end = i; break; }
            }
            if (end < 0) return null;

            // 1. Update frontmatter
            var newHash = HtmlHelpers.ComputeStatusHash(newStatus);
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
            var newClass = $"status-{newStatus.ToLowerInvariant()}";

            for (int i = end + 1; i < lines.Count; i++)
            {
                if (lines[i].Contains("status-badge"))
                    lines[i] = StatusBadgePattern.Replace(lines[i],
                        $"class=\"status-badge {newClass}\">{newStatus}</span>");

                if (lines[i].Contains("id=\"ea-status-editor\""))
                    lines[i] = StatusWidgetPattern.Replace(lines[i],
                        $"data-status=\"{newStatus}\"");

                if (lines[i].Contains("**Modified:**"))
                    lines[i] = PatchModifiedDate(lines[i]);
            }

            return string.Join("\n", lines);
        });

    private static readonly Regex StatusBadgePattern = new(@"class=""status-badge status-[^""]*"">[^<]*</span>", RegexOptions.Compiled);
    private static readonly Regex StatusWidgetPattern = new(@"data-status=""[^""]*""", RegexOptions.Compiled);

    private static readonly Regex ModifiedDatePattern = new(@"(\*\*Modified:\*\*\s*)\d{4}-\d{2}-\d{2}", RegexOptions.Compiled);

    /// <summary>
    /// Bumps the "**Modified:**" date to today. EA stamps element.Modified on every COM Update()
    /// call, but the exporter's incremental skip check compares the .md file's own write-time
    /// against that EA-side date — since this write-back patch happens right after the COM call,
    /// the file's write-time would otherwise permanently exceed EA's date, causing the page to be
    /// skipped forever on future exports instead of just until the next run.
    /// </summary>
    private static string PatchModifiedDate(string line) =>
        ModifiedDatePattern.Replace(line, $"${{1}}{DateTime.Now:yyyy-MM-dd}");

    private static readonly Regex NotesMarkerPattern = new(@"<!--\s*ea-notes-(start|end)\s*-->", RegexOptions.Compiled);

    /// <summary>
    /// Strips the ea-notes-start/end markers (the widget's client-side innerHTML capture includes
    /// them as literal comment text, since they sit inside .ea-notes-content) and, if what remains
    /// contains no HTML tags at all, wraps blank-line-separated blocks in &lt;p&gt; so multiple
    /// paragraphs survive Markdown's block-HTML passthrough. Notes that already contain markup
    /// (beyond the stripped markers) are run through an allowlist HTML sanitizer (HtmlSanitizer /
    /// AngleSharp) — this text is user-typed, gets persisted into the live EA repository via COM,
    /// and is embedded as raw HTML into the generated wiki page, so unsanitized markup would be a
    /// stored-XSS vector. The sanitizer keeps ordinary rich-text markup (p, lists, bold, links, ...)
    /// and strips script/style/iframe/object/embed/form tags, event-handler attributes, and
    /// javascript:/data: URLs.
    /// </summary>
    public static string NormalizeNotesHtml(string? notes)
    {
        var cleaned = NotesMarkerPattern.Replace(notes ?? string.Empty, string.Empty).Trim();
        if (cleaned.Length == 0)
            return cleaned;

        if (!cleaned.Contains('<'))
        {
            var paragraphs = Regex.Split(cleaned.Replace("\r\n", "\n"), @"\n\s*\n")
                .Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join("\n", paragraphs.Select(p => $"<p>{p.Trim()}</p>"));
        }

        return NotesSanitizer.Sanitize(cleaned);
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
    public static void UpdateNotes(string filePath, string newNotesHtml) =>
        UpdateNotesLike(filePath, newNotesHtml, "<!--ea-notes-start-->", "<!--ea-notes-end-->");

    /// <summary>
    /// Extracts the current notes body for one row-level editor (attribute/method/tagged-value
    /// description), identified by <paramref name="rowId"/>, or null if no such marker pair exists.
    /// Unlike element/diagram notes, a single page can contain many independent row editors, so
    /// each marker pair is tagged with its own id rather than assuming there's only one per file.
    /// </summary>
    public static string? ExtractRowNotesContent(string filePath, string rowId)
    {
        string text;
        try { text = File.ReadAllText(filePath).Replace("\r\n", "\n"); }
        catch { return null; }

        var match = Regex.Match(text,
            $@"<!--ea-row-notes-start:{Regex.Escape(rowId)}-->(.*?)<!--ea-row-notes-end:{Regex.Escape(rowId)}-->",
            RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// Updates one row-level notes block (attribute/method/tagged-value description) identified by
    /// <paramref name="rowId"/>: the content between its ea-row-notes-start/end markers, the
    /// data-notes-hash attribute on the row declaring that same id, and the page's Modified date.
    /// Requires the emitted markup to place data-row-id before data-notes-hash on the same tag.
    /// </summary>
    public static void UpdateRowNotes(string filePath, string rowId, string newNotesHtml) =>
        RewriteMarkdown(filePath, text =>
        {
            var newHash = HtmlHelpers.ComputeNotesHash(newNotesHtml);

            var hashPattern = new Regex($"(data-row-id=\"{Regex.Escape(rowId)}\"[^>]*?data-notes-hash=\")[^\"]*(\")");
            text = hashPattern.Replace(text, $"${{1}}{newHash}$2", 1);

            var contentPattern = new Regex(
                $@"(<!--ea-row-notes-start:{Regex.Escape(rowId)}-->).*?(<!--ea-row-notes-end:{Regex.Escape(rowId)}-->)",
                RegexOptions.Singleline);
            text = contentPattern.Replace(text, m => m.Groups[1].Value + newNotesHtml + m.Groups[2].Value, 1);

            return ModifiedDatePattern.Replace(text, $"${{1}}{DateTime.Now:yyyy-MM-dd}");
        });

    /// <summary>
    /// Extracts the current notes body from between the ea-package-notes-start/end markers,
    /// or null if the page has no package notes widget.
    /// </summary>
    public static string? ExtractPackageNotesContent(string filePath)
    {
        string text;
        try { text = File.ReadAllText(filePath).Replace("\r\n", "\n"); }
        catch { return null; }

        var match = Regex.Match(text, @"<!--ea-package-notes-start-->\n?(.*?)\n?<!--ea-package-notes-end-->", RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// Updates the package notes block (between the ea-package-notes-start/end markers) and the
    /// notes_hash frontmatter field. Uses an atomic temp-file swap, same as <see cref="UpdateNotes"/>.
    /// </summary>
    public static void UpdatePackageNotes(string filePath, string newNotesHtml) =>
        UpdateNotesLike(filePath, newNotesHtml, "<!--ea-package-notes-start-->", "<!--ea-package-notes-end-->");

    /// <summary>
    /// Shared rewrite tail for every Update* method: read the file, normalize to \n, apply the transform,
    /// restore the file's original EOL, and atomically swap via a temp file so MkDocs never sees a partial
    /// file. A transform returning null means "nothing to change" and leaves the file untouched (preserves
    /// the pre-refactor early-return behavior).
    /// </summary>
    private static void RewriteMarkdown(string filePath, Func<string, string?> transform)
    {
        var original = File.ReadAllText(filePath);
        var usesCrlf = original.Contains("\r\n");
        var text = transform(original.Replace("\r\n", "\n"));
        if (text is null) return;
        if (usesCrlf) text = text.Replace("\n", "\r\n");
        var tmp = filePath + ".tmp";
        File.WriteAllText(tmp, text);
        File.Move(tmp, filePath, overwrite: true);
    }

    /// <summary>
    /// Shared implementation of UpdateNotes/UpdatePackageNotes: swap the notes_hash frontmatter field
    /// (appending it if absent), replace the content between the start/end markers, and bump the page's
    /// Modified date. The two public methods differ only in the marker names.
    /// </summary>
    private static void UpdateNotesLike(string filePath, string newNotesHtml, string startMarker, string endMarker)
    {
        RewriteMarkdown(filePath, text =>
        {
            var fmMatch = Regex.Match(text, @"\A---\n(.*?\n)---\n", RegexOptions.Singleline);
            if (!fmMatch.Success) return null;

            var newHash = HtmlHelpers.ComputeNotesHash(newNotesHtml);
            var fmBody = fmMatch.Groups[1].Value;
            fmBody = Regex.IsMatch(fmBody, @"^notes_hash:.*$", RegexOptions.Multiline)
                ? Regex.Replace(fmBody, @"^notes_hash:.*$", $"notes_hash: {newHash}", RegexOptions.Multiline)
                : fmBody + $"notes_hash: {newHash}\n";

            text = $"---\n{fmBody}---\n" + text[fmMatch.Length..];

            var contentPattern = new Regex(
                $@"({Regex.Escape(startMarker)}\n).*?(\n{Regex.Escape(endMarker)})", RegexOptions.Singleline);
            text = contentPattern.Replace(text, m => m.Groups[1].Value + newNotesHtml + m.Groups[2].Value, 1);

            return ModifiedDatePattern.Replace(text, $"${{1}}{DateTime.Now:yyyy-MM-dd}");
        });
    }
}
