---
ea_id: 738
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2476c091
---

# <span class="sl" data-layer="uml">master-data</span> ContactPerson

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="738" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/ContactPerson.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="738" data-file-path="Organisation/ContactPerson.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities. Contact persons may include sustainability officers, environmental compliance managers, data stewards, or external auditors. The entity captures sufficient name components to support formal correspondence, including title, first, middle, last, and full name, as well as a phone number for direct communication. A contact person is associated with an address for postal or physical contact purposes.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the ContactPerson record. It serves as the primary key and is referenced when linking a contact person to an organisation or address. It must remain stable across name or contact detail changes to preserve the integrity of historical associations.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="b1416343" data-kind="attribute" data-el-id="738" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>title_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The professional or social title of the contact person, such as "Dr.", "Prof.", "Mr.", or "Ms.". The title is used in formal written correspondence and official reports, and should be drawn from a controlled vocabulary where possible to ensure consistent rendering across communication channels.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="7edfa0d3" data-kind="attribute" data-el-id="738" data-attr-name="title_name" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>first_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The given name or first name of the contact person as they prefer to be addressed in professional communications. It is used in conjunction with middle_name and last_name to construct the full name and to support personalised communication in notifications and correspondence.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="8cb795e9" data-kind="attribute" data-el-id="738" data-attr-name="first_name" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>middle_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The middle name or initial of the contact person, where applicable. This field is optional and is included primarily to support disambiguation between persons with identical first and last names, and to meet formal identification requirements in certain jurisdictions.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="f94738c8" data-kind="attribute" data-el-id="738" data-attr-name="middle_name" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>last_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The family name or surname of the contact person. Together with first_name and middle_name, it forms the complete personal name used in official correspondence, audit trails, and disclosure documents.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="c47e15dc" data-kind="attribute" data-el-id="738" data-attr-name="last_name" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>full_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The complete rendered name of the contact person as it should appear in reports and communications, which may include title, all name components, and any post-nominal qualifications. This field may be derived from the component name fields or separately maintained where the standard concatenation does not produce the correct rendering.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="9201a52b" data-kind="attribute" data-el-id="738" data-attr-name="full_name" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>phone_number</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The telephone number at which the contact person can be reached for queries related to emissions reporting. The number should be stored in international format (E.164) to support cross-border communication and automated dialling integrations.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="0b98b15f" data-kind="attribute" data-el-id="738" data-attr-name="phone_number" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>address_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>A foreign key identifying the Address record that represents this contact person's physical or postal contact location. This attribute enables automated generation of formal correspondence addressed to the contact person at their registered address.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="0b278b47" data-kind="attribute" data-el-id="738" data-attr-name="address_id" data-attr-type="String" data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="738" data-tag-name="description" data-tag-value="ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities." data-file-path="Organisation/ContactPerson.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |
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
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e747&quot;,&quot;label&quot;:&quot;OrganizationPersonAssoc…&quot;,&quot;fullName&quot;:&quot;OrganizationPersonAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationPersonAssociation.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;},{&quot;id&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;ContactPerson&quot;,&quot;fullName&quot;:&quot;ContactPerson&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e746&quot;,&quot;label&quot;:&quot;PersonOrganizationRoleT…&quot;,&quot;fullName&quot;:&quot;PersonOrganizationRoleType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;PersonOrganizationRoleType.html&quot;},{&quot;id&quot;:&quot;e801&quot;,&quot;label&quot;:&quot;OrganizationEmissionAll…&quot;,&quot;fullName&quot;:&quot;OrganizationEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/OrganizationEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;EmissionReport&quot;,&quot;fullName&quot;:&quot;EmissionReport&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionReport.html&quot;},{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/Facility.html&quot;},{&quot;id&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;OrganizationalBoundary&quot;,&quot;fullName&quot;:&quot;OrganizationalBoundary&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationalBoundary.html&quot;},{&quot;id&quot;:&quot;e751&quot;,&quot;label&quot;:&quot;OrganizationEquityShare&quot;,&quot;fullName&quot;:&quot;OrganizationEquityShare&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationEquityShare.html&quot;},{&quot;id&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;OrganizationIndustrySec…&quot;,&quot;fullName&quot;:&quot;OrganizationIndustrySector&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationIndustrySector.html&quot;},{&quot;id&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;OrganizationExternalIde…&quot;,&quot;fullName&quot;:&quot;OrganizationExternalIdentifier&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationExternalIdentifier.html&quot;},{&quot;id&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;OrganizationAddress&quot;,&quot;fullName&quot;:&quot;OrganizationAddress&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAddress.html&quot;},{&quot;id&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;OrganizationAssociation&quot;,&quot;fullName&quot;:&quot;OrganizationAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAssociation.html&quot;},{&quot;id&quot;:&quot;e736&quot;,&quot;label&quot;:&quot;OrganizationType&quot;,&quot;fullName&quot;:&quot;OrganizationType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationType.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionInventory.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c905&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c906&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c907&quot;,&quot;source&quot;:&quot;e746&quot;,&quot;target&quot;:&quot;e747&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c824&quot;,&quot;source&quot;:&quot;e801&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c895&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c896&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c898&quot;,&quot;source&quot;:&quot;e740&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c899&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e751&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c901&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c904&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c908&quot;,&quot;source&quot;:&quot;e738&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c912&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c914&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c915&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e736&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c917&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c808&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:16*