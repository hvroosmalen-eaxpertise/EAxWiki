# ProductLifeCycle

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. It groups the individual ProductLifeCycleStage records that enumerate each stage (raw material extraction, manufacturing, distribution, use, end-of-life) and anchors the ProductLifeCycleFootprint records that hold stage-level emission quantities. Defining the life cycle as a structured entity rather than a string field enables stage-level comparisons across products and supply-chain tiers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycle record, referenced by ProductLifeCycleStage and ProductLifeCycleFootprint records to associate stage definitions with their footprint contributions. |
| product_id | String |  | Foreign key to the Product whose life-cycle system boundary this record defines, linking the life-cycle structure to the product master record. |
| name | String |  | A descriptive label for this life-cycle scope definition, such as Cradle-to-gate, Cradle-to-grave, or Gate-to-gate, indicating the extent of the boundary and enabling high-level comparison across products. |
| description | String |  | A narrative description of the system boundary, listing the included and excluded life-cycle stages, the functional unit, and any allocation decisions that apply to the life cycle as a whole. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityFlow](EmissionActivityFlow.md) |
| Association |  | [Product](Product.md) |

---

*Generated: 2026-06-22 17:27:40*