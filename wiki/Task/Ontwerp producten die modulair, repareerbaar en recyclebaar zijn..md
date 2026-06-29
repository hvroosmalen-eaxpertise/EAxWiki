# <span class="sl" data-layer="edgy-ex">Task</span> Ontwerp producten die modulair, repareerbaar en recyclebaar zijn.

**Type:** Activity  **Stereotype:** Task  **StereotypeEx:** Task  **FQStereotype:** EDGY::Task  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-02  **Modified:** 2025-12-15


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Experience](../Experience/index.md) / [Task](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Top | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [Lifecycle & Circularity Model](../Process/Lifecycle & Circularity Model.md) |
| Association | Link | [% gerecyclede materialen in producten](../Metrics/% gerecyclede materialen in producten.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Lifecycle & Circularity Model](../Process/Lifecycle & Circularity Model.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e135","label":"Lifecycle &amp; Circularity…","fullName":"Lifecycle &amp; Circularity Model","packageName":"Process","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Process/Lifecycle &amp; Circularity Model.html"},{"id":"e182","label":"% gerecyclede materiale…","fullName":"% gerecyclede materialen in producten","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/% gerecyclede materialen in producten.html"},{"id":"e156","label":"Ontwerp producten die m…","fullName":"Ontwerp producten die modulair, repareerbaar en recyclebaar zijn.","packageName":"Task","layer":"edgy-ex","isFocal":true,"hasUrl":false,"url":""},{"id":"e14","label":"SDG 12. Responsible Con…","fullName":"SDG 12. Responsible Consumption and Production","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 12. Responsible Consumption and Production.html"},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e203","label":"Massa gerecycled materi…","fullName":"Massa gerecycled materiaal (kg/% input)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Massa gerecycled materiaal (kg_% input).html"},{"id":"e204","label":"Retourpercentages produ…","fullName":"Retourpercentages producten","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Retourpercentages producten.html"},{"id":"e205","label":"Afval per fase + recycl…","fullName":"Afval per fase + recycleerbaar aandeel","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Afval per fase + recycleerbaar aandeel.html"},{"id":"e206","label":" Levensduur product","fullName":" Levensduur product","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Levensduur product.html"},{"id":"e235","label":"% Gerecycled materiaal …","fullName":"% Gerecycled materiaal = (gerecycled / totaal input) × 100","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/% Gerecycled materiaal = (gerecycled _ totaal input) × 100.html"}],"edges":[{"id":"c50","source":"e135","target":"e156","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c79","source":"e14","target":"e135","label":"Association","sourceLayer":"edgy-id"},{"id":"c107","source":"e40","target":"e182","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c134","source":"e156","target":"e182","label":"Association","sourceLayer":"edgy-ex"},{"id":"c204","source":"e182","target":"e203","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c205","source":"e182","target":"e204","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c206","source":"e182","target":"e205","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c207","source":"e182","target":"e206","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c234","source":"e182","target":"e235","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-06-29 13:30:16*