# EmissionInventory

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period. It groups all EmissionStatement records that together constitute a complete inventory of an organisation's greenhouse gas emissions, organised by scope, source, and boundary. The inventory record holds the metadata required for formal reporting, including the applicable standard, the organisational boundary method, and audit trail information.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system identifier for this EmissionInventory record, used to associate all child emission statements and reporting artefacts with this specific inventory exercise. |
| organisation_id | String |  | Foreign key linking the inventory to the Organisation that owns and is responsible for the GHG data it contains. |
| standard_id | String |  | Foreign key to the Standard record that governs the methodology and boundary rules applied in this inventory, typically the GHG Protocol Corporate Standard or ISO 14064-1. |
| name | String |  | A human-readable label for the inventory, such as "FY2025 GHG Inventory Scope 1 and 2", used for identification and search in reporting systems. |
| description | String |  | A free-text narrative describing the scope, boundaries, and methodology choices made for this inventory, providing context for reviewers and auditors. |
| reporting_period_start | String |  | The first day of the reporting period covered by this inventory. Together with reporting_period_end it defines the temporal boundary against which emission activities are assessed. |
| reporting_period_end | String |  | The last day of the reporting period covered by this inventory. Emission activities with dates outside this window are excluded from the inventory totals. |
| organisational_boundary_method | String |  | The consolidation approach used to determine which entities and facilities fall within the inventory boundary, drawn from the GHG Protocol values: equity share, financial control, or operational control. |
| status | String |  | The lifecycle status of the inventory record, such as draft, under review, finalised, or audited, used to manage approval workflows and publication control. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionReport](EmissionReport.md) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [Standard](../Organisation/Standard.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [EmissionReport](EmissionReport.md) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.md) |
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [EmissionReport](EmissionReport.md) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.md) |

---

*Generated: 2026-06-22 22:08:54*