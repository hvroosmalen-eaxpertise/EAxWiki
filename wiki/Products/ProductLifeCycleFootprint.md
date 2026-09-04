---
ea_id: 817
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 83d335c4
---

# <span class="sl" data-layer="uml">work-product-component</span> ProductLifeCycleFootprint

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="817" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Products](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="817" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint. By disaggregating the PCF total into stage-level contributions, this entity enables hotspot analysis, targeted supplier engagement, and the stage-by-stage breakdowns required by ISO 14067 and increasingly expected by frameworks such as ESRS E1 Appendix A.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ProductLifeCycleFootprint record, used to aggregate stage contributions and verify that they sum to the total PCF for the declared unit and system boundary.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="4cca3682" data-kind="attribute" data-el-id="817" data-attr-name="id" data-attr-type="Key" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_carbon_footprint_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the parent ProductCarbonFootprint that this stage-level contribution disaggregates, linking the stage result to its total PCF context.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="a6c81297" data-kind="attribute" data-el-id="817" data-attr-name="product_carbon_footprint_id" data-attr-type="String" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_life_cycle_stage_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the ProductLifeCycleStage that this record covers, identifying which phase of the product life cycle the emission quantity is attributed to.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="8fb7bbb1" data-kind="attribute" data-el-id="817" data-attr-name="product_life_cycle_stage_id" data-attr-type="String" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The GHG emission quantity attributable to this life-cycle stage per declared unit, expressed in the unit referenced by quantity_unit_of_measure_id, typically kgCO2e.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="b377b07d" data-kind="attribute" data-el-id="817" data-attr-name="quantity" data-attr-type="String" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>Foreign key to the UnitOfMeasure in which the stage emission quantity is expressed, enabling correct aggregation with other stage records and comparison with the PCF total.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="3bf37168" data-kind="attribute" data-el-id="817" data-attr-name="quantity_unit_of_measure_id" data-attr-type="String" data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

<details class="ea-section" data-ea-section-id="tagged-values" markdown="1">
<summary><h2 id="tagged-values">Tagged Values</h2></summary>

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="817" data-tag-name="description" data-tag-value="ProductLifeCycleFootprint is a work-product-component that records the GHG emission contribution of a specific ProductLifeCycleStage to the total ProductCarbonFootprint." data-file-path="Products/ProductLifeCycleFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

</details>

<details class="ea-section" data-ea-section-id="relationships" markdown="1">
<summary><h2 id="relationships">Relationships</h2></summary>

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.html) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.html) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.html) |

</details>

## Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Products.html" class="diagram-thumb"><img src="diagrams/Products.png" alt="Products" loading="lazy"><span>Products</span></a>
</div>

<details class="ea-section" data-ea-section-id="referenced-by" markdown="1">
<summary><h2 id="referenced-by">Referenced By</h2></summary>

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.html) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.html) |
| Association |  | [ProductLifeCycleStage](ProductLifeCycleStage.html) |

</details>

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="817"></div>

<!-- ea-element-template:v3 -->
