---
ea_id: 769
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> FacilityLocationType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location. This type classification drives which subtype entity provides the detailed location attributes for a given Location record and determines how geographic attribution is performed for emission reporting purposes.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityLocationType record. It must be stable so that Location records that reference it remain valid when the type vocabulary is extended. |
| name | String |  | The descriptive label for the facility location type, drawn from the three values defined in the standard: "Business Area", "Geopolitical Entity", and "Geospatial Location". The name determines which location subtype provides the detailed attributes for Location records classified under this type. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.md) |
| Association |  | [Location](Location.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e767","label":"BusinessArea","fullName":"BusinessArea","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"BusinessArea.html"},{"id":"e768","label":"GeospatialLocation","fullName":"GeospatialLocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"GeospatialLocation.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"GeopoliticalEntity.html"}],"edges":[{"id":"c872","source":"e769","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c888","source":"e755","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c889","source":"e753","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c792","source":"e812","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c882","source":"e767","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c883","source":"e768","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c886","source":"e766","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c887","source":"e769","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 15:54:13*