using System.Reflection;
using System.Threading;
using EAxWiki.Core.Interfaces;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class InfrastructureWriter(IOutputWriter writer)
{
    public async Task WritePagesFileAsync(string outputDir, CancellationToken ct = default)
    {
        // wiki/status/health.md is written by scripts/monitor-export-and-serve.ps1 (issue #37/#38),
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
        var statusLines = File.Exists(Path.Combine(outputDir, "status", "health.md"))
            ? new[]
              {
                  "  - Status: status/",
                  "  - Pipeline Health: status/health.html",
                  "  - Model Health: status/model-health.html",
              }
            : new[]
              {
                  "  - Status: status/",
                  "  - Model Health: status/model-health.html",
              };

        await writer.WriteFileAsync(Path.Combine(outputDir, ".pages"), string.Join(Environment.NewLine,
        [
            "nav:",
            "  - Structure: ''",
            "  - Diagrams: diagrams/",
            "  - Types: types/",
            "  - Glossary: glossary/",
            "  - Recent: recent/",
            .. statusLines,
            string.Empty,
        ]), ct);

        var diagramsDir = Path.Combine(outputDir, "diagrams");
        await writer.CreateDirectoryAsync(diagramsDir, ct);
        await writer.WriteFileAsync(Path.Combine(diagramsDir, ".pages"),
            string.Join(Environment.NewLine, ["title: Diagrams", string.Empty]), ct);

        var typesDir = Path.Combine(outputDir, "types");
        await writer.CreateDirectoryAsync(typesDir, ct);
        await writer.WriteFileAsync(Path.Combine(typesDir, ".pages"),
            string.Join(Environment.NewLine, ["title: Types", string.Empty]), ct);
    }

    public async Task WriteGraphScriptsAsync(string outputDir, string brand, CancellationToken ct = default)
    {
        // Extract embedded cytoscape.min.js to the wiki output so it works offline.
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("cytoscape.min.js", StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        var cytoscapeJs = await reader.ReadToEndAsync();
        await writer.WriteFileAsync(Path.Combine(outputDir, "cytoscape.min.js"), cytoscapeJs, ct);

        var graphInitJs = """
var EA_LAYER_COLORS = /*EA_LAYER_COLORS*/;
var EA_LAYER_DARK_TEXT = /*EA_LAYER_DARK_TEXT*/;
var EA_DISTANCE_COLORS = ['#e65100', '#ff8a65', '#a1887f', '#9e9e9e', '#757575', '#616161'];

var _graphIndexPromise = null;

function fetchGraphIndex() {
    if (!_graphIndexPromise) {
        _graphIndexPromise = fetch(resolveUrl('graph-index.json')).then(function (r) {
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

        graphInitJs = graphInitJs.Replace("/*EA_LAYER_COLORS*/", "{\n" + SerializeColors(layerColors) + "\n}")
                                 .Replace("/*EA_LAYER_DARK_TEXT*/", "{ " + SerializeDarkText(darkText) + " }");

        await writer.WriteFileAsync(Path.Combine(outputDir, "graph-init.js"), graphInitJs, ct);
    }

    public async Task WriteStatusEditorScriptAsync(string outputDir, CancellationToken ct = default)
    {
        const string js = """
(function () {
  'use strict';

  if (typeof EAxIcons === 'undefined' && !window.__eaIconsWarned) {
    window.__eaIconsWarned = true;
    console.error('EAxIcons helper not loaded');
  }

  function initStatusEditor() {
    var widget = document.getElementById('ea-status-editor');
    if (!widget || widget.dataset.initialized) return;
    widget.dataset.initialized = 'true';

    var eaId    = parseInt(widget.dataset.eaId, 10);
    var current = widget.dataset.status;
    var options = JSON.parse(widget.dataset.options);
    var file    = widget.dataset.filePath;
    var port    = widget.dataset.apiPort || '8001';
    var token   = widget.dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;

    var badge   = widget.querySelector('.status-badge');
    var editBtn = widget.querySelector('.ea-status-edit-btn');
    if (!badge || !editBtn) return;

    var select, applyBtn, cancelBtn, msg;

    function acquireEditLock(eaId) {
      var port = (document.getElementById('ea-status-editor') || {}).dataset.apiPort || '8001';
      var token = (document.getElementById('ea-status-editor') || {}).dataset.apiToken || '';
      var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
      fetch(apiBase + '/api/edit-lock', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
        body: JSON.stringify({ action: 'acquire', elementId: eaId })
      }).catch(function () {});
    }

    function releaseEditLock() {
      var port = (document.getElementById('ea-status-editor') || {}).dataset.apiPort || '8001';
      var token = (document.getElementById('ea-status-editor') || {}).dataset.apiToken || '';
      var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
      fetch(apiBase + '/api/edit-lock', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
        body: JSON.stringify({ action: 'release' })
      }).catch(function () {});
    }

    function enterEditMode() {
      select = document.createElement('select');
      select.className = 'ea-status-select';
      options.forEach(function (opt) {
        var o = document.createElement('option');
        o.value = opt;
        o.textContent = opt;
        if (opt === current) o.selected = true;
        select.appendChild(o);
      });

      applyBtn = document.createElement('button');
      applyBtn.className = 'ea-status-btn';
      applyBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(applyBtn, 'apply', 'Apply');

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-status-cancel-btn';
      cancelBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(cancelBtn, 'cancel', 'Cancel');

      msg = document.createElement('span');
      msg.className = 'ea-status-msg';

      badge.style.display = 'none';
      editBtn.style.display = 'none';

      widget.appendChild(select);
      widget.appendChild(applyBtn);
      widget.appendChild(cancelBtn);
      widget.appendChild(msg);

      cancelBtn.addEventListener('click', exitEditMode);
      applyBtn.addEventListener('click', apply);
      acquireEditLock(eaId);
    }

    function exitEditMode() {
      releaseEditLock();
      [select, applyBtn, cancelBtn, msg].forEach(function (el) {
        if (el && el.parentNode) el.parentNode.removeChild(el);
      });
      select = applyBtn = cancelBtn = msg = null;
      badge.style.display = '';
      editBtn.style.display = '';
    }

    function apply() {
      var chosen = select.value;
      if (chosen === current) { msg.textContent = 'No change.'; return; }

      applyBtn.disabled = true;
      cancelBtn.disabled = true;
      msg.textContent = 'Saving…';

      var retries = 5;

      function doFetch() {
        fetch(apiBase + '/api/status', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
          body: JSON.stringify({ elementId: eaId, newStatus: chosen, filePath: file })
        })
        .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, status: r.status, data: d }; }); })
        .then(function (res) {
          if (res.ok) {
            current = chosen;
            badge.textContent = chosen;
            badge.className = 'status-badge status-' + chosen.toLowerCase();
            releaseEditLock();
            exitEditMode();
          } else if (res.status === 401) {
            msg.textContent = '✗ Not authenticated — re-export with --force to refresh this page.';
            msg.style.color = '#c62828';
            applyBtn.disabled = false;
            cancelBtn.disabled = false;
          } else if (res.status >= 500 && retries > 0) {
            retries--;
            msg.textContent = 'Retrying…';
            setTimeout(doFetch, 2000);
          } else {
            msg.textContent = '✗ ' + (res.data.message || 'Error');
            msg.style.color = '#c62828';
            applyBtn.disabled = false;
            cancelBtn.disabled = false;
          }
        })
        .catch(function (e) {
          if (retries > 0) {
            retries--;
            msg.textContent = 'Retrying…';
            setTimeout(doFetch, 2000);
          } else {
            msg.textContent = '✗ Could not reach API — is EAxWiki --api running?';
            msg.style.color = '#c62828';
            applyBtn.disabled = false;
            cancelBtn.disabled = false;
            console.error('EAxWiki status-editor error:', e);
          }
        });
      }

      doFetch();
    }

    editBtn.addEventListener('click', enterEditMode);
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initStatusEditor(); });
  } else {
    document.addEventListener('DOMContentLoaded', initStatusEditor);
  }
})();
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "status-editor.js"), js, ct);
    }

    public async Task WriteNotesEditorScriptAsync(string outputDir, CancellationToken ct = default)
    {
        const string js = """
(function () {
  'use strict';

  if (typeof EAxIcons === 'undefined' && !window.__eaIconsWarned) {
    window.__eaIconsWarned = true;
    console.error('EAxIcons helper not loaded');
  }

  function initNotesEditor() {
    var widget = document.getElementById('ea-notes-editor');
    if (!widget || widget.dataset.initialized) return;
    widget.dataset.initialized = 'true';

    var eaId    = parseInt(widget.dataset.eaId, 10);
    var kind    = widget.dataset.kind || 'element';
    var file    = widget.dataset.filePath;
    var port    = widget.dataset.apiPort || '8001';
    var token   = widget.dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
    var endpoint = kind === 'diagram' ? '/api/diagram-notes' : '/api/notes';
    var idField  = kind === 'diagram' ? 'diagramId' : kind === 'package' ? 'packageId' : 'elementId';

    var editBtn = document.getElementById('ea-notes-edit-btn');
    var contentDiv = widget.querySelector('.ea-notes-content');
    var hint = widget.querySelector('.ea-notes-derived-hint');
    if (!editBtn || !contentDiv) return;

    var notesMarkerPattern = /<!--\s*ea-(package-)?notes-(start|end)\s*-->/g;
    var placeholderHtml = '<em class="ea-notes-placeholder">No description set.</em>';

    if (contentDiv.innerHTML.replace(notesMarkerPattern, '').trim() === '') {
      contentDiv.innerHTML = placeholderHtml;
    }

    function stripHtml(html) {
      var tmp = document.createElement('div');
      tmp.innerHTML = html;
      return tmp.textContent || tmp.innerText || '';
    }

    var textarea, controls, saveBtn, cancelBtn, suggestBtn, msg;

    function enterEditMode() {
      var isPlaceholder = !!contentDiv.querySelector('.ea-notes-placeholder');

      textarea = document.createElement('textarea');
      textarea.className = 'ea-notes-textarea';
      textarea.value = isPlaceholder ? '' : stripHtml(contentDiv.innerHTML.replace(notesMarkerPattern, '').trim());

      controls = document.createElement('div');
      controls.className = 'ea-notes-controls';

      saveBtn = document.createElement('button');
      saveBtn.className = 'ea-notes-save-btn';
      saveBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(saveBtn, 'save', 'Save');

      suggestBtn = null;
      if (widget.dataset.aiConfigured === 'true') {
        suggestBtn = document.createElement('button');
        suggestBtn.className = 'ea-notes-suggest-btn';
        suggestBtn.type = 'button';
        if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'suggest', 'Suggest');
      }

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-notes-cancel-btn';
      cancelBtn.type = 'button';
      if (typeof EAxIcons !== 'undefined') EAxIcons.set(cancelBtn, 'cancel', 'Cancel');

      msg = document.createElement('span');
      msg.className = 'ea-notes-msg';

      controls.appendChild(saveBtn);
      if (suggestBtn) controls.appendChild(suggestBtn);
      controls.appendChild(cancelBtn);
      controls.appendChild(msg);

      contentDiv.style.display = 'none';
      editBtn.style.display = 'none';
      if (hint) hint.style.display = 'none';
      widget.appendChild(textarea);
      widget.appendChild(controls);
      textarea.focus();
      acquireEditLock(eaId);

      cancelBtn.addEventListener('click', exitEditMode);
      saveBtn.addEventListener('click', save);
      if (suggestBtn) {
        suggestBtn.addEventListener('click', function () {
          suggestBtn.disabled = true;
          if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'spinner', 'Generating…');
          msg.textContent = '';
          msg.style.color = '';

          var suggestEndpoint = kind === 'diagram' ? '/api/ai-suggest-diagram' : '/api/ai-suggest';
          var suggestPayload  = kind === 'diagram' ? { diagramId: eaId } : { elementId: eaId };

          fetch(apiBase + suggestEndpoint, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
            body: JSON.stringify(suggestPayload)
          })
          .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, status: r.status, data: d }; }); })
          .then(function (res) {
            if (res.ok) {
              textarea.value = res.data.suggestion;
              msg.textContent = 'Draft loaded \u2014 review and save.';
              msg.style.color = '#2e7d32';
            } else if (res.status === 204) {
              msg.textContent = res.data.message || 'Not enough context to suggest a description.';
              msg.style.color = '#666';
            } else {
              msg.textContent = 'Error: ' + (res.data.message || 'Unknown error');
              msg.style.color = '#c62828';
            }
            suggestBtn.disabled = false;
            if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'suggest', 'Suggest');
          })
          .catch(function (e) {
            msg.textContent = 'Could not reach AI service.';
            msg.style.color = '#c62828';
            suggestBtn.disabled = false;
            if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'suggest', 'Suggest');
            console.error('EAxWiki ai-suggest error:', e);
          });
        });
      }
    }

    function exitEditMode() {
      releaseEditLock();
      if (textarea) widget.removeChild(textarea);
      if (controls) widget.removeChild(controls);
      textarea = controls = null;
      contentDiv.style.display = '';
      editBtn.style.display = '';
      if (hint) hint.style.display = '';
    }

    function save() {
      var newNotes = textarea.value;
      var body = { newNotes: newNotes, filePath: file };
      body[idField] = eaId;

      saveBtn.disabled = true;
      cancelBtn.disabled = true;
      msg.textContent = 'Saving…';

      var retries = 5;

      function doFetch() {
        fetch(apiBase + endpoint, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
          body: JSON.stringify(body)
        })
        .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, status: r.status, data: d }; }); })
        .then(function (res) {
          if (res.ok) {
            releaseEditLock();
            contentDiv.innerHTML = res.data.html || placeholderHtml;
            if (hint && hint.parentNode) hint.parentNode.removeChild(hint);
            hint = null;
            exitEditMode();
          } else if (res.status === 401) {
            msg.textContent = '✗ Not authenticated — re-export with --force to refresh this page.';
            msg.style.color = '#c62828';
            saveBtn.disabled = false;
            cancelBtn.disabled = false;
          } else if (res.status >= 500 && retries > 0) {
            retries--;
            msg.textContent = 'Retrying…';
            setTimeout(doFetch, 2000);
          } else {
            msg.textContent = '✗ ' + (res.data.message || 'Error');
            msg.style.color = '#c62828';
            saveBtn.disabled = false;
            cancelBtn.disabled = false;
          }
        })
        .catch(function (e) {
          if (retries > 0) {
            retries--;
            msg.textContent = 'Retrying…';
            setTimeout(doFetch, 2000);
          } else {
            msg.textContent = '✗ Could not reach API — is EAxWiki --api running?';
            msg.style.color = '#c62828';
            saveBtn.disabled = false;
            cancelBtn.disabled = false;
            console.error('EAxWiki notes-editor error:', e);
          }
        });
      }

      doFetch();
    }

    editBtn.addEventListener('click', enterEditMode);
  }

  function acquireEditLock(eaId) {
    var port = (document.getElementById('ea-notes-editor') || {}).dataset.apiPort || '8001';
    var token = (document.getElementById('ea-notes-editor') || {}).dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
    fetch(apiBase + '/api/edit-lock', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
      body: JSON.stringify({ action: 'acquire', elementId: eaId })
    }).catch(function () {});
  }

  function releaseEditLock() {
    var port = (document.getElementById('ea-notes-editor') || {}).dataset.apiPort || '8001';
    var token = (document.getElementById('ea-notes-editor') || {}).dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
    fetch(apiBase + '/api/edit-lock', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
      body: JSON.stringify({ action: 'release' })
    }).catch(function () {});
  }

  function init() {
    initNotesEditor();
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { init(); });
  } else {
    document.addEventListener('DOMContentLoaded', init);
  }
})();
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "notes-editor.js"), js, ct);
    }

    public async Task WriteRowNotesEditorScriptAsync(string outputDir, CancellationToken ct = default)
    {
        const string js = """
(function () {
  'use strict';

  if (typeof EAxIcons === 'undefined' && !window.__eaIconsWarned) {
    window.__eaIconsWarned = true;
    console.error('EAxIcons helper not loaded');
  }

  var currentOpen = null;

  function closeCurrent() {
    if (currentOpen) { currentOpen.cleanup(); currentOpen = null; }
  }

  function initPlaceholders() {
    var markerPattern = /<!--\s*ea-row-notes-(start|end):[^>]*?-->/g;
    document.querySelectorAll('.ea-row-notes-text').forEach(function (span) {
      if (span.innerHTML.replace(markerPattern, '').trim() === '') {
        span.innerHTML = '<em class="ea-row-notes-placeholder">No description set.</em>';
      }
    });
  }

  function buildRequestBody(btn, rowId, newNotes) {
    var kind = btn.dataset.kind;
    var body = {
      newNotes: newNotes,
      filePath: btn.dataset.filePath,
      rowId: rowId,
      elementId: parseInt(btn.dataset.elId, 10),
      kind: kind
    };
    if (kind === 'attribute') {
      body.attributeName = btn.dataset.attrName;
      body.attributeType = btn.dataset.attrType;
    } else if (kind === 'method') {
      body.methodName = btn.dataset.methodName;
      body.returnType = btn.dataset.returnType;
      body.isStatic = btn.dataset.isStatic === 'true';
    } else if (kind === 'tagged-value') {
      body.tagName = btn.dataset.tagName;
      body.tagValue = btn.dataset.tagValue;
    }
    return body;
  }

  function openEditor(btn) {
    var rowId = btn.dataset.rowId;
    if (currentOpen && currentOpen.rowId === rowId) { closeCurrent(); return; }
    closeCurrent();

    var surface = btn.dataset.surface;
    var markerPattern = /<!--\s*ea-row-notes-(start|end):[^>]*?-->/g;
    var widget = btn.closest(surface === 'inline' ? '.ea-row-notes-widget' : 'tr');
    var textSpan = widget.querySelector('.ea-row-notes-text');
    if (!widget || !textSpan) return;

    var isPlaceholder = !!textSpan.querySelector('.ea-row-notes-placeholder');
    var seedValue = isPlaceholder ? '' : textSpan.innerHTML.replace(markerPattern, '').trim();

    var host;
    if (surface === 'table-row') {
      var editRow = document.querySelector('tr.ea-row-edit[data-row-id="' + rowId + '"]');
      if (!editRow) return;
      host = editRow.querySelector('td');
      editRow.style.display = '';
    } else {
      host = widget;
    }

    var textarea = document.createElement('textarea');
    textarea.className = 'ea-row-notes-textarea';
    textarea.value = seedValue;

    var controls = document.createElement('div');
    controls.className = 'ea-row-notes-controls';

    var saveBtn = document.createElement('button');
    saveBtn.className = 'ea-notes-save-btn';
    saveBtn.type = 'button';
    if (typeof EAxIcons !== 'undefined') EAxIcons.set(saveBtn, 'save', 'Save');

    var cancelBtn = document.createElement('button');
    cancelBtn.className = 'ea-notes-cancel-btn';
    cancelBtn.type = 'button';
    if (typeof EAxIcons !== 'undefined') EAxIcons.set(cancelBtn, 'cancel', 'Cancel');

    var msg = document.createElement('span');
    msg.className = 'ea-notes-msg';

    controls.appendChild(saveBtn);
    controls.appendChild(cancelBtn);
    controls.appendChild(msg);

    textSpan.style.display = 'none';
    btn.style.display = 'none';
    host.appendChild(textarea);
    host.appendChild(controls);
    textarea.focus();

    function cleanup() {
      if (textarea.parentNode) textarea.parentNode.removeChild(textarea);
      if (controls.parentNode) controls.parentNode.removeChild(controls);
      textSpan.style.display = '';
      btn.style.display = '';
      if (surface === 'table-row') {
        var editRow2 = document.querySelector('tr.ea-row-edit[data-row-id="' + rowId + '"]');
        if (editRow2) editRow2.style.display = 'none';
      }
    }

    currentOpen = { rowId: rowId, cleanup: cleanup };

    cancelBtn.addEventListener('click', function () { closeCurrent(); });

    saveBtn.addEventListener('click', function () {
      saveBtn.disabled = true;
      cancelBtn.disabled = true;
      msg.textContent = 'Saving…';

      var port = btn.dataset.apiPort || '8001';
      var token = btn.dataset.apiToken || '';
      var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
      var body = buildRequestBody(btn, rowId, textarea.value);
      var retries = 5;

      function doFetch() {
        fetch(apiBase + '/api/row-notes', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
          body: JSON.stringify(body)
        })
        .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, status: r.status, data: d }; }); })
        .then(function (res) {
          if (res.ok) {
            var html = res.data.html;
            textSpan.innerHTML = html && html.trim() ? html : '<em class="ea-row-notes-placeholder">No description set.</em>';
            closeCurrent();
          } else if (res.status === 401) {
            msg.textContent = '✗ Not authenticated — re-export with --force to refresh this page.';
            msg.style.color = '#c62828';
            saveBtn.disabled = false;
            cancelBtn.disabled = false;
          } else if (res.status >= 500 && retries > 0) {
            retries--;
            msg.textContent = 'Retrying…';
            setTimeout(doFetch, 2000);
          } else {
            msg.textContent = '✗ ' + (res.data.message || 'Error');
            msg.style.color = '#c62828';
            saveBtn.disabled = false;
            cancelBtn.disabled = false;
          }
        })
        .catch(function (e) {
          if (retries > 0) {
            retries--;
            msg.textContent = 'Retrying…';
            setTimeout(doFetch, 2000);
          } else {
            msg.textContent = '✗ Could not reach API — is EAxWiki --api running?';
            msg.style.color = '#c62828';
            saveBtn.disabled = false;
            cancelBtn.disabled = false;
            console.error('EAxWiki row-notes-editor error:', e);
          }
        });
      }

      doFetch();
    });
  }

  function initRowNotesEditors() {
    document.querySelectorAll('.ea-row-notes-edit-btn').forEach(function (btn) {
      if (btn.dataset.initialized) return;
      btn.dataset.initialized = 'true';
      btn.addEventListener('click', function () { openEditor(btn); });
    });
  }

  function init() {
    initPlaceholders();
    initRowNotesEditors();
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { init(); });
  } else {
    document.addEventListener('DOMContentLoaded', init);
  }
})();
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "row-notes-editor.js"), js, ct);
    }

    public async Task WriteIconsScriptAsync(string outputDir, CancellationToken ct = default)
    {
        const string js = """
window.EAxIcons = {
  ICONS: {
    save: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>',
    cancel: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>',
    suggest: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 0L14.6 9.4 24 12 14.6 14.6 12 24 9.4 14.6 0 12 9.4 9.4z"/></svg>',
    apply: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>',
    spinner: '<svg viewBox="0 0 24 24" class="ea-icon-spinner" fill="currentColor" aria-hidden="true"><path d="M12 6v3l4-4-4-4v3c-4.42 0-8 3.58-8 8 0 1.57.46 3.03 1.24 4.26L6.7 14.8c-.45-.83-.7-1.79-.7-2.8 0-3.31 2.69-6 6-6zm6.76 1.74L17.3 9.2c.44.84.7 1.79.7 2.8 0 3.31-2.69 6-6 6v-3l-4 4 4 4v-3c4.42 0 8-3.58 8-8 0-1.57-.46-3.03-1.24-4.26z"/></svg>'
  },
  set: function (btn, name, label) {
    btn.innerHTML = this.ICONS[name] || '';
    btn.setAttribute('aria-label', label);
    btn.setAttribute('title', label);
  }
};
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "ea-icons.js"), js, ct);
    }

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
