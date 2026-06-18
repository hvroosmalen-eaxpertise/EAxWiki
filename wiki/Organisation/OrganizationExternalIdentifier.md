# OrganizationExternalIdentifier

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number. An organisation may carry multiple external identifiers from different registries simultaneously, each classified by an OrganizationExternalIdentifierType. This supports cross-system reconciliation and supply chain data exchange, particularly in contexts where trading partners reference the organisation by different external codes.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationExternalIdentifier record. It is the primary key and must remain stable across changes to the identifier value or type to preserve historical traceability. |
| value | String |  | The actual code or identifier string assigned by the external registry, such as "724500XWKLYJMHYXQ242" (LEI) or "12-3456789" (EIN). The format of this value depends entirely on the referenced OrganizationExternalIdentifierType and should be stored as received from the issuing registry without normalisation. |
| organization_id | String |  | A foreign key identifying the Organisation to which this external identifier has been assigned. One organisation may have many OrganizationExternalIdentifier records, one per external registry that has issued a code for it. |
| organization_external_identifier_type_id | String |  | A foreign key referencing the OrganizationExternalIdentifierType that classifies the scheme under which this identifier was issued, such as "DUNS" or "Employer Identification Number (EIN)". This attribute enables consuming systems to select the correct identifier for a specific registry. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 749 → 748 |
| Association |  | 735 → 748 |

---

*Generated: 2026-06-18 13:26:10*