# EquipmentInstallation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

EquipmentInstallation captures the temporal fact of a piece of Equipment being installed at a Facility to perform specific operational activities. The effective and termination datetimes allow the history of equipment deployments to be tracked, supporting accurate attribution of equipment-level emissions to the facility and period in which the equipment was operational. An Emission Inventory can be associated with a particular piece of equipment through this relationship, and the link to Equipment is optional for an Emission Inventory but required for a Facility.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the EquipmentInstallation record. It is the primary key for this temporal installation fact and must remain stable for the full operational period. |
| equipment_id | String |  | A foreign key identifying the Equipment item that is installed at the referenced facility during this installation period. |
| facility_id | String |  | A foreign key identifying the Facility at which the equipment is installed and operational during the defined period. |
| effective_datetime | String |  | The date and time from which the equipment is installed and operational at the facility, in ISO 8601 format. Marks the start of the period during which emission measurements from this equipment should be attributed to this facility. |
| termination_datetime | String |  | The date and time at which the equipment installation ends, in ISO 8601 format. Null indicates the equipment is currently installed and operational. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EquipmentInstallation captures the temporal fact of a piece of Equipment being installed at a Facility to perform specific operational activities. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Equipment](Equipment.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [Equipment](Equipment.md) |
| Association |  | [Facility](Facility.md) |
| Association |  | [Equipment](Equipment.md) |

---

*Generated: 2026-06-24 14:20:12*