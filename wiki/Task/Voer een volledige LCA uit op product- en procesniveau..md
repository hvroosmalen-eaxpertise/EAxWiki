---
ea_id: 153
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-ex">Task</span> Voer een volledige LCA uit op product- en procesniveau.

**Type:** Activity  **Stereotype:** Task  **StereotypeEx:** Task  **FQStereotype:** EDGY::Task  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="153" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Task/Voer een volledige LCA uit op product- en procesniveau..md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
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
| ControlFlow | Flow | [Environmental Impact Map](../Asset/Environmental Impact Map.md) |
| Association | Link | [Totaal afval (ton/jaar)](../Metrics/Totaal afval (ton_jaar).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Architecture/diagrams/Architecture.html" class="diagram-thumb"><img src="../Architecture/diagrams/Architecture.png" alt="Architecture" loading="lazy"><span>Architecture</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Environmental Impact Map](../Asset/Environmental Impact Map.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e132","label":"Environmental Impact Map","fullName":"Environmental Impact Map","packageName":"Asset","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Asset/Environmental Impact Map.html"},{"id":"e179","label":"Totaal afval (ton/jaar)","fullName":"Totaal afval (ton/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Totaal afval (ton_jaar).html"},{"id":"e153","label":"Voer een volledige LCA …","fullName":"Voer een volledige LCA uit op product- en procesniveau.","packageName":"Task","layer":"edgy-ex","isFocal":true,"hasUrl":false,"url":""},{"id":"e14","label":"SDG 12. Responsible Con…","fullName":"SDG 12. Responsible Consumption and Production","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 12. Responsible Consumption and Production.html"},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e194","label":"Energie, materiaal, wat…","fullName":"Energie, materiaal, water, afval per eenheid","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Energie, materiaal, water, afval per eenheid.html"},{"id":"e195","label":"CO₂e per processtap of …","fullName":"CO₂e per processtap of productlevensfase","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/CO₂e per processtap of productlevensfase.html"},{"id":"e196","label":"Outputvolumes voor norm…","fullName":"Outputvolumes voor normalisatie","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Outputvolumes voor normalisatie.html"},{"id":"e232","label":"Product Carbon Footprin…","fullName":"Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Product Carbon Footprint = totale CO₂e _ aantal eenheden geproduceerd.html"}],"edges":[{"id":"c47","source":"e132","target":"e153","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c75","source":"e14","target":"e132","label":"Association","sourceLayer":"edgy-id"},{"id":"c76","source":"e15","target":"e132","label":"Association","sourceLayer":"edgy-id"},{"id":"c104","source":"e40","target":"e179","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c131","source":"e153","target":"e179","label":"Association","sourceLayer":"edgy-ex"},{"id":"c195","source":"e179","target":"e194","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c196","source":"e179","target":"e195","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c197","source":"e179","target":"e196","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c231","source":"e179","target":"e232","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 12:05:09*