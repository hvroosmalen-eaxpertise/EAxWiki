# EmissionRecordingMethodType

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, derived from a published default, or extrapolated from related data. This classification is required by ISO 14064-1 and GHG Protocol for data quality assessments and for identifying which statements require improved monitoring as part of an improvement plan.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionRecordingMethodType record, referenced by EmissionStatement records to describe how each emission quantity was obtained. |
| name | String |  | The label for the recording method, drawn from values such as Measured, Calculated, Estimated, Default factor, or Extrapolated, used in data quality tables and assurance documentation. |
| description | String |  | A description of the recording method approach, its typical applicability, and the data quality tier it represents, supporting selection guidance for practitioners completing inventory data entry. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, der |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](EmissionStatement.md) |

---

*Generated: 2026-06-24 14:20:12*