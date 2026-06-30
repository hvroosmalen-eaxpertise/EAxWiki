---
ea_id: 757
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">master-data</span> FacilityStructure

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityStructure records the fact that one facility is a structural component of another facility — for example, one facility is mounted on, connected to, or dependent upon another. This parent-child composition allows complex facility assemblies to be modelled as hierarchies, enabling aggregation of emissions from sub-facilities to parent facilities and supporting site-level reporting that reflects the physical dependencies between operational units. Most likely only the parent Facility in the FacilityStructure would be assigned to the lowest level of the Location hierarchy.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityStructure record. It is the primary key for this parent-child facility composition record and must remain stable for reporting and audit purposes. |
| facility_id | String |  | A foreign key identifying the child Facility in this structure — the facility that is a component of, or dependent upon, the parent facility. |
| parent_facility_id | String |  | A foreign key identifying the parent Facility in this structure — the facility that contains or hosts the child facility. Implementations must enforce acyclicity on this composition relationship. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityStructure records the fact that one facility is a structural component of another facility — for example, one facility is mounted on, connected to, or dependent upon another. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityStructure](FacilityStructure.md) |
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [FacilityStructure](FacilityStructure.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityEmissionAllocation.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c873","source":"e753","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c875","source":"e753","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c877","source":"e753","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c879","source":"e753","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c880","source":"e753","target":"e759","label":"Association","sourceLayer":"uml"},{"id":"c889","source":"e753","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c891","source":"e753","target":"e757","label":"Association","sourceLayer":"uml"},{"id":"c892","source":"e753","target":"e754","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c890","source":"e757","target":"e757","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 15:54:13*