(function () {
  'use strict';

  function initAiSuggest() {
    var widget = document.getElementById('ea-notes-editor');
    if (!widget || widget.dataset.aiConfigured !== 'true') return;
    if (widget.querySelector('.ea-suggest-btn')) return;

    var contentDiv = widget.querySelector('.ea-notes-content');
    if (!contentDiv) return;
    var isPlaceholder = !!contentDiv.querySelector('.ea-notes-placeholder');
    if (!isPlaceholder) return;

    var eaId  = parseInt(widget.dataset.eaId, 10);
    var port  = widget.dataset.apiPort || '8001';
    var token = widget.dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;

    var btn = document.createElement('button');
    btn.className = 'ea-suggest-btn';
    btn.textContent = 'Suggest a description';
    btn.type = 'button';

    var msg = document.createElement('span');
    msg.className = 'ea-suggest-msg';
    msg.style.marginLeft = '8px';

    var container = document.createElement('div');
    container.style.marginTop = '8px';
    container.appendChild(btn);
    container.appendChild(msg);

    var editBtn = document.getElementById('ea-notes-edit-btn');
    if (editBtn && editBtn.parentNode) {
      editBtn.parentNode.insertBefore(container, editBtn.nextSibling);
    }

    btn.addEventListener('click', function () {
      btn.disabled = true;
      btn.textContent = 'Generating...';
      msg.textContent = '';
      msg.style.color = '';

      fetch(apiBase + '/api/ai-suggest', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
        body: JSON.stringify({ elementId: eaId })
      })
      .then(function (r) {
        return r.json().then(function (d) { return { ok: r.ok, status: r.status, data: d }; });
      })
      .then(function (res) {
        if (res.ok) {
          var textarea = widget.querySelector('.ea-notes-textarea');
          if (textarea) {
            textarea.value = res.data.suggestion;
            msg.textContent = 'Draft loaded — review and save.';
            msg.style.color = '#2e7d32';
          }
        } else if (res.status === 204) {
          msg.textContent = res.data.message || 'Not enough context to suggest a description.';
          msg.style.color = '#666';
        } else {
          msg.textContent = 'Error: ' + (res.data.message || 'Unknown error');
          msg.style.color = '#c62828';
        }
        btn.disabled = false;
        btn.textContent = 'Suggest a description';
      })
      .catch(function (e) {
        msg.textContent = 'Could not reach AI service.';
        msg.style.color = '#c62828';
        btn.disabled = false;
        btn.textContent = 'Suggest a description';
        console.error('EAxWiki ai-suggest error:', e);
      });
    });
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initAiSuggest(); });
  } else {
    document.addEventListener('DOMContentLoaded', initAiSuggest);
  }
})();