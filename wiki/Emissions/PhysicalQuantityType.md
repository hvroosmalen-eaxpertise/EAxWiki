# PhysicalQuantityType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area. It enables dimensional analysis validation in calculation models, ensuring for example that an emission factor expressed in kgCO2/kWh is applied to an activity parameter expressed in an energy unit rather than a mass or distance unit. This prevents a class of calculation errors that are otherwise difficult to detect programmatically.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this PhysicalQuantityType record, referenced by UnitOfMeasure records to establish which physical dimension a unit measures. |
| name | String |  | The standard name for the physical quantity, such as Mass, Energy, Thermodynamic Temperature, or Volume, used in validation error messages and unit catalogue documentation. |
| description | String |  | A description of the physical quantity, its SI base dimensions, and any special considerations relevant to GHG accounting, such as the distinction between higher and lower calorific values for energy quantities. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 797 → 779 |

---

*Generated: 2026-06-18 12:23:55*