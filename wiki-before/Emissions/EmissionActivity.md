# EmissionActivity

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivity is a master-data entity that represents a discrete operational process or event that generates, absorbs, or transfers greenhouse gas emissions. Each activity is linked to an EmissionActivityType and an EmissionActivityCategory, enabling aggregation and scope attribution. The entity supports a self-referential hierarchy through parent_id, allowing complex multi-level activity structures to be modelled without loss of granularity.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system identifier for this EmissionActivity record, referenced by EmissionStatement, EmissionActivityFlow, and parameter records to associate measurements with a specific activity. |
| parent_id | String |  | A self-referential foreign key that points to the parent EmissionActivity in a hierarchical decomposition. A null value indicates a root-level activity. |
| emission_activity_type_id | String |  | Foreign key to the EmissionActivityType record that classifies the nature of this activity, for example Stationary Combustion or Mobile Combustion. |
| emission_activity_category_id | String |  | Foreign key to the EmissionActivityCategory that places this activity within the GHG Protocol or ISO 14064 category structure, such as Category 1 Purchased goods and services. |
| name | String |  | A descriptive label for the activity instance, uniquely identifying it within its parent context, such as "Boiler 3 Site A natural gas combustion". |
| description | String |  | Free-text narrative providing additional context on how the activity is performed, what sources or sinks it involves, and any special treatment applied during calculation. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivity is a master-data entity that represents a discrete operational process or event that generates, absorbs, or transfers greenhouse gas emissions. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.md) |
| Association |  | [EmissionActivityFlow](../Products/EmissionActivityFlow.md) |
| Association |  | [EmissionSink](EmissionSink.md) |
| Association |  | [EmissionSource](EmissionSource.md) |
| Association |  | [FacilityActivityParticipation](../Facilities/FacilityActivityParticipation.md) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.md) |
| Association |  | [EmissionActivityType](EmissionActivityType.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.md) |
| Association |  | [EmissionSink](EmissionSink.md) |
| Association |  | [EmissionSource](EmissionSource.md) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.md) |
| Association |  | [EmissionActivityType](EmissionActivityType.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.md) |
| Association |  | [EmissionSource](EmissionSource.md) |
| Association |  | [EmissionSink](EmissionSink.md) |
| Association |  | [EmissionActivityType](EmissionActivityType.md) |
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.md) |

---

*Generated: 2026-06-24 14:20:12*