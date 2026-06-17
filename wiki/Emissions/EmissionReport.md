# EmissionReport

**Type:** Class  
**Stereotype:** work-product-component  

EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public. It carries the report metadata, including the applicable reporting framework, the consolidation boundary, and the sign-off status. The entity acts as an envelope that groups EmissionReportPeriod records and drives the generation of summary tables and narrative sections in published disclosures.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionReport record, used to link all period detail records, supporting documents, and approval workflow steps to this specific published artefact. |
| organisation_id | String |  | Foreign key to the Organisation that is the reporting entity, identifying whose emissions are being disclosed in this report. |
| standard_id | String |  | Foreign key to the Standard or reporting framework under which this report is prepared, such as GHG Protocol, ESRS E1, or CDP Climate Change, governing the structure and content requirements. |
| name | String |  | The human-readable title of the report, used for filing and archive identification. |
| status | String |  | The approval and publication status of the report, such as draft, under assurance, board approved, or published, supporting governance workflow tracking. |
| publication_date | String |  | The date on which the report was formally published or filed with the relevant authority, establishing the official record date for regulatory and audit purposes. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionReport is a work-product-component that represents a formal, structured output document produced from one or more EmissionInventory records for disclosure to regulators, investors, or the public. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 782 → 771 |
| Association |  | 782 → 783 |
| Association |  | 734 → 782 |
| Association |  | 735 → 782 |
