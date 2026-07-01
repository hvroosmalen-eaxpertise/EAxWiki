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