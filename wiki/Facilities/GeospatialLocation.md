---
ea_id: 768
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: eec547e4
---

# <span class="sl" data-layer="uml">master-data</span> GeospatialLocation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Location&quot;,&quot;fullName&quot;:&quot;Location&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Location.html&quot;},{&quot;id&quot;:&quot;e768&quot;,&quot;label&quot;:&quot;GeospatialLocation&quot;,&quot;fullName&quot;:&quot;GeospatialLocation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e767&quot;,&quot;label&quot;:&quot;BusinessArea&quot;,&quot;fullName&quot;:&quot;BusinessArea&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;BusinessArea.html&quot;},{&quot;id&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;GeopoliticalEntity&quot;,&quot;fullName&quot;:&quot;GeopoliticalEntity&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;GeopoliticalEntity.html&quot;},{&quot;id&quot;:&quot;e769&quot;,&quot;label&quot;:&quot;FacilityLocationType&quot;,&quot;fullName&quot;:&quot;FacilityLocationType&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationType.html&quot;},{&quot;id&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;FacilityLocationAssocia…&quot;,&quot;fullName&quot;:&quot;FacilityLocationAssociation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationAssociation.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c792&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c881&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c882&quot;,&quot;source&quot;:&quot;e767&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c883&quot;,&quot;source&quot;:&quot;e768&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c886&quot;,&quot;source&quot;:&quot;e766&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c887&quot;,&quot;source&quot;:&quot;e769&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c888&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c884&quot;,&quot;source&quot;:&quot;e766&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c872&quot;,&quot;source&quot;:&quot;e769&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*