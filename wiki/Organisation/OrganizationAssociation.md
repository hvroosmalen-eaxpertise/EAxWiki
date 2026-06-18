# OrganizationAssociation

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association. It records which organisation is the parent and which is the child in the association, and is classified by an OrganizationAssociationType that describes the nature of the relationship. This entity enables the modelling of complex corporate structures without embedding hierarchy information directly in the Organization entity, and supports the non-hierarchical many-to-many relationships (e.g., Auditor, Affiliate) that are common in emissions reporting ecosystems.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAssociation record. It is the primary key used to identify and reference this inter-organisational relationship. It must be globally unique and stable for as long as the association is in force. |
| organization_id | String |  | A foreign key identifying the parent Organisation in this association — the organisation that is the subject of the relationship, for example the parent company in a subsidiary relationship or the engaging organisation in a verifier relationship. |
| associated_organization_id | String |  | A foreign key identifying the associated (child or counterpart) Organisation in this relationship. For example, in a subsidiary association this would be the subsidiary organisation, and in a verifier association this would be the verifying body. |
| organization_association_type_id | String |  | A foreign key referencing the OrganizationAssociationType that classifies the nature of this inter-organisational relationship. The type controls how the association is interpreted in boundary consolidation calculations and data exchange scenarios. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 742 → 741 |
| Association |  | 735 → 741 |

---

*Generated: 2026-06-18 12:23:55*