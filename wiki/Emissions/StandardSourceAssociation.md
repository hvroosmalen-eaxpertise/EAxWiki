---
ea_id: 794
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 6a1b9dc0
---

# <span class="sl" data-layer="uml">master-data</span> StandardSourceAssociation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="794" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="794" data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard. A standard such as the GHG Protocol may endorse specific factor databases (IPCC, national inventory agencies, DESNZ) while another framework mandates different sources. Capturing these endorsements as data avoids hard-coding source eligibility rules in application logic and supports audit queries confirming that all factors used were sourced from a framework-approved database.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this StandardSourceAssociation record, used in audit queries to verify that emission factors were drawn from a source approved under the applicable standard.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="1ddbc0de" data-kind="attribute" data-el-id="794" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>standard_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the Standard that endorses or mandates the use of the referenced emission factor source.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="a3fceff9" data-kind="attribute" data-el-id="794" data-attr-name="standard_id" data-attr-type="String" data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_factor_source_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionFactorSource that is endorsed or mandated by the referenced standard, establishing the permissible source set for calculations under that framework.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="e4f4342a" data-kind="attribute" data-el-id="794" data-attr-name="emission_factor_source_id" data-attr-type="String" data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>notes</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Free-text notes describing any conditions or restrictions on the use of this source under the referenced standard, such as mandatory for UK Scope 2 market-based method.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="dfa65b13" data-kind="attribute" data-el-id="794" data-attr-name="notes" data-attr-type="String" data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="794" data-tag-name="description" data-tag-value="StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard." data-file-path="Emissions/StandardSourceAssociation.md" data-api-port="8001" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |
| Association |  | [Standard](../Organisation/Standard.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;EmissionFactorSource&quot;,&quot;fullName&quot;:&quot;EmissionFactorSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactorSource.html&quot;},{&quot;id&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Standard&quot;,&quot;fullName&quot;:&quot;Standard&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Standard.html&quot;},{&quot;id&quot;:&quot;e794&quot;,&quot;label&quot;:&quot;StandardSourceAssociati…&quot;,&quot;fullName&quot;:&quot;StandardSourceAssociation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;ProductCarbonFootprintF…&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprintFactorSource&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprintFactorSource.html&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e789&quot;,&quot;label&quot;:&quot;EmissionComponentPerSta…&quot;,&quot;fullName&quot;:&quot;EmissionComponentPerStandard&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentPerStandard.html&quot;},{&quot;id&quot;:&quot;e788&quot;,&quot;label&quot;:&quot;EmissionStatementPerSta…&quot;,&quot;fullName&quot;:&quot;EmissionStatementPerStandard&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatementPerStandard.html&quot;},{&quot;id&quot;:&quot;e787&quot;,&quot;label&quot;:&quot;EmissionCategoryStandar…&quot;,&quot;fullName&quot;:&quot;EmissionCategoryStandardAssociation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCategoryStandardAssociation.html&quot;},{&quot;id&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;EmissionReport&quot;,&quot;fullName&quot;:&quot;EmissionReport&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionReport.html&quot;},{&quot;id&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;OrganizationalBoundary&quot;,&quot;fullName&quot;:&quot;OrganizationalBoundary&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/OrganizationalBoundary.html&quot;},{&quot;id&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;EmissionCalculationModel&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModel&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModel.html&quot;},{&quot;id&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;EmissionInventory&quot;,&quot;fullName&quot;:&quot;EmissionInventory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionInventory.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c806&quot;,&quot;source&quot;:&quot;e781&quot;,&quot;target&quot;:&quot;e794&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c835&quot;,&quot;source&quot;:&quot;e781&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c849&quot;,&quot;source&quot;:&quot;e794&quot;,&quot;target&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c857&quot;,&quot;source&quot;:&quot;e780&quot;,&quot;target&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c843&quot;,&quot;source&quot;:&quot;e789&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c845&quot;,&quot;source&quot;:&quot;e788&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c847&quot;,&quot;source&quot;:&quot;e787&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c850&quot;,&quot;source&quot;:&quot;e794&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c894&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e782&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c897&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e740&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c916&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e777&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c918&quot;,&quot;source&quot;:&quot;e734&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c808&quot;,&quot;source&quot;:&quot;e782&quot;,&quot;target&quot;:&quot;e771&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-01 14:13:17*