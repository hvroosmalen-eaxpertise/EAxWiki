# <span class="sl" data-layer="strategy">Capability</span> ESG Performance Management

**Type:** Class  **Stereotype:** ArchiMate_Capability  **StereotypeEx:** ArchiMate_Capability  **FQStereotype:** ArchiMate3::ArchiMate_Capability  
**Created:** 2025-12-11  **Modified:** 2025-12-11


[Home](../index.md) / [Archimate](../Archimate/index.md) / [Elements](../Elements/index.md) / [Capabilities](index.md)

Capability to systematically measure, monitor, and manage Environmental, Social, and Governance performance.         
Encompasses KPI frameworks, data collection systems, performance analysis, and continuous improvement methodologies.         
Enables evidence-based sustainability management and transparent reporting to stakeholders.

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| archimate_element_identifier | capability-002 |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Dependency | ArchiMate_Realization | [Integrate Sustainability into Corporate Strategy](../Courses of Action/Integrate Sustainability into Corporate Strategy.md) |
| Dependency | ArchiMate_Realization | [Implement ESG KPI Framework](../Courses of Action/Implement ESG KPI Framework.md) |
| Dependency | ArchiMate_Realization | [Carbon Footprint Reduction Program](../Courses of Action/Carbon Footprint Reduction Program.md) |
| Association | ArchiMate_Association | [CO₂ Reduction %](../Assessments/CO₂ Reduction %.md) |
| Association | ArchiMate_Association | [ESG Score](../Assessments/ESG Score.md) |
| Association | ArchiMate_Association | [Energy Efficiency Index](../Assessments/Energy Efficiency Index.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="../Strategic Sustainability Management Model (Bodenstein)/diagrams/Strategic Sustainability Management Model (Bodenstein).html" class="diagram-thumb diagram-thumb--noimg"><span>Strategic Sustainability Management Model (Bodenstein)</span></a>
  <a href="../Stakeholder Expectations/diagrams/Stakeholder Expectations.html" class="diagram-thumb diagram-thumb--noimg"><span>Stakeholder Expectations</span></a>
  <a href="../Climate Risk/diagrams/Climate Risk.html" class="diagram-thumb diagram-thumb--noimg"><span>Climate Risk</span></a>
  <a href="../Regulatory Pressure/diagrams/Regulatory Pressure.html" class="diagram-thumb diagram-thumb--noimg"><span>Regulatory Pressure</span></a>
  <a href="../Market Differentiation Pressure/diagrams/Market Differentiation Pressure.html" class="diagram-thumb diagram-thumb--noimg"><span>Market Differentiation Pressure</span></a>
  <a href="../Reputation Risk/diagrams/Reputation Risk.html" class="diagram-thumb diagram-thumb--noimg"><span>Reputation Risk</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | ArchiMate_Association | [CO₂ Reduction %](../Assessments/CO₂ Reduction %.md) |
| Association | ArchiMate_Association | [ESG Score](../Assessments/ESG Score.md) |
| Association | ArchiMate_Association | [Energy Efficiency Index](../Assessments/Energy Efficiency Index.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e439","label":"Integrate Sustainabilit…","fullName":"Integrate Sustainability into Corporate Strategy","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Integrate Sustainability into Corporate Strategy.html"},{"id":"e451","label":"Implement ESG KPI Frame…","fullName":"Implement ESG KPI Framework","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Implement ESG KPI Framework.html"},{"id":"e456","label":"Carbon Footprint Reduct…","fullName":"Carbon Footprint Reduction Program","packageName":"Courses of Action","layer":"strategy","isFocal":false,"hasUrl":true,"url":"../Courses of Action/Carbon Footprint Reduction Program.html"},{"id":"e462","label":"CO₂ Reduction %","fullName":"CO₂ Reduction %","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/CO₂ Reduction %.html"},{"id":"e463","label":"ESG Score","fullName":"ESG Score","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/ESG Score.html"},{"id":"e465","label":"Energy Efficiency Index","fullName":"Energy Efficiency Index","packageName":"Assessments","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Assessments/Energy Efficiency Index.html"},{"id":"e445","label":"ESG Performance Managem…","fullName":"ESG Performance Management","packageName":"Capabilities","layer":"strategy","isFocal":true,"hasUrl":false,"url":""},{"id":"e437","label":"Corporate Sustainabilit…","fullName":"Corporate Sustainability Integration","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Corporate Sustainability Integration.html"},{"id":"e434","label":"Long-term Value Creation","fullName":"Long-term Value Creation","packageName":"Goals","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Goals/Long-term Value Creation.html"},{"id":"e444","label":"Sustainability Governan…","fullName":"Sustainability Governance","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"Sustainability Governance.html"},{"id":"e448","label":"Sustainability Data and…","fullName":"Sustainability Data and Reporting","packageName":"Capabilities","layer":"strategy","isFocal":false,"hasUrl":true,"url":"Sustainability Data and Reporting.html"},{"id":"e458","label":"Improved ESG Rating","fullName":"Improved ESG Rating","packageName":"Outcomes","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Outcomes/Improved ESG Rating.html"},{"id":"e457","label":"Reduced Environmental I…","fullName":"Reduced Environmental Impact","packageName":"Outcomes","layer":"motivation","isFocal":false,"hasUrl":true,"url":"../Outcomes/Reduced Environmental Impact.html"}],"edges":[{"id":"c419","source":"e439","target":"e437","label":"realizes","sourceLayer":"strategy"},{"id":"c420","source":"e439","target":"e434","label":"realizes","sourceLayer":"strategy"},{"id":"c426","source":"e444","target":"e439","label":"realizes","sourceLayer":"strategy"},{"id":"c427","source":"e445","target":"e439","label":"realizes","sourceLayer":"strategy"},{"id":"c431","source":"e448","target":"e439","label":"realizes","sourceLayer":"strategy"},{"id":"c434","source":"e445","target":"e451","label":"realizes","sourceLayer":"strategy"},{"id":"c440","source":"e451","target":"e458","label":"influences","sourceLayer":"strategy"},{"id":"c439","source":"e445","target":"e456","label":"realizes","sourceLayer":"strategy"},{"id":"c445","source":"e456","target":"e457","label":"influences","sourceLayer":"strategy"},{"id":"c452","source":"e462","target":"e457","label":"measures","sourceLayer":"motivation"},{"id":"c453","source":"e462","target":"e445","label":"measures","sourceLayer":"motivation"},{"id":"c454","source":"e463","target":"e458","label":"measures","sourceLayer":"motivation"},{"id":"c455","source":"e463","target":"e445","label":"measures","sourceLayer":"motivation"},{"id":"c458","source":"e465","target":"e457","label":"measures","sourceLayer":"motivation"},{"id":"c459","source":"e465","target":"e445","label":"measures","sourceLayer":"motivation"},{"id":"c446","source":"e457","target":"e437","label":"realizes","sourceLayer":"motivation"}]}</div>

---

*Generated: 2026-06-29 18:57:22*