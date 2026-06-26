# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationFormulaComponent

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant. This decomposition enables calculation engines to evaluate formulas programmatically from structured data rather than parsed strings, and supports formula-level audit trails that show exactly which factors and parameters contributed to an emission result.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this formula component record, used by calculation engines to retrieve all terms of a formula in the correct evaluation sequence. |
| emission_calculation_formula_id | String |  | Foreign key to the parent EmissionCalculationFormula that this component belongs to, grouping all terms of a single formula expression. |
| sequence_number | String |  | The position of this term in the formula evaluation sequence, used to enforce a defined multiplication or addition order when the formula has more than one term. |
| component_type | String |  | The role of this term in the formula, drawn from values such as ActivityParameter, EmissionFactor, GWPFactor, or Constant, determining how the calculation engine retrieves the term value. |
| emission_factor_id | String |  | Foreign key to the EmissionFactor record that provides this term value when component_type is EmissionFactor; null for parameter or constant terms. |
| parameter_type_id | String |  | Foreign key to the EmissionParameterType that provides this term value when component_type is ActivityParameter; null for factor or constant terms. |
| constant_value | String |  | The fixed numeric value of this term when component_type is Constant, such as a GWP correction coefficient or unit conversion factor embedded directly in the formula. |
| constant_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure for the constant_value, required when the constant carries a unit that participates in dimensional analysis of the overall formula. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionCalculationFormula](EmissionCalculationFormula.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormula.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e807","label":"EmissionActivityTypePar…","fullName":"EmissionActivityTypeParameterTypeAssignment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityTypeParameterTypeAssignment.html"},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelParameterArgument.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameter.html"},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactorSource.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"}],"edges":[{"id":"c811","source":"e807","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c815","source":"e805","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c841","source":"e775","target":"e762","label":"Association","sourceLayer":"uml"},{"id":"c817","source":"e804","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c856","source":"e792","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c857","source":"e780","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c821","source":"e803","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c859","source":"e777","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c816","source":"e805","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c818","source":"e804","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c851","source":"e793","target":"e777","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-26 17:02:52*