---
ea_id: 815
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 253bacf7
---

# <span class="sl" data-layer="uml">reference-data</span> CentralProductClassificationCode

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="815" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Products](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="815" data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or supply-chain matching purposes. Linking a product to its CPC code supports automated mapping to applicable product category rules, sector-specific emission factors, and ESRS sector-classification requirements.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this CentralProductClassificationCode record, referenced by Product records and ProductCategoryRule records to establish the product classification.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="f9a5e5fc" data-kind="attribute" data-el-id="815" data-attr-name="id" data-attr-type="Key" data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>classification_system</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The name of the classification system from which this code is drawn, such as UN CPC v2.1, NACE Rev.2, or HS 2022, enabling correct code interpretation by receiving systems.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="3d3c0eb8" data-kind="attribute" data-el-id="815" data-attr-name="classification_system" data-attr-type="String" data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The alphanumeric code value within the referenced classification system, such as 27110 for Mining of hard coal under UN CPC, used for cross-system product matching and regulatory filing.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="3771f03d" data-kind="attribute" data-el-id="815" data-attr-name="code" data-attr-type="String" data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The human-readable label assigned to this code within its classification system, used in disclosures and product catalogues alongside the numeric code.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="03aea26f" data-kind="attribute" data-el-id="815" data-attr-name="name" data-attr-type="String" data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A description of the product scope covered by this classification code, supporting practitioners in assigning the correct code to products that span multiple categories.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="64d44696" data-kind="attribute" data-el-id="815" data-attr-name="description" data-attr-type="String" data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or </td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="815" data-tag-name="description" data-tag-value="CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or " data-file-path="Products/CentralProductClassificationCode.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
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

<div id="ea-graph-container" data-focal-id="815"></div>

---

*Generated: 2026-08-03 08:46:17*