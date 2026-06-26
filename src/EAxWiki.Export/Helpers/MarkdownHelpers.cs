using System.Collections.Concurrent;
using EAxWiki.Core.Models;

namespace EAxWiki.Export.Helpers;

internal static class MarkdownHelpers
{
    private static readonly char[] _invalidChars = Path.GetInvalidFileNameChars().Append('#').ToArray();
    private static volatile ConcurrentDictionary<string, string> _sanitizeCache = new();

    // Replaces the cache atomically so concurrent readers always see a valid dictionary.
    internal static void ClearCache() =>
        Interlocked.Exchange(ref _sanitizeCache!, new ConcurrentDictionary<string, string>());

    internal static string SanitizeName(string name)
    {
        name = name.Trim();
        var cache = _sanitizeCache;
        if (cache.TryGetValue(name, out var cached))
            return cached;
        var sanitized = new string(name.Select(ch => _invalidChars.Contains(ch) ? '_' : ch).ToArray());
        var result = string.IsNullOrWhiteSpace(sanitized) ? "unnamed" : sanitized;
        cache[name] = result;
        return result;
    }

    internal static string EscapeCell(string raw)
        => raw.Replace("|", "\\|").ReplaceLineEndings(" ");

    internal static string FormatTimestamp()
        => $"---\n\n*Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";

    internal static string CreateElementLink(EaElement element, string pkgDir, string fromDir)
    {
        var elemName = SanitizeName(element.Name);
        return Path.GetRelativePath(fromDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
    }

    internal static string BuildBreadcrumb(int packageId, string currentPageDir, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup, Action<string>? onMissingPackage = null)
    {
        var segments = new List<(string Name, int Id)>();
        var id = packageId;
        while (id != 0)
        {
            if (!packageLookup.TryGetValue(id, out var info))
            {
                onMissingPackage?.Invoke($"Package ID {id} not found in lookup — breadcrumb may be incomplete");
                break;
            }
            segments.Add((info.Name, id));
            id = info.ParentId ?? 0;
        }
        segments.Reverse();

        var breadcrumbParts = new List<string>();
        if (segments.Count > 0)
        {
            var homeRelPath = Path.GetRelativePath(currentPageDir, Path.Combine(outputDir, "index.md")).Replace('\\', '/');
            breadcrumbParts.Add($"[Home]({homeRelPath})");
        }
        foreach (var (name, _) in segments)
        {
            var pkgDir = Path.Combine(outputDir, SanitizeName(name));
            var relPath = Path.GetRelativePath(currentPageDir, Path.Combine(pkgDir, "index.md")).Replace('\\', '/');
            breadcrumbParts.Add($"[{name}]({relPath})");
        }
        return string.Join(" / ", breadcrumbParts);
    }

    internal static (string Language, string Type) ParseStereotype(string stereotype)
    {
        if (string.IsNullOrWhiteSpace(stereotype))
            return ("UML", "Uncategorized");

        string language;
        string type;

        var separatorIndex = stereotype.IndexOf("::", StringComparison.Ordinal);
        if (separatorIndex >= 0)
        {
            language = stereotype[..separatorIndex];
            type = stereotype[(separatorIndex + 2)..];
        }
        else
        {
            var underscoreIndex = stereotype.IndexOf('_');
            if (underscoreIndex > 0 && LooksLikeLanguageName(stereotype[..underscoreIndex]))
            {
                language = stereotype[..underscoreIndex];
                type = stereotype[(underscoreIndex + 1)..];
            }
            else
            {
                language = "UML";
                type = stereotype;
            }
        }

        type = StripLanguagePrefix(type, language);
        return (language, type);
    }

    private static bool LooksLikeLanguageName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (!char.IsUpper(name[0])) return false;
        for (var i = 1; i < name.Length; i++)
            if (!char.IsLetterOrDigit(name[i])) return false;
        return name.Any(char.IsLetter);
    }

    private static string StripLanguagePrefix(string type, string language)
    {
        if (type.StartsWith(language + "_", StringComparison.OrdinalIgnoreCase))
            return type[(language.Length + 1)..];

        var baseName = new string(language.TakeWhile(c => !char.IsDigit(c)).ToArray());
        if (!string.IsNullOrEmpty(baseName) && type.StartsWith(baseName + "_", StringComparison.OrdinalIgnoreCase))
            return type[(baseName.Length + 1)..];

        return type;
    }
}
