# EmissionCalculationMethodType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationMethodType is a reference entity that classifies the high-level methodological approach applied in an EmissionCalculationModel. The WBCSD PACT technical specification defines four primary method types: supplier-specific, hybrid, activity-based, and spend-based, each with different data quality implications and applicability conditions. Registering these types as reference data enables transparent method disclosure and drives data quality scoring in product carbon footprint assessments.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionCalculationMethodType record, used in EmissionCalculationModel records and method disclosure fields of ProductCarbonFootprint. |
| name | String |  | The standard label for the method type, such as Activity-based, Spend-based, Supplier-specific, or Hybrid, as defined in the GHG Protocol or PACT technical specification. |
| description | String |  | A description of the calculation approach, its data requirements, typical use cases, and the data quality tier it represents relative to other method types. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationMethodType is a reference entity that classifies the high-level methodological approach applied in an EmissionCalculationModel. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 791 → 777 |

---

*Generated: 2026-06-19 11:58:40*