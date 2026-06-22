# EmissionComponentCategoryGroup

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures. Typical groups include Carbon dioxide, Methane and nitrous oxide, Fluorinated gases (F-gases), Biogenic CO2, and Aggregate CO2-equivalent. This grouping is required by several reporting frameworks that present emission totals at the gas-group level in addition to individual gas breakdowns.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionComponentCategoryGroup record, referenced by EmissionComponentCategory records via emission_component_category_group_id to establish the roll-up hierarchy. |
| name | String |  | The label for this gas-category group, such as Fluorinated gases or Biogenic carbon flows, used in summary tables and disclosure headings. |
| description | String |  | A description of which greenhouse gases are included in this group, the rationale for the grouping (e.g. regulatory definition or characterisation method), and how the group total is aggregated. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

---

*Generated: 2026-06-22 17:43:22*