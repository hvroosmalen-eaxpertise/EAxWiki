# FacilitySpecification

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilitySpecification is the work-product entity that assigns a specific value to an EmissionParameterType for a given Facility through a FacilityStructure link. It enables flexible, configurable specification of facility-level parameters such as energy efficiency ratings, capacity figures, temperature operating ranges, or process-specific constants without requiring schema extensions. The name-value pair is typed by the EmissionParameterType, which defines the dimension and expected units of the value.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilitySpecification record. It is the primary key for this parameter assignment and must remain stable for reporting and audit purposes. |
| name | String |  | The descriptive name of the parameter assignment for this facility, such as "Unit AB Temperature" or "Annual Installed Capacity". This name distinguishes this specification record from other parameter assignments on the same facility. |
| value | String |  | The value of the specified parameter, stored as a string to accommodate numeric, coded, and free-text parameter values uniformly. Numeric values should carry units as defined in the associated EmissionParameterType record. |
| emission_parameter_type_id | String |  | A foreign key referencing the EmissionParameterType that defines what kind of parameter is being specified, such as "Temperature", "Energy Efficiency Rating", or "Product Yield". |
| facility_id | String |  | A foreign key identifying the Facility to which this specification applies. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilitySpecification is the work-product entity that assigns a specific value to an EmissionParameterType for a given Facility through a FacilityStructure link. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Facility](Facility.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Facility](Facility.md) |

---

*Generated: 2026-06-22 17:43:22*