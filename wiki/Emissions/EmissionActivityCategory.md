# EmissionActivityCategory

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1. For Scope 3, this includes the fifteen upstream and downstream categories such as purchased goods and services, capital goods, fuel and energy-related activities, and so on. The category drives which reporting lines an emission activity contributes to and enables cross-organisation comparability.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityCategory record, such as S3C01 for Scope 3 Category 1, used to group and aggregate emission statements by reporting category. |
| emission_scope_type_id | String |  | Foreign key linking this category to the EmissionScopeType (Scope 1, 2, or 3) to which it belongs, ensuring that emission statements are correctly scope-attributed through their activity category. |
| name | String |  | The standard name for the category, such as "Category 4 - Upstream transportation and distribution", used in disclosures and summary tables. |
| description | String |  | A normative description of what activities and emission sources are included in this category per the applicable standard, providing boundary guidance for activity classification. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 787 → 774 |
| Association |  | 772 → 774 |
| Association |  | 774 → 773 |

---

*Generated: 2026-06-18 13:11:38*