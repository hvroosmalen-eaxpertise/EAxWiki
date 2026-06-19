# FacilityType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityType provides a controlled vocabulary of facility classifications, sourced from references such as ISO 15926-4 for non-moving oil and gas facilities and the US EPA Subpart W classification for petroleum and natural gas systems. Examples include "Compressor Station", "Gas Plant", "Offshore Production", "Natural Gas Processing", and "LNG storage". Classifying facilities by type enables sector-appropriate emissions factor selection, benchmarking against industry peers, and consistent aggregation of site-level data in portfolio analyses.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityType record. It serves as the primary key used when classifying a Facility with its operational type. It must be unique across all facility type records and must not change once assigned. |
| code | String |  | The standardised classification code for the facility type, aligned with an external taxonomy such as ISO 15926-4 or EPA Subpart W where applicable. The code supports automated crosswalk with emissions factor databases and regulatory reporting schemas that classify facilities by type using structured codes. |
| name | String |  | The human-readable label for the facility type, such as "Gas Plant" or "LNG Storage". The name is displayed in facility lists, dashboards, and reports, and should be drawn from a recognised taxonomy to ensure consistent interpretation across the organisation and its reporting partners. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityType provides a controlled vocabulary of facility classifications, sourced from references such as ISO 15926-4 for non-moving oil and gas facilities and the US EPA Subpart W classification for petroleum and natural gas systems. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 753 → 754 |

---

*Generated: 2026-06-19 13:04:05*