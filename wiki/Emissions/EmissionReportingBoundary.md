# EmissionReportingBoundary

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

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

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionReportingBoundary is a work-product-component that defines and documents the organisational, operational, and geographic boundary applied to a specific EmissionInventory or EmissionReport. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 808 → 771 |

---

*Generated: 2026-06-18 13:26:10*