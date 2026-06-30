# <span class="sl" data-layer="uml">master-data</span> EmissionFactorSource

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
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
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"StandardSourceAssociation.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprintFactorSource.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprint.html"},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategory.html"}],"edges":[{"id":"c806","source":"e781","target":"e794","label":"Association","sourceLayer":"uml"},{"id":"c849","source":"e794","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c850","source":"e794","target":"e734","label":"Association","sourceLayer":"uml"},{"id":"c798","source":"e812","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c835","source":"e781","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c817","source":"e804","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c856","source":"e792","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c857","source":"e780","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 11:43:29*