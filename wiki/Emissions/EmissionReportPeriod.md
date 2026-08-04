---
ea_id: 783
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2827b849
---

# <span class="sl" data-layer="uml">master-data</span> EmissionReportPeriod

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="783" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="783" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionReportPeriod is a work-product-component that holds the aggregated emission totals for a specific time period within an EmissionReport, broken down by scope and boundary. It carries the period dates and may record the period at year, quarter, or month granularity. Multiple period records within a single report support multi-year trend disclosures and interim reporting required by frameworks such as CDP or ESRS E1.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionReportPeriod record, used to link period-level totals to the parent EmissionReport.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="554b5c87" data-kind="attribute" data-el-id="783" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_report_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the parent EmissionReport that this period record belongs to, grouping period totals within their formal disclosure artefact.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="26d8a7b6" data-kind="attribute" data-el-id="783" data-attr-name="emission_report_id" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>reporting_period_start</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The first day of the reporting period that this record covers, used together with reporting_period_end to define the temporal window for emission aggregation.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="fa42e930" data-kind="attribute" data-el-id="783" data-attr-name="reporting_period_start" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>reporting_period_end</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The last day of the reporting period, enabling both annual and sub-annual (quarterly, monthly) aggregation and disclosure granularity.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="0bafba83" data-kind="attribute" data-el-id="783" data-attr-name="reporting_period_end" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>year</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The calendar or fiscal year of the reporting period, stored as a discrete attribute to support year-over-year comparison queries without requiring date arithmetic.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="f2e214b8" data-kind="attribute" data-el-id="783" data-attr-name="year" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>quarter</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The calendar quarter (1-4) of the reporting period, populated for quarterly disclosures and null for annual or monthly records.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="904810e7" data-kind="attribute" data-el-id="783" data-attr-name="quarter" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>month</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The calendar month (1-12) of the reporting period, populated for monthly disclosures and null otherwise, enabling operational monitoring at monthly granularity.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="3663965d" data-kind="attribute" data-el-id="783" data-attr-name="month" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>total_scope1_quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The aggregated Scope 1 direct emission total for this period, expressed in the unit referenced by quantity_unit_of_measure_id, typically tCO2e.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="ee1d762d" data-kind="attribute" data-el-id="783" data-attr-name="total_scope1_quantity" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>total_scope2_location_quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>The aggregated Scope 2 location-based indirect emission total for purchased energy in this period, expressed in tCO2e.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="9fb4c3f6" data-kind="attribute" data-el-id="783" data-attr-name="total_scope2_location_quantity" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
<tr><td>total_scope2_market_quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-9--><p>The aggregated Scope 2 market-based indirect emission total using contracted energy instruments for this period, expressed in tCO2e.</p><!--ea-row-notes-end:attr-9--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-9" data-notes-hash="74ce0580" data-kind="attribute" data-el-id="783" data-attr-name="total_scope2_market_quantity" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-9" style="display:none"><td colspan="4"></td></tr>
<tr><td>total_scope3_quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-10--><p>The aggregated Scope 3 indirect emission total across all categories for this period, expressed in tCO2e.</p><!--ea-row-notes-end:attr-10--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-10" data-notes-hash="7fa65ec0" data-kind="attribute" data-el-id="783" data-attr-name="total_scope3_quantity" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-10" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-11--><p>Foreign key to the UnitOfMeasure record for the unit in which all scope totals in this period record are expressed.</p><!--ea-row-notes-end:attr-11--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-11" data-notes-hash="7a3f343c" data-kind="attribute" data-el-id="783" data-attr-name="quantity_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-11" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionReportPeriod is a work-product-component that holds the aggregated emission totals for a specific time period within an EmissionReport, broken down by scope and boundary.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="783" data-tag-name="description" data-tag-value="EmissionReportPeriod is a work-product-component that holds the aggregated emission totals for a specific time period within an EmissionReport, broken down by scope and boundary." data-file-path="Emissions/EmissionReportPeriod.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionReport](EmissionReport.html) |

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
| Association |  | [EmissionReport](EmissionReport.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="783"></div>

---

*Generated: 2026-08-04 12:35:51*