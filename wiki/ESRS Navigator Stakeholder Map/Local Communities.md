---
ea_id: 249
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 07b8db4a
---

# <span class="sl" data-layer="motivation">Stakeholder</span> Local Communities

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="249" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="ESRS Navigator Stakeholder Map/Local Communities.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="249" data-file-path="ESRS Navigator Stakeholder Map/Local Communities.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Communities around production facilities and supplier regions</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-comm-002 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Generalization | ArchiMate_Specialization | [Affected Communities (ESRS S3)](Affected Communities (ESRS S3).md) |
| Abstraction | trace | [Local Communities](../People/Local Communities.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e251","label":"Affected Communities (E…","fullName":"Affected Communities (ESRS S3)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Affected Communities (ESRS S3).html"},{"id":"e524","label":"Local Communities","fullName":"Local Communities","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Local Communities.html"},{"id":"e249","label":"Local Communities","fullName":"Local Communities","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e344","label":"ESRS S3 - Communities","fullName":"ESRS S3 - Communities","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"ESRS S3 - Communities.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e314","label":"Indigenous Peoples","fullName":"Indigenous Peoples","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Indigenous Peoples.html"},{"id":"e523","label":"Affected Communities (E…","fullName":"Affected Communities (ESRS S3)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Affected Communities (ESRS S3).html"}],"edges":[{"id":"c246","source":"e251","target":"e344","label":"covered by","sourceLayer":"motivation"},{"id":"c270","source":"e331","target":"e251","label":"impacts","sourceLayer":"motivation"},{"id":"c284","source":"e249","target":"e251","label":"Generalization","sourceLayer":"motivation"},{"id":"c330","source":"e314","target":"e251","label":"Generalization","sourceLayer":"motivation"},{"id":"c498","source":"e251","target":"e523","label":"Abstraction","sourceLayer":"motivation"},{"id":"c499","source":"e249","target":"e524","label":"Abstraction","sourceLayer":"motivation"},{"id":"c522","source":"e523","target":"e524","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-07-01 12:21:53*