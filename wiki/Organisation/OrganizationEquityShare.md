# OrganizationEquityShare

**Type:** Class  
**Stereotype:** master-data  

OrganizationEquityShare records the percentage equity stake that an organisation holds in a specific OrganizationalBoundary, supporting the equity-share consolidation approach for GHG accounting as defined in the GHG Protocol. Under the equity share approach, an organisation accounts for GHG emissions from operations in proportion to its equity share, regardless of operational control. This entity enables accurate attribution of partial ownership stakes to the correct boundary and also supports Scope 3 Category 15 (Investments) reporting under the GHG Protocol.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationEquityShare record. It is the primary key and must remain stable across changes to the equity percentage to preserve the historical record of ownership stakes. |
| percentage | String |  | The equity share percentage held by the organisation in the referenced organisational boundary, expressed as a value between 0 and 100. This value directly determines the proportion of the boundary's emissions that are attributed to the organisation under the equity share consolidation approach. |
| organization_id | String |  | A foreign key identifying the Organisation that holds the equity share. This is the reporting entity for which the equity-weighted emission attribution will be calculated. |
| organizational_boundary_id | String |  | A foreign key referencing the OrganizationalBoundary in which the organisation holds the recorded equity share. The boundary determines which emission activities are subject to equity-share attribution. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationEquityShare records the percentage equity stake that an organisation holds in a specific OrganizationalBoundary, supporting the equity-share consolidation approach for GHG accounting as defined in the GHG Protocol. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 735 → 751 |
