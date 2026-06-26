# <span class="sl" data-layer="uml">MAS</span> OrganizationPersonAssociation

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType. This design allows one person to hold multiple roles across one or more organisations without duplicating contact person records. It is the mechanism through which an organisation's contact directory is maintained for emissions reporting, audit communication, and regulatory correspondence.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationPersonAssociation record. It is the primary key for this intersection and must remain stable for audit and correspondence history purposes. |
| organization_id | String |  | A foreign key identifying the Organisation to which the contact person is associated in this role. An organisation may have many OrganizationPersonAssociation records covering different roles and different persons. |
| person_id | String |  | A foreign key identifying the ContactPerson who holds the specified role in the referenced organisation. A single person may appear in multiple association records across different organisations or different roles within the same organisation. |
| person_organization_role_type_id | String |  | A foreign key referencing the PersonOrganizationRoleType that describes the specific role played by the contact person in the referenced organisation, such as "Primary Contact" or "Plant Manager". |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ContactPerson](ContactPerson.md) |
| Association |  | [Organization](Organization.md) |
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e746","label":"PersonOrganizationRoleT…","fullName":"PersonOrganizationRoleType","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"PersonOrganizationRoleType.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":Organisation,"isFocal":true,"hasUrl":false,"url":""},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c905","source":"e747","target":"e738","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c907","source":"e746","target":"e747","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"}]};</script>

---

*Generated: 2026-06-26 09:44:49*