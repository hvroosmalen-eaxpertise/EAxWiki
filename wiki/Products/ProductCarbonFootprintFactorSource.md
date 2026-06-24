# ProductCarbonFootprintFactorSource

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductCarbonFootprintFactorSource is an intersection entity that records which emission factor databases were used as secondary or background data sources in calculating a specific ProductCarbonFootprint. This entity provides the structured traceability required by the PACT technical specification secondary_emission_factor_sources field, capturing not just the database name but also the version and applicable life-cycle stage, enabling systematic data quality assessment by receivers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductCarbonFootprintFactorSource record, used to enumerate all background databases applied in a PCF calculation. |
| product_carbon_footprint_id | String |  | Foreign key to the ProductCarbonFootprint for which this emission factor source was used, grouping all secondary database references under the relevant PCF. |
| emission_factor_source_id | String |  | Foreign key to the EmissionFactorSource record that identifies the secondary database, providing structured traceability to the source rather than a free-text citation. |
| applicable_life_cycle_stage_id | String |  | Foreign key to the ProductLifeCycleStage to which this factor source was applied, enabling stage-level data quality assessment when multiple databases were used for different stages. |
| notes | String |  | Free-text notes describing how this source was applied, including any version adjustments or geographic mapping decisions made when using the database for this specific product. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductCarbonFootprintFactorSource is an intersection entity that records which emission factor databases were used as secondary or background data sources in calculating a specific ProductCarbonFootprint. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [EmissionFactorSource](../Emissions/EmissionFactorSource.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [EmissionFactorSource](../Emissions/EmissionFactorSource.md) |
| Association |  | [EmissionFactorSource](../Emissions/EmissionFactorSource.md) |

---

*Generated: 2026-06-24 10:33:17*