# <span class="sl" data-layer="edgy-lb">Metric</span> Hernieuwbare energieopwekking (kWh)

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
| Aggregation | Tree | [% energie uit hernieuwbare bronnen](% energie uit hernieuwbare bronnen.md) |
| Association | Link | [BMS (HVAC/lighting)](BMS (HVAC_lighting).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb diagram-thumb--noimg"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [% energie uit hernieuwbare bronnen](% energie uit hernieuwbare bronnen.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e177","label":"% energie uit hernieuwb…","fullName":"% energie uit hernieuwbare bronnen","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% energie uit hernieuwbare bronnen.html"},{"id":"e211","label":"BMS (HVAC/lighting)","fullName":"BMS (HVAC/lighting)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"BMS (HVAC_lighting).html"},{"id":"e191","label":"Hernieuwbare energieopw…","fullName":"Hernieuwbare energieopwekking (kWh)","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e151","label":"Installeer zonnepanelen…","fullName":"Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen..html"},{"id":"e190","label":"Energieverbruik (kWh)","fullName":"Energieverbruik (kWh)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Energieverbruik (kWh).html"},{"id":"e192","label":"Brandstofverbruik voert…","fullName":"Brandstofverbruik voertuigen/machines","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Brandstofverbruik voertuigen_machines.html"},{"id":"e193","label":"Aantal duurzame assets …","fullName":"Aantal duurzame assets (bv. EV's, zonnepanelen)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Aantal duurzame assets (bv. EV's, zonnepanelen).html"},{"id":"e230","label":"% Hernieuwbare Energie …","fullName":"% Hernieuwbare Energie = (opgewekte hernieuwbare energie / totaal verbruik) × 100","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% Hernieuwbare Energie = (opgewekte hernieuwbare energie _ totaal verbruik) × 100.html"}],"edges":[{"id":"c102","source":"e36","target":"e177","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c129","source":"e151","target":"e177","label":"Association","sourceLayer":"edgy-ex"},{"id":"c191","source":"e177","target":"e190","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c192","source":"e177","target":"e191","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c193","source":"e177","target":"e192","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c194","source":"e177","target":"e193","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c230","source":"e177","target":"e230","label":"ControlFlow","sourceLayer":"edgy-lb"},{"id":"c212","source":"e191","target":"e211","label":"Association","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-06-29 18:57:22*