# <span class="sl" data-layer="uml">work-product-component</span> ProductFootprintDataQualityIndicator

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductFootprintDataQualityIndicator is a work-product-component that records a structured assessment of the data quality dimensions for a specific ProductCarbonFootprint, using the five DQI dimensions defined in the PACT technical specification: completeness, geographical representativeness, reliability, technological representativeness, and temporal representativeness. Each dimension is rated on a scale of 1 (very good) to 3 (acceptable) as a minimum, enabling systematic data quality scoring, disclosure, and improvement prioritisation across PCF datasets.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductFootprintDataQualityIndicator record, linked one-to-one with a ProductCarbonFootprint. |
| product_carbon_footprint_id | String |  | Foreign key to the ProductCarbonFootprint that this data quality assessment covers, establishing the one-to-one relationship between a PCF and its DQI record. |
| completeness_data_quality_rating | String |  | A rating from 1 (very good) to 3 (acceptable) for the completeness dimension, reflecting the degree to which all material emission flows and life-cycle stages have been included in the PCF data. |
| geographical_data_quality_rating | String |  | A rating from 1 to 3 for the geographical representativeness dimension, reflecting how closely the emission factor geographies used match the actual production geography of the product. |
| reliability_data_quality_rating | String |  | A rating from 1 to 3 for the reliability dimension, reflecting the quality and credibility of the data sources and measurement methods used, with 1 indicating measured or verified data and 3 indicating modelled or estimated data. |
| technological_data_quality_rating | String |  | A rating from 1 to 3 for the technological representativeness dimension, reflecting whether the emission factors and activity data accurately represent the specific technology used to produce the product. |
| temporal_data_quality_rating | String |  | A rating from 1 to 3 for the temporal representativeness dimension, reflecting whether the emission factors and activity data are current and representative of the period in which the product was manufactured. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductFootprintDataQualityIndicator is a work-product-component that records a structured assessment of the data quality dimensions for a specific ProductCarbonFootprint, using the five DQI dimensions defined in the PACT technical specification: complete |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"},{"id":"e821","label":"ProductFootprintDataQua…","fullName":"ProductFootprintDataQualityIndicator","packageName":"Products","isFocal":true,"hasUrl":false,"url":""},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e820","label":"ProductCategoryRule","fullName":"ProductCategoryRule","packageName":"Products","isFocal":false,"hasUrl":true,"url":"ProductCategoryRule.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprintFactorSource.html"},{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":"Products","isFocal":false,"hasUrl":true,"url":"ProductLifeCycleFootprint.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association"},{"id":"c795","source":"e812","target":"e821","label":"Association"},{"id":"c796","source":"e812","target":"e820","label":"Association"},{"id":"c798","source":"e812","target":"e818","label":"Association"},{"id":"c803","source":"e812","target":"e817","label":"Association"},{"id":"c804","source":"e811","target":"e812","label":"Association"},{"id":"c833","source":"e779","target":"e812","label":"Association"},{"id":"c881","source":"e755","target":"e755","label":"Association"},{"id":"c794","source":"e811","target":"e811","label":"Association"}]};</script>

---

*Generated: 2026-06-26 15:09:23*