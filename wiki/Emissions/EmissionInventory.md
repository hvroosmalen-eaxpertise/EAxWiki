---
ea_id: 771
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: ce68dc6f
---

# <span class="sl" data-layer="uml">master-data</span> EmissionInventory

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="771" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="771" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period. It groups all EmissionStatement records that together constitute a complete inventory of an organisation's greenhouse gas emissions, organised by scope, source, and boundary. The inventory record holds the metadata required for formal reporting, including the applicable standard, the organisational boundary method, and audit trail information.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system identifier for this EmissionInventory record, used to associate all child emission statements and reporting artefacts with this specific inventory exercise.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="e41a2f84" data-kind="attribute" data-el-id="771" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>organisation_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key linking the inventory to the Organisation that owns and is responsible for the GHG data it contains.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="6e041cea" data-kind="attribute" data-el-id="771" data-attr-name="organisation_id" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>standard_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the Standard record that governs the methodology and boundary rules applied in this inventory, typically the GHG Protocol Corporate Standard or ISO 14064-1.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="958e599e" data-kind="attribute" data-el-id="771" data-attr-name="standard_id" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A human-readable label for the inventory, such as "FY2025 GHG Inventory Scope 1 and 2", used for identification and search in reporting systems.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="b14f7cbe" data-kind="attribute" data-el-id="771" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A free-text narrative describing the scope, boundaries, and methodology choices made for this inventory, providing context for reviewers and auditors.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="3832fb94" data-kind="attribute" data-el-id="771" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>reporting_period_start</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The first day of the reporting period covered by this inventory. Together with reporting_period_end it defines the temporal boundary against which emission activities are assessed.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="91f669a9" data-kind="attribute" data-el-id="771" data-attr-name="reporting_period_start" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>reporting_period_end</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The last day of the reporting period covered by this inventory. Emission activities with dates outside this window are excluded from the inventory totals.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="1dfce473" data-kind="attribute" data-el-id="771" data-attr-name="reporting_period_end" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>organisational_boundary_method</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The consolidation approach used to determine which entities and facilities fall within the inventory boundary, drawn from the GHG Protocol values: equity share, financial control, or operational control.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="a1723522" data-kind="attribute" data-el-id="771" data-attr-name="organisational_boundary_method" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>status</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>The lifecycle status of the inventory record, such as draft, under review, finalised, or audited, used to manage approval workflows and publication control.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="492ffbad" data-kind="attribute" data-el-id="771" data-attr-name="status" data-attr-type="String" data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="771" data-tag-name="description" data-tag-value="EmissionInventory is the top-level work-product-component that represents a single, bounded GHG emissions accounting exercise performed by an Organisation for a defined reporting period." data-file-path="Emissions/EmissionInventory.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionReport](EmissionReport.html) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.html) |
| Association |  | [EmissionStatement](EmissionStatement.html) |
| Association |  | [Organization](../Organisation/Organization.html) |
| Association |  | [Standard](../Organisation/Standard.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](../Organisation/Standard.html) |
| Association |  | [Organization](../Organisation/Organization.html) |
| Association |  | [EmissionReport](EmissionReport.html) |
| Association |  | [EmissionReportingBoundary](EmissionReportingBoundary.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="771"></div>

---

*Generated: 2026-08-03 11:11:53*