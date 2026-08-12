(function () {
  'use strict';

  function findWidget() {
    return document.getElementById('ea-status-editor')
        || document.getElementById('ea-notes-editor')
        || document.querySelector('.ea-row-notes-edit-btn');
  }

  function applyState(reason) {
    var ready = reason === null;
    document.body.classList.toggle('ea-api-ready', ready);
    document.body.classList.toggle('ea-api-unavailable', !ready);
    if (!ready) document.body.setAttribute('data-ea-reason', reason);
    else document.body.removeAttribute('data-ea-reason');

    var title = ready ? 'Edit'
      : reason === 'no-ea'
        ? 'Read-only: API is up but cannot reach the EA model. Check the EA repository connection.'
        : 'Read-only: EAxWiki API not reachable. Start EAxWiki --api to enable editing.';
    document.querySelectorAll('.ea-status-edit-btn, .ea-notes-edit-btn, .ea-row-notes-edit-btn')
      .forEach(function (btn) { btn.setAttribute('title', title); });

    document.dispatchEvent(new CustomEvent('ea-api-status', { detail: { ready: ready, reason: reason } }));
  }

  function probe() {
    var widget = findWidget();
    if (!widget) return;
    var port = widget.dataset.apiPort || '8001';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;

    var controller = typeof AbortController !== 'undefined' ? new AbortController() : null;
    var timer = setTimeout(function () { if (controller) controller.abort(); }, 2500);

    fetch(apiBase + '/readyz', { signal: controller ? controller.signal : undefined })
      .then(function (r) {
        clearTimeout(timer);
        if (r.ok) { applyState(null); return; }
        if (r.status === 503) { applyState('no-ea'); return; }
        applyState('no-api');
      })
      .catch(function () {
        clearTimeout(timer);
        applyState('no-api');
      });
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { probe(); });
  } else {
    document.addEventListener('DOMContentLoaded', probe);
  }
})();