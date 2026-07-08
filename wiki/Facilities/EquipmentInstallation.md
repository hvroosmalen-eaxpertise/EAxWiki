---
ea_id: 761
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2396216d
---

# <span class="sl" data-layer="uml">work-product-component</span> EquipmentInstallation

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Facility.html&quot;},{&quot;id&quot;:&quot;e756&quot;,&quot;label&quot;:&quot;Equipment&quot;,&quot;fullName&quot;:&quot;Equipment&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Equipment.html&quot;},{&quot;id&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;EquipmentInstallation&quot;,&quot;fullName&quot;:&quot;EquipmentInstallation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;FacilityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;FacilityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameter&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityParameter.html&quot;},{&quot;id&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;FacilityActivityPartici…&quot;,&quot;fullName&quot;:&quot;FacilityActivityParticipation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityActivityParticipation.html&quot;},{&quot;id&quot;:&quot;e759&quot;,&quot;label&quot;:&quot;FacilitySpecification&quot;,&quot;fullName&quot;:&quot;FacilitySpecification&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilitySpecification.html&quot;},{&quot;id&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;FacilityLocationAssocia…&quot;,&quot;fullName&quot;:&quot;FacilityLocationAssociation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationAssociation.html&quot;},{&quot;id&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;FacilityStructure&quot;,&quot;fullName&quot;:&quot;FacilityStructure&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityStructure.html&quot;},{&quot;id&quot;:&quot;e754&quot;,&quot;label&quot;:&quot;FacilityType&quot;,&quot;fullName&quot;:&quot;FacilityType&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityType.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c873&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c875&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c877&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c879&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c880&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e759&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c889&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c891&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c892&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e754&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c896&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c876&quot;,&quot;source&quot;:&quot;e756&quot;,&quot;target&quot;:&quot;e756&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c878&quot;,&quot;source&quot;:&quot;e756&quot;,&quot;target&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c890&quot;,&quot;source&quot;:&quot;e757&quot;,&quot;target&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*