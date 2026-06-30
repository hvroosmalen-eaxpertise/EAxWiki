---
ea_id: 786
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionActivityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory. Examples include Stationary Combustion, Mobile Combustion, Process Emissions, Fugitive Emissions, and Land Use Change. The type is used in calculation model selection and in parameter recording template assignment to determine which measurement parameters are required for a given activity.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityType record, referenced by EmissionActivity records and by EmissionActivityParameterRecordingTemplate assignments to drive parameter requirements. |
| name | String |  | The standard label for the activity type, such as Mobile Combustion Road Transport or Process Emissions Cement Clinker Production, used in model configuration and report category labels. |
| description | String |  | A description of the physical or chemical processes that characterise activities of this type and the typical emission sources and parameters associated with them. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.md) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.md) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameterRecordingTemplate.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityTypeParameterTypeAssignment.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/GeopoliticalEntity.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/EmissionActivityFlow.html"},{"id":"e785","label":"EmissionSink","fullName":"EmissionSink","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionSink.html"},{"id":"e784","label":"EmissionSource","fullName":"EmissionSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionSource.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityActivityParticipation.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityCategory.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c812","source":"e807","target":"e806","label":"Association","sourceLayer":"uml"},{"id":"c813","source":"e806","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c814","source":"e806","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c851","source":"e793","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c853","source":"e793","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c822","source":"e802","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c836","source":"e773","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c838","source":"e785","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c839","source":"e784","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c842","source":"e773","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c861","source":"e774","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c862","source":"e786","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c870","source":"e773","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c823","source":"e802","target":"e776","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 15:54:14*