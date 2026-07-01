---
ea_id: 766
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: e81450d9
---

# <span class="sl" data-layer="uml">master-data</span> GeopoliticalEntity

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="766" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="766" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces. It is classified by a GeopoliticalEntityType and optionally linked to a Country. Examples include "USA", "Canada", "United Kingdom", "Yellow Stone National Park", "Alberta", and "North Sea". Geopolitical entities provide the administrative geographic context required for regulatory jurisdiction mapping, reporting by jurisdiction, and alignment with country-level emission factor datasets.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the GeopoliticalEntity record. This serves as both the primary key of the entity and as the foreign key value linking to a Location record. |
| name | String |  | The official name of the geopolitical entity as it is defined and administered by the relevant authority, such as "Flanders", "Bavaria", or "Yellow Stone National Park". |
| geopolitical_entity_type_id | String |  | A foreign key referencing the GeopoliticalEntityType that classifies this entity, such as "Country", "State", or "Nature Reserve". |
| parent_location_id | String |  | A foreign key referencing the parent Location record in the geographic hierarchy, for example linking a State to its Country parent. |
| effective_datetime | String |  | The date and time from which this geopolitical entity record is valid, in ISO 8601 format. Supports tracking of boundary changes or the creation of new entities over time. |
| termination_datetime | String |  | The date and time at which this geopolitical entity record is terminated, in ISO 8601 format. Null if the entity is currently active. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityParameterRecordingTemplate](../Emissions/EmissionActivityParameterRecordingTemplate.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [GeopoliticalEntityType](GeopoliticalEntityType.md) |
| Association |  | [Location](Location.md) |
| Association |  | [Country](../Organisation/Country.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Country](../Organisation/Country.md) |
| Association |  | [GeopoliticalEntityType](GeopoliticalEntityType.md) |
| Association |  | [EmissionActivityParameterRecordingTemplate](../Emissions/EmissionActivityParameterRecordingTemplate.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityParameterRecordingTemplate.html"},{"id":"e765","label":"GeopoliticalEntityType","fullName":"GeopoliticalEntityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"GeopoliticalEntityType.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e745","label":"Country","fullName":"Country","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Country.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityTypeParameterTypeAssignment.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityType.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e767","label":"BusinessArea","fullName":"BusinessArea","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"BusinessArea.html"},{"id":"e768","label":"GeospatialLocation","fullName":"GeospatialLocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"GeospatialLocation.html"},{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationType.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e737","label":"Address","fullName":"Address","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Address.html"}],"edges":[{"id":"c812","source":"e807","target":"e806","label":"Association","sourceLayer":"uml"},{"id":"c813","source":"e806","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c814","source":"e806","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c885","source":"e765","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c792","source":"e812","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c882","source":"e767","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c883","source":"e768","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c886","source":"e766","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c887","source":"e769","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c888","source":"e755","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c893","source":"e745","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c909","source":"e745","target":"e737","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c872","source":"e769","target":"e758","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*