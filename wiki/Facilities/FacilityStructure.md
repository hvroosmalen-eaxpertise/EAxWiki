# FacilityStructure

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityStructure records the fact that one facility is a structural component of another facility — for example, one facility is mounted on, connected to, or dependent upon another. This parent-child composition allows complex facility assemblies to be modelled as hierarchies, enabling aggregation of emissions from sub-facilities to parent facilities and supporting site-level reporting that reflects the physical dependencies between operational units. Most likely only the parent Facility in the FacilityStructure would be assigned to the lowest level of the Location hierarchy.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityStructure record. It is the primary key for this parent-child facility composition record and must remain stable for reporting and audit purposes. |
| facility_id | String |  | A foreign key identifying the child Facility in this structure — the facility that is a component of, or dependent upon, the parent facility. |
| parent_facility_id | String |  | A foreign key identifying the parent Facility in this structure — the facility that contains or hosts the child facility. Implementations must enforce acyclicity on this composition relationship. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityStructure records the fact that one facility is a structural component of another facility — for example, one facility is mounted on, connected to, or dependent upon another. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 757 → 757 |
| Association |  | 753 → 757 |

---

*Generated: 2026-06-18 13:11:38*