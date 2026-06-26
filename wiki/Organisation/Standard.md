# <span class="sl" data-layer="uml">master-data</span> Standard

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions. Examples include the GHG Protocol Corporate Standard, ISO 14064, and TCFD recommendations. Each Standard provides a named reference that can be cited in emission inventories or organisational boundary definitions, ensuring traceability between reported data and the methodology used to produce it. The Standard entity also carries a descriptive text and a URL so that consumers of the data can navigate directly to the authoritative source of the referenced specification.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Standard record. This is the primary key used to reference the standard from related entities such as OrganizationalBoundary and EmissionInventory. It must be stable across updates and must not be reused once assigned. |
| name | String |  | The human-readable official name of the standard or protocol, for example "GHG Protocol Corporate Standard Edition 2015". The name should be rendered exactly as published by the issuing body so that cross-referencing with external catalogues is unambiguous. It is displayed in reports and selection lists throughout user interfaces. |
| description | String |  | A free-text narrative providing additional context about the scope, applicability, and key requirements of the standard. The description may include version notes, jurisdictional applicability, or relationships to other standards. It is intended to help analysts determine whether this standard is appropriate for a given emission inventory or boundary definition. |
| url_description | String |  | A resolvable URL pointing to the official publication, web page, or registry entry for the standard. The URL enables automated or manual retrieval of the full text and supporting documentation. It should be maintained and verified periodically to ensure it remains active and points to the current authoritative version. |
| organization_id | String |  | A foreign key reference to the Organisation that governs or owns this standard. An organisation may define its own internal standards or act as the custodian of an industry standard, and this attribute records that governing relationship for traceability purposes. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentPerStandard](../Emissions/EmissionComponentPerStandard.md) |
| Association |  | [EmissionStatementPerStandard](../Emissions/EmissionStatementPerStandard.md) |
| Association |  | [EmissionCategoryStandardAssociation](../Emissions/EmissionCategoryStandardAssociation.md) |
| Association |  | [StandardSourceAssociation](../Emissions/StandardSourceAssociation.md) |
| Association |  | [EmissionReport](../Emissions/EmissionReport.md) |
| Association |  | [OrganizationalBoundary](OrganizationalBoundary.md) |
| Association |  | [EmissionCalculationModel](../Emissions/EmissionCalculationModel.md) |
| Association |  | [EmissionInventory](../Emissions/EmissionInventory.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentPerStandard](../Emissions/EmissionComponentPerStandard.md) |
| Association |  | [EmissionStatementPerStandard](../Emissions/EmissionStatementPerStandard.md) |
| Association |  | [EmissionCategoryStandardAssociation](../Emissions/EmissionCategoryStandardAssociation.md) |
| Association |  | [StandardSourceAssociation](../Emissions/StandardSourceAssociation.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionComponentPerStandard.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatementPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/StandardSourceAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationModel.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityCategory.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionFactorSource.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReportPeriod.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationModelParameterArgument.html"},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationModelFactorArgument.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityFactor.html"},{"id":"e791","label":"EmissionCalculationMeth…","fullName":"EmissionCalculationMethodType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationMethodType.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationFormula.html"},{"id":"e808","label":"EmissionReportingBounda…","fullName":"EmissionReportingBoundary","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReportingBoundary.html"}],"edges":[{"id":"c843","source":"e789","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c844","source":"e789","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c845","source":"e788","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c846","source":"e788","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c847","source":"e787","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c848","source":"e787","target":"e774","label":"Association","sourceLayer":"uml"},{"id":"c806","source":"e781","target":"e794","label":"Association","sourceLayer":"uml"},{"id":"c849","source":"e794","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c850","source":"e794","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c837","source":"e782","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c897","source":"e734","target":"e740","label":"Association","sourceLayer":"uml"},{"id":"c898","source":"e740","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c816","source":"e805","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c818","source":"e804","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c851","source":"e793","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c858","source":"e791","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c859","source":"e777","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c916","source":"e734","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c810","source":"e808","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-26 17:14:27*