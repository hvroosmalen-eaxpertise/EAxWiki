# Country

**Type:** Class  
**Stereotype:** reference-data  

Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name. The Country entity serves as a reference point for linking addresses and geopolitical entities to a standardised country identifier, enabling geographic analysis and cross-border data alignment across the model. Country is modelled as a subtype of GeopoliticalEntity in the standard's conceptual model, distinguished by GeopoliticalEntityType, but is represented as a dedicated reference entity here to support direct foreign key relationships from Address and other entities that specifically require a country reference.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Country record. It serves as the primary key referenced by Address, GeopoliticalEntity, and other entities that require an unambiguous country context. |
| iso_alpha_3_code | String |  | The three-letter ISO 3166-1 alpha-3 country code, such as "NLD" for the Netherlands or "DEU" for Germany. This code is the primary standardised identifier used in international data exchange, regulatory reporting, and cross-border emissions factor selection. |
| name | String |  | The official name of the country or territory as published in the ISO 3166-1 country list, such as "Kingdom of the Netherlands" or "Federal Republic of Germany". The name is used in user interfaces and reports to present a human-readable country reference alongside the ISO code. |
| geopolitical_entity_type_id | String |  | A foreign key identifying the GeopoliticalEntityType that classifies this country record, typically "Country". This attribute supports the integration of Country with the broader GeopoliticalEntity hierarchy. |
| location_id | String |  | A foreign key linking this Country to a corresponding Location record in the location hierarchy, enabling the country to participate in the location parent–child structure used across the model. |
| parent_location_id | String |  | A foreign key referencing the parent Location record in the geographic hierarchy, used to build multi-level geographic structures where a country is nested within a larger region or grouping. |
| effective_datetime | String |  | The date and time from which this country record is valid, in ISO 8601 format. This attribute supports the tracking of country code changes or the creation of new countries over time, preserving a historical record of geographic attribution. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 745 → 766 |
| Association |  | 745 → 737 |
