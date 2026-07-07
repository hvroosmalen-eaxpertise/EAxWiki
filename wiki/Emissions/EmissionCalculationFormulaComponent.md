---
ea_id: 803
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7cb70136
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationFormulaComponent

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;EmissionParameterType&quot;,&quot;fullName&quot;:&quot;EmissionParameterType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionParameterType.html&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e790&quot;,&quot;label&quot;:&quot;EmissionCalculationForm…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationFormula&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationFormula.html&quot;},{&quot;id&quot;:&quot;e803&quot;,&quot;label&quot;:&quot;EmissionCalculationForm…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationFormulaComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e807&quot;,&quot;label&quot;:&quot;EmissionActivityTypePar…&quot;,&quot;fullName&quot;:&quot;EmissionActivityTypeParameterTypeAssignment&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityTypeParameterTypeAssignment.html&quot;},{&quot;id&quot;:&quot;e805&quot;,&quot;label&quot;:&quot;EmissionCalculationMode…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModelParameterArgument&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModelParameterArgument.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;EmissionActivityParamet…&quot;,&quot;fullName&quot;:&quot;EmissionActivityParameter&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/EmissionActivityParameter.html&quot;},{&quot;id&quot;:&quot;e804&quot;,&quot;label&quot;:&quot;EmissionCalculationMode…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModelFactorArgument&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModelFactorArgument.html&quot;},{&quot;id&quot;:&quot;e793&quot;,&quot;label&quot;:&quot;EmissionActivityFactor&quot;,&quot;fullName&quot;:&quot;EmissionActivityFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityFactor.html&quot;},{&quot;id&quot;:&quot;e792&quot;,&quot;label&quot;:&quot;EmissionComponentCatego…&quot;,&quot;fullName&quot;:&quot;EmissionComponentCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentCategory.html&quot;},{&quot;id&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;EmissionFactorSource&quot;,&quot;fullName&quot;:&quot;EmissionFactorSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactorSource.html&quot;},{&quot;id&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;EmissionCalculationModel&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModel&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModel.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c811&quot;,&quot;source&quot;:&quot;e807&quot;,&quot;target&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c815&quot;,&quot;source&quot;:&quot;e805&quot;,&quot;target&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c819&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e775&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c832&quot;,&quot;source&quot;:&quot;e775&quot;,&quot;target&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c841&quot;,&quot;source&quot;:&quot;e775&quot;,&quot;target&quot;:&quot;e762&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c817&quot;,&quot;source&quot;:&quot;e804&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c820&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c852&quot;,&quot;source&quot;:&quot;e793&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c854&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c855&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c856&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c857&quot;,&quot;source&quot;:&quot;e780&quot;,&quot;target&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c821&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e790&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c859&quot;,&quot;source&quot;:&quot;e777&quot;,&quot;target&quot;:&quot;e790&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c816&quot;,&quot;source&quot;:&quot;e805&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c818&quot;,&quot;source&quot;:&quot;e804&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c851&quot;,&quot;source&quot;:&quot;e793&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*