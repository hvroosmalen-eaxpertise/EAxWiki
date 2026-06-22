# EmissionActivityFactor

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions. The association may be scoped by geography, time period, or calculation model, allowing the model to represent the context-dependency of factor applicability without embedding applicability rules inside the factor record itself. This pattern supports rule-based factor selection in calculation engines.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityFactor association record, used in calculation audit trails to identify exactly which factor was selected for which activity type in a given context. |
| emission_activity_type_id | String |  | Foreign key to the EmissionActivityType for which this factor is applicable, defining the activity class to which the factor should be applied. |
| emission_factor_id | String |  | Foreign key to the EmissionFactor that provides the coefficient for this activity type in this context, establishing the specific numeric value and its unit. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel under which this activity-to-factor mapping is valid, ensuring that factors are only selected within the model that defines their applicability context. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionActivityType](EmissionActivityType.md) |

---

*Generated: 2026-06-22 17:43:22*