---
ea_id: 774
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 72809227
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionActivityCategory

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1. For Scope 3, this includes the fifteen upstream and downstream categories such as purchased goods and services, capital goods, fuel and energy-related activities, and so on. The category drives which reporting lines an emission activity contributes to and enables cross-organisation comparability.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityCategory record, such as S3C01 for Scope 3 Category 1, used to group and aggregate emission statements by reporting category. |
| emission_scope_type_id | String |  | Foreign key linking this category to the EmissionScopeType (Scope 1, 2, or 3) to which it belongs, ensuring that emission statements are correctly scope-attributed through their activity category. |
| name | String |  | The standard name for the category, such as "Category 4 - Upstream transportation and distribution", used in disclosures and summary tables. |
| description | String |  | A normative description of what activities and emission sources are included in this category per the applicable standard, providing boundary guidance for activity classification. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCategoryStandardAssociation](EmissionCategoryStandardAssociation.md) |
| Association |  | [EmissionScopeType](EmissionScopeType.md) |
| Association |  | [EmissionActivity](EmissionActivity.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionScopeType](EmissionScopeType.md) |
| Association |  | [EmissionCategoryStandardAssociation](EmissionCategoryStandardAssociation.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e787&quot;,&quot;label&quot;:&quot;EmissionCategoryStandar…&quot;,&quot;fullName&quot;:&quot;EmissionCategoryStandardAssociation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCategoryStandardAssociation.html&quot;},{&quot;id&quot;:&quot;e772&quot;,&quot;label&quot;:&quot;EmissionScopeType&quot;,&quot;fullName&quot;:&quot;EmissionScopeType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionScopeType.html&quot;},{&quot;id&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;EmissionActivity&quot;,&quot;fullName&quot;:&quot;EmissionActivity&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivity.html&quot;},{&quot;id&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;EmissionActivityCategory&quot;,&quot;fullName&quot;:&quot;EmissionActivityCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Standard&quot;,&quot;fullName&quot;:&quot;Standard&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Standard.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;},{&quot;id&quot;:&quot;e802&quot;,&quot;label&quot;:&quot;ActivityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;ActivityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ActivityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;EmissionActivityFlow&quot;,&quot;fullName&quot;:&quot;EmissionActivityFlow&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/EmissionActivityFlow.html&quot;},{&quot;id&quot;:&quot;e785&quot;,&quot;label&quot;:&quot;EmissionSink&quot;,&quot;fullName&quot;:&quot;EmissionSink&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionSink.html&quot;},{&quot;id&quot;:&quot;e784&quot;,&quot;label&quot;:&quot;EmissionSource&quot;,&quot;fullName&quot;:&quot;EmissionSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionSource.html&quot;},{&quot;id&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;FacilityActivityPartici…&quot;,&quot;fullName&quot;:&quot;FacilityActivityParticipation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/FacilityActivityParticipation.html&quot;},{&quot;id&quot;:&quot;e786&quot;,&quot;label&quot;:&quot;EmissionActivityType&quot;,&quot;fullName&quot;:&quot;EmissionActivityType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityType.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c847&quot;,&quot;source&quot;:&quot;e787&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c848&quot;,&quot;source&quot;:&quot;e787&quot;,&quot;target&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c860&quot;,&quot;source&quot;:&quot;e772&quot;,&quot;target&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c869&quot;,&quot;source&quot;:&quot;e772&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c822&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c836&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c838&quot;,&quot;source&quot;:&quot;e785&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c839&quot;,&quot;source&quot;:&quot;e784&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c842&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c861&quot;,&quot;source&quot;:&quot;e774&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c862&quot;,&quot;source&quot;:&quot;e786&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c863&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c870&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c823&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*