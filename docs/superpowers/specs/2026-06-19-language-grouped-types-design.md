# Language-Grouped Types Pages

## Summary

Subdivide the types pages by modelling language. The language is extracted from the element's stereotype by parsing the `::` prefix. The type name is cleaned by stripping the language base + `_` prefix where present.

## Parsing Logic

```
stereotype                          → language     | type
─────────────────────────────────────────────────────────────
ArchiMate3::ArchiMate_Assessment    → ArchiMate3   | Assessment
ArchiMate3::ArchiMate_Goal          → ArchiMate3   | Goal
EDGY::Capability                    → EDGY         | Capability
EDGY::EDGY_Something                → EDGY         | Something
Asset                                → UML          | Asset
Brand                                → UML          | Brand
(empty)                              → UML          | Uncategorized
```

### Rules

1. Split on first `::` — left side = language, right side = type.
2. If no `::` present → language = `"UML"`, the whole value is the type.
3. If the value is empty/whitespace → language = `"UML"`, type = `"Uncategorized"`.
4. **Cleanup rule**: After splitting, extract the language base name (everything before the first digit or end) and strip `{base}_` from the type if it starts with it.
   - e.g. `ArchiMate3` → base `ArchiMate` → strip `ArchiMate_` from type.
   - `EDGY` → base `EDGY` → strip `EDGY_` from type if present.

## File Structure

```
wiki/
  types/
    .pages                         # title: Types
    index.md                       # Lists languages, each linking to language/index.md
    ArchiMate3/
      .pages                       # title: ArchiMate3
      index.md                     # Lists types in this language
      Assessment.md                # Elements with Assessment type
      Goal.md
      ...
    EDGY/
      .pages                       # title: EDGY
      index.md                     # Lists types
      Capability.md
      ...
    UML/
      .pages                       # title: UML
      index.md
      Asset.md
      Brand.md
      ...
```

### Nav Behavior

Root `.pages` already has `Types: types/`. Each language directory gets a `.pages` with `title: <language>`. The awesome-pages plugin renders a collapsible tree:

```
Types
├── ArchiMate3
│   ├── Assessment
│   ├── Goal
│   └── ...
├── EDGY
│   ├── Capability
│   └── ...
└── UML
    ├── Asset
    ├── Brand
    └── ...
```

## Code Changes

### `MarkdownExporter.cs` — `GenerateTypesPagesAsync`

Current flat structure (one level of files in `types/`) needs to change to two-level (language subdirectories with index + type pages):

1. **Parse step**: Map each element to `(language, type)` using the rules above.
2. **Group**: Build a nested `Lookup<string, (string type, EaElement, string pkgDir)>` — first keyed by language, then by type within each language.
3. **Language index**: For each language, write `types/{language}/index.md` listing all types in that language (with links to the type pages).
4. **Type pages**: For each type, write `types/{language}/{sanitizedType}.md` with links to elements (same content as current type pages, just in subdirectories).
5. **Root index**: `types/index.md` now lists languages instead of individual stereotypes.
6. **`.pages` files**: Write `types/{language}/.pages` for each language directory.

### Relative Path Fix

Current type pages use `../{relativeDir}/{elemName}.md` to link to elements (from `types/` dir). Now they're two levels deep at `types/{language}/`, so links become `../../{relativeDir}/{elemName}.md`.

### `--force` Required

This changes the directory structure inside `types/` — must regenerate all pages with `serve-full.ps1`.

## Edge Cases

- **Language name sanitization**: Language names go through `SanitizeName()` like all other user-derived names.
- **Duplicate language names**: Two languages that sanitize to the same name would collide. Unlikely in practice; if it happens, the second overwrites the first (existing behavior for duplicate elements).
- **Empty language/type**: Empty language defaults to `"UML"`, empty type defaults to `"Uncategorized"`.
- **`types/` `.pages`**: Already exists, no change needed — just `title: Types`.
