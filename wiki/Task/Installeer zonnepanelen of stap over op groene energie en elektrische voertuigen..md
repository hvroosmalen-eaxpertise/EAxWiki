---
ea_id: 151
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-ex">Task</span> Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen.

**Type:** Activity  **Stereotype:** Task  **StereotypeEx:** Task  **FQStereotype:** EDGY::Task  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="151" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Task/Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen..md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
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
| ControlFlow | Flow | [Sustainable Resources / Assets](../Asset/Sustainable Resources _ Assets.md) |
| Association | Link | [% energie uit hernieuwbare bronnen](../Metrics/% energie uit hernieuwbare bronnen.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Architecture/diagrams/Architecture.html" class="diagram-thumb"><img src="../Architecture/diagrams/Architecture.png" alt="Architecture" loading="lazy"><span>Architecture</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Sustainable Resources / Assets](../Asset/Sustainable Resources _ Assets.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e131","label":"Sustainable Resources /…","fullName":"Sustainable Resources / Assets","packageName":"Asset","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Asset/Sustainable Resources _ Assets.html"},{"id":"e177","label":"% energie uit hernieuwb…","fullName":"% energie uit hernieuwbare bronnen","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/% energie uit hernieuwbare bronnen.html"},{"id":"e151","label":"Installeer zonnepanelen…","fullName":"Installeer zonnepanelen of stap over op groene energie en elektrische voertuigen.","packageName":"Task","layer":"edgy-ex","isFocal":true,"hasUrl":false,"url":""},{"id":"e9","label":"SDG  7. Affordable and …","fullName":"SDG  7. Affordable and Clean Energy","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG  7. Affordable and Clean Energy.html"},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e190","label":"Energieverbruik (kWh)","fullName":"Energieverbruik (kWh)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Energieverbruik (kWh).html"},{"id":"e191","label":"Hernieuwbare energieopw…","fullName":"Hernieuwbare energieopwekking (kWh)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Hernieuwbare energieopwekking (kWh).html"},{"id":"e192","label":"Brandstofverbruik voert…","fullName":"Brandstofverbruik voertuigen/machines","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Brandstofverbruik voertuigen_machines.html"},{"id":"e193","label":"Aantal duurzame assets …","fullName":"Aantal duurzame assets (bv. EV's, zonnepanelen)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Aantal duurzame assets (bv. EV's, zonnepanelen).html"},{"id":"e230","label":"% Hernieuwbare Energie …","fullName":"% Hernieuwbare Energie = (opgewekte hernieuwbare energie / totaal verbruik) × 100","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/% Hernieuwbare Energie = (opgewekte hernieuwbare energie _ totaal verbruik) × 100.html"}],"edges":[{"id":"c45","source":"e131","target":"e151","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c71","source":"e9","target":"e131","label":"Association","sourceLayer":"edgy-id"},{"id":"c72","source":"e15","target":"e131","label":"Association","sourceLayer":"edgy-id"},{"id":"c102","source":"e36","target":"e177","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c129","source":"e151","target":"e177","label":"Association","sourceLayer":"edgy-ex"},{"id":"c191","source":"e177","target":"e190","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c192","source":"e177","target":"e191","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c193","source":"e177","target":"e192","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c194","source":"e177","target":"e193","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c230","source":"e177","target":"e230","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 11:29:53*