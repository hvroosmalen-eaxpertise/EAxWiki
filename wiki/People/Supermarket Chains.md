---
ea_id: 515
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: c4e10ec2
---

# <span class="sl" data-layer="edgy-pe">People</span> Supermarket Chains

**Type:** Actor  **Stereotype:** People  **StereotypeEx:** People  **FQStereotype:** EDGY::People  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="515" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="People/Supermarket Chains.md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-12-12  **Modified:** 2025-12-15


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Base](../Base/index.md) / [People](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="515" data-file-path="People/Supermarket Chains.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Retail customers</p>
<!--ea-notes-end-->
</div>
</div>

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Abstraction | trace | [Supermarket Chains](../ESRS Navigator Stakeholder Map/Supermarket Chains.md) |
| Aggregation | Tree | [Customers](Customers.md) |

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
| Aggregation | Tree | [Customers](Customers.md) |
| Abstraction | trace | [Supermarket Chains](../ESRS Navigator Stakeholder Map/Supermarket Chains.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e246","label":"Supermarket Chains","fullName":"Supermarket Chains","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Supermarket Chains.html"},{"id":"e516","label":"Customers","fullName":"Customers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Customers.html"},{"id":"e515","label":"Supermarket Chains","fullName":"Supermarket Chains","packageName":"People","layer":"edgy-pe","isFocal":true,"hasUrl":false,"url":""},{"id":"e274","label":"Customers","fullName":"Customers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/Customers.html"},{"id":"e517","label":"Consumers and End Users…","fullName":"Consumers and End Users (ESRS S4)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Consumers and End Users (ESRS S4).html"},{"id":"e519","label":"Foodservice Companies","fullName":"Foodservice Companies","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Foodservice Companies.html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"Company subject to CSRD.html"}],"edges":[{"id":"c255","source":"e246","target":"e274","label":"Generalization","sourceLayer":"motivation"},{"id":"c489","source":"e246","target":"e515","label":"Abstraction","sourceLayer":"motivation"},{"id":"c490","source":"e274","target":"e516","label":"Abstraction","sourceLayer":"motivation"},{"id":"c525","source":"e516","target":"e517","label":"Association","sourceLayer":"edgy-pe"},{"id":"c526","source":"e516","target":"e519","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c527","source":"e516","target":"e515","label":"Aggregation","sourceLayer":"edgy-pe"},{"id":"c660","source":"e510","target":"e516","label":"Association","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-07-01 11:29:53*