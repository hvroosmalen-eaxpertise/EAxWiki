---
ea_id: 780
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 862b6f05
---

# <span class="sl" data-layer="uml">master-data</span> EmissionFactor

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="780" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="780" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source. Factors are typed by the component category they represent (e.g. CO2 fossil, CH4), scoped by applicability (geography, activity type, technology, time period), and versioned to support year-over-year comparability. They form the primary input to activity-based calculation models.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionFactor record, referenced by EmissionActivityFactor associations to map an activity type to the factor applicable in a given context.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="6a932182" data-kind="attribute" data-el-id="780" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_factor_source_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionFactorSource that published this factor, enabling traceability to the originating database or official publication.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="e5152331" data-kind="attribute" data-el-id="780" data-attr-name="emission_factor_source_id" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_component_category_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionComponentCategory that this factor applies to, such as CO2 fossil or CH4, determining which component category the factor quantity feeds.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="4ece7393" data-kind="attribute" data-el-id="780" data-attr-name="emission_component_category_id" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>activity_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the UnitOfMeasure of the activity parameter denominator, such as MWh for an electricity emission factor, defining what one unit of activity produces.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="65d5dabc" data-kind="attribute" data-el-id="780" data-attr-name="activity_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>factor_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>Foreign key to the UnitOfMeasure of the emission quantity numerator, typically kgCO2 or kgCH4, expressing the gas mass emitted per unit of activity.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="275c0c70" data-kind="attribute" data-el-id="780" data-attr-name="factor_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>value</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The numeric emission factor coefficient: the quantity of greenhouse gas emitted per unit of activity.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="ba9866e1" data-kind="attribute" data-el-id="780" data-attr-name="value" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>geography</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The geographic scope for which this factor is applicable, expressed as an ISO 3166-1 alpha-2 country code or a regional grouping such as EU27, used to select the correct factor for a given facility location.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="33ca319a" data-kind="attribute" data-el-id="780" data-attr-name="geography" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_from</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The start date of the period for which this factor is valid, used by calculation engines to select the factor appropriate to a specific reporting year.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="299502c3" data-kind="attribute" data-el-id="780" data-attr-name="valid_from" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_to</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>The end date of the factor applicability period. A null value indicates the factor remains current, while a populated date triggers selection of a more recent factor for periods after this date.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="1401d6ca" data-kind="attribute" data-el-id="780" data-attr-name="valid_to" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
<tr><td>version</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-9--><p>The version label of this factor within its source database, such as 2024 v1.0, supporting audit trails and reproducibility of historical calculations.</p><!--ea-row-notes-end:attr-9--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-9" data-notes-hash="fdf24409" data-kind="attribute" data-el-id="780" data-attr-name="version" data-attr-type="String" data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-9" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="780" data-tag-name="description" data-tag-value="EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source." data-file-path="Emissions/EmissionFactor.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.html) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.html) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.html) |
| Association |  | [EmissionFactorSource](EmissionFactorSource.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.html) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.html) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.html) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="780"></div>

---

*Generated: 2026-07-17 16:59:36*