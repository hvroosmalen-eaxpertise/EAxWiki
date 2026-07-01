(function () {
  'use strict';

  var currentOpen = null;

  function closeCurrent() {
    if (currentOpen) { currentOpen.cleanup(); currentOpen = null; }
  }

  function initPlaceholders() {
    var markerPattern = /<!--\s*ea-row-notes-(start|end):[^>]*?-->/g;
    document.querySelectorAll('.ea-row-notes-text').forEach(function (span) {
      if (span.innerHTML.replace(markerPattern, '').trim() === '') {
        span.innerHTML = '<em class="ea-row-notes-placeholder">No description set.</em>';
      }
    });
  }

  function buildRequestBody(btn, rowId, newNotes) {
    var kind = btn.dataset.kind;
    var body = {
      newNotes: newNotes,
      filePath: btn.dataset.filePath,
      rowId: rowId,
      elementId: parseInt(btn.dataset.elId, 10),
      kind: kind
    };
    if (kind === 'attribute') {
      body.attributeName = btn.dataset.attrName;
      body.attributeType = btn.dataset.attrType;
    } else if (kind === 'method') {
      body.methodName = btn.dataset.methodName;
      body.returnType = btn.dataset.returnType;
      body.isStatic = btn.dataset.isStatic === 'true';
    } else if (kind === 'tagged-value') {
      body.tagName = btn.dataset.tagName;
      body.tagValue = btn.dataset.tagValue;
    }
    return body;
  }

  function openEditor(btn) {
    var rowId = btn.dataset.rowId;
    if (currentOpen && currentOpen.rowId === rowId) { closeCurrent(); return; }
    closeCurrent();

    var surface = btn.dataset.surface;
    var markerPattern = /<!--\s*ea-row-notes-(start|end):[^>]*?-->/g;
    var widget = btn.closest(surface === 'inline' ? '.ea-row-notes-widget' : 'tr');
    var textSpan = widget.querySelector('.ea-row-notes-text');
    if (!widget || !textSpan) return;

    var isPlaceholder = !!textSpan.querySelector('.ea-row-notes-placeholder');
    var seedValue = isPlaceholder ? '' : textSpan.innerHTML.replace(markerPattern, '').trim();

    var host;
    if (surface === 'table-row') {
      var editRow = document.querySelector('tr.ea-row-edit[data-row-id="' + rowId + '"]');
      if (!editRow) return;
      host = editRow.querySelector('td');
      editRow.style.display = '';
    } else {
      host = widget;
    }

    var textarea = document.createElement('textarea');
    textarea.className = 'ea-row-notes-textarea';
    textarea.value = seedValue;

    var controls = document.createElement('div');
    controls.className = 'ea-row-notes-controls';

    var saveBtn = document.createElement('button');
    saveBtn.className = 'ea-notes-save-btn';
    saveBtn.textContent = 'Save';

    var cancelBtn = document.createElement('button');
    cancelBtn.className = 'ea-notes-cancel-btn';
    cancelBtn.textContent = 'Cancel';

    var msg = document.createElement('span');
    msg.className = 'ea-notes-msg';

    controls.appendChild(saveBtn);
    controls.appendChild(cancelBtn);
    controls.appendChild(msg);

    textSpan.style.display = 'none';
    btn.style.display = 'none';
    host.appendChild(textarea);
    host.appendChild(controls);
    textarea.focus();

    function cleanup() {
      if (textarea.parentNode) textarea.parentNode.removeChild(textarea);
      if (controls.parentNode) controls.parentNode.removeChild(controls);
      textSpan.style.display = '';
      btn.style.display = '';
      if (surface === 'table-row') {
        var editRow2 = document.querySelector('tr.ea-row-edit[data-row-id="' + rowId + '"]');
        if (editRow2) editRow2.style.display = 'none';
      }
    }

    currentOpen = { rowId: rowId, cleanup: cleanup };

    cancelBtn.addEventListener('click', function () { closeCurrent(); });

    saveBtn.addEventListener('click', function () {
      saveBtn.disabled = true;
      cancelBtn.disabled = true;
      msg.textContent = 'Saving…';

      var port = btn.dataset.apiPort || '8001';
      var apiBase = 'http://localhost:' + port;
      var body = buildRequestBody(btn, rowId, textarea.value);

      fetch(apiBase + '/api/row-notes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      })
      .then(function (r) { return r.json().then(function (d) { return { ok: r.ok, data: d }; }); })
      .then(function (res) {
        if (res.ok) {
          var html = res.data.html;
          textSpan.innerHTML = html && html.trim() ? html : '<em class="ea-row-notes-placeholder">No description set.</em>';
          closeCurrent();
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
        console.error('EAxWiki row-notes-editor error:', e);
      });
    });
  }

  function initRowNotesEditors() {
    document.querySelectorAll('.ea-row-notes-edit-btn').forEach(function (btn) {
      if (btn.dataset.initialized) return;
      btn.dataset.initialized = 'true';
      btn.addEventListener('click', function () { openEditor(btn); });
    });
  }

  function init() {
    initPlaceholders();
    initRowNotesEditors();
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { init(); });
  } else {
    document.addEventListener('DOMContentLoaded', init);
  }
})();