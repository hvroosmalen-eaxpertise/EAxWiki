---
ea_id: 795
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 1146d900
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionComponentCategoryGroup

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="795" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionComponentCategoryGroup.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="795" data-file-path="Emissions/EmissionComponentCategoryGroup.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures. Typical groups include Carbon dioxide, Methane and nitrous oxide, Fluorinated gases (F-gases), Biogenic CO2, and Aggregate CO2-equivalent. This grouping is required by several reporting frameworks that present emission totals at the gas-group level in addition to individual gas breakdowns.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionComponentCategoryGroup record, referenced by EmissionComponentCategory records via emission_component_category_group_id to establish the roll-up hierarchy.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="1106fb03" data-kind="attribute" data-el-id="795" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionComponentCategoryGroup.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The label for this gas-category group, such as Fluorinated gases or Biogenic carbon flows, used in summary tables and disclosure headings.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="65438c00" data-kind="attribute" data-el-id="795" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategoryGroup.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of which greenhouse gases are included in this group, the rationale for the grouping (e.g. regulatory definition or characterisation method), and how the group total is aggregated.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="ccbc4c69" data-kind="attribute" data-el-id="795" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategoryGroup.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="795" data-tag-name="description" data-tag-value="EmissionComponentCategoryGroup is a reference entity that provides a higher-level grouping of EmissionComponentCategory records, enabling roll-up from individual gas categories to broad gas families for summary disclosures." data-file-path="Emissions/EmissionComponentCategoryGroup.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e792&quot;,&quot;label&quot;:&quot;EmissionComponentCatego…&quot;,&quot;fullName&quot;:&quot;EmissionComponentCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentCategory.html&quot;},{&quot;id&quot;:&quot;e795&quot;,&quot;label&quot;:&quot;EmissionComponentCatego…&quot;,&quot;fullName&quot;:&quot;EmissionComponentCategoryGroup&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;EmissionComponent&quot;,&quot;fullName&quot;:&quot;EmissionComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponent.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c809&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e795&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c831&quot;,&quot;source&quot;:&quot;e795&quot;,&quot;target&quot;:&quot;e792&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c856&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c866&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*