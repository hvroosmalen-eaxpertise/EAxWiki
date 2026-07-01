---
ea_id: 821
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 9d2360e6
---

# <span class="sl" data-layer="uml">work-product-component</span> ProductFootprintDataQualityIndicator

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="821" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="821" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductFootprintDataQualityIndicator is a work-product-component that records a structured assessment of the data quality dimensions for a specific ProductCarbonFootprint, using the five DQI dimensions defined in the PACT technical specification: completeness, geographical representativeness, reliability, technological representativeness, and temporal representativeness. Each dimension is rated on a scale of 1 (very good) to 3 (acceptable) as a minimum, enabling systematic data quality scoring, disclosure, and improvement prioritisation across PCF datasets.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ProductFootprintDataQualityIndicator record, linked one-to-one with a ProductCarbonFootprint.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="fed33972" data-kind="attribute" data-el-id="821" data-attr-name="id" data-attr-type="Key" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_carbon_footprint_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the ProductCarbonFootprint that this data quality assessment covers, establishing the one-to-one relationship between a PCF and its DQI record.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="906e73e4" data-kind="attribute" data-el-id="821" data-attr-name="product_carbon_footprint_id" data-attr-type="String" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>completeness_data_quality_rating</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A rating from 1 (very good) to 3 (acceptable) for the completeness dimension, reflecting the degree to which all material emission flows and life-cycle stages have been included in the PCF data.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="5d145121" data-kind="attribute" data-el-id="821" data-attr-name="completeness_data_quality_rating" data-attr-type="String" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>geographical_data_quality_rating</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A rating from 1 to 3 for the geographical representativeness dimension, reflecting how closely the emission factor geographies used match the actual production geography of the product.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="50385e1d" data-kind="attribute" data-el-id="821" data-attr-name="geographical_data_quality_rating" data-attr-type="String" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>reliability_data_quality_rating</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A rating from 1 to 3 for the reliability dimension, reflecting the quality and credibility of the data sources and measurement methods used, with 1 indicating measured or verified data and 3 indicating modelled or estimated data.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="eacb650c" data-kind="attribute" data-el-id="821" data-attr-name="reliability_data_quality_rating" data-attr-type="String" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>technological_data_quality_rating</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>A rating from 1 to 3 for the technological representativeness dimension, reflecting whether the emission factors and activity data accurately represent the specific technology used to produce the product.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="8209c046" data-kind="attribute" data-el-id="821" data-attr-name="technological_data_quality_rating" data-attr-type="String" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>temporal_data_quality_rating</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>A rating from 1 to 3 for the temporal representativeness dimension, reflecting whether the emission factors and activity data are current and representative of the period in which the product was manufactured.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="a34e18bd" data-kind="attribute" data-el-id="821" data-attr-name="temporal_data_quality_rating" data-attr-type="String" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ProductFootprintDataQualityIndicator is a work-product-component that records a structured assessment of the data quality dimensions for a specific ProductCarbonFootprint, using the five DQI dimensions defined in the PACT technical specification: complete</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="821" data-tag-name="description" data-tag-value="ProductFootprintDataQualityIndicator is a work-product-component that records a structured assessment of the data quality dimensions for a specific ProductCarbonFootprint, using the five DQI dimensions defined in the PACT technical specification: complete" data-file-path="Products/ProductFootprintDataQualityIndicator.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;ProductFootprintDataQua…&quot;,&quot;fullName&quot;:&quot;ProductFootprintDataQualityIndicator&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Location&quot;,&quot;fullName&quot;:&quot;Location&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/Location.html&quot;},{&quot;id&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;ProductCategoryRule&quot;,&quot;fullName&quot;:&quot;ProductCategoryRule&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCategoryRule.html&quot;},{&quot;id&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;ProductCarbonFootprintF…&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprintFactorSource&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductCarbonFootprintFactorSource.html&quot;},{&quot;id&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;ProductLifeCycleFootpri…&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycleFootprint.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/UnitOfMeasure.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c792&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c795&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c796&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c798&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c803&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c804&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c881&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*