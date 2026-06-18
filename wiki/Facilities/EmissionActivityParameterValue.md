# EmissionActivityParameterValue

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time. This entity provides the time-series data that feeds into emission calculation models, capturing how activity data values evolve across reporting periods. The datetime attribute records the precise moment the value was measured or estimated, enabling time-series analysis and period-level aggregation in inventory calculations.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the EmissionActivityParameterValue record. It is the primary key and the anchor for a single activity data measurement in the time series. |
| value | String |  | The numeric value of the emission activity parameter at the recorded point in time, expressed in the unit of measure referenced by the parent EmissionActivityParameter. |
| datetime | String |  | The date and time at which this parameter value was recorded or estimated, in ISO 8601 format. This timestamp is the primary temporal key for the time-series record. |
| emission_activity_id | String |  | A foreign key identifying the EmissionActivity with which this parameter value is associated, supporting direct querying of all parameter values for a given activity. |
| emission_activity_parameter_id | String |  | A foreign key identifying the EmissionActivityParameter definition to which this time-series value belongs, linking the measured value to its parameter type, unit, facility, and activity context. |
| emission_parameter_type_id | String |  | A foreign key referencing the EmissionParameterType of the parameter being recorded, enabling direct filtering of parameter values by type. |
| equipment_id | String |  | An optional foreign key identifying the Equipment item from which this parameter value was measured, when the measurement is equipment-specific. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 779 → 763 |
| Association |  | 762 → 763 |

---

*Generated: 2026-06-18 13:54:45*