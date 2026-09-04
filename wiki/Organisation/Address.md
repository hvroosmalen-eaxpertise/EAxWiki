---
ea_id: 737
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: de6729a6
---

# <span class="sl" data-layer="uml">master-data</span> Address

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="737" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="737" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Address captures the postal or physical address associated with a contact person, facility, or organisational unit. A structured address representation is preferred over a free-text field because it enables geographic analysis, regulatory jurisdiction mapping, and integration with postal validation services. The Address entity is deliberately kept simple, covering the most commonly required address components, and is linked to a Location entity that provides ISO country coding. A single contact or organisation may have multiple address records representing, for example, registered, trading, and correspondence addresses.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Address record. It serves as the primary key referenced by ContactPerson and other entities that require a postal or physical address. It must be unique across all address records in the system and must not be reused after deletion.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="215d0555" data-kind="attribute" data-el-id="737" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>line1_text</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The first line of the street address, typically containing the building number and street name, or a named building. This is the primary navigational element of the address and is required for postal delivery purposes. It is rendered as the first address line in formatted address displays and on correspondence.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="767afa1b" data-kind="attribute" data-el-id="737" data-attr-name="line1_text" data-attr-type="String" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>line2_text</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The second line of the address, used for suite numbers, floor designations, care-of information, or other supplementary addressing details that do not fit on the first line. This field is optional and should be left blank when not required rather than populated with placeholder text.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="a9981500" data-kind="attribute" data-el-id="737" data-attr-name="line2_text" data-attr-type="String" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>city</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The city, town, or locality component of the address. It is used in geographic aggregations, jurisdiction mapping, and together with postal_code provides sufficient information to uniquely identify the delivery locality in most postal systems.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="aa81e92a" data-kind="attribute" data-el-id="737" data-attr-name="city" data-attr-type="String" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>postal_code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The postal or ZIP code associated with the address. The format varies by country and is validated against country-specific rules where possible. It is used for geographic analysis, logistics routing, and regulatory jurisdiction determination.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="4998b9d6" data-kind="attribute" data-el-id="737" data-attr-name="postal_code" data-attr-type="String" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>geopolitical_entity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>A foreign key referencing the GeopoliticalEntityType that classifies the geographic entity in which this address resides. This supports geographic analysis and regulatory jurisdiction mapping for emission reporting purposes.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="e2f2fbeb" data-kind="attribute" data-el-id="737" data-attr-name="geopolitical_entity_type_id" data-attr-type="String" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>A foreign key identifying the Country or Location entity associated with this address. This attribute links the address to the standardised geographic reference hierarchy and enables alignment with country-level emission factor datasets and regulatory jurisdiction rules.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="26aef6ae" data-kind="attribute" data-el-id="737" data-attr-name="location_id" data-attr-type="String" data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

<details class="ea-section" data-ea-section-id="tagged-values" markdown="1">
<summary><h2 id="tagged-values">Tagged Values</h2></summary>

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Address captures the postal or physical address associated with a contact person, facility, or organisational unit.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="737" data-tag-name="description" data-tag-value="Address captures the postal or physical address associated with a contact person, facility, or organisational unit." data-file-path="Organisation/Address.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

</details>

<details class="ea-section" data-ea-section-id="relationships" markdown="1">
<summary><h2 id="relationships">Relationships</h2></summary>

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Country](Country.html) |
| Association |  | [OrganizationAddress](OrganizationAddress.html) |

</details>

## Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

<details class="ea-section" data-ea-section-id="referenced-by" markdown="1">
<summary><h2 id="referenced-by">Referenced By</h2></summary>

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Country](Country.html) |

</details>

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="737"></div>

<!-- ea-element-template:v3 -->
