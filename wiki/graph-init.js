var EA_LAYER_COLORS = {
    'business':       '#A8C6C7',
    'application':    '#103135',
    'technology':     '#C4E5E7',
    'physical':       '#6FB4B6',
    'motivation':     '#D0F391',
    'strategy':       '#7FA8A9',
    'implementation': '#5C8A8B',
    'composite':      '#405B5C',
    'uml':            '#F3F7F7',
    'edgy-id':        '#75F0A5',
    'edgy-ar':        '#9DB9F6',
    'edgy-ex':        '#F985B4',
    'edgy-ix':        '#4ECDC4',
    'edgy-pe':        '#FFD93D',
    'edgy-lb':        '#E8E8E8'
};
var EA_LAYER_DARK_TEXT = { 'business': true, 'technology': true, 'physical': true, 'motivation': true, 'strategy': true, 'uml': true, 'edgy-id': true, 'edgy-pe': true, 'edgy-lb': true };
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

// Resolves a wiki-root-relative URL (e.g. "Pkg/Elem.html" or "graph-index.json")
// to an absolute URL. Uses the cytoscape.min.js <script> tag as an anchor because
// mkdocs computes that path correctly for the current page — including under a
// GitHub Pages project base path (/EAxWiki/…) where deriving the depth from
// window.location.pathname alone lands the fetch at the wrong host root (issue #84).
function wikiBase() {
    var s = document.querySelector('script[src$="cytoscape.min.js"]')
         || document.querySelector('script[src$="graph-init.js"]');
    if (s) return s.src.replace(/[^\/]+$/, '');
    var parts = window.location.pathname.replace(/\/$/, '').split('/');
    var depth = Math.max(0, parts.length - 2);
    var up = depth > 0 ? Array(depth + 1).join('../') : '';
    var a = document.createElement('a');
    a.href = up;
    return a.href;
}

function resolveUrl(relPath) {
    if (!relPath) return '';
    var a = document.createElement('a');
    a.href = wikiBase() + relPath;
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