---
ea_id: 447
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 5d29223f
---

# <span class="sl" data-layer="strategy">Capability</span> Sustainable Supply Chain Management

**Type:** Class  **Stereotype:** ArchiMate_Capability  **StereotypeEx:** ArchiMate_Capability  **FQStereotype:** ArchiMate3::ArchiMate_Capability  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Capabilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="447" data-file-path="Capabilities/Sustainable Supply Chain Management.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Capability to manage environmental and social sustainability throughout the supply chain.         
Includes supplier assessment, auditing, development programs, and collaborative sustainability initiatives.         
Ensures responsible sourcing and addresses upstream environmental and social impacts.</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | capability-004 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Dependency | ArchiMate_Realization | [Responsible Supply Chain Strategy](../Courses of Action/Responsible Supply Chain Strategy.md) |
| Dependency | ArchiMate_Realization | [Supplier Sustainability Audits](../Courses of Action/Supplier Sustainability Audits.md) |
| Association | ArchiMate_Association | [Supplier Compliance Rate](../Assessments/Supplier Compliance Rate.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).html" class="diagram-thumb"><img src="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).png" alt="Strategic Sustainability Management Model (Bodenstein)" loading="lazy"><span>Strategic Sustainability Management Model (Bodenstein)</span></a>
  <a href="../Climate Risk/diagrams/Climate Risk.html" class="diagram-thumb"><img src="../Climate Risk/diagrams/Climate Risk.png" alt="Climate Risk" loading="lazy"><span>Climate Risk</span></a>
  <a href="../Regulatory Pressure/diagrams/Regulatory Pressure.html" class="diagram-thumb"><img src="../Regulatory Pressure/diagrams/Regulatory Pressure.png" alt="Regulatory Pressure" loading="lazy"><span>Regulatory Pressure</span></a>
  <a href="../Reputation Risk/diagrams/Reputation Risk.html" class="diagram-thumb"><img src="../Reputation Risk/diagrams/Reputation Risk.png" alt="Reputation Risk" loading="lazy"><span>Reputation Risk</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | ArchiMate_Association | [Supplier Compliance Rate](../Assessments/Supplier Compliance Rate.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e442","label":"Responsible Supply Chai…","fullName":"Responsible Supply Chain Strategy","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Responsible Supply Chain Strategy.html"},{"id":"e454","label":"Supplier Sustainability…","fullName":"Supplier Sustainability Audits","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Supplier Sustainability Audits.html"},{"id":"e464","label":"Supplier Compliance Rate","fullName":"Supplier Compliance Rate","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/Supplier Compliance Rate.html"},{"id":"e447","label":"Sustainable Supply Chai…","fullName":"Sustainable Supply Chain Management","packageName":"Capabilities","layer":"strategy","isFocal":true,"hasUrl":false,"url":""},{"id":"e436","label":"Risk Reduction","fullName":"Risk Reduction","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Risk Reduction.html"},{"id":"e459","label":"Lower Operational Risk","fullName":"Lower Operational Risk","packageName":"Outcomes","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Outcomes/Lower Operational Risk.html"}],"edges":[{"id":"c423","source":"e442","target":"e436","label":"realizes","sourceLayer":"strategy"},{"id":"c430","source":"e447","target":"e442","label":"realizes","sourceLayer":"strategy"},{"id":"c437","source":"e447","target":"e454","label":"realizes","sourceLayer":"strategy"},{"id":"c443","source":"e454","target":"e459","label":"influences","sourceLayer":"strategy"},{"id":"c456","source":"e464","target":"e459","label":"measures","sourceLayer":"motivation"},{"id":"c457","source":"e464","target":"e447","label":"measures","sourceLayer":"motivation"},{"id":"c449","source":"e459","target":"e436","label":"realizes","sourceLayer":"motivation"}]}</div>

---

*Generated: 2026-07-01 09:47:23*