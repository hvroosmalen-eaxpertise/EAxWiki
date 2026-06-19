# Standard

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions. Examples include the GHG Protocol Corporate Standard, ISO 14064, and TCFD recommendations. Each Standard provides a named reference that can be cited in emission inventories or organisational boundary definitions, ensuring traceability between reported data and the methodology used to produce it. The Standard entity also carries a descriptive text and a URL so that consumers of the data can navigate directly to the authoritative source of the referenced specification.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Standard record. This is the primary key used to reference the standard from related entities such as OrganizationalBoundary and EmissionInventory. It must be stable across updates and must not be reused once assigned. |
| name | String |  | The human-readable official name of the standard or protocol, for example "GHG Protocol Corporate Standard Edition 2015". The name should be rendered exactly as published by the issuing body so that cross-referencing with external catalogues is unambiguous. It is displayed in reports and selection lists throughout user interfaces. |
| description | String |  | A free-text narrative providing additional context about the scope, applicability, and key requirements of the standard. The description may include version notes, jurisdictional applicability, or relationships to other standards. It is intended to help analysts determine whether this standard is appropriate for a given emission inventory or boundary definition. |
| url_description | String |  | A resolvable URL pointing to the official publication, web page, or registry entry for the standard. The URL enables automated or manual retrieval of the full text and supporting documentation. It should be maintained and verified periodically to ensure it remains active and points to the current authoritative version. |
| organization_id | String |  | A foreign key reference to the Organisation that governs or owns this standard. An organisation may define its own internal standards or act as the custodian of an industry standard, and this attribute records that governing relationship for traceability purposes. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 789 → 734 |
| Association |  | 788 → 734 |
| Association |  | 787 → 734 |
| Association |  | 794 → 734 |
| Association |  | 734 → 782 |
| Association |  | 734 → 740 |
| Association |  | 734 → 777 |
| Association |  | 734 → 771 |

---

*Generated: 2026-06-19 11:58:39*