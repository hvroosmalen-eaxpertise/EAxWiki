---
ea_id: 293
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="business">BusinessRole</span> CFO

**Type:** Class  **Stereotype:** ArchiMate_BusinessRole  **StereotypeEx:** ArchiMate_BusinessRole  **FQStereotype:** ArchiMate3::ArchiMate_BusinessRole  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Chief Financial Officer - Often responsible for compliance

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-gov-003 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Composition | [Board of Directors (Directie)](Board of Directors (Directie).md) |
| Abstraction | trace | [Chief Financial Officer](../People/Chief Financial Officer.md) |

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

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e266","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"Board of Directors (Directie).html"},{"id":"e497","label":"Chief Financial Officer","fullName":"Chief Financial Officer","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Chief Financial Officer.html"},{"id":"e293","label":"CFO","fullName":"CFO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":true,"hasUrl":false,"url":""},{"id":"e268","label":"COO","fullName":"COO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"COO.html"},{"id":"e265","label":"CEO","fullName":"CEO","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"CEO.html"},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e496","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Board of Directors (Directie).html"}],"edges":[{"id":"c241","source":"e266","target":"e268","label":"Association","sourceLayer":"business"},{"id":"c304","source":"e266","target":"e265","label":"Association","sourceLayer":"business"},{"id":"c318","source":"e266","target":"e293","label":"Association","sourceLayer":"business"},{"id":"c322","source":"e266","target":"e331","label":"Association","sourceLayer":"business"},{"id":"c470","source":"e266","target":"e496","label":"Abstraction","sourceLayer":"business"},{"id":"c471","source":"e293","target":"e497","label":"Abstraction","sourceLayer":"business"},{"id":"c540","source":"e496","target":"e497","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-06-30 15:54:13*