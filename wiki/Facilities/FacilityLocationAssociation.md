# FacilityLocationAssociation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time. Because facilities can be relocated or their location assignment can change over time, this entity carries effective and termination datetimes to capture the full history of facility-location associations. The FacilityLocationAssociation can be used to determine the geographic location that a physical emission record belongs to, and is particularly important for mobile facilities such as fleets of shipping tankers that operate across multiple locations in sequence.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityLocationAssociation record. It must remain stable for the full period of the assignment to support geographic attribution of historical emission records. |
| location_id | String |  | A foreign key identifying the Location to which the facility is assigned during the validity period. |
| facility_id | String |  | A foreign key identifying the Facility that is assigned to the referenced location during this association period. |
| effective_datetime | String |  | The date and time from which the facility is assigned to the referenced location, in ISO 8601 format. Together with termination_datetime, this defines the precise temporal window of the geographic assignment. |
| termination_datetime | String |  | The date and time at which the facility-location assignment ends, in ISO 8601 format. Null indicates the assignment is currently active. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityLocationType](FacilityLocationType.md) |
| Association |  | [Location](Location.md) |
| Association |  | [Facility](Facility.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Location](Location.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |
| Association |  | [Location](Location.md) |
| Association |  | [Facility](Facility.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |

---

*Generated: 2026-06-22 22:08:54*