# FacilityActivityParticipation

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity. This intersection entity supports cases where a single emission activity spans multiple facilities, or where multiple emission activities are associated with the same facility, enabling accurate physical attribution of GHG-generating processes to the sites at which they occur.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityActivityParticipation record. It is the primary key for this participation link and must remain stable for reporting history and audit purposes. |
| emission_activity_id | String |  | A foreign key identifying the EmissionActivity in which the referenced facility participates, linking the physical facility to the specific emission-generating or emission-removing process. |
| facility_id | String |  | A foreign key identifying the Facility that participates in the referenced emission activity, linking the physical site to the activity for geographic attribution and site-level reporting. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 773 → 760 |
| Association |  | 753 → 760 |

---

*Generated: 2026-06-18 12:23:55*