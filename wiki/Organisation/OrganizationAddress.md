# OrganizationAddress

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAddress is the intersection entity that associates an Organisation with a physical or postal Address at a specific address type (e.g., visiting, correspondence, statutory). It acts as a bridge between the Organisation, the Address, and the OrganizationAddressType, allowing an organisation to maintain multiple categorised addresses simultaneously without ambiguity. This design supports the full range of address types required by legal, regulatory, and operational contexts.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAddress record. It is the primary key for this intersection and must remain stable so that address assignments can be audited over time. |
| address_id | String |  | A foreign key referencing the Address record assigned to this organisation. This links the organisation to the actual postal or physical address details stored in the Address entity. |
| organization_address_type_id | String |  | A foreign key referencing the OrganizationAddressType that classifies this address assignment, for example "Visiting Address" or "Statutory Address". This attribute enables consuming systems to select the appropriate address for a given operational purpose. |
| organization_id | String |  | A foreign key identifying the Organisation to which this address is assigned. An organisation may have multiple OrganizationAddress records, each with a different type, all linked through this attribute. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAddress is the intersection entity that associates an Organisation with a physical or postal Address at a specific address type (e.g., visiting, correspondence, statutory). |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 744 → 743 |
| Association |  | 737 → 743 |
| Association |  | 735 → 743 |

---

*Generated: 2026-06-19 12:59:14*