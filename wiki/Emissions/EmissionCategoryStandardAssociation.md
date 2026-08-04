---
ea_id: 787
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 69803db2
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionCategoryStandardAssociation

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="787" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="787" data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard. It enables the model to represent the fact that GHG Protocol, ISO 14064-1, and ESRS each define overlapping but not identical category taxonomies, and it allows emission statements to be classified under the correct category hierarchy for each applicable standard without duplicating the statement records themselves.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this association record, used as a stable reference when auditing which standards recognise a given emission category classification.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="10ca02d7" data-kind="attribute" data-el-id="787" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_category_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionActivityCategory whose recognition under a specific standard is being recorded by this association.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="5131f311" data-kind="attribute" data-el-id="787" data-attr-name="emission_activity_category_id" data-attr-type="String" data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>standard_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the Standard that recognises this emission activity category, enabling category-to-standard traceability for multi-framework reporting.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="cffc1849" data-kind="attribute" data-el-id="787" data-attr-name="standard_id" data-attr-type="String" data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>category_code_per_standard</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The code or identifier that the referenced Standard uses for this category, which may differ from the category own identifier, supporting standards-specific labelling in disclosures.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="b91974af" data-kind="attribute" data-el-id="787" data-attr-name="category_code_per_standard" data-attr-type="String" data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="787" data-tag-name="description" data-tag-value="EmissionCategoryStandardAssociation is an intersection entity that records which emission category classifications are recognised by a particular Standard." data-file-path="Emissions/EmissionCategoryStandardAssociation.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Standard](../Organisation/Standard.html) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="787"></div>

---

*Generated: 2026-08-04 11:36:53*