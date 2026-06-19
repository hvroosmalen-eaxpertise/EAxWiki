# EmissionCategoryStandardAssociation

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard. It enables the model to represent the fact that GHG Protocol, ISO 14064-1, and ESRS each define overlapping but not identical category taxonomies, and it allows emission statements to be classified under the correct category hierarchy for each applicable standard without duplicating the statement records themselves.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this association record, used as a stable reference when auditing which standards recognise a given emission category classification. |
| emission_activity_category_id | String |  | Foreign key to the EmissionActivityCategory whose recognition under a specific standard is being recorded by this association. |
| standard_id | String |  | Foreign key to the Standard that recognises this emission activity category, enabling category-to-standard traceability for multi-framework reporting. |
| category_code_per_standard | String |  | The code or identifier that the referenced Standard uses for this category, which may differ from the category own identifier, supporting standards-specific labelling in disclosures. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 787 → 734 |
| Association |  | 787 → 774 |

---

*Generated: 2026-06-19 11:58:40*