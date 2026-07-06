using EAxWiki.Core.Models;

namespace EAxWiki.Export;

internal static class LookupBuilder
{
    public static Dictionary<int, (EaElement Element, string PackageDir)> BuildElementLookup(
        List<(EaElement Element, string PackageDir)> elements)
    {
        return elements
            .GroupBy(e => e.Element.Id)
            .ToDictionary(g => g.Key, g => g.First());
    }

    public static Dictionary<int, (string Name, int? ParentId)> BuildPackageLookup(
        List<EaPackage> packages)
    {
        var lookup = new Dictionary<int, (string Name, int? ParentId)>();
        foreach (var pkg in packages)
            BuildPackageLookupRecursive(pkg, lookup);
        return lookup;
    }

    private static void BuildPackageLookupRecursive(EaPackage package, Dictionary<int, (string Name, int? ParentId)> lookup)
    {
        lookup[package.Id] = (package.Name, package.ParentId);
        foreach (var child in package.Children)
            BuildPackageLookupRecursive(child, lookup);
    }
}
