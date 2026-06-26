# <span class="sl" data-layer="uml">REF</span> EmissionComponent

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately. This granularity supports characterisation factor application, gas-specific disclosure requirements, and cross-protocol comparability beyond CO2-equivalent totals alone.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionComponent record, used when linking per-standard breakdowns via EmissionComponentPerStandard or when applying gas-specific characterisation factors. |
| emission_statement_id | String |  | Foreign key to the parent EmissionStatement that this component disaggregates, linking the gas-level detail to its aggregated emission total. |
| emission_component_category_id | String |  | Foreign key to the EmissionComponentCategory that classifies which greenhouse gas or aggregate category this component represents, such as CO2 fossil, CH4, or CO2e total. |
| quantity | String |  | The emission quantity for this specific gas component, expressed in the unit referenced by quantity_unit_of_measure_id, before global-warming-potential conversion. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure record that expresses the unit for this component quantity, such as t (metric tonnes) for gas mass or tCO2e for GWP-weighted totals. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentPerStandard](EmissionComponentPerStandard.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionStatement](EmissionStatement.md) |
| Association |  | [EmissionComponentPerStandard](EmissionComponentPerStandard.md) |
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |
| Association |  | [EmissionComponentCategory](EmissionComponentCategory.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":Emissions,"isFocal":true,"hasUrl":false,"url":""},{"id":"e734","label":"Standard","fullName":"Standard","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e783","label":"EmissionReportPeriod","fullName":"EmissionReportPeriod","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionReportPeriod.html"},{"id":"e798","label":"UnitOfMeasureSourceRefe…","fullName":"UnitOfMeasureSourceReference","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasureSourceReference.html"},{"id":"e797","label":"PhysicalQuantityType","fullName":"PhysicalQuantityType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"PhysicalQuantityType.html"},{"id":"e796","label":"SystemOfUnits","fullName":"SystemOfUnits","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"SystemOfUnits.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionParameterType.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/EmissionActivityParameterValue.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e795","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategoryGroup","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponentCategoryGroup.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"}],"edges":[{"id":"c843","source":"e789","target":"e734","label":"Association"},{"id":"c844","source":"e789","target":"e778","label":"Association"},{"id":"c807","source":"e779","target":"e783","label":"Association"},{"id":"c828","source":"e798","target":"e779","label":"Association"},{"id":"c829","source":"e797","target":"e779","label":"Association"},{"id":"c830","source":"e796","target":"e779","label":"Association"},{"id":"c832","source":"e775","target":"e779","label":"Association"},{"id":"c833","source":"e779","target":"e812","label":"Association"},{"id":"c834","source":"e779","target":"e763","label":"Association"},{"id":"c854","source":"e779","target":"e780","label":"Association"},{"id":"c855","source":"e779","target":"e780","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c809","source":"e792","target":"e795","label":"Association"},{"id":"c831","source":"e795","target":"e792","label":"Association"},{"id":"c856","source":"e792","target":"e780","label":"Association"},{"id":"c866","source":"e792","target":"e778","label":"Association"},{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c826","source":"e800","target":"e776","label":"Association"},{"id":"c827","source":"e799","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c869","source":"e772","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c845","source":"e788","target":"e734","label":"Association"},{"id":"c916","source":"e734","target":"e777","label":"Association"},{"id":"c918","source":"e734","target":"e771","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*