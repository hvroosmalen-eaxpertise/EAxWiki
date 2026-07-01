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