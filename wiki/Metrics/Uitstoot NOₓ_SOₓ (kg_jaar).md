---
ea_id: 168
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Metric</span> Uitstoot NOₓ/SOₓ (kg/jaar)

**Type:** Requirement  **Stereotype:** Metric  **StereotypeEx:** Metric  **FQStereotype:** EDGY::Metric  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="168" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Metrics/Uitstoot NOₓ_SOₓ (kg_jaar).md" data-api-port="8001"></div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::MetricStatus | Good | Default: Good  |
| EDGY::MetricValue | <VALUE> | Default: <VALUE>  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [ESRS E2 Pollution](../ESRS E2/ESRS E2 Pollution.md) |
| Association | Link | [Maak een publiek impactrapport over emissies, afval, water en circulariteit.](../Task/Maak een publiek impactrapport over emissies, afval, water en circulariteit..md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Identity/diagrams/Identity.html" class="diagram-thumb"><img src="../Identity/diagrams/Identity.png" alt="Identity" loading="lazy"><span>Identity</span></a>
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [ESRS E2 Pollution](../ESRS E2/ESRS E2 Pollution.md) |
| Association | Link | [Maak een publiek impactrapport over emissies, afval, water en circulariteit.](../Task/Maak een publiek impactrapport over emissies, afval, water en circulariteit..md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e37","label":"ESRS E2 Pollution","fullName":"ESRS E2 Pollution","packageName":"ESRS E2","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E2/ESRS E2 Pollution.html"},{"id":"e142","label":"Maak een publiek impact…","fullName":"Maak een publiek impactrapport over emissies, afval, water en circulariteit.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Maak een publiek impactrapport over emissies, afval, water en circulariteit..html"},{"id":"e168","label":"Uitstoot NOₓ/SOₓ (kg/ja…","fullName":"Uitstoot NOₓ/SOₓ (kg/jaar)","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e185","label":"# SDGs met meetbare KPI…","fullName":"# SDGs met meetbare KPI’s","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"_ SDGs met meetbare KPI’s.html"},{"id":"e510","label":"Company subject to CSRD","fullName":"Company subject to CSRD","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/Company subject to CSRD.html"},{"id":"e248","label":"ESRS E2 - Pollution","fullName":"ESRS E2 - Pollution","packageName":"ESRS Navigator Stakeholder Map","layer":"business","isFocal":false,"hasUrl":true,"url":"../ESRS Navigator Stakeholder Map/ESRS E2 - Pollution.html"},{"id":"e531","label":"European Commission","fullName":"European Commission","packageName":"People","layer":"edgy-pe","isFocal":false,"hasUrl":true,"url":"../People/European Commission.html"},{"id":"e555","label":"European Sustainability…","fullName":"European Sustainability Reporting Standards","packageName":"European Sustainability Reporting Standards","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../European Sustainability Reporting Standards/European Sustainability Reporting Standards.html"},{"id":"e122","label":"Environmental Impact St…","fullName":"Environmental Impact Story","packageName":"Story","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Story/Environmental Impact Story.html"}],"edges":[{"id":"c92","source":"e37","target":"e168","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c114","source":"e37","target":"e185","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c557","source":"e510","target":"e37","label":"reports according to","sourceLayer":"edgy-pe"},{"id":"c567","source":"e248","target":"e37","label":"Abstraction","sourceLayer":"business"},{"id":"c587","source":"e531","target":"e37","label":"Association","sourceLayer":"edgy-pe"},{"id":"c600","source":"e555","target":"e37","label":"Aggregation","sourceLayer":"edgy-id"},{"id":"c35","source":"e122","target":"e142","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c121","source":"e142","target":"e168","label":"Association","sourceLayer":"edgy-ex"}]}</div>

---

*Generated: 2026-07-01 10:25:43*