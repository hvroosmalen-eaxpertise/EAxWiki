---
ea_id: 806
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 28439fce
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityParameterRecordingTemplate

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="806" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context. It acts as a data-collection template that tells facility operators which parameters they must record for each activity type, ensuring that all inputs needed by the applicable calculation models are systematically collected. Templates may be jurisdiction-specific to reflect local regulatory data requirements.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this recording template record, referenced by EmissionActivityTypeParameterTypeAssignment records that enumerate the individual parameter slots making up this template. |
| emission_activity_type_id | String |  | Foreign key to the EmissionActivityType for which this template defines the required parameter measurements, linking the template to the activity classification it serves. |
| geopolitical_entity_id | String |  | Foreign key to the GeopoliticalEntity (typically a country or regulatory jurisdiction) for which this template applies, enabling jurisdiction-specific parameter requirements. |
| name | String |  | A human-readable label for the template, such as UK Stationary Combustion Natural Gas Tier 2 Parameters, used in data collection system configuration and operator guidance. |
| description | String |  | A description of the template purpose, the regulatory or methodological rationale for the parameter requirements, and any conditions under which this template is mandatory or optional. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |
| Association |  | [GeopoliticalEntity](../Facilities/GeopoliticalEntity.md) |
| Association |  | [EmissionActivityType](EmissionActivityType.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityTypeParameterTypeAssignment.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/GeopoliticalEntity.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e765","label":"GeopoliticalEntityType","fullName":"GeopoliticalEntityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/GeopoliticalEntityType.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e745","label":"Country","fullName":"Country","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Country.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"}],"edges":[{"id":"c811","source":"e807","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c812","source":"e807","target":"e806","label":"Association","sourceLayer":"uml"},{"id":"c813","source":"e806","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c885","source":"e765","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c886","source":"e766","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c893","source":"e745","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c814","source":"e806","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c853","source":"e793","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c862","source":"e786","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 09:47:24*