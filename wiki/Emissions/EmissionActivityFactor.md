---
ea_id: 793
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 179b7ba3
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionActivityFactor

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="793" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="793" data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions. The association may be scoped by geography, time period, or calculation model, allowing the model to represent the context-dependency of factor applicability without embedding applicability rules inside the factor record itself. This pattern supports rule-based factor selection in calculation engines.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionActivityFactor association record, used in calculation audit trails to identify exactly which factor was selected for which activity type in a given context.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="93365d95" data-kind="attribute" data-el-id="793" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionActivityType for which this factor is applicable, defining the activity class to which the factor should be applied.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="6cfce1e3" data-kind="attribute" data-el-id="793" data-attr-name="emission_activity_type_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_factor_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionFactor that provides the coefficient for this activity type in this context, establishing the specific numeric value and its unit.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="80b7da0c" data-kind="attribute" data-el-id="793" data-attr-name="emission_factor_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_model_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the EmissionCalculationModel under which this activity-to-factor mapping is valid, ensuring that factors are only selected within the model that defines their applicability context.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="00f9eed7" data-kind="attribute" data-el-id="793" data-attr-name="emission_calculation_model_id" data-attr-type="String" data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="793" data-tag-name="description" data-tag-value="EmissionActivityFactor is an intersection entity that associates a specific EmissionActivityType with the EmissionFactor applicable to it under given conditions." data-file-path="Emissions/EmissionActivityFactor.md" data-api-port="8001" data-api-token="f3a0491e87a05fb2833bbd1e4cdb8499a0101703959665df" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModel](EmissionCalculationModel.html) |
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [EmissionActivityType](EmissionActivityType.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="793"></div>

---

*Generated: 2026-07-31 18:00:34*