---
ea_id: 735
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">master-data</span> Organization

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

Organization is the central anchor entity for emissions reporting. It represents any legal entity, company, subsidiary, joint venture, or other organisational unit that is subject to emissions accounting obligations or that voluntarily participates in a carbon disclosure programme. The Organization entity links directly to standards, types, contact persons, addresses, and organisational boundaries, forming the root context from which all emission inventories and product footprints are ultimately traceable. An organization may have an external identifier issued by a third-party registry such as GLEIF LEI codes, enabling cross-system reconciliation.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Organisation record. It serves as the primary key referenced by emission inventories, organisational boundaries, and contact persons. It must be immutable once assigned and must be globally unique within the system. |
| name | String |  | The registered legal or trading name of the organisation as it appears in official filings or public disclosures. The name is used in reports, dashboards, and data exchange messages to identify the reporting entity and must correspond to the name used in external registries where applicable. |
| description | String |  | A free-text field providing supplementary information about the organisation, such as its primary business activities, geographic operating regions, or its role within a supply chain. This field supports both analytical queries and human-readable reporting by providing context that is not captured in structured attributes. |
| external_identifier | String |  | An identifier assigned to the organisation by an external system or registry, such as a Legal Entity Identifier (LEI), DUNS number, or national company registration number. The external identifier enables interoperability with external data sources and is used when exchanging emissions data with supply chain partners or regulators who reference the organisation by an external code. |
| organization_type_id | String |  | A foreign key reference to the OrganizationType that classifies this organisation. Storing the type reference on the Organisation entity allows rapid filtering by organisational type without joining through a separate intersection table in the most common query patterns. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Organization is the central anchor entity for emissions reporting. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationEmissionAllocation](../Emissions/OrganizationEmissionAllocation.md) |
| Association |  | [EmissionReport](../Emissions/EmissionReport.md) |
| Association |  | [Facility](../Facilities/Facility.md) |
| Association |  | [OrganizationalBoundary](OrganizationalBoundary.md) |
| Association |  | [OrganizationEquityShare](OrganizationEquityShare.md) |
| Association |  | [OrganizationIndustrySector](OrganizationIndustrySector.md) |
| Association |  | [OrganizationExternalIdentifier](OrganizationExternalIdentifier.md) |
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |
| Association |  | [ContactPerson](ContactPerson.md) |
| Association |  | [OrganizationAddress](OrganizationAddress.md) |
| Association |  | [OrganizationAssociation](OrganizationAssociation.md) |
| Association |  | [OrganizationType](OrganizationType.md) |
| Association |  | [EmissionInventory](../Emissions/EmissionInventory.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [OrganizationEmissionAllocation](../Emissions/OrganizationEmissionAllocation.md) |
| Association |  | [OrganizationalBoundary](OrganizationalBoundary.md) |
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |
| Association |  | [ContactPerson](ContactPerson.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReportPeriod.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Standard.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameter.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EquipmentInstallation.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityLocationAssociation.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityType.html"},{"id":"e739","label":"IndustrySectorType","fullName":"IndustrySectorType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"IndustrySectorType.html"},{"id":"e749","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifierType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifierType.html"},{"id":"e746","label":"PersonOrganizationRoleT…","fullName":"PersonOrganizationRoleType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"PersonOrganizationRoleType.html"},{"id":"e744","label":"OrganizationAddressType","fullName":"OrganizationAddressType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAddressType.html"},{"id":"e737","label":"Address","fullName":"Address","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Address.html"},{"id":"e742","label":"OrganizationAssociation…","fullName":"OrganizationAssociationType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAssociationType.html"},{"id":"e808","label":"EmissionReportingBounda…","fullName":"EmissionReportingBoundary","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReportingBoundary.html"}],"edges":[{"id":"c824","source":"e801","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c825","source":"e801","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c837","source":"e782","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c873","source":"e753","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c875","source":"e753","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c877","source":"e753","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c879","source":"e753","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c880","source":"e753","target":"e759","label":"Association","sourceLayer":"uml"},{"id":"c889","source":"e753","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c891","source":"e753","target":"e757","label":"Association","sourceLayer":"uml"},{"id":"c892","source":"e753","target":"e754","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c897","source":"e734","target":"e740","label":"Association","sourceLayer":"uml"},{"id":"c898","source":"e740","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c899","source":"e735","target":"e751","label":"Association","sourceLayer":"uml"},{"id":"c901","source":"e735","target":"e750","label":"Association","sourceLayer":"uml"},{"id":"c902","source":"e739","target":"e750","label":"Association","sourceLayer":"uml"},{"id":"c903","source":"e749","target":"e748","label":"Association","sourceLayer":"uml"},{"id":"c904","source":"e735","target":"e748","label":"Association","sourceLayer":"uml"},{"id":"c905","source":"e747","target":"e738","label":"Association","sourceLayer":"uml"},{"id":"c906","source":"e747","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c907","source":"e746","target":"e747","label":"Association","sourceLayer":"uml"},{"id":"c908","source":"e738","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c910","source":"e744","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c911","source":"e737","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c912","source":"e735","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c913","source":"e742","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c914","source":"e735","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c915","source":"e735","target":"e736","label":"Association","sourceLayer":"uml"},{"id":"c810","source":"e808","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c840","source":"e776","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c890","source":"e757","target":"e757","label":"Association","sourceLayer":"uml"},{"id":"c900","source":"e739","target":"e739","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 15:54:13*