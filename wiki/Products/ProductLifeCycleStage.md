---
ea_id: 816
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 5d47fbd9
---

# <span class="sl" data-layer="uml">master-data</span> ProductLifeCycleStage

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life. Stages are defined at the vocabulary level as reference data so that all products in the model share a consistent set of stage labels, enabling stage-level emission comparisons across products, industries, and reporting frameworks including ISO 14040/14044 and ESRS E1.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycleStage record, referenced by ProductLifeCycleFootprint records to attribute stage-level carbon contributions within a product life-cycle boundary. |
| name | String |  | The standard label for the life-cycle stage, such as Cradle (raw material extraction), Gate (end of manufacturing), Use, or End-of-Life, used in footprint breakdowns and disclosure tables. |
| description | String |  | A description of the processes, activities, and geographic scope typically included in this stage, providing guidance for consistent stage boundary assignment across different products and assessment practitioners. |
| stage_order | String |  | The default sequential order of this stage within a cradle-to-grave life cycle, used to present stage-level footprint data in a logical, temporally consistent sequence in reports and dashboards. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycleFootprint](ProductLifeCycleFootprint.md) |
| Association |  | [ProductLifeCycleFootprint](ProductLifeCycleFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;ProductLifeCycleFootpri…&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycleFootprint.html&quot;},{&quot;id&quot;:&quot;e816&quot;,&quot;label&quot;:&quot;ProductLifeCycleStage&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleStage&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprint.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c793&quot;,&quot;source&quot;:&quot;e816&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c802&quot;,&quot;source&quot;:&quot;e816&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c803&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*