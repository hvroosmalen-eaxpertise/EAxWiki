# <span class="sl" data-layer="edgy-lb">Metric</span> Energieverbruik vóór/na project

**Type:** Requirement  **Stereotype:** Metric  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::MetricStatus | Good | Default: Good  |
| EDGY::MetricValue | <VALUE> | Default: <VALUE>  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Aggregation | Tree | [Vermeden CO₂ door maatregelen (ton/jaar)](Vermeden CO₂ door maatregelen (ton_jaar).md) |
| Association | Link | [EMS-trends, IoT-logs](EMS-trends, IoT-logs.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Metrics](diagrams/Metrics.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [Vermeden CO₂ door maatregelen (ton/jaar)](Vermeden CO₂ door maatregelen (ton_jaar).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e181","label":"Vermeden CO₂ door maatr…","fullName":"Vermeden CO₂ door maatregelen (ton/jaar)","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Vermeden CO₂ door maatregelen (ton_jaar).html"},{"id":"e221","label":"EMS-trends, IoT-logs","fullName":"EMS-trends, IoT-logs","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"EMS-trends, IoT-logs.html"},{"id":"e200","label":"Energieverbruik vóór/na…","fullName":"Energieverbruik vóór/na project","packageName":Metrics,"isFocal":true,"hasUrl":false,"url":""},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":ESRS E1,"isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e155","label":"Start projecten voor en…","fullName":"Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.","packageName":Task,"isFocal":false,"hasUrl":true,"url":"../Task/Start projecten voor energiebesparing, transportoptimalisatie en circulariteit..html"},{"id":"e201","label":"Emissie vóór/na project","fullName":"Emissie vóór/na project","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Emissie vóór_na project.html"},{"id":"e202","label":"Projectgegevens en tech…","fullName":"Projectgegevens en tech specs","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Projectgegevens en tech specs.html"},{"id":"e234","label":"Emissiereductie (%) = (…","fullName":"Emissiereductie (%) = ((baseline – huidig) / baseline) × 100","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"Emissiereductie (%) = ((baseline – huidig) _ baseline) × 100.html"}],"edges":[{"id":"c106","source":"e36","target":"e181","label":"ControlFlow"},{"id":"c133","source":"e155","target":"e181","label":"Association"},{"id":"c201","source":"e181","target":"e200","label":"Aggregation"},{"id":"c202","source":"e181","target":"e201","label":"Aggregation"},{"id":"c203","source":"e181","target":"e202","label":"Aggregation"},{"id":"c233","source":"e181","target":"e234","label":"ControlFlow"},{"id":"c221","source":"e200","target":"e221","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:35*