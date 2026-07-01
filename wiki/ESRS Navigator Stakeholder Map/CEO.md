---
ea_id: 265
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: d8928d52
---

# <span class="sl" data-layer="business">BusinessRole</span> CEO

**Type:** Class  **Stereotype:** ArchiMate_BusinessRole  **StereotypeEx:** ArchiMate_BusinessRole  **FQStereotype:** ArchiMate3::ArchiMate_BusinessRole  **Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="265" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="ESRS Navigator Stakeholder Map/CEO.md" data-api-port="8001"></div>

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="265" data-file-path="ESRS Navigator Stakeholder Map/CEO.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Chief Executive Officer</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-gov-002 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Association | [Sustainability Manager/Director](Sustainability Manager_Director.md) |
| Association | ArchiMate_Composition | [Board of Directors (Directie)](Board of Directors (Directie).md) |
| Abstraction | trace | [CEO](../People/CEO.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | ArchiMate_Composition | [Board of Directors (Directie)](Board of Directors (Directie).md) |
| Association | ArchiMate_Association | [Sustainability Manager/Director](Sustainability Manager_Director.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e269","label":"Sustainability Manager/…","fullName":"Sustainability Manager/Director","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"Sustainability Manager_Director.html"},{"id":"e266","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"Board of Directors (Directie).html"},{"id":"e495","label":"CEO","fullName":"CEO","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/CEO.html"},{"id":"e265","label":"CEO","fullName":"CEO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":true,"hasUrl":false,"url":""},{"id":"e494","label":"Sustainability Manager/…","fullName":"Sustainability Manager/Director","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Sustainability Manager_Director.html"},{"id":"e268","label":"COO","fullName":"COO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"COO.html"},{"id":"e293","label":"CFO","fullName":"CFO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"CFO.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e496","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Board of Directors (Directie).html"}],"edges":[{"id":"c290","source":"e269","target":"e265","label":"reports to","sourceLayer":"business"},{"id":"c468","source":"e269","target":"e494","label":"Abstraction","sourceLayer":"business"},{"id":"c241","source":"e266","target":"e268","label":"Association","sourceLayer":"business"},{"id":"c304","source":"e266","target":"e265","label":"Association","sourceLayer":"business"},{"id":"c318","source":"e266","target":"e293","label":"Association","sourceLayer":"business"},{"id":"c322","source":"e266","target":"e331","label":"Association","sourceLayer":"business"},{"id":"c470","source":"e266","target":"e496","label":"Abstraction","sourceLayer":"business"},{"id":"c469","source":"e265","target":"e495","label":"Abstraction","sourceLayer":"business"},{"id":"c542","source":"e496","target":"e495","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c543","source":"e494","target":"e495","label":"reports to","sourceLayer":"edgy-pe"},{"id":"c541","source":"e496","target":"e494","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-07-01 10:25:43*