# <span class="sl" data-layer="uml">work-product-component</span> ProductCarbonFootprintFactorSource

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductCarbonFootprintFactorSource is an intersection entity that records which emission factor databases were used as secondary or background data sources in calculating a specific ProductCarbonFootprint. This entity provides the structured traceability required by the PACT technical specification secondary_emission_factor_sources field, capturing not just the database name but also the version and applicable life-cycle stage, enabling systematic data quality assessment by receivers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductCarbonFootprintFactorSource record, used to enumerate all background databases applied in a PCF calculation. |
| product_carbon_footprint_id | String |  | Foreign key to the ProductCarbonFootprint for which this emission factor source was used, grouping all secondary database references under the relevant PCF. |
| emission_factor_source_id | String |  | Foreign key to the EmissionFactorSource record that identifies the secondary database, providing structured traceability to the source rather than a free-text citation. |
| applicable_life_cycle_stage_id | String |  | Foreign key to the ProductLifeCycleStage to which this factor source was applied, enabling stage-level data quality assessment when multiple databases were used for different stages. |
| notes | String |  | Free-text notes describing how this source was applied, including any version adjustments or geographic mapping decisions made when using the database for this specific product. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductCarbonFootprintFactorSource is an intersection entity that records which emission factor databases were used as secondary or background data sources in calculating a specific ProductCarbonFootprint. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [EmissionFactorSource](../Emissions/EmissionFactorSource.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [EmissionFactorSource](../Emissions/EmissionFactorSource.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionFactorSource.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":Products,"isFocal":true,"hasUrl":false,"url":""},{"id":"e755","label":"Location","fullName":"Location","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e821","label":"ProductFootprintDataQua…","fullName":"ProductFootprintDataQualityIndicator","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductFootprintDataQualityIndicator.html"},{"id":"e820","label":"ProductCategoryRule","fullName":"ProductCategoryRule","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductCategoryRule.html"},{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductLifeCycleFootprint.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/StandardSourceAssociation.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionFactor.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association"},{"id":"c795","source":"e812","target":"e821","label":"Association"},{"id":"c796","source":"e812","target":"e820","label":"Association"},{"id":"c798","source":"e812","target":"e818","label":"Association"},{"id":"c803","source":"e812","target":"e817","label":"Association"},{"id":"c804","source":"e811","target":"e812","label":"Association"},{"id":"c833","source":"e779","target":"e812","label":"Association"},{"id":"c806","source":"e781","target":"e794","label":"Association"},{"id":"c835","source":"e781","target":"e818","label":"Association"},{"id":"c849","source":"e794","target":"e781","label":"Association"},{"id":"c857","source":"e780","target":"e781","label":"Association"},{"id":"c881","source":"e755","target":"e755","label":"Association"},{"id":"c794","source":"e811","target":"e811","label":"Association"},{"id":"c854","source":"e779","target":"e780","label":"Association"},{"id":"c855","source":"e779","target":"e780","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:44:31*