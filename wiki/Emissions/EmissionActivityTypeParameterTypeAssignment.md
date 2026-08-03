---
ea_id: 807
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 76ee9f40
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityTypeParameterTypeAssignment

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="807" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="807" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement is mandatory or optional. Each template is composed of one or more of these assignment records, forming the complete parameter checklist for the associated activity type and jurisdiction.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this parameter type assignment record, used to enumerate the complete list of required measurements within a recording template.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="9aa8b175" data-kind="attribute" data-el-id="807" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_parameter_recording_template_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the parent EmissionActivityParameterRecordingTemplate that this assignment belongs to, grouping all parameter requirements for a given activity type and jurisdiction.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="a2306d98" data-kind="attribute" data-el-id="807" data-attr-name="emission_activity_parameter_recording_template_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_parameter_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionParameterType that must be recorded, identifying the specific measurement slot that this assignment adds to the template.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="ed353fa3" data-kind="attribute" data-el-id="807" data-attr-name="emission_parameter_type_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>is_mandatory</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Indicates whether measurement of this parameter type is required (true) or optional (false) for the activity type and jurisdiction defined by the parent template, supporting data completeness validation.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="1bbe796e" data-kind="attribute" data-el-id="807" data-attr-name="is_mandatory" data-attr-type="String" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>sequence_number</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The display order of this parameter assignment within the template, used to present data entry fields in a logical sequence to facility operators.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="77199061" data-kind="attribute" data-el-id="807" data-attr-name="sequence_number" data-attr-type="String" data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement </td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="807" data-tag-name="description" data-tag-value="EmissionActivityTypeParameterTypeAssignment is a master-data entity that records a single parameter type requirement within an EmissionActivityParameterRecordingTemplate, specifying which EmissionParameterType must be recorded and whether the measurement " data-file-path="Emissions/EmissionActivityTypeParameterTypeAssignment.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.html) |
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="807"></div>

---

*Generated: 2026-08-03 08:46:17*