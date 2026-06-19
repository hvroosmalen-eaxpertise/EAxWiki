# PersonOrganizationRoleType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

PersonOrganizationRoleType provides the controlled vocabulary of roles that a person can play within an organisation, such as "Primary Contact", "Secondary Contact", "Sustainability Director", "CEO", or "Plant Manager". Classifying person–organisation relationships by role type enables structured retrieval of the appropriate contact for a given operational context, such as emissions reporting queries or audit communications. The role vocabulary may be extended by individual implementations to meet sector-specific requirements.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the PersonOrganizationRoleType record. It must be globally unique and stable so that existing OrganizationPersonAssociation records remain valid when the role vocabulary is extended. |
| name | String |  | The descriptive label for the role type, such as "Primary Contact", "Chief Executive Officer", or "Sustainability Director". The name should reflect the functional responsibility of the person in the context of emissions reporting and should be drawn from a controlled list agreed across the reporting ecosystem. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | PersonOrganizationRoleType provides the controlled vocabulary of roles that a person can play within an organisation, such as "Primary Contact", "Secondary Contact", "Sustainability Director", "CEO", or "Plant Manager". |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 746 → 747 |

---

*Generated: 2026-06-19 11:58:39*