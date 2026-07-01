---
ea_id: 233
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Outcome</span> Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e)

**Type:** Activity  **Stereotype:** Outcome  **StereotypeEx:** Outcome  **FQStereotype:** EDGY::Outcome  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="233" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Metrics/Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e).md" data-api-port="8001"></div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Center | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [Scope 1 uitstoot per bron (ton CO₂e)](Scope 1 uitstoot per bron (ton CO₂e).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Scope 1 uitstoot per bron (ton CO₂e)](Scope 1 uitstoot per bron (ton CO₂e).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e180","label":"Scope 1 uitstoot per br…","fullName":"Scope 1 uitstoot per bron (ton CO₂e)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Scope 1 uitstoot per bron (ton CO₂e).html"},{"id":"e233","label":"Totale uitstoot = Scope…","fullName":"Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e)","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e154","label":"Identificeer uitstootbr…","fullName":"Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen..html"},{"id":"e197","label":"Brandstofgebruik (Scope…","fullName":"Brandstofgebruik (Scope 1)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Brandstofgebruik (Scope 1).html"},{"id":"e198","label":"Elektriciteitsverbruik …","fullName":"Elektriciteitsverbruik (Scope 2)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Elektriciteitsverbruik (Scope 2).html"},{"id":"e199","label":"Leveranciers-, transpor…","fullName":"Leveranciers-, transport-, gebruiksdata (Scope 3)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Leveranciers-, transport-, gebruiksdata (Scope 3).html"}],"edges":[{"id":"c105","source":"e36","target":"e180","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c132","source":"e154","target":"e180","label":"Association","sourceLayer":"edgy-ex"},{"id":"c198","source":"e180","target":"e197","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c199","source":"e180","target":"e198","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c200","source":"e180","target":"e199","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c232","source":"e180","target":"e233","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 10:25:43*