# EmissionActivityParameterRecordingTemplate

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context. It acts as a data-collection template that tells facility operators which parameters they must record for each activity type, ensuring that all inputs needed by the applicable calculation models are systematically collected. Templates may be jurisdiction-specific to reflect local regulatory data requirements.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this recording template record, referenced by EmissionActivityTypeParameterTypeAssignment records that enumerate the individual parameter slots making up this template. |
| emission_activity_type_id | String |  | Foreign key to the EmissionActivityType for which this template defines the required parameter measurements, linking the template to the activity classification it serves. |
| geopolitical_entity_id | String |  | Foreign key to the GeopoliticalEntity (typically a country or regulatory jurisdiction) for which this template applies, enabling jurisdiction-specific parameter requirements. |
| name | String |  | A human-readable label for the template, such as UK Stationary Combustion Natural Gas Tier 2 Parameters, used in data collection system configuration and operator guidance. |
| description | String |  | A description of the template purpose, the regulatory or methodological rationale for the parameter requirements, and any conditions under which this template is mandatory or optional. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 807 → 806 |
| Association |  | 806 → 766 |
| Association |  | 806 → 786 |

---

*Generated: 2026-06-18 12:23:55*