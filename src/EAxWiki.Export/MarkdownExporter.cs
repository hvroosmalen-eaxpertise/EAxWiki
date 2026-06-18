using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;

namespace EAxWiki.Export;

public class MarkdownExporter : IWikiExporter
{
    private readonly IOutputWriter _writer;
    private readonly ILogger<MarkdownExporter> _logger;
    private static readonly char[] _invalidChars = Path.GetInvalidFileNameChars().Append('#').ToArray();
    private static readonly ConcurrentDictionary<string, string> _sanitizeCache = new();

    public MarkdownExporter(IOutputWriter writer, ILogger<MarkdownExporter> logger)
    {
        _writer = writer;
        _logger = logger;
    }

    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null)
    {
        try
        {
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, recursive: true);
            }

            var totalStopwatch = Stopwatch.StartNew();

            var packages = startPackage != null
                ? new List<EaPackage> { startPackage }
                : repository.RootPackages;

            var elements = new List<(EaElement Element, string PackageDir)>();

            var packageLookup = BuildPackageLookup(packages);

            foreach (var pkg in packages)
            {
                await ExportPackageAsync(pkg, outputPath, elements, packageLookup);
            }

            await WriteRootIndexAsync(packages, outputPath);
            await GenerateTypesPagesAsync(elements, outputPath);
            await WritePagesFileAsync(outputPath);

            if (reader != null)
            {
                try
                {
                    await ExportDiagramsAsync(packages, elements, outputPath, reader, packageLookup);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Diagram export failed");
                }
            }

            totalStopwatch.Stop();
            _logger.LogInformation("Export complete: {TotalElapsedMs}ms total", totalStopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Export failed unexpectedly");
        }
    }

    private async Task ExportPackageAsync(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var pkgStopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Exporting package {PackageName} ({ElementCount} elements, {DiagramCount} diagrams)",
            package.Name, package.Elements.Count, package.Diagrams.Count);

        var dir = Path.Combine(outputDir, SanitizeName(package.Name));
        await _writer.CreateDirectoryAsync(dir);

        var indexLines = new List<string>
        {
            $"# {package.Name}",
            string.Empty
        };

        indexLines.Add(BuildBreadcrumb(package.Id, dir, outputDir, packageLookup));
        indexLines.Add(string.Empty);

        if (!string.IsNullOrWhiteSpace(package.Notes))
        {
            indexLines.Add(package.Notes);
            indexLines.Add(string.Empty);
        }

        if (package.Diagrams.Count > 0)
        {
            indexLines.Add("## Diagrams");
            indexLines.Add(string.Empty);

            foreach (var diag in package.Diagrams)
            {
                var diagFile = $"diagrams/{SanitizeName(diag.Name)}.md";
                indexLines.Add($"- [{diag.Name}]({diagFile}) ({diag.Type})");

                if (!string.IsNullOrWhiteSpace(diag.Notes))
                {
                    var notesPreview = diag.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    indexLines.Add($"  - *{notesPreview}*");
                }
            }

            indexLines.Add(string.Empty);
        }

        if (package.Elements.Count > 0)
        {
            indexLines.Add("## Elements");
            indexLines.Add(string.Empty);

            foreach (var elem in package.Elements)
            {
                var elemFile = $"{SanitizeName(elem.Name)}.md";
                await WriteElementAsync(elem, dir, outputDir, packageLookup);
                elements.Add((elem, dir));

                var typeLabel = string.IsNullOrEmpty(elem.Stereotype)
                    ? elem.Type
                    : $"{elem.Stereotype} «{elem.Type}»";
                indexLines.Add($"- [{elem.Name}]({elemFile}) — {typeLabel}");

                if (!string.IsNullOrWhiteSpace(elem.Notes))
                {
                    var notesPreview = elem.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    indexLines.Add($"  - *{notesPreview}*");
                }
            }

            indexLines.Add(string.Empty);
        }

        if (package.Children.Count > 0)
        {
            indexLines.Add("## Sub-packages");
            indexLines.Add(string.Empty);

            foreach (var child in package.Children)
            {
                var childDir = Path.Combine(outputDir, SanitizeName(child.Name));
                var childRelPath = Path.GetRelativePath(dir, Path.Combine(childDir, "index.md")).Replace('\\', '/');
                indexLines.Add($"- [{child.Name}]({childRelPath})");
            }

            indexLines.Add(string.Empty);
        }

        indexLines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(dir, "index.md"), string.Join(Environment.NewLine, indexLines));

        pkgStopwatch.Stop();
        _logger.LogInformation("Exported package {PackageName} in {ElapsedMs}ms",
            package.Name, pkgStopwatch.ElapsedMilliseconds);

        foreach (var child in package.Children)
        {
            await ExportPackageAsync(child, outputDir, elements, packageLookup);
        }
    }

    private async Task WriteRootIndexAsync(List<EaPackage> rootPackages, string outputDir)
    {
        var lines = new List<string>
        {
            "# EurSuRA Wiki",
            string.Empty,
            "## Structure",
            string.Empty,
        };

        foreach (var pkg in rootPackages)
        {
            var dir = SanitizeName(pkg.Name);
            lines.Add($"- [{pkg.Name}]({dir}/index.md)");
        }

        lines.Add(string.Empty);
        lines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(outputDir, "index.md"), string.Join(Environment.NewLine, lines));
    }

    private async Task GenerateTypesPagesAsync(List<(EaElement Element, string PackageDir)> elements, string outputDir)
    {
        var typesDir = Path.Combine(outputDir, "types");
        await _writer.CreateDirectoryAsync(typesDir);

        var typesStopwatch = Stopwatch.StartNew();

        var groups = elements
            .Select(e => (
                Element: e.Element,
                PackageDir: e.PackageDir,
                Stereo: string.IsNullOrWhiteSpace(e.Element.Stereotype) ? "Uncategorized" : e.Element.Stereotype
            ))
            .ToLookup(x => x.Stereo, x => (x.Element, x.PackageDir));

        var stereotypes = groups.Select(g => g.Key).OrderBy(s => s).ToList();

        _logger.LogInformation("Generating {StereotypeCount} type pages", stereotypes.Count);

        var indexLines = new List<string>
        {
            "# Types",
            string.Empty,
            "Elements grouped by stereotype:",
            string.Empty,
        };

        foreach (var stereo in stereotypes)
        {
            indexLines.Add($"- [{stereo}]({SanitizeName(stereo)}.md)");
        }

        indexLines.Add(string.Empty);
        indexLines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(typesDir, "index.md"), string.Join(Environment.NewLine, indexLines));

        foreach (var stereo in stereotypes)
        {
            var matching = groups[stereo].ToList();

            var lines = new List<string>
            {
                $"# {stereo}",
                string.Empty,
                $"{matching.Count} element(s) with this stereotype:",
                string.Empty,
            };

            foreach (var (elem, pkgDir) in matching)
            {
                var elemName = SanitizeName(elem.Name);
                var relativeDir = Path.GetRelativePath(outputDir, pkgDir);
                var relativePath = $"../{relativeDir}/{elemName}.md".Replace('\\', '/');
                lines.Add($"- [{elem.Name}]({relativePath})");
            }

            lines.Add(string.Empty);
            lines.Add(FormatTimestamp());
            await _writer.WriteFileAsync(Path.Combine(typesDir, $"{SanitizeName(stereo)}.md"), string.Join(Environment.NewLine, lines));
        }

        typesStopwatch.Stop();
        _logger.LogInformation("Generated {StereotypeCount} type pages in {ElapsedMs}ms",
            stereotypes.Count, typesStopwatch.ElapsedMilliseconds);
    }

    private async Task WritePagesFileAsync(string outputDir)
    {
        var pagesPath = Path.Combine(outputDir, ".pages");
        var content = new List<string>
        {
            "nav:",
            "  - Structure: ''",
            "  - Types: types/",
            string.Empty,
        };
        await _writer.WriteFileAsync(pagesPath, string.Join(Environment.NewLine, content));

        var typesPagesDir = Path.Combine(outputDir, "types");
        await _writer.CreateDirectoryAsync(typesPagesDir);
        var typesPagesPath = Path.Combine(typesPagesDir, ".pages");
        var typesContent = new List<string>
        {
            "title: Types",
            string.Empty,
        };
        await _writer.WriteFileAsync(typesPagesPath, string.Join(Environment.NewLine, typesContent));
    }

    private async Task WriteElementAsync(EaElement element, string dir, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        _logger.LogDebug("Writing element {ElementName}", element.Name);
        var lines = new List<string>
        {
            $"# {element.Name}",
            string.Empty,
            $"**Type:** {element.Type}  ",
            $"**Stereotype:** {element.Stereotype}  ",
            string.Empty
        };

        lines.Add(string.Empty);
        lines.Add(BuildBreadcrumb(element.PackageId, dir, outputDir, packageLookup));
        lines.Add(string.Empty);

        if (!string.IsNullOrWhiteSpace(element.Notes))
        {
            lines.Add(element.Notes);
            lines.Add(string.Empty);
        }

        if (element.Attributes.Count > 0)
        {
            lines.Add("## Attributes");
            lines.Add(string.Empty);
            lines.Add("| Name | Type | Default | Description |");
            lines.Add("|------|------|---------|-------------|");

            foreach (var attr in element.Attributes)
            {
                var desc = attr.Notes?.Replace("\n", "<br/>") ?? "";
                lines.Add($"| {attr.Name} | {attr.Type} | {attr.DefaultValue} | {desc} |");
            }

            lines.Add(string.Empty);
        }

        if (element.Methods.Count > 0)
        {
            lines.Add("## Methods");
            lines.Add(string.Empty);

            foreach (var method in element.Methods)
            {
                var staticTag = method.IsStatic ? " *(static)*" : "";
                lines.Add($"### {method.Name}{staticTag}");
                lines.Add(string.Empty);
                lines.Add($"**Returns:** `{method.Type}`");
                lines.Add(string.Empty);
                if (!string.IsNullOrWhiteSpace(method.Notes))
                {
                    lines.Add(method.Notes);
                    lines.Add(string.Empty);
                }
            }
        }

        if (element.TaggedValues.Count > 0)
        {
            lines.Add("## Tagged Values");
            lines.Add(string.Empty);
            lines.Add("| Name | Value | Notes |");
            lines.Add("|------|-------|-------|");

            foreach (var tv in element.TaggedValues)
            {
                lines.Add($"| {tv.Name} | {tv.Value} | {tv.Notes} |");
            }

            lines.Add(string.Empty);
        }

        if (element.Connectors.Count > 0)
        {
            lines.Add("## Relationships");
            lines.Add(string.Empty);
            lines.Add("| Type | Stereotype | Source → Target |");
            lines.Add("|------|------------|-----------------|");

            foreach (var conn in element.Connectors)
            {
                lines.Add($"| {conn.Type} | {conn.Stereotype} | {conn.SourceId} → {conn.TargetId} |");
            }

            lines.Add(string.Empty);
        }

        var filePath = Path.Combine(dir, $"{SanitizeName(element.Name)}.md");
        lines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(filePath, string.Join(Environment.NewLine, lines));
    }

    private async Task ExportDiagramsAsync(List<EaPackage> rootPackages, List<(EaElement Element, string PackageDir)> elements, string outputDir, IEaReader reader, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var elementLookup = elements
            .GroupBy(e => e.Element.Id)
            .ToDictionary(g => g.Key, g => g.First());

        var diagrams = CollectDiagrams(rootPackages, outputDir);
        _logger.LogInformation("Exporting {DiagramCount} diagrams with PNG images", diagrams.Count);

        foreach (var (diagram, pkgDir) in diagrams)
        {
            try
            {
                var diagramsDir = Path.Combine(pkgDir, "diagrams");
                await _writer.CreateDirectoryAsync(diagramsDir);

                var fileName = SanitizeName(diagram.Name);
                var pngPath = Path.Combine(diagramsDir, $"{fileName}.png");
                var mdPath = Path.Combine(diagramsDir, $"{fileName}.md");
                var diagramStopwatch = Stopwatch.StartNew();

                var pngSuccess = reader.ExportDiagramImage(diagram.Guid, pngPath);
                if (!pngSuccess)
                    _logger.LogWarning("Failed to export PNG for diagram {DiagramName}", diagram.Name);

                var lines = new List<string>
                {
                    $"# {diagram.Name}",
                    string.Empty,
                };

                lines.Add(string.Empty);
                lines.Add(BuildBreadcrumb(diagram.PackageId, diagramsDir, outputDir, packageLookup));
                lines.Add(string.Empty);

                if (!string.IsNullOrWhiteSpace(diagram.Notes))
                {
                    lines.Add(diagram.Notes);
                    lines.Add(string.Empty);
                }

                if (File.Exists(pngPath))
                {
                    lines.Add($"![{diagram.Name}]({fileName}.png)");
                    lines.Add(string.Empty);
                }

                var diagramElements = new List<(EaElement Element, string PackageDir)>();
                foreach (var dob in diagram.DiagramObjects)
                {
                    if (elementLookup.TryGetValue(dob.ElementId, out var elem))
                        diagramElements.Add(elem);
                }

                if (diagramElements.Count > 0)
                {
                    lines.Add("## Elements");
                    lines.Add(string.Empty);

                    foreach (var (elemEa, elemDir) in diagramElements)
                    {
                        var elemLink = Path.GetRelativePath(diagramsDir, Path.Combine(elemDir, $"{SanitizeName(elemEa.Name)}.md")).Replace('\\', '/');
                        lines.Add($"- [{elemEa.Name}]({elemLink})");
                    }

                    lines.Add(string.Empty);
                }

                lines.Add(FormatTimestamp());
                await _writer.WriteFileAsync(mdPath, string.Join(Environment.NewLine, lines));

                diagramStopwatch.Stop();
                _logger.LogInformation("Exported diagram {DiagramName} in {ElapsedMs}ms",
                    diagram.Name, diagramStopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to export diagram {DiagramName}", diagram.Name);
            }
        }
    }

    private static List<(EaDiagram Diagram, string PackageDir)> CollectDiagrams(List<EaPackage> packages, string outputDir)
    {
        var result = new List<(EaDiagram, string)>();
        foreach (var pkg in packages)
        {
            CollectDiagramsRecursive(pkg, outputDir, result);
        }
        return result;
    }

    private static void CollectDiagramsRecursive(EaPackage package, string outputDir, List<(EaDiagram, string)> result)
    {
        var pkgDir = Path.Combine(outputDir, SanitizeName(package.Name));

        foreach (var diagram in package.Diagrams)
        {
            result.Add((diagram, pkgDir));
        }

        foreach (var child in package.Children)
        {
            CollectDiagramsRecursive(child, outputDir, result);
        }
    }

    private static string FormatTimestamp()
    {
        return $"---\n\n*Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";
    }

    private static string SanitizeName(string name)
    {
        name = name.Trim();
        if (_sanitizeCache.TryGetValue(name, out var cached))
            return cached;
        var sanitized = new string(name.Select(ch => _invalidChars.Contains(ch) ? '_' : ch).ToArray());
        var result = string.IsNullOrWhiteSpace(sanitized) ? "unnamed" : sanitized;
        _sanitizeCache[name] = result;
        return result;
    }

    private static Dictionary<int, (string Name, int? ParentId)> BuildPackageLookup(List<EaPackage> packages)
    {
        var lookup = new Dictionary<int, (string Name, int? ParentId)>();
        foreach (var pkg in packages)
        {
            BuildPackageLookupRecursive(pkg, lookup);
        }
        return lookup;
    }

    private static void BuildPackageLookupRecursive(EaPackage package, Dictionary<int, (string Name, int? ParentId)> lookup)
    {
        lookup[package.Id] = (package.Name, package.ParentId);
        foreach (var child in package.Children)
        {
            BuildPackageLookupRecursive(child, lookup);
        }
    }

    private static string BuildBreadcrumb(int packageId, string currentPageDir, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var segments = new List<(string Name, int Id)>();
        var id = packageId;
        while (id != 0 && packageLookup.TryGetValue(id, out var info))
        {
            segments.Add((info.Name, id));
            id = info.ParentId ?? 0;
        }
        segments.Reverse();

        var breadcrumbParts = new List<string>();
        foreach (var (name, _) in segments)
        {
            var pkgDir = Path.Combine(outputDir, SanitizeName(name));
            var relPath = Path.GetRelativePath(currentPageDir, Path.Combine(pkgDir, "index.md")).Replace('\\', '/');
            breadcrumbParts.Add($"[{name}]({relPath})");
        }
        return string.Join(" / ", breadcrumbParts);
    }
}
