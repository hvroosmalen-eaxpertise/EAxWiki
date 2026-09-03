using System.Reflection;
using System.Threading;
using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class InfrastructureWriter(IOutputWriter writer)
{
    public async Task WritePagesFileAsync(string outputDir, IReadOnlyList<string>? rootPackageDirs = null, CancellationToken ct = default)
    {
        rootPackageDirs ??= [];

        // wiki/status/health.md is written by EAxWiki.Monitor (issue #37/#38),
        // not by this exporter — only add its nav entry when it actually exists, so a plain export.ps1
        // run (no monitor wrapper in use) doesn't get a link to a missing page.
        //
        // The Pipeline Health entry deliberately points at "status/health.html", not "status/health.md":
        // mkdocs-awesome-pages-plugin has a bug (confirmed empirically, not just in this project's own
        // .pages files — reproduced with a minimal mkdocs.yml + plugins: [awesome-pages] config) where an
        // explicit nav entry referencing a second .md file in an already-referenced directory renders with
        // the source .md extension as the href instead of being resolved to .html, while plain mkdocs nav
        // (no awesome-pages) resolves the exact same entry correctly. A bare .html reference isn't matched
        // against any known page by the plugin at all, so it passes through as a literal relative link —
        // and since use_directory_urls: false means the built file genuinely exists at that literal path,
        // the link just works. The main "Status: status/" entry is unaffected (a directory reference was
        // already the working, unchanged pattern here) and still resolves via directory-index serving.
        // Model Health (status/model-health.md) is written unconditionally by ModelHealthExporter as
        // part of every export, unlike status/health.md above which only exists when the monitor
        // wrapper is in use — so its nav entry doesn't need the same existence check. It hits the same
        // awesome-pages .html-vs-.md quirk as Pipeline Health (a second .md file in an
        // already-referenced directory), so it's referenced the same way.
        var statusLines = new List<string>
        {
            "  - Status: status/",
            "  - Model Health: status/model-health.html",
        };
        if (File.Exists(Path.Combine(outputDir, "status", "health.md")))
            statusLines.Insert(1, "  - Pipeline Health: status/health.html");
        if (File.Exists(Path.Combine(outputDir, "status", "errors.md")))
            statusLines.Add("  - Error Log: status/errors.html");
        if (File.Exists(Path.Combine(outputDir, "status", "config.md")))
            statusLines.Add("  - Configuration: status/config.html");

        // Issue #89 item 4: Repository becomes a collapsible nav section that groups the root
        // packages, instead of a bare link to the site home page. Awesome-pages nests entries
        // when their `nav:` key has no value and the next lines are indented. Empty
        // rootPackageDirs falls back to the old bare-link form (used by StatusPagesNavTests
        // and by direct-caller code paths that don't have the package list handy).
        List<string> repositoryLines;
        if (rootPackageDirs.Count == 0)
        {
            repositoryLines = ["  - Repository: ''"];
        }
        else
        {
            repositoryLines = ["  - Repository:"];
            foreach (var dir in rootPackageDirs)
                repositoryLines.Add($"    - {dir}");
        }

        await writer.WriteFileAsync(Path.Combine(outputDir, ".pages"), string.Join(Environment.NewLine,
        [
            "nav:",
            .. repositoryLines,
            "  - Diagrams: diagrams/",
            "  - Element Types: types/",
            "  - Glossary: glossary/",
            "  - Recent: recent/",
            .. statusLines,
            string.Empty,
        ]), ct);

        var diagramsDir = Path.Combine(outputDir, "diagrams");
        await writer.CreateDirectoryAsync(diagramsDir, ct);
        await writer.WriteFileAsync(Path.Combine(diagramsDir, ".pages"),
            string.Join(Environment.NewLine, ["title: \U0001F5FA️ Diagrams", string.Empty]), ct);

        var typesDir = Path.Combine(outputDir, "types");
        await writer.CreateDirectoryAsync(typesDir, ct);
        await writer.WriteFileAsync(Path.Combine(typesDir, ".pages"),
            string.Join(Environment.NewLine, ["title: Element Types", string.Empty]), ct);
    }


    public async Task WriteGraphScriptsAsync(string outputDir, string brand, CancellationToken ct = default)
    {
        // Extract embedded cytoscape.min.js to the wiki output so it works offline.
        await writer.WriteFileAsync(Path.Combine(outputDir, "cytoscape.min.js"),
            EmbeddedResource.ReadText("cytoscape.min.js"), ct);

        var (layerColors, darkText) = brand == "eursura"
            ? (new Dictionary<string, string>
               {
                   ["business"] = "#A8C6C7",
                   ["application"] = "#103135",
                   ["technology"] = "#C4E5E7",
                   ["physical"] = "#6FB4B6",
                   ["motivation"] = "#D0F391",
                   ["strategy"] = "#7FA8A9",
                   ["implementation"] = "#5C8A8B",
                   ["composite"] = "#405B5C",
                   ["uml"] = "#F3F7F7",
                   ["edgy-id"] = "#75F0A5",
                   ["edgy-ar"] = "#9DB9F6",
                   ["edgy-ex"] = "#F985B4",
                   ["edgy-ix"] = "#4ECDC4",
                   ["edgy-pe"] = "#FFD93D",
                   ["edgy-lb"] = "#E8E8E8",
               },
               new Dictionary<string, bool> { ["business"] = true, ["technology"] = true, ["physical"] = true, ["motivation"] = true, ["strategy"] = true, ["uml"] = true, ["edgy-id"] = true, ["edgy-pe"] = true, ["edgy-lb"] = true })
            : (new Dictionary<string, string>
               {
                   ["business"] = "#D4A017",
                   ["application"] = "#2E86C1",
                   ["technology"] = "#27AE60",
                   ["physical"] = "#17A589",
                   ["motivation"] = "#8E44AD",
                   ["strategy"] = "#A0682B",
                   ["implementation"] = "#D84B79",
                   ["composite"] = "#5D6D7E",
                   ["uml"] = "#7F8C8D",
                   ["edgy-id"] = "#75F0A5",
                   ["edgy-ar"] = "#9DB9F6",
                   ["edgy-ex"] = "#F985B4",
                   ["edgy-ix"] = "#4ECDC4",
                   ["edgy-pe"] = "#FFD93D",
                   ["edgy-lb"] = "#E8E8E8",
               },
               new Dictionary<string, bool> { ["edgy-id"] = true, ["edgy-pe"] = true, ["edgy-lb"] = true, ["business"] = true });

        string SerializeColors(Dictionary<string, string> map) =>
            string.Join(",\n", map.Select(kv => $"    '{kv.Key}':{new string(' ', 15 - kv.Key.Length)}'{kv.Value}'"));

        string SerializeDarkText(Dictionary<string, bool> map) =>
            string.Join(", ", map.Select(kv => $"'{kv.Key}': {kv.Value.ToString().ToLowerInvariant()}"));

        var graphInitJs = EmbeddedResource.ReadText("graph-init.js.tmpl")
            .Replace("/*EA_LAYER_COLORS*/", "{\n" + SerializeColors(layerColors) + "\n}")
            .Replace("/*EA_LAYER_DARK_TEXT*/", "{ " + SerializeDarkText(darkText) + " }");

        await writer.WriteFileAsync(Path.Combine(outputDir, "graph-init.js"), graphInitJs, ct);
    }

    // The five editor / helper scripts live as-is in Resources/*.js — no per-brand substitution,
    // so extracting them out of C# raw-strings is pure ergonomics: JS files get syntax highlighting,
    // linting, and a real diff view. See issue #85 for the follow-up dedupe of shared helpers.

    public Task WriteStatusEditorScriptAsync(string outputDir, CancellationToken ct = default) =>
        writer.WriteFileAsync(Path.Combine(outputDir, "status-editor.js"), EmbeddedResource.ReadText("status-editor.js"), ct);

    public Task WriteNotesEditorScriptAsync(string outputDir, CancellationToken ct = default) =>
        writer.WriteFileAsync(Path.Combine(outputDir, "notes-editor.js"), EmbeddedResource.ReadText("notes-editor.js"), ct);

    public Task WriteRowNotesEditorScriptAsync(string outputDir, CancellationToken ct = default) =>
        writer.WriteFileAsync(Path.Combine(outputDir, "row-notes-editor.js"), EmbeddedResource.ReadText("row-notes-editor.js"), ct);

    public Task WriteIconsScriptAsync(string outputDir, CancellationToken ct = default) =>
        writer.WriteFileAsync(Path.Combine(outputDir, "ea-icons.js"), EmbeddedResource.ReadText("ea-icons.js"), ct);

    public Task WriteApiProbeScriptAsync(string outputDir, CancellationToken ct = default) =>
        writer.WriteFileAsync(Path.Combine(outputDir, "api-probe.js"), EmbeddedResource.ReadText("api-probe.js"), ct);

    public async Task WriteExtraCssAsync(string outputDir, CancellationToken ct = default)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("extra.css", StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        var css = await reader.ReadToEndAsync();
        await writer.WriteFileAsync(Path.Combine(outputDir, "extra.css"), css, ct);
    }

    public async Task WriteBrandAssetsAsync(string outputDir, string brand, CancellationToken ct = default)
    {
        if (brand != "eursura") return;

        var assembly = Assembly.GetExecutingAssembly();

        var cssResource = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("brand-eursura.css", StringComparison.OrdinalIgnoreCase));
        using var cssStream = assembly.GetManifestResourceStream(cssResource)!;
        using var cssReader = new StreamReader(cssStream);
        var css = await cssReader.ReadToEndAsync(ct);
        await writer.WriteFileAsync(Path.Combine(outputDir, "brand.css"), css, ct);

        var pngResource = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("eursura-logo.png", StringComparison.OrdinalIgnoreCase));
        using var pngStream = assembly.GetManifestResourceStream(pngResource)!;
        var pngBytes = new byte[pngStream.Length];
        await pngStream.ReadAsync(pngBytes, ct);
        Directory.CreateDirectory(Path.Combine(outputDir, "assets"));
        await File.WriteAllBytesAsync(Path.Combine(outputDir, "assets", "eursura-logo.png"), pngBytes, ct);
    }

    public static async Task CleanupOrphanedFilesAsync(ExportContext ctx, CancellationToken ct = default)
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
            Path.Combine(ctx.OutputPath, "status"),
            Path.Combine(ctx.OutputPath, "assets"),
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
        // Clean up orphaned element .md files in this directory (skip index.md and diagrams/).
        if (!isRoot && !string.Equals(Path.GetFileName(dir), "diagrams", StringComparison.OrdinalIgnoreCase))
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
                // Keep diagrams/ subdirectories — they contain generated diagram pages, not orphaned packages.
                if (string.Equals(Path.GetFileName(subDir), "diagrams", StringComparison.OrdinalIgnoreCase))
                    continue;

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