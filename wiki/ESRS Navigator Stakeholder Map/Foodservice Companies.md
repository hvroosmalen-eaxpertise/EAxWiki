---
ea_id: 253
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 65b1bf14
---

# <span class="sl" data-layer="motivation">Stakeholder</span> Foodservice Companies

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="253" data-file-path="ESRS Navigator Stakeholder Map/Foodservice Companies.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>B2B foodservice customers</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-down-003 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Generalization | ArchiMate_Specialization | [Customers](Customers.md) |
| Abstraction | trace | [Foodservice Companies](../People/Foodservice Companies.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e274","label":"Customers","fullName":"Customers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Customers.html"},{"id":"e519","label":"Foodservice Companies","fullName":"Foodservice Companies","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Foodservice Companies.html"},{"id":"e253","label":"Foodservice Companies","fullName":"Foodservice Companies","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e246","label":"Supermarket Chains","fullName":"Supermarket Chains","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Supermarket Chains.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e280","label":"Consumers and End Users…","fullName":"Consumers and End Users (ESRS S4)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Consumers and End Users (ESRS S4).html"},{"id":"e516","label":"Customers","fullName":"Customers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Customers.html"}],"edges":[{"id":"c255","source":"e246","target":"e274","label":"Generalization","sourceLayer":"motivation"},{"id":"c256","source":"e253","target":"e274","label":"Generalization","sourceLayer":"motivation"},{"id":"c268","source":"e331","target":"e274","label":"serves","sourceLayer":"motivation"},{"id":"c292","source":"e274","target":"e280","label":"serves","sourceLayer":"motivation"},{"id":"c490","source":"e274","target":"e516","label":"Abstraction","sourceLayer":"motivation"},{"id":"c494","source":"e253","target":"e519","label":"Abstraction","sourceLayer":"motivation"},{"id":"c526","source":"e516","target":"e519","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-07-01 09:47:23*