# <span class="sl" data-layer="motivation">Stakeholder</span> Seasonal Workers

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Temporary/seasonal employees

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-emp-003 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Generalization | ArchiMate_Specialization | [Own Personnel (ESRS S1)](Own Personnel (ESRS S1).md) |
| Abstraction | trace | [Seasonal Workers](../People/Seasonal Workers.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb diagram-thumb--noimg"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e329","label":"Own Personnel (ESRS S1)","fullName":"Own Personnel (ESRS S1)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Own Personnel (ESRS S1).html"},{"id":"e491","label":"Seasonal Workers","fullName":"Seasonal Workers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Seasonal Workers.html"},{"id":"e318","label":"Seasonal Workers","fullName":"Seasonal Workers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e331","label":"Large Companies (&gt;250 e…","fullName":"Large Companies (&gt;250 employees)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Large Companies (_250 employees).html"},{"id":"e323","label":"Production Workers","fullName":"Production Workers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Production Workers.html"},{"id":"e287","label":"Trade Unions","fullName":"Trade Unions","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Trade Unions.html"},{"id":"e277","label":"Permanent Employees","fullName":"Permanent Employees","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Permanent Employees.html"},{"id":"e279","label":"Works Council (Ondernem…","fullName":"Works Council (Ondernemingsraad)","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"Works Council (Ondernemingsraad).html"},{"id":"e321","label":"ESRS S1 - Own Personnel","fullName":"ESRS S1 - Own Personnel","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"ESRS S1 - Own Personnel.html"},{"id":"e489","label":"Own Personnel (ESRS S1)","fullName":"Own Personnel (ESRS S1)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Own Personnel (ESRS S1).html"}],"edges":[{"id":"c247","source":"e329","target":"e331","label":"Association","sourceLayer":"motivation"},{"id":"c283","source":"e323","target":"e329","label":"Generalization","sourceLayer":"motivation"},{"id":"c295","source":"e287","target":"e329","label":"represents","sourceLayer":"motivation"},{"id":"c308","source":"e277","target":"e329","label":"Generalization","sourceLayer":"motivation"},{"id":"c316","source":"e279","target":"e329","label":"represents","sourceLayer":"business"},{"id":"c338","source":"e329","target":"e321","label":"covered by","sourceLayer":"motivation"},{"id":"c343","source":"e318","target":"e329","label":"Generalization","sourceLayer":"motivation"},{"id":"c463","source":"e329","target":"e489","label":"Abstraction","sourceLayer":"motivation"},{"id":"c465","source":"e318","target":"e491","label":"Abstraction","sourceLayer":"motivation"},{"id":"c546","source":"e489","target":"e491","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-06-29 18:57:22*