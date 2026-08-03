---
ea_id: 762
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7adb7be2
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityParameter

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="762" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="762" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations. It links a Facility and optionally an Equipment item to an EmissionParameterType and to the EmissionActivity it monitors, providing the structural metadata that describes what is being measured or estimated for a given activity. Parameter values over time are recorded in the associated EmissionActivityParameterValue entity.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the EmissionActivityParameter record. It is the primary key referenced by EmissionActivityParameterValue to associate time-series values with this parameter definition.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="f097fe40" data-kind="attribute" data-el-id="762" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The descriptive name of this specific parameter instance, such as "Tower A Gas Flow Rate" or "Boiler 3 Natural Gas Consumption", distinguishing this parameter instance from other parameter definitions on the same facility or activity.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="cf351955" data-kind="attribute" data-el-id="762" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>equipment_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>An optional foreign key identifying the Equipment item to which this parameter applies, enabling asset-level activity data tracking when the parameter is equipment-specific rather than facility-level.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="ea5f48ec" data-kind="attribute" data-el-id="762" data-attr-name="equipment_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key identifying the Facility to which this emission activity parameter is assigned, linking the parameter definition to the physical site context.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="886c06d4" data-kind="attribute" data-el-id="762" data-attr-name="facility_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A foreign key identifying the EmissionActivity that this parameter monitors, linking the activity data definition to the specific emission-generating or emission-removing process.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="36b0b88f" data-kind="attribute" data-el-id="762" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_parameter_type</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>A foreign key referencing the EmissionParameterType that defines the kind of parameter being monitored, such as "Energy Quantity", "Material Consumption Rate", or "Product Yield".</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="e30ce70a" data-kind="attribute" data-el-id="762" data-attr-name="emission_parameter_type" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>A foreign key referencing the UnitOfMeasure applicable to values recorded for this parameter, such as cubic metres per hour or megawatt-hours.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="7ccae1e0" data-kind="attribute" data-el-id="762" data-attr-name="unit_of_measure_id" data-attr-type="String" data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="762" data-tag-name="description" data-tag-value="EmissionActivityParameter identifies a specific instance of an EmissionParameterType that is applicable to a facility or equipment item for use in emission calculations." data-file-path="Facilities/EmissionActivityParameter.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](../Emissions/EmissionParameterType.html) |
| Association |  | [EmissionActivityParameterValue](EmissionActivityParameterValue.html) |
| Association |  | [Facility](Facility.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.html) |
| Association |  | [EmissionParameterType](../Emissions/EmissionParameterType.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="762"></div>

---

*Generated: 2026-08-03 10:55:46*