# <span class="sl" data-layer="uml">MET</span> Totaal afval (ton/jaar)

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
| ControlFlow | Flow | [ESRS E5 Resource Use and Circular Economy](../ESRS E5/ESRS E5 Resource Use and Circular Economy.md) |
| Association | Link | [Voer een volledige LCA uit op product- en procesniveau.](../Task/Voer een volledige LCA uit op product- en procesniveau..md) |
| Aggregation | Tree | [Energie, materiaal, water, afval per eenheid](Energie, materiaal, water, afval per eenheid.md) |
| Aggregation | Tree | [CO₂e per processtap of productlevensfase](CO₂e per processtap of productlevensfase.md) |
| Aggregation | Tree | [Outputvolumes voor normalisatie](Outputvolumes voor normalisatie.md) |
| ControlFlow | Flow | [Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd](Product Carbon Footprint = totale CO₂e _ aantal eenheden geproduceerd.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)
- [Metrics](diagrams/Metrics.md)

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
<script>window.eaGraphData={"nodes":[{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":ESRS E5,"isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"},{"id":"e153","label":"Voer een volledige LCA …","fullName":"Voer een volledige LCA uit op product- en procesniveau.","packageName":Task,"isFocal":false,"hasUrl":true,"url":"../Task/Voer een volledige LCA uit op product- en procesniveau..html"},{"id":"e194","label":"Energie, materiaal, wat…","fullName":"Energie, materiaal, water, afval per eenheid","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Energie, materiaal, water, afval per eenheid.html"},{"id":"e195","label":"CO₂e per processtap of …","fullName":"CO₂e per processtap of productlevensfase","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"CO₂e per processtap of productlevensfase.html"},{"id":"e196","label":"Outputvolumes voor norm…","fullName":"Outputvolumes voor normalisatie","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Outputvolumes voor normalisatie.html"},{"id":"e232","label":"Product Carbon Footprin…","fullName":"Product Carbon Footprint = totale CO₂e / aantal eenheden geproduceerd","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Product Carbon Footprint = totale CO₂e _ aantal eenheden geproduceerd.html"},{"id":"e179","label":"Totaal afval (ton/jaar)","fullName":"Totaal afval (ton/jaar)","packageName":Metrics,"isFocal":true,"hasUrl":false,"url":""},{"id":"e118","label":"% hernieuwbare grondsto…","fullName":"% hernieuwbare grondstoffen","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% hernieuwbare grondstoffen.html"},{"id":"e167","label":"% producten met circula…","fullName":"% producten met circulair ontwerp","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% producten met circulair ontwerp.html"},{"id":"e170","label":"% minder plasticverpakk…","fullName":"% minder plasticverpakkingen","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% minder plasticverpakkingen.html"},{"id":"e171","label":"# teruggenomen producte…","fullName":"# teruggenomen producten (per jaar)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"_ teruggenomen producten (per jaar).html"},{"id":"e174","label":"% omzet uit duurzame pr…","fullName":"% omzet uit duurzame producten","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% omzet uit duurzame producten.html"},{"id":"e175","label":"% R&D naar duurzame inn…","fullName":"% R&D naar duurzame innovatie","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% R&D naar duurzame innovatie.html"},{"id":"e182","label":"% gerecyclede materiale…","fullName":"% gerecyclede materialen in producten","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"% gerecyclede materialen in producten.html"},{"id":"e185","label":"# SDGs met meetbare KPI…","fullName":"# SDGs met meetbare KPI’s","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"_ SDGs met meetbare KPI’s.html"},{"id":"e186","label":"Energie (MWh/jaar)","fullName":"Energie (MWh/jaar)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Energie (MWh_jaar).html"},{"id":"e187","label":"Recycling % van afval","fullName":"Recycling % van afval","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Recycling % van afval.html"},{"id":"e188","label":"Restafval per medewerke…","fullName":"Restafval per medewerker (kg/jaar)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Restafval per medewerker (kg_jaar).html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":People,"isFocal":false,"hasUrl":true,"url":"../People/Company subject to CSRD.html"},{"id":"e295","label":"ESRS E5 - Circular Econ…","fullName":"ESRS E5 - Circular Economy","packageName":ESRS Navigator Stakeholder Map,"isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/ESRS E5 - Circular Economy.html"},{"id":"e531","label":"European Commission","fullName":"European Commission","packageName":People,"isFocal":false,"hasUrl":true,"url":"../People/European Commission.html"},{"id":"e555","label":"European Sustainability…","fullName":"European Sustainability Reporting Standards","packageName":European Sustainability Reporting Standards,"isFocal":false,"hasUrl":true,"url":"../European Sustainability Reporting Standards/European Sustainability Reporting Standards.html"},{"id":"e132","label":"Environmental Impact Map","fullName":"Environmental Impact Map","packageName":Asset,"isFocal":false,"hasUrl":true,"url":"../Asset/Environmental Impact Map.html"},{"id":"e214","label":"IoT op apparatuur","fullName":"IoT op apparatuur","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"IoT op apparatuur.html"},{"id":"e215","label":"Productiesoftware (SCAD…","fullName":"Productiesoftware (SCADA, MES)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Productiesoftware (SCADA, MES).html"},{"id":"e216","label":"LCA-software + leveranc…","fullName":"LCA-software + leveranciersdata","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"LCA-software + leveranciersdata.html"}],"edges":[{"id":"c33","source":"e40","target":"e118","label":"ControlFlow"},{"id":"c91","source":"e40","target":"e167","label":"ControlFlow"},{"id":"c94","source":"e40","target":"e170","label":"ControlFlow"},{"id":"c95","source":"e40","target":"e171","label":"ControlFlow"},{"id":"c98","source":"e40","target":"e174","label":"ControlFlow"},{"id":"c99","source":"e40","target":"e175","label":"ControlFlow"},{"id":"c104","source":"e40","target":"e179","label":"ControlFlow"},{"id":"c107","source":"e40","target":"e182","label":"ControlFlow"},{"id":"c113","source":"e40","target":"e185","label":"ControlFlow"},{"id":"c116","source":"e40","target":"e186","label":"ControlFlow"},{"id":"c118","source":"e40","target":"e187","label":"ControlFlow"},{"id":"c119","source":"e40","target":"e188","label":"ControlFlow"},{"id":"c560","source":"e510","target":"e40","label":"reports according to"},{"id":"c570","source":"e295","target":"e40","label":"Abstraction"},{"id":"c590","source":"e531","target":"e40","label":"Association"},{"id":"c603","source":"e555","target":"e40","label":"Aggregation"},{"id":"c47","source":"e132","target":"e153","label":"ControlFlow"},{"id":"c131","source":"e153","target":"e179","label":"Association"},{"id":"c195","source":"e179","target":"e194","label":"Aggregation"},{"id":"c215","source":"e194","target":"e214","label":"Association"},{"id":"c196","source":"e179","target":"e195","label":"Aggregation"},{"id":"c216","source":"e195","target":"e215","label":"Association"},{"id":"c197","source":"e179","target":"e196","label":"Aggregation"},{"id":"c217","source":"e196","target":"e216","label":"Association"},{"id":"c231","source":"e179","target":"e232","label":"ControlFlow"}]};</script>

---

*Generated: 2026-06-26 09:44:48*