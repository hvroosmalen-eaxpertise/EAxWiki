# OrganizationIndustrySector

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType. An organisation may operate across multiple industry sectors, so this entity allows multiple sector classifications to be recorded without modifying the Organisation entity itself. It supports sector-level benchmarking and the selection of sector-appropriate emission factors, and enables regulatory groupings that aggregate organisations by industry for sector-level disclosure obligations.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationIndustrySector record. It is the primary key for this intersection and must remain stable across sector reclassifications to preserve the history of sector assignments. |
| industry_type_id | String |  | A foreign key referencing the IndustrySectorType into which the organisation is classified. This attribute links the organisation to the standardised sector vocabulary and enables automated selection of sector-appropriate emission factors. |
| organization_id | String |  | A foreign key identifying the Organisation being classified into the referenced industry sector. An organisation may have multiple OrganizationIndustrySector records if it operates across several sectors. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

---

*Generated: 2026-06-24 14:21:20*