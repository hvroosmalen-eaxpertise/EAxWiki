# OrganizationEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

OrganizationEmissionAllocation is a work-product-component that records the share of an EmissionStatement total quantity allocated to a specific Organisation when equity-share or financial-control consolidation requires that group emissions be apportioned across multiple legal entities or subsidiaries. The allocation percentage and method are stored alongside the allocated quantity to maintain a complete audit trail from group total to entity share.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this OrganizationEmissionAllocation record, used to aggregate all shares of a given statement and verify that allocations sum to 100% for fully consolidated inventories. |
| emission_statement_id | String |  | Foreign key to the EmissionStatement whose total is being apportioned across organisations by this and other allocation records. |
| organisation_id | String |  | Foreign key to the Organisation receiving the allocated share of the emission quantity, identifying which entity inventory this apportioned amount feeds into. |
| allocation_percentage | String |  | The percentage of the parent statement emission quantity allocated to this organisation, derived from the equity share, financial control ratio, or other allocation rule applied. |
| allocated_quantity | String |  | The absolute emission quantity allocated to this organisation, calculated as the parent statement quantity multiplied by the allocation percentage, for direct use in the organisation inventory totals. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the allocated_quantity is expressed, ensuring consistent unit handling when allocation records are aggregated. |
| allocation_method | String |  | A description of the method used to determine the allocation percentage, such as equity share per shareholder register or financial control 100% consolidation, supporting audit traceability. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationEmissionAllocation is a work-product-component that records the share of an EmissionStatement total quantity allocated to a specific Organisation when equity-share or financial-control consolidation requires that group emissions be apportioned |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

---

*Generated: 2026-06-24 14:21:20*