---
ea_id: 805
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 360f22f0
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationModelParameterArgument

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="805" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="805" data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationModelParameterArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the EmissionParameterType whose recorded values are used as the activity data input for that argument during calculation. This entity complements EmissionCalculationModelFactorArgument by declaring the activity-data inputs alongside the factor inputs, giving a complete specification of what a calculation model needs to execute.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this parameter argument record, used alongside factor argument records to provide a complete machine-readable specification of a calculation model inputs.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="0d689f40" data-kind="attribute" data-el-id="805" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_model_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionCalculationModel that declares this parameter argument, grouping all required activity-data inputs for the model.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="a1559eb1" data-kind="attribute" data-el-id="805" data-attr-name="emission_calculation_model_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>argument_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The named slot identifier within the calculation model, such as fuel_quantity_consumed or vehicle_distance_km, matching the variable name in the formula expression.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="f2f2c91e" data-kind="attribute" data-el-id="805" data-attr-name="argument_name" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_parameter_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the EmissionParameterType whose recorded values are used for this argument, linking the model input specification to the data entry requirements imposed on facility operators.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="f4c9568e" data-kind="attribute" data-el-id="805" data-attr-name="emission_parameter_type_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A description of the role of this parameter argument in the calculation, specifying the expected measurement approach and any unit conversion requirements before the parameter value is applied.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="13a50d40" data-kind="attribute" data-el-id="805" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionCalculationModelParameterArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the EmissionParameterType whose recorded values are used as the activity data input for that argument during calculation.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="805" data-tag-name="description" data-tag-value="EmissionCalculationModelParameterArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the EmissionParameterType whose recorded values are used as the activity data input for that argument during calculation." data-file-path="Emissions/EmissionCalculationModelParameterArgument.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;EmissionParameterType&quot;,&quot;fullName&quot;:&quot;EmissionParameterType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionParameterType.html&quot;},{&quot;id&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;EmissionCalculationModel&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModel&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModel.html&quot;},{&quot;id&quot;:&quot;e805&quot;,&quot;label&quot;:&quot;EmissionCalculationMode…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModelParameterArgument&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e807&quot;,&quot;label&quot;:&quot;EmissionActivityTypePar…&quot;,&quot;fullName&quot;:&quot;EmissionActivityTypeParameterTypeAssignment&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityTypeParameterTypeAssignment.html&quot;},{&quot;id&quot;:&quot;e803&quot;,&quot;label&quot;:&quot;EmissionCalculationForm…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationFormulaComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationFormulaComponent.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameter&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/EmissionActivityParameter.html&quot;},{&quot;id&quot;:&quot;e804&quot;,&quot;label&quot;:&quot;EmissionCalculationMode…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModelFactorArgument&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModelFactorArgument.html&quot;},{&quot;id&quot;:&quot;e793&quot;,&quot;label&quot;:&quot;EmissionActivityFactor&quot;,&quot;fullName&quot;:&quot;EmissionActivityFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityFactor.html&quot;},{&quot;id&quot;:&quot;e791&quot;,&quot;label&quot;:&quot;EmissionCalculationMeth…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationMethodType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationMethodType.html&quot;},{&quot;id&quot;:&quot;e790&quot;,&quot;label&quot;:&quot;EmissionCalculationForm…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationFormula&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationFormula.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;},{&quot;id&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Standard&quot;,&quot;fullName&quot;:&quot;Standard&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Standard.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c811&quot;,&quot;source&quot;:&quot;e807&quot;,&quot;target&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c815&quot;,&quot;source&quot;:&quot;e805&quot;,&quot;target&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c819&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c832&quot;,&quot;source&quot;:&quot;e775&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c841&quot;,&quot;source&quot;:&quot;e775&quot;,&quot;target&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c816&quot;,&quot;source&quot;:&quot;e805&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c818&quot;,&quot;source&quot;:&quot;e804&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c851&quot;,&quot;source&quot;:&quot;e793&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c858&quot;,&quot;source&quot;:&quot;e791&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c859&quot;,&quot;source&quot;:&quot;e777&quot;,&quot;target&quot;:&quot;e790&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c868&quot;,&quot;source&quot;:&quot;e777&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c916&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c821&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e790&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c865&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*