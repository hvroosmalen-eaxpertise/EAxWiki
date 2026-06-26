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

function initEaGraph() {
    var container = document.getElementById('ea-graph-container');
    if (!container || typeof cytoscape === 'undefined') return;

    var dataEl = document.getElementById('ea-graph-data');
    if (!dataEl) return;
    var data = JSON.parse(dataEl.textContent);

    // Remove any tooltip left over from a previous page navigation.
    var old = document.getElementById('ea-graph-tooltip');
    if (old) old.remove();
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
                    'background-color': function (ele) { return EA_LAYER_COLORS[ele.data('layer')] || '#7F8C8D'; },
                    'color': function (ele) { return EA_LAYER_DARK_TEXT[ele.data('layer')] ? '#1a1a1a' : '#ffffff'; }
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
    tooltip.id = 'ea-graph-tooltip';
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
}

// MkDocs Material instant navigation fires document$ on every page load.
// Without it, fall back to running once on DOMContentLoaded.
if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initEaGraph(); });
} else {
    document.addEventListener('DOMContentLoaded', initEaGraph);
}