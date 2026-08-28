---
ea_id: 802
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: ac1cc17a
---

# <span class="sl" data-layer="uml">work-product-component</span> ActivityEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="802" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="802" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source. This entity is used, for example, when a shared boiler serves multiple processes and its total emissions must be apportioned across each process emission activity record in proportion to energy consumed, production output, or another allocation base.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this ActivityEmissionAllocation record, used to verify that all allocations from a shared source sum to the total source emission.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="a847b1f2" data-kind="attribute" data-el-id="802" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_statement_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the source EmissionStatement whose total emission quantity is being allocated across activities.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e6f6fb6b" data-kind="attribute" data-el-id="802" data-attr-name="emission_statement_id" data-attr-type="String" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionActivity receiving the allocated portion, identifying which activity-level record absorbs this share.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="7a722bdc" data-kind="attribute" data-el-id="802" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>allocation_percentage</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The percentage share of the source emission quantity assigned to this activity, determined by the allocation base.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="1e8be3fe" data-kind="attribute" data-el-id="802" data-attr-name="allocation_percentage" data-attr-type="String" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>allocated_quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The absolute emission quantity allocated to this activity, equal to the source statement quantity multiplied by the allocation_percentage, for direct use in activity-level totals.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="a056170b" data-kind="attribute" data-el-id="802" data-attr-name="allocated_quantity" data-attr-type="String" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>Foreign key to the UnitOfMeasure for the allocated_quantity, ensuring unit consistency when activity-level allocations are summed.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="db624a42" data-kind="attribute" data-el-id="802" data-attr-name="quantity_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>allocation_base</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>A description of the physical or economic basis used to determine the allocation percentages, such as proportion of energy consumed by each process or floor area fraction.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="7ab345db" data-kind="attribute" data-el-id="802" data-attr-name="allocation_base" data-attr-type="String" data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="802" data-tag-name="description" data-tag-value="ActivityEmissionAllocation is a work-product-component that records the portion of a shared or joint emission activity total emission quantity assigned to a specific EmissionActivity when multiple activities share a common emission source." data-file-path="Emissions/ActivityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.html) |
| Association |  | [EmissionStatement](EmissionStatement.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="802"></div>
