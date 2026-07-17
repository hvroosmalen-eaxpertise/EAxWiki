---
ea_id: 790
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 0a5f4094
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationFormula

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="790" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="790" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values. A formula belongs to one calculation model and may be decomposed into multiple EmissionCalculationFormulaComponent records that capture the individual multiplicative terms. Storing formulas as structured data rather than code allows them to be audited, versioned, and applied consistently by calculation engines without custom programming per model.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionCalculationFormula record, referenced by its parent EmissionCalculationModel and by the component records that decompose it into terms.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="fdbcb600" data-kind="attribute" data-el-id="790" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_model_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionCalculationModel that this formula belongs to, ensuring that each model mathematical logic is fully traceable to its component terms.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="84d785f8" data-kind="attribute" data-el-id="790" data-attr-name="emission_calculation_model_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>A human-readable label for the formula, such as Fuel Combustion CO2 Formula Tier 2, used in model documentation and audit trail displays.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="13873cca" data-kind="attribute" data-el-id="790" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A narrative description of what the formula calculates, what inputs it requires, and any boundary conditions or applicability constraints that govern its use.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="e3f47e40" data-kind="attribute" data-el-id="790" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>formula_expression</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A symbolic representation of the formula in a standardised notation, such as E = Q x EF x GWP, providing a human-auditable record of the mathematical relationship before it is encoded as structured components.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="09631b45" data-kind="attribute" data-el-id="790" data-attr-name="formula_expression" data-attr-type="String" data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="790" data-tag-name="description" data-tag-value="EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values." data-file-path="Emissions/EmissionCalculationFormula.md" data-api-port="8001" data-api-token="28ba5ca38017843d31ceee1f4b6fb60f5b087780a7c0e6dc" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.html) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.html) |
| Association |  | [EmissionCalculationFormulaComponent](EmissionCalculationFormulaComponent.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="790"></div>

---

*Generated: 2026-07-17 16:59:36*