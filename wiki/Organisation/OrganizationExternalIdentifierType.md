---
ea_id: 749
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 63616c0f
---

# <span class="sl" data-layer="uml">reference-data</span> OrganizationExternalIdentifierType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationExternalIdentifierType provides the controlled vocabulary of external identifier schemes used to publicly identify organisations, such as "Federal Information Processing System (FIPS) Codes", "Data Universal Numbering System (DUNS)", "Employer Identification Number (EIN)", or "North American Industry Classification System (NAICS)". Classifying identifier types enables systems to select the correct identifier when communicating with a specific external registry or trading partner system.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationExternalIdentifierType record. It must be stable so that existing OrganizationExternalIdentifier records that reference it remain valid when the type vocabulary is extended. |
| name | String |  | The descriptive label for the identifier type, such as "Legal Entity Identifier (LEI)" or "DUNS Number". The name must unambiguously identify the issuing registry or classification system to which the code belongs and should match the official name used by the issuing authority. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationExternalIdentifierType provides the controlled vocabulary of external identifier schemes used to publicly identify organisations, such as "Federal Information Processing System (FIPS) Codes", "Data Universal Numbering System (DUNS)", "Employer |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationExternalIdentifier](OrganizationExternalIdentifier.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;OrganizationExternalIde…&quot;,&quot;fullName&quot;:&quot;OrganizationExternalIdentifier&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationExternalIdentifier.html&quot;},{&quot;id&quot;:&quot;e749&quot;,&quot;label&quot;:&quot;OrganizationExternalIde…&quot;,&quot;fullName&quot;:&quot;OrganizationExternalIdentifierType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c903&quot;,&quot;source&quot;:&quot;e749&quot;,&quot;target&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c904&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e748&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*