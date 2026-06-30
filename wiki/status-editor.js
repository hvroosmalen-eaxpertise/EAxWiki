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

    var label = document.createElement('span');
    label.className = 'ea-status-label';
    label.textContent = 'Status:';

    var select = document.createElement('select');
    select.className = 'ea-status-select';
    options.forEach(function (opt) {
      var o = document.createElement('option');
      o.value = opt;
      o.textContent = opt;
      if (opt === current) o.selected = true;
      select.appendChild(o);
    });

    var btn = document.createElement('button');
    btn.className = 'ea-status-btn';
    btn.textContent = 'Apply';

    var msg = document.createElement('span');
    msg.className = 'ea-status-msg';

    widget.appendChild(label);
    widget.appendChild(select);
    widget.appendChild(btn);
    widget.appendChild(msg);

    btn.addEventListener('click', function () {
      var chosen = select.value;
      if (chosen === current) { msg.textContent = 'No change.'; return; }

      btn.disabled = true;
      msg.textContent = 'Saving…';

      fetch(apiBase + '/api/status', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ elementId: eaId, newStatus: chosen, filePath: file })
      })
      .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, data: d }; }); })
      .then(function (res) {
        if (res.ok) {
          msg.textContent = '✓ ' + res.data.message;
          msg.style.color = 'var(--md-typeset-a-color)';
          current = chosen;
          // Update the status badge on the page without a full reload
          var badge = document.querySelector('.status-badge');
          if (badge) {
            badge.textContent = chosen;
            badge.className = 'status-badge status-' + chosen.toLowerCase();
          }
        } else {
          msg.textContent = '✗ ' + (res.data.message || 'Error');
          msg.style.color = '#c62828';
        }
        btn.disabled = false;
      })
      .catch(function (e) {
        msg.textContent = '✗ ' + (e && e.message ? e.message : 'Could not reach API — is EAxWiki --api running?');
        msg.style.color = '#c62828';
        btn.disabled = false;
        console.error('EAxWiki status-editor error:', e);
      });
    });
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initStatusEditor(); });
  } else {
    document.addEventListener('DOMContentLoaded', initStatusEditor);
  }
})();