---
ea_id: 807
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 76ee9f40
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityTypeParameterTypeAssignment

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="807" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="807" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement is mandatory or optional. Each template is composed of one or more of these assignment records, forming the complete parameter checklist for the associated activity type and jurisdiction.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this parameter type assignment record, used to enumerate the complete list of required measurements within a recording template. |
| emission_activity_parameter_recording_template_id | String |  | Foreign key to the parent EmissionActivityParameterRecordingTemplate that this assignment belongs to, grouping all parameter requirements for a given activity type and jurisdiction. |
| emission_parameter_type_id | String |  | Foreign key to the EmissionParameterType that must be recorded, identifying the specific measurement slot that this assignment adds to the template. |
| is_mandatory | String |  | Indicates whether measurement of this parameter type is required (true) or optional (false) for the activity type and jurisdiction defined by the parent template, supporting data completeness validation. |
| sequence_number | String |  | The display order of this parameter assignment within the template, used to present data entry fields in a logical sequence to facility operators. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement  |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameterRecordingTemplate.html"},{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelParameterArgument.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameter.html"},{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/GeopoliticalEntity.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"}],"edges":[{"id":"c811","source":"e807","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c815","source":"e805","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c841","source":"e775","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c812","source":"e807","target":"e806","label":"Association","sourceLayer":"uml"},{"id":"c813","source":"e806","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c814","source":"e806","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 12:05:10*