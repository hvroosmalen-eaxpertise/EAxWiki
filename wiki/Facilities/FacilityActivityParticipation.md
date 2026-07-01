---
ea_id: 760
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: c4a54ffa
---

# <span class="sl" data-layer="uml">master-data</span> FacilityActivityParticipation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="760" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="760" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity. This intersection entity supports cases where a single emission activity spans multiple facilities, or where multiple emission activities are associated with the same facility, enabling accurate physical attribution of GHG-generating processes to the sites at which they occur.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the FacilityActivityParticipation record. It is the primary key for this participation link and must remain stable for reporting history and audit purposes.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="c6b28400" data-kind="attribute" data-el-id="760" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A foreign key identifying the EmissionActivity in which the referenced facility participates, linking the physical facility to the specific emission-generating or emission-removing process.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e62822e9" data-kind="attribute" data-el-id="760" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the Facility that participates in the referenced emission activity, linking the physical site to the activity for geographic attribution and site-level reporting.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="401aa9be" data-kind="attribute" data-el-id="760" data-attr-name="facility_id" data-attr-type="String" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="760" data-tag-name="description" data-tag-value="FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity." data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.md) |
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;EmissionActivity&quot;,&quot;fullName&quot;:&quot;EmissionActivity&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionActivity.html&quot;},{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Facility.html&quot;},{&quot;id&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;FacilityActivityPartici…&quot;,&quot;fullName&quot;:&quot;FacilityActivityParticipation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e802&quot;,&quot;label&quot;:&quot;ActivityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;ActivityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/ActivityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;EmissionActivityFlow&quot;,&quot;fullName&quot;:&quot;EmissionActivityFlow&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/EmissionActivityFlow.html&quot;},{&quot;id&quot;:&quot;e785&quot;,&quot;label&quot;:&quot;EmissionSink&quot;,&quot;fullName&quot;:&quot;EmissionSink&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionSink.html&quot;},{&quot;id&quot;:&quot;e784&quot;,&quot;label&quot;:&quot;EmissionSource&quot;,&quot;fullName&quot;:&quot;EmissionSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionSource.html&quot;},{&quot;id&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;EmissionActivityCategory&quot;,&quot;fullName&quot;:&quot;EmissionActivityCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionActivityCategory.html&quot;},{&quot;id&quot;:&quot;e786&quot;,&quot;label&quot;:&quot;EmissionActivityType&quot;,&quot;fullName&quot;:&quot;EmissionActivityType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionActivityType.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionStatement.html&quot;},{&quot;id&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;FacilityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;FacilityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameter&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityParameter.html&quot;},{&quot;id&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;EquipmentInstallation&quot;,&quot;fullName&quot;:&quot;EquipmentInstallation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EquipmentInstallation.html&quot;},{&quot;id&quot;:&quot;e759&quot;,&quot;label&quot;:&quot;FacilitySpecification&quot;,&quot;fullName&quot;:&quot;FacilitySpecification&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilitySpecification.html&quot;},{&quot;id&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;FacilityLocationAssocia…&quot;,&quot;fullName&quot;:&quot;FacilityLocationAssociation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationAssociation.html&quot;},{&quot;id&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;FacilityStructure&quot;,&quot;fullName&quot;:&quot;FacilityStructure&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityStructure.html&quot;},{&quot;id&quot;:&quot;e754&quot;,&quot;label&quot;:&quot;FacilityType&quot;,&quot;fullName&quot;:&quot;FacilityType&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityType.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c822&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c836&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c838&quot;,&quot;source&quot;:&quot;e785&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c839&quot;,&quot;source&quot;:&quot;e784&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c842&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c861&quot;,&quot;source&quot;:&quot;e774&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c862&quot;,&quot;source&quot;:&quot;e786&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c863&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c870&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c873&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c875&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c877&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e761&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c879&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c880&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e759&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c889&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c891&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c892&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e754&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c896&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c823&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c840&quot;,&quot;source&quot;:&quot;e776&quot;,&quot;target&quot;:&quot;e764&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c890&quot;,&quot;source&quot;:&quot;e757&quot;,&quot;target&quot;:&quot;e757&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*