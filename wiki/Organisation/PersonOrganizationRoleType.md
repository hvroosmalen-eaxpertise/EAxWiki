# <span class="sl" data-layer="uml">REF</span> PersonOrganizationRoleType

**Type:** Class  **Stereotype:** reference-data  
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

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e746","label":"PersonOrganizationRoleT…","fullName":"PersonOrganizationRoleType","packageName":Organisation,"isFocal":true,"hasUrl":false,"url":""},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Organization.html"}],"edges":[{"id":"c905","source":"e747","target":"e738","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c907","source":"e746","target":"e747","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*