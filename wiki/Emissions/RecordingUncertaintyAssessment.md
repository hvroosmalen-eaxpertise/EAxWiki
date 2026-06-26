# <span class="sl" data-layer="uml">WOR</span> RecordingUncertaintyAssessment

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

RecordingUncertaintyAssessment is a work-product-component that captures the quantitative or qualitative uncertainty associated with a specific EmissionStatement, as required by ISO 14064-1 for first-party assurance. It records the uncertainty range, the assessment methodology, and the primary uncertainty drivers identified for that emission quantity. Systematic uncertainty documentation supports both the GHG Protocol accuracy principle and the assurance engagement process for third-party verification.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this RecordingUncertaintyAssessment record, used to link it to the parent EmissionStatement and to aggregate uncertainty across scope totals. |
| emission_statement_id | String |  | Foreign key to the EmissionStatement whose uncertainty this record assesses, establishing the one-to-one correspondence between an emission result and its uncertainty characterisation. |
| uncertainty_range_lower | String |  | The lower bound of the uncertainty range expressed as a percentage deviation from the central estimate, such as -15, indicating that the true value may be 15% lower than stated. |
| uncertainty_range_upper | String |  | The upper bound of the uncertainty range expressed as a percentage deviation from the central estimate, such as +20, indicating that the true value may be 20% higher than stated. |
| assessment_methodology | String |  | A description of the approach used to derive the uncertainty range, such as Monte Carlo simulation, IPCC Tier 1 uncertainty table, or expert elicitation, supporting methodological transparency. |
| primary_uncertainty_drivers | String |  | A free-text summary of the main sources of uncertainty identified for this emission statement, such as activity data estimation error or emission factor geographic applicability, guiding improvement prioritisation. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | RecordingUncertaintyAssessment is a work-product-component that captures the quantitative or qualitative uncertainty associated with a specific EmissionStatement, as required by ISO 14064-1 for first-party assurance. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":Emissions,"isFocal":true,"hasUrl":false,"url":""},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"}],"edges":[{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c826","source":"e800","target":"e776","label":"Association"},{"id":"c827","source":"e799","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c869","source":"e772","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"}]};</script>

---

*Generated: 2026-06-26 09:44:49*