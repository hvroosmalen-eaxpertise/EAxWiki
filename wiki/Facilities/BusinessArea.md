---
ea_id: 767
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: d536291e
---

# <span class="sl" data-layer="uml">master-data</span> BusinessArea

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="767" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="767" data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>BusinessArea represents a location that is identified and defined by an organisation in which business is done by that organisation. It can be used to refer to the location of an office, a plant, a port, or any other operationally significant geographic area defined from the organisation's operational perspective. Unlike formal geopolitical entities, a business area is a master-data concept owned by the reporting organisation. Temporal validity attributes allow the business area definition to be tracked over time as the organisation's operational structure evolves.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the BusinessArea record. It serves as both the primary key and as the foreign key value for a corresponding Location record.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="30788125" data-kind="attribute" data-el-id="767" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The name of the business area as used operationally by the organisation, such as "EMEA Manufacturing Hub" or "Rotterdam Port Operations". The name should be stable enough to support historical reporting.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="81fcd290" data-kind="attribute" data-el-id="767" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The date and time from which this business area record is valid, in ISO 8601 format. Enables temporal tracking of business area definitions as the organisation's operational structure evolves.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="b2ce6bac" data-kind="attribute" data-el-id="767" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>termination_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The date and time at which this business area record is terminated, in ISO 8601 format. Null if the business area is currently active.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="c32e3d44" data-kind="attribute" data-el-id="767" data-attr-name="termination_datetime" data-attr-type="String" data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key identifying the corresponding Location record in the geographic hierarchy for this business area.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="2063e7f9" data-kind="attribute" data-el-id="767" data-attr-name="location_id" data-attr-type="String" data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>BusinessArea represents a location that is identified and defined by an organisation in which business is done by that organisation.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="767" data-tag-name="description" data-tag-value="BusinessArea represents a location that is identified and defined by an organisation in which business is done by that organisation." data-file-path="Facilities/BusinessArea.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Location](Location.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="767"></div>

---

*Generated: 2026-08-04 11:36:53*