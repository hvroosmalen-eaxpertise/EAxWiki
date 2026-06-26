# <span class="sl" data-layer="uml">reference-data</span> EmissionCategoryStandardAssociation

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard. It enables the model to represent the fact that GHG Protocol, ISO 14064-1, and ESRS each define overlapping but not identical category taxonomies, and it allows emission statements to be classified under the correct category hierarchy for each applicable standard without duplicating the statement records themselves.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this association record, used as a stable reference when auditing which standards recognise a given emission category classification. |
| emission_activity_category_id | String |  | Foreign key to the EmissionActivityCategory whose recognition under a specific standard is being recorded by this association. |
| standard_id | String |  | Foreign key to the Standard that recognises this emission activity category, enabling category-to-standard traceability for multi-framework reporting. |
| category_code_per_standard | String |  | The code or identifier that the referenced Standard uses for this category, which may differ from the category own identifier, supporting standards-specific labelling in disclosures. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityCategory.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"}],"edges":[{"id":"c843","source":"e789","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c845","source":"e788","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c847","source":"e787","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c850","source":"e794","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c897","source":"e734","target":"e740","label":"Association","sourceLayer":"uml"},{"id":"c916","source":"e734","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c848","source":"e787","target":"e774","label":"Association","sourceLayer":"uml"},{"id":"c860","source":"e772","target":"e774","label":"Association","sourceLayer":"uml"},{"id":"c861","source":"e774","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-26 17:14:27*