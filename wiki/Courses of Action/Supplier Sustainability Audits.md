# <span class="sl" data-layer="strategy">CourseOfAction</span> Supplier Sustainability Audits

**Type:** Class  **Stereotype:** ArchiMate_CourseOfAction  **StereotypeEx:** ArchiMate_CourseOfAction  **FQStereotype:** ArchiMate3::ArchiMate_CourseOfAction  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Courses of Action](index.md)

Systematic program to assess and verify supplier compliance with environmental and social standards.         
Conducts on-site audits, evaluates supplier practices, and implements corrective action programs.         
Ensures supply chain transparency and responsible sourcing practices.

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | action-004 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Dependency | ArchiMate_Realization | [Sustainable Supply Chain Management](../Capabilities/Sustainable Supply Chain Management.md) |
| ControlFlow | ArchiMate_Influence | [Lower Operational Risk](../Outcomes/Lower Operational Risk.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Strategic Sustainability Management Model (Bodenstein)](../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).md)
- [Climate Risk](../Climate Risk/diagrams/Climate Risk.md)
- [Regulatory Pressure](../Regulatory Pressure/diagrams/Regulatory Pressure.md)
- [Reputation Risk](../Reputation Risk/diagrams/Reputation Risk.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Dependency | ArchiMate_Realization | [Sustainable Supply Chain Management](../Capabilities/Sustainable Supply Chain Management.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e447","label":"Sustainable Supply Chai…","fullName":"Sustainable Supply Chain Management","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Capabilities/Sustainable Supply Chain Management.html"},{"id":"e459","label":"Lower Operational Risk","fullName":"Lower Operational Risk","packageName":"Outcomes","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Outcomes/Lower Operational Risk.html"},{"id":"e454","label":"Supplier Sustainability…","fullName":"Supplier Sustainability Audits","packageName":"Courses of Action","layer":"strategy","isFocal":true,"hasUrl":false,"url":""},{"id":"e442","label":"Responsible Supply Chai…","fullName":"Responsible Supply Chain Strategy","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"Responsible Supply Chain Strategy.html"},{"id":"e464","label":"Supplier Compliance Rate","fullName":"Supplier Compliance Rate","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/Supplier Compliance Rate.html"},{"id":"e436","label":"Risk Reduction","fullName":"Risk Reduction","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Risk Reduction.html"}],"edges":[{"id":"c430","source":"e447","target":"e442","label":"realizes","sourceLayer":"strategy"},{"id":"c437","source":"e447","target":"e454","label":"realizes","sourceLayer":"strategy"},{"id":"c457","source":"e464","target":"e447","label":"measures","sourceLayer":"motivation"},{"id":"c443","source":"e454","target":"e459","label":"influences","sourceLayer":"strategy"},{"id":"c449","source":"e459","target":"e436","label":"realizes","sourceLayer":"motivation"},{"id":"c456","source":"e464","target":"e459","label":"measures","sourceLayer":"motivation"},{"id":"c423","source":"e442","target":"e436","label":"realizes","sourceLayer":"strategy"}]}</div>

---

*Generated: 2026-06-29 13:30:17*