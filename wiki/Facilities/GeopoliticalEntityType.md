---
ea_id: 765
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 9b868291
---

# <span class="sl" data-layer="uml">reference-data</span> GeopoliticalEntityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="765" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/GeopoliticalEntityType.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="765" data-file-path="Facilities/GeopoliticalEntityType.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including "Country", "Nature Reserve", "State", and "Region". Classifying geopolitical entities by type enables geographic analysis at multiple administrative levels and supports regulatory jurisdiction mapping in emissions reporting. The code attribute supports alignment with external geopolitical classification systems such as those maintained by the United Nations.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the GeopoliticalEntityType record. It must be stable so that GeopoliticalEntity records that reference it remain valid when the type vocabulary is extended.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="9217882b" data-kind="attribute" data-el-id="765" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/GeopoliticalEntityType.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A short alphanumeric code for the geopolitical entity type, such as "COUNTRY" or "STATE", drawn from an external geopolitical classification system where one exists. The code is provided by the source for Geopolitical Entity Names.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="4044b44b" data-kind="attribute" data-el-id="765" data-attr-name="code" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntityType.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The descriptive label for the geopolitical entity type, representing the types of geographical areas that can be used to identify the location of a facility, such as "Country", "Nature Reserve", "State", or "Region".</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="bbb5c6cb" data-kind="attribute" data-el-id="765" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntityType.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including &quot;Country&quot;, &quot;Nature Reserve&quot;, &quot;State&quot;, and &quot;Region&quot;.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="765" data-tag-name="description" data-tag-value="GeopoliticalEntityType provides the controlled vocabulary used to classify geopolitical entities, with examples including &quot;Country&quot;, &quot;Nature Reserve&quot;, &quot;State&quot;, and &quot;Region&quot;." data-file-path="Facilities/GeopoliticalEntityType.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="765"></div>
