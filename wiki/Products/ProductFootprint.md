---
ea_id: 811
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 24fe1e50
---

# <span class="sl" data-layer="uml">work-product-component</span> ProductFootprint

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="811" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Products](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="811" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration. It holds the declaration metadata including the status, version history, validity period, and preceding footprint reference, grouping one or more ProductCarbonFootprint records that carry the quantified GHG data. The ProductFootprint entity aligns with the PACT technical specification version 2 data model, where a footprint declaration can be updated or superseded while retaining the historical chain of prior versions.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ProductFootprint envelope record, typically a UUID, used to reference the footprint in data exchange messages and to link the footprint to its superseding or preceding records.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="dc0cea89" data-kind="attribute" data-el-id="811" data-attr-name="id" data-attr-type="Key" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>product_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the Product that this footprint declaration covers, linking the quantified carbon data to its product master record.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="ba522a1d" data-kind="attribute" data-el-id="811" data-attr-name="product_id" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>spec_version</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The version of the PACT technical specification (or other applicable framework) under which this footprint was compiled, such as 2.0.0 or 2.1.0, enabling receivers to apply the correct validation rules.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="0d1e3c5f" data-kind="attribute" data-el-id="811" data-attr-name="spec_version" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>version</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A sequential integer counter indicating the revision number of this footprint declaration, incremented each time a correction or update is published for the same product and scope.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="9a7fc51b" data-kind="attribute" data-el-id="811" data-attr-name="version" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>status</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The lifecycle status of the footprint declaration, drawn from PACT values: Active, Deprecated, or an implementation-specific value, used to determine whether the declaration is current or superseded.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="ab1c5937" data-kind="attribute" data-el-id="811" data-attr-name="status" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>status_comment</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>A free-text explanation of the current status, particularly when the status is Deprecated or Inactive, describing the reason for the change and any corrective action taken.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="5116942c" data-kind="attribute" data-el-id="811" data-attr-name="status_comment" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>validity_period_start</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The first date for which this footprint declaration is valid and may be used by receiving parties, establishing when the declaration becomes effective.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="9b1350f0" data-kind="attribute" data-el-id="811" data-attr-name="validity_period_start" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>validity_period_end</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The last date for which this footprint declaration is valid; a null value indicates no defined expiry, while a populated date requires the receiver to obtain a refreshed declaration after this date.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="5975637f" data-kind="attribute" data-el-id="811" data-attr-name="validity_period_end" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>preceding_product_footprint_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>A self-referential foreign key pointing to the ProductFootprint record that this declaration supersedes or corrects, enabling receivers to navigate the revision history and always identify the latest active version.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="e0b776de" data-kind="attribute" data-el-id="811" data-attr-name="preceding_product_footprint_id" data-attr-type="String" data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="811" data-tag-name="description" data-tag-value="ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration." data-file-path="Products/ProductFootprint.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductFootprint](ProductFootprint.html) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.html) |
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
| Association |  | [ProductFootprint](ProductFootprint.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="811"></div>
