---
ea_id: 768
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">master-data</span> GeospatialLocation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

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

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | GeospatialLocation provides a spatial representation of a Location, defined by coordinate values and an EPSG coordinate reference system code. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Location](Location.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e768","label":"GeospatialLocation","fullName":"GeospatialLocation","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e767","label":"BusinessArea","fullName":"BusinessArea","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"BusinessArea.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"GeopoliticalEntity.html"},{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationType.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c882","source":"e767","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c883","source":"e768","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c886","source":"e766","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c887","source":"e769","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c888","source":"e755","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c872","source":"e769","target":"e758","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 14:47:48*