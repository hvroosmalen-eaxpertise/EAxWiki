---
ea_id: 756
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: e4fedc0b
---

# <span class="sl" data-layer="uml">master-data</span> Equipment

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="756" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Facilities/Equipment.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="756" data-file-path="Facilities/Equipment.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Equipment describes a device used in an operation or activity that is located or installed at a particular Facility. It is a manufactured, non-consumable object delivering the required functionality of facilities. Examples include Natural Gas Pneumatic Devices, Blowdown Vent Stacks, Flare Stacks, Centrifugal Compressors, Reciprocating Compressors, Atmospheric Storage Tanks, and Combustion Equipment, as catalogued in the US EPA Subpart W reference. A self-referential parent relationship allows the modelling of equipment assemblies. Both Facility and Equipment may be assigned an EmissionActivityParameter and an associated EmissionActivityParameterValue at a given point in time.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Equipment record. It serves as the primary key referenced by EmissionInventory and EmissionActivityParameter when associating records with a specific equipment item. It must be globally unique and stable across equipment moves or renames.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="1499ccd8" data-kind="attribute" data-el-id="756" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/Equipment.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The operational name or asset tag description of the equipment item, such as "Centrifugal Compressor C-001" or "Flare Stack F-002". The name is used in inventory reports, maintenance records, and asset dashboards to identify the specific physical asset associated with a given emission measurement.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="ec5305d0" data-kind="attribute" data-el-id="756" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/Equipment.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key referencing the parent Equipment record in the equipment assembly hierarchy, enabling the modelling of sub-components that roll up to a parent asset for aggregated emission reporting. Implementations must enforce acyclicity.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="0a7e3bfe" data-kind="attribute" data-el-id="756" data-attr-name="parent_id" data-attr-type="String" data-file-path="Facilities/Equipment.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Equipment describes a device used in an operation or activity that is located or installed at a particular Facility.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="756" data-tag-name="description" data-tag-value="Equipment describes a device used in an operation or activity that is located or installed at a particular Facility." data-file-path="Facilities/Equipment.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;EquipmentInstallation&quot;,&quot;fullName&quot;:&quot;EquipmentInstallation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EquipmentInstallation.html&quot;},{&quot;id&quot;:&quot;e756&quot;,&quot;label&quot;:&quot;Equipment&quot;,&quot;fullName&quot;:&quot;Equipment&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Facility.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c877&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c878&quot;,&quot;source&quot;:&quot;e756&quot;,&quot;target&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c876&quot;,&quot;source&quot;:&quot;e756&quot;,&quot;target&quot;:&quot;e756&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*