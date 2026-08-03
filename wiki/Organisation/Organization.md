---
ea_id: 735
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7eedb1f0
---

# <span class="sl" data-layer="uml">master-data</span> Organization

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="735" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Organisation](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="735" data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Organization is the central anchor entity for emissions reporting. It represents any legal entity, company, subsidiary, joint venture, or other organisational unit that is subject to emissions accounting obligations or that voluntarily participates in a carbon disclosure programme. The Organization entity links directly to standards, types, contact persons, addresses, and organisational boundaries, forming the root context from which all emission inventories and product footprints are ultimately traceable. An organization may have an external identifier issued by a third-party registry such as GLEIF LEI codes, enabling cross-system reconciliation.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Organisation record. It serves as the primary key referenced by emission inventories, organisational boundaries, and contact persons. It must be immutable once assigned and must be globally unique within the system.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="848df53c" data-kind="attribute" data-el-id="735" data-attr-name="id" data-attr-type="Key" data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The registered legal or trading name of the organisation as it appears in official filings or public disclosures. The name is used in reports, dashboards, and data exchange messages to identify the reporting entity and must correspond to the name used in external registries where applicable.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="6e5d915f" data-kind="attribute" data-el-id="735" data-attr-name="name" data-attr-type="String" data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A free-text field providing supplementary information about the organisation, such as its primary business activities, geographic operating regions, or its role within a supply chain. This field supports both analytical queries and human-readable reporting by providing context that is not captured in structured attributes.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="651be106" data-kind="attribute" data-el-id="735" data-attr-name="description" data-attr-type="String" data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>external_identifier</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>An identifier assigned to the organisation by an external system or registry, such as a Legal Entity Identifier (LEI), DUNS number, or national company registration number. The external identifier enables interoperability with external data sources and is used when exchanging emissions data with supply chain partners or regulators who reference the organisation by an external code.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="54d4f018" data-kind="attribute" data-el-id="735" data-attr-name="external_identifier" data-attr-type="String" data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>organization_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key reference to the OrganizationType that classifies this organisation. Storing the type reference on the Organisation entity allows rapid filtering by organisational type without joining through a separate intersection table in the most common query patterns.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="3f263967" data-kind="attribute" data-el-id="735" data-attr-name="organization_type_id" data-attr-type="String" data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Organization is the central anchor entity for emissions reporting.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="735" data-tag-name="description" data-tag-value="Organization is the central anchor entity for emissions reporting." data-file-path="Organisation/Organization.md" data-api-port="8001" data-api-token="0161fd334a3f013401ef2574a96a6a5d70559edb80d92c4d" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationEmissionAllocation](../Emissions/OrganizationEmissionAllocation.html) |
| Association |  | [EmissionReport](../Emissions/EmissionReport.html) |
| Association |  | [Facility](../Facilities/Facility.html) |
| Association |  | [OrganizationalBoundary](OrganizationalBoundary.html) |
| Association |  | [OrganizationEquityShare](OrganizationEquityShare.html) |
| Association |  | [OrganizationIndustrySector](OrganizationIndustrySector.html) |
| Association |  | [OrganizationExternalIdentifier](OrganizationExternalIdentifier.html) |
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.html) |
| Association |  | [ContactPerson](ContactPerson.html) |
| Association |  | [OrganizationAddress](OrganizationAddress.html) |
| Association |  | [OrganizationAssociation](OrganizationAssociation.html) |
| Association |  | [OrganizationType](OrganizationType.html) |
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
| Association |  | [OrganizationEmissionAllocation](../Emissions/OrganizationEmissionAllocation.html) |
| Association |  | [OrganizationalBoundary](OrganizationalBoundary.html) |
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.html) |
| Association |  | [ContactPerson](ContactPerson.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="735"></div>

---

*Generated: 2026-08-03 11:11:53*