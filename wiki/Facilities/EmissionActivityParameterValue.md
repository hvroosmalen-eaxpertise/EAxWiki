---
ea_id: 763
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: a10e5b45
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionActivityParameterValue

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="763" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="763" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time. This entity provides the time-series data that feeds into emission calculation models, capturing how activity data values evolve across reporting periods. The datetime attribute records the precise moment the value was measured or estimated, enabling time-series analysis and period-level aggregation in inventory calculations.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the EmissionActivityParameterValue record. It is the primary key and the anchor for a single activity data measurement in the time series.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="3840a2f5" data-kind="attribute" data-el-id="763" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>value</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The numeric value of the emission activity parameter at the recorded point in time, expressed in the unit of measure referenced by the parent EmissionActivityParameter.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="16b9b553" data-kind="attribute" data-el-id="763" data-attr-name="value" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The date and time at which this parameter value was recorded or estimated, in ISO 8601 format. This timestamp is the primary temporal key for the time-series record.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="e4abcc0e" data-kind="attribute" data-el-id="763" data-attr-name="datetime" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key identifying the EmissionActivity with which this parameter value is associated, supporting direct querying of all parameter values for a given activity.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="03f558cf" data-kind="attribute" data-el-id="763" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_parameter_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key identifying the EmissionActivityParameter definition to which this time-series value belongs, linking the measured value to its parameter type, unit, facility, and activity context.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="734b7b0b" data-kind="attribute" data-el-id="763" data-attr-name="emission_activity_parameter_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_parameter_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>A foreign key referencing the EmissionParameterType of the parameter being recorded, enabling direct filtering of parameter values by type.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="36cf7ddf" data-kind="attribute" data-el-id="763" data-attr-name="emission_parameter_type_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>equipment_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>An optional foreign key identifying the Equipment item from which this parameter value was measured, when the measurement is equipment-specific.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="9f7285c2" data-kind="attribute" data-el-id="763" data-attr-name="equipment_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="763" data-tag-name="description" data-tag-value="EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time." data-file-path="Facilities/EmissionActivityParameterValue.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.html) |
| Association |  | [EmissionActivityParameter](EmissionActivityParameter.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityParameter](EmissionActivityParameter.html) |
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="763"></div>

---

*Generated: 2026-08-04 11:36:53*