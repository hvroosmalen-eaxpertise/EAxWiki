---
ea_id: 742
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> OrganizationAssociationType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
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
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e742","label":"OrganizationAssociation…","fullName":"OrganizationAssociationType","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Organization.html"}],"edges":[{"id":"c913","source":"e742","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c914","source":"e735","target":"e741","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 17:14:23*