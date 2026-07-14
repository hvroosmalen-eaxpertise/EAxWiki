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
    var token   = widget.dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;
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
      saveBtn.textContent = 'Save';

      suggestBtn = null;
      if (widget.dataset.aiConfigured === 'true') {
        suggestBtn = document.createElement('button');
        suggestBtn.className = 'ea-notes-suggest-btn';
        suggestBtn.textContent = 'Suggest';
        suggestBtn.type = 'button';
      }

      cancelBtn = document.createElement('button');
      cancelBtn.className = 'ea-notes-cancel-btn';
      cancelBtn.textContent = 'Cancel';

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
          suggestBtn.textContent = 'Generating...';
          msg.textContent = '';
          msg.style.color = '';

          fetch(apiBase + '/api/ai-suggest', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
            body: JSON.stringify({ elementId: eaId })
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
            suggestBtn.textContent = 'Suggest';
          })
          .catch(function (e) {
            msg.textContent = 'Could not reach AI service.';
            msg.style.color = '#c62828';
            suggestBtn.disabled = false;
            suggestBtn.textContent = 'Suggest';
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