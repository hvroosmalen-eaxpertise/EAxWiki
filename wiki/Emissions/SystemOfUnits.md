# <span class="sl" data-layer="uml">REF</span> SystemOfUnits

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

SystemOfUnits is a reference entity that identifies the measurement system to which a UnitOfMeasure belongs, such as the International System of Units (SI), the Imperial system, or US Customary units. Recording the system of units as a reference entity enables validation rules that prevent mixing incompatible unit systems in a single calculation and supports conversion path determination by identifying a common base for inter-system conversions.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this SystemOfUnits record, referenced by UnitOfMeasure records via system_of_units_id to classify each unit into its measurement system family. |
| name | String |  | The standard name for this system of units, such as International System of Units (SI), Imperial, or US Customary, used in validation messages and unit selection guidance. |
| description | String |  | A description of the system origin, authority, and the physical quantities it covers, enabling implementors to understand applicability scope and any known incompatibilities with other systems. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | SystemOfUnits is a reference entity that identifies the measurement system to which a UnitOfMeasure belongs, such as the International System of Units (SI), the Imperial system, or US Customary units. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":Emissions,"isFocal":true,"hasUrl":false,"url":""},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasureSourceReference.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"PhysicalQuantityType.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameterValue.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c807","source":"e779","target":"e783","label":"Association"},{"id":"c828","source":"e798","target":"e779","label":"Association"},{"id":"c829","source":"e797","target":"e779","label":"Association"},{"id":"c830","source":"e796","target":"e779","label":"Association"},{"id":"c832","source":"e775","target":"e779","label":"Association"},{"id":"c833","source":"e779","target":"e812","label":"Association"},{"id":"c834","source":"e779","target":"e763","label":"Association"},{"id":"c854","source":"e779","target":"e780","label":"Association"},{"id":"c855","source":"e779","target":"e780","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*