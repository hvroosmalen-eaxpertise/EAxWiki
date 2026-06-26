# <span class="sl" data-layer="uml">reference-data</span> EmissionComponentCategoryGroup

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures. Typical groups include Carbon dioxide, Methane and nitrous oxide, Fluorinated gases (F-gases), Biogenic CO2, and Aggregate CO2-equivalent. This grouping is required by several reporting frameworks that present emission totals at the gas-group level in addition to individual gas breakdowns.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionComponentCategoryGroup record, referenced by EmissionComponentCategory records via emission_component_category_group_id to establish the roll-up hierarchy. |
| name | String |  | The label for this gas-category group, such as Fluorinated gases or Biogenic carbon flows, used in summary tables and disclosure headings. |
| description | String |  | A description of which greenhouse gases are included in this group, the rationale for the grouping (e.g. regulatory definition or characterisation method), and how the group total is aggregated. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"},{"id":"e795","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategoryGroup","packageName":"Emissions","isFocal":true,"hasUrl":false,"url":""},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"}],"edges":[{"id":"c809","source":"e792","target":"e795","label":"Association"},{"id":"c831","source":"e795","target":"e792","label":"Association"},{"id":"c856","source":"e792","target":"e780","label":"Association"},{"id":"c866","source":"e792","target":"e778","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:40*