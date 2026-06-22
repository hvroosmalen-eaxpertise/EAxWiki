# FacilityEmissionAllocation

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement. This entity supports scenarios where an emission source or activity is shared between multiple facilities, enabling the portion of the emission attributable to each site to be recorded separately for site-level inventory and regulatory reporting purposes. This is of particular relevance to US EPA Subpart W reporting for Petroleum and Natural Gas Systems.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityEmissionAllocation record. It must remain stable for audit and regulatory reporting purposes. |
| percentage | String |  | The fraction of the total emission quantity in the linked EmissionStatement that is allocated to the linked Facility, expressed as a value between 0 and 100. The sum of percentages across all allocations for a given EmissionStatement should equal 100. |
| emission_statement_id | String |  | A foreign key identifying the EmissionStatement whose emission quantity is being allocated to the referenced facility. |
| facility_id | String |  | A foreign key identifying the Facility to which the specified percentage of the emission quantity is allocated. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](../Emissions/EmissionStatement.md) |
| Association |  | [Facility](Facility.md) |

---

*Generated: 2026-06-22 16:50:36*