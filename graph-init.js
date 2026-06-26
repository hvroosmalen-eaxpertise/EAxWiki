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