---
ea_id: 769
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 6e276453
---

# <span class="sl" data-layer="uml">reference-data</span> FacilityLocationType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="769" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/FacilityLocationType.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="769" data-file-path="Facilities/FacilityLocationType.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location. This type classification drives which subtype entity provides the detailed location attributes for a given Location record and determines how geographic attribution is performed for emission reporting purposes.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the FacilityLocationType record. It must be stable so that Location records that reference it remain valid when the type vocabulary is extended.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="e5e7e2da" data-kind="attribute" data-el-id="769" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/FacilityLocationType.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The descriptive label for the facility location type, drawn from the three values defined in the standard: "Business Area", "Geopolitical Entity", and "Geospatial Location". The name determines which location subtype provides the detailed attributes for Location records classified under this type.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="9dc6621a" data-kind="attribute" data-el-id="769" data-attr-name="name" data-attr-type="String" data-file-path="Facilities/FacilityLocationType.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="769" data-tag-name="description" data-tag-value="FacilityLocationType is a reference entity that classifies how a facility location is to be determined, distinguishing between the three location subtypes available in the model: Business Area, Geopolitical Entity, and Geospatial Location." data-file-path="Facilities/FacilityLocationType.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.html) |
| Association |  | [Location](Location.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="769"></div>

---

*Generated: 2026-07-17 16:59:36*