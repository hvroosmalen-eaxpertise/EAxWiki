# EmissionSource

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. Emission sources provide a more granular technical classification than the EmissionActivityType and are used in calculation model selection and emission factor lookup to narrow the applicable factor set to the correct physical process.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionSource record, used by EmissionActivity and EmissionFactor records to associate a specific physical source with a measurement or factor entry. |
| name | String |  | The human-readable name of the emission source, such as Stationary Combustion Natural Gas or Fugitive Emissions HFC-134a refrigerant, used in data entry forms and technical reports. |
| description | String |  | A technical description of the emission mechanism and the physical or chemical process by which the GHG is released, supporting correct classification and factor selection by practitioners. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.md) |

---

*Generated: 2026-06-25 10:51:16*