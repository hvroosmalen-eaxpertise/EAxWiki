---
ea_id: 750
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7c1bc67d
---

# <span class="sl" data-layer="uml">master-data</span> OrganizationIndustrySector

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="750" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/OrganizationIndustrySector.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="750" data-file-path="Organisation/OrganizationIndustrySector.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType. An organisation may operate across multiple industry sectors, so this entity allows multiple sector classifications to be recorded without modifying the Organisation entity itself. It supports sector-level benchmarking and the selection of sector-appropriate emission factors, and enables regulatory groupings that aggregate organisations by industry for sector-level disclosure obligations.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the OrganizationIndustrySector record. It is the primary key for this intersection and must remain stable across sector reclassifications to preserve the history of sector assignments.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="047aee77" data-kind="attribute" data-el-id="750" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/OrganizationIndustrySector.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>industry_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A foreign key referencing the IndustrySectorType into which the organisation is classified. This attribute links the organisation to the standardised sector vocabulary and enables automated selection of sector-appropriate emission factors.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="1942a474" data-kind="attribute" data-el-id="750" data-attr-name="industry_type_id" data-attr-type="String" data-file-path="Organisation/OrganizationIndustrySector.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the Organisation being classified into the referenced industry sector. An organisation may have multiple OrganizationIndustrySector records if it operates across several sectors.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="d3e07c2c" data-kind="attribute" data-el-id="750" data-attr-name="organization_id" data-attr-type="String" data-file-path="Organisation/OrganizationIndustrySector.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="750" data-tag-name="description" data-tag-value="OrganizationIndustrySector is the intersection entity that classifies an Organisation into an IndustrySectorType." data-file-path="Organisation/OrganizationIndustrySector.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.md) |
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;},{&quot;id&quot;:&quot;e739&quot;,&quot;label&quot;:&quot;IndustrySectorType&quot;,&quot;fullName&quot;:&quot;IndustrySectorType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;IndustrySectorType.html&quot;},{&quot;id&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;OrganizationIndustrySec…&quot;,&quot;fullName&quot;:&quot;OrganizationIndustrySector&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e801&quot;,&quot;label&quot;:&quot;OrganizationEmissionAll…&quot;,&quot;fullName&quot;:&quot;OrganizationEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/OrganizationEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;EmissionReport&quot;,&quot;fullName&quot;:&quot;EmissionReport&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionReport.html&quot;},{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/Facility.html&quot;},{&quot;id&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;OrganizationalBoundary&quot;,&quot;fullName&quot;:&quot;OrganizationalBoundary&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationalBoundary.html&quot;},{&quot;id&quot;:&quot;e751&quot;,&quot;label&quot;:&quot;OrganizationEquityShare&quot;,&quot;fullName&quot;:&quot;OrganizationEquityShare&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationEquityShare.html&quot;},{&quot;id&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;OrganizationExternalIde…&quot;,&quot;fullName&quot;:&quot;OrganizationExternalIdentifier&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationExternalIdentifier.html&quot;},{&quot;id&quot;:&quot;e747&quot;,&quot;label&quot;:&quot;OrganizationPersonAssoc…&quot;,&quot;fullName&quot;:&quot;OrganizationPersonAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationPersonAssociation.html&quot;},{&quot;id&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;ContactPerson&quot;,&quot;fullName&quot;:&quot;ContactPerson&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ContactPerson.html&quot;},{&quot;id&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;OrganizationAddress&quot;,&quot;fullName&quot;:&quot;OrganizationAddress&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAddress.html&quot;},{&quot;id&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;OrganizationAssociation&quot;,&quot;fullName&quot;:&quot;OrganizationAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAssociation.html&quot;},{&quot;id&quot;:&quot;e736&quot;,&quot;label&quot;:&quot;OrganizationType&quot;,&quot;fullName&quot;:&quot;OrganizationType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationType.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionInventory.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c824&quot;,&quot;source&quot;:&quot;e801&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c895&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c896&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c898&quot;,&quot;source&quot;:&quot;e740&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c899&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e751&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c901&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c904&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c906&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c908&quot;,&quot;source&quot;:&quot;e738&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c912&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c914&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c915&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e736&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c917&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c900&quot;,&quot;source&quot;:&quot;e739&quot;,&quot;target&quot;:&quot;e739&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c902&quot;,&quot;source&quot;:&quot;e739&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c808&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c905&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*