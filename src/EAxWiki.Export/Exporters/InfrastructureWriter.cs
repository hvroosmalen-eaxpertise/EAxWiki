using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class InfrastructureWriter(IOutputWriter writer)
{
    public async Task WritePagesFileAsync(string outputDir)
    {
        await writer.WriteFileAsync(Path.Combine(outputDir, ".pages"), string.Join(Environment.NewLine,
        [
            "nav:",
            "  - Structure: ''",
            "  - Diagrams: diagrams/",
            "  - Types: types/",
            "  - Glossary: glossary/",
            "  - Recent: recent/",
            string.Empty,
        ]));

        var diagramsDir = Path.Combine(outputDir, "diagrams");
        await writer.CreateDirectoryAsync(diagramsDir);
        await writer.WriteFileAsync(Path.Combine(diagramsDir, ".pages"),
            string.Join(Environment.NewLine, ["title: Diagrams", string.Empty]));

        var typesDir = Path.Combine(outputDir, "types");
        await writer.CreateDirectoryAsync(typesDir);
        await writer.WriteFileAsync(Path.Combine(typesDir, ".pages"),
            string.Join(Environment.NewLine, ["title: Types", string.Empty]));
    }

    public async Task WriteExtraCssAsync(string outputDir)
    {
        var content = ".md-grid {\n  max-width: 75rem;\n}\n\n.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(1),\n.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(1) {\n  width: 22%;\n}\n\n.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(2),\n.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(2) {\n  width: 50%;\n}\n\n.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(3),\n.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(3) {\n  width: 28%;\n}\n\n.md-typeset table:not([class]):has(th:nth-child(4)) th:nth-child(2),\n.md-typeset table:not([class]):has(td:nth-child(4)) td:nth-child(2) {\n  width: 8%;\n}\n\n.md-typeset table:not([class]) td {\n  word-break: break-word;\n}\n";
        await writer.WriteFileAsync(Path.Combine(outputDir, "extra.css"), content);
    }

    public static async Task CleanupOrphanedFilesAsync(List<(EAxWiki.Core.Models.EaElement Element, string PackageDir)> elements)
    {
        var expectedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (element, pkgDir) in elements)
            expectedFiles.Add(Path.Combine(pkgDir, $"{MarkdownHelpers.SanitizeName(element.Name)}.md"));

        foreach (var dir in elements.Select(e => e.PackageDir).Distinct())
        {
            if (!Directory.Exists(dir)) continue;
            foreach (var file in Directory.EnumerateFiles(dir, "*.md"))
            {
                if (Path.GetFileName(file).Equals("index.md", StringComparison.OrdinalIgnoreCase)) continue;
                if (!expectedFiles.Contains(file))
                    File.Delete(file);
            }
        }

        await Task.CompletedTask;
    }
}
