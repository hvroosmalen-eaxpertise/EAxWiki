---
ea_id: 781
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7abf150f
---

# <span class="sl" data-layer="uml">master-data</span> EmissionFactorSource

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="781" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="781" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn. Examples include the UK DESNZ Conversion Factors, the US EPA Emission Factors Hub, the IPCC Assessment Report factor sets, and the ecoinvent database. Registering sources as a reference entity ensures that every factor used in a calculation can be traced to its origin, supporting third-party verification and regulatory acceptance.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionFactorSource record, referenced by EmissionFactor records to indicate provenance and by StandardSourceAssociation to link source databases to standards.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="e8310979" data-kind="attribute" data-el-id="781" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The official name of the emission factor source, such as UK Department for Energy Security and Net Zero Greenhouse Gas Conversion Factors, used in citations and report references.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e492a92d" data-kind="attribute" data-el-id="781" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A description of the scope, coverage, update frequency, and methodology of the source database, helping practitioners assess suitability for their reporting context.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="dd408a9c" data-kind="attribute" data-el-id="781" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>publisher</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The organisation responsible for maintaining and publishing the emission factor source, such as DESNZ, US EPA, or IPCC, used in bibliographic citations.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="8d038a67" data-kind="attribute" data-el-id="781" data-attr-name="publisher" data-attr-type="String" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>version</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The edition or release version of the source publication, such as 2024 or v8.2, used to distinguish factor sets from different reporting years when multiple versions are retained.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="1cc84934" data-kind="attribute" data-el-id="781" data-attr-name="version" data-attr-type="String" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>publication_date</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The date on which this version of the source was published or made effective, used to determine which version was current during a given reporting period.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="b505dd8e" data-kind="attribute" data-el-id="781" data-attr-name="publication_date" data-attr-type="String" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>url</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>A persistent URL or DOI pointing to the source publication or download location, enabling automated factor retrieval and supporting auditability.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="f1278007" data-kind="attribute" data-el-id="781" data-attr-name="url" data-attr-type="String" data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="781" data-tag-name="description" data-tag-value="EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn." data-file-path="Emissions/EmissionFactorSource.md" data-api-port="8001" data-api-token="cb00ff8e615c4dac03751156a603075df7bdab65ec6c0040" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [StandardSourceAssociation](StandardSourceAssociation.html) |
| Association |  | [ProductCarbonFootprintFactorSource](../Products/ProductCarbonFootprintFactorSource.html) |
| Association |  | [StandardSourceAssociation](StandardSourceAssociation.html) |
| Association |  | [EmissionFactor](EmissionFactor.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [StandardSourceAssociation](StandardSourceAssociation.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="781"></div>

---

*Generated: 2026-07-16 21:11:27*