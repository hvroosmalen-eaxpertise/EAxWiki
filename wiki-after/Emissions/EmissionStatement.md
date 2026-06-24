# EmissionStatement

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionStatement is the central work-product-component that records a single quantified emission result: the GHG emissions or removals attributable to one EmissionActivity within one reporting period. It links the activity, the organisational boundary, the scope type, the calculation model used, and the resulting emission quantity and unit, forming the atomic building block of an emission inventory. Multiple statements are aggregated into an EmissionInventory to produce total scope-level and entity-level disclosures.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system identifier for this EmissionStatement record, used to link emission components, per-standard breakdowns, and uncertainty assessments to this specific quantified result. |
| emission_inventory_id | String |  | Foreign key to the EmissionInventory that this statement belongs to, grouping the statement within its parent accounting exercise. |
| emission_activity_id | String |  | Foreign key to the EmissionActivity that generated or absorbed the emissions recorded in this statement. |
| emission_scope_type_id | String |  | Foreign key to the EmissionScopeType (Scope 1, 2, or 3) attributed to this statement, derived from the activity category unless overridden by a specific business rule. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel applied to derive the emission quantity from activity data, capturing the methodological choice made for this statement. |
| quantity | String |  | The total GHG emission or removal quantity calculated for this activity instance, expressed in the unit of measure referenced by quantity_unit_of_measure_id. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure record expressing the unit in which the emission quantity is reported, typically tCO2e. |
| reporting_period_start | String |  | The start date of the specific period to which this statement applies, which may be shorter than the parent inventory period for partial-period corrections. |
| reporting_period_end | String |  | The end date of the period to which this statement applies, enabling time-series analysis within and across inventory cycles. |
| recording_method_type_id | String |  | Foreign key to the EmissionRecordingMethodType that indicates whether the emission quantity was measured, calculated, estimated, or derived from a default factor, supporting data quality assessments. |
| notes | String |  | Free-text field for the preparer to record assumptions, data sources, or explanatory comments specific to this emission statement that support auditability. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionStatement is the central work-product-component that records a single quantified emission result: the GHG emissions or removals attributable to one EmissionActivity within one reporting period. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.md) |
| Association |  | [OrganizationEmissionAllocation](OrganizationEmissionAllocation.md) |
| Association |  | [RecordingUncertaintyAssessment](RecordingUncertaintyAssessment.md) |
| Association |  | [EmissionRecordingMethodType](EmissionRecordingMethodType.md) |
| Association |  | [FacilityEmissionAllocation](../Facilities/FacilityEmissionAllocation.md) |
| Association |  | [EmissionStatementPerStandard](EmissionStatementPerStandard.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionComponent](EmissionComponent.md) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |
| Association |  | [EmissionScopeType](EmissionScopeType.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [EmissionInventory](EmissionInventory.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionInventory](EmissionInventory.md) |
| Association |  | [EmissionScopeType](EmissionScopeType.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.md) |
| Association |  | [OrganizationEmissionAllocation](OrganizationEmissionAllocation.md) |
| Association |  | [RecordingUncertaintyAssessment](RecordingUncertaintyAssessment.md) |
| Association |  | [EmissionRecordingMethodType](EmissionRecordingMethodType.md) |
| Association |  | [EmissionStatementPerStandard](EmissionStatementPerStandard.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |
| Association |  | [EmissionScopeType](EmissionScopeType.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [EmissionInventory](EmissionInventory.md) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionStatementPerStandard](EmissionStatementPerStandard.md) |
| Association |  | [EmissionRecordingMethodType](EmissionRecordingMethodType.md) |
| Association |  | [RecordingUncertaintyAssessment](RecordingUncertaintyAssessment.md) |
| Association |  | [OrganizationEmissionAllocation](OrganizationEmissionAllocation.md) |
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.md) |

---

*Generated: 2026-06-24 14:21:20*