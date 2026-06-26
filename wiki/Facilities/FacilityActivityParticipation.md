# <span class="sl" data-layer="uml">master-data</span> FacilityActivityParticipation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity. This intersection entity supports cases where a single emission activity spans multiple facilities, or where multiple emission activities are associated with the same facility, enabling accurate physical attribution of GHG-generating processes to the sites at which they occur.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the FacilityActivityParticipation record. It is the primary key for this participation link and must remain stable for reporting history and audit purposes. |
| emission_activity_id | String |  | A foreign key identifying the EmissionActivity in which the referenced facility participates, linking the physical facility to the specific emission-generating or emission-removing process. |
| facility_id | String |  | A foreign key identifying the Facility that participates in the referenced emission activity, linking the physical site to the activity for geographic attribution and site-level reporting. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | FacilityActivityParticipation records the fact that a specific Facility participates in a specific EmissionActivity. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.md) |
| Association |  | [Facility](Facility.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Facility](Facility.md) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivity.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Facility.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":Facilities,"isFocal":true,"hasUrl":false,"url":""},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/ActivityEmissionAllocation.html"},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":Products,"isFocal":false,"hasUrl":true,"url":"../Products/EmissionActivityFlow.html"},{"id":"e785","label":"EmissionSink","fullName":"EmissionSink","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionSink.html"},{"id":"e784","label":"EmissionSource","fullName":"EmissionSource","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionSource.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityCategory.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityType.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"},{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityEmissionAllocation.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"}],"edges":[{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c836","source":"e773","target":"e814","label":"Association"},{"id":"c838","source":"e785","target":"e773","label":"Association"},{"id":"c839","source":"e784","target":"e773","label":"Association"},{"id":"c842","source":"e773","target":"e760","label":"Association"},{"id":"c861","source":"e774","target":"e773","label":"Association"},{"id":"c862","source":"e786","target":"e773","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c873","source":"e753","target":"e764","label":"Association"},{"id":"c875","source":"e753","target":"e762","label":"Association"},{"id":"c877","source":"e753","target":"e761","label":"Association"},{"id":"c879","source":"e753","target":"e760","label":"Association"},{"id":"c880","source":"e753","target":"e759","label":"Association"},{"id":"c889","source":"e753","target":"e758","label":"Association"},{"id":"c891","source":"e753","target":"e757","label":"Association"},{"id":"c892","source":"e753","target":"e754","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c823","source":"e802","target":"e776","label":"Association"},{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c890","source":"e757","target":"e757","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:44:31*