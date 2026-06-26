# <span class="sl" data-layer="uml">work-product-component</span> EmissionReport

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public. It carries the report metadata, including the applicable reporting framework, the consolidation boundary, and the sign-off status. The entity acts as an envelope that groups EmissionReportPeriod records and drives the generation of summary tables and narrative sections in published disclosures.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionReport record, used to link all period detail records, supporting documents, and approval workflow steps to this specific published artefact. |
| organisation_id | String |  | Foreign key to the Organisation that is the reporting entity, identifying whose emissions are being disclosed in this report. |
| standard_id | String |  | Foreign key to the Standard or reporting framework under which this report is prepared, such as GHG Protocol, ESRS E1, or CDP Climate Change, governing the structure and content requirements. |
| name | String |  | The human-readable title of the report, used for filing and archive identification. |
| status | String |  | The approval and publication status of the report, such as draft, under assurance, board approved, or published, supporting governance workflow tracking. |
| publication_date | String |  | The date on which the report was formally published or filed with the relevant authority, establishing the official record date for regulatory and audit purposes. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionInventory](EmissionInventory.md) |
| Association |  | [EmissionReportPeriod](EmissionReportPeriod.md) |
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [Organization](../Organisation/Organization.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [Organization](../Organisation/Organization.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","isFocal":true,"hasUrl":false,"url":""},{"id":"e808","label":"EmissionReportingBounda…","fullName":"EmissionReportingBoundary","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionReportingBoundary.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationType.html"}],"edges":[{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c810","source":"e808","target":"e771","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c918","source":"e734","target":"e771","label":"Association"},{"id":"c807","source":"e779","target":"e783","label":"Association"},{"id":"c837","source":"e782","target":"e783","label":"Association"},{"id":"c843","source":"e789","target":"e734","label":"Association"},{"id":"c845","source":"e788","target":"e734","label":"Association"},{"id":"c847","source":"e787","target":"e734","label":"Association"},{"id":"c850","source":"e794","target":"e734","label":"Association"},{"id":"c894","source":"e734","target":"e782","label":"Association"},{"id":"c897","source":"e734","target":"e740","label":"Association"},{"id":"c916","source":"e734","target":"e777","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:40*