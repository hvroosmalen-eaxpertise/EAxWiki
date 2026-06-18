# FacilityLocationType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location. This type classification drives which subtype entity provides the detailed location attributes for a given Location record and determines how geographic attribution is performed for emission reporting purposes.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityLocationType record. It must be stable so that Location records that reference it remain valid when the type vocabulary is extended. |
| name | String |  | The descriptive label for the facility location type, drawn from the three values defined in the standard: "Business Area", "Geopolitical Entity", and "Geospatial Location". The name determines which location subtype provides the detailed attributes for Location records classified under this type. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 769 → 758 |
| Association |  | 769 → 755 |

---

*Generated: 2026-06-18 13:26:10*