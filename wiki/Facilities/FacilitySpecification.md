---
ea_id: 759
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: a9ffc1fe
---

# <span class="sl" data-layer="uml">work-product-component</span> FacilitySpecification

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="759" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="759" data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>FacilitySpecification is the work-product entity that assigns a specific value to an EmissionParameterType for a given Facility through a FacilityStructure link. It enables flexible, configurable specification of facility-level parameters such as energy efficiency ratings, capacity figures, temperature operating ranges, or process-specific constants without requiring schema extensions. The name-value pair is typed by the EmissionParameterType, which defines the dimension and expected units of the value.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the FacilitySpecification record. It is the primary key for this parameter assignment and must remain stable for reporting and audit purposes.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="204c966c" data-kind="attribute" data-el-id="759" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The descriptive name of the parameter assignment for this facility, such as "Unit AB Temperature" or "Annual Installed Capacity". This name distinguishes this specification record from other parameter assignments on the same facility.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="45dba889" data-kind="attribute" data-el-id="759" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>value</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The value of the specified parameter, stored as a string to accommodate numeric, coded, and free-text parameter values uniformly. Numeric values should carry units as defined in the associated EmissionParameterType record.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="63bd78fe" data-kind="attribute" data-el-id="759" data-attr-name="value" data-attr-type="String" data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_parameter_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the EmissionParameterType that defines what kind of parameter is being specified, such as "Temperature", "Energy Efficiency Rating", or "Product Yield".</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="cf3490d0" data-kind="attribute" data-el-id="759" data-attr-name="emission_parameter_type_id" data-attr-type="String" data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key identifying the Facility to which this specification applies.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="c8a2a237" data-kind="attribute" data-el-id="759" data-attr-name="facility_id" data-attr-type="String" data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>FacilitySpecification is the work-product entity that assigns a specific value to an EmissionParameterType for a given Facility through a FacilityStructure link.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="759" data-tag-name="description" data-tag-value="FacilitySpecification is the work-product entity that assigns a specific value to an EmissionParameterType for a given Facility through a FacilityStructure link." data-file-path="Facilities/FacilitySpecification.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Facility.html&quot;},{&quot;id&quot;:&quot;e759&quot;,&quot;label&quot;:&quot;FacilitySpecification&quot;,&quot;fullName&quot;:&quot;FacilitySpecification&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;FacilityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;FacilityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameter&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityParameter.html&quot;},{&quot;id&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;EquipmentInstallation&quot;,&quot;fullName&quot;:&quot;EquipmentInstallation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EquipmentInstallation.html&quot;},{&quot;id&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;FacilityActivityPartici…&quot;,&quot;fullName&quot;:&quot;FacilityActivityParticipation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityActivityParticipation.html&quot;},{&quot;id&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;FacilityLocationAssocia…&quot;,&quot;fullName&quot;:&quot;FacilityLocationAssociation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationAssociation.html&quot;},{&quot;id&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;FacilityStructure&quot;,&quot;fullName&quot;:&quot;FacilityStructure&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityStructure.html&quot;},{&quot;id&quot;:&quot;e754&quot;,&quot;label&quot;:&quot;FacilityType&quot;,&quot;fullName&quot;:&quot;FacilityType&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityType.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c873&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c875&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c877&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c879&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c880&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e759&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c889&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c891&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c892&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e754&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c896&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c890&quot;,&quot;source&quot;:&quot;e757&quot;,&quot;target&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*