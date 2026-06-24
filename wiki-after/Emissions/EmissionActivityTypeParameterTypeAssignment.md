# EmissionActivityTypeParameterTypeAssignment

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement is mandatory or optional. Each template is composed of one or more of these assignment records, forming the complete parameter checklist for the associated activity type and jurisdiction.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this parameter type assignment record, used to enumerate the complete list of required measurements within a recording template. |
| emission_activity_parameter_recording_template_id | String |  | Foreign key to the parent EmissionActivityParameterRecordingTemplate that this assignment belongs to, grouping all parameter requirements for a given activity type and jurisdiction. |
| emission_parameter_type_id | String |  | Foreign key to the EmissionParameterType that must be recorded, identifying the specific measurement slot that this assignment adds to the template. |
| is_mandatory | String |  | Indicates whether measurement of this parameter type is required (true) or optional (false) for the activity type and jurisdiction defined by the parent template, supporting data completeness validation. |
| sequence_number | String |  | The display order of this parameter assignment within the template, used to present data entry fields in a logical sequence to facility operators. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement  |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.md) |

---

*Generated: 2026-06-24 14:21:20*