# Replace Action Labels with Icons

- **Issue:** [#81](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues/81)
- **Date:** 2026-08-03
- **Status:** Approved

## Problem

Some wiki actions are text-labeled buttons (Save, Cancel, Suggest, Apply). The issue requests replacing those labels with representative icons. Text labels are visually noisy and take more horizontal space than needed, especially in the inline editing controls.

## Scope

Exactly the **7 runtime-created action buttons** across the three generated editor scripts:

| Editor | Button | className | Current label |
|---|---|---|---|
| status-editor | Apply | `ea-status-btn` | `'Apply'` |
| status-editor | Cancel | `ea-status-cancel-btn` | `'Cancel'` |
| notes-editor | Save | `ea-notes-save-btn` | `'Save'` |
| notes-editor | Suggest | `ea-notes-suggest-btn` | `'Suggest'` |
| notes-editor | Cancel | `ea-notes-cancel-btn` | `'Cancel'` |
| row-notes-editor | Save | `ea-notes-save-btn` | `'Save'` |
| row-notes-editor | Cancel | `ea-notes-cancel-btn` | `'Cancel'` |

Out of scope:

- Static server-rendered buttons (pencil edit buttons `&#9998;`) — already icon-based.
- Inline message text (`msg.textContent`, e.g. `Retrying…`, error strings) — not buttons.
- Breadcrumbs, pagination, navigation.

Note: the issue text mentions "Submit" but no such button exists; the status editor's primary action is labeled **Apply**.

## Decisions

1. **Icon-only buttons.** No text remains in the button; meaning comes from the icon plus `aria-label`/`title`.
2. **Inline SVG icons** (24×24 viewBox Material-style paths), fill `currentColor`, sized ~1em. No unicode glyphs, no icon-font dependency.
3. **Shared external helper** `wiki/ea-icons.js` exposing `window.EAxIcons`, loaded via `mkdocs.yml` `extra_javascript` before the editor scripts.
4. **Suggest in-flight** swaps its sparkle icon for an animated CSS spinner; the icon is restored on completion/error.
5. **Accessibility:** every icon button gets `aria-label` and `title` (matches existing edit-button convention).
6. **Type safety:** all buttons get `type='button'` (only Suggest has it today) to prevent accidental form submits.

## Icon Map

| Action | Icon | aria-label / title |
|---|---|---|
| Save | checkmark | `Save` |
| Cancel | X | `Cancel` |
| Suggest | sparkle (4-point star) | `Suggest` |
| Apply | check-circle | `Apply` |
| Suggest in-flight | spinner (animated) | `Generating…` |

## Architecture

### 1. New generated file: `wiki/ea-icons.js`

Emitted by a new `WriteIconsScriptAsync` method in `InfrastructureWriter.cs` (alongside the existing `WriteStatusEditorScriptAsync`, `WriteNotesEditorScriptAsync`, `WriteRowNotesEditorScriptAsync`, `WriteExtraCssAsync`). Content shape:

```js
window.EAxIcons = {
  ICONS: {
    save: '<svg viewBox="0 0 24 24" ...>...checkmark path...</svg>',
    cancel: '<svg viewBox="0 0 24 24" ...>...X path...</svg>',
    suggest: '<svg viewBox="0 0 24 24" ...>...sparkle path...</svg>',
    apply: '<svg viewBox="0 0 24 24" ...>...check-circle path...</svg>',
    spinner: '<svg viewBox="0 0 24 24" class="ea-icon-spinner" ...>...</svg>'
  },
  set: function (btn, name, label) {
    btn.innerHTML = this.ICONS[name] || '';
    btn.setAttribute('aria-label', label);
    btn.setAttribute('title', label);
  }
};
```

Root-level `.js` files are skipped by orphan cleanup (confirmed in commit `4fedaa014`), so `ea-icons.js` will not be cleaned up.

### 2. `mkdocs.yml`

Add `ea-icons.js` as the first entry in `extra_javascript`, before `cytoscape.min.js` / `graph-init.js` / the three editors, so the helper is defined before any editor calls `EAxIcons.set`.

### 3. Editor scripts (`InfrastructureWriter.cs` templates)

Each button creation changes from:

```js
saveBtn.textContent = 'Save';
```

to:

```js
saveBtn.type = 'button';
EAxIcons.set(saveBtn, 'save', 'Save');
```

Guard helper use:

```js
if (typeof EAxIcons !== 'undefined') { EAxIcons.set(saveBtn, 'save', 'Save'); }
```

The Suggest in-flight handler swaps the icon:

```js
suggestBtn.disabled = true;
if (typeof EAxIcons !== 'undefined') EAxIcons.set(suggestBtn, 'spinner', 'Generating…');
```

and restores `EAxIcons.set(suggestBtn, 'suggest', 'Suggest')` on completion/error (existing success/error paths).

### 4. CSS (`Resources/extra.css`, source of `wiki/extra.css`)

Restyle the five button classes to the compact square pattern already used by the edit buttons:

- `.ea-status-btn`, `.ea-status-cancel-btn`, `.ea-notes-save-btn`, `.ea-notes-cancel-btn`, `.ea-notes-suggest-btn`:
  - `width/height: 1.8em; padding: 0; display: inline-flex; align-items: center; justify-content: center; font-size: 0.9em;`
  - keep existing color/background rules (primary for Save/Apply, default for Cancel, purple for Suggest).
- Shared SVG rule: `.ea-status-btn svg, .ea-notes-save-btn svg, .ea-notes-cancel-btn svg, .ea-notes-suggest-btn svg { width: 1em; height: 1em; fill: currentColor; }`
- Spinner: `@keyframes ea-spin { to { transform: rotate(360deg); } }` and `.ea-icon-spinner { animation: ea-spin 0.8s linear infinite; }`
- Disabled states unchanged (opacity 0.5 / `cursor: not-allowed`).

## Data Flow

The icon helper is loaded once per page (MkDocs `extra_javascript`). When a user opens an editor, the script creates buttons and calls `EAxIcons.set` to inject the SVG + labels. The Suggest flow swaps sparkle → spinner during the fetch and back on completion. No server-side data changes; this is purely presentational.

## Error Handling

- **Missing helper:** if `EAxIcons` failed to load (mkdocs order/config error), buttons render empty but remain clickable; the `typeof` guard prevents a thrown reference error. A `console.error('EAxIcons helper not loaded')` log is emitted once for diagnosability.
- **Unknown icon name:** `EAxIcons.set` falls back to an empty string for unknown names (no exception).
- **Suggest fetch failure:** existing error path re-enables the button; the design restores the sparkle icon there too.

## Testing

- **`ScriptTemplateIntegrityTests`:** add markers asserting:
  - `ea-icons.js` content is emitted by the exporter (write a probe like the existing tests).
  - The three editor templates reference `EAxIcons.set` and no longer contain `textContent = 'Save'`-style labels (positive + negative assertions).
- **`ExportIntegrationTests`:** extend the notes-editor / status-editor regression tests to assert the generated output contains the SVG icon and that the text label strings are gone from the generated `.js`.
- Run full test suite and a forced export; verify:
  - `wiki/ea-icons.js` is produced.
  - `mkdocs.yml` lists `ea-icons.js` first.
  - The three generated `wiki/*.js` files are byte-identical to their templates.
  - No orphan-cleanup removal of `ea-icons.js`.

## Regression Notes

- The three generated `wiki/*.js` files must remain byte-identical to their templates (integrity contract established by the script-template-integrity feature).
- `mkdocs.yml` `extra_javascript` order matters: `ea-icons.js` must precede all consumers.
- Status-editor remains without package-status dispatch (that feature was dropped for EA 17.1 compat) — icon work must not re-introduce it.
