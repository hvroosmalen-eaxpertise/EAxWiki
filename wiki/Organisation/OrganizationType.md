---
ea_id: 736
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 946876ec
---

# <span class="sl" data-layer="uml">reference-data</span> OrganizationType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="736" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/OrganizationType.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="736" data-file-path="Organisation/OrganizationType.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>OrganizationType provides a controlled vocabulary of organisational classifications, such as "Headquarters", "Regional Headquarters", or "Branch". Maintaining a separate entity for organisation types rather than free-text values ensures consistency across the dataset and enables structured filtering and aggregation in reporting. The type classification is relevant for determining applicable regulatory obligations, disclosure requirements, and boundary-setting methodologies. Each OrganizationType may apply to many organisations simultaneously.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the OrganizationType record. It is the primary key used when associating an Organization with its type classification. It must be stable and not recycled once assigned.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="2c9d1e75" data-kind="attribute" data-el-id="736" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/OrganizationType.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The descriptive label for the organisation type, such as "Headquarters", "Regional Headquarters", or "Branch". The name should be drawn from a recognised taxonomy to ensure comparability across organisations and should be rendered consistently in all user-facing interfaces and reports.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="65130762" data-kind="attribute" data-el-id="736" data-attr-name="name" data-attr-type="String" data-file-path="Organisation/OrganizationType.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>OrganizationType provides a controlled vocabulary of organisational classifications, such as &quot;Headquarters&quot;, &quot;Regional Headquarters&quot;, or &quot;Branch&quot;.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="736" data-tag-name="description" data-tag-value="OrganizationType provides a controlled vocabulary of organisational classifications, such as &quot;Headquarters&quot;, &quot;Regional Headquarters&quot;, or &quot;Branch&quot;." data-file-path="Organisation/OrganizationType.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](Organization.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="736"></div>

---

*Generated: 2026-07-31 18:00:34*