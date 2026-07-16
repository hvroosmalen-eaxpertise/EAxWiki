---
ea_id: 797
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: d55ef836
---

# <span class="sl" data-layer="uml">reference-data</span> PhysicalQuantityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="797" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="797" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area. It enables dimensional analysis validation in calculation models, ensuring for example that an emission factor expressed in kgCO2/kWh is applied to an activity parameter expressed in an energy unit rather than a mass or distance unit. This prevents a class of calculation errors that are otherwise difficult to detect programmatically.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this PhysicalQuantityType record, referenced by UnitOfMeasure records to establish which physical dimension a unit measures.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="07c5df03" data-kind="attribute" data-el-id="797" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The standard name for the physical quantity, such as Mass, Energy, Thermodynamic Temperature, or Volume, used in validation error messages and unit catalogue documentation.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="194416c4" data-kind="attribute" data-el-id="797" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of the physical quantity, its SI base dimensions, and any special considerations relevant to GHG accounting, such as the distinction between higher and lower calorific values for energy quantities.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="e2107a10" data-kind="attribute" data-el-id="797" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="797" data-tag-name="description" data-tag-value="PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area." data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="797"></div>

---

*Generated: 2026-07-16 21:11:27*