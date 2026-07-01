---
ea_id: 748
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 41d72825
---

# <span class="sl" data-layer="uml">master-data</span> OrganizationExternalIdentifier

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="748" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="748" data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number. An organisation may carry multiple external identifiers from different registries simultaneously, each classified by an OrganizationExternalIdentifierType. This supports cross-system reconciliation and supply chain data exchange, particularly in contexts where trading partners reference the organisation by different external codes.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the OrganizationExternalIdentifier record. It is the primary key and must remain stable across changes to the identifier value or type to preserve historical traceability.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="9118a458" data-kind="attribute" data-el-id="748" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>value</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The actual code or identifier string assigned by the external registry, such as "724500XWKLYJMHYXQ242" (LEI) or "12-3456789" (EIN). The format of this value depends entirely on the referenced OrganizationExternalIdentifierType and should be stored as received from the issuing registry without normalisation.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="d5a1d97e" data-kind="attribute" data-el-id="748" data-attr-name="value" data-attr-type="String" data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the Organisation to which this external identifier has been assigned. One organisation may have many OrganizationExternalIdentifier records, one per external registry that has issued a code for it.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="2cb86747" data-kind="attribute" data-el-id="748" data-attr-name="organization_id" data-attr-type="String" data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_external_identifier_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the OrganizationExternalIdentifierType that classifies the scheme under which this identifier was issued, such as "DUNS" or "Employer Identification Number (EIN)". This attribute enables consuming systems to select the correct identifier for a specific registry.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="14580160" data-kind="attribute" data-el-id="748" data-attr-name="organization_external_identifier_type_id" data-attr-type="String" data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="748" data-tag-name="description" data-tag-value="OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number." data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationExternalIdentifierType](OrganizationExternalIdentifierType.md) |
| Association |  | [Organization](Organization.md) |

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
| Association |  | [OrganizationExternalIdentifierType](OrganizationExternalIdentifierType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e749&quot;,&quot;label&quot;:&quot;OrganizationExternalIde…&quot;,&quot;fullName&quot;:&quot;OrganizationExternalIdentifierType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationExternalIdentifierType.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;},{&quot;id&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;OrganizationExternalIde…&quot;,&quot;fullName&quot;:&quot;OrganizationExternalIdentifier&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e801&quot;,&quot;label&quot;:&quot;OrganizationEmissionAll…&quot;,&quot;fullName&quot;:&quot;OrganizationEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/OrganizationEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;EmissionReport&quot;,&quot;fullName&quot;:&quot;EmissionReport&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionReport.html&quot;},{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/Facility.html&quot;},{&quot;id&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;OrganizationalBoundary&quot;,&quot;fullName&quot;:&quot;OrganizationalBoundary&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationalBoundary.html&quot;},{&quot;id&quot;:&quot;e751&quot;,&quot;label&quot;:&quot;OrganizationEquityShare&quot;,&quot;fullName&quot;:&quot;OrganizationEquityShare&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationEquityShare.html&quot;},{&quot;id&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;OrganizationIndustrySec…&quot;,&quot;fullName&quot;:&quot;OrganizationIndustrySector&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationIndustrySector.html&quot;},{&quot;id&quot;:&quot;e747&quot;,&quot;label&quot;:&quot;OrganizationPersonAssoc…&quot;,&quot;fullName&quot;:&quot;OrganizationPersonAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationPersonAssociation.html&quot;},{&quot;id&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;ContactPerson&quot;,&quot;fullName&quot;:&quot;ContactPerson&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ContactPerson.html&quot;},{&quot;id&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;OrganizationAddress&quot;,&quot;fullName&quot;:&quot;OrganizationAddress&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAddress.html&quot;},{&quot;id&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;OrganizationAssociation&quot;,&quot;fullName&quot;:&quot;OrganizationAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAssociation.html&quot;},{&quot;id&quot;:&quot;e736&quot;,&quot;label&quot;:&quot;OrganizationType&quot;,&quot;fullName&quot;:&quot;OrganizationType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationType.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionInventory.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c903&quot;,&quot;source&quot;:&quot;e749&quot;,&quot;target&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c824&quot;,&quot;source&quot;:&quot;e801&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c895&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c896&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c898&quot;,&quot;source&quot;:&quot;e740&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c899&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e751&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c901&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c904&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c906&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c908&quot;,&quot;source&quot;:&quot;e738&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c912&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c914&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c915&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e736&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c917&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c808&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c905&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*