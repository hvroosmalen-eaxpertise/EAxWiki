---
ea_id: 742
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 8fe38e0a
---

# <span class="sl" data-layer="uml">reference-data</span> OrganizationAssociationType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAssociationType provides the controlled vocabulary of association types that can exist between two organisations, such as "Auditor", "Department", "Subsidiary", "Verifier", "Branch", or "Affiliate". Classifying association types enables structured querying of organisational networks and supports automated determination of consolidation scope in GHG inventories. This is not a hierarchical association and can have many-to-many relationships, so the same organisation may be classified as both a subsidiary of one entity and a verifier for another simultaneously.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAssociationType record. It must be globally unique and stable so that existing OrganizationAssociation records are not affected when the type vocabulary is extended. |
| name | String |  | The descriptive label for the association type, such as "Subsidiary", "Verifier", "Branch", or "Affiliate". The name should be drawn from a consistent controlled vocabulary agreed across the reporting ecosystem to ensure that association types carry the same meaning when shared between organisations in a supply chain. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAssociationType provides the controlled vocabulary of association types that can exist between two organisations, such as "Auditor", "Department", "Subsidiary", "Verifier", "Branch", or "Affiliate". |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationAssociation](OrganizationAssociation.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;OrganizationAssociation&quot;,&quot;fullName&quot;:&quot;OrganizationAssociation&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAssociation.html&quot;},{&quot;id&quot;:&quot;e742&quot;,&quot;label&quot;:&quot;OrganizationAssociation…&quot;,&quot;fullName&quot;:&quot;OrganizationAssociationType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c913&quot;,&quot;source&quot;:&quot;e742&quot;,&quot;target&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c914&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e741&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*