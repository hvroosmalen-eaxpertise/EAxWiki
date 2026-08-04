---
ea_id: 766
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: e81450d9
---

# <span class="sl" data-layer="uml">master-data</span> GeopoliticalEntity

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="766" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="766" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces. It is classified by a GeopoliticalEntityType and optionally linked to a Country. Examples include "USA", "Canada", "United Kingdom", "Yellow Stone National Park", "Alberta", and "North Sea". Geopolitical entities provide the administrative geographic context required for regulatory jurisdiction mapping, reporting by jurisdiction, and alignment with country-level emission factor datasets.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the GeopoliticalEntity record. This serves as both the primary key of the entity and as the foreign key value linking to a Location record.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="5ede3941" data-kind="attribute" data-el-id="766" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The official name of the geopolitical entity as it is defined and administered by the relevant authority, such as "Flanders", "Bavaria", or "Yellow Stone National Park".</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="098e87d8" data-kind="attribute" data-el-id="766" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>geopolitical_entity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key referencing the GeopoliticalEntityType that classifies this entity, such as "Country", "State", or "Nature Reserve".</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="492fcb4f" data-kind="attribute" data-el-id="766" data-attr-name="geopolitical_entity_type_id" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the parent Location record in the geographic hierarchy, for example linking a State to its Country parent.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="ebdb3bdb" data-kind="attribute" data-el-id="766" data-attr-name="parent_location_id" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The date and time from which this geopolitical entity record is valid, in ISO 8601 format. Supports tracking of boundary changes or the creation of new entities over time.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="ac25fdf7" data-kind="attribute" data-el-id="766" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>termination_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The date and time at which this geopolitical entity record is terminated, in ISO 8601 format. Null if the entity is currently active.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="c7ba9b96" data-kind="attribute" data-el-id="766" data-attr-name="termination_datetime" data-attr-type="String" data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="766" data-tag-name="description" data-tag-value="GeopoliticalEntity represents a named geographical area that is defined and administered by an official entity, such as countries, nature reserves, states, regions, or provinces." data-file-path="Facilities/GeopoliticalEntity.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityParameterRecordingTemplate](../Emissions/EmissionActivityParameterRecordingTemplate.html) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.html) |
| Association |  | [GeopoliticalEntityType](GeopoliticalEntityType.html) |
| Association |  | [Location](Location.html) |
| Association |  | [Country](../Organisation/Country.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Country](../Organisation/Country.html) |
| Association |  | [GeopoliticalEntityType](GeopoliticalEntityType.html) |
| Association |  | [EmissionActivityParameterRecordingTemplate](../Emissions/EmissionActivityParameterRecordingTemplate.html) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="766"></div>

---

*Generated: 2026-08-04 12:35:51*