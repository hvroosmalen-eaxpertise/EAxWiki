---
ea_id: 758
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 338be351
---

# <span class="sl" data-layer="uml">work-product-component</span> FacilityLocationAssociation

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="758" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="758" data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time. Because facilities can be relocated or their location assignment can change over time, this entity carries effective and termination datetimes to capture the full history of facility-location associations. The FacilityLocationAssociation can be used to determine the geographic location that a physical emission record belongs to, and is particularly important for mobile facilities such as fleets of shipping tankers that operate across multiple locations in sequence.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the FacilityLocationAssociation record. It must remain stable for the full period of the assignment to support geographic attribution of historical emission records.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="dfd6b125" data-kind="attribute" data-el-id="758" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A foreign key identifying the Location to which the facility is assigned during the validity period.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="c4215bfd" data-kind="attribute" data-el-id="758" data-attr-name="location_id" data-attr-type="String" data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key identifying the Facility that is assigned to the referenced location during this association period.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="f7edcbbb" data-kind="attribute" data-el-id="758" data-attr-name="facility_id" data-attr-type="String" data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The date and time from which the facility is assigned to the referenced location, in ISO 8601 format. Together with termination_datetime, this defines the precise temporal window of the geographic assignment.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="ba24786d" data-kind="attribute" data-el-id="758" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>termination_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The date and time at which the facility-location assignment ends, in ISO 8601 format. Null indicates the assignment is currently active.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="02db445b" data-kind="attribute" data-el-id="758" data-attr-name="termination_datetime" data-attr-type="String" data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="758" data-tag-name="description" data-tag-value="FacilityLocationAssociation is the temporal intersection entity that assigns a Facility to a Location at a specific point in time." data-file-path="Facilities/FacilityLocationAssociation.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityLocationType](FacilityLocationType.html) |
| Association |  | [Location](Location.html) |
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
| Association |  | [Location](Location.html) |
| Association |  | [FacilityLocationType](FacilityLocationType.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="758"></div>

---

*Generated: 2026-07-17 16:59:36*