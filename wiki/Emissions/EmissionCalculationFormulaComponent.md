# EmissionCalculationFormulaComponent

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant. This decomposition enables calculation engines to evaluate formulas programmatically from structured data rather than parsed strings, and supports formula-level audit trails that show exactly which factors and parameters contributed to an emission result.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this formula component record, used by calculation engines to retrieve all terms of a formula in the correct evaluation sequence. |
| emission_calculation_formula_id | String |  | Foreign key to the parent EmissionCalculationFormula that this component belongs to, grouping all terms of a single formula expression. |
| sequence_number | String |  | The position of this term in the formula evaluation sequence, used to enforce a defined multiplication or addition order when the formula has more than one term. |
| component_type | String |  | The role of this term in the formula, drawn from values such as ActivityParameter, EmissionFactor, GWPFactor, or Constant, determining how the calculation engine retrieves the term value. |
| emission_factor_id | String |  | Foreign key to the EmissionFactor record that provides this term value when component_type is EmissionFactor; null for parameter or constant terms. |
| parameter_type_id | String |  | Foreign key to the EmissionParameterType that provides this term value when component_type is ActivityParameter; null for factor or constant terms. |
| constant_value | String |  | The fixed numeric value of this term when component_type is Constant, such as a GWP correction coefficient or unit conversion factor embedded directly in the formula. |
| constant_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure for the constant_value, required when the constant carries a unit that participates in dimensional analysis of the overall formula. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionCalculationFormula](EmissionCalculationFormula.md) |

---

*Generated: 2026-06-22 17:43:22*