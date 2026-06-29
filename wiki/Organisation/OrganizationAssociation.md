# <span class="sl" data-layer="uml">master-data</span> OrganizationAssociation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association. It records which organisation is the parent and which is the child in the association, and is classified by an OrganizationAssociationType that describes the nature of the relationship. This entity enables the modelling of complex corporate structures without embedding hierarchy information directly in the Organization entity, and supports the non-hierarchical many-to-many relationships (e.g., Auditor, Affiliate) that are common in emissions reporting ecosystems.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAssociation record. It is the primary key used to identify and reference this inter-organisational relationship. It must be globally unique and stable for as long as the association is in force. |
| organization_id | String |  | A foreign key identifying the parent Organisation in this association — the organisation that is the subject of the relationship, for example the parent company in a subsidiary relationship or the engaging organisation in a verifier relationship. |
| associated_organization_id | String |  | A foreign key identifying the associated (child or counterpart) Organisation in this relationship. For example, in a subsidiary association this would be the subsidiary organisation, and in a verifier association this would be the verifying body. |
| organization_association_type_id | String |  | A foreign key referencing the OrganizationAssociationType that classifies the nature of this inter-organisational relationship. The type controls how the association is interpreted in boundary consolidation calculations and data exchange scenarios. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationAssociationType](OrganizationAssociationType.md) |
| Association |  | [Organization](Organization.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb diagram-thumb--noimg"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.md) |
| Association |  | [OrganizationAssociationType](OrganizationAssociationType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e742","label":"OrganizationAssociation…","fullName":"OrganizationAssociationType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAssociationType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c913","source":"e742","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c824","source":"e801","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c898","source":"e740","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c899","source":"e735","target":"e751","label":"Association","sourceLayer":"uml"},{"id":"c901","source":"e735","target":"e750","label":"Association","sourceLayer":"uml"},{"id":"c904","source":"e735","target":"e748","label":"Association","sourceLayer":"uml"},{"id":"c906","source":"e747","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c908","source":"e738","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c912","source":"e735","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c914","source":"e735","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c915","source":"e735","target":"e736","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c905","source":"e747","target":"e738","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 18:57:23*