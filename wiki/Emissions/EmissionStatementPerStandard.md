---
ea_id: 788
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 9f52a592
---

# <span class="sl" data-layer="uml">work-product-component</span> EmissionStatementPerStandard

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="788" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionStatementPerStandard.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="788" data-file-path="Emissions/EmissionStatementPerStandard.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionStatementPerStandard is an intersection entity that records the emission quantity attributed to a specific EmissionStatement as it must be reported under a particular Standard or reporting framework. Different frameworks may require different consolidation rules, boundary definitions, or GWP factors, so the same underlying activity data may yield different disclosed quantities depending on the framework applied. This entity preserves each framework-specific result without duplicating the underlying statement.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this per-standard emission result record. |
| emission_statement_id | String |  | Foreign key to the parent EmissionStatement from which this per-standard quantity is derived. |
| standard_id | String |  | Foreign key to the Standard or framework under which this specific quantity is reported, such as GHG Protocol, ISO 14064-1, or ESRS E1. |
| quantity | String |  | The emission quantity as calculated or adjusted for disclosure under the referenced standard, which may differ from the parent statement quantity due to framework-specific boundary or GWP rules. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure expressing the unit in which the per-standard quantity is reported, typically tCO2e but potentially differing by framework requirement. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionStatementPerStandard is an intersection entity that records the emission quantity attributed to a specific EmissionStatement as it must be reported under a particular Standard or reporting framework. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"}],"edges":[{"id":"c843","source":"e789","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c845","source":"e788","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c847","source":"e787","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c850","source":"e794","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c897","source":"e734","target":"e740","label":"Association","sourceLayer":"uml"},{"id":"c916","source":"e734","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c823","source":"e802","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c825","source":"e801","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c826","source":"e800","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c827","source":"e799","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c840","source":"e776","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c846","source":"e788","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c869","source":"e772","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c870","source":"e773","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c844","source":"e789","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c822","source":"e802","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*