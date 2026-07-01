---
ea_id: 464
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: b2b08d92
---

# <span class="sl" data-layer="motivation">Assessment</span> Supplier Compliance Rate

**Type:** Class  **Stereotype:** ArchiMate_Assessment  **StereotypeEx:** ArchiMate_Assessment  **FQStereotype:** ArchiMate3::ArchiMate_Assessment  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="464" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Assessments/Supplier Compliance Rate.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Assessments](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="464" data-file-path="Assessments/Supplier Compliance Rate.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Percentage of suppliers meeting defined environmental and social standards based on audits and assessments.         
Measures effectiveness of sustainable supply chain management programs.         
Indicates level of control over value chain sustainability impacts.</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | kpi-003 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Association | [Lower Operational Risk](../Outcomes/Lower Operational Risk.md) |
| Association | ArchiMate_Association | [Sustainable Supply Chain Management](../Capabilities/Sustainable Supply Chain Management.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).html" class="diagram-thumb"><img src="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).png" alt="Strategic Sustainability Management Model (Bodenstein)" loading="lazy"><span>Strategic Sustainability Management Model (Bodenstein)</span></a>
  <a href="../Climate Risk/diagrams/Climate Risk.html" class="diagram-thumb"><img src="../Climate Risk/diagrams/Climate Risk.png" alt="Climate Risk" loading="lazy"><span>Climate Risk</span></a>
  <a href="../Regulatory Pressure/diagrams/Regulatory Pressure.html" class="diagram-thumb"><img src="../Regulatory Pressure/diagrams/Regulatory Pressure.png" alt="Regulatory Pressure" loading="lazy"><span>Regulatory Pressure</span></a>
  <a href="../Reputation Risk/diagrams/Reputation Risk.html" class="diagram-thumb"><img src="../Reputation Risk/diagrams/Reputation Risk.png" alt="Reputation Risk" loading="lazy"><span>Reputation Risk</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e459","label":"Lower Operational Risk","fullName":"Lower Operational Risk","packageName":"Outcomes","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Outcomes/Lower Operational Risk.html"},{"id":"e447","label":"Sustainable Supply Chai…","fullName":"Sustainable Supply Chain Management","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Capabilities/Sustainable Supply Chain Management.html"},{"id":"e464","label":"Supplier Compliance Rate","fullName":"Supplier Compliance Rate","packageName":"Assessments","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e454","label":"Supplier Sustainability…","fullName":"Supplier Sustainability Audits","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Supplier Sustainability Audits.html"},{"id":"e436","label":"Risk Reduction","fullName":"Risk Reduction","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Risk Reduction.html"},{"id":"e442","label":"Responsible Supply Chai…","fullName":"Responsible Supply Chain Strategy","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Responsible Supply Chain Strategy.html"}],"edges":[{"id":"c443","source":"e454","target":"e459","label":"influences","sourceLayer":"strategy"},{"id":"c449","source":"e459","target":"e436","label":"realizes","sourceLayer":"motivation"},{"id":"c456","source":"e464","target":"e459","label":"measures","sourceLayer":"motivation"},{"id":"c430","source":"e447","target":"e442","label":"realizes","sourceLayer":"strategy"},{"id":"c437","source":"e447","target":"e454","label":"realizes","sourceLayer":"strategy"},{"id":"c457","source":"e464","target":"e447","label":"measures","sourceLayer":"motivation"},{"id":"c423","source":"e442","target":"e436","label":"realizes","sourceLayer":"strategy"}]}</div>

---

*Generated: 2026-07-01 12:21:53*