# <span class="sl" data-layer="uml">MET</span> Afval per fase + recycleerbaar aandeel

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
| Aggregation | Tree | [% gerecyclede materialen in producten](% gerecyclede materialen in producten.md) |
| Association | Link | [Slimme afvalweegschalen, product-IoT](Slimme afvalweegschalen, product-IoT.md) |

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
<script>window.eaGraphData={"nodes":[{"id":"e182","label":"% gerecyclede materiale…","fullName":"% gerecyclede materialen in producten","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% gerecyclede materialen in producten.html"},{"id":"e226","label":"Slimme afvalweegschalen…","fullName":"Slimme afvalweegschalen, product-IoT","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Slimme afvalweegschalen, product-IoT.html"},{"id":"e205","label":"Afval per fase + recycl…","fullName":"Afval per fase + recycleerbaar aandeel","packageName":Metrics,"isFocal":true,"hasUrl":false,"url":""},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":ESRS E5,"isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e156","label":"Ontwerp producten die m…","fullName":"Ontwerp producten die modulair, repareerbaar en recyclebaar zijn.","packageName":Task,"isFocal":false,"hasUrl":true,"url":"../Task/Ontwerp producten die modulair, repareerbaar en recyclebaar zijn..html"},{"id":"e203","label":"Massa gerecycled materi…","fullName":"Massa gerecycled materiaal (kg/% input)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Massa gerecycled materiaal (kg_% input).html"},{"id":"e204","label":"Retourpercentages produ…","fullName":"Retourpercentages producten","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Retourpercentages producten.html"},{"id":"e206","label":" Levensduur product","fullName":" Levensduur product","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Levensduur product.html"},{"id":"e235","label":"% Gerecycled materiaal …","fullName":"% Gerecycled materiaal = (gerecycled / totaal input) × 100","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% Gerecycled materiaal = (gerecycled _ totaal input) × 100.html"}],"edges":[{"id":"c107","source":"e40","target":"e182","label":"ControlFlow"},{"id":"c134","source":"e156","target":"e182","label":"Association"},{"id":"c204","source":"e182","target":"e203","label":"Aggregation"},{"id":"c205","source":"e182","target":"e204","label":"Aggregation"},{"id":"c206","source":"e182","target":"e205","label":"Aggregation"},{"id":"c207","source":"e182","target":"e206","label":"Aggregation"},{"id":"c234","source":"e182","target":"e235","label":"ControlFlow"},{"id":"c226","source":"e205","target":"e226","label":"Association"}]};</script>

---

*Generated: 2026-06-26 09:44:48*