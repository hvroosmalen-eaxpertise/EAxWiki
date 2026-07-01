---
ea_id: 820
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 8b6de569
---

# <span class="sl" data-layer="uml">master-data</span> ProductCategoryRule

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="820" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/ProductCategoryRule.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="820" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product category. PCRs constrain the methodological choices practitioners can make when calculating a PCF, specifying system boundary, allocation rules, data quality requirements, and declared unit definitions for the product type. Linking a PCR to a ProductCarbonFootprint provides essential methodological context for comparison across competing products.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ProductCategoryRule record, referenced by ProductCarbonFootprint records via product_or_sector_specific_rules to indicate which PCR governed the calculation methodology.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="9752b1cd" data-kind="attribute" data-el-id="820" data-attr-name="id" data-attr-type="Key" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The title of the PCR document, such as PCR 2019:14 Construction products and construction services v1.3.4, used in citations and footprint disclosure notes.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="69af6a54" data-kind="attribute" data-el-id="820" data-attr-name="name" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_category</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The product category or CPC code range to which this PCR applies, indicating which types of product should apply the rule when declaring a carbon footprint or EPD.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="3c14e6ba" data-kind="attribute" data-el-id="820" data-attr-name="product_category" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>programme_operator</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The EPD programme operator or standards body that published the PCR, such as EPD International or ecoinvent Association, identifying the authority that must approve deviations from the rule.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="11d66f92" data-kind="attribute" data-el-id="820" data-attr-name="programme_operator" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>version</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The version or edition of the PCR document, used to ensure that footprint declarations reference the correct version in force during the assessment period and to manage PCR updates.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="16c83770" data-kind="attribute" data-el-id="820" data-attr-name="version" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_from</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The date from which this PCR version is applicable, used to select the correct rule for assessments conducted during a given period.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="70bfadee" data-kind="attribute" data-el-id="820" data-attr-name="valid_from" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_to</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The date after which this PCR version is superseded, triggering a requirement to update assessments using a newer version.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="aa8ac777" data-kind="attribute" data-el-id="820" data-attr-name="valid_to" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>url</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>A persistent URL to the PCR document, enabling automated retrieval and supporting auditability of the methodological choices made in the footprint calculation.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="2cd57388" data-kind="attribute" data-el-id="820" data-attr-name="url" data-attr-type="String" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product catego</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="820" data-tag-name="description" data-tag-value="ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product catego" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;ProductCategoryRule&quot;,&quot;fullName&quot;:&quot;ProductCategoryRule&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Location&quot;,&quot;fullName&quot;:&quot;Location&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/Location.html&quot;},{&quot;id&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;ProductFootprintDataQua…&quot;,&quot;fullName&quot;:&quot;ProductFootprintDataQualityIndicator&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprintDataQualityIndicator.html&quot;},{&quot;id&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;ProductCarbonFootprintF…&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprintFactorSource&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprintFactorSource.html&quot;},{&quot;id&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;ProductLifeCycleFootpri…&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycleFootprint.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/UnitOfMeasure.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c792&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c795&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c796&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c798&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c803&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c804&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c881&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*