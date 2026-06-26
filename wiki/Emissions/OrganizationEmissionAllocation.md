# <span class="sl" data-layer="uml">WOR</span> OrganizationEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

OrganizationEmissionAllocation is a work-product-component that records the share of an EmissionStatement total quantity allocated to a specific Organisation when equity-share or financial-control consolidation requires that group emissions be apportioned across multiple legal entities or subsidiaries. The allocation percentage and method are stored alongside the allocated quantity to maintain a complete audit trail from group total to entity share.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this OrganizationEmissionAllocation record, used to aggregate all shares of a given statement and verify that allocations sum to 100% for fully consolidated inventories. |
| emission_statement_id | String |  | Foreign key to the EmissionStatement whose total is being apportioned across organisations by this and other allocation records. |
| organisation_id | String |  | Foreign key to the Organisation receiving the allocated share of the emission quantity, identifying which entity inventory this apportioned amount feeds into. |
| allocation_percentage | String |  | The percentage of the parent statement emission quantity allocated to this organisation, derived from the equity share, financial control ratio, or other allocation rule applied. |
| allocated_quantity | String |  | The absolute emission quantity allocated to this organisation, calculated as the parent statement quantity multiplied by the allocation percentage, for direct use in the organisation inventory totals. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the allocated_quantity is expressed, ensuring consistent unit handling when allocation records are aggregated. |
| allocation_method | String |  | A description of the method used to determine the allocation percentage, such as equity share per shareholder register or financial control 100% consolidation, supporting audit traceability. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationEmissionAllocation is a work-product-component that records the share of an EmissionStatement total quantity allocated to a specific Organisation when equity-share or financial-control consolidation requires that group emissions be apportioned |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Organization](../Organisation/Organization.md) |
| Association |  | [EmissionStatement](EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":true,"hasUrl":false,"url":""},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionRecordingMethodType.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityEmissionAllocation.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"}],"edges":[{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c826","source":"e800","target":"e776","label":"Association"},{"id":"c827","source":"e799","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c869","source":"e772","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c873","source":"e753","target":"e764","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:25:36*