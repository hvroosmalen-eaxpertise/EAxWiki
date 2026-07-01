---
ea_id: 762
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7adb7be2
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityParameter

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="762" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="762" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations. It links a Facility and optionally an Equipment item to an EmissionParameterType and to the EmissionActivity it monitors, providing the structural metadata that describes what is being measured or estimated for a given activity. Parameter values over time are recorded in the associated EmissionActivityParameterValue entity.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the EmissionActivityParameter record. It is the primary key referenced by EmissionActivityParameterValue to associate time-series values with this parameter definition. |
| name | String |  | The descriptive name of this specific parameter instance, such as "Tower A Gas Flow Rate" or "Boiler 3 Natural Gas Consumption", distinguishing this parameter instance from other parameter definitions on the same facility or activity. |
| equipment_id | String |  | An optional foreign key identifying the Equipment item to which this parameter applies, enabling asset-level activity data tracking when the parameter is equipment-specific rather than facility-level. |
| facility_id | String |  | A foreign key identifying the Facility to which this emission activity parameter is assigned, linking the parameter definition to the physical site context. |
| emission_activity_id | String |  | A foreign key identifying the EmissionActivity that this parameter monitors, linking the activity data definition to the specific emission-generating or emission-removing process. |
| emission_parameter_type | String |  | A foreign key referencing the EmissionParameterType that defines the kind of parameter being monitored, such as "Energy Quantity", "Material Consumption Rate", or "Product Yield". |
| unit_of_measure_id | String |  | A foreign key referencing the UnitOfMeasure applicable to values recorded for this parameter, such as cubic metres per hour or megawatt-hours. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](../Emissions/EmissionParameterType.md) |
| Association |  | [EmissionActivityParameterValue](EmissionActivityParameterValue.md) |
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [EmissionParameterType](../Emissions/EmissionParameterType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionParameterType.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameterValue.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityTypeParameterTypeAssignment.html"},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationModelParameterArgument.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationFormulaComponent.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityEmissionAllocation.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c811","source":"e807","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c815","source":"e805","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c841","source":"e775","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c834","source":"e779","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c874","source":"e762","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c873","source":"e753","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c875","source":"e753","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c877","source":"e753","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c879","source":"e753","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c880","source":"e753","target":"e759","label":"Association","sourceLayer":"uml"},{"id":"c889","source":"e753","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c891","source":"e753","target":"e757","label":"Association","sourceLayer":"uml"},{"id":"c892","source":"e753","target":"e754","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c890","source":"e757","target":"e757","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*