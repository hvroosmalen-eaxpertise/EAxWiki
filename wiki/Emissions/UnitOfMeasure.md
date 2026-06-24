# UnitOfMeasure

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

UnitOfMeasure is a reference entity that provides the controlled vocabulary of measurement units used throughout the model for quantities, emission factors, parameter values, and product footprint attributes. It supports unit conversion through conversion factor attributes and is linked to a PhysicalQuantityType to enable dimensional analysis. The entity allows the model to be system-of-units-agnostic while maintaining the traceability required for rigorous scientific and regulatory reporting.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this UnitOfMeasure record, typically a UN/CEFACT or QUDT unit code such as MTQ for cubic metres or TNE for metric tonnes, ensuring unambiguous cross-system interoperability. |
| system_of_units_id | String |  | Foreign key to the SystemOfUnits record that this unit belongs to, such as SI, Imperial, or US Customary, enabling validation and conversion path determination. |
| physical_quantity_type_id | String |  | Foreign key to the PhysicalQuantityType that this unit measures, such as Mass, Energy, or Volume, supporting dimensional consistency checks in calculation models. |
| unit_of_measure_source_reference_id | String |  | Foreign key to the UnitOfMeasureSourceReference that is the authoritative registry for this unit definition, such as the UN/CEFACT Common Codes or QUDT ontology. |
| name | String |  | The full human-readable name of the unit, such as Metric Tonne or Kilowatt-hour, used in labels and documentation. |
| symbol | String |  | The standard symbol for the unit, such as t, kWh, or m3, used in quantity displays and report tables. |
| conversion_factor_a | String |  | The multiplicative factor A in the linear conversion formula: target_value = (source_value x A) + B, enabling conversion from this unit to a defined base unit of the same physical quantity. |
| conversion_factor_b | String |  | The additive constant B in the linear conversion formula: target_value = (source_value x A) + B, required for units with a non-zero offset such as degrees Celsius to Kelvin. |
| conversion_factor_c | String |  | An optional third conversion parameter for non-linear or two-stage unit conversions, reserved for future use where the A+B formula is insufficient. |
| conversion_factor_d | String |  | An optional fourth conversion parameter providing additional flexibility for complex unit conversion formulae requiring more than two coefficients. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | UnitOfMeasure is a reference entity that provides the controlled vocabulary of measurement units used throughout the model for quantities, emission factors, parameter values, and product footprint attributes. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionReportPeriod](EmissionReportPeriod.md) |
| Association |  | [UnitOfMeasureSourceReference](UnitOfMeasureSourceReference.md) |
| Association |  | [PhysicalQuantityType](PhysicalQuantityType.md) |
| Association |  | [SystemOfUnits](SystemOfUnits.md) |
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [EmissionActivityParameterValue](../Facilities/EmissionActivityParameterValue.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionComponent](EmissionComponent.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [UnitOfMeasureSourceReference](UnitOfMeasureSourceReference.md) |
| Association |  | [PhysicalQuantityType](PhysicalQuantityType.md) |
| Association |  | [SystemOfUnits](SystemOfUnits.md) |
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [SystemOfUnits](SystemOfUnits.md) |
| Association |  | [PhysicalQuantityType](PhysicalQuantityType.md) |
| Association |  | [UnitOfMeasureSourceReference](UnitOfMeasureSourceReference.md) |

---

*Generated: 2026-06-24 10:33:17*