using System.Text.RegularExpressions;
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

        // Build a map of element name → link for cross-linking glossary definitions.
        var termLinkMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            if (!termLinkMap.ContainsKey(elem.Name))
                termLinkMap[elem.Name] = MarkdownHelpers.CreateElementLink(elem, pkgDir, glossaryDir);
        }
        var sortedTerms = termLinkMap.Keys.OrderByDescending(t => t.Length).ToList();

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            foreach (var tv in elem.TaggedValues)
            {
                if (!string.IsNullOrWhiteSpace(tv.Value) &&
                    (string.Equals(tv.Name, "Definition", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(tv.Name, "Glossary", StringComparison.OrdinalIgnoreCase)))
                {
                    var link = MarkdownHelpers.CreateElementLink(elem, pkgDir, glossaryDir);
                    entries.Add((elem.Name, tv.Value, [(elem.Name, link)]));
                }
            }

            if (!string.IsNullOrWhiteSpace(elem.Notes))
            {
                var firstDot = elem.Notes.IndexOf('.');
                var sentence = firstDot >= 0 ? elem.Notes[..firstDot].Trim() : elem.Notes.Trim();
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
                var definition = LinkTermsInDefinition(group.First().Definition, termLinkMap, sortedTerms);
                definition = MarkdownHelpers.EscapeCell(definition);
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

    private static string LinkTermsInDefinition(string text, Dictionary<string, string> termLinkMap, List<string> sortedTerms)
    {
        var result = text;
        foreach (var term in sortedTerms)
        {
            var escapedTerm = Regex.Escape(term);
            result = Regex.Replace(result, $@"\b{escapedTerm}\b",
                m => $"[{MarkdownHelpers.EscapeCell(m.Value)}]({termLinkMap[term]})");
        }
        return result;
    }
}
