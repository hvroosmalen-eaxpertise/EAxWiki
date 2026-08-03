---
ea_id: 777
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: c7310f99
---

# <span class="sl" data-layer="uml">master-data</span> EmissionCalculationModel

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="777" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="777" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity. A model links a set of EmissionCalculationFormulas and specifies the method type (spend-based, activity-based, supplier-specific, and so on) and the applicable standard. Models may be versioned and associated with specific jurisdictions or industry sectors, allowing a calculation engine to select the most appropriate model for a given emission activity and reporting context.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this EmissionCalculationModel record, referenced by EmissionStatement and EmissionActivityFactor records to trace which method produced a given result.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="b93328fe" data-kind="attribute" data-el-id="777" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_calculation_method_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the EmissionCalculationMethodType that classifies the calculation approach, such as activity-based, spend-based, or supplier-specific, supporting methodology disclosure.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="bbf6b5ff" data-kind="attribute" data-el-id="777" data-attr-name="emission_calculation_method_type_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>standard_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the Standard whose guidance governs this calculation model, ensuring that the method is clearly traceable to its normative source.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="c608beeb" data-kind="attribute" data-el-id="777" data-attr-name="standard_id" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>A descriptive label for the calculation model, such as DEFRA 2024 Electricity Consumption UK Grid or GHG Protocol Mobile Combustion Diesel, used for model selection and labelling in reports.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="6f5906a0" data-kind="attribute" data-el-id="777" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A narrative description of the model scope, assumptions, applicable activity types, and any known limitations, supporting informed methodology selection by practitioners.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="c28115b7" data-kind="attribute" data-el-id="777" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>version</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The version identifier of this model definition, allowing calculations to be re-executed with a historically consistent method and supporting year-over-year comparability.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="edce9151" data-kind="attribute" data-el-id="777" data-attr-name="version" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_from</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The date from which this model version is applicable, used by calculation engines to select the correct model version for a given reporting period.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="aa71a0d8" data-kind="attribute" data-el-id="777" data-attr-name="valid_from" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>valid_to</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The date after which this model version is superseded, ensuring that outdated methods are not inadvertently applied to new reporting periods.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="97c2d3ba" data-kind="attribute" data-el-id="777" data-attr-name="valid_to" data-attr-type="String" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="777" data-tag-name="description" data-tag-value="EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity." data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001" data-api-token="f71e4831faa78932c4078d4ddf7941b1141fc3d544ee504f" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.html) |
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.html) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.html) |
| Association |  | [EmissionCalculationMethodType](EmissionCalculationMethodType.html) |
| Association |  | [EmissionCalculationFormula](EmissionCalculationFormula.html) |
| Association |  | [EmissionStatement](EmissionStatement.html) |
| Association |  | [Standard](../Organisation/Standard.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](../Organisation/Standard.html) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.html) |
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.html) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.html) |
| Association |  | [EmissionCalculationMethodType](EmissionCalculationMethodType.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="777"></div>

---

*Generated: 2026-08-03 10:55:47*