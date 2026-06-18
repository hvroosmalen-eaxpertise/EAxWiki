# ProductFootprintDataQualityIndicator

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

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

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductFootprintDataQualityIndicator is a work-product-component that records a structured assessment of the data quality dimensions for a specific ProductCarbonFootprint, using the five DQI dimensions defined in the PACT technical specification: complete |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 812 → 821 |

---

*Generated: 2026-06-18 13:26:10*