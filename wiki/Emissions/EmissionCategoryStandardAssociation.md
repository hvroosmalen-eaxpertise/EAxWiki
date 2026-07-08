---
ea_id: 787
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 69803db2
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionCategoryStandardAssociation

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Standard&quot;,&quot;fullName&quot;:&quot;Standard&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Standard.html&quot;},{&quot;id&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;EmissionActivityCategory&quot;,&quot;fullName&quot;:&quot;EmissionActivityCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityCategory.html&quot;},{&quot;id&quot;:&quot;e787&quot;,&quot;label&quot;:&quot;EmissionCategoryStandar…&quot;,&quot;fullName&quot;:&quot;EmissionCategoryStandardAssociation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e789&quot;,&quot;label&quot;:&quot;EmissionComponentPerSta…&quot;,&quot;fullName&quot;:&quot;EmissionComponentPerStandard&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentPerStandard.html&quot;},{&quot;id&quot;:&quot;e788&quot;,&quot;label&quot;:&quot;EmissionStatementPerSta…&quot;,&quot;fullName&quot;:&quot;EmissionStatementPerStandard&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatementPerStandard.html&quot;},{&quot;id&quot;:&quot;e794&quot;,&quot;label&quot;:&quot;StandardSourceAssociati…&quot;,&quot;fullName&quot;:&quot;StandardSourceAssociation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;StandardSourceAssociation.html&quot;},{&quot;id&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;EmissionReport&quot;,&quot;fullName&quot;:&quot;EmissionReport&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionReport.html&quot;},{&quot;id&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;OrganizationalBoundary&quot;,&quot;fullName&quot;:&quot;OrganizationalBoundary&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/OrganizationalBoundary.html&quot;},{&quot;id&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;EmissionCalculationModel&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModel&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModel.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionInventory.html&quot;},{&quot;id&quot;:&quot;e772&quot;,&quot;label&quot;:&quot;EmissionScopeType&quot;,&quot;fullName&quot;:&quot;EmissionScopeType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionScopeType.html&quot;},{&quot;id&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;EmissionActivity&quot;,&quot;fullName&quot;:&quot;EmissionActivity&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivity.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c843&quot;,&quot;source&quot;:&quot;e789&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c845&quot;,&quot;source&quot;:&quot;e788&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c847&quot;,&quot;source&quot;:&quot;e787&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c850&quot;,&quot;source&quot;:&quot;e794&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c894&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c897&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c916&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c918&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c848&quot;,&quot;source&quot;:&quot;e787&quot;,&quot;target&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c860&quot;,&quot;source&quot;:&quot;e772&quot;,&quot;target&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c861&quot;,&quot;source&quot;:&quot;e774&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c808&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c863&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*