# <span class="sl" data-layer="edgy-lb">Metric</span> Massa gerecycled materiaal (kg/% input)

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
| Aggregation | Tree | [% gerecyclede materialen in producten](% gerecyclede materialen in producten.md) |
| Association | Link | [ERP/BOM met herkomst info](ERP_BOM met herkomst info.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Metrics](diagrams/Metrics.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [% gerecyclede materialen in producten](% gerecyclede materialen in producten.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e182","label":"% gerecyclede materiale…","fullName":"% gerecyclede materialen in producten","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"% gerecyclede materialen in producten.html"},{"id":"e224","label":"ERP/BOM met herkomst in…","fullName":"ERP/BOM met herkomst info","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"ERP_BOM met herkomst info.html"},{"id":"e203","label":"Massa gerecycled materi…","fullName":"Massa gerecycled materiaal (kg/% input)","packageName":"Metrics","isFocal":true,"hasUrl":false,"url":""},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e156","label":"Ontwerp producten die m…","fullName":"Ontwerp producten die modulair, repareerbaar en recyclebaar zijn.","packageName":"Task","isFocal":false,"hasUrl":true,"url":"../Task/Ontwerp producten die modulair, repareerbaar en recyclebaar zijn..html"},{"id":"e204","label":"Retourpercentages produ…","fullName":"Retourpercentages producten","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"Retourpercentages producten.html"},{"id":"e205","label":"Afval per fase + recycl…","fullName":"Afval per fase + recycleerbaar aandeel","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"Afval per fase + recycleerbaar aandeel.html"},{"id":"e206","label":" Levensduur product","fullName":" Levensduur product","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"Levensduur product.html"},{"id":"e235","label":"% Gerecycled materiaal …","fullName":"% Gerecycled materiaal = (gerecycled / totaal input) × 100","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"% Gerecycled materiaal = (gerecycled _ totaal input) × 100.html"}],"edges":[{"id":"c107","source":"e40","target":"e182","label":"ControlFlow"},{"id":"c134","source":"e156","target":"e182","label":"Association"},{"id":"c204","source":"e182","target":"e203","label":"Aggregation"},{"id":"c205","source":"e182","target":"e204","label":"Aggregation"},{"id":"c206","source":"e182","target":"e205","label":"Aggregation"},{"id":"c207","source":"e182","target":"e206","label":"Aggregation"},{"id":"c234","source":"e182","target":"e235","label":"ControlFlow"},{"id":"c224","source":"e203","target":"e224","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:38*