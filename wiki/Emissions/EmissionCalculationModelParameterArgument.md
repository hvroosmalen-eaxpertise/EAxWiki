# EmissionCalculationModelParameterArgument

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationModelParameterArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the EmissionParameterType whose recorded values are used as the activity data input for that argument during calculation. This entity complements EmissionCalculationModelFactorArgument by declaring the activity-data inputs alongside the factor inputs, giving a complete specification of what a calculation model needs to execute.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this parameter argument record, used alongside factor argument records to provide a complete machine-readable specification of a calculation model inputs. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel that declares this parameter argument, grouping all required activity-data inputs for the model. |
| argument_name | String |  | The named slot identifier within the calculation model, such as fuel_quantity_consumed or vehicle_distance_km, matching the variable name in the formula expression. |
| emission_parameter_type_id | String |  | Foreign key to the EmissionParameterType whose recorded values are used for this argument, linking the model input specification to the data entry requirements imposed on facility operators. |
| description | String |  | A description of the role of this parameter argument in the calculation, specifying the expected measurement approach and any unit conversion requirements before the parameter value is applied. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationModelParameterArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the EmissionParameterType whose recorded values are used as the activity data input for that argument during calculation. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 805 → 775 |
| Association |  | 805 → 777 |

---

*Generated: 2026-06-19 11:58:40*