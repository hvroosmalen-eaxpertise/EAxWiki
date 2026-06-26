# <span class="sl" data-layer="uml">master-data</span> Location

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

Location represents a place where a person or thing is located. It can describe geospatial aspects such as latitude/longitude coordinates, geopolitical concepts like a country, or a business area as defined by the organisation. The Location data object allows for a parent/child hierarchy with a potentially unlimited number of levels, and is classified by a FacilityLocationType into one of three subtypes: Geospatial Location, Geopolitical Entity, or Business Area. An effective_datetime attribute records when the location record became valid, supporting historical tracking of geographic assignments.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Location record. It is the primary key referenced by Address and by other Location records through the parent_location_id self-referential attribute. It must be globally unique and stable to support reliable geographic lookups and hierarchical traversal. |
| iso_alpha_3_code | String |  | The three-letter ISO 3166-1 alpha-3 country code associated with this location, such as "BEL" for Belgium or "NLD" for the Netherlands. This attribute enables unambiguous identification of the country context for geographic analysis and regulatory jurisdiction mapping. |
| parent_location_id | String |  | A foreign key referencing the parent Location record in the geographic hierarchy, enabling multi-level hierarchies such as Americas to United States to Texas to Site A. Implementations must enforce acyclicity on this self-referential relationship. |
| facility_location_type_id | String |  | A foreign key referencing the FacilityLocationType that classifies this location record as a Geospatial Location, Geopolitical Entity, or Business Area. The type determines how the location is interpreted and which subtype entity provides additional attributes. |
| effective_datetime | String |  | The date and time from which this location record is valid, expressed in ISO 8601 format. The effective_datetime supports the tracking of geographic boundary changes or re-assignments of facilities to new location records over time, preserving a historical record of geographic attribution. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Location represents a place where a person or thing is located. |  |

[↑ Back to top](#)

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

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [Location](Location.md) |
| Association |  | [BusinessArea](BusinessArea.md) |
| Association |  | [GeospatialLocation](GeospatialLocation.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e767","label":"BusinessArea","fullName":"BusinessArea","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"BusinessArea.html"},{"id":"e768","label":"GeospatialLocation","fullName":"GeospatialLocation","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"GeospatialLocation.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"GeopoliticalEntity.html"},{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"FacilityLocationType.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","isFocal":true,"hasUrl":false,"url":""},{"id":"e821","label":"ProductFootprintDataQua…","fullName":"ProductFootprintDataQualityIndicator","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductFootprintDataQualityIndicator.html"},{"id":"e820","label":"ProductCategoryRule","fullName":"ProductCategoryRule","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductCategoryRule.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprintFactorSource.html"},{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductLifeCycleFootprint.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductFootprint.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityParameterRecordingTemplate.html"},{"id":"e765","label":"GeopoliticalEntityType","fullName":"GeopoliticalEntityType","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"GeopoliticalEntityType.html"},{"id":"e745","label":"Country","fullName":"Country","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/Country.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"Facility.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association"},{"id":"c795","source":"e812","target":"e821","label":"Association"},{"id":"c796","source":"e812","target":"e820","label":"Association"},{"id":"c798","source":"e812","target":"e818","label":"Association"},{"id":"c803","source":"e812","target":"e817","label":"Association"},{"id":"c804","source":"e811","target":"e812","label":"Association"},{"id":"c833","source":"e779","target":"e812","label":"Association"},{"id":"c882","source":"e767","target":"e755","label":"Association"},{"id":"c883","source":"e768","target":"e755","label":"Association"},{"id":"c813","source":"e806","target":"e766","label":"Association"},{"id":"c884","source":"e766","target":"e766","label":"Association"},{"id":"c885","source":"e765","target":"e766","label":"Association"},{"id":"c886","source":"e766","target":"e755","label":"Association"},{"id":"c893","source":"e745","target":"e766","label":"Association"},{"id":"c872","source":"e769","target":"e758","label":"Association"},{"id":"c887","source":"e769","target":"e755","label":"Association"},{"id":"c888","source":"e755","target":"e758","label":"Association"},{"id":"c889","source":"e753","target":"e758","label":"Association"},{"id":"c881","source":"e755","target":"e755","label":"Association"},{"id":"c794","source":"e811","target":"e811","label":"Association"}]};</script>

---

*Generated: 2026-06-26 15:09:23*