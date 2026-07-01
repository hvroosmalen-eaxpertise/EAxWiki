---
ea_id: 457
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 046390c0
---

# <span class="sl" data-layer="motivation">Outcome</span> Reduced Environmental Impact

**Type:** Class  **Stereotype:** ArchiMate_Outcome  **StereotypeEx:** ArchiMate_Outcome  **FQStereotype:** ArchiMate3::ArchiMate_Outcome  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="457" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Outcomes/Reduced Environmental Impact.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Outcomes](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="457" data-file-path="Outcomes/Reduced Environmental Impact.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Measurable reduction in the organization''s environmental footprint across key indicators such as emissions, waste, water use, and resource consumption.         
Represents tangible environmental performance improvement resulting from sustainability initiatives.         
Contributes to corporate sustainability goals and regulatory compliance.</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | outcome-001 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | ArchiMate_Influence | [Introduce Sustainable Product Design Process](../Courses of Action/Introduce Sustainable Product Design Process.md) |
| ControlFlow | ArchiMate_Influence | [Carbon Footprint Reduction Program](../Courses of Action/Carbon Footprint Reduction Program.md) |
| Dependency | ArchiMate_Realization | [Corporate Sustainability Integration](../Goals/Corporate Sustainability Integration.md) |
| Association | ArchiMate_Association | [CO₂ Reduction %](../Assessments/CO₂ Reduction %.md) |
| Association | ArchiMate_Association | [Energy Efficiency Index](../Assessments/Energy Efficiency Index.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).html" class="diagram-thumb"><img src="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).png" alt="Strategic Sustainability Management Model (Bodenstein)" loading="lazy"><span>Strategic Sustainability Management Model (Bodenstein)</span></a>
  <a href="../Regulatory Pressure/diagrams/Regulatory Pressure.html" class="diagram-thumb"><img src="../Regulatory Pressure/diagrams/Regulatory Pressure.png" alt="Regulatory Pressure" loading="lazy"><span>Regulatory Pressure</span></a>
  <a href="../Reputation Risk/diagrams/Reputation Risk.html" class="diagram-thumb"><img src="../Reputation Risk/diagrams/Reputation Risk.png" alt="Reputation Risk" loading="lazy"><span>Reputation Risk</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | ArchiMate_Association | [CO₂ Reduction %](../Assessments/CO₂ Reduction %.md) |
| Association | ArchiMate_Association | [Energy Efficiency Index](../Assessments/Energy Efficiency Index.md) |
| ControlFlow | ArchiMate_Influence | [Introduce Sustainable Product Design Process](../Courses of Action/Introduce Sustainable Product Design Process.md) |
| ControlFlow | ArchiMate_Influence | [Carbon Footprint Reduction Program](../Courses of Action/Carbon Footprint Reduction Program.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e453","label":"Introduce Sustainable P…","fullName":"Introduce Sustainable Product Design Process","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Introduce Sustainable Product Design Process.html"},{"id":"e456","label":"Carbon Footprint Reduct…","fullName":"Carbon Footprint Reduction Program","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Carbon Footprint Reduction Program.html"},{"id":"e437","label":"Corporate Sustainabilit…","fullName":"Corporate Sustainability Integration","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Corporate Sustainability Integration.html"},{"id":"e462","label":"CO₂ Reduction %","fullName":"CO₂ Reduction %","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/CO₂ Reduction %.html"},{"id":"e465","label":"Energy Efficiency Index","fullName":"Energy Efficiency Index","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/Energy Efficiency Index.html"},{"id":"e457","label":"Reduced Environmental I…","fullName":"Reduced Environmental Impact","packageName":"Outcomes","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e446","label":"Sustainable Innovation …","fullName":"Sustainable Innovation Management","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Capabilities/Sustainable Innovation Management.html"},{"id":"e445","label":"ESG Performance Managem…","fullName":"ESG Performance Management","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Capabilities/ESG Performance Management.html"},{"id":"e429","label":"Regulatory Pressure","fullName":"Regulatory Pressure","packageName":"Drivers","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Drivers/Regulatory Pressure.html"},{"id":"e433","label":"Reputation Risk","fullName":"Reputation Risk","packageName":"Drivers","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Drivers/Reputation Risk.html"},{"id":"e439","label":"Integrate Sustainabilit…","fullName":"Integrate Sustainability into Corporate Strategy","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Integrate Sustainability into Corporate Strategy.html"}],"edges":[{"id":"c436","source":"e446","target":"e453","label":"realizes","sourceLayer":"strategy"},{"id":"c442","source":"e453","target":"e457","label":"influences","sourceLayer":"strategy"},{"id":"c439","source":"e445","target":"e456","label":"realizes","sourceLayer":"strategy"},{"id":"c445","source":"e456","target":"e457","label":"influences","sourceLayer":"strategy"},{"id":"c411","source":"e429","target":"e437","label":"drives","sourceLayer":"motivation"},{"id":"c418","source":"e433","target":"e437","label":"drives","sourceLayer":"motivation"},{"id":"c419","source":"e439","target":"e437","label":"realizes","sourceLayer":"strategy"},{"id":"c446","source":"e457","target":"e437","label":"realizes","sourceLayer":"motivation"},{"id":"c452","source":"e462","target":"e457","label":"measures","sourceLayer":"motivation"},{"id":"c453","source":"e462","target":"e445","label":"measures","sourceLayer":"motivation"},{"id":"c458","source":"e465","target":"e457","label":"measures","sourceLayer":"motivation"},{"id":"c459","source":"e465","target":"e445","label":"measures","sourceLayer":"motivation"},{"id":"c427","source":"e445","target":"e439","label":"realizes","sourceLayer":"strategy"}]}</div>

---

*Generated: 2026-07-01 12:05:09*