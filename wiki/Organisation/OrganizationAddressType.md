# <span class="sl" data-layer="uml">REF</span> OrganizationAddressType

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAddressType provides the controlled vocabulary of address categories applicable to an organisation, such as "Visiting Address", "Goods Return Address", "Mail Address", "Delivery Address", or "Statutory Address". Classifying address types ensures that consuming systems can select the appropriate address for a given operational purpose without manual disambiguation. The code attribute supports alignment with external postal classification systems used by logistics providers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAddressType record. It must be stable so that OrganizationAddress intersection records that reference it remain valid when the type vocabulary is extended. |
| code | String |  | A short alphanumeric code for the address type, such as "VISIT" or "STAT", used in data exchange messages and API responses where the full name would be verbose. The code should be drawn from a standardised address type taxonomy where one exists for the relevant industry or jurisdiction. |
| name | String |  | The descriptive label for the address type, such as "Visiting Address" or "Statutory Address". The name must unambiguously identify the intended use of the address and should be consistent with terminology used in the organisation's legal jurisdiction for formal correspondence and regulatory filings. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAddressType provides the controlled vocabulary of address categories applicable to an organisation, such as "Visiting Address", "Goods Return Address", "Mail Address", "Delivery Address", or "Statutory Address". |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationAddress](OrganizationAddress.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e744","label":"OrganizationAddressType","fullName":"OrganizationAddressType","packageName":Organisation,"isFocal":true,"hasUrl":false,"url":""},{"id":"e737","label":"Address","fullName":"Address","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Address.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"Organization.html"}],"edges":[{"id":"c910","source":"e744","target":"e743","label":"Association"},{"id":"c911","source":"e737","target":"e743","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"}]};</script>

---

*Generated: 2026-06-26 09:44:49*