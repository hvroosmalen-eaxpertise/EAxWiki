---
ea_id: 803
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7cb70136
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationFormulaComponent

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="803" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="803" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant. This decomposition enables calculation engines to evaluate formulas programmatically from structured data rather than parsed strings, and supports formula-level audit trails that show exactly which factors and parameters contributed to an emission result.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this formula component record, used by calculation engines to retrieve all terms of a formula in the correct evaluation sequence.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="c1b0442c" data-kind="attribute" data-el-id="803" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_formula_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the parent EmissionCalculationFormula that this component belongs to, grouping all terms of a single formula expression.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="4dc1bb47" data-kind="attribute" data-el-id="803" data-attr-name="emission_calculation_formula_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>sequence_number</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The position of this term in the formula evaluation sequence, used to enforce a defined multiplication or addition order when the formula has more than one term.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="45a11c54" data-kind="attribute" data-el-id="803" data-attr-name="sequence_number" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>component_type</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The role of this term in the formula, drawn from values such as ActivityParameter, EmissionFactor, GWPFactor, or Constant, determining how the calculation engine retrieves the term value.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="9c2499d1" data-kind="attribute" data-el-id="803" data-attr-name="component_type" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_factor_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>Foreign key to the EmissionFactor record that provides this term value when component_type is EmissionFactor; null for parameter or constant terms.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="69f52fd9" data-kind="attribute" data-el-id="803" data-attr-name="emission_factor_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>parameter_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>Foreign key to the EmissionParameterType that provides this term value when component_type is ActivityParameter; null for factor or constant terms.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="7915cb16" data-kind="attribute" data-el-id="803" data-attr-name="parameter_type_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>constant_value</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The fixed numeric value of this term when component_type is Constant, such as a GWP correction coefficient or unit conversion factor embedded directly in the formula.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="dc34d6ab" data-kind="attribute" data-el-id="803" data-attr-name="constant_value" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>constant_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>Foreign key to the UnitOfMeasure for the constant_value, required when the constant carries a unit that participates in dimensional analysis of the overall formula.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="9f66f0cc" data-kind="attribute" data-el-id="803" data-attr-name="constant_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="803" data-tag-name="description" data-tag-value="EmissionCalculationFormulaComponent is a master-data entity that decomposes an EmissionCalculationFormula into its individual multiplicative or additive terms, each term referencing either an EmissionFactor, an activity parameter type, or a constant." data-file-path="Emissions/EmissionCalculationFormulaComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionParameterType](EmissionParameterType.html) |
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [EmissionCalculationFormula](EmissionCalculationFormula.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="803"></div>

---

*Generated: 2026-08-04 12:35:51*