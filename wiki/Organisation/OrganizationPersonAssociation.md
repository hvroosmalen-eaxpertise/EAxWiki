---
ea_id: 747
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 8addd765
---

# <span class="sl" data-layer="uml">master-data</span> OrganizationPersonAssociation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="747" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="747" data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType. This design allows one person to hold multiple roles across one or more organisations without duplicating contact person records. It is the mechanism through which an organisation's contact directory is maintained for emissions reporting, audit communication, and regulatory correspondence.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the OrganizationPersonAssociation record. It is the primary key for this intersection and must remain stable for audit and correspondence history purposes.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="80171dc9" data-kind="attribute" data-el-id="747" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A foreign key identifying the Organisation to which the contact person is associated in this role. An organisation may have many OrganizationPersonAssociation records covering different roles and different persons.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="02f0cdbd" data-kind="attribute" data-el-id="747" data-attr-name="organization_id" data-attr-type="String" data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>person_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the ContactPerson who holds the specified role in the referenced organisation. A single person may appear in multiple association records across different organisations or different roles within the same organisation.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="58e65579" data-kind="attribute" data-el-id="747" data-attr-name="person_id" data-attr-type="String" data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>person_organization_role_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the PersonOrganizationRoleType that describes the specific role played by the contact person in the referenced organisation, such as "Primary Contact" or "Plant Manager".</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="58cf8f0b" data-kind="attribute" data-el-id="747" data-attr-name="person_organization_role_type_id" data-attr-type="String" data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="747" data-tag-name="description" data-tag-value="OrganizationPersonAssociation is the intersection entity that links a ContactPerson to an Organisation in a specific role, as defined by a PersonOrganizationRoleType." data-file-path="Organisation/OrganizationPersonAssociation.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ContactPerson](ContactPerson.html) |
| Association |  | [Organization](Organization.html) |
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [PersonOrganizationRoleType](PersonOrganizationRoleType.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="747"></div>

---

*Generated: 2026-08-04 12:35:51*