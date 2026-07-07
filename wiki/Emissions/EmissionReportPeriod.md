---
ea_id: 783
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2827b849
---

# <span class="sl" data-layer="uml">master-data</span> EmissionReportPeriod

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionReportPeriod is a work-product-component that holds the aggregated emission totals for a specific time period within an EmissionReport, broken down by scope and boundary. It carries the period dates and may record the period at year, quarter, or month granularity. Multiple period records within a single report support multi-year trend disclosures and interim reporting required by frameworks such as CDP or ESRS E1.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionReportPeriod record, used to link period-level totals to the parent EmissionReport. |
| emission_report_id | String |  | Foreign key to the parent EmissionReport that this period record belongs to, grouping period totals within their formal disclosure artefact. |
| reporting_period_start | String |  | The first day of the reporting period that this record covers, used together with reporting_period_end to define the temporal window for emission aggregation. |
| reporting_period_end | String |  | The last day of the reporting period, enabling both annual and sub-annual (quarterly, monthly) aggregation and disclosure granularity. |
| year | String |  | The calendar or fiscal year of the reporting period, stored as a discrete attribute to support year-over-year comparison queries without requiring date arithmetic. |
| quarter | String |  | The calendar quarter (1-4) of the reporting period, populated for quarterly disclosures and null for annual or monthly records. |
| month | String |  | The calendar month (1-12) of the reporting period, populated for monthly disclosures and null otherwise, enabling operational monitoring at monthly granularity. |
| total_scope1_quantity | String |  | The aggregated Scope 1 direct emission total for this period, expressed in the unit referenced by quantity_unit_of_measure_id, typically tCO2e. |
| total_scope2_location_quantity | String |  | The aggregated Scope 2 location-based indirect emission total for purchased energy in this period, expressed in tCO2e. |
| total_scope2_market_quantity | String |  | The aggregated Scope 2 market-based indirect emission total using contracted energy instruments for this period, expressed in tCO2e. |
| total_scope3_quantity | String |  | The aggregated Scope 3 indirect emission total across all categories for this period, expressed in tCO2e. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure record for the unit in which all scope totals in this period record are expressed. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionReportPeriod is a work-product-component that holds the aggregated emission totals for a specific time period within an EmissionReport, broken down by scope and boundary. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionReport](EmissionReport.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionReport](EmissionReport.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;EmissionReport&quot;,&quot;fullName&quot;:&quot;EmissionReport&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionReport.html&quot;},{&quot;id&quot;:&quot;e783&quot;,&quot;label&quot;:&quot;EmissionReportPeriod&quot;,&quot;fullName&quot;:&quot;EmissionReportPeriod&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e798&quot;,&quot;label&quot;:&quot;UnitOfMeasureSourceRefe…&quot;,&quot;fullName&quot;:&quot;UnitOfMeasureSourceReference&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasureSourceReference.html&quot;},{&quot;id&quot;:&quot;e797&quot;,&quot;label&quot;:&quot;PhysicalQuantityType&quot;,&quot;fullName&quot;:&quot;PhysicalQuantityType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;PhysicalQuantityType.html&quot;},{&quot;id&quot;:&quot;e796&quot;,&quot;label&quot;:&quot;SystemOfUnits&quot;,&quot;fullName&quot;:&quot;SystemOfUnits&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;SystemOfUnits.html&quot;},{&quot;id&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;EmissionParameterType&quot;,&quot;fullName&quot;:&quot;EmissionParameterType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionParameterType.html&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e763&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameterValue&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/EmissionActivityParameterValue.html&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;EmissionComponent&quot;,&quot;fullName&quot;:&quot;EmissionComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponent.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionInventory.html&quot;},{&quot;id&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Standard&quot;,&quot;fullName&quot;:&quot;Standard&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Standard.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c807&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e783&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c828&quot;,&quot;source&quot;:&quot;e798&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c829&quot;,&quot;source&quot;:&quot;e797&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c830&quot;,&quot;source&quot;:&quot;e796&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c832&quot;,&quot;source&quot;:&quot;e775&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c834&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e763&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c854&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c855&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c864&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c865&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c808&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c837&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e783&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c894&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c895&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c867&quot;,&quot;source&quot;:&quot;e776&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c871&quot;,&quot;source&quot;:&quot;e771&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c917&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c918&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*