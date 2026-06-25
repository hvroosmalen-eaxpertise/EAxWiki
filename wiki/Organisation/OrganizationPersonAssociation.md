# OrganizationPersonAssociation

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType. This design allows one person to hold multiple roles across one or more organisations without duplicating contact person records. It is the mechanism through which an organisation's contact directory is maintained for emissions reporting, audit communication, and regulatory correspondence.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationPersonAssociation record. It is the primary key for this intersection and must remain stable for audit and correspondence history purposes. |
| organization_id | String |  | A foreign key identifying the Organisation to which the contact person is associated in this role. An organisation may have many OrganizationPersonAssociation records covering different roles and different persons. |
| person_id | String |  | A foreign key identifying the ContactPerson who holds the specified role in the referenced organisation. A single person may appear in multiple association records across different organisations or different roles within the same organisation. |
| person_organization_role_type_id | String |  | A foreign key referencing the PersonOrganizationRoleType that describes the specific role played by the contact person in the referenced organisation, such as "Primary Contact" or "Plant Manager". |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ContactPerson](ContactPerson.md) |
| Association |  | [Organization](Organization.md) |
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.md) |

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.md) |
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.md) |

---

*Generated: 2026-06-25 10:51:16*