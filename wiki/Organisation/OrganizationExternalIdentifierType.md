# <span class="sl" data-layer="uml">reference-data</span> OrganizationExternalIdentifierType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
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

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e749","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifierType","packageName":Organisation,"isFocal":true,"hasUrl":false,"url":""},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Organization.html"}],"edges":[{"id":"c903","source":"e749","target":"e748","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:44:31*