---
ea_id: 280
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="motivation">Stakeholder</span> Consumers and End Users (ESRS S4)

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Final consumers - focus on food safety and product information

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-down-004 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Association | [ESRS S4 - Consumers](ESRS S4 - Consumers.md) |
| Association | ArchiMate_Association | [Customers](Customers.md) |
| Abstraction | trace | [Consumers and End Users (ESRS S4)](../People/Consumers and End Users (ESRS S4).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | ArchiMate_Association | [Customers](Customers.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e310","label":"ESRS S4 - Consumers","fullName":"ESRS S4 - Consumers","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"ESRS S4 - Consumers.html"},{"id":"e274","label":"Customers","fullName":"Customers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Customers.html"},{"id":"e517","label":"Consumers and End Users…","fullName":"Consumers and End Users (ESRS S4)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Consumers and End Users (ESRS S4).html"},{"id":"e280","label":"Consumers and End Users…","fullName":"Consumers and End Users (ESRS S4)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e44","label":"ESRS S4 Consumers and E…","fullName":"ESRS S4 Consumers and End-users","packageName":"ESRS S4","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS S4/ESRS S4 Consumers and End-users.html"},{"id":"e246","label":"Supermarket Chains","fullName":"Supermarket Chains","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Supermarket Chains.html"},{"id":"e253","label":"Foodservice Companies","fullName":"Foodservice Companies","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Foodservice Companies.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e516","label":"Customers","fullName":"Customers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Customers.html"}],"edges":[{"id":"c249","source":"e280","target":"e310","label":"covered by","sourceLayer":"motivation"},{"id":"c572","source":"e44","target":"e310","label":"Abstraction","sourceLayer":"edgy-id"},{"id":"c255","source":"e246","target":"e274","label":"Generalization","sourceLayer":"motivation"},{"id":"c256","source":"e253","target":"e274","label":"Generalization","sourceLayer":"motivation"},{"id":"c268","source":"e331","target":"e274","label":"serves","sourceLayer":"motivation"},{"id":"c292","source":"e274","target":"e280","label":"serves","sourceLayer":"motivation"},{"id":"c490","source":"e274","target":"e516","label":"Abstraction","sourceLayer":"motivation"},{"id":"c492","source":"e280","target":"e517","label":"Abstraction","sourceLayer":"motivation"},{"id":"c525","source":"e516","target":"e517","label":"Association","sourceLayer":"edgy-pe"},{"id":"c562","source":"e517","target":"e44","label":"covered by","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-06-30 17:14:22*