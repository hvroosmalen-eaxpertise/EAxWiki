using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export;

internal static class ContextBuilder
{
    internal static ExportContext Build(List<EaPackage> packages, string outputPath, bool force = false)
    {
        var elements = new List<(EaElement Element, string PackageDir)>();
        foreach (var pkg in packages)
            CollectElements(pkg, outputPath, elements);

        var allDiagrams = CollectDiagrams(packages, outputPath);

        var elementLookup = elements
            .GroupBy(e => e.Element.Id)
            .ToDictionary(g => g.Key, g => g.First());

        var packageLookup = BuildPackageLookup(packages);

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

        var incomingIndex = new Dictionary<int, List<(EaConnector Connector, int SourceId)>>();
        var seenConnectors = new HashSet<int>();
        foreach (var (elem, _) in elements)
        {
            foreach (var conn in elem.Connectors)
            {
                if (!seenConnectors.Add(conn.Id)) continue;
                if (!incomingIndex.ContainsKey(conn.TargetId))
                    incomingIndex[conn.TargetId] = new List<(EaConnector, int)>();
                incomingIndex[conn.TargetId].Add((conn, conn.SourceId));
            }
        }

        var allPackageDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pkg in packages)
            CollectPackageDirs(pkg, outputPath, allPackageDirs);

        return new ExportContext(outputPath, elements, elementLookup, allDiagrams, diagramIndex, incomingIndex, packageLookup, force)
        {
            AllPackageDirs = allPackageDirs
        };
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

    private static List<(EaDiagram Diagram, string PackageDir)> CollectDiagrams(List<EaPackage> packages, string outputDir)
    {
        var result = new List<(EaDiagram, string)>();
        foreach (var pkg in packages)
            CollectDiagramsRecursive(pkg, outputDir, result);
        return result;
    }

    private static void CollectDiagramsRecursive(EaPackage package, string outputDir, List<(EaDiagram, string)> result)
    {
        var pkgDir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
        foreach (var diagram in package.Diagrams)
            result.Add((diagram, pkgDir));
        foreach (var child in package.Children)
            CollectDiagramsRecursive(child, outputDir, result);
    }

    private static void CollectPackageDirs(EaPackage package, string outputDir, HashSet<string> dirs)
    {
        var pkgDir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
        dirs.Add(pkgDir);
        dirs.Add(Path.Combine(pkgDir, "diagrams"));
        foreach (var child in package.Children)
            CollectPackageDirs(child, outputDir, dirs);
    }

    private static Dictionary<int, (string Name, int? ParentId)> BuildPackageLookup(List<EaPackage> packages)
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
