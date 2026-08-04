---
ea_id: 792
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 465180ec
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionComponentCategory

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="792" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="792" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" data-ai-configured="false">
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
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionComponentCategory record, referenced by EmissionComponent, EmissionFactor, and EmissionComponentCategoryGroup records to classify gas-level data.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="6cc76341" data-kind="attribute" data-el-id="792" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_component_category_group_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionComponentCategoryGroup that this category belongs to, enabling roll-up from individual gas categories to higher-level groupings such as Fluorinated gases or Biogenic.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="4f9c2581" data-kind="attribute" data-el-id="792" data-attr-name="emission_component_category_group_id" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>The standard name for the GHG category, such as Carbon Dioxide Fossil, Methane, or Nitrous Oxide, used in component-level disclosures and factor lookup tables.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="24634728" data-kind="attribute" data-el-id="792" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_ar4</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>The global warming potential of this gas using IPCC Fourth Assessment Report (AR4) characterisation factors, retained for legacy reporting requirements that mandate AR4 values.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="4e3cddde" data-kind="attribute" data-el-id="792" data-attr-name="gwp_ar4" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_ar5</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The global warming potential using IPCC Fifth Assessment Report (AR5) characterisation factors, applicable to reporting periods under GHG Protocol Corporate Standard and many national inventories.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="fb07866c" data-kind="attribute" data-el-id="792" data-attr-name="gwp_ar5" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>gwp_ar6</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The global warming potential using IPCC Sixth Assessment Report (AR6) characterisation factors, required for reporting under the latest ESRS E1 and PACT v2+ specifications.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="b4fbfd55" data-kind="attribute" data-el-id="792" data-attr-name="gwp_ar6" data-attr-type="String" data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="792" data-tag-name="description" data-tag-value="EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record." data-file-path="Emissions/EmissionComponentCategory.md" data-api-port="8001" data-api-token="e9a260cc7815d7609eabcda6e3d715efdce31af9e5bbe3c0" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.html) |
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.html) |
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [EmissionComponent](EmissionComponent.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="792"></div>

---

*Generated: 2026-08-04 11:36:53*