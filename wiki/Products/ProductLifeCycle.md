---
ea_id: 813
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: f23c1c60
---

# <span class="sl" data-layer="uml">master-data</span> ProductLifeCycle

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. It groups the individual ProductLifeCycleStage records that enumerate each stage (raw material extraction, manufacturing, distribution, use, end-of-life) and anchors the ProductLifeCycleFootprint records that hold stage-level emission quantities. Defining the life cycle as a structured entity rather than a string field enables stage-level comparisons across products and supply-chain tiers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycle record, referenced by ProductLifeCycleStage and ProductLifeCycleFootprint records to associate stage definitions with their footprint contributions. |
| product_id | String |  | Foreign key to the Product whose life-cycle system boundary this record defines, linking the life-cycle structure to the product master record. |
| name | String |  | A descriptive label for this life-cycle scope definition, such as Cradle-to-gate, Cradle-to-grave, or Gate-to-gate, indicating the extent of the boundary and enabling high-level comparison across products. |
| description | String |  | A narrative description of the system boundary, listing the included and excluded life-cycle stages, the functional unit, and any allocation decisions that apply to the life cycle as a whole. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityFlow](EmissionActivityFlow.md) |
| Association |  | [Product](Product.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Product](Product.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;EmissionActivityFlow&quot;,&quot;fullName&quot;:&quot;EmissionActivityFlow&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityFlow.html&quot;},{&quot;id&quot;:&quot;e810&quot;,&quot;label&quot;:&quot;Product&quot;,&quot;fullName&quot;:&quot;Product&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Product.html&quot;},{&quot;id&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;ProductLifeCycle&quot;,&quot;fullName&quot;:&quot;ProductLifeCycle&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;EmissionActivity&quot;,&quot;fullName&quot;:&quot;EmissionActivity&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionActivity.html&quot;},{&quot;id&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;EnvironmentalProductDec…&quot;,&quot;fullName&quot;:&quot;EnvironmentalProductDeclaration&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EnvironmentalProductDeclaration.html&quot;},{&quot;id&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;CentralProductClassific…&quot;,&quot;fullName&quot;:&quot;CentralProductClassificationCode&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;CentralProductClassificationCode.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c800&quot;,&quot;source&quot;:&quot;e813&quot;,&quot;target&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c836&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c797&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c799&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c801&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c805&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c863&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*