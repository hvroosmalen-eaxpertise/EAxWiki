---
ea_id: 816
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 5d47fbd9
---

# <span class="sl" data-layer="uml">master-data</span> ProductLifeCycleStage

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="816" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="816" data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life. Stages are defined at the vocabulary level as reference data so that all products in the model share a consistent set of stage labels, enabling stage-level emission comparisons across products, industries, and reporting frameworks including ISO 14040/14044 and ESRS E1.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ProductLifeCycleStage record, referenced by ProductLifeCycleFootprint records to attribute stage-level carbon contributions within a product life-cycle boundary.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="b3e67b7a" data-kind="attribute" data-el-id="816" data-attr-name="id" data-attr-type="Key" data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The standard label for the life-cycle stage, such as Cradle (raw material extraction), Gate (end of manufacturing), Use, or End-of-Life, used in footprint breakdowns and disclosure tables.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="adada9fa" data-kind="attribute" data-el-id="816" data-attr-name="name" data-attr-type="String" data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of the processes, activities, and geographic scope typically included in this stage, providing guidance for consistent stage boundary assignment across different products and assessment practitioners.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="8c289194" data-kind="attribute" data-el-id="816" data-attr-name="description" data-attr-type="String" data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>stage_order</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The default sequential order of this stage within a cradle-to-grave life cycle, used to present stage-level footprint data in a logical, temporally consistent sequence in reports and dashboards.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="42dd928b" data-kind="attribute" data-el-id="816" data-attr-name="stage_order" data-attr-type="String" data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="816" data-tag-name="description" data-tag-value="ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life." data-file-path="Products/ProductLifeCycleStage.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

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

*Generated: 2026-07-01 14:13:17*