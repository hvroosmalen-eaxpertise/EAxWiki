---
ea_id: 739
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 96ac0e12
---

# <span class="sl" data-layer="uml">reference-data</span> IndustrySectorType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="739" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="739" data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection. Common sector classification systems include NACE (European), SIC (US), NAICS (North American), and ISIC (International). A self-referential parent relationship allows the construction of multi-level sector hierarchies, enabling both high-level sector summaries and granular sub-sector analysis. The code attribute supports direct alignment with external classification system codes, enabling automated crosswalk with reference datasets.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the IndustrySectorType record. It serves as the primary key and is also referenced in the self-referential parent association to build sector hierarchies. It must be globally unique across all sector classification records.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="7b7fa9d1" data-kind="attribute" data-el-id="739" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The standardised industry sector classification code as defined by the relevant classification system (e.g., NAICS code "325" for "Chemical Manufacturing" or "32511" for "Petrochemical Manufacturing"). The code enables automated alignment with external databases, emission factor repositories, and regulatory reporting schemas that reference industry sectors by code.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="04cb7c7b" data-kind="attribute" data-el-id="739" data-attr-name="code" data-attr-type="String" data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The human-readable name of the industry sector, such as "Oil and Gas" or "Iron and Steel". The name provides a user-friendly label for display in reports and selection interfaces and should match the official label used by the classification system referenced by the code attribute.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="5ad95a62" data-kind="attribute" data-el-id="739" data-attr-name="name" data-attr-type="String" data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the parent IndustrySectorType record in the sector hierarchy, enabling multi-level classifications such as Chemical Manufacturing (325) as the parent of Petrochemical Manufacturing (32511). This attribute must not create cycles; implementations should enforce acyclicity.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="0e2a329a" data-kind="attribute" data-el-id="739" data-attr-name="parent_id" data-attr-type="String" data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="739" data-tag-name="description" data-tag-value="IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection." data-file-path="Organisation/IndustrySectorType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [IndustrySectorType](IndustrySectorType.md) |
| Association |  | [OrganizationIndustrySector](OrganizationIndustrySector.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;OrganizationIndustrySec…&quot;,&quot;fullName&quot;:&quot;OrganizationIndustrySector&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationIndustrySector.html&quot;},{&quot;id&quot;:&quot;e739&quot;,&quot;label&quot;:&quot;IndustrySectorType&quot;,&quot;fullName&quot;:&quot;IndustrySectorType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c901&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c902&quot;,&quot;source&quot;:&quot;e739&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c900&quot;,&quot;source&quot;:&quot;e739&quot;,&quot;target&quot;:&quot;e739&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:16*