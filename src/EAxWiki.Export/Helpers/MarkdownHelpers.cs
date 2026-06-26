using System.Collections.Concurrent;
using EAxWiki.Core.Models;

namespace EAxWiki.Export.Helpers;

internal static class MarkdownHelpers
{
    private static readonly char[] _invalidChars = Path.GetInvalidFileNameChars().Append('#').ToArray();
    private static volatile ConcurrentDictionary<string, string> _sanitizeCache = new();

    // ArchiMate stereotype → (short code, layer) mapping.
    private static readonly Dictionary<string, (string Code, string Layer)> ArchiMateMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ArchiMate_BusinessActor"] = ("BA", "business"),
        ["ArchiMate_BusinessRole"] = ("BR", "business"),
        ["ArchiMate_BusinessCollaboration"] = ("BC", "business"),
        ["ArchiMate_BusinessInterface"] = ("BI", "business"),
        ["ArchiMate_BusinessProcess"] = ("BP", "business"),
        ["ArchiMate_BusinessFunction"] = ("BF", "business"),
        ["ArchiMate_BusinessInteraction"] = ("BIN", "business"),
        ["ArchiMate_BusinessEvent"] = ("BE", "business"),
        ["ArchiMate_BusinessService"] = ("BS", "business"),
        ["ArchiMate_BusinessObject"] = ("BO", "business"),
        ["ArchiMate_Contract"] = ("CTR", "business"),
        ["ArchiMate_Product"] = ("PRD", "business"),
        ["ArchiMate_Representation"] = ("REP", "application"),
        ["ArchiMate_Meaning"] = ("MNG", "business"),
        ["ArchiMate_Value"] = ("VAL", "business"),
        ["ArchiMate_ApplicationComponent"] = ("AC", "application"),
        ["ArchiMate_ApplicationCollaboration"] = ("APC", "application"),
        ["ArchiMate_ApplicationInterface"] = ("AI", "application"),
        ["ArchiMate_ApplicationFunction"] = ("AF", "application"),
        ["ArchiMate_ApplicationInteraction"] = ("APIN", "application"),
        ["ArchiMate_ApplicationProcess"] = ("APP", "application"),
        ["ArchiMate_ApplicationEvent"] = ("AE", "application"),
        ["ArchiMate_ApplicationService"] = ("AS", "application"),
        ["ArchiMate_DataObject"] = ("DO", "application"),
        ["ArchiMate_Node"] = ("ND", "technology"),
        ["ArchiMate_Device"] = ("DV", "technology"),
        ["ArchiMate_SystemSoftware"] = ("SS", "technology"),
        ["ArchiMate_TechnologyCollaboration"] = ("TC", "technology"),
        ["ArchiMate_TechnologyInterface"] = ("TI", "technology"),
        ["ArchiMate_TechnologyProcess"] = ("TP", "technology"),
        ["ArchiMate_TechnologyFunction"] = ("TF", "technology"),
        ["ArchiMate_TechnologyInteraction"] = ("TIN", "technology"),
        ["ArchiMate_TechnologyEvent"] = ("TE", "technology"),
        ["ArchiMate_TechnologyService"] = ("TS", "technology"),
        ["ArchiMate_Artifact"] = ("ART", "technology"),
        ["ArchiMate_CommunicationNetwork"] = ("CNW", "technology"),
        ["ArchiMate_Path"] = ("PTH", "technology"),
        ["ArchiMate_InfrastructureInterface"] = ("II", "technology"),
        ["ArchiMate_Network"] = ("NW", "technology"),
        ["ArchiMate_Equipment"] = ("EQ", "physical"),
        ["ArchiMate_Facility"] = ("FC", "physical"),
        ["ArchiMate_Material"] = ("MT", "physical"),
        ["ArchiMate_DistributionNetwork"] = ("DN", "physical"),
        ["ArchiMate_Stakeholder"] = ("SH", "motivation"),
        ["ArchiMate_Driver"] = ("DR", "motivation"),
        ["ArchiMate_Assessment"] = ("AS", "motivation"),
        ["ArchiMate_Goal"] = ("GL", "motivation"),
        ["ArchiMate_Outcome"] = ("OC", "motivation"),
        ["ArchiMate_Principle"] = ("PR", "motivation"),
        ["ArchiMate_Requirement"] = ("RQ", "motivation"),
        ["ArchiMate_Constraint"] = ("CN", "motivation"),
        ["ArchiMate_Resource"] = ("RS", "strategy"),
        ["ArchiMate_Capability"] = ("CAP", "strategy"),
        ["ArchiMate_CourseOfAction"] = ("COA", "strategy"),
        ["ArchiMate_ValueStream"] = ("VS", "strategy"),
        ["ArchiMate_WorkPackage"] = ("WP", "implementation"),
        ["ArchiMate_Deliverable"] = ("DLV", "implementation"),
        ["ArchiMate_Plateau"] = ("PL", "implementation"),
        ["ArchiMate_Gap"] = ("GP", "implementation"),
        ["ArchiMate_Location"] = ("LC", "composite"),
        ["ArchiMate_Grouping"] = ("GR", "composite"),
        ["ArchiMate_Junction"] = ("JN", "composite"),
        ["ArchiMate_AndJunction"] = ("AND", "composite"),
        ["ArchiMate_OrJunction"] = ("OR", "composite"),
    };

    // Reverse lookup: bare type name → (code, layer), e.g. "BusinessActor" → ("BA", "business")
    private static readonly Dictionary<string, (string Code, string Layer)> ArchiMateTypeMap =
        ArchiMateMap.ToDictionary(kvp => kvp.Key.Split('_', 2)[1], kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

    // EDGY element types → (full name, layer) mapping.
    private static readonly Dictionary<string, (string Code, string Layer)> EdgyMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Identity facet (green)
        ["Purpose"] = ("Purpose", "edgy-id"),
        ["Story"] = ("Story", "edgy-id"),
        ["Content"] = ("Content", "edgy-id"),
        // Architecture facet (blue)
        ["Capability"] = ("Capability", "edgy-ar"),
        ["Process"] = ("Process", "edgy-ar"),
        ["Asset"] = ("Asset", "edgy-ar"),
        // Experience facet (pink)
        ["Task"] = ("Task", "edgy-ex"),
        ["Journey"] = ("Journey", "edgy-ex"),
        ["Channel"] = ("Channel", "edgy-ex"),
        // Intersection elements
        ["Organisation"] = ("Organisation", "edgy-ix"),
        ["Product"] = ("Product", "edgy-ix"),
        ["Brand"] = ("Brand", "edgy-ix"),
        // Base element
        ["People"] = ("People", "edgy-pe"),
        // Labels / utilities
        ["Metric"] = ("Metric", "edgy-lb"),
        ["Tag"] = ("Tag", "edgy-lb"),
        ["Object"] = ("Object", "edgy-lb"),
        ["Outcome"] = ("Outcome", "edgy-lb"),
        ["Activity"] = ("Activity", "edgy-lb"),
    };

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

    internal static string GetLayer(EaElement element)
    {
        var stereotype = !string.IsNullOrWhiteSpace(element.FQStereotype) ? element.FQStereotype
            : !string.IsNullOrWhiteSpace(element.StereotypeEx) ? element.StereotypeEx
            : element.Stereotype;

        var (language, type) = ParseStereotype(stereotype);

        if (!string.IsNullOrWhiteSpace(stereotype) && ArchiMateMap.TryGetValue(stereotype, out var entry))
            return entry.Layer;

        if (string.Equals(language, "EDGY", StringComparison.OrdinalIgnoreCase) &&
            EdgyMap.TryGetValue(type, out var edgyEntry))
            return edgyEntry.Layer;

        if (language.StartsWith("ArchiMate", StringComparison.OrdinalIgnoreCase))
        {
            var archiKey = $"ArchiMate_{type}";
            if (ArchiMateMap.TryGetValue(archiKey, out entry))
                return entry.Layer;
        }

        if (!string.IsNullOrWhiteSpace(type) && ArchiMateTypeMap.TryGetValue(type, out entry))
            return entry.Layer;

        return "uml";
    }

    internal static string GetStereotypeLabel(EaElement element)
    {
        var stereotype = !string.IsNullOrWhiteSpace(element.FQStereotype) ? element.FQStereotype
            : !string.IsNullOrWhiteSpace(element.StereotypeEx) ? element.StereotypeEx
            : element.Stereotype;

        var (_, type) = ParseStereotype(stereotype);
        var layer = GetLayer(element);
        return $"<span class=\"sl\" data-layer=\"{layer}\">{type}</span>";
    }
}
