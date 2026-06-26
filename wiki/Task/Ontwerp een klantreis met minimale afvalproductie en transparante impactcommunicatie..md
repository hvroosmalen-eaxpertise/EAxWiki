# <span class="sl" data-layer="edgy-ex">Task</span> Ontwerp een klantreis met minimale afvalproductie en transparante impactcommunicatie.

**Type:** Activity  **Stereotype:** Task  **StereotypeEx:** Task  **FQStereotype:** EDGY::Task  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-02  **Modified:** 2025-12-15


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Experience](../Experience/index.md) / [Task](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Top | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [Green Customer Journey](../Journey/Green Customer Journey.md) |
| Association | Link | [CO₂ per klanttransactie (kg)](../Metrics/CO₂ per klanttransactie (kg).md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Experience](../Experience/diagrams/Experience.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Green Customer Journey](../Journey/Green Customer Journey.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e126","label":"Green Customer Journey","fullName":"Green Customer Journey","packageName":"Journey","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Journey/Green Customer Journey.html"},{"id":"e172","label":"CO₂ per klanttransactie…","fullName":"CO₂ per klanttransactie (kg)","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/CO₂ per klanttransactie (kg).html"},{"id":"e146","label":"Ontwerp een klantreis m…","fullName":"Ontwerp een klantreis met minimale afvalproductie en transparante impactcommunicatie.","packageName":"Task","layer":"edgy-ex","isFocal":true,"hasUrl":false,"url":""},{"id":"e14","label":"SDG 12. Responsible Con…","fullName":"SDG 12. Responsible Consumption and Production","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 12. Responsible Consumption and Production.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"}],"edges":[{"id":"c39","source":"e126","target":"e146","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c63","source":"e14","target":"e126","label":"Association","sourceLayer":"edgy-id"},{"id":"c96","source":"e36","target":"e172","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c140","source":"e146","target":"e172","label":"Association","sourceLayer":"edgy-ex"}]}</div>

---

*Generated: 2026-06-26 17:02:52*