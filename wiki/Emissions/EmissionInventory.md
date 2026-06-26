# <span class="sl" data-layer="uml">master-data</span> EmissionInventory

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period. It groups all EmissionStatement records that together constitute a complete inventory of an organisation's greenhouse gas emissions, organised by scope, source, and boundary. The inventory record holds the metadata required for formal reporting, including the applicable standard, the organisational boundary method, and audit trail information.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system identifier for this EmissionInventory record, used to associate all child emission statements and reporting artefacts with this specific inventory exercise. |
| organisation_id | String |  | Foreign key linking the inventory to the Organisation that owns and is responsible for the GHG data it contains. |
| standard_id | String |  | Foreign key to the Standard record that governs the methodology and boundary rules applied in this inventory, typically the GHG Protocol Corporate Standard or ISO 14064-1. |
| name | String |  | A human-readable label for the inventory, such as "FY2025 GHG Inventory Scope 1 and 2", used for identification and search in reporting systems. |
| description | String |  | A free-text narrative describing the scope, boundaries, and methodology choices made for this inventory, providing context for reviewers and auditors. |
| reporting_period_start | String |  | The first day of the reporting period covered by this inventory. Together with reporting_period_end it defines the temporal boundary against which emission activities are assessed. |
| reporting_period_end | String |  | The last day of the reporting period covered by this inventory. Emission activities with dates outside this window are excluded from the inventory totals. |
| organisational_boundary_method | String |  | The consolidation approach used to determine which entities and facilities fall within the inventory boundary, drawn from the GHG Protocol values: equity share, financial control, or operational control. |
| status | String |  | The lifecycle status of the inventory record, such as draft, under review, finalised, or audited, used to manage approval workflows and publication control. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionReport](EmissionReport.md) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [Standard](../Organisation/Standard.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [EmissionReport](EmissionReport.md) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e808","label":"EmissionReportingBounda…","fullName":"EmissionReportingBoundary","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionReportingBoundary.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","isFocal":true,"hasUrl":false,"url":""},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationType.html"},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"}],"edges":[{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c837","source":"e782","target":"e783","label":"Association"},{"id":"c894","source":"e734","target":"e782","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c810","source":"e808","target":"e771","label":"Association"},{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c826","source":"e800","target":"e776","label":"Association"},{"id":"c827","source":"e799","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c869","source":"e772","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c843","source":"e789","target":"e734","label":"Association"},{"id":"c845","source":"e788","target":"e734","label":"Association"},{"id":"c847","source":"e787","target":"e734","label":"Association"},{"id":"c850","source":"e794","target":"e734","label":"Association"},{"id":"c897","source":"e734","target":"e740","label":"Association"},{"id":"c916","source":"e734","target":"e777","label":"Association"},{"id":"c918","source":"e734","target":"e771","label":"Association"},{"id":"c807","source":"e779","target":"e783","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c873","source":"e753","target":"e764","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c844","source":"e789","target":"e778","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"}]};</script>

---

*Generated: 2026-06-26 15:09:23*