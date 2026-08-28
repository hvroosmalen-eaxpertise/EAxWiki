---
ea_id: 782
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 0ad1eba6
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionReport

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="782" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="782" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public. It carries the report metadata, including the applicable reporting framework, the consolidation boundary, and the sign-off status. The entity acts as an envelope that groups EmissionReportPeriod records and drives the generation of summary tables and narrative sections in published disclosures.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionReport record, used to link all period detail records, supporting documents, and approval workflow steps to this specific published artefact.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="1901d052" data-kind="attribute" data-el-id="782" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>organisation_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the Organisation that is the reporting entity, identifying whose emissions are being disclosed in this report.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="2ba0d5e2" data-kind="attribute" data-el-id="782" data-attr-name="organisation_id" data-attr-type="String" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>standard_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the Standard or reporting framework under which this report is prepared, such as GHG Protocol, ESRS E1, or CDP Climate Change, governing the structure and content requirements.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="c1c001ba" data-kind="attribute" data-el-id="782" data-attr-name="standard_id" data-attr-type="String" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The human-readable title of the report, used for filing and archive identification.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="3a8433d0" data-kind="attribute" data-el-id="782" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>status</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The approval and publication status of the report, such as draft, under assurance, board approved, or published, supporting governance workflow tracking.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="c2bf3252" data-kind="attribute" data-el-id="782" data-attr-name="status" data-attr-type="String" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>publication_date</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The date on which the report was formally published or filed with the relevant authority, establishing the official record date for regulatory and audit purposes.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="eb279544" data-kind="attribute" data-el-id="782" data-attr-name="publication_date" data-attr-type="String" data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="782" data-tag-name="description" data-tag-value="EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public." data-file-path="Emissions/EmissionReport.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionInventory](EmissionInventory.html) |
| Association |  | [EmissionReportPeriod](EmissionReportPeriod.html) |
| Association |  | [Standard](../Organisation/Standard.html) |
| Association |  | [Organization](../Organisation/Organization.html) |

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

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="782"></div>
