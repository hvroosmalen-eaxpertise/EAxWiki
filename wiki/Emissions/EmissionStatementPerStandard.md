# EmissionStatementPerStandard

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionStatementPerStandard is an intersection entity that records the emission quantity attributed to a specific EmissionStatement as it must be reported under a particular Standard or reporting framework. Different frameworks may require different consolidation rules, boundary definitions, or GWP factors, so the same underlying activity data may yield different disclosed quantities depending on the framework applied. This entity preserves each framework-specific result without duplicating the underlying statement.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this per-standard emission result record. |
| emission_statement_id | String |  | Foreign key to the parent EmissionStatement from which this per-standard quantity is derived. |
| standard_id | String |  | Foreign key to the Standard or framework under which this specific quantity is reported, such as GHG Protocol, ISO 14064-1, or ESRS E1. |
| quantity | String |  | The emission quantity as calculated or adjusted for disclosure under the referenced standard, which may differ from the parent statement quantity due to framework-specific boundary or GWP rules. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure expressing the unit in which the per-standard quantity is reported, typically tCO2e but potentially differing by framework requirement. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionStatementPerStandard is an intersection entity that records the emission quantity attributed to a specific EmissionStatement as it must be reported under a particular Standard or reporting framework. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

---

*Generated: 2026-06-22 17:43:22*