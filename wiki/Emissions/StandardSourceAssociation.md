# <span class="sl" data-layer="uml">master-data</span> StandardSourceAssociation

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard. A standard such as the GHG Protocol may endorse specific factor databases (IPCC, national inventory agencies, DESNZ) while another framework mandates different sources. Capturing these endorsements as data avoids hard-coding source eligibility rules in application logic and supports audit queries confirming that all factors used were sourced from a framework-approved database.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this StandardSourceAssociation record, used in audit queries to verify that emission factors were drawn from a source approved under the applicable standard. |
| standard_id | String |  | Foreign key to the Standard that endorses or mandates the use of the referenced emission factor source. |
| emission_factor_source_id | String |  | Foreign key to the EmissionFactorSource that is endorsed or mandated by the referenced standard, establishing the permissible source set for calculations under that framework. |
| notes | String |  | Free-text notes describing any conditions or restrictions on the use of this source under the referenced standard, such as mandatory for UK Scope 2 market-based method. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | StandardSourceAssociation is an intersection entity that records which emission factor databases or reference sources are recognised as appropriate inputs under a given Standard. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |
| Association |  | [Standard](../Organisation/Standard.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionFactorSource](EmissionFactorSource.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionFactorSource.html"},{"id":"e734","label":"Standard","fullName":"Standard","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/Standard.html"},{"id":"e794","label":"StandardSourceAssociati…","fullName":"StandardSourceAssociation","packageName":"Emissions","isFocal":true,"hasUrl":false,"url":""},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","isFocal":false,"hasUrl":true,"url":"../Products/ProductCarbonFootprintFactorSource.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e788","label":"EmissionStatementPerSta…","fullName":"EmissionStatementPerStandard","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionStatementPerStandard.html"},{"id":"e787","label":"EmissionCategoryStandar…","fullName":"EmissionCategoryStandardAssociation","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCategoryStandardAssociation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e777","label":"EmissionCalculationModel","fullName":"EmissionCalculationModel","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModel.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","isFocal":false,"hasUrl":true,"url":"EmissionInventory.html"}],"edges":[{"id":"c806","source":"e781","target":"e794","label":"Association"},{"id":"c835","source":"e781","target":"e818","label":"Association"},{"id":"c849","source":"e794","target":"e781","label":"Association"},{"id":"c857","source":"e780","target":"e781","label":"Association"},{"id":"c843","source":"e789","target":"e734","label":"Association"},{"id":"c845","source":"e788","target":"e734","label":"Association"},{"id":"c847","source":"e787","target":"e734","label":"Association"},{"id":"c850","source":"e794","target":"e734","label":"Association"},{"id":"c894","source":"e734","target":"e782","label":"Association"},{"id":"c897","source":"e734","target":"e740","label":"Association"},{"id":"c916","source":"e734","target":"e777","label":"Association"},{"id":"c918","source":"e734","target":"e771","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:40*