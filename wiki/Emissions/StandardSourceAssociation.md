# StandardSourceAssociation

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard. A standard such as the GHG Protocol may endorse specific factor databases (IPCC, national inventory agencies, DESNZ) while another framework mandates different sources. Capturing these endorsements as data avoids hard-coding source eligibility rules in application logic and supports audit queries confirming that all factors used were sourced from a framework-approved database.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this StandardSourceAssociation record, used in audit queries to verify that emission factors were drawn from a source approved under the applicable standard. |
| standard_id | String |  | Foreign key to the Standard that endorses or mandates the use of the referenced emission factor source. |
| emission_factor_source_id | String |  | Foreign key to the EmissionFactorSource that is endorsed or mandated by the referenced standard, establishing the permissible source set for calculations under that framework. |
| notes | String |  | Free-text notes describing any conditions or restrictions on the use of this source under the referenced standard, such as mandatory for UK Scope 2 market-based method. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 781 → 794 |
| Association |  | 794 → 781 |
| Association |  | 794 → 734 |

---

*Generated: 2026-06-19 13:04:05*