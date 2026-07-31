---
ea_id: 741
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 4cea77c8
---

# <span class="sl" data-layer="uml">master-data</span> OrganizationAssociation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="741" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="741" data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association. It records which organisation is the parent and which is the child in the association, and is classified by an OrganizationAssociationType that describes the nature of the relationship. This entity enables the modelling of complex corporate structures without embedding hierarchy information directly in the Organization entity, and supports the non-hierarchical many-to-many relationships (e.g., Auditor, Affiliate) that are common in emissions reporting ecosystems.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the OrganizationAssociation record. It is the primary key used to identify and reference this inter-organisational relationship. It must be globally unique and stable for as long as the association is in force.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="e78f8598" data-kind="attribute" data-el-id="741" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A foreign key identifying the parent Organisation in this association — the organisation that is the subject of the relationship, for example the parent company in a subsidiary relationship or the engaging organisation in a verifier relationship.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="035410fb" data-kind="attribute" data-el-id="741" data-attr-name="organization_id" data-attr-type="String" data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>associated_organization_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the associated (child or counterpart) Organisation in this relationship. For example, in a subsidiary association this would be the subsidiary organisation, and in a verifier association this would be the verifying body.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="40bcb110" data-kind="attribute" data-el-id="741" data-attr-name="associated_organization_id" data-attr-type="String" data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_association_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the OrganizationAssociationType that classifies the nature of this inter-organisational relationship. The type controls how the association is interpreted in boundary consolidation calculations and data exchange scenarios.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="01fbe13c" data-kind="attribute" data-el-id="741" data-attr-name="organization_association_type_id" data-attr-type="String" data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="741" data-tag-name="description" data-tag-value="OrganizationAssociation represents a typed relationship between two organisations, such as a parent–subsidiary link, a joint-venture partnership, a verifier relationship, or a department association." data-file-path="Organisation/OrganizationAssociation.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationAssociationType](OrganizationAssociationType.html) |
| Association |  | [Organization](Organization.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](Organization.html) |
| Association |  | [OrganizationAssociationType](OrganizationAssociationType.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="741"></div>

---

*Generated: 2026-07-31 18:00:34*