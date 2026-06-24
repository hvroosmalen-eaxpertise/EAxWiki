# GeospatialLocation

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

GeospatialLocation provides a spatial representation of a Location, defined by coordinate values and an EPSG coordinate reference system code. It can represent a point on the earth such as a latitude/longitude coordinate pair, or may capture the polygon outline of a location. Both original and normalised coordinate values are stored to preserve the source data while enabling consistent geographic comparison. Geospatial information may be either master data specific to a company location or reference data obtained from a third-party dataset.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the GeospatialLocation record. This serves as both the primary key and as the foreign key value for the corresponding Location record. |
| epsg_code | String |  | An EPSG numerical identifier used in geospatial data to reference a specific coordinate reference system, providing a standardised way to represent and work with location-based information. For example, EPSG 4326 refers to the WGS84 coordinate system used in GPS. |
| normalized_x | String |  | The normalised X (longitude or easting) coordinate value transformed to a standard coordinate system for consistent geographic comparison and display. |
| normalized_y | String |  | The normalised Y (latitude or northing) coordinate value transformed to a standard coordinate system for consistent geographic comparison and display. |
| original_x | String |  | The raw X (longitude or easting) coordinate value for this geospatial location in the coordinate reference system identified by the epsg_code attribute, stored as received from the source dataset. |
| original_y | String |  | The raw Y (latitude or northing) coordinate value for this geospatial location in the coordinate reference system identified by the epsg_code attribute, stored as received from the source dataset. |
| effective_datetime | String |  | The date and time from which this geospatial location record is valid, in ISO 8601 format. |
| termination_datetime | String |  | The date and time at which this geospatial location record is terminated, in ISO 8601 format. Null if the record is currently active. |
| location_id | String |  | A foreign key linking this GeospatialLocation to its parent Location record. |
| parent_location_id | String |  | A foreign key referencing the parent Location record in the geographic hierarchy for this geospatial location. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | GeospatialLocation provides a spatial representation of a Location, defined by coordinate values and an EPSG coordinate reference system code. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Location](Location.md) |

---

*Generated: 2026-06-24 14:21:20*