# OrganizationalBoundary

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationalBoundary defines the scope boundary used to determine which emission sources are included in an organisation's GHG inventory. The GHG Protocol specifies three common approaches: equity share, financial control, and operational control. A boundary record links a named boundary definition to the organisation it applies to, enabling an organisation to maintain multiple concurrent boundary definitions for different reporting frameworks or stakeholder requirements. The boundary concept is foundational to GHG accounting because it determines which emission activities are attributed to the reporting organisation.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationalBoundary record. It is the primary key referenced by EmissionActivity to associate each activity with the boundary under which it is reported. It must be unique and stable across changes to the boundary's name or associated organisation. |
| name | String |  | The descriptive name of the organisational boundary approach, such as "Operational Control", "Financial Control", or "Equity Share 50%". The name conveys the boundary-setting methodology applied and is displayed in inventory reports and disclosure documents to indicate the scope of the reported emissions data. |
| organization_id | String |  | A foreign key identifying the Organisation for which this boundary is defined. An organisation may have multiple boundary records reflecting different consolidation approaches or different reporting frameworks, all linked through this attribute. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationalBoundary defines the scope boundary used to determine which emission sources are included in an organisation's GHG inventory. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 734 → 740 |
| Association |  | 740 → 735 |

---

*Generated: 2026-06-18 13:26:10*