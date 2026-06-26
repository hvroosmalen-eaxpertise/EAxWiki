using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class TypesExporter(IOutputWriter writer, ILogger logger)
{
    public async Task ExportAsync(ExportContext ctx)
    {
        var outputDir = ctx.OutputPath;
        var typesDir = Path.Combine(outputDir, "types");
        await writer.CreateDirectoryAsync(typesDir);

        var parsed = ctx.Elements
            .Select(e =>
            {
                var fq = e.Element.FQStereotype;
                var ex = e.Element.StereotypeEx;
                var stereotypeSrc = !string.IsNullOrWhiteSpace(fq) ? fq
                    : !string.IsNullOrWhiteSpace(ex) ? ex
                    : e.Element.Stereotype;
                var (language, type) = MarkdownHelpers.ParseStereotype(stereotypeSrc);
                return (e.Element, e.PackageDir, Language: language, Type: type);
            })
            .ToList();

        var languages = parsed.Select(x => x.Language).Distinct().OrderBy(l => l).ToList();
        logger.LogInformation("Generating type pages across {LanguageCount} languages", languages.Count);

        var indexLines = new List<string>
        {
            "# Types",
            string.Empty,
            "Elements grouped by modelling language:",
            string.Empty,
        };

        foreach (var lang in languages)
            indexLines.Add($"- [{lang}]({MarkdownHelpers.SanitizeName(lang)}/index.md)");

        indexLines.Add(string.Empty);
        indexLines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(typesDir, "index.md"), string.Join(Environment.NewLine, indexLines));

        foreach (var lang in languages)
        {
            var langDir = Path.Combine(typesDir, MarkdownHelpers.SanitizeName(lang));
            await writer.CreateDirectoryAsync(langDir);

            await writer.WriteFileAsync(Path.Combine(langDir, ".pages"),
                string.Join(Environment.NewLine, [$"title: {lang}", string.Empty]));

            var typeGroups = parsed
                .Where(x => x.Language == lang)
                .GroupBy(x => x.Type)
                .OrderBy(g => g.Key)
                .ToList();

            var langIndexLines = new List<string>
            {
                $"# {lang}",
                string.Empty,
                $"{typeGroups.Sum(g => g.Count())} element(s) across {typeGroups.Count} type(s):",
                string.Empty,
            };

            foreach (var group in typeGroups)
                langIndexLines.Add($"- [{group.Key}]({MarkdownHelpers.SanitizeName(group.Key)}.md)");

            langIndexLines.Add(string.Empty);
            langIndexLines.Add(MarkdownHelpers.FormatTimestamp());
            await writer.WriteFileAsync(Path.Combine(langDir, "index.md"), string.Join(Environment.NewLine, langIndexLines));

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
                    var elemName = MarkdownHelpers.SanitizeName(item.Element.Name);
                    var relativeDir = Path.GetRelativePath(outputDir, item.PackageDir);
                    var relativePath = $"../../{relativeDir}/{elemName}.md".Replace('\\', '/');
                    lines.Add($"- {MarkdownHelpers.GetStereotypeLabel(item.Element)} [{item.Element.Name}]({relativePath})");
                }

                lines.Add(string.Empty);
                lines.Add(MarkdownHelpers.FormatTimestamp());
                await writer.WriteFileAsync(Path.Combine(langDir, $"{MarkdownHelpers.SanitizeName(group.Key)}.md"), string.Join(Environment.NewLine, lines));
            }
        }

        logger.LogInformation("Generated type pages across {LanguageCount} languages", languages.Count);
    }
}
