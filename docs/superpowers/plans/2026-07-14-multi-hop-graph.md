# Multi-Hop Graph Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the per-page 2-hop embedded graph with a single `graph-index.json` and client-side BFS with configurable depth control.

**Architecture:** A new `GraphIndexExporter` produces `graph-index.json` (all nodes + edges) at the wiki root during every export. JS fetches this once and BFS-walks from the focal element at any depth. `RelationshipGraphRenderer.cs` is removed; per-page `#ea-graph-data` is replaced with `data-focal-id`. Single-tap navigates to the element page.

**Tech Stack:** C# .NET (System.Text.Json, string builders), JavaScript (vanilla ES5, cytoscape.js)

## Global Constraints

- No new NuGet packages — use `System.Text.Json` (already referenced)
- graph-index.json uses plain integer IDs (not `"e42"` string-prefixed) — the `"e"` prefix was a JS string-key workaround, not needed with ints
- All element URLs in graph-index.json are root-relative `.html` paths (e.g. `PkgName/ElemName.html`), resolved in JS by prepending `../` for page depth
- The focal element has `url: ""` (navigating to current page is a no-op)
- graph-index.json is written unconditionally alongside other parallel exports in `MarkdownExporter`
- Fallback: if `graph-index.json` fetch fails (404/network), parse legacy `<div id="ea-graph-data">`
- Target framework: `net10.0-windows`
- Test framework: xUnit (existing pattern)
- `.md` source links are correct for mkdocs — the built `.html` is what JS navigates to

---

### Task 1: GraphIndexExporter — new C# class producing graph-index.json

**Files:**
- Create: `src/EAxWiki.Export/Exporters/GraphIndexExporter.cs`
- Test: `src/EAxWiki.Tests/GraphIndexExporterTests.cs`

**Interfaces:**
- Consumes: `ExportContext` (`.Elements`, `.ElementLookup`, `.PackageLookup`, `.OutputPath`)
- Produces: `graph-index.json` at `Path.Combine(ctx.OutputPath, "graph-index.json")`

- [ ] **Step 1: Write failing tests**

Create `src/EAxWiki.Tests/GraphIndexExporterTests.cs`:

```csharp
using EAxWiki.Core.Models;
using EAxWiki.Export;
using EAxWiki.Export.Exporters;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace EAxWiki.Tests;

public class GraphIndexExporterTests
{
    private sealed class MemoryWriter : IOutputWriter
    {
        public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
        public Task CreateDirectoryAsync(string path, CancellationToken ct = default) => Task.CompletedTask;
        public Task WriteFileAsync(string filePath, string content, CancellationToken ct = default)
        {
            Files[filePath.Replace('\\', '/')] = content;
            return Task.CompletedTask;
        }
    }

    private static ExportContext MakeContext(List<(EaElement, string)> elements, Dictionary<int, (string Name, int? ParentId)> packageLookup)
    {
        var lookup = new Dictionary<int, (EaElement, string)>();
        foreach (var (el, dir) in elements)
            lookup[el.Id] = (el, dir);
        var pkgLookup = packageLookup;
        return new ExportContext(
            OutputPath: "C:\\out",
            Elements: elements,
            ElementLookup: lookup,
            AllDiagrams: [],
            DiagramIndex: [],
            IncomingIndex: [],
            PackageLookup: pkgLookup
        );
    }

    private static EaElement MakeElement(int id, string name, int pkgId, string type = "Class", string stereotype = "ArchiMate_BusinessActor")
    {
        return new EaElement
        {
            Id = id,
            Name = name,
            Type = type,
            Stereotype = stereotype,
            PackageId = pkgId,
        };
    }

    [Fact]
    public async Task Produces_Valid_Json_With_Nodes_And_Edges()
    {
        var pkg = ("Pkg", (int?)null);
        var a = MakeElement(1, "Alpha", 10, stereotype: "ArchiMate_BusinessActor");
        var b = MakeElement(2, "Beta", 10, stereotype: "ArchiMate_ApplicationComponent");
        a.Connectors.Add(new EaConnector { Id = 101, SourceId = 1, TargetId = 2, Name = "serves", Type = "Dependency" });
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg"), (b, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        var json = writer.Files["C:/out/graph-index.json"];
        Assert.NotNull(json);

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var nodes = root.GetProperty("nodes").EnumerateArray().ToList();
        Assert.Contains(nodes, n => n.GetProperty("id").GetInt32() == 1);
        Assert.Contains(nodes, n => n.GetProperty("id").GetInt32() == 2);

        var edges = root.GetProperty("edges").EnumerateArray().ToList();
        Assert.Single(edges);
        Assert.Equal(101, edges[0].GetProperty("id").GetInt32());
        Assert.Equal(1, edges[0].GetProperty("source").GetInt32());
        Assert.Equal(2, edges[0].GetProperty("target").GetInt32());
    }

    [Fact]
    public async Task Focal_Element_Has_Empty_Url()
    {
        var pkg = ("Pkg", (int?)null);
        var a = MakeElement(1, "Alpha", 10);
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var node = doc.RootElement.GetProperty("nodes")[0];
        Assert.Equal("", node.GetProperty("url").GetString());
    }

    [Fact]
    public async Task Edges_Deduplicated_By_ConnectorId()
    {
        var pkg = ("Pkg", (int?)null);
        var a = MakeElement(1, "Alpha", 10);
        var b = MakeElement(2, "Beta", 10);
        // Both endpoints reference the same connector.
        a.Connectors.Add(new EaConnector { Id = 101, SourceId = 1, TargetId = 2, Name = "link", Type = "Association" });
        b.Connectors.Add(new EaConnector { Id = 101, SourceId = 1, TargetId = 2, Name = "link", Type = "Association" });
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg"), (b, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var edges = doc.RootElement.GetProperty("edges").EnumerateArray().ToList();
        Assert.Single(edges);
    }

    [Fact]
    public async Task Node_Has_Layer_From_Stereotype()
    {
        var a = MakeElement(1, "Alpha", 10, stereotype: "ArchiMate_BusinessActor");
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var node = doc.RootElement.GetProperty("nodes")[0];
        Assert.Equal("business", node.GetProperty("layer").GetString());
    }

    [Fact]
    public async Task Node_Url_Is_RootRelative_Html()
    {
        var a = MakeElement(1, "My Element", 10);
        var ctx = MakeContext(
            [(a, "C:\\out\\MyPkg")],
            new() { [10] = ("MyPkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        var node = doc.RootElement.GetProperty("nodes")[0];
        Assert.Equal("MyPkg/My Element.html", node.GetProperty("url").GetString());
    }

    [Fact]
    public async Task Package_With_Two_Elements_No_Edges_Still_Produces_Nodes()
    {
        var a = MakeElement(1, "Alpha", 10);
        var b = MakeElement(2, "Beta", 10);
        var ctx = MakeContext(
            [(a, "C:\\out\\Pkg"), (b, "C:\\out\\Pkg")],
            new() { [10] = ("Pkg", null) }
        );
        var writer = new MemoryWriter();
        var exporter = new GraphIndexExporter(writer, NullLogger<GraphIndexExporter>.Instance);

        await exporter.ExportAsync(ctx, CancellationToken.None);

        using var doc = JsonDocument.Parse(writer.Files["C:/out/graph-index.json"]);
        Assert.Equal(2, doc.RootElement.GetProperty("nodes").GetArrayLength());
        Assert.Equal(0, doc.RootElement.GetProperty("edges").GetArrayLength());
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

```bash
dotnet test src/EAxWiki.Tests/GraphIndexExporterTests.cs --filter "GraphIndexExporterTests" -v n
```
Expected: 6 failures (types not found).

- [ ] **Step 3: Write minimal GraphIndexExporter implementation**

Create `src/EAxWiki.Export/Exporters/GraphIndexExporter.cs`:

```csharp
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class GraphIndexExporter(IOutputWriter writer, ILogger logger)
{
    public async Task ExportAsync(ExportContext ctx, CancellationToken ct = default)
    {
        var nodes = new List<Dictionary<string, object?>>(ctx.Elements.Count);
        var nodeLayers = new Dictionary<int, string>(ctx.Elements.Count);

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            var layer = MarkdownHelpers.GetLayer(elem);
            nodeLayers[elem.Id] = layer;

            var relPath = Path.GetRelativePath(ctx.OutputPath, Path.Combine(pkgDir, MarkdownHelpers.SanitizeName(elem.Name) + ".md"));
            var url = Path.ChangeExtension(relPath, ".html").Replace('\\', '/');

            nodes.Add(new Dictionary<string, object?>
            {
                ["id"] = elem.Id,
                ["label"] = elem.Name.Length > 24 ? elem.Name[..23] + "…" : elem.Name,
                ["fullName"] = elem.Name,
                ["packageName"] = ctx.PackageLookup.TryGetValue(elem.PackageId, out var pkg) ? pkg.Name : "",
                ["layer"] = layer,
                ["url"] = url,
            });
        }

        var edges = new List<Dictionary<string, object?>>();
        var seenEdges = new HashSet<int>();

        foreach (var (elem, _) in ctx.Elements)
        {
            foreach (var conn in elem.Connectors)
            {
                if (!seenEdges.Add(conn.Id)) continue;
                if (!ctx.ElementLookup.ContainsKey(conn.SourceId) || !ctx.ElementLookup.ContainsKey(conn.TargetId)) continue;

                var edgeLabel = !string.IsNullOrEmpty(conn.Name) ? conn.Name : conn.Type;
                var sourceLayer = nodeLayers.TryGetValue(conn.SourceId, out var sl) ? sl : "uml";

                edges.Add(new Dictionary<string, object?>
                {
                    ["id"] = conn.Id,
                    ["source"] = conn.SourceId,
                    ["target"] = conn.TargetId,
                    ["label"] = edgeLabel,
                    ["sourceLayer"] = sourceLayer,
                });
            }
        }

        var root = new Dictionary<string, object?>
        {
            ["nodes"] = nodes,
            ["edges"] = edges,
        };

        var json = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = false });
        await writer.WriteFileAsync(Path.Combine(ctx.OutputPath, "graph-index.json"), json, ct);
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

```bash
dotnet test src/EAxWiki.Tests/GraphIndexExporterTests.cs --filter "GraphIndexExporterTests" -v n
```
Expected: 6 PASS.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Exporters/GraphIndexExporter.cs src/EAxWiki.Tests/GraphIndexExporterTests.cs
git commit -m "feat: add GraphIndexExporter producing graph-index.json"
```

---

### Task 2: Remove RelationshipGraphRenderer, update ElementPageWriter

**Files:**
- Remove: `src/EAxWiki.Export/Renderers/RelationshipGraphRenderer.cs`
- Modify: `src/EAxWiki.Export/Exporters/ElementPageWriter.cs`

**Interfaces:**
- Consumes: `RelationshipGraphRenderer.Render()` call → removed; `element.Id` → used in `data-focal-id=""`
- Produces: Per-element pages with `<div id="ea-graph-container" data-focal-id="{id}"></div>` instead of graph data embed

- [ ] **Step 1: Delete RelationshipGraphRenderer.cs and update ElementPageWriter**

Delete `src/EAxWiki.Export/Renderers/RelationshipGraphRenderer.cs`.

In `src/EAxWiki.Export/Exporters/ElementPageWriter.cs`:

1. Remove `using EAxWiki.Export.Renderers;` (line 6)
2. Replace lines 92-94:

Old:
```csharp
        var graphHtml = RelationshipGraphRenderer.Render(element, dir, ctx);
        if (graphHtml.Length > 0)
            lines.AddRange(["---", string.Empty, "## Relationship Graph", string.Empty, graphHtml, string.Empty]);
```

New:
```csharp
        lines.AddRange(["---", string.Empty, "## Relationship Graph", string.Empty,
            $"<div id=\"ea-graph-container\" data-focal-id=\"{element.Id}\"></div>", string.Empty]);
```

- [ ] **Step 2: Verify tests still pass**

```bash
dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName!~GraphIndexExporter" -v n
```
Expected: all existing tests PASS (ignore GraphIndexExporterTests which we verified in Task 1).

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/Renderers/RelationshipGraphRenderer.cs src/EAxWiki.Export/Exporters/ElementPageWriter.cs
git commit -m "refactor: remove RelationshipGraphRenderer, use data-focal-id on container"
```

---

### Task 3: Update graph-init.js — BFS, depth control, single-tap navigation

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (embedded `graphInitJs` string)

**Interfaces:**
- Consumes: `graph-index.json` from wiki root, `#ea-graph-container[data-focal-id]`
- Produces: cytoscape graph rendered with BFS-walked subgraph at configurable depth

- [ ] **Step 1: Update the embedded graphInitJs string in InfrastructureWriter.cs**

Replace lines 78-280 (the `const string graphInitJs = """ ... """` block) with:

```csharp
        const string graphInitJs = """
var EA_LAYER_COLORS = {
    'business':       '#D4A017',
    'application':    '#2E86C1',
    'technology':     '#27AE60',
    'physical':       '#17A589',
    'motivation':     '#8E44AD',
    'strategy':       '#A0682B',
    'implementation': '#D84B79',
    'composite':      '#5D6D7E',
    'uml':            '#7F8C8D',
    'edgy-id':        '#75F0A5',
    'edgy-ar':        '#9DB9F6',
    'edgy-ex':        '#F985B4',
    'edgy-ix':        '#4ECDC4',
    'edgy-pe':        '#FFD93D',
    'edgy-lb':        '#E8E8E8'
};
var EA_LAYER_DARK_TEXT = { 'edgy-id': true, 'edgy-pe': true, 'edgy-lb': true, 'business': true };
var EA_DISTANCE_COLORS = ['#e65100', '#ff8a65', '#a1887f', '#9e9e9e', '#757575', '#616161'];

var _graphIndexPromise = null;

function fetchGraphIndex() {
    if (!_graphIndexPromise) {
        _graphIndexPromise = fetch('graph-index.json').then(function (r) {
            if (!r.ok) throw new Error('Not found');
            return r.json();
        });
    }
    return _graphIndexPromise;
}

function bfsSubgraph(graph, focalId, maxDepth) {
    var adj = {};
    graph.nodes.forEach(function (n) { adj[n.id] = { node: n, neighbors: [], depth: Infinity }; });
    graph.edges.forEach(function (e) {
        if (adj[e.source]) adj[e.source].neighbors.push({ edge: e, targetId: e.target });
        if (adj[e.target]) adj[e.target].neighbors.push({ edge: e, targetId: e.source });
    });

    if (!adj[focalId]) return { nodes: [], edges: [] };

    adj[focalId].depth = 0;
    var queue = [focalId];
    while (queue.length > 0) {
        var cur = queue.shift();
        if (adj[cur].depth >= maxDepth) continue;
        adj[cur].neighbors.forEach(function (nb) {
            if (adj[nb.targetId].depth === Infinity) {
                adj[nb.targetId].depth = adj[cur].depth + 1;
                queue.push(nb.targetId);
            }
        });
    }

    var nodeSet = {};
    var edgeSet = {};
    Object.keys(adj).forEach(function (idStr) {
        var id = parseInt(idStr, 10);
        var info = adj[id];
        if (info.depth === Infinity) return;
        nodeSet[id] = info.node;
        info.neighbors.forEach(function (nb) {
            if (nodeSet[nb.targetId]) {
                edgeSet[nb.edge.id] = nb.edge;
            }
        });
    });

    var resultNodes = [];
    Object.keys(nodeSet).forEach(function (k) {
        var n = nodeSet[parseInt(k, 10)];
        n.bfsDepth = adj[parseInt(k, 10)].depth;
        resultNodes.push(n);
    });

    return {
        nodes: resultNodes,
        edges: Object.keys(edgeSet).map(function (k) { return edgeSet[parseInt(k, 10)]; })
    };
}

function getDistanceColor(depth) {
    if (depth < EA_DISTANCE_COLORS.length) return EA_DISTANCE_COLORS[depth];
    return EA_DISTANCE_COLORS[EA_DISTANCE_COLORS.length - 1];
}

// Resolves a root-relative URL (e.g. "Pkg/Elem.html") to an absolute URL
// suitable for window.location.href, based on the current page depth.
function resolveUrl(relPath) {
    if (!relPath) return '';
    var parts = window.location.pathname.replace(/\/$/, '').split('/');
    var depth = Math.max(0, parts.length - 2);
    var up = depth > 0 ? Array(depth + 1).join('../') : '';
    var a = document.createElement('a');
    a.href = up + relPath;
    return a.href;
}

// Resolves a page-relative URL from legacy embedded data.
function resolveLegacyUrl(relUrl) {
    var a = document.createElement('a');
    a.href = relUrl;
    return a.href;
}

function initEaGraph() {
    var container = document.getElementById('ea-graph-container');
    if (!container || typeof cytoscape === 'undefined') return;

    var focalIdStr = container.getAttribute('data-focal-id');
    if (!focalIdStr) return;
    var focalId = parseInt(focalIdStr, 10);

    var oldTooltip = document.getElementById('ea-graph-tooltip');
    if (oldTooltip) oldTooltip.remove();
    var oldDepthControl = document.getElementById('ea-graph-depth-control');
    if (oldDepthControl) oldDepthControl.remove();

    var depthControl = document.createElement('div');
    depthControl.id = 'ea-graph-depth-control';
    depthControl.style.cssText = 'margin-bottom:8px;font-size:13px;';
    depthControl.innerHTML = '<label for="ea-depth-select">Traversal depth:</label> ';
    var select = document.createElement('select');
    select.id = 'ea-depth-select';
    select.style.cssText = 'margin-left:6px;';
    [1,2,3,4,5,6,7,8,9].forEach(function (d) {
        var opt = document.createElement('option');
        opt.value = d;
        opt.textContent = d;
        if (d === 2) opt.selected = true;
        select.appendChild(opt);
    });
    var fullOpt = document.createElement('option');
    fullOpt.value = 'full';
    fullOpt.textContent = 'Full';
    select.appendChild(fullOpt);
    depthControl.appendChild(select);
    container.parentNode.insertBefore(depthControl, container);

    var tooltip = document.createElement('div');
    tooltip.id = 'ea-graph-tooltip';
    tooltip.style.cssText = 'position:fixed;background:#fff;border:1px solid #ddd;border-radius:6px;padding:8px 12px;font-size:12px;pointer-events:none;display:none;box-shadow:0 4px 12px rgba(0,0,0,.15);z-index:9999;max-width:240px;line-height:1.6;';
    document.body.appendChild(tooltip);

    // Clears the cytoscape canvas within container.
    function clearCy() {
        var canvas = container.querySelector('canvas');
        if (canvas) canvas.parentElement.innerHTML = '';
        else container.innerHTML = '';
    }

    function renderGraph(maxDepth) {
        clearCy();
        fetchGraphIndex().then(function (graph) {
            var sub = bfsSubgraph(graph, focalId, maxDepth === 'full' ? Infinity : parseInt(maxDepth, 10));

            if (sub.nodes.length === 0) {
                container.innerHTML = '<p style="color:#888;font-style:italic;padding:20px;text-align:center;">No relationships found for this element.</p>';
                return;
            }

            var cy = cytoscape({
                container: container,
                elements: {
                    nodes: sub.nodes.map(function (n) {
                        return { data: { id: 'n' + n.id, bfsDepth: n.bfsDepth, label: n.label, fullName: n.fullName, packageName: n.packageName, layer: n.layer, url: n.url } };
                    }),
                    edges: sub.edges.map(function (e) {
                        return { data: { id: 'e' + e.id, source: 'n' + e.source, target: 'n' + e.target, label: e.label, sourceLayer: e.sourceLayer } };
                    })
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
                            'background-color': function (ele) {
                                var d = ele.data('bfsDepth');
                                if (d === 0 || d) return getDistanceColor(d);
                                return EA_LAYER_COLORS[ele.data('layer')] || '#7F8C8D';
                            },
                            'color': function (ele) {
                                var d = ele.data('bfsDepth');
                                if (d === 0) return '#ffffff';
                                if (d === 1) return '#1a1a1a';
                                return EA_LAYER_DARK_TEXT[ele.data('layer')] ? '#1a1a1a' : '#ffffff';
                            },
                            'border-width': function (ele) { return ele.data('bfsDepth') === 0 ? 3 : 0; },
                            'border-color': function (ele) { return ele.data('bfsDepth') === 0 ? '#bf360c' : 'transparent'; },
                            'font-weight': function (ele) { return ele.data('bfsDepth') === 0 ? 'bold' : 'normal'; }
                        }
                    },
                    {
                        selector: 'node[!url]',
                        style: { 'opacity': 0.55 }
                    },
                    {
                        selector: 'edge',
                        style: {
                            'label': 'data(label)',
                            'font-size': '10px',
                            'curve-style': 'bezier',
                            'target-arrow-shape': 'triangle',
                            'target-arrow-color': function (ele) { return EA_LAYER_COLORS[ele.data('sourceLayer')] || '#90a4ae'; },
                            'line-color': function (ele) { return EA_LAYER_COLORS[ele.data('sourceLayer')] || '#90a4ae'; },
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
                    animate: true,
                    animationDuration: 400,
                    randomize: false,
                    nodeRepulsion: function () { return 400000; },
                    nodeOverlap: 20,
                    idealEdgeLength: function () { return 120; },
                    gravity: 80
                },
                minZoom: 0.2,
                maxZoom: 3
            });

            cy.on('mouseover', 'node', function (evt) {
                var d = evt.target.data();
                var html = '<strong>' + d.fullName + '</strong>';
                if (d.packageName) html += '<br><span style="color:#777;font-size:11px">' + d.packageName + '</span>';
                if (d.url) html += '<br><span style="color:#1565c0;font-size:11px">click to open</span>';
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
                if (url) window.location.href = resolveUrl(url);
            });
            cy.on('mouseover', 'node[url]', function () { container.style.cursor = 'pointer'; });
            cy.on('mouseout', 'node', function () { container.style.cursor = 'default'; });
        }).catch(function () {
            var dataEl = document.getElementById('ea-graph-data');
            if (!dataEl) {
                container.innerHTML = '<p style="color:#888;font-style:italic;padding:20px;text-align:center;">Graph data unavailable.</p>';
                return;
            }
            var legacyData;
            try { legacyData = JSON.parse(dataEl.textContent); } catch (e) { return; }
            if (!legacyData || !legacyData.nodes || legacyData.nodes.length === 0) {
                container.innerHTML = '<p style="color:#888;font-style:italic;padding:20px;text-align:center;">No relationships found for this element.</p>';
                return;
            }
            var dc = document.getElementById('ea-graph-depth-control');
            if (dc) dc.style.display = 'none';
            var cy = cytoscape({
                container: container,
                elements: {
                    nodes: legacyData.nodes.map(function (n) { return { data: n }; }),
                    edges: legacyData.edges.map(function (e) { return { data: e }; })
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
                            'background-color': function (ele) { return EA_LAYER_COLORS[ele.data('layer')] || '#7F8C8D'; },
                            'color': function (ele) { return EA_LAYER_DARK_TEXT[ele.data('layer')] ? '#1a1a1a' : '#ffffff'; }
                        }
                    },
                    {
                        selector: 'node[?isFocal]',
                        style: { 'background-color': '#e65100', 'border-width': 3, 'border-color': '#bf360c', 'font-weight': 'bold' }
                    },
                    {
                        selector: 'node[!hasUrl]',
                        style: { 'opacity': 0.55 }
                    },
                    {
                        selector: 'edge',
                        style: {
                            'label': 'data(label)',
                            'font-size': '10px',
                            'curve-style': 'bezier',
                            'target-arrow-shape': 'triangle',
                            'target-arrow-color': function (ele) { return EA_LAYER_COLORS[ele.data('sourceLayer')] || '#90a4ae'; },
                            'line-color': function (ele) { return EA_LAYER_COLORS[ele.data('sourceLayer')] || '#90a4ae'; },
                            'color': '#555',
                            'text-background-opacity': 1,
                            'text-background-color': '#f5f5f5',
                            'text-background-padding': '2px',
                            'text-background-shape': 'round-rectangle'
                        }
                    }
                ],
                layout: { name: 'cose', animate: false, randomize: false, nodeRepulsion: function () { return 400000; }, nodeOverlap: 20, idealEdgeLength: function () { return 120; }, gravity: 80 },
                minZoom: 0.2, maxZoom: 3
            });
            cy.fit(cy.elements(), 40);
            cy.on('tap', 'node', function (evt) {
                var url = evt.target.data('url');
                if (url) window.location.href = resolveLegacyUrl(url);
            });
        });
    }

    renderGraph(2);

    select.addEventListener('change', function () {
        renderGraph(select.value);
    });
}

if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initEaGraph(); });
} else {
    document.addEventListener('DOMContentLoaded', initEaGraph);
}
""";
```

- [ ] **Step 2: Verify tests still pass**

```bash
dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName!~GraphIndexExporter" -v n
```
Expected: all existing tests PASS.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs
git commit -m "feat: rewrite graph-init.js with BFS, depth control, single-tap navigation"
```

---

### Task 4: Wire GraphIndexExporter into MarkdownExporter

**Files:**
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs`

**Interfaces:**
- Consumes: `GraphIndexExporter` class
- Produces: `graph-index.json` written alongside other parallel exports

- [ ] **Step 1: Add GraphIndexExporter to the parallel task list**

In `src/EAxWiki.Export/MarkdownExporter.cs`, after line 103 (`infrastructure.WriteAiSuggestScriptAsync(...)`) and before `}`:

Add the following line at the end of the `viewTasks` list:
```csharp
                new GraphIndexExporter(_writer, _logger).ExportAsync(ctx, cancellationToken),
```

The full `viewTasks` block (lines 89–104) should become:

```csharp
            var viewTasks = new List<Task>
            {
                new TypesExporter(_writer, _logger).ExportAsync(ctx, cancellationToken),
                new GlossaryExporter(_writer).ExportAsync(ctx, cancellationToken),
                new RecentChangesExporter(_writer).ExportAsync(ctx, cancellationToken),
                new StatusDashboardExporter(_writer).ExportAsync(ctx, cancellationToken),
                new ModelHealthExporter(_writer).ExportAsync(ctx, cancellationToken),
                diagramExporter.WriteIndexAsync(ctx, cancellationToken),
                infrastructure.WritePagesFileAsync(outputPath, cancellationToken),
                infrastructure.WriteExtraCssAsync(outputPath, cancellationToken),
                infrastructure.WriteGraphScriptsAsync(outputPath, cancellationToken),
                infrastructure.WriteStatusEditorScriptAsync(outputPath, cancellationToken),
                infrastructure.WriteNotesEditorScriptAsync(outputPath, cancellationToken),
                infrastructure.WriteRowNotesEditorScriptAsync(outputPath, cancellationToken),
                infrastructure.WriteAiSuggestScriptAsync(outputPath, cancellationToken),
                new GraphIndexExporter(_writer, _logger).ExportAsync(ctx, cancellationToken),
            };
```

- [ ] **Step 2: Verify tests still pass**

```bash
dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName!~GraphIndexExporter" -v n
```
Expected: all existing tests PASS.

Then run the full test suite:

```bash
dotnet test src/EAxWiki.Tests -v n
```
Expected: all tests pass (including GraphIndexExporterTests).

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: wire GraphIndexExporter into MarkdownExporter parallel tasks"
```

---

### Task 5: Full build verification

- [ ] **Step 1: Build the solution**

```bash
dotnet build src/EAxWiki.sln
```
Expected: Build succeeded.

- [ ] **Step 2: Run all tests**

```bash
dotnet test src/EAxWiki.Tests -v n
```
Expected: all tests PASS.

- [ ] **Step 3: Commit any remaining changes**

```bash
git add -A
git commit -m "chore: finalize multi-hop graph implementation"
```
