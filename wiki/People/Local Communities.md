---
ea_id: 524
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: 07b8db4a
---

# <span class="sl" data-layer="edgy-pe">People</span> Local Communities

**Type:** Actor  **Stereotype:** People  **StereotypeEx:** People  **FQStereotype:** EDGY::People  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="524" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="People/Local Communities.md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-12-12  **Modified:** 2025-12-15


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Base](../Base/index.md) / [People](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="524" data-file-path="People/Local Communities.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Communities around production facilities and supplier regions</p>
<!--ea-notes-end-->
</div>
</div>

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Abstraction | trace | [Local Communities](../ESRS Navigator Stakeholder Map/Local Communities.md) |
| Aggregation | Tree | [Affected Communities (ESRS S3)](Affected Communities (ESRS S3).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../ESRS and People/diagrams/ESRS Stakeholder Map.html" class="diagram-thumb"><img src="../ESRS and People/diagrams/ESRS Stakeholder Map.png" alt="ESRS Stakeholder Map" loading="lazy"><span>ESRS Stakeholder Map</span></a>
  <a href="../ESRS S3 Affected Communities - People/diagrams/ESRS S3 Affected Communities - People.html" class="diagram-thumb"><img src="../ESRS S3 Affected Communities - People/diagrams/ESRS S3 Affected Communities - People.png" alt="ESRS S3 Affected Communities - People" loading="lazy"><span>ESRS S3 Affected Communities - People</span></a>
  <a href="../ESRS Navigator Stakeholder Map/diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="../ESRS Navigator Stakeholder Map/diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [Affected Communities (ESRS S3)](Affected Communities (ESRS S3).md) |
| Abstraction | trace | [Local Communities](../ESRS Navigator Stakeholder Map/Local Communities.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e249","label":"Local Communities","fullName":"Local Communities","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Local Communities.html"},{"id":"e523","label":"Affected Communities (E…","fullName":"Affected Communities (ESRS S3)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Affected Communities (ESRS S3).html"},{"id":"e524","label":"Local Communities","fullName":"Local Communities","packageName":"People","layer":"edgy-pe","isFocal":true,"hasUrl":false,"url":""},{"id":"e251","label":"Affected Communities (E…","fullName":"Affected Communities (ESRS S3)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Affected Communities (ESRS S3).html"},{"id":"e525","label":"Indigenous Peoples","fullName":"Indigenous Peoples","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Indigenous Peoples.html"},{"id":"e43","label":"ESRS S3 Affected Commun…","fullName":"ESRS S3 Affected Communities","packageName":"ESRS S3","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS S3/ESRS S3 Affected Communities.html"}],"edges":[{"id":"c284","source":"e249","target":"e251","label":"Generalization","sourceLayer":"motivation"},{"id":"c499","source":"e249","target":"e524","label":"Abstraction","sourceLayer":"motivation"},{"id":"c498","source":"e251","target":"e523","label":"Abstraction","sourceLayer":"motivation"},{"id":"c521","source":"e523","target":"e525","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c522","source":"e523","target":"e524","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c563","source":"e523","target":"e43","label":"covered by","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-07-01 12:21:52*