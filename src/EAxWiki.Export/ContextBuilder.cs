using EAxWiki.Core.Models;

namespace EAxWiki.Export;

internal static class ContextBuilder
{
    internal static ExportContext Build(List<EaPackage> packages, string outputPath, bool force = false)
    {
        var elements = ElementCollector.Collect(packages, outputPath);
        var (allDiagrams, diagramIndex) = DiagramIndexBuilder.Build(packages, outputPath);
        var elementLookup = LookupBuilder.BuildElementLookup(elements);
        var packageLookup = LookupBuilder.BuildPackageLookup(packages);
        var incomingIndex = ConnectorIndexBuilder.Build(elements);
        var allPackageDirs = PackageDirCollector.Collect(packages, outputPath);

        return new ExportContext(outputPath, elements, elementLookup, allDiagrams, diagramIndex, incomingIndex, packageLookup, force)
        {
            AllPackageDirs = allPackageDirs
        };
    }
}
