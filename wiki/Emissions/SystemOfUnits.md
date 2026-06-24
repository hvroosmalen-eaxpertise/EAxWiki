# SystemOfUnits

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

SystemOfUnits is a reference entity that identifies the measurement system to which a UnitOfMeasure belongs, such as the International System of Units (SI), the Imperial system, or US Customary units. Recording the system of units as a reference entity enables validation rules that prevent mixing incompatible unit systems in a single calculation and supports conversion path determination by identifying a common base for inter-system conversions.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this SystemOfUnits record, referenced by UnitOfMeasure records via system_of_units_id to classify each unit into its measurement system family. |
| name | String |  | The standard name for this system of units, such as International System of Units (SI), Imperial, or US Customary, used in validation messages and unit selection guidance. |
| description | String |  | A description of the system origin, authority, and the physical quantities it covers, enabling implementors to understand applicability scope and any known incompatibilities with other systems. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | SystemOfUnits is a reference entity that identifies the measurement system to which a UnitOfMeasure belongs, such as the International System of Units (SI), the Imperial system, or US Customary units. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |

---

*Generated: 2026-06-24 10:33:17*