---
ea_id: 789
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionComponentPerStandard

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponentPerStandard is an intersection entity analogous to EmissionStatementPerStandard but at the component (individual gas) level, recording the gas-specific emission quantity as it must be disclosed under a particular standard. Different standards apply different global-warming-potential assessment reports (AR4, AR5, AR6) which change the CO2-equivalent values of non-CO2 gases, meaning the same physical emission quantity may produce different tCO2e results across frameworks.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this per-standard component result record. |
| emission_component_id | String |  | Foreign key to the parent EmissionComponent from which this framework-adjusted quantity is derived. |
| standard_id | String |  | Foreign key to the Standard under which this component quantity is reported and to which the applicable GWP values belong. |
| quantity | String |  | The gas-specific emission quantity as adjusted for the GWP factors mandated by the referenced standard, enabling cross-standard comparison of individual gas contributions. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which this per-standard component quantity is expressed, typically tCO2e using the GWP factor set of the referenced standard. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponentPerStandard is an intersection entity analogous to EmissionStatementPerStandard but at the component (individual gas) level, recording the gas-specific emission quantity as it must be disclosed under a particular standard. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [EmissionComponent](EmissionComponent.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c843","source":"e789","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c845","source":"e788","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c847","source":"e787","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c850","source":"e794","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c897","source":"e734","target":"e740","label":"Association","sourceLayer":"uml"},{"id":"c916","source":"e734","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c844","source":"e789","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c866","source":"e792","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c846","source":"e788","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 14:47:48*