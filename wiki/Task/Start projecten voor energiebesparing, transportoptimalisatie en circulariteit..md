# <span class="sl" data-layer="uml">TAS</span> Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.

**Type:** Activity  **Stereotype:** Task  **Status:** <span class="status-badge status-proposed">Proposed</span>  
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
| ControlFlow | Flow | [Reduction Initiatives](../Process/Reduction Initiatives.md) |
| Association | Link | [Vermeden CO₂ door maatregelen (ton/jaar)](../Metrics/Vermeden CO₂ door maatregelen (ton_jaar).md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Reduction Initiatives](../Process/Reduction Initiatives.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e134","label":"Reduction Initiatives","fullName":"Reduction Initiatives","packageName":Process,"isFocal":false,"hasUrl":true,"url":"../Process/Reduction Initiatives.html"},{"id":"e181","label":"Vermeden CO₂ door maatr…","fullName":"Vermeden CO₂ door maatregelen (ton/jaar)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Vermeden CO₂ door maatregelen (ton_jaar).html"},{"id":"e155","label":"Start projecten voor en…","fullName":"Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.","packageName":Task,"isFocal":true,"hasUrl":false,"url":""},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":Sustainability Development Goals,"isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":ESRS E1,"isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e200","label":"Energieverbruik vóór/na…","fullName":"Energieverbruik vóór/na project","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Energieverbruik vóór_na project.html"},{"id":"e201","label":"Emissie vóór/na project","fullName":"Emissie vóór/na project","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Emissie vóór_na project.html"},{"id":"e202","label":"Projectgegevens en tech…","fullName":"Projectgegevens en tech specs","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Projectgegevens en tech specs.html"},{"id":"e234","label":"Emissiereductie (%) = (…","fullName":"Emissiereductie (%) = ((baseline – huidig) / baseline) × 100","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Emissiereductie (%) = ((baseline – huidig) _ baseline) × 100.html"}],"edges":[{"id":"c49","source":"e134","target":"e155","label":"ControlFlow"},{"id":"c78","source":"e15","target":"e134","label":"Association"},{"id":"c106","source":"e36","target":"e181","label":"ControlFlow"},{"id":"c133","source":"e155","target":"e181","label":"Association"},{"id":"c201","source":"e181","target":"e200","label":"Aggregation"},{"id":"c202","source":"e181","target":"e201","label":"Aggregation"},{"id":"c203","source":"e181","target":"e202","label":"Aggregation"},{"id":"c233","source":"e181","target":"e234","label":"ControlFlow"}]};</script>

---

*Generated: 2026-06-26 09:44:48*