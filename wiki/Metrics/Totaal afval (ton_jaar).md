---
ea_id: 179
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Metric</span> Totaal afval (ton/jaar)

**Type:** Requirement  **Stereotype:** Metric  **StereotypeEx:** Metric  **FQStereotype:** EDGY::Metric  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="179" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Metrics/Totaal afval (ton_jaar).md" data-api-port="8001"></div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::MetricStatus | Good | Default: Good  |
| EDGY::MetricValue | <VALUE> | Default: <VALUE>  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [ESRS E5 Resource Use and Circular Economy](../ESRS E5/ESRS E5 Resource Use and Circular Economy.md) |
| Association | Link | [Voer een volledige LCA uit op product- en procesniveau.](../Task/Voer een volledige LCA uit op product- en procesniveau..md) |
| Aggregation | Tree | [Energie, materiaal, water, afval per eenheid](Energie, materiaal, water, afval per eenheid.md) |
| Aggregation | Tree | [CO₂e per processtap of productlevensfase](CO₂e per processtap of productlevensfase.md) |
| Aggregation | Tree | [Outputvolumes voor normalisatie](Outputvolumes voor normalisatie.md) |
| ControlFlow | Flow | [Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd](Product Carbon Footprint = totale CO₂e _ aantal eenheden geproduceerd.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Architecture/diagrams/Architecture.html" class="diagram-thumb"><img src="../Architecture/diagrams/Architecture.png" alt="Architecture" loading="lazy"><span>Architecture</span></a>
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [ESRS E5 Resource Use and Circular Economy](../ESRS E5/ESRS E5 Resource Use and Circular Economy.md) |
| Association | Link | [Voer een volledige LCA uit op product- en procesniveau.](../Task/Voer een volledige LCA uit op product- en procesniveau..md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e153","label":"Voer een volledige LCA …","fullName":"Voer een volledige LCA uit op product- en procesniveau.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Voer een volledige LCA uit op product- en procesniveau..html"},{"id":"e194","label":"Energie, materiaal, wat…","fullName":"Energie, materiaal, water, afval per eenheid","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energie, materiaal, water, afval per eenheid.html"},{"id":"e195","label":"CO₂e per processtap of …","fullName":"CO₂e per processtap of productlevensfase","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂e per processtap of productlevensfase.html"},{"id":"e196","label":"Outputvolumes voor norm…","fullName":"Outputvolumes voor normalisatie","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Outputvolumes voor normalisatie.html"},{"id":"e232","label":"Product Carbon Footprin…","fullName":"Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Product Carbon Footprint = totale CO₂e _ aantal eenheden geproduceerd.html"},{"id":"e179","label":"Totaal afval (ton/jaar)","fullName":"Totaal afval (ton/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e118","label":"% hernieuwbare grondsto…","fullName":"% hernieuwbare grondstoffen","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% hernieuwbare grondstoffen.html"},{"id":"e167","label":"% producten met circula…","fullName":"% producten met circulair ontwerp","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% producten met circulair ontwerp.html"},{"id":"e170","label":"% minder plasticverpakk…","fullName":"% minder plasticverpakkingen","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% minder plasticverpakkingen.html"},{"id":"e171","label":"# teruggenomen producte…","fullName":"# teruggenomen producten (per jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"_ teruggenomen producten (per jaar).html"},{"id":"e174","label":"% omzet uit duurzame pr…","fullName":"% omzet uit duurzame producten","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% omzet uit duurzame producten.html"},{"id":"e175","label":"% R&amp;D naar duurzame inn…","fullName":"% R&amp;D naar duurzame innovatie","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% R&amp;D naar duurzame innovatie.html"},{"id":"e182","label":"% gerecyclede materiale…","fullName":"% gerecyclede materialen in producten","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% gerecyclede materialen in producten.html"},{"id":"e185","label":"# SDGs met meetbare KPI…","fullName":"# SDGs met meetbare KPI’s","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"_ SDGs met meetbare KPI’s.html"},{"id":"e186","label":"Energie (MWh/jaar)","fullName":"Energie (MWh/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energie (MWh_jaar).html"},{"id":"e187","label":"Recycling % van afval","fullName":"Recycling % van afval","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Recycling % van afval.html"},{"id":"e188","label":"Restafval per medewerke…","fullName":"Restafval per medewerker (kg/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Restafval per medewerker (kg_jaar).html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Company subject to CSRD.html"},{"id":"e295","label":"ESRS E5 - Circular Econ…","fullName":"ESRS E5 - Circular Economy","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/ESRS E5 - Circular Economy.html"},{"id":"e531","label":"European Commission","fullName":"European Commission","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/European Commission.html"},{"id":"e555","label":"European Sustainability…","fullName":"European Sustainability Reporting Standards","packageName":"European Sustainability Reporting Standards","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../European Sustainability Reporting Standards/European Sustainability Reporting Standards.html"},{"id":"e132","label":"Environmental Impact Map","fullName":"Environmental Impact Map","packageName":"Asset","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Asset/Environmental Impact Map.html"},{"id":"e214","label":"IoT op apparatuur","fullName":"IoT op apparatuur","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"IoT op apparatuur.html"},{"id":"e215","label":"Productiesoftware (SCAD…","fullName":"Productiesoftware (SCADA, MES)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Productiesoftware (SCADA, MES).html"},{"id":"e216","label":"LCA-software + leveranc…","fullName":"LCA-software + leveranciersdata","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"LCA-software + leveranciersdata.html"}],"edges":[{"id":"c33","source":"e40","target":"e118","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c91","source":"e40","target":"e167","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c94","source":"e40","target":"e170","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c95","source":"e40","target":"e171","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c98","source":"e40","target":"e174","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c99","source":"e40","target":"e175","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c104","source":"e40","target":"e179","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c107","source":"e40","target":"e182","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c113","source":"e40","target":"e185","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c116","source":"e40","target":"e186","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c118","source":"e40","target":"e187","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c119","source":"e40","target":"e188","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c560","source":"e510","target":"e40","label":"reports according to","sourceLayer":"edgy-pe"},{"id":"c570","source":"e295","target":"e40","label":"Abstraction","sourceLayer":"business"},{"id":"c590","source":"e531","target":"e40","label":"Association","sourceLayer":"edgy-pe"},{"id":"c603","source":"e555","target":"e40","label":"Aggregation","sourceLayer":"edgy-id"},{"id":"c47","source":"e132","target":"e153","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c131","source":"e153","target":"e179","label":"Association","sourceLayer":"edgy-ex"},{"id":"c195","source":"e179","target":"e194","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c215","source":"e194","target":"e214","label":"Association","sourceLayer":"edgy-lb"},{"id":"c196","source":"e179","target":"e195","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c216","source":"e195","target":"e215","label":"Association","sourceLayer":"edgy-lb"},{"id":"c197","source":"e179","target":"e196","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c217","source":"e196","target":"e216","label":"Association","sourceLayer":"edgy-lb"},{"id":"c231","source":"e179","target":"e232","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 10:25:43*