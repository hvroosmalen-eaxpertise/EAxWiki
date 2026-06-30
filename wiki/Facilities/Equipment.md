---
ea_id: 756
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">master-data</span> Equipment

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

Equipment describes a device used in an operation or activity that is located or installed at a particular Facility. It is a manufactured, non-consumable object delivering the required functionality of facilities. Examples include Natural Gas Pneumatic Devices, Blowdown Vent Stacks, Flare Stacks, Centrifugal Compressors, Reciprocating Compressors, Atmospheric Storage Tanks, and Combustion Equipment, as catalogued in the US EPA Subpart W reference. A self-referential parent relationship allows the modelling of equipment assemblies. Both Facility and Equipment may be assigned an EmissionActivityParameter and an associated EmissionActivityParameterValue at a given point in time.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Equipment record. It serves as the primary key referenced by EmissionInventory and EmissionActivityParameter when associating records with a specific equipment item. It must be globally unique and stable across equipment moves or renames. |
| name | String |  | The operational name or asset tag description of the equipment item, such as "Centrifugal Compressor C-001" or "Flare Stack F-002". The name is used in inventory reports, maintenance records, and asset dashboards to identify the specific physical asset associated with a given emission measurement. |
| parent_id | String |  | A foreign key referencing the parent Equipment record in the equipment assembly hierarchy, enabling the modelling of sub-components that roll up to a parent asset for aggregated emission reporting. Implementations must enforce acyclicity. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Equipment describes a device used in an operation or activity that is located or installed at a particular Facility. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Equipment](Equipment.md) |
| Association |  | [EquipmentInstallation](EquipmentInstallation.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Equipment](Equipment.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e756","label":"Equipment","fullName":"Equipment","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Facility.html"}],"edges":[{"id":"c877","source":"e753","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c878","source":"e756","target":"e761","label":"Association","sourceLayer":"uml"},{"id":"c876","source":"e756","target":"e756","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 17:14:23*