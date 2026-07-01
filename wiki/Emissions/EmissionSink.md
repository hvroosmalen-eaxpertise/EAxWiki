---
ea_id: 785
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: ac489c84
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionSink

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="785" data-file-path="Emissions/EmissionSink.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>EmissionSink is a reference entity that classifies a physical destination into which greenhouse gases are absorbed or sequestered, such as forest biomass, soil carbon, or geological storage. Sinks are used to record carbon removal activities alongside emission activities in an inventory, enabling net-emission calculations and supporting reporting of land-use, land-use-change, and forestry (LULUCF) activities as required by ISO 14064-1 and voluntary carbon standards.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionSink record, used by EmissionActivity records that represent removal activities to classify the type of sink involved. |
| name | String |  | The human-readable name of the emission sink, such as Forest Carbon Sequestration or Biochar Soil Amendment, used in inventory category tables and removal disclosure sections. |
| description | String |  | A technical description of the removal mechanism, the permanence characteristics, and any monitoring or verification approach applicable to this sink type, supporting credible removal claims. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionSink is a reference entity that classifies a physical destination into which greenhouse gases are absorbed or sequestered, such as forest biomass, soil carbon, or geological storage. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivity.html"},{"id":"e785","label":"EmissionSink","fullName":"EmissionSink","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e802","label":"ActivityEmissionAllocat…","fullName":"ActivityEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"ActivityEmissionAllocation.html"},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/EmissionActivityFlow.html"},{"id":"e784","label":"EmissionSource","fullName":"EmissionSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionSource.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/FacilityActivityParticipation.html"},{"id":"e774","label":"EmissionActivityCategory","fullName":"EmissionActivityCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityCategory.html"},{"id":"e786","label":"EmissionActivityType","fullName":"EmissionActivityType","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityType.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c822","source":"e802","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c836","source":"e773","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c838","source":"e785","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c839","source":"e784","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c842","source":"e773","target":"e760","label":"Association","sourceLayer":"uml"},{"id":"c861","source":"e774","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c862","source":"e786","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c870","source":"e773","target":"e776","label":"Association","sourceLayer":"uml"},{"id":"c823","source":"e802","target":"e776","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 09:47:24*