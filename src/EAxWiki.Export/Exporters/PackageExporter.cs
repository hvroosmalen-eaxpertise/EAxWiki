using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;
using EAxWiki.Export.Renderers;

namespace EAxWiki.Export.Exporters;

internal class PackageExporter(IOutputWriter writer, ILogger logger)
{
    public async Task<(int Succeeded, int Failed)> ExportAsync(EaPackage package, ExportContext ctx, Action<int>? onElementsWritten = null, CancellationToken ct = default)
    {
        var pkgStopwatch = Stopwatch.StartNew();
        logger.LogInformation("Exporting package {PackageName} ({ElementCount} elements, {DiagramCount} diagrams)",
            package.Name, package.Elements.Count, package.Diagrams.Count);

        var outputDir = ctx.OutputPath;
        var dir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(package.Name));
        await writer.CreateDirectoryAsync(dir, ct);

        var indexLines = new List<string>
        {
            $"# {package.Name}",
            string.Empty,
        };

        indexLines.Add(MarkdownHelpers.BuildBreadcrumb(package.Id, dir, outputDir, ctx.PackageLookup,
            msg => logger.LogWarning("{Message} (package '{Name}')", msg, package.Name)));
        indexLines.Add(string.Empty);

        if (ctx.ApiPort > 0)
        {
            var notesHash = HtmlHelpers.ComputeNotesHash(package.Notes);

            indexLines.Insert(0, "---");
            indexLines.Insert(1, $"package_id: {package.Id}");
            indexLines.Insert(2, $"notes_hash: {notesHash}");
            indexLines.Insert(3, "---");
            indexLines.Insert(4, string.Empty);

            var wikiRelPath = Path.GetRelativePath(outputDir, Path.Combine(dir, "index.md")).Replace('\\', '/');
            var wikiRelPathHtml = HtmlHelpers.HtmlEscape(wikiRelPath);
            var normalizedNotes = FrontmatterParser.NormalizeNotesHtml(package.Notes);
            indexLines.AddRange(NotesWidgetRenderer.Render(package, ctx, normalizedNotes, wikiRelPathHtml, kind: "package"));
        }
        else if (!string.IsNullOrWhiteSpace(package.Notes))
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
                var diagFile = $"diagrams/{MarkdownHelpers.SanitizeName(diag.Name)}.html";
                indexLines.Add($"- [{diag.Name}]({diagFile}) ({diag.Type})");

                if (!string.IsNullOrWhiteSpace(diag.Notes))
                {
                    var notesPreview = diag.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    indexLines.Add($"  - *{notesPreview}*");
                }
            }

            indexLines.Add(string.Empty);
        }

        var totalFailed = 0;

        if (package.Elements.Count > 0)
        {
            indexLines.Add("## Elements");
            indexLines.Add(string.Empty);

            var elementWriter = new ElementPageWriter(writer, logger);

            var seenNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var elementFileNames = new Dictionary<int, string>(package.Elements.Count);
            foreach (var elem in package.Elements)
            {
                var sanitized = MarkdownHelpers.SanitizeName(elem.Name);
                if (seenNames.TryGetValue(sanitized, out _))
                {
                    sanitized = $"{sanitized}_{elem.Id}";
                    logger.LogWarning("Duplicate sanitized name in package '{Package}': element '{Name}' (ID {Id}) renamed to '{NewName}'",
                        package.Name, elem.Name, elem.Id, sanitized);
                }
                seenNames[sanitized] = elem.Id;
                elementFileNames[elem.Id] = sanitized;
            }

            var elementTasks = new List<Task>();
            foreach (var elem in package.Elements)
            {
                var baseName = elementFileNames[elem.Id];
                var elemFile = $"{baseName}.md";
                ctx.RegisteredElementFiles.Add(Path.Combine(dir, elemFile));
                elementTasks.Add(elementWriter.WriteAsync(elem, dir, ctx, baseName, ct));

                indexLines.Add($"- {MarkdownHelpers.GetStereotypeLabel(elem)} [{elem.Name}]({baseName}.html)");

                if (!string.IsNullOrWhiteSpace(elem.Notes))
                {
                    var notesPreview = elem.Notes.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    indexLines.Add($"  - *{notesPreview}*");
                }
            }

            // Await all tasks, swallowing exceptions since we inspect individual tasks.
            var whenAll = Task.WhenAll(elementTasks);
            try { await whenAll; } catch { }

            foreach (var t in elementTasks)
            {
                if (!t.IsFaulted) continue;
                var ex = t.Exception?.InnerException ?? t.Exception;
                if (ex is OperationCanceledException) throw ex;
                logger.LogWarning(ex, "Failed to write element in package {PackageName}", package.Name);
                totalFailed++;
            }

            onElementsWritten?.Invoke(package.Elements.Count);

            indexLines.Add(string.Empty);
        }

        if (package.Children.Count > 0)
        {
            indexLines.Add("## Sub-packages");
            indexLines.Add(string.Empty);

            foreach (var child in package.Children)
            {
                var childDir = Path.Combine(outputDir, MarkdownHelpers.SanitizeName(child.Name));
                var childRelPath = Path.GetRelativePath(dir, Path.Combine(childDir, "index.html")).Replace('\\', '/');
                indexLines.Add($"- [{child.Name}]({childRelPath})");
            }

            indexLines.Add(string.Empty);
        }

        var indexPath = Path.Combine(dir, "index.md");
        await writer.WriteFileAsync(indexPath, string.Join(Environment.NewLine, indexLines), ct);
        ctx.WrittenMdFiles.Add(indexPath);

        pkgStopwatch.Stop();
        var succeeded = package.Elements.Count - totalFailed;
        logger.LogInformation("Exported package {PackageName} in {ElapsedMs}ms ({Succeeded} succeeded, {Failed} failed)",
            package.Name, pkgStopwatch.ElapsedMilliseconds, succeeded, totalFailed);

        foreach (var child in package.Children)
        {
            var (childSucceeded, childFailed) = await ExportAsync(child, ctx, onElementsWritten, ct);
            succeeded += childSucceeded;
            totalFailed += childFailed;
        }

        return (succeeded, totalFailed);
    }
}
