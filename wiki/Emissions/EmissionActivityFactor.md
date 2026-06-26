# <span class="sl" data-layer="uml">WOR</span> EmissionActivityFactor

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions. The association may be scoped by geography, time period, or calculation model, allowing the model to represent the context-dependency of factor applicability without embedding applicability rules inside the factor record itself. This pattern supports rule-based factor selection in calculation engines.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityFactor association record, used in calculation audit trails to identify exactly which factor was selected for which activity type in a given context. |
| emission_activity_type_id | String |  | Foreign key to the EmissionActivityType for which this factor is applicable, defining the activity class to which the factor should be applied. |
| emission_factor_id | String |  | Foreign key to the EmissionFactor that provides the coefficient for this activity type in this context, establishing the specific numeric value and its unit. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel under which this activity-to-factor mapping is valid, ensuring that factors are only selected within the model that defines their applicability context. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionActivityType](EmissionActivityType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":Emissions,"isFocal":true,"hasUrl":false,"url":""},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelParameterArgument.html"},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e791","label":"EmissionCalculationMeth…","fullName":"EmissionCalculationMethodType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationMethodType.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormula.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionFactorSource.html"},{"id":"e806","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterRecordingTemplate","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionActivityParameterRecordingTemplate.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"}],"edges":[{"id":"c816","source":"e805","target":"e777","label":"Association"},{"id":"c818","source":"e804","target":"e777","label":"Association"},{"id":"c851","source":"e793","target":"e777","label":"Association"},{"id":"c858","source":"e791","target":"e777","label":"Association"},{"id":"c859","source":"e777","target":"e790","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c916","source":"e734","target":"e777","label":"Association"},{"id":"c817","source":"e804","target":"e780","label":"Association"},{"id":"c820","source":"e803","target":"e780","label":"Association"},{"id":"c852","source":"e793","target":"e780","label":"Association"},{"id":"c854","source":"e779","target":"e780","label":"Association"},{"id":"c855","source":"e779","target":"e780","label":"Association"},{"id":"c856","source":"e792","target":"e780","label":"Association"},{"id":"c857","source":"e780","target":"e781","label":"Association"},{"id":"c814","source":"e806","target":"e786","label":"Association"},{"id":"c853","source":"e793","target":"e786","label":"Association"},{"id":"c862","source":"e786","target":"e773","label":"Association"},{"id":"c821","source":"e803","target":"e790","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*