# <span class="sl" data-layer="uml">MET</span> Leveranciers-, transport-, gebruiksdata (Scope 3)

**Type:** Requirement  **Stereotype:** Metric  **Status:** <span class="status-badge status-proposed">Proposed</span>  
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
| Association | Link | [ERP + Scope 3-calculatietools](ERP + Scope 3-calculatietools.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Metrics](diagrams/Metrics.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [Scope 1 uitstoot per bron (ton CO₂e)](Scope 1 uitstoot per bron (ton CO₂e).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e180","label":"Scope 1 uitstoot per br…","fullName":"Scope 1 uitstoot per bron (ton CO₂e)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Scope 1 uitstoot per bron (ton CO₂e).html"},{"id":"e220","label":"ERP + Scope 3-calculati…","fullName":"ERP + Scope 3-calculatietools","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"ERP + Scope 3-calculatietools.html"},{"id":"e199","label":"Leveranciers-, transpor…","fullName":"Leveranciers-, transport-, gebruiksdata (Scope 3)","packageName":Metrics,"isFocal":true,"hasUrl":false,"url":""},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":ESRS E1,"isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e154","label":"Identificeer uitstootbr…","fullName":"Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen.","packageName":Task,"isFocal":false,"hasUrl":true,"url":"../Task/Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen..html"},{"id":"e197","label":"Brandstofgebruik (Scope…","fullName":"Brandstofgebruik (Scope 1)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Brandstofgebruik (Scope 1).html"},{"id":"e198","label":"Elektriciteitsverbruik …","fullName":"Elektriciteitsverbruik (Scope 2)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Elektriciteitsverbruik (Scope 2).html"},{"id":"e233","label":"Totale uitstoot = Scope…","fullName":"Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e).html"}],"edges":[{"id":"c105","source":"e36","target":"e180","label":"ControlFlow"},{"id":"c132","source":"e154","target":"e180","label":"Association"},{"id":"c198","source":"e180","target":"e197","label":"Aggregation"},{"id":"c199","source":"e180","target":"e198","label":"Aggregation"},{"id":"c200","source":"e180","target":"e199","label":"Aggregation"},{"id":"c232","source":"e180","target":"e233","label":"ControlFlow"},{"id":"c220","source":"e199","target":"e220","label":"Association"}]};</script>

---

*Generated: 2026-06-26 09:44:48*