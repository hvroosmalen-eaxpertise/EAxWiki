using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;

namespace EAxWiki.Export;

public class MarkdownExporter : IWikiExporter
{
    private readonly IOutputWriter _writer;

    public MarkdownExporter(IOutputWriter writer)
    {
        _writer = writer;
    }

    public async Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath)
    {
        var packages = startPackage != null
            ? new List<EaPackage> { startPackage }
            : repository.RootPackages;

        foreach (var pkg in packages)
        {
            await ExportPackageAsync(pkg, outputPath);
        }
    }

    private async Task ExportPackageAsync(EaPackage package, string outputDir)
    {
        var dir = Path.Combine(outputDir, SanitizeName(package.Name));
        await _writer.CreateDirectoryAsync(dir);

        var indexLines = new List<string>
        {
            $"# {package.Name}",
            string.Empty
        };

        if (!string.IsNullOrWhiteSpace(package.Notes))
        {
            indexLines.Add(package.Notes);
            indexLines.Add(string.Empty);
        }

        if (package.Elements.Count > 0)
        {
            indexLines.Add("## Elements");
            indexLines.Add(string.Empty);

            foreach (var elem in package.Elements)
            {
                var elemFile = $"{SanitizeName(elem.Name)}.md";
                await WriteElementAsync(elem, dir);

                var typeLabel = string.IsNullOrEmpty(elem.Stereotype)
                    ? elem.Type
                    : $"{elem.Stereotype} «{elem.Type}»";
                indexLines.Add($"- [{elem.Name}]({elemFile}) — {typeLabel}");
            }

            indexLines.Add(string.Empty);
        }

        if (package.Diagrams.Count > 0)
        {
            indexLines.Add("## Diagrams");
            indexLines.Add(string.Empty);

            foreach (var diag in package.Diagrams)
            {
                indexLines.Add($"- {diag.Name} ({diag.Type})");
            }

            indexLines.Add(string.Empty);
        }

        if (package.Children.Count > 0)
        {
            indexLines.Add("## Sub-packages");
            indexLines.Add(string.Empty);

            foreach (var child in package.Children)
            {
                indexLines.Add($"- [{child.Name}]({SanitizeName(child.Name)}/index.md)");
            }

            indexLines.Add(string.Empty);
        }

        await _writer.WriteFileAsync(Path.Combine(dir, "index.md"), string.Join(Environment.NewLine, indexLines));

        foreach (var child in package.Children)
        {
            await ExportPackageAsync(child, outputDir);
        }
    }

    private async Task WriteElementAsync(EaElement element, string dir)
    {
        var lines = new List<string>
        {
            $"# {element.Name}",
            string.Empty,
            $"**Type:** {element.Type}  ",
            $"**Stereotype:** {element.Stereotype}  ",
            string.Empty
        };

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
        await _writer.WriteFileAsync(filePath, string.Join(Environment.NewLine, lines));
    }

    private static string SanitizeName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "unnamed" : sanitized;
    }
}
