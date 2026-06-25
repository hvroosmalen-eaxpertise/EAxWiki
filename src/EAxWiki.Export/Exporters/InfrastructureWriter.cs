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

    public static async Task CleanupOrphanedFilesAsync(ExportContext ctx)
    {
        if (!Directory.Exists(ctx.OutputPath))
        {
            await Task.CompletedTask;
            return;
        }

        var expectedFiles = new HashSet<string>(ctx.RegisteredElementFiles, StringComparer.OrdinalIgnoreCase);

        // Root-level dirs managed by infrastructure — never treated as orphans.
        var specialDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine(ctx.OutputPath, "diagrams"),
            Path.Combine(ctx.OutputPath, "types"),
            Path.Combine(ctx.OutputPath, "glossary"),
            Path.Combine(ctx.OutputPath, "recent"),
        };

        CleanupDirectory(ctx.OutputPath, ctx.AllPackageDirs, expectedFiles, specialDirs, isRoot: true);

        await Task.CompletedTask;
    }

    private static void CleanupDirectory(
        string dir,
        HashSet<string> expectedDirs,
        HashSet<string> expectedFiles,
        HashSet<string> specialDirs,
        bool isRoot)
    {
        // Clean up orphaned element .md files in this directory (skip index.md).
        if (!isRoot)
        {
            foreach (var file in Directory.EnumerateFiles(dir, "*.md"))
            {
                if (Path.GetFileName(file).Equals("index.md", StringComparison.OrdinalIgnoreCase)) continue;
                if (!expectedFiles.Contains(file))
                    File.Delete(file);
            }
        }

        // Recurse into subdirectories; delete any that are not in the expected model dirs.
        foreach (var subDir in Directory.EnumerateDirectories(dir))
        {
            if (specialDirs.Contains(subDir)) continue;

            if (!expectedDirs.Contains(subDir))
            {
                // Orphaned package directory (renamed or emptied) — remove entirely.
                Directory.Delete(subDir, recursive: true);
            }
            else
            {
                CleanupDirectory(subDir, expectedDirs, expectedFiles, specialDirs, isRoot: false);
            }
        }
    }
}
