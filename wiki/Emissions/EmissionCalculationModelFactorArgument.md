# EmissionCalculationModelFactorArgument

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationModelFactorArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the specific EmissionFactor that should be used to fill that slot during calculation. A model may reference multiple factors; for example, a Scope 2 market-based model may require both an electricity emission factor and a transmission-and-distribution loss factor. Defining factor arguments as data rather than code enables model configuration without software deployment and supports systematic factor update workflows.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this factor argument record, used to enumerate all factor inputs required by a calculation model and to audit which factor was in place during a given reporting period. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel that declares this factor argument, grouping all required factor inputs for a model in one place. |
| argument_name | String |  | The named slot identifier within the calculation model, such as electricity_emission_factor or td_loss_factor, matching the variable name used in the formula expression. |
| emission_factor_id | String |  | Foreign key to the EmissionFactor record bound to this argument slot, providing the specific coefficient the model uses for calculation. |
| description | String |  | A description of the role of this factor argument in the calculation, clarifying what emission source or process it represents and why this particular factor was selected. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationModelFactorArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the specific EmissionFactor that should be used to fill that slot during calculation. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |

---

*Generated: 2026-06-25 10:51:16*