---
ea_id: 798
status: Approved
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 87b42e40
notes_hash: bf8a66ce
---

# <span class="sl" data-layer="uml">reference-data</span> UnitOfMeasureSourceReference

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="798" data-status="Approved" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-approved">Approved</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-07-01


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="798" data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>UnitOfMeasureSourceReference is a reference entity that identifies the authoritative registry or specification from which a UnitOfMeasure definition is drawn. Examples include the UN/CEFACT Common Codes for Units of Measurement, the QUDT Units Ontology, and the NIST SP 811 guide to SI units. Tracking the source reference ensures that unit definitions used in the model can be validated against a canonical authority and that imported data using the same reference can be reconciled without ambiguity.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this UnitOfMeasureSourceReference record, referenced by UnitOfMeasure records via unit_of_measure_source_reference_id to indicate the authority for each unit definition. This attribute was manually changed.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="ae254a45" data-kind="attribute" data-el-id="798" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>The standard name or acronym of the reference authority, such as UN/CEFACT Recommendation 20 or QUDT Units Ontology 2.1, used in citations and unit catalogue metadata.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="0ea2bbec" data-kind="attribute" data-el-id="798" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>url</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A persistent URL or DOI to the authoritative source document or registry, enabling automated validation against the latest version of the reference.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="5b87720d" data-kind="attribute" data-el-id="798" data-attr-name="url" data-attr-type="String" data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A description of the scope and coverage of this reference, including which physical quantities it covers, the update cadence, and how it is used within the Open Footprint Data Model.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="3bcc427b" data-kind="attribute" data-el-id="798" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>UnitOfMeasureSourceReference is a reference entity that identifies the authoritative registry or specification from which a UnitOfMeasure definition is drawn.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="798" data-tag-name="description" data-tag-value="UnitOfMeasureSourceReference is a reference entity that identifies the authoritative registry or specification from which a UnitOfMeasure definition is drawn." data-file-path="Emissions/UnitOfMeasureSourceReference.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="798"></div>

---

*Generated: 2026-08-03 08:46:17*