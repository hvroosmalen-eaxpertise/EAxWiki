---
ea_id: 777
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: c7310f99
---

# <span class="sl" data-layer="uml">master-data</span> EmissionCalculationModel

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  **Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="777" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001"></div>

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="777" data-file-path="Emissions/EmissionCalculationModel.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity. A model links a set of EmissionCalculationFormulas and specifies the method type (spend-based, activity-based, supplier-specific, and so on) and the applicable standard. Models may be versioned and associated with specific jurisdictions or industry sectors, allowing a calculation engine to select the most appropriate model for a given emission activity and reporting context.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionCalculationModel record, referenced by EmissionStatement and EmissionActivityFactor records to trace which method produced a given result. |
| emission_calculation_method_type_id | String |  | Foreign key to the EmissionCalculationMethodType that classifies the calculation approach, such as activity-based, spend-based, or supplier-specific, supporting methodology disclosure. |
| standard_id | String |  | Foreign key to the Standard whose guidance governs this calculation model, ensuring that the method is clearly traceable to its normative source. |
| name | String |  | A descriptive label for the calculation model, such as DEFRA 2024 Electricity Consumption UK Grid or GHG Protocol Mobile Combustion Diesel, used for model selection and labelling in reports. |
| description | String |  | A narrative description of the model scope, assumptions, applicable activity types, and any known limitations, supporting informed methodology selection by practitioners. |
| version | String |  | The version identifier of this model definition, allowing calculations to be re-executed with a historically consistent method and supporting year-over-year comparability. |
| valid_from | String |  | The date from which this model version is applicable, used by calculation engines to select the correct model version for a given reporting period. |
| valid_to | String |  | The date after which this model version is superseded, ensuring that outdated methods are not inadvertently applied to new reporting periods. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.md) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.md) |
| Association |  | [EmissionCalculationMethodType](EmissionCalculationMethodType.md) |
| Association |  | [EmissionCalculationFormula](EmissionCalculationFormula.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |
| Association |  | [Standard](../Organisation/Standard.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Standard](../Organisation/Standard.md) |
| Association |  | [EmissionCalculationModelParameterArgument](EmissionCalculationModelParameterArgument.md) |
| Association |  | [EmissionCalculationModelFactorArgument](EmissionCalculationModelFactorArgument.md) |
| Association |  | [EmissionActivityFactor](EmissionActivityFactor.md) |
| Association |  | [EmissionCalculationMethodType](EmissionCalculationMethodType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e805","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelParameterArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelParameterArgument.html"},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e791","label":"EmissionCalculationMeth…","fullName":"EmissionCalculationMethodType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationMethodType.html"},{"id":"e790","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormula","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormula.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCategoryStandardAssociation.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"}],"edges":[{"id":"c815","source":"e805","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c816","source":"e805","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c817","source":"e804","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c818","source":"e804","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c851","source":"e793","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c853","source":"e793","target":"e786","label":"Association","sourceLayer":"uml"},{"id":"c858","source":"e791","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c821","source":"e803","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c859","source":"e777","target":"e790","label":"Association","sourceLayer":"uml"},{"id":"c823","source":"e802","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c825","source":"e801","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c826","source":"e800","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c827","source":"e799","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c840","source":"e776","target":"e764","label":"Association","sourceLayer":"uml"},{"id":"c846","source":"e788","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c868","source":"e777","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c869","source":"e772","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c870","source":"e773","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c871","source":"e771","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c843","source":"e789","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c845","source":"e788","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c847","source":"e787","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c850","source":"e794","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c894","source":"e734","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c897","source":"e734","target":"e740","label":"Association","sourceLayer":"uml"},{"id":"c916","source":"e734","target":"e777","label":"Association","sourceLayer":"uml"},{"id":"c918","source":"e734","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c819","source":"e803","target":"e775","label":"Association","sourceLayer":"uml"},{"id":"c832","source":"e775","target":"e779","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c862","source":"e786","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c822","source":"e802","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c844","source":"e789","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 10:25:44*