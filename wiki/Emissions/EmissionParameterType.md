# EmissionParameterType

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionParameterType is a reference entity that defines a named, typed slot for measurement or operational data used in emission calculation. Examples include fuel quantity consumed (MWh), vehicle distance travelled (km), and refrigerant charge (kg). Each EmissionActivityParameter value recorded against a Facility references an EmissionParameterType to establish what was measured and in what unit, enabling calculation engines to apply the correct emission factor formula.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionParameterType record, referenced by EmissionActivityParameter records and by EmissionCalculationModel argument mappings to identify the correct input slot. |
| name | String |  | The human-readable name of the parameter type, such as Natural Gas Consumption or Electricity Purchased, used in data entry forms and calculation model configuration. |
| description | String |  | A description of what this parameter measures, how it should be obtained, and any measurement or estimation guidance relevant to its use in GHG calculations. |
| data_type | String |  | The expected data type of values recorded against this parameter type, such as Decimal, Integer, or Boolean, used for validation during data entry. |
| unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure record that expresses the default measurement unit for values of this parameter type, such as MWh, km, or kg. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionParameterType is a reference entity that defines a named, typed slot for measurement or operational data used in emission calculation. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionActivityParameter](../Facilities/EmissionActivityParameter.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |

---

*Generated: 2026-06-22 21:50:29*