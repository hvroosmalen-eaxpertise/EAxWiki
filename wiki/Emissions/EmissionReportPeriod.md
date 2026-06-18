# EmissionReportPeriod

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

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

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionReportPeriod is a work-product-component that holds the aggregated emission totals for a specific time period within an EmissionReport, broken down by scope and boundary. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 779 → 783 |
| Association |  | 782 → 783 |

---

*Generated: 2026-06-18 13:26:10*