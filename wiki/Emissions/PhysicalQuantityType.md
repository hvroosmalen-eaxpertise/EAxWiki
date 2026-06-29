# <span class="sl" data-layer="uml">reference-data</span> PhysicalQuantityType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area. It enables dimensional analysis validation in calculation models, ensuring for example that an emission factor expressed in kgCO2/kWh is applied to an activity parameter expressed in an energy unit rather than a mass or distance unit. This prevents a class of calculation errors that are otherwise difficult to detect programmatically.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this PhysicalQuantityType record, referenced by UnitOfMeasure records to establish which physical dimension a unit measures. |
| name | String |  | The standard name for the physical quantity, such as Mass, Energy, Thermodynamic Temperature, or Volume, used in validation error messages and unit catalogue documentation. |
| description | String |  | A description of the physical quantity, its SI base dimensions, and any special considerations relevant to GHG accounting, such as the distinction between higher and lower calorific values for energy quantities. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | PhysicalQuantityType is a reference entity that classifies what physical property a UnitOfMeasure measures, such as Mass, Energy, Volume, Length, Temperature, or Area. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasureSourceReference.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"SystemOfUnits.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameterValue.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c807","source":"e779","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c828","source":"e798","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c829","source":"e797","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c830","source":"e796","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c834","source":"e779","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 13:30:17*