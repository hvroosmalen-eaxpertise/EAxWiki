---
ea_id: 784
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 096fdc3a
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionSource

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="784" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionSource.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="784" data-file-path="Emissions/EmissionSource.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. Emission sources provide a more granular technical classification than the EmissionActivityType and are used in calculation model selection and emission factor lookup to narrow the applicable factor set to the correct physical process.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionSource record, used by EmissionActivity and EmissionFactor records to associate a specific physical source with a measurement or factor entry. |
| name | String |  | The human-readable name of the emission source, such as Stationary Combustion Natural Gas or Fugitive Emissions HFC-134a refrigerant, used in data entry forms and technical reports. |
| description | String |  | A technical description of the emission mechanism and the physical or chemical process by which the GHG is released, supporting correct classification and factor selection by practitioners. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e784","label":"EmissionSource","fullName":"EmissionSource","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/EmissionActivityFlow.html"},{"id":"e785","label":"EmissionSink","fullName":"EmissionSink","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionSink.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityActivityParticipation.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityCategory.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c822","source":"e802","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c836","source":"e773","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c838","source":"e785","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c839","source":"e784","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c842","source":"e773","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c861","source":"e774","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c862","source":"e786","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c870","source":"e773","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c823","source":"e802","target":"e776","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*