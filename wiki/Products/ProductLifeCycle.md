---
ea_id: 813
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: f23c1c60
---

# <span class="sl" data-layer="uml">master-data</span> ProductLifeCycle

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="813" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Products](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="813" data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. It groups the individual ProductLifeCycleStage records that enumerate each stage (raw material extraction, manufacturing, distribution, use, end-of-life) and anchors the ProductLifeCycleFootprint records that hold stage-level emission quantities. Defining the life cycle as a structured entity rather than a string field enables stage-level comparisons across products and supply-chain tiers.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ProductLifeCycle record, referenced by ProductLifeCycleStage and ProductLifeCycleFootprint records to associate stage definitions with their footprint contributions.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="0db50edd" data-kind="attribute" data-el-id="813" data-attr-name="id" data-attr-type="Key" data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the Product whose life-cycle system boundary this record defines, linking the life-cycle structure to the product master record.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="47edd031" data-kind="attribute" data-el-id="813" data-attr-name="product_id" data-attr-type="String" data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A descriptive label for this life-cycle scope definition, such as Cradle-to-gate, Cradle-to-grave, or Gate-to-gate, indicating the extent of the boundary and enabling high-level comparison across products.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="4e219f28" data-kind="attribute" data-el-id="813" data-attr-name="name" data-attr-type="String" data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A narrative description of the system boundary, listing the included and excluded life-cycle stages, the functional unit, and any allocation decisions that apply to the life cycle as a whole.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="c0863ace" data-kind="attribute" data-el-id="813" data-attr-name="description" data-attr-type="String" data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="813" data-tag-name="description" data-tag-value="ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope." data-file-path="Products/ProductLifeCycle.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityFlow](EmissionActivityFlow.html) |
| Association |  | [Product](Product.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Products.html" class="diagram-thumb"><img src="diagrams/Products.png" alt="Products" loading="lazy"><span>Products</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Product](Product.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="813"></div>
