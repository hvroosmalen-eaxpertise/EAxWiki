# <span class="sl" data-layer="uml">master-data</span> EmissionReportingBoundary

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionReportingBoundary is a work-product-component that defines and documents the organisational, operational, and geographic boundary applied to a specific EmissionInventory or EmissionReport. It records the boundary-setting methodology, the entities included and excluded, and the rationale for any exclusions, providing the complete boundary documentation required by GHG Protocol and ISO 14064-1 first-party assertions and supporting the assurance engagement.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionReportingBoundary record, linking it to the inventory or report whose boundary it describes. |
| emission_inventory_id | String |  | Foreign key to the EmissionInventory whose organisational and operational boundary this record documents, establishing which activities and entities are in scope for that inventory. |
| boundary_method | String |  | The consolidation approach used to set the organisational boundary, drawn from the GHG Protocol values: equity share, financial control, or operational control. |
| description | String |  | A narrative description of the boundary, listing the legal entities, facilities, and activities included within scope and the justification for any material exclusions. |
| excluded_entities | String |  | A structured or free-text list of organisational entities, facilities, or activities explicitly excluded from the inventory boundary, with the reason for each exclusion. |
| excluded_entities_rationale | String |  | A narrative explanation of why the excluded entities fall outside the chosen boundary methodology, supporting the transparency and completeness principles required for credible GHG disclosures. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionReportingBoundary is a work-product-component that defines and documents the organisational, operational, and geographic boundary applied to a specific EmissionInventory or EmissionReport. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionInventory](EmissionInventory.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e808","label":"EmissionReportingBounda…","fullName":"EmissionReportingBoundary","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"}],"edges":[{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c810","source":"e808","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-26 17:02:53*