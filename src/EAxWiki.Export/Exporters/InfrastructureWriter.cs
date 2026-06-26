using System.Reflection;
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

    public async Task WriteGraphScriptsAsync(string outputDir)
    {
        // Extract embedded cytoscape.min.js to the wiki output so it works offline.
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("cytoscape.min.js", StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        var cytoscapeJs = await reader.ReadToEndAsync();
        await writer.WriteFileAsync(Path.Combine(outputDir, "cytoscape.min.js"), cytoscapeJs);

        const string graphInitJs = """
(function () {
    var container = document.getElementById('ea-graph-container');
    if (!container || !window.eaGraphData || typeof cytoscape === 'undefined') return;
    var data = window.eaGraphData;
    var cy = cytoscape({
        container: container,
        elements: {
            nodes: data.nodes.map(function (n) { return { data: n }; }),
            edges: data.edges.map(function (e) { return { data: e }; })
        },
        style: [
            {
                selector: 'node',
                style: {
                    'label': 'data(label)',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'text-wrap': 'wrap',
                    'text-max-width': '90px',
                    'font-size': '11px',
                    'width': 'label',
                    'height': 'label',
                    'padding': '10px',
                    'shape': 'round-rectangle',
                    'background-color': '#1565c0',
                    'color': '#ffffff'
                }
            },
            {
                selector: 'node[?isFocal]',
                style: {
                    'background-color': '#e65100',
                    'border-width': 3,
                    'border-color': '#bf360c',
                    'font-weight': 'bold'
                }
            },
            {
                selector: 'node[!hasUrl]',
                style: { 'background-color': '#607d8b' }
            },
            {
                selector: 'edge',
                style: {
                    'label': 'data(label)',
                    'font-size': '10px',
                    'curve-style': 'bezier',
                    'target-arrow-shape': 'triangle',
                    'target-arrow-color': '#90a4ae',
                    'line-color': '#90a4ae',
                    'color': '#555',
                    'text-background-opacity': 1,
                    'text-background-color': '#f5f5f5',
                    'text-background-padding': '2px',
                    'text-background-shape': 'round-rectangle'
                }
            }
        ],
        layout: {
            name: 'cose',
            animate: false,
            randomize: false,
            nodeRepulsion: function () { return 400000; },
            nodeOverlap: 20,
            idealEdgeLength: function () { return 120; },
            gravity: 80
        },
        minZoom: 0.2,
        maxZoom: 3
    });

    cy.fit(cy.elements(), 40);

    var tooltip = document.createElement('div');
    tooltip.style.cssText = 'position:fixed;background:#fff;border:1px solid #ddd;border-radius:6px;padding:8px 12px;font-size:12px;pointer-events:none;display:none;box-shadow:0 4px 12px rgba(0,0,0,.15);z-index:9999;max-width:240px;line-height:1.6;';
    document.body.appendChild(tooltip);

    cy.on('mouseover', 'node', function (evt) {
        var d = evt.target.data();
        var html = '<strong>' + d.fullName + '</strong>';
        if (d.packageName) html += '<br><span style="color:#777;font-size:11px">' + d.packageName + '</span>';
        if (d.hasUrl) html += '<br><span style="color:#1565c0;font-size:11px">→ click to open</span>';
        tooltip.innerHTML = html;
        tooltip.style.display = 'block';
    });
    cy.on('mousemove', function (evt) {
        if (tooltip.style.display === 'none') return;
        tooltip.style.left = (evt.originalEvent.clientX + 14) + 'px';
        tooltip.style.top = (evt.originalEvent.clientY - 10) + 'px';
    });
    cy.on('mouseout', 'node', function () { tooltip.style.display = 'none'; });
    cy.on('tap', 'node', function (evt) {
        var url = evt.target.data('url');
        if (url) window.location.href = url;
    });
    cy.on('mouseover', 'node[?hasUrl]', function () { container.style.cursor = 'pointer'; });
    cy.on('mouseout', 'node', function () { container.style.cursor = 'default'; });
})();
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "graph-init.js"), graphInitJs);
    }

    public async Task WriteExtraCssAsync(string outputDir)
    {
        var content = """
.md-grid {
  max-width: 75rem;
}

.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(1),
.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(1) {
  width: 22%;
}

.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(2),
.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(2) {
  width: 50%;
}

.md-typeset table:not([class]):not(:has(th:nth-child(4))) th:nth-child(3),
.md-typeset table:not([class]):not(:has(td:nth-child(4))) td:nth-child(3) {
  width: 28%;
}

.md-typeset table:not([class]):has(th:nth-child(4)) th:nth-child(2),
.md-typeset table:not([class]):has(td:nth-child(4)) td:nth-child(2) {
  width: 8%;
}

.md-typeset table:not([class]) td {
  word-break: break-word;
}

#ea-graph-container {
  height: 480px;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 6px;
  margin: 0.5rem 0 1rem;
  background: var(--md-default-bg-color);
}

.status-badge {
  display: inline-block;
  padding: 0.15em 0.5em;
  border-radius: 4px;
  font-size: 0.8em;
  font-weight: 600;
}

.status-proposed { background: #fff3cd; color: #856404; }
.status-approved { background: #d4edda; color: #155724; }
.status-implemented { background: #cce5ff; color: #004085; }
.status-mandatory { background: #f8d7da; color: #721c24; }
.status-invalid { background: #e2e3e5; color: #383d41; }

.sl {
  display: inline-block;
  padding: 0.1em 0.45em;
  border-radius: 3px;
  font-size: 0.7em;
  font-weight: 600;
  font-family: inherit;
  vertical-align: middle;
  margin-right: 0.3em;
}

.sl[data-layer="business"] { background: #2E86C1; color: #fff; }
.sl[data-layer="application"] { background: #27AE60; color: #fff; }
.sl[data-layer="technology"] { background: #8E44AD; color: #fff; }
.sl[data-layer="physical"] { background: #17A589; color: #fff; }
.sl[data-layer="motivation"] { background: #C0392B; color: #fff; }
.sl[data-layer="strategy"] { background: #D4AC0D; color: #fff; }
.sl[data-layer="implementation"] { background: #E67E22; color: #fff; }
.sl[data-layer="composite"] { background: #5D6D7E; color: #fff; }
.sl[data-layer="uml"] { background: #7F8C8D; color: #fff; }

/* EDGY facet colors — light backgrounds with dark text for readability */
.sl[data-layer="edgy-id"] { background: #75F0A5; color: #1a3a2a; }
.sl[data-layer="edgy-ar"] { background: #9DB9F6; color: #1a2a4a; }
.sl[data-layer="edgy-ex"] { background: #F985B4; color: #4a1a2a; }
.sl[data-layer="edgy-ix"] { background: #4ECDC4; color: #1a3a3a; }
.sl[data-layer="edgy-pe"] { background: #FFD93D; color: #4a3a1a; }
.sl[data-layer="edgy-lb"] { background: #E8E8E8; color: #555; }
""";
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
