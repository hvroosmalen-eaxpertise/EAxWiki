# ActivityEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source. This entity is used, for example, when a shared boiler serves multiple processes and its total emissions must be apportioned across each process emission activity record in proportion to energy consumed, production output, or another allocation base.

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

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

---

*Generated: 2026-06-24 10:33:17*