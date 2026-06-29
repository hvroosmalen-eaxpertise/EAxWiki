# <span class="sl" data-layer="uml">work-product-component</span> EquipmentInstallation

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

EquipmentInstallation captures the temporal fact of a piece of Equipment being installed at a Facility to perform specific operational activities. The effective and termination datetimes allow the history of equipment deployments to be tracked, supporting accurate attribution of equipment-level emissions to the facility and period in which the equipment was operational. An Emission Inventory can be associated with a particular piece of equipment through this relationship, and the link to Equipment is optional for an Emission Inventory but required for a Facility.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the EquipmentInstallation record. It is the primary key for this temporal installation fact and must remain stable for the full operational period. |
| equipment_id | String |  | A foreign key identifying the Equipment item that is installed at the referenced facility during this installation period. |
| facility_id | String |  | A foreign key identifying the Facility at which the equipment is installed and operational during the defined period. |
| effective_datetime | String |  | The date and time from which the equipment is installed and operational at the facility, in ISO 8601 format. Marks the start of the period during which emission measurements from this equipment should be attributed to this facility. |
| termination_datetime | String |  | The date and time at which the equipment installation ends, in ISO 8601 format. Null indicates the equipment is currently installed and operational. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EquipmentInstallation captures the temporal fact of a piece of Equipment being installed at a Facility to perform specific operational activities. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Equipment](Equipment.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Equipment](Equipment.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e756","label":"Equipment","fullName":"Equipment","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Equipment.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityEmissionAllocation.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c873","source":"e753","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c875","source":"e753","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c877","source":"e753","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c879","source":"e753","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c880","source":"e753","target":"e759","label":"Association","sourceLayer":"uml"},{"id":"c889","source":"e753","target":"e758","label":"Association","sourceLayer":"uml"},{"id":"c891","source":"e753","target":"e757","label":"Association","sourceLayer":"uml"},{"id":"c892","source":"e753","target":"e754","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c876","source":"e756","target":"e756","label":"Association","sourceLayer":"uml"},{"id":"c878","source":"e756","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c890","source":"e757","target":"e757","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 18:57:23*