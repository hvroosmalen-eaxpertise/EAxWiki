# EmissionActivityType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory. Examples include Stationary Combustion, Mobile Combustion, Process Emissions, Fugitive Emissions, and Land Use Change. The type is used in calculation model selection and in parameter recording template assignment to determine which measurement parameters are required for a given activity.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityType record, referenced by EmissionActivity records and by EmissionActivityParameterRecordingTemplate assignments to drive parameter requirements. |
| name | String |  | The standard label for the activity type, such as Mobile Combustion Road Transport or Process Emissions Cement Clinker Production, used in model configuration and report category labels. |
| description | String |  | A description of the physical or chemical processes that characterise activities of this type and the typical emission sources and parameters associated with them. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 806 → 786 |
| Association |  | 793 → 786 |
| Association |  | 786 → 773 |

---

*Generated: 2026-06-19 13:04:05*