---
ea_id: 799
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: c782bb64
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionRecordingMethodType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="799" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionRecordingMethodType.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="799" data-file-path="Emissions/EmissionRecordingMethodType.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, derived from a published default, or extrapolated from related data. This classification is required by ISO 14064-1 and GHG Protocol for data quality assessments and for identifying which statements require improved monitoring as part of an improvement plan.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionRecordingMethodType record, referenced by EmissionStatement records to describe how each emission quantity was obtained.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="f13d63d3" data-kind="attribute" data-el-id="799" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionRecordingMethodType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The label for the recording method, drawn from values such as Measured, Calculated, Estimated, Default factor, or Extrapolated, used in data quality tables and assurance documentation.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="b9e7155b" data-kind="attribute" data-el-id="799" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionRecordingMethodType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of the recording method approach, its typical applicability, and the data quality tier it represents, supporting selection guidance for practitioners completing inventory data entry.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="21419f44" data-kind="attribute" data-el-id="799" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionRecordingMethodType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, der</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="799" data-tag-name="description" data-tag-value="EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, der" data-file-path="Emissions/EmissionRecordingMethodType.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;},{&quot;id&quot;:&quot;e799&quot;,&quot;label&quot;:&quot;EmissionRecordingMethod…&quot;,&quot;fullName&quot;:&quot;EmissionRecordingMethodType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e802&quot;,&quot;label&quot;:&quot;ActivityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;ActivityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ActivityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e801&quot;,&quot;label&quot;:&quot;OrganizationEmissionAll…&quot;,&quot;fullName&quot;:&quot;OrganizationEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e800&quot;,&quot;label&quot;:&quot;RecordingUncertaintyAss…&quot;,&quot;fullName&quot;:&quot;RecordingUncertaintyAssessment&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;RecordingUncertaintyAssessment.html&quot;},{&quot;id&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;FacilityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;FacilityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/FacilityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e788&quot;,&quot;label&quot;:&quot;EmissionStatementPerSta…&quot;,&quot;fullName&quot;:&quot;EmissionStatementPerStandard&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatementPerStandard.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;EmissionComponent&quot;,&quot;fullName&quot;:&quot;EmissionComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponent.html&quot;},{&quot;id&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;EmissionCalculationModel&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModel&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModel.html&quot;},{&quot;id&quot;:&quot;e772&quot;,&quot;label&quot;:&quot;EmissionScopeType&quot;,&quot;fullName&quot;:&quot;EmissionScopeType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionScopeType.html&quot;},{&quot;id&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;EmissionActivity&quot;,&quot;fullName&quot;:&quot;EmissionActivity&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivity.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionInventory.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c823&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c825&quot;,&quot;source&quot;:&quot;e801&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c826&quot;,&quot;source&quot;:&quot;e800&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c827&quot;,&quot;source&quot;:&quot;e799&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c840&quot;,&quot;source&quot;:&quot;e776&quot;,&quot;target&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c846&quot;,&quot;source&quot;:&quot;e788&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c865&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c867&quot;,&quot;source&quot;:&quot;e776&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c868&quot;,&quot;source&quot;:&quot;e777&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c869&quot;,&quot;source&quot;:&quot;e772&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c870&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c871&quot;,&quot;source&quot;:&quot;e771&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c822&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c864&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c863&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*