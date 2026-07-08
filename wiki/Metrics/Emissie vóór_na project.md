---
ea_id: 201
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Metric</span> Emissie vóór/na project

**Type:** Requirement  **Stereotype:** Metric  **StereotypeEx:** Metric  **FQStereotype:** EDGY::Metric  
**Status:** <span class="status-badge status-proposed">Proposed</span>  
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
| Association | Link | [CMMS / projectenregistratie](CMMS _ projectenregistratie.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Aggregation | Tree | [Vermeden CO₂ door maatregelen (ton/jaar)](Vermeden CO₂ door maatregelen (ton_jaar).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e181&quot;,&quot;label&quot;:&quot;Vermeden CO₂ door maatr…&quot;,&quot;fullName&quot;:&quot;Vermeden CO₂ door maatregelen (ton/jaar)&quot;,&quot;packageName&quot;:&quot;Metrics&quot;,&quot;layer&quot;:&quot;edgy-lb&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Vermeden CO₂ door maatregelen (ton_jaar).html&quot;},{&quot;id&quot;:&quot;e222&quot;,&quot;label&quot;:&quot;CMMS / projectenregistr…&quot;,&quot;fullName&quot;:&quot;CMMS / projectenregistratie&quot;,&quot;packageName&quot;:&quot;Metrics&quot;,&quot;layer&quot;:&quot;edgy-lb&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;CMMS _ projectenregistratie.html&quot;},{&quot;id&quot;:&quot;e201&quot;,&quot;label&quot;:&quot;Emissie vóór/na project&quot;,&quot;fullName&quot;:&quot;Emissie vóór/na project&quot;,&quot;packageName&quot;:&quot;Metrics&quot;,&quot;layer&quot;:&quot;edgy-lb&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e36&quot;,&quot;label&quot;:&quot;ESRS E1 Climate Change&quot;,&quot;fullName&quot;:&quot;ESRS E1 Climate Change&quot;,&quot;packageName&quot;:&quot;ESRS E1&quot;,&quot;layer&quot;:&quot;edgy-id&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../ESRS E1/ESRS E1 Climate Change.html&quot;},{&quot;id&quot;:&quot;e155&quot;,&quot;label&quot;:&quot;Start projecten voor en…&quot;,&quot;fullName&quot;:&quot;Start projecten voor energiebesparing, transportoptimalisatie en circulariteit.&quot;,&quot;packageName&quot;:&quot;Task&quot;,&quot;layer&quot;:&quot;edgy-ex&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Task/Start projecten voor energiebesparing, transportoptimalisatie en circulariteit..html&quot;},{&quot;id&quot;:&quot;e200&quot;,&quot;label&quot;:&quot;Energieverbruik vóór/na…&quot;,&quot;fullName&quot;:&quot;Energieverbruik vóór/na project&quot;,&quot;packageName&quot;:&quot;Metrics&quot;,&quot;layer&quot;:&quot;edgy-lb&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Energieverbruik vóór_na project.html&quot;},{&quot;id&quot;:&quot;e202&quot;,&quot;label&quot;:&quot;Projectgegevens en tech…&quot;,&quot;fullName&quot;:&quot;Projectgegevens en tech specs&quot;,&quot;packageName&quot;:&quot;Metrics&quot;,&quot;layer&quot;:&quot;edgy-lb&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Projectgegevens en tech specs.html&quot;},{&quot;id&quot;:&quot;e234&quot;,&quot;label&quot;:&quot;Emissiereductie (%) = (…&quot;,&quot;fullName&quot;:&quot;Emissiereductie (%) = ((baseline – huidig) / baseline) × 100&quot;,&quot;packageName&quot;:&quot;Metrics&quot;,&quot;layer&quot;:&quot;edgy-lb&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Emissiereductie (%) = ((baseline – huidig) _ baseline) × 100.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c106&quot;,&quot;source&quot;:&quot;e36&quot;,&quot;target&quot;:&quot;e181&quot;,&quot;label&quot;:&quot;ControlFlow&quot;,&quot;sourceLayer&quot;:&quot;edgy-id&quot;},{&quot;id&quot;:&quot;c133&quot;,&quot;source&quot;:&quot;e155&quot;,&quot;target&quot;:&quot;e181&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;edgy-ex&quot;},{&quot;id&quot;:&quot;c201&quot;,&quot;source&quot;:&quot;e181&quot;,&quot;target&quot;:&quot;e200&quot;,&quot;label&quot;:&quot;Aggregation&quot;,&quot;sourceLayer&quot;:&quot;edgy-lb&quot;},{&quot;id&quot;:&quot;c202&quot;,&quot;source&quot;:&quot;e181&quot;,&quot;target&quot;:&quot;e201&quot;,&quot;label&quot;:&quot;Aggregation&quot;,&quot;sourceLayer&quot;:&quot;edgy-lb&quot;},{&quot;id&quot;:&quot;c203&quot;,&quot;source&quot;:&quot;e181&quot;,&quot;target&quot;:&quot;e202&quot;,&quot;label&quot;:&quot;Aggregation&quot;,&quot;sourceLayer&quot;:&quot;edgy-lb&quot;},{&quot;id&quot;:&quot;c233&quot;,&quot;source&quot;:&quot;e181&quot;,&quot;target&quot;:&quot;e234&quot;,&quot;label&quot;:&quot;ControlFlow&quot;,&quot;sourceLayer&quot;:&quot;edgy-lb&quot;},{&quot;id&quot;:&quot;c222&quot;,&quot;source&quot;:&quot;e201&quot;,&quot;target&quot;:&quot;e222&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;edgy-lb&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:48*