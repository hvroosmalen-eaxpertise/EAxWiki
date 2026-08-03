# Design: EurSuRA Branding for EAxWiki (Issue #79)

**Date:** 2026-08-03
**Status:** Approved
**Scope:** Configurable brand support in the exporter + EurSuRA as the first brand, applied to the demo wiki
**Issue:** https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues/79

## Problem

The wiki renders with MkDocs Material's default indigo look — no logo, no brand palette, no custom typography. Issue #79 asks to move the look-and-feel closer to the EurSuRA brand. The complication: the exporter **wipes `wiki/` on every export** and writes all styling itself (embedded `extra.css`, embedded `graph-init.js`), and `mkdocs.yml` ships to every EAxWiki user via the installer. Branding must therefore (a) be *configurable* so EAxWiki stays neutral for other users, and (b) survive exports when activated.

## Brand Data (from issue #79 attachments)

| Asset | Value |
|-------|-------|
| Logo | 720×150 PNG (issue comment attachment) |
| Light Cyan | `#C4E5E7` |
| Jet Black | `#103135` |
| Platinum | `#F3F7F7` |
| Opal | `#A8C6C7` |
| Lime Cream | `#D0F391` |
| Primary typeface | **Geist** (headings + body) |
| Secondary typeface | **Geist Mono** (tags, labels — sparingly) |

## Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Brand activation | `--brand <name>` CLI flag + `.eaxwiki` config field | Follows existing config patterns; neutral default |
| Default behavior | No flag → current output, byte-identical | Zero risk to existing users |
| First brand | `eursura` | The only brand needed now |
| Theme palette | CSS variables in `brand.css`, not `theme.palette` | Material's `theme.palette` only supports named Material colors, not custom hex |
| Fonts | Google Fonts `@import` inside `brand.css` | Only branded wikis load the fonts; keeps `mkdocs.yml` neutral |
| `mkdocs.yml` | Static, neutral, references `brand.css` + logo (harmless when absent) | Verified MkDocs builds clean (exit 0) with missing `extra_css` and missing `theme.logo` files |
| Demo wiki | This repo's `.eaxwiki`/monitor gets `brand=eursura`; demo wiki regenerated branded | User chose "brand the demo" |
| AI suggest / write-back | Unaffected | Branding is presentation-only |

## Architecture

```
Exporter run (--brand eursura)
├── Config resolution: CLI --brand → env EAXWIKI_BRAND → .eaxwiki "brand" field
├── Neutral default: unknown/empty brand → current extra.css + graph colors, no logo
└── brand == "eursura":
    ├── wiki/brand.css              ← new embedded resource (palette + widget overrides)
    ├── wiki/assets/eursura-logo.png ← new embedded resource (referenced by theme.logo)
    └── graph-init.js EA_LAYER_COLORS ← parameterized to EurSuRA tones

Static mkdocs.yml (unchanged for other users, demo gains references):
    theme.logo: assets/eursura-logo.png
    extra_css: [extra.css, brand.css]
```

## Changes by File

### 1. `Config.cs` (EAxWiki)

New property + flag parsing:
```csharp
public string Brand { get; set; } = "";
// case "--brand": Brand = args[++i]; (requires value)
```
Add `--brand <name>` to the `--help` text in `Program.cs`.

### 2. `.eaxwiki` config (`LocalConfigStore.cs`)

New field `Brand` (serialized as `brand`, camelCase), nullable string, following the existing pattern. The monitor script's `--brand` arg parsing and `.eaxwiki` fallback mirror `--telegram-bot-token` handling in `monitor-export-and-serve.ps1`. No SchedulerUI or wizard UI changes are required for this issue — the demo is driven via `.eaxwiki` + monitor; interactive prompts for brand can be a later enhancement.

### 3. `MarkdownExporter.cs`

- Resolve brand once at export start: CLI/env/`.eaxwiki` (env `EAXWIKI_BRAND`, following the existing `EAXWIKI_API_PORT` env-read pattern).
- Pass `brand` into `InfrastructureWriter` (constructor or method param).
- Read via `Environment.GetEnvironmentVariable("EAXWIKI_BRAND")` like the existing `EAXWIKI_API_PORT` read, so CLI→env→.eaxwiki plumbing in `Program.cs` keeps working unchanged.

### 4. `InfrastructureWriter.cs`

- **`WriteExtraCssAsync`** — unchanged (always writes neutral `extra.css`).
- **New `WriteBrandAssetsAsync(outputDir, brand)`**:
  - `brand == "eursura"`: extract embedded `brand.css` resource → `wiki/brand.css`; extract embedded `eursura-logo.png` → `wiki/assets/eursura-logo.png`.
  - otherwise: no-op.
- **`WriteGraphScriptsAsync(outputDir, brand)`** — parameterize `EA_LAYER_COLORS`:
  - Neutral: current map verbatim (byte-identical output).
  - EurSuRA: brand map (see layer→color table in section 7 below).
- Add embedded resources to `EAxWiki.Export.csproj`:
  ```xml
  <EmbeddedResource Include="Resources\brand-eursura.css" />
  <EmbeddedResource Include="Resources\eursura-logo.png" />
  ```

### 5. `CleanupOrphanedFilesAsync`

Add `assets` to `specialDirs` and `brand.css` to expected root files so a branded incremental export never deletes them.

### 6. `Resources/brand-eursura.css` (new embedded resource)

```css
@import url('https://fonts.googleapis.com/css2?family=Geist:wght@400;500;600;700&family=Geist+Mono&display=swap');

:root {
  --md-primary-fg-color:        #103135;  /* Jet Black */
  --md-primary-fg-color--light: #A8C6C7;  /* Opal */
  --md-accent-fg-color:         #D0F391;  /* Lime Cream */
  --md-typeset-a-color:         #103135;  /* Jet Black links */
  --md-text-font:                'Geist', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  --md-code-font:                'Geist Mono', SFMono-Regular, Consolas, monospace;
}
```
Plus overrides for status badges, layer chips (`--sl`), diagram-thumb hover accent, and editor button accent colors, remapped from the current hardcoded palette to EurSuRA tones. Contrast verified during implementation (some light backgrounds flip to dark text).

### 7. `graph-init.js` layer colors (parameterized in InfrastructureWriter)

| Layer | Current | EurSuRA | Text |
|-------|---------|---------|------|
| business | `#D4A017` | `#A8C6C7` (Opal) | dark |
| application | `#2E86C1` | `#103135` (Jet Black) | light |
| technology | `#27AE60` | `#C4E5E7` (Light Cyan) | dark |
| physical | `#17A589` | `#6FB4B6` | dark |
| motivation | `#8E44AD` | `#D0F391` (Lime Cream) | dark |
| strategy | `#A0682B` | `#7FA8A9` | dark |
| implementation | `#D84B79` | `#5C8A8B` | light |
| composite | `#5D6D7E` | `#405B5C` | light |
| uml | `#7F8C8D` | `#F3F7F7` (Platinum) | dark |
| edgy-* | unchanged | unchanged | — |

`EA_DISTANCE_COLORS` and `EA_LAYER_DARK_TEXT` unchanged (functional, not brand).

### 8. Demo wiki (this repo)

- `.eaxwiki` → `brand: eursura`.
- `monitor-export-and-serve.ps1` passes `--brand` through (parse arg + `.eaxwiki` fallback).
- Re-run export with the flag; commit regenerated `wiki/` (brand.css, assets/, branded graph-init.js).

## Backward Compatibility

- No flag / unknown flag → output identical to today (neutral).
- Existing `mkdocs.yml` for other users: `theme.logo` + `extra_css` reference missing files → MkDocs builds clean (verified exit 0).
- No changes to write-back, API server, or markdown content generation.

## Testing

1. **Default neutrality**: export without `--brand` → no `brand.css`, no `assets/`, neutral `graph-init.js`. Compare to current output (should be identical).
2. **Brand emits files**: `--brand eursura` → `brand.css` + `assets/eursura-logo.png` present, `graph-init.js` contains brand layer colors.
3. **Unknown brand**: `--brand nonsense` → neutral output + warning, no throw.
4. **Orphan cleanup**: branded export → incremental export → `brand.css` + `assets/` still present.
5. **JS validity**: `EA_LAYER_COLORS` block is brace-balanced / parses after parameterization.
6. **ScriptTemplateIntegrityTests**: extend to cover `brand.css` presence rules.
7. **Manual**: `--brand eursura` + serve locally → header logo, Jet Black/Lime Cream palette, Geist fonts, brand graph node colors, status badges.

## Out of Scope

- Light/dark theme toggle.
- Additional brands beyond `eursura`.
- Rebranding the installer/README screenshots.
- AI-suggest widget restyling beyond accent color consistency.
