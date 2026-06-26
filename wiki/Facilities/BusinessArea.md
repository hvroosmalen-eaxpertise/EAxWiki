# <span class="sl" data-layer="uml">master-data</span> BusinessArea

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

BusinessArea represents a location that is identified and defined by an organisation in which business is done by that organisation. It can be used to refer to the location of an office, a plant, a port, or any other operationally significant geographic area defined from the organisation's operational perspective. Unlike formal geopolitical entities, a business area is a master-data concept owned by the reporting organisation. Temporal validity attributes allow the business area definition to be tracked over time as the organisation's operational structure evolves.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the BusinessArea record. It serves as both the primary key and as the foreign key value for a corresponding Location record. |
| name | String |  | The name of the business area as used operationally by the organisation, such as "EMEA Manufacturing Hub" or "Rotterdam Port Operations". The name should be stable enough to support historical reporting. |
| effective_datetime | String |  | The date and time from which this business area record is valid, in ISO 8601 format. Enables temporal tracking of business area definitions as the organisation's operational structure evolves. |
| termination_datetime | String |  | The date and time at which this business area record is terminated, in ISO 8601 format. Null if the business area is currently active. |
| location_id | String |  | A foreign key identifying the corresponding Location record in the geographic hierarchy for this business area. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | BusinessArea represents a location that is identified and defined by an organisation in which business is done by that organisation. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Location](Location.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e755","label":"Location","fullName":"Location","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e767","label":"BusinessArea","fullName":"BusinessArea","packageName":Facilities,"isFocal":true,"hasUrl":false,"url":""},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e768","label":"GeospatialLocation","fullName":"GeospatialLocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"GeospatialLocation.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"GeopoliticalEntity.html"},{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationType.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association"},{"id":"c881","source":"e755","target":"e755","label":"Association"},{"id":"c882","source":"e767","target":"e755","label":"Association"},{"id":"c883","source":"e768","target":"e755","label":"Association"},{"id":"c886","source":"e766","target":"e755","label":"Association"},{"id":"c887","source":"e769","target":"e755","label":"Association"},{"id":"c888","source":"e755","target":"e758","label":"Association"},{"id":"c884","source":"e766","target":"e766","label":"Association"},{"id":"c872","source":"e769","target":"e758","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:44:31*