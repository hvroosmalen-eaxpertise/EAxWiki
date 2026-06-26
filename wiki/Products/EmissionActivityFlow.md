# <span class="sl" data-layer="uml">master-data</span> EmissionActivityFlow

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process. It enables the model to disaggregate a product carbon footprint into its contributing emission activities, providing the activity-level transparency required for hotspot analysis, supplier engagement, and data quality improvement roadmaps.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionActivityFlow record, used to navigate between the product life-cycle domain and the emission activity domain when performing stage-level attribution analysis. |
| product_life_cycle_id | String |  | Foreign key to the ProductLifeCycle that this flow belongs to, placing the emission activity within the context of a defined system boundary. |
| emission_activity_id | String |  | Foreign key to the EmissionActivity whose GHG contribution to the product life cycle is represented by this flow record. |
| flow_quantity | String |  | The quantity of the activity that occurs within this life-cycle stage per declared unit of the product, expressed in the activity parameter unit, used as the input to the corresponding emission calculation. |
| flow_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the flow_quantity is expressed, such as kWh of electricity consumed or km of transport per declared unit, enabling calculation engines to apply the correct emission factor. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionActivityFlow is an intersection entity that links an EmissionActivity to a specific ProductLifeCycle, representing the flow of emissions associated with a particular life-cycle stage process. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycle](ProductLifeCycle.md) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductLifeCycle](ProductLifeCycle.md) |
| Association |  | [EmissionActivity](../Emissions/EmissionActivity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e813","label":"ProductLifeCycle","fullName":"ProductLifeCycle","packageName":"Products","isFocal":false,"hasUrl":true,"url":"ProductLifeCycle.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivity.html"},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","isFocal":true,"hasUrl":false,"url":""},{"id":"e810","label":"Product","fullName":"Product","packageName":"Products","isFocal":false,"hasUrl":true,"url":"Product.html"},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/ActivityEmissionAllocation.html"},{"id":"e785","label":"EmissionSink","fullName":"EmissionSink","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionSink.html"},{"id":"e784","label":"EmissionSource","fullName":"EmissionSource","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionSource.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityActivityParticipation.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityCategory.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivityType.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"}],"edges":[{"id":"c800","source":"e813","target":"e814","label":"Association"},{"id":"c801","source":"e810","target":"e813","label":"Association"},{"id":"c822","source":"e802","target":"e773","label":"Association"},{"id":"c836","source":"e773","target":"e814","label":"Association"},{"id":"c838","source":"e785","target":"e773","label":"Association"},{"id":"c839","source":"e784","target":"e773","label":"Association"},{"id":"c842","source":"e773","target":"e760","label":"Association"},{"id":"c861","source":"e774","target":"e773","label":"Association"},{"id":"c862","source":"e786","target":"e773","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c823","source":"e802","target":"e776","label":"Association"}]};</script>

---

*Generated: 2026-06-26 15:09:23*