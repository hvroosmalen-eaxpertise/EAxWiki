window.EAxIcons = {
  ICONS: {
    save: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>',
    cancel: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>',
    suggest: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 0L14.6 9.4 24 12 14.6 14.6 12 24 9.4 14.6 0 12 9.4 9.4z"/></svg>',
    apply: '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>',
    spinner: '<svg viewBox="0 0 24 24" class="ea-icon-spinner" fill="currentColor" aria-hidden="true"><path d="M12 6v3l4-4-4-4v3c-4.42 0-8 3.58-8 8 0 1.57.46 3.03 1.24 4.26L6.7 14.8c-.45-.83-.7-1.79-.7-2.8 0-3.31 2.69-6 6-6zm6.76 1.74L17.3 9.2c.44.84.7 1.79.7 2.8 0 3.31-2.69 6-6 6v-3l-4 4 4 4v-3c4.42 0 8-3.58 8-8 0-1.57-.46-3.03-1.24-4.26z"/></svg>'
  },
  set: function (btn, name, label) {
    btn.innerHTML = this.ICONS[name] || '';
    btn.setAttribute('aria-label', label);
    btn.setAttribute('title', label);
  }
};

// Sidebar nav glyph swap: awesome-pages .pages titles are plain text (no
// pymdownx.emoji processing), so 📁 / 🗺️ leading a nav label stay as OS-rendered
// Unicode by default. Swap them for Material Design SVG icons so the nav
// matches the body's :material-folder-outline: / :material-map-outline:
// glyphs across OSes and themes. Runs on Material's document$ subscribe so
// instant navigation still triggers it. Idempotent via data-ea-nav-glyph.
(function () {
  var glyph = function (svgPath) {
    return '<svg class="ea-nav-glyph" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">' +
      '<path d="' + svgPath + '"/></svg>';
  };
  var folder = glyph('M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z');
  var map    = glyph('M20.5,3L20.34,3.03L15,5.1L9,3L3.36,4.9C3.15,4.97 3,5.15 3,5.38V20.5A0.5,0.5 0 0,0 3.5,21L3.66,20.97L9,18.9L15,21L20.64,19.1C20.85,19.03 21,18.85 21,18.62V3.5A0.5,0.5 0 0,0 20.5,3M10,5.47L14,6.87V18.53L10,17.13V5.47M5,6.46L8,5.45V17.15L5,18.31V6.46M19,17.54L16,18.55V6.86L19,5.7V17.54Z');
  // Regex captures a leading 📁 or 🗺️ (with optional variation selector) plus surrounding
  // whitespace. Material Design wraps nav-link text with formatting whitespace, so the
  // emoji is rarely at position 0 of textContent — allow leading \s* too.
  var LEADING = /^\s*([\uD83D][\uDCC1\uDDFA])️?\s*/;

  function swapNavGlyphs(root) {
    (root || document).querySelectorAll('.md-nav__link:not([data-ea-nav-glyph])').forEach(function (link) {
      var textEl = link.querySelector('.md-ellipsis') || link;
      var text = (textEl.textContent || '').trim();
      var match = LEADING.exec(text);
      if (!match) return;
      var codepoint = match[1].charCodeAt(1);
      var svg = (codepoint === 0xDCC1) ? folder : (codepoint === 0xDDFA) ? map : null;
      if (!svg) return;
      var rest = text.replace(LEADING, '');
      textEl.innerHTML = svg + '<span class="ea-nav-glyph-text">' + rest.replace(/[&<>]/g, function (c) {
        return c === '&' ? '&amp;' : c === '<' ? '&lt;' : '&gt;';
      }) + '</span>';
      link.setAttribute('data-ea-nav-glyph', '1');
    });
  }

  if (typeof document$ !== 'undefined') document$.subscribe(function () { swapNavGlyphs(); });
  else document.addEventListener('DOMContentLoaded', function () { swapNavGlyphs(); });
})();