---
ea_id: 746
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2b45222f
---

# <span class="sl" data-layer="uml">reference-data</span> PersonOrganizationRoleType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

PersonOrganizationRoleType provides the controlled vocabulary of roles that a person can play within an organisation, such as "Primary Contact", "Secondary Contact", "Sustainability Director", "CEO", or "Plant Manager". Classifying person–organisation relationships by role type enables structured retrieval of the appropriate contact for a given operational context, such as emissions reporting queries or audit communications. The role vocabulary may be extended by individual implementations to meet sector-specific requirements.

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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e747&quot;,&quot;label&quot;:&quot;OrganizationPersonAssoc…&quot;,&quot;fullName&quot;:&quot;OrganizationPersonAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationPersonAssociation.html&quot;},{&quot;id&quot;:&quot;e746&quot;,&quot;label&quot;:&quot;PersonOrganizationRoleT…&quot;,&quot;fullName&quot;:&quot;PersonOrganizationRoleType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;ContactPerson&quot;,&quot;fullName&quot;:&quot;ContactPerson&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ContactPerson.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c905&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e738&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c906&quot;,&quot;source&quot;:&quot;e747&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c907&quot;,&quot;source&quot;:&quot;e746&quot;,&quot;target&quot;:&quot;e747&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c908&quot;,&quot;source&quot;:&quot;e738&quot;,&quot;target&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*