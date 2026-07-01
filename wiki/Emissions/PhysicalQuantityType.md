---
ea_id: 797
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: d55ef836
---

# <span class="sl" data-layer="uml">reference-data</span> PhysicalQuantityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="797" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="797" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area. It enables dimensional analysis validation in calculation models, ensuring for example that an emission factor expressed in kgCO2/kWh is applied to an activity parameter expressed in an energy unit rather than a mass or distance unit. This prevents a class of calculation errors that are otherwise difficult to detect programmatically.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this PhysicalQuantityType record, referenced by UnitOfMeasure records to establish which physical dimension a unit measures.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="07c5df03" data-kind="attribute" data-el-id="797" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The standard name for the physical quantity, such as Mass, Energy, Thermodynamic Temperature, or Volume, used in validation error messages and unit catalogue documentation.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="194416c4" data-kind="attribute" data-el-id="797" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of the physical quantity, its SI base dimensions, and any special considerations relevant to GHG accounting, such as the distinction between higher and lower calorific values for energy quantities.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="e2107a10" data-kind="attribute" data-el-id="797" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="797" data-tag-name="description" data-tag-value="PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area." data-file-path="Emissions/PhysicalQuantityType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e797&quot;,&quot;label&quot;:&quot;PhysicalQuantityType&quot;,&quot;fullName&quot;:&quot;PhysicalQuantityType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e783&quot;,&quot;label&quot;:&quot;EmissionReportPeriod&quot;,&quot;fullName&quot;:&quot;EmissionReportPeriod&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionReportPeriod.html&quot;},{&quot;id&quot;:&quot;e798&quot;,&quot;label&quot;:&quot;UnitOfMeasureSourceRefe…&quot;,&quot;fullName&quot;:&quot;UnitOfMeasureSourceReference&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasureSourceReference.html&quot;},{&quot;id&quot;:&quot;e796&quot;,&quot;label&quot;:&quot;SystemOfUnits&quot;,&quot;fullName&quot;:&quot;SystemOfUnits&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;SystemOfUnits.html&quot;},{&quot;id&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;EmissionParameterType&quot;,&quot;fullName&quot;:&quot;EmissionParameterType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionParameterType.html&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e763&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameterValue&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/EmissionActivityParameterValue.html&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;EmissionComponent&quot;,&quot;fullName&quot;:&quot;EmissionComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponent.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c807&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e783&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c828&quot;,&quot;source&quot;:&quot;e798&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c829&quot;,&quot;source&quot;:&quot;e797&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c830&quot;,&quot;source&quot;:&quot;e796&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c832&quot;,&quot;source&quot;:&quot;e775&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c834&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e763&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c854&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c855&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c864&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c865&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c867&quot;,&quot;source&quot;:&quot;e776&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*