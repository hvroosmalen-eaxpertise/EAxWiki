# <span class="sl" data-layer="motivation">Assessment</span> Supplier Compliance Rate

**Type:** Class  **Stereotype:** ArchiMate_Assessment  **StereotypeEx:** ArchiMate_Assessment  **FQStereotype:** ArchiMate3::ArchiMate_Assessment  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Assessments](index.md)

Percentage of suppliers meeting defined environmental and social standards based on audits and assessments.         
Measures effectiveness of sustainable supply chain management programs.         
Indicates level of control over value chain sustainability impacts.

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | kpi-003 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association | ArchiMate_Association | [Lower Operational Risk](../Outcomes/Lower Operational Risk.md) |
| Association | ArchiMate_Association | [Sustainable Supply Chain Management](../Capabilities/Sustainable Supply Chain Management.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Strategic Sustainability Management Model (Bodenstein)](../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).md)
- [Climate Risk](../Climate Risk/diagrams/Climate Risk.md)
- [Regulatory Pressure](../Regulatory Pressure/diagrams/Regulatory Pressure.md)
- [Reputation Risk](../Reputation Risk/diagrams/Reputation Risk.md)

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e459","label":"Lower Operational Risk","fullName":"Lower Operational Risk","packageName":"Outcomes","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Outcomes/Lower Operational Risk.html"},{"id":"e447","label":"Sustainable Supply Chai…","fullName":"Sustainable Supply Chain Management","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Capabilities/Sustainable Supply Chain Management.html"},{"id":"e464","label":"Supplier Compliance Rate","fullName":"Supplier Compliance Rate","packageName":"Assessments","layer":"motivation","isFocal":true,"hasUrl":false,"url":""},{"id":"e454","label":"Supplier Sustainability…","fullName":"Supplier Sustainability Audits","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Supplier Sustainability Audits.html"},{"id":"e436","label":"Risk Reduction","fullName":"Risk Reduction","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Risk Reduction.html"},{"id":"e442","label":"Responsible Supply Chai…","fullName":"Responsible Supply Chain Strategy","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Responsible Supply Chain Strategy.html"}],"edges":[{"id":"c443","source":"e454","target":"e459","label":"influences","sourceLayer":"strategy"},{"id":"c449","source":"e459","target":"e436","label":"realizes","sourceLayer":"motivation"},{"id":"c456","source":"e464","target":"e459","label":"measures","sourceLayer":"motivation"},{"id":"c430","source":"e447","target":"e442","label":"realizes","sourceLayer":"strategy"},{"id":"c437","source":"e447","target":"e454","label":"realizes","sourceLayer":"strategy"},{"id":"c457","source":"e464","target":"e447","label":"measures","sourceLayer":"motivation"},{"id":"c423","source":"e442","target":"e436","label":"realizes","sourceLayer":"strategy"}]}</div>

---

*Generated: 2026-06-26 17:14:26*