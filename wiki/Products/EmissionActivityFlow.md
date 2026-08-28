---
ea_id: 814
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 29dada76
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityFlow

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="814" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Products](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="814" data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process. It enables the model to disaggregate a product carbon footprint into its contributing emission activities, providing the activity-level transparency required for hotspot analysis, supplier engagement, and data quality improvement roadmaps.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionActivityFlow record, used to navigate between the product life-cycle domain and the emission activity domain when performing stage-level attribution analysis.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="09b0300a" data-kind="attribute" data-el-id="814" data-attr-name="id" data-attr-type="Key" data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_life_cycle_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the ProductLifeCycle that this flow belongs to, placing the emission activity within the context of a defined system boundary.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="afaa0176" data-kind="attribute" data-el-id="814" data-attr-name="product_life_cycle_id" data-attr-type="String" data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionActivity whose GHG contribution to the product life cycle is represented by this flow record.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="9ca0161f" data-kind="attribute" data-el-id="814" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>flow_quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The quantity of the activity that occurs within this life-cycle stage per declared unit of the product, expressed in the activity parameter unit, used as the input to the corresponding emission calculation.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="415c4067" data-kind="attribute" data-el-id="814" data-attr-name="flow_quantity" data-attr-type="String" data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>flow_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>Foreign key to the UnitOfMeasure in which the flow_quantity is expressed, such as kWh of electricity consumed or km of transport per declared unit, enabling calculation engines to apply the correct emission factor.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="827cbf3b" data-kind="attribute" data-el-id="814" data-attr-name="flow_unit_of_measure_id" data-attr-type="String" data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="814" data-tag-name="description" data-tag-value="EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process." data-file-path="Products/EmissionActivityFlow.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycle](ProductLifeCycle.html) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Products.html" class="diagram-thumb"><img src="diagrams/Products.png" alt="Products" loading="lazy"><span>Products</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductLifeCycle](ProductLifeCycle.html) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="814"></div>
