# <span class="sl" data-layer="edgy-ex">Task</span> Meet CO₂, circulariteit, energiegebruik en stel KPI’s op.

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
| ControlFlow | Flow | [Sustainability Metrics](../Asset/Sustainability Metrics.md) |
| Association | Link | [Energie (MWh/jaar)](../Metrics/Energie (MWh_jaar).md) |
| Association | Link | [Recycling % van afval](../Metrics/Recycling % van afval.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Sustainability Metrics](../Asset/Sustainability Metrics.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e139","label":"Sustainability Metrics","fullName":"Sustainability Metrics","packageName":"Asset","isFocal":false,"hasUrl":true,"url":"../Asset/Sustainability Metrics.html"},{"id":"e186","label":"Energie (MWh/jaar)","fullName":"Energie (MWh/jaar)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Energie (MWh_jaar).html"},{"id":"e187","label":"Recycling % van afval","fullName":"Recycling % van afval","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Recycling % van afval.html"},{"id":"e160","label":"Meet CO₂, circulariteit…","fullName":"Meet CO₂, circulariteit, energiegebruik en stel KPI’s op.","packageName":"Task","isFocal":true,"hasUrl":false,"url":""},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":"Sustainability Development Goals","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e40","label":"ESRS E5 Resource Use an…","fullName":"ESRS E5 Resource Use and Circular Economy","packageName":"ESRS E5","isFocal":false,"hasUrl":true,"url":"../ESRS E5/ESRS E5 Resource Use and Circular Economy.html"}],"edges":[{"id":"c54","source":"e139","target":"e160","label":"ControlFlow"},{"id":"c88","source":"e15","target":"e139","label":"Association"},{"id":"c115","source":"e36","target":"e186","label":"ControlFlow"},{"id":"c116","source":"e40","target":"e186","label":"ControlFlow"},{"id":"c136","source":"e160","target":"e186","label":"Association"},{"id":"c117","source":"e36","target":"e187","label":"ControlFlow"},{"id":"c118","source":"e40","target":"e187","label":"ControlFlow"},{"id":"c137","source":"e160","target":"e187","label":"Association"}]};</script>

---

*Generated: 2026-06-26 15:09:21*