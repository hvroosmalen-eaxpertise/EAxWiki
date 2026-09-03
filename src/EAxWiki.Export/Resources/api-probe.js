(function () {
  'use strict';

  // Shared edit guard: while any editor widget on the page has an open textarea, we suspend
  // page reloads (mkdocs livereload fires on every wiki file change — including status
  // pages the monitor writes every ~10 min and every write-back save — which otherwise
  // destroys an in-progress edit mid-typing, mid-Suggest, mid-Save). Each editor calls
  // EAxEditGuard.acquire() on enterEditMode and .release() on exit; when the count is
  // non-zero, location.reload() is buffered and applied when the last editor releases.
  if (!window.EAxEditGuard) {
    var count = 0;
    var pending = false;
    var origReload = window.location.reload.bind(window.location);
    try {
      Object.defineProperty(window.location, 'reload', {
        configurable: true,
        value: function () {
          if (count > 0) { pending = true; return; }
          return origReload.apply(this, arguments);
        }
      });
    } catch (e) {
      // Some browsers freeze the location object; if we can't override, fall back to no-op guard.
      console.warn('EAxEditGuard: could not override location.reload:', e);
    }
    window.EAxEditGuard = {
      acquire: function () { count++; },
      release: function () {
        if (count > 0) count--;
        if (count === 0 && pending) { pending = false; origReload(); }
      },
      isActive: function () { return count > 0; }
    };
  }

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