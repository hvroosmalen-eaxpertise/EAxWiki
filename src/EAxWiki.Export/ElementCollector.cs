using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export;

internal static class ElementCollector
{
    public static List<(EaElement Element, string PackageDir)> Collect(List<EaPackage> packages, string outputDir)
    {
        var elements = new List<(EaElement Element, string PackageDir)>();
        foreach (var pkg in packages)
            CollectElements(pkg, outputDir, elements);
        return elements;
    }

    private static void CollectElements(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements)
    {
        if (package.Elements.Count > 0)
        {
            var dir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
            foreach (var elem in package.Elements)
                elements.Add((elem, dir));
        }

        foreach (var child in package.Children)
            CollectElements(child, outputDir, elements);
    }
}
