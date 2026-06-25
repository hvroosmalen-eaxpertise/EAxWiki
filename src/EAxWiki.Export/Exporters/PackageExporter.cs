using System.Diagnostics;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class PackageExporter(IOutputWriter writer, ILogger logger)
{
    public async Task ExportAsync(EaPackage package, ExportContext ctx, Action<int>? onElementsWritten = null)
    {
        var pkgStopwatch = Stopwatch.StartNew();
        logger.LogInformation("Exporting package {PackageName} ({ElementCount} elements, {DiagramCount} diagrams)",
            package.Name, package.Elements.Count, package.Diagrams.Count);

        var outputDir = ctx.OutputPath;
        var dir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
        await writer.CreateDirectoryAsync(dir);

        var indexLines = new List<string>
        {
            $"# {package.Name}",
            string.Empty,
        };

        indexLines.Add(MarkdownHelpers.BuildBreadcrumb(package.Id, dir, outputDir, ctx.PackageLookup,
            msg => logger.LogWarning("{Message} (package '{Name}')", msg, package.Name)));
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
                var diagFile = $"diagrams/{MarkdownHelpers.SanitizeName(diag.Name)}.md";
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

            var elementWriter = new ElementPageWriter(writer, logger);
            var elementTasks = new List<Task>();

            // Resolve file names once so collisions can be detected and disambiguated.
            var seenNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var elementFileNames = new Dictionary<int, string>(package.Elements.Count);
            foreach (var elem in package.Elements)
            {
                var sanitized = MarkdownHelpers.SanitizeName(elem.Name);
                if (seenNames.TryGetValue(sanitized, out _))
                {
                    // Append element ID to make the name unique within this package.
                    sanitized = $"{sanitized}_{elem.Id}";
                    logger.LogWarning("Duplicate sanitized name in package '{Package}': element '{Name}' (ID {Id}) renamed to '{NewName}'",
                        package.Name, elem.Name, elem.Id, sanitized);
                }
                seenNames[sanitized] = elem.Id;
                elementFileNames[elem.Id] = sanitized;
            }

            foreach (var elem in package.Elements)
            {
                var baseName = elementFileNames[elem.Id];
                var elemFile = $"{baseName}.md";
                ctx.RegisteredElementFiles.Add(Path.Combine(dir, elemFile));
                elementTasks.Add(elementWriter.WriteAsync(elem, dir, ctx, baseName));

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

            try
            {
                await Task.WhenAll(elementTasks);
                onElementsWritten?.Invoke(package.Elements.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to write one or more element pages in package {PackageName}", package.Name);
                throw;
            }

            indexLines.Add(string.Empty);
        }

        if (package.Children.Count > 0)
        {
            indexLines.Add("## Sub-packages");
            indexLines.Add(string.Empty);

            foreach (var child in package.Children)
            {
                var childDir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(child.Name));
                var childRelPath = Path.GetRelativePath(dir, Path.Combine(childDir, "index.md")).Replace('\\', '/');
                indexLines.Add($"- [{child.Name}]({childRelPath})");
            }

            indexLines.Add(string.Empty);
        }

        indexLines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(dir, "index.md"), string.Join(Environment.NewLine, indexLines));

        pkgStopwatch.Stop();
        logger.LogInformation("Exported package {PackageName} in {ElapsedMs}ms", package.Name, pkgStopwatch.ElapsedMilliseconds);

        foreach (var child in package.Children)
            await ExportAsync(child, ctx, onElementsWritten);
    }
}
