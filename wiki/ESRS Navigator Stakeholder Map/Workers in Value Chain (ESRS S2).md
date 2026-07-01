---
ea_id: 267
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: e24e9b7e
---

# <span class="sl" data-layer="motivation">Stakeholder</span> Workers in Value Chain (ESRS S2)

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="267" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="ESRS Navigator Stakeholder Map/Workers in Value Chain (ESRS S2).md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="267" data-file-path="ESRS Navigator Stakeholder Map/Workers in Value Chain (ESRS S2).md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Employees of suppliers and partners</p>
<!--ea-notes-end-->
</div>
</div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-up-005 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Generalization | ArchiMate_Specialization | [Seasonal Agricultural Workers](Seasonal Agricultural Workers.md) |
| Association | ArchiMate_Assignment | [Suppliers](Suppliers.md) |
| Association | ArchiMate_Association | [ESRS S2 - Value Chain Workers](ESRS S2 - Value Chain Workers.md) |
| Abstraction | trace | [Workers in Value Chain (ESRS S2)](../People/Workers in Value Chain (ESRS S2).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Generalization | ArchiMate_Specialization | [Seasonal Agricultural Workers](Seasonal Agricultural Workers.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e284","label":"Seasonal Agricultural W…","fullName":"Seasonal Agricultural Workers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Seasonal Agricultural Workers.html"},{"id":"e335","label":"Suppliers","fullName":"Suppliers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Suppliers.html"},{"id":"e282","label":"ESRS S2 - Value Chain W…","fullName":"ESRS S2 - Value Chain Workers","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"ESRS S2 - Value Chain Workers.html"},{"id":"e532","label":"Workers in Value Chain …","fullName":"Workers in Value Chain (ESRS S2)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Workers in Value Chain (ESRS S2).html"},{"id":"e267","label":"Workers in Value Chain …","fullName":"Workers in Value Chain (ESRS S2)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e533","label":"Seasonal Agricultural W…","fullName":"Seasonal Agricultural Workers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Seasonal Agricultural Workers.html"},{"id":"e294","label":"Transport Companies","fullName":"Transport Companies","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Transport Companies.html"},{"id":"e334","label":"Agricultural Suppliers","fullName":"Agricultural Suppliers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Agricultural Suppliers.html"},{"id":"e300","label":"Packaging Suppliers","fullName":"Packaging Suppliers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Packaging Suppliers.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e529","label":"Suppliers","fullName":"Suppliers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Suppliers.html"},{"id":"e42","label":"ESRS S2 Workers in the …","fullName":"ESRS S2 Workers in the Value Chain","packageName":"ESRS S2","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS S2/ESRS S2 Workers in the Value Chain.html"}],"edges":[{"id":"c273","source":"e284","target":"e267","label":"Generalization","sourceLayer":"motivation"},{"id":"c508","source":"e284","target":"e533","label":"Abstraction","sourceLayer":"motivation"},{"id":"c279","source":"e294","target":"e335","label":"Generalization","sourceLayer":"motivation"},{"id":"c282","source":"e267","target":"e335","label":"Association","sourceLayer":"motivation"},{"id":"c287","source":"e334","target":"e335","label":"Generalization","sourceLayer":"motivation"},{"id":"c327","source":"e300","target":"e335","label":"Generalization","sourceLayer":"motivation"},{"id":"c328","source":"e335","target":"e331","label":"supplies to","sourceLayer":"motivation"},{"id":"c504","source":"e335","target":"e529","label":"Abstraction","sourceLayer":"motivation"},{"id":"c297","source":"e267","target":"e282","label":"covered by","sourceLayer":"motivation"},{"id":"c574","source":"e282","target":"e42","label":"Abstraction","sourceLayer":"business"},{"id":"c507","source":"e267","target":"e532","label":"Abstraction","sourceLayer":"motivation"},{"id":"c564","source":"e532","target":"e42","label":"covered by","sourceLayer":"edgy-pe"},{"id":"c577","source":"e532","target":"e533","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c662","source":"e529","target":"e532","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-07-01 12:05:09*