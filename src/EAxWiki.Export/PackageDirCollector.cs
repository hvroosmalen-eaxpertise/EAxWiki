using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export;

internal static class PackageDirCollector
{
    public static HashSet<string> Collect(List<EaPackage> packages, string outputDir)
    {
        var dirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pkg in packages)
            CollectPackageDirs(pkg, outputDir, dirs);
        return dirs;
    }

    private static void CollectPackageDirs(EaPackage package, string outputDir, HashSet<string> dirs)
    {
        var pkgDir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
        dirs.Add(pkgDir);
        dirs.Add(Path.Combine(pkgDir, "diagrams"));
        foreach (var child in package.Children)
            CollectPackageDirs(child, outputDir, dirs);
    }
}
