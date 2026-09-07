(function () {
  'use strict';

  // Material CSS variables the pickers manage. Each is written back into brand.css
  // inside the :root { ... } block as `--name: #value;` — see composeCss below.
  var COLOR_VARS = [
    { key: 'primary',      var: '--md-primary-fg-color',         label: 'Header / tabs background' },
    { key: 'primaryLight', var: '--md-primary-fg-color--light',  label: 'Text on primary (header title)' },
    { key: 'accent',       var: '--md-accent-fg-color',          label: 'Links, focus outlines' },
    { key: 'link',         var: '--md-typeset-a-color',          label: 'In-body link color' },
    { key: 'sectionBg',    var: '--ea-section-bg',               label: 'Section header background (Relationships, Tagged Values, …)' },
    { key: 'sectionFg',    var: '--ea-section-fg',               label: 'Section header text color' }
  ];

  function initBrandEditor() {
    var widget = document.getElementById('ea-brand-editor');
    if (!widget || widget.dataset.initialized) return;
    widget.dataset.initialized = 'true';

    var port  = widget.dataset.apiPort || '8001';
    var token = widget.dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;

    var currentCss  = widget.dataset.brandCss || '';
    var currentLogo = widget.dataset.currentLogo || '';

    render(widget, currentCss, currentLogo);
    seedPickersFromCss(widget, currentCss);
    attachHandlers(widget, apiBase, token);
  }

  function render(widget, css, logoPath) {
    var pickers = COLOR_VARS.map(function (v) {
      return (
        '<div class="ea-brand-field">' +
          '<label>' + escapeHtml(v.label) + ' <code>' + v.var + '</code></label>' +
          '<input type="color" data-brand-color="' + v.key + '" value="#000000">' +
          '<button type="button" class="ea-brand-clear" data-brand-clear="' + v.key + '" title="Clear this variable">clear</button>' +
        '</div>'
      );
    }).join('');

    var logoBlock =
      '<div class="ea-brand-field">' +
        '<label>Logo</label>' +
        (logoPath
          ? '<div class="ea-brand-logo-preview"><img src="../' + escapeAttr(logoPath) + '" alt="current logo"><code>' + escapeHtml(logoPath) + '</code></div>'
          : '<div class="ea-brand-logo-preview"><em>No logo configured.</em></div>') +
        '<input type="file" accept="image/png,image/jpeg,image/svg+xml,image/webp,image/gif,image/x-icon" class="ea-brand-logo-file">' +
        '<button type="button" class="ea-brand-logo-upload">Upload</button>' +
        (logoPath ? '<button type="button" class="ea-brand-logo-remove">Remove logo</button>' : '') +
      '</div>';

    widget.innerHTML =
      '<div class="ea-brand-pickers">' + pickers + '</div>' +
      '<div class="ea-brand-font">' +
        '<label>Body font (e.g. <code>Geist</code> or a full stack) — sets <code>--md-text-font</code></label>' +
        '<input type="text" class="ea-brand-font-input" placeholder="(Material default)">' +
      '</div>' +
      logoBlock +
      '<div class="ea-brand-raw">' +
        '<label>Raw <code>brand.css</code> — edit anything the widgets above don\'t cover.</label>' +
        '<textarea class="ea-brand-css-textarea" spellcheck="false" rows="14">' + escapeHtml(css) + '</textarea>' +
      '</div>' +
      '<div class="ea-brand-actions">' +
        '<button type="button" class="ea-brand-save">Save brand.css</button>' +
        '<span class="ea-brand-msg" aria-live="polite"></span>' +
      '</div>';
  }

  function seedPickersFromCss(widget, css) {
    COLOR_VARS.forEach(function (v) {
      var color = extractVar(css, v.var);
      if (color && /^#[0-9a-f]{3,8}$/i.test(color)) {
        var input = widget.querySelector('input[data-brand-color="' + v.key + '"]');
        if (input) input.value = color.length === 4 ? expandShortHex(color) : color.slice(0, 7);
      }
    });
    var font = extractVar(css, '--md-text-font');
    if (font) {
      var fontInput = widget.querySelector('.ea-brand-font-input');
      if (fontInput) fontInput.value = font.replace(/,.*$/, '').replace(/^['"]|['"]$/g, '').trim();
    }
  }

  // Best-effort read of `--name: value;` inside the first :root { ... } block.
  function extractVar(css, name) {
    var rootMatch = css.match(/:root\s*\{([\s\S]*?)\}/);
    var scope = rootMatch ? rootMatch[1] : css;
    // Skip commented-out lines.
    var lines = scope.split(/\r?\n/).filter(function (l) {
      var t = l.trim();
      return !t.startsWith('/*') && !t.startsWith('*') && !t.startsWith('//');
    });
    var re = new RegExp('(^|[;\\s])' + escapeRegExp(name) + '\\s*:\\s*([^;]+);');
    var m = lines.join('\n').match(re);
    return m ? m[2].trim() : null;
  }

  function attachHandlers(widget, apiBase, token) {
    var textarea = widget.querySelector('.ea-brand-css-textarea');
    var msg = widget.querySelector('.ea-brand-msg');

    widget.querySelectorAll('input[data-brand-color]').forEach(function (input) {
      input.addEventListener('change', function () {
        var meta = COLOR_VARS.find(function (v) { return v.key === input.dataset.brandColor; });
        if (!meta) return;
        textarea.value = upsertVar(textarea.value, meta.var, input.value);
      });
    });
    widget.querySelectorAll('button[data-brand-clear]').forEach(function (btn) {
      btn.addEventListener('click', function () {
        var meta = COLOR_VARS.find(function (v) { return v.key === btn.dataset.brandClear; });
        if (!meta) return;
        textarea.value = removeVar(textarea.value, meta.var);
      });
    });

    var fontInput = widget.querySelector('.ea-brand-font-input');
    fontInput.addEventListener('change', function () {
      var v = fontInput.value.trim();
      if (!v) {
        textarea.value = removeVar(textarea.value, '--md-text-font');
        return;
      }
      // Bare font name: quote it and add a fallback stack.
      var value = v.indexOf(',') >= 0 || v.indexOf(' ') >= 0
        ? v.replace(/^['"]|['"]$/g, '')
        : "'" + v + "', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif";
      textarea.value = upsertVar(textarea.value, '--md-text-font', value);
      // Auto-add a Google Fonts import if the user typed a bare name.
      if (v.indexOf(',') < 0 && v.indexOf(' ') < 0) {
        var importLine = "@import url('https://fonts.googleapis.com/css2?family=" +
          encodeURIComponent(v).replace(/%20/g, '+') + ":wght@400;500;600;700&display=swap');";
        if (textarea.value.indexOf(importLine) < 0) {
          textarea.value = importLine + '\n' + textarea.value;
        }
      }
    });

    widget.querySelector('.ea-brand-save').addEventListener('click', function () {
      msg.textContent = 'Saving…';
      fetch(apiBase + '/api/brand-css', {
        method: 'POST',
        headers: { 'Content-Type': 'text/plain', 'X-EAxWiki-Token': token },
        body: textarea.value
      }).then(function (r) { return r.json().then(function (j) { return { ok: r.ok, j: j }; }); })
        .then(function (res) {
          msg.textContent = res.ok ? 'Saved. Reload the page to see changes.' : ('Failed: ' + (res.j && res.j.message || 'unknown'));
        })
        .catch(function (e) { msg.textContent = 'Failed: ' + e.message; });
    });

    var uploadBtn = widget.querySelector('.ea-brand-logo-upload');
    if (uploadBtn) uploadBtn.addEventListener('click', function () {
      var fileInput = widget.querySelector('.ea-brand-logo-file');
      var f = fileInput && fileInput.files && fileInput.files[0];
      if (!f) { msg.textContent = 'Pick a file first.'; return; }
      if (f.size > 2 * 1024 * 1024) { msg.textContent = 'Logo exceeds 2 MB.'; return; }
      var fd = new FormData();
      fd.append('file', f);
      msg.textContent = 'Uploading…';
      fetch(apiBase + '/api/brand-logo', {
        method: 'POST',
        headers: { 'X-EAxWiki-Token': token },
        body: fd
      }).then(function (r) { return r.json().then(function (j) { return { ok: r.ok, j: j }; }); })
        .then(function (res) {
          msg.textContent = res.ok
            ? 'Logo uploaded. Restart the wiki serve so mkdocs picks up the new logo.'
            : ('Failed: ' + (res.j && res.j.message || 'unknown'));
        })
        .catch(function (e) { msg.textContent = 'Failed: ' + e.message; });
    });

    var removeBtn = widget.querySelector('.ea-brand-logo-remove');
    if (removeBtn) removeBtn.addEventListener('click', function () {
      if (!confirm('Remove the logo entry from mkdocs.yml? The image file itself stays in wiki/assets/.')) return;
      msg.textContent = 'Removing…';
      fetch(apiBase + '/api/brand-logo', {
        method: 'DELETE',
        headers: { 'X-EAxWiki-Token': token }
      }).then(function (r) { return r.json().then(function (j) { return { ok: r.ok, j: j }; }); })
        .then(function (res) {
          msg.textContent = res.ok
            ? 'Logo removed. Restart the wiki serve to see the change.'
            : ('Failed: ' + (res.j && res.j.message || 'unknown'));
        })
        .catch(function (e) { msg.textContent = 'Failed: ' + e.message; });
    });
  }

  // Insert or update a `--name: value;` line inside the :root { ... } block. If the block does not
  // exist, prepend one. Idempotent — repeated saves don't stack duplicate declarations.
  function upsertVar(css, name, value) {
    var line = '  ' + name + ': ' + value + ';';
    var re = new RegExp('(:root\\s*\\{[\\s\\S]*?\\})');
    if (!re.test(css)) return ':root {\n' + line + '\n}\n' + css;

    return css.replace(re, function (block) {
      var pattern = new RegExp('^\\s*(?:/\\*[^*]*\\*/\\s*)?' + escapeRegExp(name) + '\\s*:[^;]*;\\s*$', 'm');
      if (pattern.test(block)) return block.replace(pattern, line);
      return block.replace(/\}$/, line + '\n}');
    });
  }

  function removeVar(css, name) {
    var re = new RegExp('(:root\\s*\\{[\\s\\S]*?\\})');
    return css.replace(re, function (block) {
      var pattern = new RegExp('^\\s*' + escapeRegExp(name) + '\\s*:[^;]*;\\s*\\n?', 'gm');
      return block.replace(pattern, '');
    });
  }

  function escapeHtml(s) {
    return String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
  }
  function escapeAttr(s) {
    return escapeHtml(s).replace(/"/g, '&quot;');
  }
  function escapeRegExp(s) {
    return s.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }
  function expandShortHex(c) {
    return '#' + c[1] + c[1] + c[2] + c[2] + c[3] + c[3];
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initBrandEditor);
  } else {
    initBrandEditor();
  }
})();
