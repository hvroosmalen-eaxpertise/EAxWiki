# EmissionFactorSource

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn. Examples include the UK DESNZ Conversion Factors, the US EPA Emission Factors Hub, the IPCC Assessment Report factor sets, and the ecoinvent database. Registering sources as a reference entity ensures that every factor used in a calculation can be traced to its origin, supporting third-party verification and regulatory acceptance.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionFactorSource record, referenced by EmissionFactor records to indicate provenance and by StandardSourceAssociation to link source databases to standards. |
| name | String |  | The official name of the emission factor source, such as UK Department for Energy Security and Net Zero Greenhouse Gas Conversion Factors, used in citations and report references. |
| description | String |  | A description of the scope, coverage, update frequency, and methodology of the source database, helping practitioners assess suitability for their reporting context. |
| publisher | String |  | The organisation responsible for maintaining and publishing the emission factor source, such as DESNZ, US EPA, or IPCC, used in bibliographic citations. |
| version | String |  | The edition or release version of the source publication, such as 2024 or v8.2, used to distinguish factor sets from different reporting years when multiple versions are retained. |
| publication_date | String |  | The date on which this version of the source was published or made effective, used to determine which version was current during a given reporting period. |
| url | String |  | A persistent URL or DOI pointing to the source publication or download location, enabling automated factor retrieval and supporting auditability. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 781 → 794 |
| Association |  | 781 → 818 |
| Association |  | 794 → 781 |
| Association |  | 780 → 781 |

---

*Generated: 2026-06-18 13:26:10*