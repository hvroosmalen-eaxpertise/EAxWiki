---
ea_id: 784
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 096fdc3a
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionSource

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="784" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionSource.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="784" data-file-path="Emissions/EmissionSource.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. Emission sources provide a more granular technical classification than the EmissionActivityType and are used in calculation model selection and emission factor lookup to narrow the applicable factor set to the correct physical process.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionSource record, used by EmissionActivity and EmissionFactor records to associate a specific physical source with a measurement or factor entry.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="3b87a0ed" data-kind="attribute" data-el-id="784" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionSource.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The human-readable name of the emission source, such as Stationary Combustion Natural Gas or Fugitive Emissions HFC-134a refrigerant, used in data entry forms and technical reports.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="42aef63b" data-kind="attribute" data-el-id="784" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionSource.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A technical description of the emission mechanism and the physical or chemical process by which the GHG is released, supporting correct classification and factor selection by practitioners.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="23205875" data-kind="attribute" data-el-id="784" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionSource.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="784" data-tag-name="description" data-tag-value="EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment." data-file-path="Emissions/EmissionSource.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="784"></div>

---

*Generated: 2026-07-17 16:59:36*