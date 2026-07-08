---
ea_id: 781
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7abf150f
---

# <span class="sl" data-layer="uml">master-data</span> EmissionFactorSource

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn. Examples include the UK DESNZ Conversion Factors, the US EPA Emission Factors Hub, the IPCC Assessment Report factor sets, and the ecoinvent database. Registering sources as a reference entity ensures that every factor used in a calculation can be traced to its origin, supporting third-party verification and regulatory acceptance.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionFactorSource record, referenced by EmissionFactor records to indicate provenance and by StandardSourceAssociation to link source databases to standards. |
| name | String |  | The official name of the emission factor source, such as UK Department for Energy Security and Net Zero Greenhouse Gas Conversion Factors, used in citations and report references. |
| description | String |  | A description of the scope, coverage, update frequency, and methodology of the source database, helping practitioners assess suitability for their reporting context. |
| publisher | String |  | The organisation responsible for maintaining and publishing the emission factor source, such as DESNZ, US EPA, or IPCC, used in bibliographic citations. |
| version | String |  | The edition or release version of the source publication, such as 2024 or v8.2, used to distinguish factor sets from different reporting years when multiple versions are retained. |
| publication_date | String |  | The date on which this version of the source was published or made effective, used to determine which version was current during a given reporting period. |
| url | String |  | A persistent URL or DOI pointing to the source publication or download location, enabling automated factor retrieval and supporting auditability. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionFactorSource is a reference entity that identifies the authoritative database, publication, or programme from which emission factors are drawn. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [StandardSourceAssociation](StandardSourceAssociation.md) |
| Association |  | [ProductCarbonFootprintFactorSource](../Products/ProductCarbonFootprintFactorSource.md) |
| Association |  | [StandardSourceAssociation](StandardSourceAssociation.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [StandardSourceAssociation](StandardSourceAssociation.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e794&quot;,&quot;label&quot;:&quot;StandardSourceAssociati…&quot;,&quot;fullName&quot;:&quot;StandardSourceAssociation&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;StandardSourceAssociation.html&quot;},{&quot;id&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;ProductCarbonFootprintF…&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprintFactorSource&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprintFactorSource.html&quot;},{&quot;id&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;EmissionFactor&quot;,&quot;fullName&quot;:&quot;EmissionFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionFactor.html&quot;},{&quot;id&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;EmissionFactorSource&quot;,&quot;fullName&quot;:&quot;EmissionFactorSource&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Standard&quot;,&quot;fullName&quot;:&quot;Standard&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Organisation/Standard.html&quot;},{&quot;id&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;ProductCarbonFootprint&quot;,&quot;fullName&quot;:&quot;ProductCarbonFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Products/ProductCarbonFootprint.html&quot;},{&quot;id&quot;:&quot;e804&quot;,&quot;label&quot;:&quot;EmissionCalculationMode…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationModelFactorArgument&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationModelFactorArgument.html&quot;},{&quot;id&quot;:&quot;e803&quot;,&quot;label&quot;:&quot;EmissionCalculationForm…&quot;,&quot;fullName&quot;:&quot;EmissionCalculationFormulaComponent&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionCalculationFormulaComponent.html&quot;},{&quot;id&quot;:&quot;e793&quot;,&quot;label&quot;:&quot;EmissionActivityFactor&quot;,&quot;fullName&quot;:&quot;EmissionActivityFactor&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionActivityFactor.html&quot;},{&quot;id&quot;:&quot;e779&quot;,&quot;label&quot;:&quot;UnitOfMeasure&quot;,&quot;fullName&quot;:&quot;UnitOfMeasure&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;UnitOfMeasure.html&quot;},{&quot;id&quot;:&quot;e792&quot;,&quot;label&quot;:&quot;EmissionComponentCatego…&quot;,&quot;fullName&quot;:&quot;EmissionComponentCategory&quot;,&quot;packageName&quot;:&quot;Emissions&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;EmissionComponentCategory.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c806&quot;,&quot;source&quot;:&quot;e781&quot;,&quot;target&quot;:&quot;e794&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c849&quot;,&quot;source&quot;:&quot;e794&quot;,&quot;target&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c850&quot;,&quot;source&quot;:&quot;e794&quot;,&quot;target&quot;:&quot;e734&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c798&quot;,&quot;source&quot;:&quot;e812&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c835&quot;,&quot;source&quot;:&quot;e781&quot;,&quot;target&quot;:&quot;e818&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c817&quot;,&quot;source&quot;:&quot;e804&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c820&quot;,&quot;source&quot;:&quot;e803&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c852&quot;,&quot;source&quot;:&quot;e793&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c854&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c855&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c856&quot;,&quot;source&quot;:&quot;e792&quot;,&quot;target&quot;:&quot;e780&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c857&quot;,&quot;source&quot;:&quot;e780&quot;,&quot;target&quot;:&quot;e781&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c833&quot;,&quot;source&quot;:&quot;e779&quot;,&quot;target&quot;:&quot;e812&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*