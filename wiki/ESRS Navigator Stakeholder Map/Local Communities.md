# <span class="sl" data-layer="motivation">Stakeholder</span> Local Communities

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Communities around production facilities and supplier regions

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

*Generated: 2026-06-30 11:43:28*