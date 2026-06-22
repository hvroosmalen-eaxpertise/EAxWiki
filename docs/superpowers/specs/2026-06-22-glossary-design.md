# Glossary Page (Issue #12)

## Problem

EA models contain domain-specific terminology, acronyms, and definitions embedded as tagged values or element notes. There is no generated glossary.

## Solution

Generate `glossary/index.md` that extracts terminology from the model. Two sources: "Definition"/"Glossary" tagged values, and first sentence from element notes.

## Architecture

New method `GenerateGlossaryAsync` called from `ExportAsync` after `GenerateTypesPagesAsync`. Pattern follows existing type page generation: iterate elements, extract entries, deduplicate, write page. Add glossary nav entry to root `.pages`.

## Glossary Page Generation

### Source 1: Tagged Values

Scan every element's `TaggedValues` list. Match tagged values where `Name` equals "Definition" or "Glossary" (case-insensitive). For each match:
- **Term**: element name
- **Definition**: tagged value `Value`
- **Source**: link to element page

### Source 2: First Sentence from Notes

Scan every element with non-empty `Notes`. Extract the first sentence as the definition — text before the first `.` character, trimmed. Skip if the extracted sentence is less than 20 characters (filters noise like "See diagram." or "TBD.").

### Deduplication

Same term (element name) appearing from multiple sources → merge into one table row. Source column shows comma-separated links to each contributing element page.

### Output

`glossary/index.md`:

```markdown
# Glossary

| Term | Definition | Source |
|------|------------|--------|
| ESRS | European Sustainability Reporting Standards | [ESRS E1](../link), [ESRS](../link) |
| GHG | Greenhouse Gas emissions | [Emission Sources](../link) |

*Generated: 2026-06-22 15:30:00*
```

If no terms found:

```markdown
# Glossary

No glossary terms found.

*Generated: 2026-06-22 15:30:00*
```

### Navigation

Add to root `.pages`:

```
- Glossary: glossary/
```

## Edge Cases

- No matching tagged values and no notes with content → empty glossary with "No glossary terms found"
- First sentence under 20 chars → skip
- Multiple elements with same name → merge sources
- Tagged value with empty Value → skip
