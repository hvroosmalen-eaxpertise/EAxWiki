---
ea_id: 496
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: 8694b769
---

# <span class="sl" data-layer="edgy-pe">People</span> Board of Directors (Directie)

**Type:** Actor  **Stereotype:** People  **StereotypeEx:** People  **FQStereotype:** EDGY::People  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-12  **Modified:** 2025-12-15


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Base](../Base/index.md) / [People](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="496" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="People/Board of Directors (Directie).md" data-api-port="8001"></div>

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="496" data-file-path="People/Board of Directors (Directie).md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>High-level view of all stakeholders in ESRS reporting ecosystem</p>
<!--ea-notes-end-->
</div>
</div>

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Abstraction | trace | [Board of Directors (Directie)](../ESRS Navigator Stakeholder Map/Board of Directors (Directie).md) |
| Aggregation | Tree | [COO](COO.md) |
| Aggregation | Tree | [Chief Financial Officer](Chief Financial Officer.md) |
| Aggregation | Tree | [Sustainability Manager/Director](Sustainability Manager_Director.md) |
| Aggregation | Tree | [CEO](CEO.md) |
| Aggregation | Tree | [Supervisory Board (RvC)](Supervisory Board (RvC).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../ESRS and People/diagrams/ESRS Stakeholder Map.html" class="diagram-thumb"><img src="../ESRS and People/diagrams/ESRS Stakeholder Map.png" alt="ESRS Stakeholder Map" loading="lazy"><span>ESRS Stakeholder Map</span></a>
  <a href="../ESRS Navigator Stakeholder Map/diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="../ESRS Navigator Stakeholder Map/diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Abstraction | trace | [Board of Directors (Directie)](../ESRS Navigator Stakeholder Map/Board of Directors (Directie).md) |
| Aggregation | Tree | [Supervisory Board (RvC)](Supervisory Board (RvC).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e266","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Board of Directors (Directie).html"},{"id":"e498","label":"COO","fullName":"COO","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"COO.html"},{"id":"e497","label":"Chief Financial Officer","fullName":"Chief Financial Officer","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Chief Financial Officer.html"},{"id":"e494","label":"Sustainability Manager/…","fullName":"Sustainability Manager/Director","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Sustainability Manager_Director.html"},{"id":"e495","label":"CEO","fullName":"CEO","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"CEO.html"},{"id":"e504","label":"Supervisory Board (RvC)","fullName":"Supervisory Board (RvC)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Supervisory Board (RvC).html"},{"id":"e496","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"People","layer":"edgy-pe","isFocal":true,"hasUrl":false,"url":""},{"id":"e268","label":"COO","fullName":"COO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/COO.html"},{"id":"e265","label":"CEO","fullName":"CEO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/CEO.html"},{"id":"e293","label":"CFO","fullName":"CFO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/CFO.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Large Companies (_250 employees).html"},{"id":"e269","label":"Sustainability Manager/…","fullName":"Sustainability Manager/Director","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Sustainability Manager_Director.html"},{"id":"e337","label":"Supervisory Board (RvC)","fullName":"Supervisory Board (RvC)","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Supervisory Board (RvC).html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Company subject to CSRD.html"}],"edges":[{"id":"c241","source":"e266","target":"e268","label":"Association","sourceLayer":"business"},{"id":"c304","source":"e266","target":"e265","label":"Association","sourceLayer":"business"},{"id":"c318","source":"e266","target":"e293","label":"Association","sourceLayer":"business"},{"id":"c322","source":"e266","target":"e331","label":"Association","sourceLayer":"business"},{"id":"c470","source":"e266","target":"e496","label":"Abstraction","sourceLayer":"business"},{"id":"c472","source":"e268","target":"e498","label":"Abstraction","sourceLayer":"business"},{"id":"c539","source":"e496","target":"e498","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c471","source":"e293","target":"e497","label":"Abstraction","sourceLayer":"business"},{"id":"c540","source":"e496","target":"e497","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c468","source":"e269","target":"e494","label":"Abstraction","sourceLayer":"business"},{"id":"c541","source":"e496","target":"e494","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c543","source":"e494","target":"e495","label":"reports to","sourceLayer":"edgy-pe"},{"id":"c469","source":"e265","target":"e495","label":"Abstraction","sourceLayer":"business"},{"id":"c542","source":"e496","target":"e495","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c478","source":"e337","target":"e504","label":"Abstraction","sourceLayer":"business"},{"id":"c538","source":"e504","target":"e510","label":"supervises","sourceLayer":"edgy-pe"},{"id":"c659","source":"e504","target":"e496","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c290","source":"e269","target":"e265","label":"reports to","sourceLayer":"business"},{"id":"c288","source":"e337","target":"e331","label":"Association","sourceLayer":"business"},{"id":"c484","source":"e331","target":"e510","label":"Abstraction","sourceLayer":"motivation"}]}</div>

---

*Generated: 2026-07-01 09:47:23*