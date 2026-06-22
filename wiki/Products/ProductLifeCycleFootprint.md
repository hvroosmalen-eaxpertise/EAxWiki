# ProductLifeCycleFootprint

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint. By disaggregating the PCF total into stage-level contributions, this entity enables hotspot analysis, targeted supplier engagement, and the stage-by-stage breakdowns required by ISO 14067 and increasingly expected by frameworks such as ESRS E1 Appendix A.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycleFootprint record, used to aggregate stage contributions and verify that they sum to the total PCF for the declared unit and system boundary. |
| product_carbon_footprint_id | String |  | Foreign key to the parent ProductCarbonFootprint that this stage-level contribution disaggregates, linking the stage result to its total PCF context. |
| product_life_cycle_stage_id | String |  | Foreign key to the ProductLifeCycleStage that this record covers, identifying which phase of the product life cycle the emission quantity is attributed to. |
| quantity | String |  | The GHG emission quantity attributable to this life-cycle stage per declared unit, expressed in the unit referenced by quantity_unit_of_measure_id, typically kgCO2e. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the stage emission quantity is expressed, enabling correct aggregation with other stage records and comparison with the PCF total. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.md) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.md) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

---

*Generated: 2026-06-22 17:27:40*