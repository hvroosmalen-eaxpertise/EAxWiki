---
ea_id: 810
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 5d96fe61
---

# <span class="sl" data-layer="uml">master-data</span> Product

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="810" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/Product.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="810" data-file-path="Products/Product.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared. It acts as the anchor for all product-level sustainability data, linking the product to its classification codes (CentralProductClassificationCode), its product carbon footprint records (ProductFootprint), and the life-cycle stages used to structure its footprint assessment. In a supply-chain context, Product enables data exchange between buyer and supplier organisations under the WBCSD PACT framework.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this Product record, referenced by ProductFootprint, EmissionActivityFlow, and classification code association records.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="0232ff59" data-kind="attribute" data-el-id="810" data-attr-name="id" data-attr-type="Key" data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The human-readable commercial or technical name of the product, used in disclosures, data exchanges, and product catalogue displays.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="4a145d88" data-kind="attribute" data-el-id="810" data-attr-name="name" data-attr-type="String" data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A free-text description of the product, including its functional unit, primary materials, and typical use context, providing the background needed to interpret its carbon footprint data.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="ec83e853" data-kind="attribute" data-el-id="810" data-attr-name="description" data-attr-type="String" data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_id_type</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The type of product identifier used in the product_id field, such as UUID, EAN, GTIN-13, or internal SKU code, enabling correct interpretation of the identifier by receiving systems.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="1fc40e2e" data-kind="attribute" data-el-id="810" data-attr-name="product_id_type" data-attr-type="String" data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The specific identifier for this product in the system indicated by product_id_type, such as a GTIN barcode or internal stock-keeping unit number, supporting cross-system product matching.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="fe230ec7" data-kind="attribute" data-el-id="810" data-attr-name="product_id" data-attr-type="String" data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>Foreign key to the UnitOfMeasure that defines the declared unit (functional unit) for this product, such as 1 kg, 1 piece, or 1 kWh, establishing the denominator for all carbon intensity expressions.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="23183f74" data-kind="attribute" data-el-id="810" data-attr-name="unit_of_measure_id" data-attr-type="String" data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="810" data-tag-name="description" data-tag-value="Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared." data-file-path="Products/Product.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EnvironmentalProductDeclaration](EnvironmentalProductDeclaration.md) |
| Association |  | [CentralProductClassificationCode](CentralProductClassificationCode.md) |
| Association |  | [ProductLifeCycle](ProductLifeCycle.md) |
| Association |  | [ProductFootprint](ProductFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;EnvironmentalProductDec…&quot;,&quot;fullName&quot;:&quot;EnvironmentalProductDeclaration&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EnvironmentalProductDeclaration.html&quot;},{&quot;id&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;CentralProductClassific…&quot;,&quot;fullName&quot;:&quot;CentralProductClassificationCode&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;CentralProductClassificationCode.html&quot;},{&quot;id&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;ProductLifeCycle&quot;,&quot;fullName&quot;:&quot;ProductLifeCycle&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycle.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;},{&quot;id&quot;:&quot;e810&quot;,&quot;label&quot;:&quot;Product&quot;,&quot;fullName&quot;:&quot;Product&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;EmissionActivityFlow&quot;,&quot;fullName&quot;:&quot;EmissionActivityFlow&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityFlow.html&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprint.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c797&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c799&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c800&quot;,&quot;source&quot;:&quot;e813&quot;,&quot;target&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c801&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c804&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c805&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*