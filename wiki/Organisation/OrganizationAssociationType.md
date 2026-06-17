# OrganizationAssociationType

**Type:** Class  
**Stereotype:** reference-data  

OrganizationAssociationType provides the controlled vocabulary of association types that can exist between two organisations, such as "Auditor", "Department", "Subsidiary", "Verifier", "Branch", or "Affiliate". Classifying association types enables structured querying of organisational networks and supports automated determination of consolidation scope in GHG inventories. This is not a hierarchical association and can have many-to-many relationships, so the same organisation may be classified as both a subsidiary of one entity and a verifier for another simultaneously.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAssociationType record. It must be globally unique and stable so that existing OrganizationAssociation records are not affected when the type vocabulary is extended. |
| name | String |  | The descriptive label for the association type, such as "Subsidiary", "Verifier", "Branch", or "Affiliate". The name should be drawn from a consistent controlled vocabulary agreed across the reporting ecosystem to ensure that association types carry the same meaning when shared between organisations in a supply chain. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAssociationType provides the controlled vocabulary of association types that can exist between two organisations, such as "Auditor", "Department", "Subsidiary", "Verifier", "Branch", or "Affiliate". |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 742 → 741 |
