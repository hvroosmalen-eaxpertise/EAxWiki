(function () {
  'use strict';

  // Shared edit guard: while any editor widget on the page has an open textarea, mkdocs
  // livereload must not reload the page (it fires on every wiki file change — status pages
  // the monitor writes every ~10 min, every write-back save, every export — which otherwise
  // destroys an in-progress edit mid-typing, mid-Suggest, mid-Save).
  //
  // We can't monkey-patch window.location.reload (browsers make it non-configurable and
  // non-writable). Instead we intercept mkdocs livereload's polling XHR to /livereload/*:
  // when the guard is active AND the response would trigger location.reload(), we substitute
  // a fake response so the poll reschedules instead. mkdocs livereload's own retry loop
  // resumes normal behavior automatically once the guard releases.
  //
  // Each editor calls EAxEditGuard.acquire() on enterEditMode and .release() on exit;
  // reference-counted so multiple concurrent editors work.
  if (!window.EAxEditGuard) {
    var count = 0;
    window.EAxEditGuard = {
      acquire: function () { count++; },
      release: function () { if (count > 0) count--; },
      isActive: function () { return count > 0; }
    };

    // onloadend lives on XMLHttpRequestEventTarget.prototype, not XMLHttpRequest.prototype.
    // Setting xhr.onloadend = fn goes through the prototype's native setter, which stores
    // the handler in an internal slot. Object.defineProperty on the INSTANCE would add a
    // shadow property that never sees the internal slot — so we grab the native descriptor
    // and use its getter/setter to read the stored handler and replace it with our wrapper.
    var eventTargetProto = (typeof XMLHttpRequestEventTarget !== 'undefined')
      ? XMLHttpRequestEventTarget.prototype : XMLHttpRequest.prototype;
    var lrDesc = Object.getOwnPropertyDescriptor(eventTargetProto, 'onloadend');
    if (!lrDesc || !lrDesc.get || !lrDesc.set) {
      console.warn('EAxEditGuard: onloadend descriptor missing; livereload interception disabled');
    } else {
      var OrigOpen = XMLHttpRequest.prototype.open;
      var OrigSend = XMLHttpRequest.prototype.send;
      XMLHttpRequest.prototype.open = function (method, url) {
        if (typeof url === 'string' && url.indexOf('/livereload/') !== -1) {
          this.__eaLivereload = true;
        }
        return OrigOpen.apply(this, arguments);
      };
      XMLHttpRequest.prototype.send = function () {
        if (this.__eaLivereload) {
          var handler = lrDesc.get.call(this);
          if (typeof handler === 'function') {
            var self = this;
            lrDesc.set.call(this, function () {
              // livereload's onloadend does:
              //   if (parseFloat(this.responseText) > epoch) location.reload();
              //   else setTimeout(poll, this.status === 200 ? 0 : 3000);
              // While the guard is active, hand the handler a fake `this` with a low
              // responseText and status 503 so it skips reload and reschedules ~3s later.
              // Each subsequent poll passes through this filter until the edit ends.
              if (window.EAxEditGuard.isActive()) {
                handler.call({ responseText: '0', status: 503 });
              } else {
                handler.call(self);
              }
            });
          }
        }
        return OrigSend.apply(this, arguments);
      };
    }
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