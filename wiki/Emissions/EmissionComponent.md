---
ea_id: 778
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7a155802
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionComponent

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="778" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="778" data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately. This granularity supports characterisation factor application, gas-specific disclosure requirements, and cross-protocol comparability beyond CO2-equivalent totals alone.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionComponent record, used when linking per-standard breakdowns via EmissionComponentPerStandard or when applying gas-specific characterisation factors.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="3af4d033" data-kind="attribute" data-el-id="778" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_statement_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the parent EmissionStatement that this component disaggregates, linking the gas-level detail to its aggregated emission total.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="aa454e4d" data-kind="attribute" data-el-id="778" data-attr-name="emission_statement_id" data-attr-type="String" data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_component_category_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionComponentCategory that classifies which greenhouse gas or aggregate category this component represents, such as CO2 fossil, CH4, or CO2e total.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="96e1140c" data-kind="attribute" data-el-id="778" data-attr-name="emission_component_category_id" data-attr-type="String" data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The emission quantity for this specific gas component, expressed in the unit referenced by quantity_unit_of_measure_id, before global-warming-potential conversion.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="82d935cd" data-kind="attribute" data-el-id="778" data-attr-name="quantity" data-attr-type="String" data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>quantity_unit_of_measure_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>Foreign key to the UnitOfMeasure record that expresses the unit for this component quantity, such as t (metric tonnes) for gas mass or tCO2e for GWP-weighted totals.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="823d91df" data-kind="attribute" data-el-id="778" data-attr-name="quantity_unit_of_measure_id" data-attr-type="String" data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="778" data-tag-name="description" data-tag-value="EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately." data-file-path="Emissions/EmissionComponent.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentPerStandard](EmissionComponentPerStandard.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.html) |
| Association |  | [EmissionStatement](EmissionStatement.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionStatement](EmissionStatement.html) |
| Association |  | [EmissionComponentPerStandard](EmissionComponentPerStandard.html) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.html) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="778"></div>

---

*Generated: 2026-08-04 12:35:51*