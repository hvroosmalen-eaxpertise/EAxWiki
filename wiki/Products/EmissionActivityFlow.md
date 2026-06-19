# EmissionActivityFlow

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process. It enables the model to disaggregate a product carbon footprint into its contributing emission activities, providing the activity-level transparency required for hotspot analysis, supplier engagement, and data quality improvement roadmaps.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityFlow record, used to navigate between the product life-cycle domain and the emission activity domain when performing stage-level attribution analysis. |
| product_life_cycle_id | String |  | Foreign key to the ProductLifeCycle that this flow belongs to, placing the emission activity within the context of a defined system boundary. |
| emission_activity_id | String |  | Foreign key to the EmissionActivity whose GHG contribution to the product life cycle is represented by this flow record. |
| flow_quantity | String |  | The quantity of the activity that occurs within this life-cycle stage per declared unit of the product, expressed in the activity parameter unit, used as the input to the corresponding emission calculation. |
| flow_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the flow_quantity is expressed, such as kWh of electricity consumed or km of transport per declared unit, enabling calculation engines to apply the correct emission factor. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 813 → 814 |
| Association |  | 773 → 814 |

---

*Generated: 2026-06-19 12:59:14*