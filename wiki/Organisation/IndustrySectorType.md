# IndustrySectorType

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection. Common sector classification systems include NACE (European), SIC (US), NAICS (North American), and ISIC (International). A self-referential parent relationship allows the construction of multi-level sector hierarchies, enabling both high-level sector summaries and granular sub-sector analysis. The code attribute supports direct alignment with external classification system codes, enabling automated crosswalk with reference datasets.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the IndustrySectorType record. It serves as the primary key and is also referenced in the self-referential parent association to build sector hierarchies. It must be globally unique across all sector classification records. |
| code | String |  | The standardised industry sector classification code as defined by the relevant classification system (e.g., NAICS code "325" for "Chemical Manufacturing" or "32511" for "Petrochemical Manufacturing"). The code enables automated alignment with external databases, emission factor repositories, and regulatory reporting schemas that reference industry sectors by code. |
| name | String |  | The human-readable name of the industry sector, such as "Oil and Gas" or "Iron and Steel". The name provides a user-friendly label for display in reports and selection interfaces and should match the official label used by the classification system referenced by the code attribute. |
| parent_id | String |  | A foreign key referencing the parent IndustrySectorType record in the sector hierarchy, enabling multi-level classifications such as Chemical Manufacturing (325) as the parent of Petrochemical Manufacturing (32511). This attribute must not create cycles; implementations should enforce acyclicity. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [IndustrySectorType](IndustrySectorType.md) |
| Association |  | [OrganizationIndustrySector](OrganizationIndustrySector.md) |

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

---

*Generated: 2026-06-22 17:43:22*