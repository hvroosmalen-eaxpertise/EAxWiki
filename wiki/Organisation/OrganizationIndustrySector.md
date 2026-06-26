# <span class="sl" data-layer="uml">master-data</span> OrganizationIndustrySector

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType. An organisation may operate across multiple industry sectors, so this entity allows multiple sector classifications to be recorded without modifying the Organisation entity itself. It supports sector-level benchmarking and the selection of sector-appropriate emission factors, and enables regulatory groupings that aggregate organisations by industry for sector-level disclosure obligations.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationIndustrySector record. It is the primary key for this intersection and must remain stable across sector reclassifications to preserve the history of sector assignments. |
| industry_type_id | String |  | A foreign key referencing the IndustrySectorType into which the organisation is classified. This attribute links the organisation to the standardised sector vocabulary and enables automated selection of sector-appropriate emission factors. |
| organization_id | String |  | A foreign key identifying the Organisation being classified into the referenced industry sector. An organisation may have multiple OrganizationIndustrySector records if it operates across several sectors. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e739","label":"IndustrySectorType","fullName":"IndustrySectorType","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"IndustrySectorType.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","isFocal":true,"hasUrl":false,"url":""},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c900","source":"e739","target":"e739","label":"Association"},{"id":"c902","source":"e739","target":"e750","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:39*