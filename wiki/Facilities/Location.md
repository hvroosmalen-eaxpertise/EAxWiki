---
ea_id: 755
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 38cbb578
---

# <span class="sl" data-layer="uml">master-data</span> Location

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="755" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="755" data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>Location represents a place where a person or thing is located. It can describe geospatial aspects such as latitude/longitude coordinates, geopolitical concepts like a country, or a business area as defined by the organisation. The Location data object allows for a parent/child hierarchy with a potentially unlimited number of levels, and is classified by a FacilityLocationType into one of three subtypes: Geospatial Location, Geopolitical Entity, or Business Area. An effective_datetime attribute records when the location record became valid, supporting historical tracking of geographic assignments.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Location record. It is the primary key referenced by Address and by other Location records through the parent_location_id self-referential attribute. It must be globally unique and stable to support reliable geographic lookups and hierarchical traversal.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="5cd9f197" data-kind="attribute" data-el-id="755" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>iso_alpha_3_code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The three-letter ISO 3166-1 alpha-3 country code associated with this location, such as "BEL" for Belgium or "NLD" for the Netherlands. This attribute enables unambiguous identification of the country context for geographic analysis and regulatory jurisdiction mapping.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="43e6a26d" data-kind="attribute" data-el-id="755" data-attr-name="iso_alpha_3_code" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key referencing the parent Location record in the geographic hierarchy, enabling multi-level hierarchies such as Americas to United States to Texas to Site A. Implementations must enforce acyclicity on this self-referential relationship.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="2047fc00" data-kind="attribute" data-el-id="755" data-attr-name="parent_location_id" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_location_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the FacilityLocationType that classifies this location record as a Geospatial Location, Geopolitical Entity, or Business Area. The type determines how the location is interpreted and which subtype entity provides additional attributes.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="7d27584b" data-kind="attribute" data-el-id="755" data-attr-name="facility_location_type_id" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The date and time from which this location record is valid, expressed in ISO 8601 format. The effective_datetime supports the tracking of geographic boundary changes or re-assignments of facilities to new location records over time, preserving a historical record of geographic attribution.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="d5fb8b35" data-kind="attribute" data-el-id="755" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

<details class="ea-section" data-ea-section-id="tagged-values" markdown="1">
<summary><h2 id="tagged-values">Tagged Values</h2></summary>

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Location represents a place where a person or thing is located.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="755" data-tag-name="description" data-tag-value="Location represents a place where a person or thing is located." data-file-path="Facilities/Location.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

</details>

<details class="ea-section" data-ea-section-id="relationships" markdown="1">
<summary><h2 id="relationships">Relationships</h2></summary>

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.html) |
| Association |  | [Location](Location.html) |
| Association |  | [BusinessArea](BusinessArea.html) |
| Association |  | [GeospatialLocation](GeospatialLocation.html) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.html) |
| Association |  | [FacilityLocationType](FacilityLocationType.html) |
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.html) |

</details>

## Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

<details class="ea-section" data-ea-section-id="referenced-by" markdown="1">
<summary><h2 id="referenced-by">Referenced By</h2></summary>

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.html) |
| Association |  | [Location](Location.html) |
| Association |  | [BusinessArea](BusinessArea.html) |
| Association |  | [GeospatialLocation](GeospatialLocation.html) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.html) |
| Association |  | [FacilityLocationType](FacilityLocationType.html) |

</details>

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="755"></div>

<!-- ea-element-template:v3 -->
