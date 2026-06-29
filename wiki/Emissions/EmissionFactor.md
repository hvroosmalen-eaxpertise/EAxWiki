# <span class="sl" data-layer="uml">master-data</span> EmissionFactor

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source. Factors are typed by the component category they represent (e.g. CO2 fossil, CH4), scoped by applicability (geography, activity type, technology, time period), and versioned to support year-over-year comparability. They form the primary input to activity-based calculation models.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionFactor record, referenced by EmissionActivityFactor associations to map an activity type to the factor applicable in a given context. |
| emission_factor_source_id | String |  | Foreign key to the EmissionFactorSource that published this factor, enabling traceability to the originating database or official publication. |
| emission_component_category_id | String |  | Foreign key to the EmissionComponentCategory that this factor applies to, such as CO2 fossil or CH4, determining which component category the factor quantity feeds. |
| activity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure of the activity parameter denominator, such as MWh for an electricity emission factor, defining what one unit of activity produces. |
| factor_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure of the emission quantity numerator, typically kgCO2 or kgCH4, expressing the gas mass emitted per unit of activity. |
| value | String |  | The numeric emission factor coefficient: the quantity of greenhouse gas emitted per unit of activity. |
| geography | String |  | The geographic scope for which this factor is applicable, expressed as an ISO 3166-1 alpha-2 country code or a regional grouping such as EU27, used to select the correct factor for a given facility location. |
| valid_from | String |  | The start date of the period for which this factor is valid, used by calculation engines to select the factor appropriate to a specific reporting year. |
| valid_to | String |  | The end date of the factor applicability period. A null value indicates the factor remains current, while a populated date triggers selection of a more recent factor for periods after this date. |
| version | String |  | The version label of this factor within its source database, such as 2024 v1.0, supporting audit trails and reproducibility of historical calculations. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactorSource.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormula.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasureSourceReference.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"PhysicalQuantityType.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"SystemOfUnits.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameterValue.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e795","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategoryGroup","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategoryGroup.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprintFactorSource.html"}],"edges":[{"id":"c817","source":"e804","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c818","source":"e804","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c821","source":"e803","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c851","source":"e793","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c853","source":"e793","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c807","source":"e779","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c828","source":"e798","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c829","source":"e797","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c830","source":"e796","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c834","source":"e779","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c809","source":"e792","target":"e795","label":"Association","sourceLayer":"uml"},{"id":"c831","source":"e795","target":"e792","label":"Association","sourceLayer":"uml"},{"id":"c856","source":"e792","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c866","source":"e792","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c806","source":"e781","target":"e794","label":"Association","sourceLayer":"uml"},{"id":"c835","source":"e781","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c849","source":"e794","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c857","source":"e780","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c859","source":"e777","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c798","source":"e812","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 18:57:23*