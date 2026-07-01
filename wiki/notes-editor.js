(function () {
  'use strict';

  function initNotesEditor() {
    var widget = document.getElementById('ea-notes-editor');
    if (!widget || widget.dataset.initialized) return;
    widget.dataset.initialized = 'true';

    var eaId = parseInt(widget.dataset.eaId, 10);
    var file = widget.dataset.filePath;
    var port = widget.dataset.apiPort || '8001';
    var apiBase = 'http://localhost:' + port;

    var editBtn = document.getElementById('ea-notes-edit-btn');
    var contentDiv = widget.querySelector('.ea-notes-content');
    if (!editBtn || !contentDiv) return;

    var notesMarkerPattern = /<!--\s*ea-notes-(start|end)\s*-->/g;
    var textarea, controls, saveBtn, cancelBtn, msg;

    function enterEditMode() {
      textarea = document.createElement('textarea');
      textarea.className = 'ea-notes-textarea';
      textarea.value = contentDiv.innerHTML.replace(notesMarkerPattern, '').trim();

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
    }

    function save() {
      var newNotes = textarea.value;
      saveBtn.disabled = true;
      cancelBtn.disabled = true;
      msg.textContent = 'Saving…';

      fetch(apiBase + '/api/notes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ elementId: eaId, newNotes: newNotes, filePath: file })
      })
      .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, data: d }; }); })
      .then(function (res) {
        if (res.ok) {
          contentDiv.innerHTML = res.data.html;
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