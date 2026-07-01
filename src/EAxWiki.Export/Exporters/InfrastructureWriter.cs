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
            "  - Status: status/",
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
                selector: 'node.expanded',
                style: { 'border-width': 2, 'border-color': 'rgba(255,255,255,0.7)', 'border-style': 'dashed' }
            },
            {
                selector: 'node.loading',
                style: { 'opacity': 0.4 }
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
        var expanded = evt.target.hasClass('expanded');
        var html = '<strong>' + d.fullName + '</strong>';
        if (d.packageName) html += '<br><span style="color:#777;font-size:11px">' + d.packageName + '</span>';
        if (d.hasUrl && !expanded) html += '<br><span style="color:#1565c0;font-size:11px">click to expand · double-click to open</span>';
        else if (d.hasUrl) html += '<br><span style="color:#1565c0;font-size:11px">double-click to open</span>';
        tooltip.innerHTML = html;
        tooltip.style.display = 'block';
    });
    cy.on('mousemove', function (evt) {
        if (tooltip.style.display === 'none') return;
        tooltip.style.left = (evt.originalEvent.clientX + 14) + 'px';
        tooltip.style.top = (evt.originalEvent.clientY - 10) + 'px';
    });
    cy.on('mouseout', 'node', function () { tooltip.style.display = 'none'; });

    var tapTimer;
    cy.on('tap', 'node', function (evt) {
        var node = evt.target;
        clearTimeout(tapTimer);
        tapTimer = setTimeout(function () { expandNode(node); }, 280);
    });
    cy.on('dbltap', 'node', function (evt) {
        clearTimeout(tapTimer);
        var url = evt.target.data('url');
        if (url) window.location.href = resolveUrl(url);
    });

    cy.on('mouseover', 'node[?hasUrl]', function () { container.style.cursor = 'pointer'; });
    cy.on('mouseout', 'node', function () { container.style.cursor = 'default'; });

    function resolveUrl(relUrl) {
        var a = document.createElement('a');
        a.href = relUrl;
        return a.href;
    }

    var coseLayout = {
        name: 'cose',
        animate: true,
        animationDuration: 500,
        randomize: false,
        nodeRepulsion: function () { return 400000; },
        nodeOverlap: 20,
        idealEdgeLength: function () { return 120; },
        gravity: 80
    };

    function expandNode(node) {
        var url = node.data('url');
        if (!url || node.hasClass('expanded') || node.hasClass('loading')) return;
        node.addClass('loading');
        fetch(resolveUrl(url))
            .then(function (r) { return r.text(); })
            .then(function (html) {
                var doc = new DOMParser().parseFromString(html, 'text/html');
                var el = doc.getElementById('ea-graph-data');
                if (!el) { node.removeClass('loading'); return; }
                var pageData = JSON.parse(el.textContent);
                var added = false;
                pageData.nodes.forEach(function (n) {
                    if (!cy.getElementById(n.id).length) {
                        cy.add({ group: 'nodes', data: n });
                        added = true;
                    }
                });
                pageData.edges.forEach(function (e) {
                    if (!cy.getElementById(e.id).length &&
                        cy.getElementById(e.source).length &&
                        cy.getElementById(e.target).length) {
                        cy.add({ group: 'edges', data: e });
                    }
                });
                node.removeClass('loading');
                node.addClass('expanded');
                if (added) cy.layout(coseLayout).run();
            })
            .catch(function () { node.removeClass('loading'); });
    }
}

// MkDocs Material instant navigation fires document$ on every page load.
// Without it, fall back to running once on DOMContentLoaded.
if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initEaGraph(); });
} else {
    document.addEventListener('DOMContentLoaded', initEaGraph);
}
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "graph-init.js"), graphInitJs);
    }

    public async Task WriteStatusEditorScriptAsync(string outputDir)
    {
        const string js = """
(function () {
  'use strict';

  function initStatusEditor() {
    var widget = document.getElementById('ea-status-editor');
    if (!widget || widget.dataset.initialized) return;
    widget.dataset.initialized = 'true';

    var eaId    = parseInt(widget.dataset.eaId, 10);
    var current = widget.dataset.status;
    var options = JSON.parse(widget.dataset.options);
    var file    = widget.dataset.filePath;
    var port    = widget.dataset.apiPort || '8001';
    var apiBase = 'http://localhost:' + port;

    var badge   = widget.querySelector('.status-badge');
    var editBtn = widget.querySelector('.ea-status-edit-btn');
    if (!badge || !editBtn) return;

    var select, applyBtn, cancelBtn, msg;

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
      applyBtn.textContent = 'Apply';

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-status-cancel-btn';
      cancelBtn.textContent = 'Cancel';

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
    }

    function exitEditMode() {
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

      fetch(apiBase + '/api/status', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ elementId: eaId, newStatus: chosen, filePath: file })
      })
      .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, data: d }; }); })
      .then(function (res) {
        if (res.ok) {
          current = chosen;
          badge.textContent = chosen;
          badge.className = 'status-badge status-' + chosen.toLowerCase();
          exitEditMode();
        } else {
          msg.textContent = '✗ ' + (res.data.message || 'Error');
          msg.style.color = '#c62828';
          applyBtn.disabled = false;
          cancelBtn.disabled = false;
        }
      })
      .catch(function (e) {
        msg.textContent = '✗ ' + (e && e.message ? e.message : 'Could not reach API — is EAxWiki --api running?');
        msg.style.color = '#c62828';
        applyBtn.disabled = false;
        cancelBtn.disabled = false;
        console.error('EAxWiki status-editor error:', e);
      });
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
        await writer.WriteFileAsync(Path.Combine(outputDir, "status-editor.js"), js);
    }

    public async Task WriteNotesEditorScriptAsync(string outputDir)
    {
        const string js = """
(function () {
  'use strict';

  function initNotesEditor() {
    var widget = document.getElementById('ea-notes-editor');
    if (!widget || widget.dataset.initialized) return;
    widget.dataset.initialized = 'true';

    var eaId    = parseInt(widget.dataset.eaId, 10);
    var kind    = widget.dataset.kind || 'element';
    var file    = widget.dataset.filePath;
    var port    = widget.dataset.apiPort || '8001';
    var apiBase = 'http://localhost:' + port;
    var endpoint = kind === 'diagram' ? '/api/diagram-notes' : '/api/notes';
    var idField  = kind === 'diagram' ? 'diagramId' : 'elementId';

    var editBtn = document.getElementById('ea-notes-edit-btn');
    var contentDiv = widget.querySelector('.ea-notes-content');
    var hint = widget.querySelector('.ea-notes-derived-hint');
    if (!editBtn || !contentDiv) return;

    var notesMarkerPattern = /<!--\s*ea-notes-(start|end)\s*-->/g;
    var placeholderHtml = '<em class="ea-notes-placeholder">No description set.</em>';

    if (contentDiv.innerHTML.replace(notesMarkerPattern, '').trim() === '') {
      contentDiv.innerHTML = placeholderHtml;
    }

    var textarea, controls, saveBtn, cancelBtn, msg;

    function enterEditMode() {
      var isPlaceholder = !!contentDiv.querySelector('.ea-notes-placeholder');

      textarea = document.createElement('textarea');
      textarea.className = 'ea-notes-textarea';
      textarea.value = isPlaceholder ? '' : contentDiv.innerHTML.replace(notesMarkerPattern, '').trim();

      controls = document.createElement('div');
      controls.className = 'ea-notes-controls';

      saveBtn = document.createElement('button');
      saveBtn.className = 'ea-notes-save-btn';
      saveBtn.textContent = 'Save';

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-notes-cancel-btn';
      cancelBtn.textContent = 'Cancel';

      msg = document.createElement('span');
      msg.className = 'ea-notes-msg';

      controls.appendChild(saveBtn);
      controls.appendChild(cancelBtn);
      controls.appendChild(msg);

      contentDiv.style.display = 'none';
      editBtn.style.display = 'none';
      if (hint) hint.style.display = 'none';
      widget.appendChild(textarea);
      widget.appendChild(controls);
      textarea.focus();

      cancelBtn.addEventListener('click', exitEditMode);
      saveBtn.addEventListener('click', save);
    }

    function exitEditMode() {
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

      fetch(apiBase + endpoint, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      })
      .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, data: d }; }); })
      .then(function (res) {
        if (res.ok) {
          contentDiv.innerHTML = res.data.html || placeholderHtml;
          if (hint && hint.parentNode) hint.parentNode.removeChild(hint);
          hint = null;
          exitEditMode();
        } else {
          msg.textContent = '✗ ' + (res.data.message || 'Error');
          msg.style.color = '#c62828';
          saveBtn.disabled = false;
          cancelBtn.disabled = false;
        }
      })
      .catch(function (e) {
        msg.textContent = '✗ ' + (e && e.message ? e.message : 'Could not reach API — is EAxWiki --api running?');
        msg.style.color = '#c62828';
        saveBtn.disabled = false;
        cancelBtn.disabled = false;
        console.error('EAxWiki notes-editor error:', e);
      });
    }

    editBtn.addEventListener('click', enterEditMode);
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initNotesEditor(); });
  } else {
    document.addEventListener('DOMContentLoaded', initNotesEditor);
  }
})();
""";
        await writer.WriteFileAsync(Path.Combine(outputDir, "notes-editor.js"), js);
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

.diagram-thumbs {
  display: flex;
  flex-wrap: wrap;
  gap: 1em;
  margin: 1em 0;
}
.diagram-thumb {
  text-align: center;
  text-decoration: none;
  width: 220px;
}
.diagram-thumb img {
  width: 100%;
  height: auto;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 6px;
  transition: border-color 0.2s;
}
.diagram-thumb:hover img {
  border-color: var(--md-primary-fg-color);
}
.diagram-thumb span {
  display: block;
  font-size: 0.85em;
  margin-top: 0.35em;
  color: var(--md-typeset-a-color);
}
.diagram-thumb--noimg {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 220px;
  height: 120px;
  border: 1px dashed var(--md-default-fg-color--lightest);
  border-radius: 6px;
  background: var(--md-code-bg-color);
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
.status-not-set { background: #e2e3e5; color: #6c757d; font-style: italic; }

.status-bar-container {
  margin: 0.25em 0;
}
.status-bar {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0.15em 0.6em;
  font-size: 0.85em;
  font-weight: 600;
  color: #fff;
  white-space: nowrap;
  border-radius: 4px;
  min-width: fit-content;
  height: 1.6em;
  box-sizing: border-box;
}

details.status-details {
  margin: 0.3em 0;
  padding: 0.15em 0.6em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-code-bg-color);
}
details.status-details summary {
  cursor: pointer;
  font-weight: 600;
  padding: 0.15em 0;
}
details.status-details[open] summary {
  margin-bottom: 0.15em;
  border-bottom: 1px solid var(--md-default-fg-color--lightest);
  padding-bottom: 0.3em;
}
details.status-details ul {
  margin: 0.25em 0;
  padding-left: 1.5em;
}
details.status-details li {
  margin: 0.1em 0;
}

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

.sl[data-layer="business"] { background: #D4A017; color: #fff; }
.sl[data-layer="application"] { background: #2E86C1; color: #fff; }
.sl[data-layer="technology"] { background: #27AE60; color: #fff; }
.sl[data-layer="physical"] { background: #17A589; color: #fff; }
.sl[data-layer="motivation"] { background: #8E44AD; color: #fff; }
.sl[data-layer="strategy"] { background: #A0682B; color: #fff; }
.sl[data-layer="implementation"] { background: #D84B79; color: #fff; }
.sl[data-layer="composite"] { background: #5D6D7E; color: #fff; }
.sl[data-layer="uml"] { background: #7F8C8D; color: #fff; }

/* EDGY facet colors — light backgrounds with dark text for readability */
.sl[data-layer="edgy-id"] { background: #75F0A5; color: #1a3a2a; }
.sl[data-layer="edgy-ar"] { background: #9DB9F6; color: #1a2a4a; }
.sl[data-layer="edgy-ex"] { background: #F985B4; color: #4a1a2a; }
.sl[data-layer="edgy-ix"] { background: #4ECDC4; color: #1a3a3a; }
.sl[data-layer="edgy-pe"] { background: #FFD93D; color: #4a3a1a; }
.sl[data-layer="edgy-lb"] { background: #E8E8E8; color: #555; }

/* Status editor widget */
.ea-status-editor {
  display: inline-flex;
  align-items: center;
  gap: 0.3em;
  vertical-align: middle;
}
.ea-status-edit-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.6em;
  height: 1.6em;
  padding: 0;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-code-bg-color);
  color: var(--md-default-fg-color--light);
  cursor: pointer;
  font-size: 0.8em;
  line-height: 1;
  vertical-align: middle;
}
.ea-status-edit-btn:hover {
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
}
.ea-status-select {
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  padding: 0.2em 0.4em;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  font-size: inherit;
}
.ea-status-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-primary-fg-color);
  border-radius: 4px;
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
  cursor: pointer;
  font-size: inherit;
  font-weight: 600;
}
.ea-status-cancel-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  cursor: pointer;
  font-size: inherit;
}
.ea-status-btn:disabled, .ea-status-cancel-btn:disabled { opacity: 0.5; cursor: not-allowed; }
.ea-status-msg { font-style: italic; font-size: 0.95em; }

/* Notes editor widget */
.ea-notes-editor {
  position: relative;
  margin: 0.6em 0 1em;
}
.ea-notes-content {
  padding-right: 2em;
}
.ea-notes-edit-btn {
  position: absolute;
  top: 0;
  right: 0;
  width: 1.8em;
  height: 1.8em;
  padding: 0;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-code-bg-color);
  color: var(--md-default-fg-color--light);
  cursor: pointer;
  font-size: 0.9em;
  line-height: 1;
}
.ea-notes-edit-btn:hover {
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
}
.ea-notes-derived-hint {
  display: block;
  font-size: 0.8em;
  font-style: italic;
  color: var(--md-default-fg-color--light);
  margin-bottom: 0.2em;
}
.ea-notes-placeholder {
  color: var(--md-default-fg-color--light);
}
.ea-notes-textarea {
  width: 100%;
  min-height: 8em;
  box-sizing: border-box;
  padding: 0.5em;
  font-family: var(--md-code-font-family, monospace);
  font-size: 0.85em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
}
.ea-notes-controls {
  display: flex;
  align-items: center;
  gap: 0.5em;
  margin-top: 0.4em;
}
.ea-notes-save-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-primary-fg-color);
  border-radius: 4px;
  background: var(--md-primary-fg-color);
  color: var(--md-primary-bg-color);
  cursor: pointer;
  font-size: 0.85em;
  font-weight: 600;
}
.ea-notes-cancel-btn {
  padding: 0.2em 0.8em;
  border: 1px solid var(--md-default-fg-color--lightest);
  border-radius: 4px;
  background: var(--md-default-bg-color);
  color: var(--md-default-fg-color);
  cursor: pointer;
  font-size: 0.85em;
}
.ea-notes-save-btn:disabled, .ea-notes-cancel-btn:disabled { opacity: 0.5; cursor: not-allowed; }
.ea-notes-msg { font-style: italic; font-size: 0.85em; }
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
            Path.Combine(ctx.OutputPath, "status"),
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
