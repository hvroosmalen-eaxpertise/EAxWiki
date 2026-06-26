# <span class="sl" data-layer="uml">reference-data</span> EmissionRecordingMethodType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, derived from a published default, or extrapolated from related data. This classification is required by ISO 14064-1 and GHG Protocol for data quality assessments and for identifying which statements require improved monitoring as part of an improvement plan.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionRecordingMethodType record, referenced by EmissionStatement records to describe how each emission quantity was obtained. |
| name | String |  | The label for the recording method, drawn from values such as Measured, Calculated, Estimated, Default factor, or Extrapolated, used in data quality tables and assurance documentation. |
| description | String |  | A description of the recording method approach, its typical applicability, and the data quality tier it represents, supporting selection guidance for practitioners completing inventory data entry. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionRecordingMethodType is a reference entity that classifies how an emission quantity was obtained: whether it was directly measured using instrumentation, calculated from activity data and emission factors, estimated using engineering judgement, der |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":"Emissions","isFocal":true,"hasUrl":false,"url":""},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"}],"edges":[{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c826","source":"e800","target":"e776","label":"Association"},{"id":"c827","source":"e799","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c869","source":"e772","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:40*