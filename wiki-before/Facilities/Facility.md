# Facility

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

Facility represents the capability of an organisation to perform a particular business function or service. It is a concept used to describe the functional capability that may arise from the installation of equipment and materials provided by a collection of assets and different locations. A facility can represent both a general aggregate capability of the organisation or a specific asset that is built, installed, or established to serve a particular purpose, such as a Plant, Research Laboratory, Office, or Offshore Platform. Facilities are classified by FacilityType and are assigned to locations through FacilityLocationAssociation, enabling accurate geographic attribution of site-level emission data.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Facility record. It is the primary key referenced by EmissionInventory to associate an inventory with the facility at which the emissions were measured. It must be globally unique and must not be reused after a facility is decommissioned or retired from the system. |
| name | String |  | The operational or registered name of the facility, such as "Plant A", "Unit AA", or "Offshore Platform North Sea Block 42". The name is used in reports, maps, and dashboards to identify the physical site and should correspond to the name used in regulatory submissions and site permits. |
| type_id | String |  | A foreign key referencing the FacilityType that classifies this facility. The type determines the applicable emission factor sets, regulatory reporting categories, and benchmarking peer group for this facility. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Facility represents the capability of an organisation to perform a particular business function or service. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityEmissionAllocation](FacilityEmissionAllocation.md) |
| Association |  | [EmissionActivityParameter](EmissionActivityParameter.md) |
| Association |  | [EquipmentInstallation](EquipmentInstallation.md) |
| Association |  | [FacilityActivityParticipation](FacilityActivityParticipation.md) |
| Association |  | [FacilitySpecification](FacilitySpecification.md) |
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.md) |
| Association |  | [FacilityStructure](FacilityStructure.md) |
| Association |  | [FacilityType](FacilityType.md) |
| Association |  | [Organization](../Organisation/Organization.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [Organization](../Organisation/Organization.md) |

---

*Generated: 2026-06-24 14:20:12*