# <span class="sl" data-layer="uml">WOR</span> FacilityLocationAssociation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time. Because facilities can be relocated or their location assignment can change over time, this entity carries effective and termination datetimes to capture the full history of facility-location associations. The FacilityLocationAssociation can be used to determine the geographic location that a physical emission record belongs to, and is particularly important for mobile facilities such as fleets of shipping tankers that operate across multiple locations in sequence.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityLocationAssociation record. It must remain stable for the full period of the assignment to support geographic attribution of historical emission records. |
| location_id | String |  | A foreign key identifying the Location to which the facility is assigned during the validity period. |
| facility_id | String |  | A foreign key identifying the Facility that is assigned to the referenced location during this association period. |
| effective_datetime | String |  | The date and time from which the facility is assigned to the referenced location, in ISO 8601 format. Together with termination_datetime, this defines the precise temporal window of the geographic assignment. |
| termination_datetime | String |  | The date and time at which the facility-location assignment ends, in ISO 8601 format. Null indicates the assignment is currently active. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityLocationType](FacilityLocationType.md) |
| Association |  | [Location](Location.md) |
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Location](Location.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationType.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":Facilities,"isFocal":true,"hasUrl":false,"url":""},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e767","label":"BusinessArea","fullName":"BusinessArea","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"BusinessArea.html"},{"id":"e768","label":"GeospatialLocation","fullName":"GeospatialLocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"GeospatialLocation.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"GeopoliticalEntity.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityEmissionAllocation.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c872","source":"e769","target":"e758","label":"Association"},{"id":"c887","source":"e769","target":"e755","label":"Association"},{"id":"c792","source":"e812","target":"e755","label":"Association"},{"id":"c881","source":"e755","target":"e755","label":"Association"},{"id":"c882","source":"e767","target":"e755","label":"Association"},{"id":"c883","source":"e768","target":"e755","label":"Association"},{"id":"c886","source":"e766","target":"e755","label":"Association"},{"id":"c888","source":"e755","target":"e758","label":"Association"},{"id":"c873","source":"e753","target":"e764","label":"Association"},{"id":"c875","source":"e753","target":"e762","label":"Association"},{"id":"c877","source":"e753","target":"e761","label":"Association"},{"id":"c879","source":"e753","target":"e760","label":"Association"},{"id":"c880","source":"e753","target":"e759","label":"Association"},{"id":"c889","source":"e753","target":"e758","label":"Association"},{"id":"c891","source":"e753","target":"e757","label":"Association"},{"id":"c892","source":"e753","target":"e754","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c884","source":"e766","target":"e766","label":"Association"},{"id":"c890","source":"e757","target":"e757","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*