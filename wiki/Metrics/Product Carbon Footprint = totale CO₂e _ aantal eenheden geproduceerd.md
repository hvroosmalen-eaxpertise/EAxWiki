---
ea_id: 232
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
---

# <span class="sl" data-layer="edgy-lb">Outcome</span> Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd

**Type:** Activity  **Stereotype:** Outcome  **StereotypeEx:** Outcome  **FQStereotype:** EDGY::Outcome  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Center | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [Totaal afval (ton/jaar)](Totaal afval (ton_jaar).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Totaal afval (ton/jaar)](Totaal afval (ton_jaar).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e179","label":"Totaal afval (ton/jaar)","fullName":"Totaal afval (ton/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Totaal afval (ton_jaar).html"},{"id":"e232","label":"Product Carbon Footprin…","fullName":"Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e153","label":"Voer een volledige LCA …","fullName":"Voer een volledige LCA uit op product- en procesniveau.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Voer een volledige LCA uit op product- en procesniveau..html"},{"id":"e194","label":"Energie, materiaal, wat…","fullName":"Energie, materiaal, water, afval per eenheid","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energie, materiaal, water, afval per eenheid.html"},{"id":"e195","label":"CO₂e per processtap of …","fullName":"CO₂e per processtap of productlevensfase","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂e per processtap of productlevensfase.html"},{"id":"e196","label":"Outputvolumes voor norm…","fullName":"Outputvolumes voor normalisatie","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Outputvolumes voor normalisatie.html"}],"edges":[{"id":"c104","source":"e40","target":"e179","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c131","source":"e153","target":"e179","label":"Association","sourceLayer":"edgy-ex"},{"id":"c195","source":"e179","target":"e194","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c196","source":"e179","target":"e195","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c197","source":"e179","target":"e196","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c231","source":"e179","target":"e232","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-06-30 14:47:47*