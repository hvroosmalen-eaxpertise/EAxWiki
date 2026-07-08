---
ea_id: 784
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 096fdc3a
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionSource

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. Emission sources provide a more granular technical classification than the EmissionActivityType and are used in calculation model selection and emission factor lookup to narrow the applicable factor set to the correct physical process.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionSource record, used by EmissionActivity and EmissionFactor records to associate a specific physical source with a measurement or factor entry. |
| name | String |  | The human-readable name of the emission source, such as Stationary Combustion Natural Gas or Fugitive Emissions HFC-134a refrigerant, used in data entry forms and technical reports. |
| description | String |  | A technical description of the emission mechanism and the physical or chemical process by which the GHG is released, supporting correct classification and factor selection by practitioners. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionSource is a reference entity that classifies the physical origin from which greenhouse gas emissions are released, such as natural gas combustion, diesel combustion, refrigerant leakage, or wastewater treatment. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivity](EmissionActivity.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;EmissionActivity&quot;,&quot;fullName&quot;:&quot;EmissionActivity&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivity.html&quot;},{&quot;id&quot;:&quot;e784&quot;,&quot;label&quot;:&quot;EmissionSource&quot;,&quot;fullName&quot;:&quot;EmissionSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e802&quot;,&quot;label&quot;:&quot;ActivityEmissionAllocat…&quot;,&quot;fullName&quot;:&quot;ActivityEmissionAllocation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ActivityEmissionAllocation.html&quot;},{&quot;id&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;EmissionActivityFlow&quot;,&quot;fullName&quot;:&quot;EmissionActivityFlow&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/EmissionActivityFlow.html&quot;},{&quot;id&quot;:&quot;e785&quot;,&quot;label&quot;:&quot;EmissionSink&quot;,&quot;fullName&quot;:&quot;EmissionSink&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionSink.html&quot;},{&quot;id&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;FacilityActivityPartici…&quot;,&quot;fullName&quot;:&quot;FacilityActivityParticipation&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/FacilityActivityParticipation.html&quot;},{&quot;id&quot;:&quot;e774&quot;,&quot;label&quot;:&quot;EmissionActivityCategory&quot;,&quot;fullName&quot;:&quot;EmissionActivityCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityCategory.html&quot;},{&quot;id&quot;:&quot;e786&quot;,&quot;label&quot;:&quot;EmissionActivityType&quot;,&quot;fullName&quot;:&quot;EmissionActivityType&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityType.html&quot;},{&quot;id&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;EmissionStatement&quot;,&quot;fullName&quot;:&quot;EmissionStatement&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionStatement.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c822&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c836&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e814&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c838&quot;,&quot;source&quot;:&quot;e785&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c839&quot;,&quot;source&quot;:&quot;e784&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c842&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e760&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c861&quot;,&quot;source&quot;:&quot;e774&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c862&quot;,&quot;source&quot;:&quot;e786&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c863&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e773&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c870&quot;,&quot;source&quot;:&quot;e773&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c823&quot;,&quot;source&quot;:&quot;e802&quot;,&quot;target&quot;:&quot;e776&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*