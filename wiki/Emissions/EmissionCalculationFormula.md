---
ea_id: 790
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 0a5f4094
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationFormula

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="790" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="790" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values. A formula belongs to one calculation model and may be decomposed into multiple EmissionCalculationFormulaComponent records that capture the individual multiplicative terms. Storing formulas as structured data rather than code allows them to be audited, versioned, and applied consistently by calculation engines without custom programming per model.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionCalculationFormula record, referenced by its parent EmissionCalculationModel and by the component records that decompose it into terms. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel that this formula belongs to, ensuring that each model mathematical logic is fully traceable to its component terms. |
| name | String |  | A human-readable label for the formula, such as Fuel Combustion CO2 Formula Tier 2, used in model documentation and audit trail displays. |
| description | String |  | A narrative description of what the formula calculates, what inputs it requires, and any boundary conditions or applicability constraints that govern its use. |
| formula_expression | String |  | A symbolic representation of the formula in a standardised notation, such as E = Q x EF x GWP, providing a human-auditable record of the mathematical relationship before it is encoded as structured components. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.md) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelParameterArgument.html"},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e791","label":"EmissionCalculationMeth…","fullName":"EmissionCalculationMethodType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationMethodType.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"}],"edges":[{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c821","source":"e803","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c816","source":"e805","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c818","source":"e804","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c851","source":"e793","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c858","source":"e791","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c859","source":"e777","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c916","source":"e734","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c815","source":"e805","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c817","source":"e804","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*