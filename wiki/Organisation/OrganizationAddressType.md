# OrganizationAddressType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAddressType provides the controlled vocabulary of address categories applicable to an organisation, such as "Visiting Address", "Goods Return Address", "Mail Address", "Delivery Address", or "Statutory Address". Classifying address types ensures that consuming systems can select the appropriate address for a given operational purpose without manual disambiguation. The code attribute supports alignment with external postal classification systems used by logistics providers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAddressType record. It must be stable so that OrganizationAddress intersection records that reference it remain valid when the type vocabulary is extended. |
| code | String |  | A short alphanumeric code for the address type, such as "VISIT" or "STAT", used in data exchange messages and API responses where the full name would be verbose. The code should be drawn from a standardised address type taxonomy where one exists for the relevant industry or jurisdiction. |
| name | String |  | The descriptive label for the address type, such as "Visiting Address" or "Statutory Address". The name must unambiguously identify the intended use of the address and should be consistent with terminology used in the organisation's legal jurisdiction for formal correspondence and regulatory filings. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAddressType provides the controlled vocabulary of address categories applicable to an organisation, such as "Visiting Address", "Goods Return Address", "Mail Address", "Delivery Address", or "Statutory Address". |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 744 → 743 |

---

*Generated: 2026-06-18 12:23:55*