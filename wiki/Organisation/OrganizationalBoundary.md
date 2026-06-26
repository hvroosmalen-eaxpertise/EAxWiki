# <span class="sl" data-layer="uml">master-data</span> OrganizationalBoundary

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationalBoundary defines the scope boundary used to determine which emission sources are included in an organisation's GHG inventory. The GHG Protocol specifies three common approaches: equity share, financial control, and operational control. A boundary record links a named boundary definition to the organisation it applies to, enabling an organisation to maintain multiple concurrent boundary definitions for different reporting frameworks or stakeholder requirements. The boundary concept is foundational to GHG accounting because it determines which emission activities are attributed to the reporting organisation.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationalBoundary record. It is the primary key referenced by EmissionActivity to associate each activity with the boundary under which it is reported. It must be unique and stable across changes to the boundary's name or associated organisation. |
| name | String |  | The descriptive name of the organisational boundary approach, such as "Operational Control", "Financial Control", or "Equity Share 50%". The name conveys the boundary-setting methodology applied and is displayed in inventory reports and disclosure documents to indicate the scope of the reported emissions data. |
| organization_id | String |  | A foreign key identifying the Organisation for which this boundary is defined. An organisation may have multiple boundary records reflecting different consolidation approaches or different reporting frameworks, all linked through this attribute. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationalBoundary defines the scope boundary used to determine which emission sources are included in an organisation's GHG inventory. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Standard](Standard.md) |
| Association |  | [Organization](Organization.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](Standard.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"Standard.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","isFocal":true,"hasUrl":false,"url":""},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionComponentPerStandard.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatementPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/StandardSourceAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationModel.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationType.html"}],"edges":[{"id":"c843","source":"e789","target":"e734","label":"Association"},{"id":"c845","source":"e788","target":"e734","label":"Association"},{"id":"c847","source":"e787","target":"e734","label":"Association"},{"id":"c850","source":"e794","target":"e734","label":"Association"},{"id":"c894","source":"e734","target":"e782","label":"Association"},{"id":"c897","source":"e734","target":"e740","label":"Association"},{"id":"c916","source":"e734","target":"e777","label":"Association"},{"id":"c918","source":"e734","target":"e771","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"}]};</script>

---

*Generated: 2026-06-26 15:09:22*