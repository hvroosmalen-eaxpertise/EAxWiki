---
ea_id: 746
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2b45222f
---

# <span class="sl" data-layer="uml">reference-data</span> PersonOrganizationRoleType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  **Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="746" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/PersonOrganizationRoleType.md" data-api-port="8001"></div>

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="746" data-file-path="Organisation/PersonOrganizationRoleType.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>PersonOrganizationRoleType provides the controlled vocabulary of roles that a person can play within an organisation, such as "Primary Contact", "Secondary Contact", "Sustainability Director", "CEO", or "Plant Manager". Classifying person–organisation relationships by role type enables structured retrieval of the appropriate contact for a given operational context, such as emissions reporting queries or audit communications. The role vocabulary may be extended by individual implementations to meet sector-specific requirements.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the PersonOrganizationRoleType record. It must be globally unique and stable so that existing OrganizationPersonAssociation records remain valid when the role vocabulary is extended. |
| name | String |  | The descriptive label for the role type, such as "Primary Contact", "Chief Executive Officer", or "Sustainability Director". The name should reflect the functional responsibility of the person in the context of emissions reporting and should be drawn from a controlled list agreed across the reporting ecosystem. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | PersonOrganizationRoleType provides the controlled vocabulary of roles that a person can play within an organisation, such as "Primary Contact", "Secondary Contact", "Sustainability Director", "CEO", or "Plant Manager". |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e746","label":"PersonOrganizationRoleT…","fullName":"PersonOrganizationRoleType","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Organization.html"}],"edges":[{"id":"c905","source":"e747","target":"e738","label":"Association","sourceLayer":"uml"},{"id":"c906","source":"e747","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c907","source":"e746","target":"e747","label":"Association","sourceLayer":"uml"},{"id":"c908","source":"e738","target":"e735","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 10:25:44*