# <span class="sl" data-layer="uml">master-data</span> EmissionReportPeriod

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
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
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasureSourceReference.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"PhysicalQuantityType.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"SystemOfUnits.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameterValue.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c807","source":"e779","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c828","source":"e798","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c829","source":"e797","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c830","source":"e796","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c834","source":"e779","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c837","source":"e782","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-26 17:02:52*