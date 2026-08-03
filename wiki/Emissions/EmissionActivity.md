---
ea_id: 773
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: ec99f5f6
---

# <span class="sl" data-layer="uml">master-data</span> EmissionActivity

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="773" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="773" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" data-ai-configured="true">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionActivity is a master-data entity that represents a discrete operational process or event that generates, absorbs, or transfers greenhouse gas emissions. Each activity is linked to an EmissionActivityType and an EmissionActivityCategory, enabling aggregation and scope attribution. The entity supports a self-referential hierarchy through parent_id, allowing complex multi-level activity structures to be modelled without loss of granularity.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique system identifier for this EmissionActivity record, referenced by EmissionStatement, EmissionActivityFlow, and parameter records to associate measurements with a specific activity.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="6f5e85c1" data-kind="attribute" data-el-id="773" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>parent_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>A self-referential foreign key that points to the parent EmissionActivity in a hierarchical decomposition. A null value indicates a root-level activity.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="36db8df1" data-kind="attribute" data-el-id="773" data-attr-name="parent_id" data-attr-type="String" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the EmissionActivityType record that classifies the nature of this activity, for example Stationary Combustion or Mobile Combustion.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="c2eea649" data-kind="attribute" data-el-id="773" data-attr-name="emission_activity_type_id" data-attr-type="String" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>emission_activity_category_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the EmissionActivityCategory that places this activity within the GHG Protocol or ISO 14064 category structure, such as Category 1 Purchased goods and services.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="7adff900" data-kind="attribute" data-el-id="773" data-attr-name="emission_activity_category_id" data-attr-type="String" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>A descriptive label for the activity instance, uniquely identifying it within its parent context, such as "Boiler 3 Site A natural gas combustion".</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="0a9c062d" data-kind="attribute" data-el-id="773" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>description</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>Free-text narrative providing additional context on how the activity is performed, what sources or sinks it involves, and any special treatment applied during calculation.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="f59ead5c" data-kind="attribute" data-el-id="773" data-attr-name="description" data-attr-type="String" data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>EmissionActivity is a master-data entity that represents a discrete operational process or event that generates, absorbs, or transfers greenhouse gas emissions.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="773" data-tag-name="description" data-tag-value="EmissionActivity is a master-data entity that represents a discrete operational process or event that generates, absorbs, or transfers greenhouse gas emissions." data-file-path="Emissions/EmissionActivity.md" data-api-port="8001" data-api-token="d54ac7f4ba1b9561901225e0195c664d0fa006b906b25c92" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.html) |
| Association |  | [EmissionActivityFlow](../Products/EmissionActivityFlow.html) |
| Association |  | [EmissionSink](EmissionSink.html) |
| Association |  | [EmissionSource](EmissionSource.html) |
| Association |  | [FacilityActivityParticipation](../Facilities/FacilityActivityParticipation.html) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.html) |
| Association |  | [EmissionActivityType](EmissionActivityType.html) |
| Association |  | [EmissionActivity](EmissionActivity.html) |
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
| Association |  | [ActivityEmissionAllocation](ActivityEmissionAllocation.html) |
| Association |  | [EmissionSink](EmissionSink.html) |
| Association |  | [EmissionSource](EmissionSource.html) |
| Association |  | [EmissionActivityCategory](EmissionActivityCategory.html) |
| Association |  | [EmissionActivityType](EmissionActivityType.html) |
| Association |  | [EmissionActivity](EmissionActivity.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="773"></div>

---

*Generated: 2026-08-03 08:46:17*