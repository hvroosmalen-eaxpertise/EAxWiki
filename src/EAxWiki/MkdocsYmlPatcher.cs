using System.Text;

namespace EAxWiki;

/// <summary>
/// Minimal, line-oriented edit of mkdocs.yml's <c>theme:</c> block to set or remove the
/// <c>logo:</c> line — enough for the brand editor's logo upload (issue #98) without pulling in a
/// full YAML parser (which YamlDotNet's default emitter would rewrite the whole file, losing
/// comments and formatting that users edit by hand).
///
/// The <c>logo:</c> line is a child of <c>theme:</c>; we locate the <c>theme:</c> block by its
/// zero-indent header line, then within its indented body scan for an existing <c>logo:</c> line.
/// Comment lines (starting with <c>#</c>) are ignored so the seeded template's comment above the
/// logo slot doesn't confuse the parser.
/// </summary>
internal static class MkdocsYmlPatcher
{
    public static void SetLogo(string mkdocsYmlPath, string logoRelPath)
    {
        if (string.IsNullOrWhiteSpace(logoRelPath))
            throw new ArgumentException("logo path is required", nameof(logoRelPath));

        var lines = File.ReadAllLines(mkdocsYmlPath).ToList();
        var (themeStart, themeEnd) = FindThemeBlock(lines);
        if (themeStart < 0) throw new InvalidOperationException("theme: block not found in mkdocs.yml");

        var indent = GetChildIndent(lines, themeStart, themeEnd);
        var newLine = $"{indent}logo: {logoRelPath}";

        var existing = FindLogoLine(lines, themeStart, themeEnd);
        if (existing >= 0)
            lines[existing] = newLine;
        else
            lines.Insert(themeStart + 1, newLine);

        File.WriteAllLines(mkdocsYmlPath, lines);
    }

    public static void RemoveLogo(string mkdocsYmlPath)
    {
        var lines = File.ReadAllLines(mkdocsYmlPath).ToList();
        var (themeStart, themeEnd) = FindThemeBlock(lines);
        if (themeStart < 0) return;

        var existing = FindLogoLine(lines, themeStart, themeEnd);
        if (existing >= 0)
        {
            lines.RemoveAt(existing);
            File.WriteAllLines(mkdocsYmlPath, lines);
        }
    }

    public static string? GetLogo(string mkdocsYmlPath)
    {
        if (!File.Exists(mkdocsYmlPath)) return null;
        var lines = File.ReadAllLines(mkdocsYmlPath);
        var (themeStart, themeEnd) = FindThemeBlock(lines);
        if (themeStart < 0) return null;

        var existing = FindLogoLine(lines, themeStart, themeEnd);
        if (existing < 0) return null;

        var line = lines[existing];
        var colon = line.IndexOf(':');
        return colon < 0 ? null : line[(colon + 1)..].Trim();
    }

    private static (int Start, int End) FindThemeBlock(IReadOnlyList<string> lines)
    {
        int start = -1;
        for (int i = 0; i < lines.Count; i++)
        {
            var raw = lines[i];
            if (!raw.StartsWith("theme:")) continue;
            // Zero-indent, top-level key
            if (raw.Length == 6 || char.IsWhiteSpace(raw[6]) || raw[6] == '#')
            {
                start = i;
                break;
            }
        }
        if (start < 0) return (-1, -1);

        // End of the block: next zero-indent, non-comment, non-blank line
        int end = lines.Count;
        for (int i = start + 1; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Length == 0) continue;
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith('#')) continue;
            if (line[0] != ' ' && line[0] != '\t')
            {
                end = i;
                break;
            }
        }
        return (start, end);
    }

    private static string GetChildIndent(IReadOnlyList<string> lines, int themeStart, int themeEnd)
    {
        for (int i = themeStart + 1; i < themeEnd; i++)
        {
            var line = lines[i];
            if (line.Length == 0) continue;
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith('#')) continue;
            var indentLen = line.Length - trimmed.Length;
            if (indentLen > 0) return line[..indentLen];
        }
        return "  ";
    }

    private static int FindLogoLine(IReadOnlyList<string> lines, int themeStart, int themeEnd)
    {
        for (int i = themeStart + 1; i < themeEnd; i++)
        {
            var trimmed = lines[i].TrimStart();
            if (trimmed.StartsWith('#')) continue;
            if (trimmed.StartsWith("logo:")) return i;
        }
        return -1;
    }
}
