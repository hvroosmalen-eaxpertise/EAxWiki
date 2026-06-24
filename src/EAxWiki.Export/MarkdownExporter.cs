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

    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null, bool force = false)
    {
        _sanitizeCache.Clear();
        try
        {
            if (force && Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, recursive: true);
            }
            Directory.CreateDirectory(outputPath);

            var testFile = Path.Combine(outputPath, ".write-test");
            try { File.WriteAllText(testFile, ""); File.Delete(testFile); }
            catch (Exception ex) { throw new InvalidOperationException($"Output path is not writable: {outputPath}", ex); }

            var totalStopwatch = Stopwatch.StartNew();

            var packages = startPackage != null
                ? new List<EaPackage> { startPackage }
                : repository.RootPackages;

            var elements = new List<(EaElement Element, string PackageDir)>();

            var packageLookup = BuildPackageLookup(packages);

            // Phase 1: collect all elements before writing pages
            foreach (var pkg in packages)
            {
                CollectElements(pkg, outputPath, elements);
            }

            // Build diagram index: element ID → list of (diagram, package dir)
            var diagramIndex = new Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>>();
            foreach (var (diagram, pkgDir) in CollectDiagrams(packages, outputPath))
            {
                foreach (var dob in diagram.DiagramObjects)
                {
                    if (!diagramIndex.ContainsKey(dob.ElementId))
                        diagramIndex[dob.ElementId] = new List<(EaDiagram, string)>();
                    diagramIndex[dob.ElementId].Add((diagram, pkgDir));
                }
            }

            // Build incoming connector index: element ID → list of (connector, source element ID)
            var incomingIndex = new Dictionary<int, List<(EaConnector Connector, int SourceId)>>();
            foreach (var (elem, _) in elements)
            {
                foreach (var conn in elem.Connectors)
                {
                    if (!incomingIndex.ContainsKey(conn.TargetId))
                        incomingIndex[conn.TargetId] = new List<(EaConnector, int)>();
                    incomingIndex[conn.TargetId].Add((conn, conn.SourceId));
                }
            }

            // Phase 2: write pages with complete lookup
            foreach (var pkg in packages)
            {
                await ExportPackageAsync(pkg, outputPath, elements, packageLookup, diagramIndex, incomingIndex);
            }

            await WriteRootIndexAsync(packages, outputPath, repository.ConnectionString);
            await GenerateTypesPagesAsync(elements, outputPath);
            await GenerateGlossaryAsync(elements, outputPath);
            await GenerateRecentChangesAsync(packages, elements, outputPath, packageLookup);
            await WritePagesFileAsync(outputPath);
            await WriteExtraCssAsync(outputPath);

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

            await WriteDiagramsIndexAsync(packages, outputPath, packageLookup);

            await CleanupOrphanedFilesAsync(elements);

            totalStopwatch.Stop();
            _logger.LogInformation("Export complete: {TotalElapsedMs}ms total", totalStopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Export failed unexpectedly");
        }
    }

    private async Task ExportPackageAsync(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup, Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> diagramIndex, Dictionary<int, List<(EaConnector Connector, int SourceId)>> incomingIndex)
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

            var elementTasks = new List<Task>();
            foreach (var elem in package.Elements)
            {
                var elemFile = $"{SanitizeName(elem.Name)}.md";
                elementTasks.Add(WriteElementAsync(elem, dir, outputDir, elements, packageLookup, diagramIndex, incomingIndex));

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write one or more element pages in package {PackageName}", package.Name);
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
            await ExportPackageAsync(child, outputDir, elements, packageLookup, diagramIndex, incomingIndex);
        }
    }

    private static void CollectElements(EaPackage package, string outputDir, List<(EaElement Element, string PackageDir)> elements)
    {
        if (package.Elements.Count > 0)
        {
            var dir = Path.Combine(outputDir, SanitizeName(package.Name));
            foreach (var elem in package.Elements)
            {
                elements.Add((elem, dir));
            }
        }

        foreach (var child in package.Children)
        {
            CollectElements(child, outputDir, elements);
        }
    }

    private async Task WriteRootIndexAsync(List<EaPackage> rootPackages, string outputDir, string repositoryPath)
    {
        var lines = new List<string>
        {
            "# EurSuRA Wiki",
            string.Empty,
        };

        if (!string.IsNullOrEmpty(repositoryPath))
        {
            lines.Add("## Repository");
            lines.Add(string.Empty);
            lines.Add(repositoryPath);
            lines.Add(string.Empty);
        }

        lines.Add("## Repository Structure");
        lines.Add(string.Empty);

        foreach (var pkg in rootPackages)
        {
            var dir = SanitizeName(pkg.Name);
            lines.Add($"- [{pkg.Name}]({dir}/index.md)");
        }

        lines.Add(string.Empty);
        lines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(outputDir, "index.md"), string.Join(Environment.NewLine, lines));
    }

    private static (string Language, string Type) ParseStereotype(string stereotype)
    {
        if (string.IsNullOrWhiteSpace(stereotype))
            return ("UML", "Uncategorized");

        string language;
        string type;

        // Qualified format: Language::Type
        var separatorIndex = stereotype.IndexOf("::", StringComparison.Ordinal);
        if (separatorIndex >= 0)
        {
            language = stereotype[..separatorIndex];
            type = stereotype[(separatorIndex + 2)..];
        }
        else
        {
            // Flat format: Language_Type (e.g. ArchiMate3_ApplicationComponent)
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

        // Strip language prefix from type (try full name first, then base name)
        type = StripLanguagePrefix(type, language);

        return (language, type);
    }

    private static bool LooksLikeLanguageName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        if (!char.IsUpper(name[0]))
            return false;
        for (var i = 1; i < name.Length; i++)
        {
            if (!char.IsLetterOrDigit(name[i]))
                return false;
        }
        return name.Any(char.IsLetter);
    }

    private static string StripLanguagePrefix(string type, string language)
    {
        // Try full language name first (e.g. ArchiMate3_)
        if (type.StartsWith(language + "_", StringComparison.OrdinalIgnoreCase))
            return type[(language.Length + 1)..];

        // Try base name (non-digit part) (e.g. ArchiMate_)
        var baseName = new string(language.TakeWhile(c => !char.IsDigit(c)).ToArray());
        if (!string.IsNullOrEmpty(baseName) && type.StartsWith(baseName + "_", StringComparison.OrdinalIgnoreCase))
            return type[(baseName.Length + 1)..];

        return type;
    }

    private async Task GenerateTypesPagesAsync(List<(EaElement Element, string PackageDir)> elements, string outputDir)
    {
        var typesDir = Path.Combine(outputDir, "types");
        await _writer.CreateDirectoryAsync(typesDir);

        var typesStopwatch = Stopwatch.StartNew();

        var parsed = elements
            .Select(e =>
            {
                var fq = e.Element.FQStereotype;
                var ex = e.Element.StereotypeEx;
                var stereotypeSrc = !string.IsNullOrWhiteSpace(fq) ? fq
                    : !string.IsNullOrWhiteSpace(ex) ? ex
                    : e.Element.Stereotype;
                var (language, type) = ParseStereotype(stereotypeSrc);
                return (e.Element, e.PackageDir, Language: language, Type: type);
            })
            .ToList();

        var languages = parsed.Select(x => x.Language).Distinct().OrderBy(l => l).ToList();

        _logger.LogInformation("Generating type pages across {LanguageCount} languages", languages.Count);

        // Root types/index.md — list languages
        var indexLines = new List<string>
        {
            "# Types",
            string.Empty,
            "Elements grouped by modelling language:",
            string.Empty,
        };

        foreach (var lang in languages)
        {
            indexLines.Add($"- [{lang}]({SanitizeName(lang)}/index.md)");
        }

        indexLines.Add(string.Empty);
        indexLines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(typesDir, "index.md"), string.Join(Environment.NewLine, indexLines));

        // Per-language pages
        foreach (var lang in languages)
        {
            var langDir = Path.Combine(typesDir, SanitizeName(lang));
            await _writer.CreateDirectoryAsync(langDir);

            // .pages for this language
            var langPages = new List<string>
            {
                $"title: {lang}",
                string.Empty,
            };
            await _writer.WriteFileAsync(Path.Combine(langDir, ".pages"), string.Join(Environment.NewLine, langPages));

            var typeGroups = parsed
                .Where(x => x.Language == lang)
                .GroupBy(x => x.Type)
                .OrderBy(g => g.Key)
                .ToList();

            // Language index.md — list types
            var langIndexLines = new List<string>
            {
                $"# {lang}",
                string.Empty,
                $"{typeGroups.Sum(g => g.Count())} element(s) across {typeGroups.Count} type(s):",
                string.Empty,
            };

            foreach (var group in typeGroups)
            {
                langIndexLines.Add($"- [{group.Key}]({SanitizeName(group.Key)}.md)");
            }

            langIndexLines.Add(string.Empty);
            langIndexLines.Add(FormatTimestamp());
            await _writer.WriteFileAsync(Path.Combine(langDir, "index.md"), string.Join(Environment.NewLine, langIndexLines));

            // Per-type pages
            foreach (var group in typeGroups)
            {
                var lines = new List<string>
                {
                    $"# {group.Key}",
                    string.Empty,
                    $"{group.Count()} element(s):",
                    string.Empty,
                };

                foreach (var item in group)
                {
                    var elemName = SanitizeName(item.Element.Name);
                    var relativeDir = Path.GetRelativePath(outputDir, item.PackageDir);
                    var relativePath = $"../../{relativeDir}/{elemName}.md".Replace('\\', '/');
                    lines.Add($"- [{item.Element.Name}]({relativePath})");
                }

                lines.Add(string.Empty);
                lines.Add(FormatTimestamp());
                await _writer.WriteFileAsync(Path.Combine(langDir, $"{SanitizeName(group.Key)}.md"), string.Join(Environment.NewLine, lines));
            }
        }

        typesStopwatch.Stop();
        _logger.LogInformation("Generated type pages across {LanguageCount} languages in {ElapsedMs}ms",
            languages.Count, typesStopwatch.ElapsedMilliseconds);
    }

    private async Task GenerateGlossaryAsync(List<(EaElement Element, string PackageDir)> elements, string outputDir)
    {
        var glossaryDir = Path.Combine(outputDir, "glossary");
        await _writer.CreateDirectoryAsync(glossaryDir);

        var entries = new List<(string Term, string Definition, List<(string Name, string Link)> Sources)>();

        foreach (var (elem, pkgDir) in elements)
        {
            foreach (var tv in elem.TaggedValues)
            {
                if (!string.IsNullOrWhiteSpace(tv.Value) &&
                    (string.Equals(tv.Name, "Definition", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(tv.Name, "Glossary", StringComparison.OrdinalIgnoreCase)))
                {
                    var link = CreateElementLink(elem, pkgDir, glossaryDir);
                    entries.Add((elem.Name, tv.Value, new List<(string Name, string Link)> { (elem.Name, link) }));
                }
            }

            if (!string.IsNullOrWhiteSpace(elem.Notes))
            {
                var firstDot = elem.Notes.IndexOf('.');
                var sentence = firstDot >= 0
                    ? elem.Notes[..firstDot].Trim()
                    : elem.Notes.Trim();
                if (sentence.Length >= 20)
                {
                    var link = CreateElementLink(elem, pkgDir, glossaryDir);
                    entries.Add((elem.Name, sentence, new List<(string Name, string Link)> { (elem.Name, link) }));
                }
            }
        }

        var grouped = entries
            .GroupBy(e => e.Term, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var lines = new List<string>
        {
            "# Glossary",
            string.Empty,
        };

        if (grouped.Count > 0)
        {
            lines.Add("| Term | Definition | Source |");
            lines.Add("|------|------------|--------|");

            foreach (var group in grouped.OrderBy(g => g.Key))
            {
                var term = EscapeCell(group.Key);
                var definition = EscapeCell(group.First().Definition);
                var sources = group.SelectMany(g => g.Sources)
                                   .DistinctBy(s => s.Link)
                                   .ToList();
                var sourceCol = string.Join(", ", sources.Select(s => $"[{EscapeCell(s.Name)}]({s.Link})"));
                lines.Add($"| {term} | {definition} | {sourceCol} |");
            }
        }
        else
        {
            lines.Add("No glossary terms found.");
        }

        lines.Add(string.Empty);
        lines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(glossaryDir, "index.md"), string.Join(Environment.NewLine, lines));
    }

    private async Task GenerateRecentChangesAsync(
        List<EaPackage> rootPackages,
        List<(EaElement Element, string PackageDir)> elements,
        string outputDir,
        Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var recentDir = Path.Combine(outputDir, "recent");
        await _writer.CreateDirectoryAsync(recentDir);

        const int topN = 50;

        var entries = new List<(string Name, string Type, DateTime? ModifiedDate, string Path)>();

        foreach (var (elem, pkgDir) in elements)
        {
            var elemName = SanitizeName(elem.Name);
            var link = Path.GetRelativePath(recentDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
            var path = BuildBreadcrumb(elem.PackageId, recentDir, outputDir, packageLookup);
            var modified = elem.ModifiedDate == DateTime.MinValue ? (DateTime?)null : elem.ModifiedDate;
            entries.Add(($"[{EscapeCell(elem.Name)}]({link})", EscapeCell(elem.Type), modified, path));
        }

        var diagrams = CollectDiagrams(rootPackages, outputDir);
        foreach (var (diagram, pkgDir) in diagrams)
        {
            var diagramPage = Path.GetRelativePath(recentDir, Path.Combine(pkgDir, "diagrams", $"{SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
            var path = BuildBreadcrumb(diagram.PackageId, recentDir, outputDir, packageLookup);
            DateTime? modified = null;
            if (!string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt))
                modified = dt;
            entries.Add(($"[{EscapeCell(diagram.Name)}]({diagramPage})", "Diagram", modified, path));
        }

        var sorted = entries
            .OrderByDescending(e => e.ModifiedDate.HasValue)
            .ThenByDescending(e => e.ModifiedDate)
            .Take(topN)
            .ToList();

        var lines = new List<string>
        {
            "# Recent Changes",
            string.Empty,
        };

        if (sorted.Count > 0)
        {
            lines.Add("| Name | Type | Modified | Path |");
            lines.Add("|------|------|----------|------|");

            foreach (var (name, type, modified, path) in sorted)
            {
                var modifiedStr = modified?.ToString("yyyy-MM-dd") ?? "-";
                lines.Add($"| {name} | {type} | {modifiedStr} | {path} |");
            }
        }
        else
        {
            lines.Add("No recent changes found.");
        }

        lines.Add(string.Empty);
        lines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(recentDir, "index.md"), string.Join(Environment.NewLine, lines));
    }

    private async Task WritePagesFileAsync(string outputDir)
    {
        var pagesPath = Path.Combine(outputDir, ".pages");
        var content = new List<string>
        {
            "nav:",
            "  - Structure: ''",
            "  - Diagrams: diagrams/",
            "  - Types: types/",
            "  - Glossary: glossary/",
            "  - Recent: recent/",
            string.Empty,
        };
        await _writer.WriteFileAsync(pagesPath, string.Join(Environment.NewLine, content));

        var diagramsPagesDir = Path.Combine(outputDir, "diagrams");
        await _writer.CreateDirectoryAsync(diagramsPagesDir);
        var diagramsPagesPath = Path.Combine(diagramsPagesDir, ".pages");
        var diagramsContent = new List<string>
        {
            "title: Diagrams",
            string.Empty,
        };
        await _writer.WriteFileAsync(diagramsPagesPath, string.Join(Environment.NewLine, diagramsContent));

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

    private async Task WriteExtraCssAsync(string outputDir)
    {
        var cssPath = Path.Combine(outputDir, "extra.css");
        var content = ".md-grid {\n  max-width: 75rem;\n}\n\n.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(1),\n.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(1) {\n  width: 22%;\n}\n\n.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(2),\n.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(2) {\n  width: 50%;\n}\n\n.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(3),\n.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(3) {\n  width: 28%;\n}\n\n.md-typeset table:not([class]):has(th:nth-child(4)) th:nth-child(2),\n.md-typeset table:not([class]):has(td:nth-child(4)) td:nth-child(2) {\n  width: 8%;\n}\n\n.md-typeset table:not([class]) td {\n  word-break: break-word;\n}\n";
        await _writer.WriteFileAsync(cssPath, content);
    }

    private async Task WriteElementAsync(EaElement element, string dir, string outputDir, List<(EaElement Element, string PackageDir)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup, Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> diagramIndex, Dictionary<int, List<(EaConnector Connector, int SourceId)>> incomingIndex)
    {
        var filePath = Path.Combine(dir, $"{SanitizeName(element.Name)}.md");
        if (element.ModifiedDate != DateTime.MinValue && File.Exists(filePath))
        {
            var fileTime = File.GetLastWriteTimeUtc(filePath);
            var elementTime = element.ModifiedDate.Kind == DateTimeKind.Utc
                ? element.ModifiedDate
                : element.ModifiedDate.ToUniversalTime();
            if (fileTime >= elementTime)
            {
                _logger.LogDebug("Skipped {ElementName}", element.Name);
                return;
            }
        }

        var isNew = !File.Exists(filePath);
        _logger.LogInformation("{Action} {ElementName}", isNew ? "Created" : "Updated", element.Name);
        var createdStr = element.CreatedDate?.ToString("yyyy-MM-dd") ?? "-";
        var modifiedStr = element.ModifiedDate == DateTime.MinValue ? "-" : element.ModifiedDate.ToString("yyyy-MM-dd");
        var lines = new List<string>
        {
            $"# {element.Name}",
            string.Empty,
            $"**Type:** {element.Type}  **Stereotype:** {element.Stereotype}  " +
            (string.IsNullOrEmpty(element.Status) ? "" : $"**Status:** {element.Status}  "),
            $"**Created:** {createdStr}  **Modified:** {modifiedStr}",
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
                var desc = (attr.Notes ?? "").Replace("|", "\\|").Replace("\n", "<br/>");
                lines.Add($"| {EscapeCell(attr.Name)} | {EscapeCell(attr.Type)} | {EscapeCell(attr.DefaultValue ?? "")} | {desc} |");
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
                lines.Add($"| {EscapeCell(tv.Name)} | {EscapeCell(tv.Value)} | {EscapeCell(tv.Notes ?? "")} |");
            }

            lines.Add(string.Empty);
        }

        if (element.Connectors.Count > 0)
        {
            lines.Add("## Relationships");
            lines.Add(string.Empty);
            lines.Add("| Type | Stereotype | Connected To |");
            lines.Add("|------|------------|-------------|");

            var lookup = elements.GroupBy(e => e.Element.Id)
                                 .ToDictionary(g => g.Key, g => g.First());

            foreach (var conn in element.Connectors)
            {
                var otherId = conn.SourceId == element.Id ? conn.TargetId
                    : conn.TargetId == element.Id ? conn.SourceId
                    : -1;

                if (otherId <= 0) continue;

                string connectedTo;
                if (lookup.TryGetValue(otherId, out var other))
                {
                    var otherName = SanitizeName(other.Element.Name);
                    var relativePath = Path.GetRelativePath(dir, Path.Combine(other.PackageDir, $"{otherName}.md")).Replace('\\', '/');
                    connectedTo = $"[{EscapeCell(other.Element.Name)}]({relativePath})";
                }
                else
                {
                    connectedTo = $"Element ID {otherId} (not in export)";
                }

                lines.Add($"| {EscapeCell(conn.Type)} | {EscapeCell(conn.Stereotype)} | {connectedTo} |");
            }

            lines.Add(string.Empty);
        }

        if (diagramIndex.TryGetValue(element.Id, out var elementDiagrams))
        {
            lines.Add("### Appears on Diagrams");
            lines.Add(string.Empty);
            foreach (var (diagram, pkgDir) in elementDiagrams)
            {
                var diagDir = Path.Combine(pkgDir, "diagrams");
                var diagLink = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
                lines.Add($"- [{diagram.Name}]({diagLink})");
            }
            lines.Add(string.Empty);
        }

        if (incomingIndex.TryGetValue(element.Id, out var incomingConns))
        {
            lines.Add("### Referenced By");
            lines.Add(string.Empty);
            lines.Add("| Type | Stereotype | Source |");
            lines.Add("|------|------------|--------|");
            var lookup = elements.GroupBy(e => e.Element.Id)
                                 .ToDictionary(g => g.Key, g => g.First());
            foreach (var (conn, sourceId) in incomingConns)
            {
                string source;
                if (lookup.TryGetValue(sourceId, out var srcElem))
                {
                    var srcName = SanitizeName(srcElem.Element.Name);
                    var relativePath = Path.GetRelativePath(dir, Path.Combine(srcElem.PackageDir, $"{srcName}.md")).Replace('\\', '/');
                    source = $"[{EscapeCell(srcElem.Element.Name)}]({relativePath})";
                }
                else
                {
                    source = $"Element ID {sourceId} (not in export)";
                }
                lines.Add($"| {EscapeCell(conn.Type)} | {EscapeCell(conn.Stereotype)} | {source} |");
            }
            lines.Add(string.Empty);
        }

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

                if (File.Exists(pngPath))
                {
                    lines.Add($"![{diagram.Name}]({fileName}.png)");
                    lines.Add(string.Empty);
                }

                lines.Add($"**Description:** {diagram.Notes ?? "-"}");
                lines.Add(string.Empty);

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

                    foreach (var (elemEa, elemDir) in diagramElements.OrderBy(e => e.Element.Name))
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

    private async Task WriteDiagramsIndexAsync(List<EaPackage> rootPackages, string outputDir, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var diagramsDir = Path.Combine(outputDir, "diagrams");
        await _writer.CreateDirectoryAsync(diagramsDir);

        var allDiagrams = CollectDiagrams(rootPackages, outputDir);

        var sorted = allDiagrams
            .Select(d => (d.Diagram, d.PackageDir, Path: BuildBreadcrumb(d.Diagram.PackageId, diagramsDir, outputDir, packageLookup)))
            .OrderBy(d => d.Path)
            .ThenBy(d => d.Diagram.Name)
            .ToList();

        var lines = new List<string>
        {
            "# Diagrams",
            string.Empty,
        };

        if (sorted.Count == 0)
        {
            lines.Add("No diagrams found in model.");
        }
        else
        {
            lines.Add("| Diagram | Modified | Description | Path |");
            lines.Add("|---------|----------|-------------|------|");

            foreach (var (diagram, pkgDir, path) in sorted)
            {
                var diagramPage = Path.GetRelativePath(diagramsDir, Path.Combine(pkgDir, "diagrams", $"{SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
                var link = $"[{EscapeCell(diagram.Name)}]({diagramPage})";

                var modified = !string.IsNullOrWhiteSpace(diagram.ModifiedDate) && DateTime.TryParse(diagram.ModifiedDate, out var dt)
                    ? dt.ToString("yyyy-MM-dd")
                    : "-";

                var desc = string.IsNullOrWhiteSpace(diagram.Notes) ? "-" : EscapeCell(diagram.Notes.Trim());
                if (desc.Length > 100) desc = desc[..100] + "...";

                lines.Add($"| {link} | {modified} | {desc} | {path} |");
            }
        }

        lines.Add(string.Empty);
        lines.Add(FormatTimestamp());
        await _writer.WriteFileAsync(Path.Combine(diagramsDir, "index.md"), string.Join(Environment.NewLine, lines));
    }

    private static async Task CleanupOrphanedFilesAsync(List<(EaElement Element, string PackageDir)> elements)
    {
        var expectedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (element, pkgDir) in elements)
        {
            expectedFiles.Add(Path.Combine(pkgDir, $"{SanitizeName(element.Name)}.md"));
        }

        var packageDirs = elements.Select(e => e.PackageDir).Distinct().ToList();
        foreach (var dir in packageDirs)
        {
            if (!Directory.Exists(dir)) continue;
            foreach (var file in Directory.EnumerateFiles(dir, "*.md"))
            {
                if (Path.GetFileName(file).Equals("index.md", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!expectedFiles.Contains(file))
                {
                    File.Delete(file);
                }
            }
        }
        await Task.CompletedTask;
    }

    private static string FormatTimestamp()
    {
        return $"---\n\n*Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";
    }

    private static string EscapeCell(string raw)
    {
        return raw.Replace("|", "\\|").ReplaceLineEndings(" ");
    }

    private static string CreateElementLink(EaElement element, string pkgDir, string fromDir)
    {
        var elemName = SanitizeName(element.Name);
        return Path.GetRelativePath(fromDir, Path.Combine(pkgDir, $"{elemName}.md")).Replace('\\', '/');
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
