# <span class="sl" data-layer="uml">WOR</span> FacilityEmissionAllocation

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement. This entity supports scenarios where an emission source or activity is shared between multiple facilities, enabling the portion of the emission attributable to each site to be recorded separately for site-level inventory and regulatory reporting purposes. This is of particular relevance to US EPA Subpart W reporting for Petroleum and Natural Gas Systems.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityEmissionAllocation record. It must remain stable for audit and regulatory reporting purposes. |
| percentage | String |  | The fraction of the total emission quantity in the linked EmissionStatement that is allocated to the linked Facility, expressed as a value between 0 and 100. The sum of percentages across all allocations for a given EmissionStatement should equal 100. |
| emission_statement_id | String |  | A foreign key identifying the EmissionStatement whose emission quantity is being allocated to the referenced facility. |
| facility_id | String |  | A foreign key identifying the Facility to which the specified percentage of the emission quantity is allocated. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityEmissionAllocation records the allocation of a specific emission quantity to a specific Facility, expressed as a percentage of the total emission quantity in the linked EmissionStatement. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionStatement](../Emissions/EmissionStatement.md) |
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [EmissionStatement](../Emissions/EmissionStatement.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":true,"hasUrl":false,"url":""},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/ActivityEmissionAllocation.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e800","label":"RecordingUncertaintyAss…","fullName":"RecordingUncertaintyAssessment","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/RecordingUncertaintyAssessment.html"},{"id":"e799","label":"EmissionRecordingMethod…","fullName":"EmissionRecordingMethodType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionRecordingMethodType.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatementPerStandard.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionComponent.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionCalculationModel.html"},{"id":"e772","label":"EmissionScopeType","fullName":"EmissionScopeType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionScopeType.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivity.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c826","source":"e800","target":"e776","label":"Association"},{"id":"c827","source":"e799","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c846","source":"e788","target":"e776","label":"Association"},{"id":"c865","source":"e779","target":"e776","label":"Association"},{"id":"c867","source":"e776","target":"e778","label":"Association"},{"id":"c868","source":"e777","target":"e776","label":"Association"},{"id":"c869","source":"e772","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c873","source":"e753","target":"e764","label":"Association"},{"id":"c875","source":"e753","target":"e762","label":"Association"},{"id":"c877","source":"e753","target":"e761","label":"Association"},{"id":"c879","source":"e753","target":"e760","label":"Association"},{"id":"c880","source":"e753","target":"e759","label":"Association"},{"id":"c889","source":"e753","target":"e758","label":"Association"},{"id":"c891","source":"e753","target":"e757","label":"Association"},{"id":"c892","source":"e753","target":"e754","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c864","source":"e779","target":"e778","label":"Association"},{"id":"c842","source":"e773","target":"e760","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c890","source":"e757","target":"e757","label":"Association"}]};</script>

---

*Generated: 2026-06-26 09:44:49*