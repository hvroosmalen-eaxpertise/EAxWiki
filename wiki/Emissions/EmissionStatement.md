---
ea_id: 776
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 754fa64a
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionStatement

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="776" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="776" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionStatement is the central work-product-component that records a single quantified emission result: the GHG emissions or removals attributable to one EmissionActivity within one reporting period. It links the activity, the organisational boundary, the scope type, the calculation model used, and the resulting emission quantity and unit, forming the atomic building block of an emission inventory. Multiple statements are aggregated into an EmissionInventory to produce total scope-level and entity-level disclosures.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system identifier for this EmissionStatement record, used to link emission components, per-standard breakdowns, and uncertainty assessments to this specific quantified result.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="c247e3da" data-kind="attribute" data-el-id="776" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_inventory_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionInventory that this statement belongs to, grouping the statement within its parent accounting exercise.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="bf7c466d" data-kind="attribute" data-el-id="776" data-attr-name="emission_inventory_id" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionActivity that generated or absorbed the emissions recorded in this statement.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="425450bc" data-kind="attribute" data-el-id="776" data-attr-name="emission_activity_id" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_scope_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the EmissionScopeType (Scope 1, 2, or 3) attributed to this statement, derived from the activity category unless overridden by a specific business rule.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="b2ed93b2" data-kind="attribute" data-el-id="776" data-attr-name="emission_scope_type_id" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_model_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>Foreign key to the EmissionCalculationModel applied to derive the emission quantity from activity data, capturing the methodological choice made for this statement.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="9cc11ed7" data-kind="attribute" data-el-id="776" data-attr-name="emission_calculation_model_id" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The total GHG emission or removal quantity calculated for this activity instance, expressed in the unit of measure referenced by quantity_unit_of_measure_id.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="e4983aba" data-kind="attribute" data-el-id="776" data-attr-name="quantity" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>Foreign key to the UnitOfMeasure record expressing the unit in which the emission quantity is reported, typically tCO2e.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="1149bd24" data-kind="attribute" data-el-id="776" data-attr-name="quantity_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>reporting_period_start</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The start date of the specific period to which this statement applies, which may be shorter than the parent inventory period for partial-period corrections.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="e1bcd881" data-kind="attribute" data-el-id="776" data-attr-name="reporting_period_start" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>reporting_period_end</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>The end date of the period to which this statement applies, enabling time-series analysis within and across inventory cycles.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="e0455838" data-kind="attribute" data-el-id="776" data-attr-name="reporting_period_end" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
<tr><td>recording_method_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-9--><p>Foreign key to the EmissionRecordingMethodType that indicates whether the emission quantity was measured, calculated, estimated, or derived from a default factor, supporting data quality assessments.</p><!--ea-row-notes-end:attr-9--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-9" data-notes-hash="8f373a0c" data-kind="attribute" data-el-id="776" data-attr-name="recording_method_type_id" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-9" style="display:none"><td colspan="4"></td></tr>
<tr><td>notes</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-10--><p>Free-text field for the preparer to record assumptions, data sources, or explanatory comments specific to this emission statement that support auditability.</p><!--ea-row-notes-end:attr-10--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-10" data-notes-hash="917e87ef" data-kind="attribute" data-el-id="776" data-attr-name="notes" data-attr-type="String" data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-10" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionStatement is the central work-product-component that records a single quantified emission result: the GHG emissions or removals attributable to one EmissionActivity within one reporting period.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="776" data-tag-name="description" data-tag-value="EmissionStatement is the central work-product-component that records a single quantified emission result: the GHG emissions or removals attributable to one EmissionActivity within one reporting period." data-file-path="Emissions/EmissionStatement.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.html) |
| Association |  | [OrganizationEmissionAllocation](OrganizationEmissionAllocation.html) |
| Association |  | [RecordingUncertaintyAssessment](RecordingUncertaintyAssessment.html) |
| Association |  | Element ID 799 (not in export) |
| Association |  | [FacilityEmissionAllocation](../Facilities/FacilityEmissionAllocation.html) |
| Association |  | [EmissionStatementPerStandard](EmissionStatementPerStandard.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionComponent](EmissionComponent.html) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.html) |
| Association |  | [EmissionScopeType](EmissionScopeType.html) |
| Association |  | [EmissionActivity](EmissionActivity.html) |
| Association |  | [EmissionInventory](EmissionInventory.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionInventory](EmissionInventory.html) |
| Association |  | [EmissionScopeType](EmissionScopeType.html) |
| Association |  | [EmissionActivity](EmissionActivity.html) |
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.html) |
| Association |  | [OrganizationEmissionAllocation](OrganizationEmissionAllocation.html) |
| Association |  | [RecordingUncertaintyAssessment](RecordingUncertaintyAssessment.html) |
| Association |  | Element ID 799 (not in export) |
| Association |  | [EmissionStatementPerStandard](EmissionStatementPerStandard.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="776"></div>

---

*Generated: 2026-07-31 18:00:34*