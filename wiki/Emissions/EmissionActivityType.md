---
ea_id: 786
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7348f5ef
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionActivityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="786" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionActivityType.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="786" data-file-path="Emissions/EmissionActivityType.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory. Examples include Stationary Combustion, Mobile Combustion, Process Emissions, Fugitive Emissions, and Land Use Change. The type is used in calculation model selection and in parameter recording template assignment to determine which measurement parameters are required for a given activity.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionActivityType record, referenced by EmissionActivity records and by EmissionActivityParameterRecordingTemplate assignments to drive parameter requirements.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="fb3d30c5" data-kind="attribute" data-el-id="786" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionActivityType.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The standard label for the activity type, such as Mobile Combustion Road Transport or Process Emissions Cement Clinker Production, used in model configuration and report category labels.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="b994a957" data-kind="attribute" data-el-id="786" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionActivityType.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of the physical or chemical processes that characterise activities of this type and the typical emission sources and parameters associated with them.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="3c98a2fd" data-kind="attribute" data-el-id="786" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionActivityType.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="786" data-tag-name="description" data-tag-value="EmissionActivityType is a reference entity that classifies the technical nature of an emission-generating or emission-removing process, providing a finer-grained taxonomy than EmissionActivityCategory." data-file-path="Emissions/EmissionActivityType.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.html) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.html) |
| Association |  | [EmissionActivity](EmissionActivity.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityParameterRecordingTemplate](EmissionActivityParameterRecordingTemplate.html) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="786"></div>

---

*Generated: 2026-08-03 10:55:47*