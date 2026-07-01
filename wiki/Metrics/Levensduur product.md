---
ea_id: 206
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Metric</span>  Levensduur product

**Type:** Requirement  **Stereotype:** Metric  **StereotypeEx:** Metric  **FQStereotype:** EDGY::Metric  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="206" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Metrics/Levensduur product.md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
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

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [% gerecyclede materialen in producten](% gerecyclede materialen in producten.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e182","label":"% gerecyclede materiale…","fullName":"% gerecyclede materialen in producten","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% gerecyclede materialen in producten.html"},{"id":"e206","label":" Levensduur product","fullName":" Levensduur product","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e156","label":"Ontwerp producten die m…","fullName":"Ontwerp producten die modulair, repareerbaar en recyclebaar zijn.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Ontwerp producten die modulair, repareerbaar en recyclebaar zijn..html"},{"id":"e203","label":"Massa gerecycled materi…","fullName":"Massa gerecycled materiaal (kg/% input)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Massa gerecycled materiaal (kg_% input).html"},{"id":"e204","label":"Retourpercentages produ…","fullName":"Retourpercentages producten","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Retourpercentages producten.html"},{"id":"e205","label":"Afval per fase + recycl…","fullName":"Afval per fase + recycleerbaar aandeel","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Afval per fase + recycleerbaar aandeel.html"},{"id":"e235","label":"% Gerecycled materiaal …","fullName":"% Gerecycled materiaal = (gerecycled / totaal input) × 100","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% Gerecycled materiaal = (gerecycled _ totaal input) × 100.html"}],"edges":[{"id":"c107","source":"e40","target":"e182","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c134","source":"e156","target":"e182","label":"Association","sourceLayer":"edgy-ex"},{"id":"c204","source":"e182","target":"e203","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c205","source":"e182","target":"e204","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c206","source":"e182","target":"e205","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c207","source":"e182","target":"e206","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c234","source":"e182","target":"e235","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 11:29:53*