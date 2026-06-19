# EmissionActivityParameter

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations. It links a Facility and optionally an Equipment item to an EmissionParameterType and to the EmissionActivity it monitors, providing the structural metadata that describes what is being measured or estimated for a given activity. Parameter values over time are recorded in the associated EmissionActivityParameterValue entity.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the EmissionActivityParameter record. It is the primary key referenced by EmissionActivityParameterValue to associate time-series values with this parameter definition. |
| name | String |  | The descriptive name of this specific parameter instance, such as "Tower A Gas Flow Rate" or "Boiler 3 Natural Gas Consumption", distinguishing this parameter instance from other parameter definitions on the same facility or activity. |
| equipment_id | String |  | An optional foreign key identifying the Equipment item to which this parameter applies, enabling asset-level activity data tracking when the parameter is equipment-specific rather than facility-level. |
| facility_id | String |  | A foreign key identifying the Facility to which this emission activity parameter is assigned, linking the parameter definition to the physical site context. |
| emission_activity_id | String |  | A foreign key identifying the EmissionActivity that this parameter monitors, linking the activity data definition to the specific emission-generating or emission-removing process. |
| emission_parameter_type | String |  | A foreign key referencing the EmissionParameterType that defines the kind of parameter being monitored, such as "Energy Quantity", "Material Consumption Rate", or "Product Yield". |
| unit_of_measure_id | String |  | A foreign key referencing the UnitOfMeasure applicable to values recorded for this parameter, such as cubic metres per hour or megawatt-hours. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 775 → 762 |
| Association |  | 762 → 763 |
| Association |  | 753 → 762 |

---

*Generated: 2026-06-19 11:58:40*