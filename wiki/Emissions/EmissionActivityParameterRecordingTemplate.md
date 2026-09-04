---
ea_id: 806
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 28439fce
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivityParameterRecordingTemplate

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="806" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="806" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context. It acts as a data-collection template that tells facility operators which parameters they must record for each activity type, ensuring that all inputs needed by the applicable calculation models are systematically collected. Templates may be jurisdiction-specific to reflect local regulatory data requirements.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this recording template record, referenced by EmissionActivityTypeParameterTypeAssignment records that enumerate the individual parameter slots making up this template.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="086a10e4" data-kind="attribute" data-el-id="806" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionActivityType for which this template defines the required parameter measurements, linking the template to the activity classification it serves.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="2627e4cc" data-kind="attribute" data-el-id="806" data-attr-name="emission_activity_type_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>geopolitical_entity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the GeopoliticalEntity (typically a country or regulatory jurisdiction) for which this template applies, enabling jurisdiction-specific parameter requirements.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="1064890e" data-kind="attribute" data-el-id="806" data-attr-name="geopolitical_entity_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A human-readable label for the template, such as UK Stationary Combustion Natural Gas Tier 2 Parameters, used in data collection system configuration and operator guidance.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="0a80e4a1" data-kind="attribute" data-el-id="806" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A description of the template purpose, the regulatory or methodological rationale for the parameter requirements, and any conditions under which this template is mandatory or optional.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="c3622a45" data-kind="attribute" data-el-id="806" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

<details class="ea-section" data-ea-section-id="tagged-values" markdown="1">
<summary><h2 id="tagged-values">Tagged Values</h2></summary>

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="806" data-tag-name="description" data-tag-value="EmissionActivityParameterRecordingTemplate is a master-data entity that defines the set of EmissionParameterType measurements required for a specific EmissionActivityType in a specific jurisdiction or context." data-file-path="Emissions/EmissionActivityParameterRecordingTemplate.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

</details>

<details class="ea-section" data-ea-section-id="relationships" markdown="1">
<summary><h2 id="relationships">Relationships</h2></summary>

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.html) |
| Association |  | [GeopoliticalEntity](../Facilities/GeopoliticalEntity.html) |
| Association |  | [EmissionActivityType](EmissionActivityType.html) |

</details>

## Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

<details class="ea-section" data-ea-section-id="referenced-by" markdown="1">
<summary><h2 id="referenced-by">Referenced By</h2></summary>

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.html) |

</details>

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="806"></div>

<!-- ea-element-template:v3 -->
