# OrganizationType

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationType provides a controlled vocabulary of organisational classifications, such as "Headquarters", "Regional Headquarters", or "Branch". Maintaining a separate entity for organisation types rather than free-text values ensures consistency across the dataset and enables structured filtering and aggregation in reporting. The type classification is relevant for determining applicable regulatory obligations, disclosure requirements, and boundary-setting methodologies. Each OrganizationType may apply to many organisations simultaneously.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationType record. It is the primary key used when associating an Organization with its type classification. It must be stable and not recycled once assigned. |
| name | String |  | The descriptive label for the organisation type, such as "Headquarters", "Regional Headquarters", or "Branch". The name should be drawn from a recognised taxonomy to ensure comparability across organisations and should be rendered consistently in all user-facing interfaces and reports. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationType provides a controlled vocabulary of organisational classifications, such as "Headquarters", "Regional Headquarters", or "Branch". |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](Organization.md) |

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.md) |
| Association |  | [Organization](Organization.md) |

---

*Generated: 2026-06-24 14:21:20*