---
ea_id: 760
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: c4a54ffa
---

# <span class="sl" data-layer="uml">master-data</span> FacilityActivityParticipation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="760" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="760" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
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
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the FacilityActivityParticipation record. It is the primary key for this participation link and must remain stable for reporting history and audit purposes.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="c6b28400" data-kind="attribute" data-el-id="760" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A foreign key identifying the EmissionActivity in which the referenced facility participates, linking the physical facility to the specific emission-generating or emission-removing process.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e62822e9" data-kind="attribute" data-el-id="760" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the Facility that participates in the referenced emission activity, linking the physical site to the activity for geographic attribution and site-level reporting.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="401aa9be" data-kind="attribute" data-el-id="760" data-attr-name="facility_id" data-attr-type="String" data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

<details class="ea-section" data-ea-section-id="tagged-values" markdown="1">
<summary><h2 id="tagged-values">Tagged Values</h2></summary>

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="760" data-tag-name="description" data-tag-value="FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity." data-file-path="Facilities/FacilityActivityParticipation.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

</details>

<details class="ea-section" data-ea-section-id="relationships" markdown="1">
<summary><h2 id="relationships">Relationships</h2></summary>

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.html) |
| Association |  | [Facility](Facility.html) |

</details>

## Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

<details class="ea-section" data-ea-section-id="referenced-by" markdown="1">
<summary><h2 id="referenced-by">Referenced By</h2></summary>

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.html) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.html) |

</details>

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="760"></div>

<!-- ea-element-template:v3 -->
