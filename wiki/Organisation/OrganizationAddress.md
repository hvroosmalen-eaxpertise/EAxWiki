# <span class="sl" data-layer="uml">MAS</span> OrganizationAddress

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAddress is the intersection entity that associates an Organisation with a physical or postal Address at a specific address type (e.g., visiting, correspondence, statutory). It acts as a bridge between the Organisation, the Address, and the OrganizationAddressType, allowing an organisation to maintain multiple categorised addresses simultaneously without ambiguity. This design supports the full range of address types required by legal, regulatory, and operational contexts.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAddress record. It is the primary key for this intersection and must remain stable so that address assignments can be audited over time. |
| address_id | String |  | A foreign key referencing the Address record assigned to this organisation. This links the organisation to the actual postal or physical address details stored in the Address entity. |
| organization_address_type_id | String |  | A foreign key referencing the OrganizationAddressType that classifies this address assignment, for example "Visiting Address" or "Statutory Address". This attribute enables consuming systems to select the appropriate address for a given operational purpose. |
| organization_id | String |  | A foreign key identifying the Organisation to which this address is assigned. An organisation may have multiple OrganizationAddress records, each with a different type, all linked through this attribute. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAddress is the intersection entity that associates an Organisation with a physical or postal Address at a specific address type (e.g., visiting, correspondence, statutory). |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationAddressType](OrganizationAddressType.md) |
| Association |  | [Address](Address.md) |
| Association |  | [Organization](Organization.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.md) |
| Association |  | [Address](Address.md) |
| Association |  | [OrganizationAddressType](OrganizationAddressType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e744","label":"OrganizationAddressType","fullName":"OrganizationAddressType","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationAddressType.html"},{"id":"e737","label":"Address","fullName":"Address","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Address.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":Organisation,"isFocal":true,"hasUrl":false,"url":""},{"id":"e745","label":"Country","fullName":"Country","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Country.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c910","source":"e744","target":"e743","label":"Association"},{"id":"c909","source":"e745","target":"e737","label":"Association"},{"id":"c911","source":"e737","target":"e743","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*