---
ea_id: 774
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 72809227
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionActivityCategory

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="774" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="774" data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1. For Scope 3, this includes the fifteen upstream and downstream categories such as purchased goods and services, capital goods, fuel and energy-related activities, and so on. The category drives which reporting lines an emission activity contributes to and enables cross-organisation comparability.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionActivityCategory record, such as S3C01 for Scope 3 Category 1, used to group and aggregate emission statements by reporting category.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="f88d6ef6" data-kind="attribute" data-el-id="774" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_scope_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key linking this category to the EmissionScopeType (Scope 1, 2, or 3) to which it belongs, ensuring that emission statements are correctly scope-attributed through their activity category.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e947054e" data-kind="attribute" data-el-id="774" data-attr-name="emission_scope_type_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The standard name for the category, such as "Category 4 - Upstream transportation and distribution", used in disclosures and summary tables.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="d27a5b1d" data-kind="attribute" data-el-id="774" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A normative description of what activities and emission sources are included in this category per the applicable standard, providing boundary guidance for activity classification.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="d292a742" data-kind="attribute" data-el-id="774" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="774" data-tag-name="description" data-tag-value="EmissionActivityCategory is a reference entity that provides the formal taxonomy of GHG emission activity categories as defined by the GHG Protocol or ISO 14064-1." data-file-path="Emissions/EmissionActivityCategory.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCategoryStandardAssociation](EmissionCategoryStandardAssociation.html) |
| Association |  | [EmissionScopeType](EmissionScopeType.html) |
| Association |  | [EmissionActivity](EmissionActivity.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionScopeType](EmissionScopeType.html) |
| Association |  | [EmissionCategoryStandardAssociation](EmissionCategoryStandardAssociation.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="774"></div>

---

*Generated: 2026-08-03 08:46:17*