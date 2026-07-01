---
ea_id: 177
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Metric</span> % energie uit hernieuwbare bronnen

**Type:** Requirement  **Stereotype:** Metric  **StereotypeEx:** Metric  **FQStereotype:** EDGY::Metric  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="177" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Metrics/% energie uit hernieuwbare bronnen.md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
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
| Association | Link | [Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen.](../Task/Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen..md) |
| Aggregation | Tree | [Energieverbruik (kWh)](Energieverbruik (kWh).md) |
| Aggregation | Tree | [Hernieuwbare energieopwekking (kWh)](Hernieuwbare energieopwekking (kWh).md) |
| Aggregation | Tree | [Brandstofverbruik voertuigen/machines](Brandstofverbruik voertuigen_machines.md) |
| Aggregation | Tree | [Aantal duurzame assets (bv. EV's, zonnepanelen)](Aantal duurzame assets (bv. EV's, zonnepanelen).md) |
| ControlFlow | Flow | [% Hernieuwbare Energie = (opgewekte hernieuwbare energie / totaal verbruik) × 100](% Hernieuwbare Energie = (opgewekte hernieuwbare energie _ totaal verbruik) × 100.md) |

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
| ControlFlow | Flow | [ESRS E1 Climate Change](../ESRS E1/ESRS E1 Climate Change.md) |
| Association | Link | [Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen.](../Task/Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen..md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e151","label":"Installeer zonnepanelen…","fullName":"Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen..html"},{"id":"e190","label":"Energieverbruik (kWh)","fullName":"Energieverbruik (kWh)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energieverbruik (kWh).html"},{"id":"e191","label":"Hernieuwbare energieopw…","fullName":"Hernieuwbare energieopwekking (kWh)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Hernieuwbare energieopwekking (kWh).html"},{"id":"e192","label":"Brandstofverbruik voert…","fullName":"Brandstofverbruik voertuigen/machines","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Brandstofverbruik voertuigen_machines.html"},{"id":"e193","label":"Aantal duurzame assets …","fullName":"Aantal duurzame assets (bv. EV's, zonnepanelen)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Aantal duurzame assets (bv. EV's, zonnepanelen).html"},{"id":"e230","label":"% Hernieuwbare Energie …","fullName":"% Hernieuwbare Energie = (opgewekte hernieuwbare energie / totaal verbruik) × 100","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% Hernieuwbare Energie = (opgewekte hernieuwbare energie _ totaal verbruik) × 100.html"},{"id":"e177","label":"% energie uit hernieuwb…","fullName":"% energie uit hernieuwbare bronnen","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e111","label":"Totale Scope 1-3 uitsto…","fullName":"Totale Scope 1-3 uitstoot (ton CO₂e/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Totale Scope 1-3 uitstoot (ton CO₂e_jaar).html"},{"id":"e115","label":"Reductiedoel CO₂ (% t.o…","fullName":"Reductiedoel CO₂ (% t.o.v. basisjaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Reductiedoel CO₂ (% t.o.v. basisjaar).html"},{"id":"e117","label":"Jaarlijkse CO₂-reductie…","fullName":"Jaarlijkse CO₂-reductie (%)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Jaarlijkse CO₂-reductie (%).html"},{"id":"e169","label":"CO₂-intensiteit per € o…","fullName":"CO₂-intensiteit per € omzet","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂-intensiteit per € omzet.html"},{"id":"e172","label":"CO₂ per klanttransactie…","fullName":"CO₂ per klanttransactie (kg)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂ per klanttransactie (kg).html"},{"id":"e173","label":"CO₂ bespaard door digit…","fullName":"CO₂ bespaard door digitalisering (ton)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂ bespaard door digitalisering (ton).html"},{"id":"e176","label":"Energieverbruik en wate…","fullName":"Energieverbruik en waterverbruik per eenheid","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energieverbruik en waterverbruik per eenheid.html"},{"id":"e178","label":"% reductie reisgerelate…","fullName":"% reductie reisgerelateerde CO₂","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% reductie reisgerelateerde CO₂.html"},{"id":"e180","label":"Scope 1 uitstoot per br…","fullName":"Scope 1 uitstoot per bron (ton CO₂e)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Scope 1 uitstoot per bron (ton CO₂e).html"},{"id":"e181","label":"Vermeden CO₂ door maatr…","fullName":"Vermeden CO₂ door maatregelen (ton/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Vermeden CO₂ door maatregelen (ton_jaar).html"},{"id":"e183","label":"% scope 3 emissies met …","fullName":"% scope 3 emissies met gemeten data","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% scope 3 emissies met gemeten data.html"},{"id":"e185","label":"# SDGs met meetbare KPI…","fullName":"# SDGs met meetbare KPI’s","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"_ SDGs met meetbare KPI’s.html"},{"id":"e186","label":"Energie (MWh/jaar)","fullName":"Energie (MWh/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energie (MWh_jaar).html"},{"id":"e187","label":"Recycling % van afval","fullName":"Recycling % van afval","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Recycling % van afval.html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Company subject to CSRD.html"},{"id":"e350","label":"ESRS E1 - Climate","fullName":"ESRS E1 - Climate","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/ESRS E1 - Climate.html"},{"id":"e531","label":"European Commission","fullName":"European Commission","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/European Commission.html"},{"id":"e555","label":"European Sustainability…","fullName":"European Sustainability Reporting Standards","packageName":"European Sustainability Reporting Standards","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../European Sustainability Reporting Standards/European Sustainability Reporting Standards.html"},{"id":"e131","label":"Sustainable Resources /…","fullName":"Sustainable Resources / Assets","packageName":"Asset","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Asset/Sustainable Resources _ Assets.html"},{"id":"e210","label":"Slimme meters / EMS","fullName":"Slimme meters / EMS","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Slimme meters _ EMS.html"},{"id":"e211","label":"BMS (HVAC/lighting)","fullName":"BMS (HVAC/lighting)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"BMS (HVAC_lighting).html"},{"id":"e212","label":"Telematica / fleetbeheer","fullName":"Telematica / fleetbeheer","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Telematica _ fleetbeheer.html"},{"id":"e213","label":"Assetmanagementsysteem","fullName":"Assetmanagementsysteem","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Assetmanagementsysteem.html"}],"edges":[{"id":"c19","source":"e36","target":"e111","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c24","source":"e36","target":"e115","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c32","source":"e36","target":"e117","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c93","source":"e36","target":"e169","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c96","source":"e36","target":"e172","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c97","source":"e36","target":"e173","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c100","source":"e36","target":"e176","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c102","source":"e36","target":"e177","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c103","source":"e36","target":"e178","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c105","source":"e36","target":"e180","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c106","source":"e36","target":"e181","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c108","source":"e36","target":"e183","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c112","source":"e36","target":"e185","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c115","source":"e36","target":"e186","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c117","source":"e36","target":"e187","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c556","source":"e510","target":"e36","label":"reports according to","sourceLayer":"edgy-pe"},{"id":"c565","source":"e350","target":"e36","label":"Abstraction","sourceLayer":"business"},{"id":"c586","source":"e531","target":"e36","label":"Association","sourceLayer":"edgy-pe"},{"id":"c599","source":"e555","target":"e36","label":"Aggregation","sourceLayer":"edgy-id"},{"id":"c45","source":"e131","target":"e151","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c129","source":"e151","target":"e177","label":"Association","sourceLayer":"edgy-ex"},{"id":"c191","source":"e177","target":"e190","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c211","source":"e190","target":"e210","label":"Association","sourceLayer":"edgy-lb"},{"id":"c192","source":"e177","target":"e191","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c212","source":"e191","target":"e211","label":"Association","sourceLayer":"edgy-lb"},{"id":"c193","source":"e177","target":"e192","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c213","source":"e192","target":"e212","label":"Association","sourceLayer":"edgy-lb"},{"id":"c194","source":"e177","target":"e193","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c214","source":"e193","target":"e213","label":"Association","sourceLayer":"edgy-lb"},{"id":"c230","source":"e177","target":"e230","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 11:29:53*