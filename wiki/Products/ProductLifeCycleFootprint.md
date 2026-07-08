---
ea_id: 817
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 83d335c4
---

# <span class="sl" data-layer="uml">work-product-component</span> ProductLifeCycleFootprint

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint. By disaggregating the PCF total into stage-level contributions, this entity enables hotspot analysis, targeted supplier engagement, and the stage-by-stage breakdowns required by ISO 14067 and increasingly expected by frameworks such as ESRS E1 Appendix A.

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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e816&quot;,&quot;label&quot;:&quot;ProductLifeCycleStage&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleStage&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycleStage.html&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;ProductLifeCycleFootpri…&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Location&quot;,&quot;fullName&quot;:&quot;Location&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/Location.html&quot;},{&quot;id&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;ProductFootprintDataQua…&quot;,&quot;fullName&quot;:&quot;ProductFootprintDataQualityIndicator&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprintDataQualityIndicator.html&quot;},{&quot;id&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;ProductCategoryRule&quot;,&quot;fullName&quot;:&quot;ProductCategoryRule&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCategoryRule.html&quot;},{&quot;id&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;ProductCarbonFootprintF…&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprintFactorSource&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprintFactorSource.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/UnitOfMeasure.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c793&quot;,&quot;source&quot;:&quot;e816&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c802&quot;,&quot;source&quot;:&quot;e816&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c792&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c795&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c796&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c798&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c803&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c804&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c881&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*