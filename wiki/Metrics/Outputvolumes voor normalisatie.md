# <span class="sl" data-layer="edgy-lb">Metric</span> Outputvolumes voor normalisatie

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
| Aggregation | Tree | [Totaal afval (ton/jaar)](Totaal afval (ton_jaar).md) |
| Association | Link | [LCA-software + leveranciersdata](LCA-software + leveranciersdata.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Metrics](diagrams/Metrics.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [Totaal afval (ton/jaar)](Totaal afval (ton_jaar).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e179","label":"Totaal afval (ton/jaar)","fullName":"Totaal afval (ton/jaar)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"Totaal afval (ton_jaar).html"},{"id":"e216","label":"LCA-software + leveranc…","fullName":"LCA-software + leveranciersdata","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"LCA-software + leveranciersdata.html"},{"id":"e196","label":"Outputvolumes voor norm…","fullName":"Outputvolumes voor normalisatie","packageName":"Metrics","isFocal":true,"hasUrl":false,"url":""},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e153","label":"Voer een volledige LCA …","fullName":"Voer een volledige LCA uit op product- en procesniveau.","packageName":"Task","isFocal":false,"hasUrl":true,"url":"../Task/Voer een volledige LCA uit op product- en procesniveau..html"},{"id":"e194","label":"Energie, materiaal, wat…","fullName":"Energie, materiaal, water, afval per eenheid","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"Energie, materiaal, water, afval per eenheid.html"},{"id":"e195","label":"CO₂e per processtap of …","fullName":"CO₂e per processtap of productlevensfase","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"CO₂e per processtap of productlevensfase.html"},{"id":"e232","label":"Product Carbon Footprin…","fullName":"Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"Product Carbon Footprint = totale CO₂e _ aantal eenheden geproduceerd.html"}],"edges":[{"id":"c104","source":"e40","target":"e179","label":"ControlFlow"},{"id":"c131","source":"e153","target":"e179","label":"Association"},{"id":"c195","source":"e179","target":"e194","label":"Aggregation"},{"id":"c196","source":"e179","target":"e195","label":"Aggregation"},{"id":"c197","source":"e179","target":"e196","label":"Aggregation"},{"id":"c231","source":"e179","target":"e232","label":"ControlFlow"},{"id":"c217","source":"e196","target":"e216","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:38*