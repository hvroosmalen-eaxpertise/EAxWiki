# <span class="sl" data-layer="strategy">CAP</span> Sustainable Supply Chain Management

**Type:** Class  **Stereotype:** ArchiMate_Capability  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Capabilities](index.md)

Capability to manage environmental and social sustainability throughout the supply chain.         
Includes supplier assessment, auditing, development programs, and collaborative sustainability initiatives.         
Ensures responsible sourcing and addresses upstream environmental and social impacts.

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | capability-004 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Dependency | ArchiMate_Realization | [Responsible Supply Chain Strategy](../Courses of Action/Responsible Supply Chain Strategy.md) |
| Dependency | ArchiMate_Realization | [Supplier Sustainability Audits](../Courses of Action/Supplier Sustainability Audits.md) |
| Association | ArchiMate_Association | [Supplier Compliance Rate](../Assessments/Supplier Compliance Rate.md) |

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
| Association | ArchiMate_Association | [Supplier Compliance Rate](../Assessments/Supplier Compliance Rate.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e442","label":"Responsible Supply Chai…","fullName":"Responsible Supply Chain Strategy","packageName":Courses of Action,"isFocal":false,"hasUrl":true,"url":"../Courses of Action/Responsible Supply Chain Strategy.html"},{"id":"e454","label":"Supplier Sustainability…","fullName":"Supplier Sustainability Audits","packageName":Courses of Action,"isFocal":false,"hasUrl":true,"url":"../Courses of Action/Supplier Sustainability Audits.html"},{"id":"e464","label":"Supplier Compliance Rate","fullName":"Supplier Compliance Rate","packageName":Assessments,"isFocal":false,"hasUrl":true,"url":"../Assessments/Supplier Compliance Rate.html"},{"id":"e447","label":"Sustainable Supply Chai…","fullName":"Sustainable Supply Chain Management","packageName":Capabilities,"isFocal":true,"hasUrl":false,"url":""},{"id":"e436","label":"Risk Reduction","fullName":"Risk Reduction","packageName":Goals,"isFocal":false,"hasUrl":true,"url":"../Goals/Risk Reduction.html"},{"id":"e459","label":"Lower Operational Risk","fullName":"Lower Operational Risk","packageName":Outcomes,"isFocal":false,"hasUrl":true,"url":"../Outcomes/Lower Operational Risk.html"}],"edges":[{"id":"c423","source":"e442","target":"e436","label":"realizes"},{"id":"c430","source":"e447","target":"e442","label":"realizes"},{"id":"c437","source":"e447","target":"e454","label":"realizes"},{"id":"c443","source":"e454","target":"e459","label":"influences"},{"id":"c456","source":"e464","target":"e459","label":"measures"},{"id":"c457","source":"e464","target":"e447","label":"measures"},{"id":"c449","source":"e459","target":"e436","label":"realizes"}]};</script>

---

*Generated: 2026-06-26 13:25:35*