---
ea_id: 817
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 83d335c4
---

# <span class="sl" data-layer="uml">work-product-component</span> ProductLifeCycleFootprint

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="817" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="817" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint. By disaggregating the PCF total into stage-level contributions, this entity enables hotspot analysis, targeted supplier engagement, and the stage-by-stage breakdowns required by ISO 14067 and increasingly expected by frameworks such as ESRS E1 Appendix A.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycleFootprint record, used to aggregate stage contributions and verify that they sum to the total PCF for the declared unit and system boundary. |
| product_carbon_footprint_id | String |  | Foreign key to the parent ProductCarbonFootprint that this stage-level contribution disaggregates, linking the stage result to its total PCF context. |
| product_life_cycle_stage_id | String |  | Foreign key to the ProductLifeCycleStage that this record covers, identifying which phase of the product life cycle the emission quantity is attributed to. |
| quantity | String |  | The GHG emission quantity attributable to this life-cycle stage per declared unit, expressed in the unit referenced by quantity_unit_of_measure_id, typically kgCO2e. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the stage emission quantity is expressed, enabling correct aggregation with other stage records and comparison with the PCF total. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.md) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.md) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.md) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e816","label":"ProductLifeCycleStage","fullName":"ProductLifeCycleStage","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycleStage.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"},{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e821","label":"ProductFootprintDataQua…","fullName":"ProductFootprintDataQualityIndicator","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprintDataQualityIndicator.html"},{"id":"e820","label":"ProductCategoryRule","fullName":"ProductCategoryRule","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCategoryRule.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprintFactorSource.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"}],"edges":[{"id":"c793","source":"e816","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c802","source":"e816","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c792","source":"e812","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c795","source":"e812","target":"e821","label":"Association","sourceLayer":"uml"},{"id":"c796","source":"e812","target":"e820","label":"Association","sourceLayer":"uml"},{"id":"c798","source":"e812","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c803","source":"e812","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c804","source":"e811","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c794","source":"e811","target":"e811","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*