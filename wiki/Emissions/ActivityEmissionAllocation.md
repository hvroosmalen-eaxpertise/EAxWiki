---
ea_id: 802
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: ac1cc17a
---

# <span class="sl" data-layer="uml">work-product-component</span> ActivityEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="802" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="802" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source. This entity is used, for example, when a shared boiler serves multiple processes and its total emissions must be apportioned across each process emission activity record in proportion to energy consumed, production output, or another allocation base.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ActivityEmissionAllocation record, used to verify that all allocations from a shared source sum to the total source emission. |
| emission_statement_id | String |  | Foreign key to the source EmissionStatement whose total emission quantity is being allocated across activities. |
| emission_activity_id | String |  | Foreign key to the EmissionActivity receiving the allocated portion, identifying which activity-level record absorbs this share. |
| allocation_percentage | String |  | The percentage share of the source emission quantity assigned to this activity, determined by the allocation base. |
| allocated_quantity | String |  | The absolute emission quantity allocated to this activity, equal to the source statement quantity multiplied by the allocation_percentage, for direct use in activity-level totals. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure for the allocated_quantity, ensuring unit consistency when activity-level allocations are summed. |
| allocation_base | String |  | A description of the physical or economic basis used to determine the allocation percentages, such as proportion of energy consumed by each process or floor area fraction. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/EmissionActivityFlow.html"},{"id":"e785","label":"EmissionSink","fullName":"EmissionSink","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionSink.html"},{"id":"e784","label":"EmissionSource","fullName":"EmissionSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionSource.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityActivityParticipation.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityCategory.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"}],"edges":[{"id":"c822","source":"e802","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c836","source":"e773","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c838","source":"e785","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c839","source":"e784","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c842","source":"e773","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c861","source":"e774","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c862","source":"e786","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c870","source":"e773","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c823","source":"e802","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c825","source":"e801","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c826","source":"e800","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c827","source":"e799","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c840","source":"e776","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c846","source":"e788","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c869","source":"e772","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c860","source":"e772","target":"e774","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*