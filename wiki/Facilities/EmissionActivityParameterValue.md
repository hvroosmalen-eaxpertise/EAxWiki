# <span class="sl" data-layer="uml">work-product-component</span> EmissionActivityParameterValue

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time. This entity provides the time-series data that feeds into emission calculation models, capturing how activity data values evolve across reporting periods. The datetime attribute records the precise moment the value was measured or estimated, enabling time-series analysis and period-level aggregation in inventory calculations.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the EmissionActivityParameterValue record. It is the primary key and the anchor for a single activity data measurement in the time series. |
| value | String |  | The numeric value of the emission activity parameter at the recorded point in time, expressed in the unit of measure referenced by the parent EmissionActivityParameter. |
| datetime | String |  | The date and time at which this parameter value was recorded or estimated, in ISO 8601 format. This timestamp is the primary temporal key for the time-series record. |
| emission_activity_id | String |  | A foreign key identifying the EmissionActivity with which this parameter value is associated, supporting direct querying of all parameter values for a given activity. |
| emission_activity_parameter_id | String |  | A foreign key identifying the EmissionActivityParameter definition to which this time-series value belongs, linking the measured value to its parameter type, unit, facility, and activity context. |
| emission_parameter_type_id | String |  | A foreign key referencing the EmissionParameterType of the parameter being recorded, enabling direct filtering of parameter values by type. |
| equipment_id | String |  | An optional foreign key identifying the Equipment item from which this parameter value was measured, when the measurement is equipment-specific. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityParameterValue records the actual measured or estimated value of an EmissionActivityParameter at a specific moment in time. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.md) |
| Association |  | [EmissionActivityParameter](EmissionActivityParameter.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityParameter](EmissionActivityParameter.md) |
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":"Facilities","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReportPeriod.html"},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasureSourceReference.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/PhysicalQuantityType.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/SystemOfUnits.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionParameterType.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionFactor.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"Facility.html"}],"edges":[{"id":"c807","source":"e779","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c828","source":"e798","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c829","source":"e797","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c830","source":"e796","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c834","source":"e779","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c841","source":"e775","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c874","source":"e762","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c875","source":"e753","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-26 17:14:27*