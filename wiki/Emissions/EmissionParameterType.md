# <span class="sl" data-layer="uml">master-data</span> EmissionParameterType

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionParameterType is a reference entity that defines a named, typed slot for measurement or operational data used in emission calculation. Examples include fuel quantity consumed (MWh), vehicle distance travelled (km), and refrigerant charge (kg). Each EmissionActivityParameter value recorded against a Facility references an EmissionParameterType to establish what was measured and in what unit, enabling calculation engines to apply the correct emission factor formula.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionParameterType record, referenced by EmissionActivityParameter records and by EmissionCalculationModel argument mappings to identify the correct input slot. |
| name | String |  | The human-readable name of the parameter type, such as Natural Gas Consumption or Electricity Purchased, used in data entry forms and calculation model configuration. |
| description | String |  | A description of what this parameter measures, how it should be obtained, and any measurement or estimation guidance relevant to its use in GHG calculations. |
| data_type | String |  | The expected data type of values recorded against this parameter type, such as Decimal, Integer, or Boolean, used for validation during data entry. |
| unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure record that expresses the default measurement unit for values of this parameter type, such as MWh, km, or kg. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionParameterType is a reference entity that defines a named, typed slot for measurement or operational data used in emission calculation. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionActivityParameter](../Facilities/EmissionActivityParameter.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionActivityTypeParameterTypeAssignment](EmissionActivityTypeParameterTypeAssignment.md) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityTypeParameterTypeAssignment.html"},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelParameterArgument.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameter.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityParameterRecordingTemplate.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormula.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasureSourceReference.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"PhysicalQuantityType.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"SystemOfUnits.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameterValue.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"}],"edges":[{"id":"c811","source":"e807","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c812","source":"e807","target":"e806","label":"Association","sourceLayer":"uml"},{"id":"c815","source":"e805","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c816","source":"e805","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c821","source":"e803","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c807","source":"e779","target":"e783","label":"Association","sourceLayer":"uml"},{"id":"c828","source":"e798","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c829","source":"e797","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c830","source":"e796","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c834","source":"e779","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c841","source":"e775","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c874","source":"e762","target":"e763","label":"Association","sourceLayer":"uml"},{"id":"c875","source":"e753","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c859","source":"e777","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 18:57:23*