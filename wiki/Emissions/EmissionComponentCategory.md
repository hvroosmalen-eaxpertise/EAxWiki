---
ea_id: 792
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 465180ec
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionComponentCategory

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="792" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="792" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record. The seven Kyoto Protocol gases (CO2, CH4, N2O, HFCs, PFCs, SF6, and NF3) are the primary values, supplemented by aggregate categories such as CO2e total (AR5 GWPs) or biogenic CO2. Categorising components at this level enables gas-specific aggregation, GWP factor application, and the gas-level disclosures required by ISO 14064-1 and ESRS E1.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionComponentCategory record, referenced by EmissionComponent, EmissionFactor, and EmissionComponentCategoryGroup records to classify gas-level data.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="6cc76341" data-kind="attribute" data-el-id="792" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_component_category_group_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionComponentCategoryGroup that this category belongs to, enabling roll-up from individual gas categories to higher-level groupings such as Fluorinated gases or Biogenic.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="4f9c2581" data-kind="attribute" data-el-id="792" data-attr-name="emission_component_category_group_id" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The standard name for the GHG category, such as Carbon Dioxide Fossil, Methane, or Nitrous Oxide, used in component-level disclosures and factor lookup tables.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="24634728" data-kind="attribute" data-el-id="792" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_ar4</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The global warming potential of this gas using IPCC Fourth Assessment Report (AR4) characterisation factors, retained for legacy reporting requirements that mandate AR4 values.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="4e3cddde" data-kind="attribute" data-el-id="792" data-attr-name="gwp_ar4" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_ar5</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The global warming potential using IPCC Fifth Assessment Report (AR5) characterisation factors, applicable to reporting periods under GHG Protocol Corporate Standard and many national inventories.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="fb07866c" data-kind="attribute" data-el-id="792" data-attr-name="gwp_ar5" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_ar6</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The global warming potential using IPCC Sixth Assessment Report (AR6) characterisation factors, required for reporting under the latest ESRS E1 and PACT v2+ specifications.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="b4fbfd55" data-kind="attribute" data-el-id="792" data-attr-name="gwp_ar6" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="792" data-tag-name="description" data-tag-value="EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record." data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionComponent](EmissionComponent.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e795&quot;,&quot;label&quot;:&quot;EmissionComponentCatego…&quot;,&quot;fullName&quot;:&quot;EmissionComponentCategoryGroup&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentCategoryGroup.html&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;EmissionComponent&quot;,&quot;fullName&quot;:&quot;EmissionComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponent.html&quot;},{&quot;id&quot;:&quot;e792&quot;,&quot;label&quot;:&quot;EmissionComponentCatego…&quot;,&quot;fullName&quot;:&quot;EmissionComponentCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e804&quot;,&quot;label&quot;:&quot;EmissionCalculationMode…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModelFactorArgument&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModelFactorArgument.html&quot;},{&quot;id&quot;:&quot;e803&quot;,&quot;label&quot;:&quot;EmissionCalculationForm…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationFormulaComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationFormulaComponent.html&quot;},{&quot;id&quot;:&quot;e793&quot;,&quot;label&quot;:&quot;EmissionActivityFactor&quot;,&quot;fullName&quot;:&quot;EmissionActivityFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityFactor.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;EmissionFactorSource&quot;,&quot;fullName&quot;:&quot;EmissionFactorSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactorSource.html&quot;},{&quot;id&quot;:&quot;e789&quot;,&quot;label&quot;:&quot;EmissionComponentPerSta…&quot;,&quot;fullName&quot;:&quot;EmissionComponentPerStandard&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentPerStandard.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c809&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e795&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c831&quot;,&quot;source&quot;:&quot;e795&quot;,&quot;target&quot;:&quot;e792&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c817&quot;,&quot;source&quot;:&quot;e804&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c820&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c852&quot;,&quot;source&quot;:&quot;e793&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c854&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c855&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c856&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c857&quot;,&quot;source&quot;:&quot;e780&quot;,&quot;target&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c844&quot;,&quot;source&quot;:&quot;e789&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c864&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c866&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c867&quot;,&quot;source&quot;:&quot;e776&quot;,&quot;target&quot;:&quot;e778&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c865&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*