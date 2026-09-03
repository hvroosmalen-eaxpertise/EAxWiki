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
      if (!document.body.classList.contains('ea-api-ready')) return;
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
      if (window.EAxEditGuard) window.EAxEditGuard.acquire();
    }

    function exitEditMode() {
      releaseEditLock();
      [select, applyBtn, cancelBtn, msg].forEach(function (el) {
        if (el && el.parentNode) el.parentNode.removeChild(el);
      });
      select = applyBtn = cancelBtn = msg = null;
      badge.style.display = '';
      editBtn.style.display = '';
      if (window.EAxEditGuard) window.EAxEditGuard.release();
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