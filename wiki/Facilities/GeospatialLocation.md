---
ea_id: 768
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: eec547e4
---

# <span class="sl" data-layer="uml">master-data</span> GeospatialLocation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="768" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Facilities](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="768" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>GeospatialLocation provides a spatial representation of a Location, defined by coordinate values and an EPSG coordinate reference system code. It can represent a point on the earth such as a latitude/longitude coordinate pair, or may capture the polygon outline of a location. Both original and normalised coordinate values are stored to preserve the source data while enabling consistent geographic comparison. Geospatial information may be either master data specific to a company location or reference data obtained from a third-party dataset.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the GeospatialLocation record. This serves as both the primary key and as the foreign key value for the corresponding Location record.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="36c59737" data-kind="attribute" data-el-id="768" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>epsg_code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>An EPSG numerical identifier used in geospatial data to reference a specific coordinate reference system, providing a standardised way to represent and work with location-based information. For example, EPSG 4326 refers to the WGS84 coordinate system used in GPS.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="4e2c3c4a" data-kind="attribute" data-el-id="768" data-attr-name="epsg_code" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>normalized_x</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The normalised X (longitude or easting) coordinate value transformed to a standard coordinate system for consistent geographic comparison and display.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="52522858" data-kind="attribute" data-el-id="768" data-attr-name="normalized_x" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>normalized_y</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The normalised Y (latitude or northing) coordinate value transformed to a standard coordinate system for consistent geographic comparison and display.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="924d8243" data-kind="attribute" data-el-id="768" data-attr-name="normalized_y" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>original_x</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The raw X (longitude or easting) coordinate value for this geospatial location in the coordinate reference system identified by the epsg_code attribute, stored as received from the source dataset.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="bee7ed57" data-kind="attribute" data-el-id="768" data-attr-name="original_x" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>original_y</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The raw Y (latitude or northing) coordinate value for this geospatial location in the coordinate reference system identified by the epsg_code attribute, stored as received from the source dataset.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="c188cf07" data-kind="attribute" data-el-id="768" data-attr-name="original_y" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The date and time from which this geospatial location record is valid, in ISO 8601 format.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="f87b2c09" data-kind="attribute" data-el-id="768" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>termination_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The date and time at which this geospatial location record is terminated, in ISO 8601 format. Null if the record is currently active.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="ea154081" data-kind="attribute" data-el-id="768" data-attr-name="termination_datetime" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>A foreign key linking this GeospatialLocation to its parent Location record.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="8ccfda69" data-kind="attribute" data-el-id="768" data-attr-name="location_id" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-9--><p>A foreign key referencing the parent Location record in the geographic hierarchy for this geospatial location.</p><!--ea-row-notes-end:attr-9--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-9" data-notes-hash="309fa8f6" data-kind="attribute" data-el-id="768" data-attr-name="parent_location_id" data-attr-type="String" data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-9" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>GeospatialLocation provides a spatial representation of a Location, defined by coordinate values and an EPSG coordinate reference system code.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="768" data-tag-name="description" data-tag-value="GeospatialLocation provides a spatial representation of a Location, defined by coordinate values and an EPSG coordinate reference system code." data-file-path="Facilities/GeospatialLocation.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Location](Location.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Facilities.html" class="diagram-thumb"><img src="diagrams/Facilities.png" alt="Facilities" loading="lazy"><span>Facilities</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="768"></div>

---

*Generated: 2026-08-03 10:55:47*