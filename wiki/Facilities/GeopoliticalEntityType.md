# GeopoliticalEntityType

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including "Country", "Nature Reserve", "State", and "Region". Classifying geopolitical entities by type enables geographic analysis at multiple administrative levels and supports regulatory jurisdiction mapping in emissions reporting. The code attribute supports alignment with external geopolitical classification systems such as those maintained by the United Nations.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the GeopoliticalEntityType record. It must be stable so that GeopoliticalEntity records that reference it remain valid when the type vocabulary is extended. |
| code | String |  | A short alphanumeric code for the geopolitical entity type, such as "COUNTRY" or "STATE", drawn from an external geopolitical classification system where one exists. The code is provided by the source for Geopolitical Entity Names. |
| name | String |  | The descriptive label for the geopolitical entity type, representing the types of geographical areas that can be used to identify the location of a facility, such as "Country", "Nature Reserve", "State", or "Region". |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including "Country", "Nature Reserve", "State", and "Region". |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |

---

*Generated: 2026-06-22 22:08:54*