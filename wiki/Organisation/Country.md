---
ea_id: 745
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: f11fbdf0
---

# <span class="sl" data-layer="uml">reference-data</span> Country

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="745" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/Country.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="745" data-file-path="Organisation/Country.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name. The Country entity serves as a reference point for linking addresses and geopolitical entities to a standardised country identifier, enabling geographic analysis and cross-border data alignment across the model. Country is modelled as a subtype of GeopoliticalEntity in the standard's conceptual model, distinguished by GeopoliticalEntityType, but is represented as a dedicated reference entity here to support direct foreign key relationships from Address and other entities that specifically require a country reference.</p>
<!--ea-notes-end-->
</div>
</div>

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

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [GeopoliticalEntity](../Facilities/GeopoliticalEntity.md) |
| Association |  | [Address](Address.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e766","label":"GeopoliticalEntity","fullName":"GeopoliticalEntity","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/GeopoliticalEntity.html"},{"id":"e737","label":"Address","fullName":"Address","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Address.html"},{"id":"e745","label":"Country","fullName":"Country","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityParameterRecordingTemplate.html"},{"id":"e765","label":"GeopoliticalEntityType","fullName":"GeopoliticalEntityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/GeopoliticalEntityType.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"}],"edges":[{"id":"c813","source":"e806","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c884","source":"e766","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c885","source":"e765","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c886","source":"e766","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c893","source":"e745","target":"e766","label":"Association","sourceLayer":"uml"},{"id":"c909","source":"e745","target":"e737","label":"Association","sourceLayer":"uml"},{"id":"c911","source":"e737","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*