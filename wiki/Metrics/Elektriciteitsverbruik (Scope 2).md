# <span class="sl" data-layer="edgy-lb">Metric</span> Elektriciteitsverbruik (Scope 2)

**Type:** Requirement  **Stereotype:** Metric  **StereotypeEx:** Metric  **FQStereotype:** EDGY::Metric  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::MetricStatus | Good | Default: Good  |
| EDGY::MetricValue | <VALUE> | Default: <VALUE>  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Aggregation | Tree | [Scope 1 uitstoot per bron (ton CO₂e)](Scope 1 uitstoot per bron (ton CO₂e).md) |
| Association | Link | [Slimme meters, energienota’s](Slimme meters, energienota’s.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb diagram-thumb--noimg"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [Scope 1 uitstoot per bron (ton CO₂e)](Scope 1 uitstoot per bron (ton CO₂e).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e180","label":"Scope 1 uitstoot per br…","fullName":"Scope 1 uitstoot per bron (ton CO₂e)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Scope 1 uitstoot per bron (ton CO₂e).html"},{"id":"e219","label":"Slimme meters, energien…","fullName":"Slimme meters, energienota’s","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Slimme meters, energienota’s.html"},{"id":"e198","label":"Elektriciteitsverbruik …","fullName":"Elektriciteitsverbruik (Scope 2)","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e154","label":"Identificeer uitstootbr…","fullName":"Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen..html"},{"id":"e197","label":"Brandstofgebruik (Scope…","fullName":"Brandstofgebruik (Scope 1)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Brandstofgebruik (Scope 1).html"},{"id":"e199","label":"Leveranciers-, transpor…","fullName":"Leveranciers-, transport-, gebruiksdata (Scope 3)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Leveranciers-, transport-, gebruiksdata (Scope 3).html"},{"id":"e233","label":"Totale uitstoot = Scope…","fullName":"Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e).html"}],"edges":[{"id":"c105","source":"e36","target":"e180","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c132","source":"e154","target":"e180","label":"Association","sourceLayer":"edgy-ex"},{"id":"c198","source":"e180","target":"e197","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c199","source":"e180","target":"e198","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c200","source":"e180","target":"e199","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c232","source":"e180","target":"e233","label":"ControlFlow","sourceLayer":"edgy-lb"},{"id":"c219","source":"e198","target":"e219","label":"Association","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-06-29 18:57:22*