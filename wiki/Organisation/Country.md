---
ea_id: 745
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: f11fbdf0
---

# <span class="sl" data-layer="uml">reference-data</span> Country

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="745" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="745" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name. The Country entity serves as a reference point for linking addresses and geopolitical entities to a standardised country identifier, enabling geographic analysis and cross-border data alignment across the model. Country is modelled as a subtype of GeopoliticalEntity in the standard's conceptual model, distinguished by GeopoliticalEntityType, but is represented as a dedicated reference entity here to support direct foreign key relationships from Address and other entities that specifically require a country reference.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Country record. It serves as the primary key referenced by Address, GeopoliticalEntity, and other entities that require an unambiguous country context.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="ec585265" data-kind="attribute" data-el-id="745" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>iso_alpha_3_code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The three-letter ISO 3166-1 alpha-3 country code, such as "NLD" for the Netherlands or "DEU" for Germany. This code is the primary standardised identifier used in international data exchange, regulatory reporting, and cross-border emissions factor selection.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="79577d03" data-kind="attribute" data-el-id="745" data-attr-name="iso_alpha_3_code" data-attr-type="String" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The official name of the country or territory as published in the ISO 3166-1 country list, such as "Kingdom of the Netherlands" or "Federal Republic of Germany". The name is used in user interfaces and reports to present a human-readable country reference alongside the ISO code.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="82e0625a" data-kind="attribute" data-el-id="745" data-attr-name="name" data-attr-type="String" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>geopolitical_entity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key identifying the GeopoliticalEntityType that classifies this country record, typically "Country". This attribute supports the integration of Country with the broader GeopoliticalEntity hierarchy.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="2603fe3c" data-kind="attribute" data-el-id="745" data-attr-name="geopolitical_entity_type_id" data-attr-type="String" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key linking this Country to a corresponding Location record in the location hierarchy, enabling the country to participate in the location parent–child structure used across the model.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="61f44181" data-kind="attribute" data-el-id="745" data-attr-name="location_id" data-attr-type="String" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>A foreign key referencing the parent Location record in the geographic hierarchy, used to build multi-level geographic structures where a country is nested within a larger region or grouping.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="fab77e49" data-kind="attribute" data-el-id="745" data-attr-name="parent_location_id" data-attr-type="String" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The date and time from which this country record is valid, in ISO 8601 format. This attribute supports the tracking of country code changes or the creation of new countries over time, preserving a historical record of geographic attribution.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="cfbc7792" data-kind="attribute" data-el-id="745" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="745" data-tag-name="description" data-tag-value="Country represents an area of land or territory of a nation or state, identified by its ISO alpha-3 country code and geopolitical entity name." data-file-path="Organisation/Country.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [GeopoliticalEntity](../Facilities/GeopoliticalEntity.html) |
| Association |  | [Address](Address.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="745"></div>

---

*Generated: 2026-08-03 08:46:17*