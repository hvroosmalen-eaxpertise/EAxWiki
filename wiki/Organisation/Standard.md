---
ea_id: 734
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 4a22ed5d
---

# <span class="sl" data-layer="uml">master-data</span> Standard

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="734" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="734" data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions. Examples include the GHG Protocol Corporate Standard, ISO 14064, and TCFD recommendations. Each Standard provides a named reference that can be cited in emission inventories or organisational boundary definitions, ensuring traceability between reported data and the methodology used to produce it. The Standard entity also carries a descriptive text and a URL so that consumers of the data can navigate directly to the authoritative source of the referenced specification.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Standard record. This is the primary key used to reference the standard from related entities such as OrganizationalBoundary and EmissionInventory. It must be stable across updates and must not be reused once assigned.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="51362338" data-kind="attribute" data-el-id="734" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The human-readable official name of the standard or protocol, for example "GHG Protocol Corporate Standard Edition 2015". The name should be rendered exactly as published by the issuing body so that cross-referencing with external catalogues is unambiguous. It is displayed in reports and selection lists throughout user interfaces.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="062bc7f4" data-kind="attribute" data-el-id="734" data-attr-name="name" data-attr-type="String" data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A free-text narrative providing additional context about the scope, applicability, and key requirements of the standard. The description may include version notes, jurisdictional applicability, or relationships to other standards. It is intended to help analysts determine whether this standard is appropriate for a given emission inventory or boundary definition.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="ac7cf511" data-kind="attribute" data-el-id="734" data-attr-name="description" data-attr-type="String" data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>url_description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A resolvable URL pointing to the official publication, web page, or registry entry for the standard. The URL enables automated or manual retrieval of the full text and supporting documentation. It should be maintained and verified periodically to ensure it remains active and points to the current authoritative version.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="b7fe893d" data-kind="attribute" data-el-id="734" data-attr-name="url_description" data-attr-type="String" data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key reference to the Organisation that governs or owns this standard. An organisation may define its own internal standards or act as the custodian of an industry standard, and this attribute records that governing relationship for traceability purposes.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="565951d1" data-kind="attribute" data-el-id="734" data-attr-name="organization_id" data-attr-type="String" data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="734" data-tag-name="description" data-tag-value="Standard represents a formal specification, protocol, methodology, or regulatory framework that governs how an organisation measures, calculates, or reports its greenhouse gas emissions." data-file-path="Organisation/Standard.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentPerStandard](../Emissions/EmissionComponentPerStandard.html) |
| Association |  | [EmissionStatementPerStandard](../Emissions/EmissionStatementPerStandard.html) |
| Association |  | [EmissionCategoryStandardAssociation](../Emissions/EmissionCategoryStandardAssociation.html) |
| Association |  | [StandardSourceAssociation](../Emissions/StandardSourceAssociation.html) |
| Association |  | [EmissionReport](../Emissions/EmissionReport.html) |
| Association |  | [OrganizationalBoundary](OrganizationalBoundary.html) |
| Association |  | [EmissionCalculationModel](../Emissions/EmissionCalculationModel.html) |
| Association |  | [EmissionInventory](../Emissions/EmissionInventory.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentPerStandard](../Emissions/EmissionComponentPerStandard.html) |
| Association |  | [EmissionStatementPerStandard](../Emissions/EmissionStatementPerStandard.html) |
| Association |  | [EmissionCategoryStandardAssociation](../Emissions/EmissionCategoryStandardAssociation.html) |
| Association |  | [StandardSourceAssociation](../Emissions/StandardSourceAssociation.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="734"></div>

---

*Generated: 2026-08-03 08:46:17*