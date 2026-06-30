---
ea_id: 310
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="business">BusinessObject</span> ESRS S4 - Consumers

**Type:** Class  **Stereotype:** ArchiMate_BusinessObject  **StereotypeEx:** ArchiMate_BusinessObject  **FQStereotype:** ArchiMate3::ArchiMate_BusinessObject  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Consumers and end-users standard

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-std-009 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Association | [Consumers and End Users (ESRS S4)](Consumers and End Users (ESRS S4).md) |
| Abstraction | trace | [ESRS S4 Consumers and End-users](../ESRS S4/ESRS S4 Consumers and End-users.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Abstraction | trace | [ESRS S4 Consumers and End-users](../ESRS S4/ESRS S4 Consumers and End-users.md) |
| Association | ArchiMate_Association | [Consumers and End Users (ESRS S4)](Consumers and End Users (ESRS S4).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e280","label":"Consumers and End Users…","fullName":"Consumers and End Users (ESRS S4)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Consumers and End Users (ESRS S4).html"},{"id":"e44","label":"ESRS S4 Consumers and E…","fullName":"ESRS S4 Consumers and End-users","packageName":"ESRS S4","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS S4/ESRS S4 Consumers and End-users.html"},{"id":"e310","label":"ESRS S4 - Consumers","fullName":"ESRS S4 - Consumers","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":true,"hasUrl":false,"url":""},{"id":"e274","label":"Customers","fullName":"Customers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Customers.html"},{"id":"e517","label":"Consumers and End Users…","fullName":"Consumers and End Users (ESRS S4)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Consumers and End Users (ESRS S4).html"},{"id":"e531","label":"European Commission","fullName":"European Commission","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/European Commission.html"},{"id":"e555","label":"European Sustainability…","fullName":"European Sustainability Reporting Standards","packageName":"European Sustainability Reporting Standards","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../European Sustainability Reporting Standards/European Sustainability Reporting Standards.html"}],"edges":[{"id":"c249","source":"e280","target":"e310","label":"covered by","sourceLayer":"motivation"},{"id":"c292","source":"e274","target":"e280","label":"serves","sourceLayer":"motivation"},{"id":"c492","source":"e280","target":"e517","label":"Abstraction","sourceLayer":"motivation"},{"id":"c562","source":"e517","target":"e44","label":"covered by","sourceLayer":"edgy-pe"},{"id":"c572","source":"e44","target":"e310","label":"Abstraction","sourceLayer":"edgy-id"},{"id":"c594","source":"e531","target":"e44","label":"Association","sourceLayer":"edgy-pe"},{"id":"c607","source":"e555","target":"e44","label":"Aggregation","sourceLayer":"edgy-id"}]}</div>

---

*Generated: 2026-06-30 15:54:13*