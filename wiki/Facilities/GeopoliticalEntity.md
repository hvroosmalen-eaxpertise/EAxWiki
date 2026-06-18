# GeopoliticalEntity

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces. It is classified by a GeopoliticalEntityType and optionally linked to a Country. Examples include "USA", "Canada", "United Kingdom", "Yellow Stone National Park", "Alberta", and "North Sea". Geopolitical entities provide the administrative geographic context required for regulatory jurisdiction mapping, reporting by jurisdiction, and alignment with country-level emission factor datasets.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the GeopoliticalEntity record. This serves as both the primary key of the entity and as the foreign key value linking to a Location record. |
| name | String |  | The official name of the geopolitical entity as it is defined and administered by the relevant authority, such as "Flanders", "Bavaria", or "Yellow Stone National Park". |
| geopolitical_entity_type_id | String |  | A foreign key referencing the GeopoliticalEntityType that classifies this entity, such as "Country", "State", or "Nature Reserve". |
| parent_location_id | String |  | A foreign key referencing the parent Location record in the geographic hierarchy, for example linking a State to its Country parent. |
| effective_datetime | String |  | The date and time from which this geopolitical entity record is valid, in ISO 8601 format. Supports tracking of boundary changes or the creation of new entities over time. |
| termination_datetime | String |  | The date and time at which this geopolitical entity record is terminated, in ISO 8601 format. Null if the entity is currently active. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 806 → 766 |
| Association |  | 766 → 766 |
| Association |  | 765 → 766 |
| Association |  | 766 → 755 |
| Association |  | 745 → 766 |

---

*Generated: 2026-06-18 13:26:10*