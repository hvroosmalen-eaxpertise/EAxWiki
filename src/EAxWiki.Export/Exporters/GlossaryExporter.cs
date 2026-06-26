using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class GlossaryExporter(IOutputWriter writer)
{
    public async Task ExportAsync(ExportContext ctx)
    {
        var glossaryDir = Path.Combine(ctx.OutputPath, "glossary");
        await writer.CreateDirectoryAsync(glossaryDir);

        var entries = new List<(string Term, string Definition, List<(string Name, string Link)> Sources)>();

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            foreach (var tv in elem.TaggedValues)
            {
                if (!string.IsNullOrWhiteSpace(tv.Value) &&
                    (string.Equals(tv.Name, "Definition", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(tv.Name, "Glossary", StringComparison.OrdinalIgnoreCase)))
                {
                    var link = MarkdownHelpers.CreateElementLink(elem, pkgDir, glossaryDir);
                    entries.Add((elem.Name, MarkdownHelpers.StripHtml(tv.Value), [(elem.Name, link)]));
                }
            }

            if (!string.IsNullOrWhiteSpace(elem.Notes))
            {
                var plain = MarkdownHelpers.StripHtml(elem.Notes);
                var firstDot = plain.IndexOf('.');
                var sentence = firstDot >= 0 ? plain[..firstDot].Trim() : plain.Trim();
                if (sentence.Length >= 20)
                {
                    var link = MarkdownHelpers.CreateElementLink(elem, pkgDir, glossaryDir);
                    entries.Add((elem.Name, sentence, [(elem.Name, link)]));
                }
            }
        }

        var grouped = entries.GroupBy(e => e.Term, StringComparer.OrdinalIgnoreCase).ToList();

        var lines = new List<string> { "# Glossary", string.Empty };

        if (grouped.Count > 0)
        {
            lines.Add("| Term | Definition | Source |");
            lines.Add("|------|------------|--------|");

            foreach (var group in grouped.OrderBy(g => g.Key))
            {
                var term = MarkdownHelpers.EscapeCell(group.Key);
                var definition = MarkdownHelpers.EscapeCell(group.First().Definition);
                var sources = group.SelectMany(g => g.Sources).DistinctBy(s => s.Link).ToList();
                var sourceCol = string.Join(", ", sources.Select(s => $"[{MarkdownHelpers.EscapeCell(s.Name)}]({s.Link})"));
                lines.Add($"| {term} | {definition} | {sourceCol} |");
            }
        }
        else
        {
            lines.Add("No glossary terms found.");
        }

        lines.Add(string.Empty);
        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(Path.Combine(glossaryDir, "index.md"), string.Join(Environment.NewLine, lines));
    }
}
