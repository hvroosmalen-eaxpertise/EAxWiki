---
ea_id: 755
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 38cbb578
---

# <span class="sl" data-layer="uml">master-data</span> Location

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="755" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Facilities/Location.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="755" data-file-path="Facilities/Location.md" data-api-port="8001">
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
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system-assigned identifier for the Location record. It is the primary key referenced by Address and by other Location records through the parent_location_id self-referential attribute. It must be globally unique and stable to support reliable geographic lookups and hierarchical traversal.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="5cd9f197" data-kind="attribute" data-el-id="755" data-attr-name="id" data-attr-type="Key" data-file-path="Facilities/Location.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>iso_alpha_3_code</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The three-letter ISO 3166-1 alpha-3 country code associated with this location, such as "BEL" for Belgium or "NLD" for the Netherlands. This attribute enables unambiguous identification of the country context for geographic analysis and regulatory jurisdiction mapping.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="43e6a26d" data-kind="attribute" data-el-id="755" data-attr-name="iso_alpha_3_code" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_location_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A foreign key referencing the parent Location record in the geographic hierarchy, enabling multi-level hierarchies such as Americas to United States to Texas to Site A. Implementations must enforce acyclicity on this self-referential relationship.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="2047fc00" data-kind="attribute" data-el-id="755" data-attr-name="parent_location_id" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>facility_location_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A foreign key referencing the FacilityLocationType that classifies this location record as a Geospatial Location, Geopolitical Entity, or Business Area. The type determines how the location is interpreted and which subtype entity provides additional attributes.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="7d27584b" data-kind="attribute" data-el-id="755" data-attr-name="facility_location_type_id" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>effective_datetime</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The date and time from which this location record is valid, expressed in ISO 8601 format. The effective_datetime supports the tracking of geographic boundary changes or re-assignments of facilities to new location records over time, preserving a historical record of geographic attribution.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="d5fb8b35" data-kind="attribute" data-el-id="755" data-attr-name="effective_datetime" data-attr-type="String" data-file-path="Facilities/Location.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>Location represents a place where a person or thing is located.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="755" data-tag-name="description" data-tag-value="Location represents a place where a person or thing is located." data-file-path="Facilities/Location.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [Location](Location.md) |
| Association |  | [BusinessArea](BusinessArea.md) |
| Association |  | [GeospatialLocation](GeospatialLocation.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.md) |
| Association |  | [Location](Location.md) |
| Association |  | [BusinessArea](BusinessArea.md) |
| Association |  | [GeospatialLocation](GeospatialLocation.md) |
| Association |  | [GeopoliticalEntity](GeopoliticalEntity.md) |
| Association |  | [FacilityLocationType](FacilityLocationType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e767&quot;,&quot;label&quot;:&quot;BusinessArea&quot;,&quot;fullName&quot;:&quot;BusinessArea&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;BusinessArea.html&quot;},{&quot;id&quot;:&quot;e768&quot;,&quot;label&quot;:&quot;GeospatialLocation&quot;,&quot;fullName&quot;:&quot;GeospatialLocation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;GeospatialLocation.html&quot;},{&quot;id&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;GeopoliticalEntity&quot;,&quot;fullName&quot;:&quot;GeopoliticalEntity&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;GeopoliticalEntity.html&quot;},{&quot;id&quot;:&quot;e769&quot;,&quot;label&quot;:&quot;FacilityLocationType&quot;,&quot;fullName&quot;:&quot;FacilityLocationType&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationType.html&quot;},{&quot;id&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;FacilityLocationAssocia…&quot;,&quot;fullName&quot;:&quot;FacilityLocationAssociation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;FacilityLocationAssociation.html&quot;},{&quot;id&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Location&quot;,&quot;fullName&quot;:&quot;Location&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;ProductFootprintDataQua…&quot;,&quot;fullName&quot;:&quot;ProductFootprintDataQualityIndicator&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductFootprintDataQualityIndicator.html&quot;},{&quot;id&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;ProductCategoryRule&quot;,&quot;fullName&quot;:&quot;ProductCategoryRule&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCategoryRule.html&quot;},{&quot;id&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;ProductCarbonFootprintF…&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprintFactorSource&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprintFactorSource.html&quot;},{&quot;id&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;ProductLifeCycleFootpri…&quot;,&quot;fullName&quot;:&quot;ProductLifeCycleFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductLifeCycleFootprint.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductFootprint.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e806&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameterRecordingTemplate&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Emissions/EmissionActivityParameterRecordingTemplate.html&quot;},{&quot;id&quot;:&quot;e765&quot;,&quot;label&quot;:&quot;GeopoliticalEntityType&quot;,&quot;fullName&quot;:&quot;GeopoliticalEntityType&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;GeopoliticalEntityType.html&quot;},{&quot;id&quot;:&quot;e745&quot;,&quot;label&quot;:&quot;Country&quot;,&quot;fullName&quot;:&quot;Country&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Country.html&quot;},{&quot;id&quot;:&quot;e753&quot;,&quot;label&quot;:&quot;Facility&quot;,&quot;fullName&quot;:&quot;Facility&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Facility.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c792&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c795&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e821&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c796&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e820&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c798&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c803&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e817&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c804&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c882&quot;,&quot;source&quot;:&quot;e767&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c883&quot;,&quot;source&quot;:&quot;e768&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c813&quot;,&quot;source&quot;:&quot;e806&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c884&quot;,&quot;source&quot;:&quot;e766&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c885&quot;,&quot;source&quot;:&quot;e765&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c886&quot;,&quot;source&quot;:&quot;e766&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c893&quot;,&quot;source&quot;:&quot;e745&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c872&quot;,&quot;source&quot;:&quot;e769&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c887&quot;,&quot;source&quot;:&quot;e769&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c888&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c889&quot;,&quot;source&quot;:&quot;e753&quot;,&quot;target&quot;:&quot;e758&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c881&quot;,&quot;source&quot;:&quot;e755&quot;,&quot;target&quot;:&quot;e755&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*