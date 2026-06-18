# EmissionComponentPerStandard

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponentPerStandard is an intersection entity analogous to EmissionStatementPerStandard but at the component (individual gas) level, recording the gas-specific emission quantity as it must be disclosed under a particular standard. Different standards apply different global-warming-potential assessment reports (AR4, AR5, AR6) which change the CO2-equivalent values of non-CO2 gases, meaning the same physical emission quantity may produce different tCO2e results across frameworks.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this per-standard component result record. |
| emission_component_id | String |  | Foreign key to the parent EmissionComponent from which this framework-adjusted quantity is derived. |
| standard_id | String |  | Foreign key to the Standard under which this component quantity is reported and to which the applicable GWP values belong. |
| quantity | String |  | The gas-specific emission quantity as adjusted for the GWP factors mandated by the referenced standard, enabling cross-standard comparison of individual gas contributions. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which this per-standard component quantity is expressed, typically tCO2e using the GWP factor set of the referenced standard. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponentPerStandard is an intersection entity analogous to EmissionStatementPerStandard but at the component (individual gas) level, recording the gas-specific emission quantity as it must be disclosed under a particular standard. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 789 → 734 |
| Association |  | 789 → 778 |

---

*Generated: 2026-06-18 13:11:38*