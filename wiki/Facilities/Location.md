# Location

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

Location represents a place where a person or thing is located. It can describe geospatial aspects such as latitude/longitude coordinates, geopolitical concepts like a country, or a business area as defined by the organisation. The Location data object allows for a parent/child hierarchy with a potentially unlimited number of levels, and is classified by a FacilityLocationType into one of three subtypes: Geospatial Location, Geopolitical Entity, or Business Area. An effective_datetime attribute records when the location record became valid, supporting historical tracking of geographic assignments.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Location record. It is the primary key referenced by Address and by other Location records through the parent_location_id self-referential attribute. It must be globally unique and stable to support reliable geographic lookups and hierarchical traversal. |
| iso_alpha_3_code | String |  | The three-letter ISO 3166-1 alpha-3 country code associated with this location, such as "BEL" for Belgium or "NLD" for the Netherlands. This attribute enables unambiguous identification of the country context for geographic analysis and regulatory jurisdiction mapping. |
| parent_location_id | String |  | A foreign key referencing the parent Location record in the geographic hierarchy, enabling multi-level hierarchies such as Americas to United States to Texas to Site A. Implementations must enforce acyclicity on this self-referential relationship. |
| facility_location_type_id | String |  | A foreign key referencing the FacilityLocationType that classifies this location record as a Geospatial Location, Geopolitical Entity, or Business Area. The type determines how the location is interpreted and which subtype entity provides additional attributes. |
| effective_datetime | String |  | The date and time from which this location record is valid, expressed in ISO 8601 format. The effective_datetime supports the tracking of geographic boundary changes or re-assignments of facilities to new location records over time, preserving a historical record of geographic attribution. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Location represents a place where a person or thing is located. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [Location](Location.md) |
| Association |  | [BusinessArea](BusinessArea.md) |
| Association |  | [GeospatialLocation](GeospatialLocation.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [Location](Location.md) |
| Association |  | [BusinessArea](BusinessArea.md) |
| Association |  | [GeospatialLocation](GeospatialLocation.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [BusinessArea](BusinessArea.md) |
| Association |  | [GeospatialLocation](GeospatialLocation.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |

---

*Generated: 2026-06-24 10:33:17*