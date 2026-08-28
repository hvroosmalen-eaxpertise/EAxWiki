---
ea_id: 764
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 8ef0c98f
---

# <span class="sl" data-layer="uml">work-product-component</span> FacilityEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="764" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="764" data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement. This entity supports scenarios where an emission source or activity is shared between multiple facilities, enabling the portion of the emission attributable to each site to be recorded separately for site-level inventory and regulatory reporting purposes. This is of particular relevance to US EPA Subpart W reporting for Petroleum and Natural Gas Systems.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the FacilityEmissionAllocation record. It must remain stable for audit and regulatory reporting purposes.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="94196364" data-kind="attribute" data-el-id="764" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>percentage</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The fraction of the total emission quantity in the linked EmissionStatement that is allocated to the linked Facility, expressed as a value between 0 and 100. The sum of percentages across all allocations for a given EmissionStatement should equal 100.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e3514ab4" data-kind="attribute" data-el-id="764" data-attr-name="percentage" data-attr-type="String" data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_statement_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the EmissionStatement whose emission quantity is being allocated to the referenced facility.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="2c718776" data-kind="attribute" data-el-id="764" data-attr-name="emission_statement_id" data-attr-type="String" data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key identifying the Facility to which the specified percentage of the emission quantity is allocated.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="13af6153" data-kind="attribute" data-el-id="764" data-attr-name="facility_id" data-attr-type="String" data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="764" data-tag-name="description" data-tag-value="FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement." data-file-path="Facilities/FacilityEmissionAllocation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](../Emissions/EmissionStatement.html) |
| Association |  | [Facility](Facility.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.html) |
| Association |  | [EmissionStatement](../Emissions/EmissionStatement.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="764"></div>
