---
ea_id: 284
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="motivation">Stakeholder</span> Seasonal Agricultural Workers

**Type:** Class  **Stereotype:** ArchiMate_Stakeholder  **StereotypeEx:** ArchiMate_Stakeholder  **FQStereotype:** ArchiMate3::ArchiMate_Stakeholder  
**Created:** 2025-11-14  **Modified:** 2025-11-14


[Home](../index.md) / [Archimate](../Archimate/index.md) / [ESRS Navigator Stakeholder Map](index.md)

Seasonal workers at supplier farms

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | id-up-006 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Generalization | ArchiMate_Specialization | [Workers in Value Chain (ESRS S2)](Workers in Value Chain (ESRS S2).md) |
| Abstraction | trace | [Seasonal Agricultural Workers](../People/Seasonal Agricultural Workers.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/ESRS Stakeholder Overview.html" class="diagram-thumb"><img src="diagrams/ESRS Stakeholder Overview.png" alt="ESRS Stakeholder Overview" loading="lazy"><span>ESRS Stakeholder Overview</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e267","label":"Workers in Value Chain …","fullName":"Workers in Value Chain (ESRS S2)","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Workers in Value Chain (ESRS S2).html"},{"id":"e533","label":"Seasonal Agricultural W…","fullName":"Seasonal Agricultural Workers","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Seasonal Agricultural Workers.html"},{"id":"e284","label":"Seasonal Agricultural W…","fullName":"Seasonal Agricultural Workers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e335","label":"Suppliers","fullName":"Suppliers","packageName":"ESRS Navigator Stakeholder Map","layer":"motivation","isFocal":false,"hasUrl":true,"url":"Suppliers.html"},{"id":"e282","label":"ESRS S2 - Value Chain W…","fullName":"ESRS S2 - Value Chain Workers","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"ESRS S2 - Value Chain Workers.html"},{"id":"e532","label":"Workers in Value Chain …","fullName":"Workers in Value Chain (ESRS S2)","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Workers in Value Chain (ESRS S2).html"}],"edges":[{"id":"c273","source":"e284","target":"e267","label":"Generalization","sourceLayer":"motivation"},{"id":"c282","source":"e267","target":"e335","label":"Association","sourceLayer":"motivation"},{"id":"c297","source":"e267","target":"e282","label":"covered by","sourceLayer":"motivation"},{"id":"c507","source":"e267","target":"e532","label":"Abstraction","sourceLayer":"motivation"},{"id":"c508","source":"e284","target":"e533","label":"Abstraction","sourceLayer":"motivation"},{"id":"c577","source":"e532","target":"e533","label":"Aggregation","sourceLayer":"edgy-pe"}]}</div>

---

*Generated: 2026-06-30 15:54:13*