# <span class="sl" data-layer="business">BusinessRole</span> Sustainability Manager/Director

**Type:** Class  **Stereotype:** ArchiMate_BusinessRole  **StereotypeEx:** ArchiMate_BusinessRole  **FQStereotype:** ArchiMate3::ArchiMate_BusinessRole  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Coordinates implementation of sustainability measures, reports to CEO

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-gov-006 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Association | [CEO](CEO.md) |
| Abstraction | trace | [Sustainability Manager/Director](../People/Sustainability Manager_Director.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [ESRS Stakeholder Overview](diagrams/ESRS Stakeholder Overview.md)

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e265","label":"CEO","fullName":"CEO","packageName":"ESRS Navigator Stakeholder Map","isFocal":false,"hasUrl":true,"url":"CEO.html"},{"id":"e494","label":"Sustainability Manager/…","fullName":"Sustainability Manager/Director","packageName":"People","isFocal":false,"hasUrl":true,"url":"../People/Sustainability Manager_Director.html"},{"id":"e269","label":"Sustainability Manager/…","fullName":"Sustainability Manager/Director","packageName":"ESRS Navigator Stakeholder Map","isFocal":true,"hasUrl":false,"url":""},{"id":"e266","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"ESRS Navigator Stakeholder Map","isFocal":false,"hasUrl":true,"url":"Board of Directors (Directie).html"},{"id":"e495","label":"CEO","fullName":"CEO","packageName":"People","isFocal":false,"hasUrl":true,"url":"../People/CEO.html"},{"id":"e496","label":"Board of Directors (Dir…","fullName":"Board of Directors (Directie)","packageName":"People","isFocal":false,"hasUrl":true,"url":"../People/Board of Directors (Directie).html"}],"edges":[{"id":"c290","source":"e269","target":"e265","label":"reports to"},{"id":"c304","source":"e266","target":"e265","label":"Association"},{"id":"c469","source":"e265","target":"e495","label":"Abstraction"},{"id":"c468","source":"e269","target":"e494","label":"Abstraction"},{"id":"c541","source":"e496","target":"e494","label":"Aggregation"},{"id":"c543","source":"e494","target":"e495","label":"reports to"},{"id":"c470","source":"e266","target":"e496","label":"Abstraction"},{"id":"c542","source":"e496","target":"e495","label":"Aggregation"}]}</script>

---

*Generated: 2026-06-26 16:40:39*