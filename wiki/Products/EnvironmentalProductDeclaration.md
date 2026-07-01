---
ea_id: 819
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7fd275b9
---

# <span class="sl" data-layer="uml">work-product-component</span> EnvironmentalProductDeclaration

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="819" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="819" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EnvironmentalProductDeclaration is a work-product-component that represents a formal Type III environmental declaration published under a recognised EPD programme, such as those governed by ISO 14025. An EPD discloses the life-cycle environmental impacts of a product across multiple impact categories, of which the global warming potential (GWP) impact category is directly comparable to a product carbon footprint. Linking an EPD record to a product enables organisations to reference externally verified declarations alongside internally calculated footprints in their supply-chain disclosures.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EnvironmentalProductDeclaration record, referenced by the Product and ProductFootprint records that this EPD supports or supplements.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="98c777f1" data-kind="attribute" data-el-id="819" data-attr-name="id" data-attr-type="Key" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the Product that this EPD covers, linking the externally verified declaration to the product master data record.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="b06b2dc5" data-kind="attribute" data-el-id="819" data-attr-name="product_id" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>programme_operator</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The name of the EPD programme operator under whose rules and verification scheme this EPD was produced and registered, such as EPD International, Institut Bauen und Umwelt (IBU), or The Norwegian EPD Foundation.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="1245b226" data-kind="attribute" data-el-id="819" data-attr-name="programme_operator" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>registration_number</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The unique registration number assigned by the programme operator to this EPD, providing a stable external reference for document retrieval and verification.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="6d7664cc" data-kind="attribute" data-el-id="819" data-attr-name="registration_number" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>publication_date</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The date on which the EPD was published by the programme operator, establishing the document date used for temporal representativeness assessments.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="48c8726d" data-kind="attribute" data-el-id="819" data-attr-name="publication_date" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_until</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The expiry date of the EPD, after which a re-verification or update is required, used to flag outdated declarations in data quality checks.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="8f08bcf4" data-kind="attribute" data-el-id="819" data-attr-name="valid_until" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_value</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The global warming potential impact result from the EPD, expressed in kgCO2e per declared unit (A1-A5 or full cradle-to-grave depending on the product category rule), directly comparable to the ProductCarbonFootprint pcf_excluding_biogenic value.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="69562f35" data-kind="attribute" data-el-id="819" data-attr-name="gwp_value" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>Foreign key to the UnitOfMeasure in which the gwp_value is expressed, confirming the functional unit and enabling correct comparison with ProductCarbonFootprint records.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="196577bd" data-kind="attribute" data-el-id="819" data-attr-name="gwp_unit_of_measure_id" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>url</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>A persistent URL to the published EPD document in the programme operator registry, enabling automated retrieval and verification of the declaration.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="69c92f17" data-kind="attribute" data-el-id="819" data-attr-name="url" data-attr-type="String" data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EnvironmentalProductDeclaration is a work-product-component that represents a formal Type III environmental declaration published under a recognised EPD programme, such as those governed by ISO 14025.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="819" data-tag-name="description" data-tag-value="EnvironmentalProductDeclaration is a work-product-component that represents a formal Type III environmental declaration published under a recognised EPD programme, such as those governed by ISO 14025." data-file-path="Products/EnvironmentalProductDeclaration.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e810&quot;,&quot;label&quot;:&quot;Product&quot;,&quot;fullName&quot;:&quot;Product&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Product.html&quot;},{&quot;id&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;EnvironmentalProductDec…&quot;,&quot;fullName&quot;:&quot;EnvironmentalProductDeclaration&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;CentralProductClassific…&quot;,&quot;fullName&quot;:&quot;CentralProductClassificationCode&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;CentralProductClassificationCode.html&quot;},{&quot;id&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;ProductLifeCycle&quot;,&quot;fullName&quot;:&quot;ProductLifeCycle&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycle.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c797&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c799&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c801&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c805&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*