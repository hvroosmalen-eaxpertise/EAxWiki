# Organization

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

Organization is the central anchor entity for emissions reporting. It represents any legal entity, company, subsidiary, joint venture, or other organisational unit that is subject to emissions accounting obligations or that voluntarily participates in a carbon disclosure programme. The Organization entity links directly to standards, types, contact persons, addresses, and organisational boundaries, forming the root context from which all emission inventories and product footprints are ultimately traceable. An organization may have an external identifier issued by a third-party registry such as GLEIF LEI codes, enabling cross-system reconciliation.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Organisation record. It serves as the primary key referenced by emission inventories, organisational boundaries, and contact persons. It must be immutable once assigned and must be globally unique within the system. |
| name | String |  | The registered legal or trading name of the organisation as it appears in official filings or public disclosures. The name is used in reports, dashboards, and data exchange messages to identify the reporting entity and must correspond to the name used in external registries where applicable. |
| description | String |  | A free-text field providing supplementary information about the organisation, such as its primary business activities, geographic operating regions, or its role within a supply chain. This field supports both analytical queries and human-readable reporting by providing context that is not captured in structured attributes. |
| external_identifier | String |  | An identifier assigned to the organisation by an external system or registry, such as a Legal Entity Identifier (LEI), DUNS number, or national company registration number. The external identifier enables interoperability with external data sources and is used when exchanging emissions data with supply chain partners or regulators who reference the organisation by an external code. |
| organization_type_id | String |  | A foreign key reference to the OrganizationType that classifies this organisation. Storing the type reference on the Organisation entity allows rapid filtering by organisational type without joining through a separate intersection table in the most common query patterns. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Organization is the central anchor entity for emissions reporting. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 801 → 735 |
| Association |  | 735 → 782 |
| Association |  | 735 → 753 |
| Association |  | 740 → 735 |
| Association |  | 735 → 751 |
| Association |  | 735 → 750 |
| Association |  | 735 → 748 |
| Association |  | 747 → 735 |
| Association |  | 738 → 735 |
| Association |  | 735 → 743 |
| Association |  | 735 → 741 |
| Association |  | 735 → 736 |
| Association |  | 735 → 771 |

---

*Generated: 2026-06-18 13:11:38*