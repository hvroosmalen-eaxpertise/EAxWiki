---
ea_id: 765
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> GeopoliticalEntityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including "Country", "Nature Reserve", "State", and "Region". Classifying geopolitical entities by type enables geographic analysis at multiple administrative levels and supports regulatory jurisdiction mapping in emissions reporting. The code attribute supports alignment with external geopolitical classification systems such as those maintained by the United Nations.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the GeopoliticalEntityType record. It must be stable so that GeopoliticalEntity records that reference it remain valid when the type vocabulary is extended. |
| code | String |  | A short alphanumeric code for the geopolitical entity type, such as "COUNTRY" or "STATE", drawn from an external geopolitical classification system where one exists. The code is provided by the source for Geopolitical Entity Names. |
| name | String |  | The descriptive label for the geopolitical entity type, representing the types of geographical areas that can be used to identify the location of a facility, such as "Country", "Nature Reserve", "State", or "Region". |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including "Country", "Nature Reserve", "State", and "Region". |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"GeopoliticalEntity.html"},{"id":"e765","label":"GeopoliticalEntityType","fullName":"GeopoliticalEntityType","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityParameterRecordingTemplate.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e745","label":"Country","fullName":"Country","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Country.html"}],"edges":[{"id":"c813","source":"e806","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c885","source":"e765","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c886","source":"e766","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c893","source":"e745","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 14:47:48*