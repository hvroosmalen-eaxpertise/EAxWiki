---
ea_id: 804
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 5be7d0c7
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionCalculationModelFactorArgument

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="804" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="804" data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationModelFactorArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the specific EmissionFactor that should be used to fill that slot during calculation. A model may reference multiple factors; for example, a Scope 2 market-based model may require both an electricity emission factor and a transmission-and-distribution loss factor. Defining factor arguments as data rather than code enables model configuration without software deployment and supports systematic factor update workflows.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this factor argument record, used to enumerate all factor inputs required by a calculation model and to audit which factor was in place during a given reporting period.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="24df32bd" data-kind="attribute" data-el-id="804" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_model_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionCalculationModel that declares this factor argument, grouping all required factor inputs for a model in one place.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="37dd1273" data-kind="attribute" data-el-id="804" data-attr-name="emission_calculation_model_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>argument_name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The named slot identifier within the calculation model, such as electricity_emission_factor or td_loss_factor, matching the variable name used in the formula expression.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="deef37b9" data-kind="attribute" data-el-id="804" data-attr-name="argument_name" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_factor_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the EmissionFactor record bound to this argument slot, providing the specific coefficient the model uses for calculation.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="ea57a6dc" data-kind="attribute" data-el-id="804" data-attr-name="emission_factor_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A description of the role of this factor argument in the calculation, clarifying what emission source or process it represents and why this particular factor was selected.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="bddbe1e2" data-kind="attribute" data-el-id="804" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

<details class="ea-section" data-ea-section-id="tagged-values" markdown="1">
<summary><h2 id="tagged-values">Tagged Values</h2></summary>

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionCalculationModelFactorArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the specific EmissionFactor that should be used to fill that slot during calculation.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="804" data-tag-name="description" data-tag-value="EmissionCalculationModelFactorArgument is a master-data entity that binds a named argument slot in an EmissionCalculationModel to the specific EmissionFactor that should be used to fill that slot during calculation." data-file-path="Emissions/EmissionCalculationModelFactorArgument.md" data-api-port="8001" data-api-token="0a090fdc614acadb47d274812862962392b5fdee6a3e1f83" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

</details>

<details class="ea-section" data-ea-section-id="relationships" markdown="1">
<summary><h2 id="relationships">Relationships</h2></summary>

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.html) |

</details>

## Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="804"></div>

<!-- ea-element-template:v3 -->
