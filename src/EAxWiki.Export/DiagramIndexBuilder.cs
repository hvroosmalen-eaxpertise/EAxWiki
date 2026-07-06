using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export;

internal static class DiagramIndexBuilder
{
    public static (List<(EaDiagram Diagram, string PkgDir)> AllDiagrams, Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> DiagramIndex) Build(
        List<EaPackage> packages, string outputDir)
    {
        var allDiagrams = new List<(EaDiagram, string PkgDir)>();
        foreach (var pkg in packages)
            CollectDiagramsRecursive(pkg, outputDir, allDiagrams);

        var diagramIndex = new Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>>();
        foreach (var (diagram, pkgDir) in allDiagrams)
        {
            foreach (var dob in diagram.DiagramObjects)
            {
                if (!diagramIndex.ContainsKey(dob.ElementId))
                    diagramIndex[dob.ElementId] = new List<(EaDiagram, string)>();
                diagramIndex[dob.ElementId].Add((diagram, pkgDir));
            }
        }

        return (allDiagrams, diagramIndex);
    }

    private static void CollectDiagramsRecursive(EaPackage package, string outputDir, List<(EaDiagram, string)> result)
    {
        var pkgDir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
        foreach (var diagram in package.Diagrams)
            result.Add((diagram, pkgDir));
        foreach (var child in package.Children)
            CollectDiagramsRecursive(child, outputDir, result);
    }
}
