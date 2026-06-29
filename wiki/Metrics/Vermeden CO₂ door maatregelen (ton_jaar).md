# <span class="sl" data-layer="edgy-lb">Metric</span> Vermeden CO₂ door maatregelen (ton/jaar)

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
| ControlFlow | Flow | [ESRS E1 Climate Change](../ESRS E1/ESRS E1 Climate Change.md) |
| Association | Link | [Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.](../Task/Start projecten voor energiebesparing, transportoptimalisatie en circulariteit..md) |
| Aggregation | Tree | [Energieverbruik vóór/na project](Energieverbruik vóór_na project.md) |
| Aggregation | Tree | [Emissie vóór/na project](Emissie vóór_na project.md) |
| Aggregation | Tree | [Projectgegevens en tech specs](Projectgegevens en tech specs.md) |
| ControlFlow | Flow | [Emissiereductie (%) = ((baseline – huidig) / baseline) × 100](Emissiereductie (%) = ((baseline – huidig) _ baseline) × 100.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Architecture/diagrams/Architecture.html" class="diagram-thumb diagram-thumb--noimg"><span>Architecture</span></a>
  <a href="diagrams/Metrics.html" class="diagram-thumb diagram-thumb--noimg"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [ESRS E1 Climate Change](../ESRS E1/ESRS E1 Climate Change.md) |
| Association | Link | [Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.](../Task/Start projecten voor energiebesparing, transportoptimalisatie en circulariteit..md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e155","label":"Start projecten voor en…","fullName":"Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Start projecten voor energiebesparing, transportoptimalisatie en circulariteit..html"},{"id":"e200","label":"Energieverbruik vóór/na…","fullName":"Energieverbruik vóór/na project","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energieverbruik vóór_na project.html"},{"id":"e201","label":"Emissie vóór/na project","fullName":"Emissie vóór/na project","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Emissie vóór_na project.html"},{"id":"e202","label":"Projectgegevens en tech…","fullName":"Projectgegevens en tech specs","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Projectgegevens en tech specs.html"},{"id":"e234","label":"Emissiereductie (%) = (…","fullName":"Emissiereductie (%) = ((baseline – huidig) / baseline) × 100","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Emissiereductie (%) = ((baseline – huidig) _ baseline) × 100.html"},{"id":"e181","label":"Vermeden CO₂ door maatr…","fullName":"Vermeden CO₂ door maatregelen (ton/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e111","label":"Totale Scope 1-3 uitsto…","fullName":"Totale Scope 1-3 uitstoot (ton CO₂e/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Totale Scope 1-3 uitstoot (ton CO₂e_jaar).html"},{"id":"e115","label":"Reductiedoel CO₂ (% t.o…","fullName":"Reductiedoel CO₂ (% t.o.v. basisjaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Reductiedoel CO₂ (% t.o.v. basisjaar).html"},{"id":"e117","label":"Jaarlijkse CO₂-reductie…","fullName":"Jaarlijkse CO₂-reductie (%)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Jaarlijkse CO₂-reductie (%).html"},{"id":"e169","label":"CO₂-intensiteit per € o…","fullName":"CO₂-intensiteit per € omzet","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂-intensiteit per € omzet.html"},{"id":"e172","label":"CO₂ per klanttransactie…","fullName":"CO₂ per klanttransactie (kg)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂ per klanttransactie (kg).html"},{"id":"e173","label":"CO₂ bespaard door digit…","fullName":"CO₂ bespaard door digitalisering (ton)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂ bespaard door digitalisering (ton).html"},{"id":"e176","label":"Energieverbruik en wate…","fullName":"Energieverbruik en waterverbruik per eenheid","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energieverbruik en waterverbruik per eenheid.html"},{"id":"e177","label":"% energie uit hernieuwb…","fullName":"% energie uit hernieuwbare bronnen","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% energie uit hernieuwbare bronnen.html"},{"id":"e178","label":"% reductie reisgerelate…","fullName":"% reductie reisgerelateerde CO₂","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% reductie reisgerelateerde CO₂.html"},{"id":"e180","label":"Scope 1 uitstoot per br…","fullName":"Scope 1 uitstoot per bron (ton CO₂e)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Scope 1 uitstoot per bron (ton CO₂e).html"},{"id":"e183","label":"% scope 3 emissies met …","fullName":"% scope 3 emissies met gemeten data","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% scope 3 emissies met gemeten data.html"},{"id":"e185","label":"# SDGs met meetbare KPI…","fullName":"# SDGs met meetbare KPI’s","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"_ SDGs met meetbare KPI’s.html"},{"id":"e186","label":"Energie (MWh/jaar)","fullName":"Energie (MWh/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energie (MWh_jaar).html"},{"id":"e187","label":"Recycling % van afval","fullName":"Recycling % van afval","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Recycling % van afval.html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Company subject to CSRD.html"},{"id":"e350","label":"ESRS E1 - Climate","fullName":"ESRS E1 - Climate","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/ESRS E1 - Climate.html"},{"id":"e531","label":"European Commission","fullName":"European Commission","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/European Commission.html"},{"id":"e555","label":"European Sustainability…","fullName":"European Sustainability Reporting Standards","packageName":"European Sustainability Reporting Standards","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../European Sustainability Reporting Standards/European Sustainability Reporting Standards.html"},{"id":"e134","label":"Reduction Initiatives","fullName":"Reduction Initiatives","packageName":"Process","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Process/Reduction Initiatives.html"},{"id":"e221","label":"EMS-trends, IoT-logs","fullName":"EMS-trends, IoT-logs","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"EMS-trends, IoT-logs.html"},{"id":"e222","label":"CMMS / projectenregistr…","fullName":"CMMS / projectenregistratie","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CMMS _ projectenregistratie.html"},{"id":"e223","label":"Validatie via energieno…","fullName":"Validatie via energienota’s","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Validatie via energienota’s.html"}],"edges":[{"id":"c19","source":"e36","target":"e111","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c24","source":"e36","target":"e115","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c32","source":"e36","target":"e117","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c93","source":"e36","target":"e169","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c96","source":"e36","target":"e172","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c97","source":"e36","target":"e173","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c100","source":"e36","target":"e176","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c102","source":"e36","target":"e177","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c103","source":"e36","target":"e178","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c105","source":"e36","target":"e180","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c106","source":"e36","target":"e181","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c108","source":"e36","target":"e183","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c112","source":"e36","target":"e185","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c115","source":"e36","target":"e186","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c117","source":"e36","target":"e187","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c556","source":"e510","target":"e36","label":"reports according to","sourceLayer":"edgy-pe"},{"id":"c565","source":"e350","target":"e36","label":"Abstraction","sourceLayer":"business"},{"id":"c586","source":"e531","target":"e36","label":"Association","sourceLayer":"edgy-pe"},{"id":"c599","source":"e555","target":"e36","label":"Aggregation","sourceLayer":"edgy-id"},{"id":"c49","source":"e134","target":"e155","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c133","source":"e155","target":"e181","label":"Association","sourceLayer":"edgy-ex"},{"id":"c201","source":"e181","target":"e200","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c221","source":"e200","target":"e221","label":"Association","sourceLayer":"edgy-lb"},{"id":"c202","source":"e181","target":"e201","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c222","source":"e201","target":"e222","label":"Association","sourceLayer":"edgy-lb"},{"id":"c203","source":"e181","target":"e202","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c223","source":"e202","target":"e223","label":"Association","sourceLayer":"edgy-lb"},{"id":"c233","source":"e181","target":"e234","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-06-29 18:57:22*