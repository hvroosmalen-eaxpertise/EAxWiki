---
ea_id: 792
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> EmissionComponentCategory

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record. The seven Kyoto Protocol gases (CO2, CH4, N2O, HFCs, PFCs, SF6, and NF3) are the primary values, supplemented by aggregate categories such as CO2e total (AR5 GWPs) or biogenic CO2. Categorising components at this level enables gas-specific aggregation, GWP factor application, and the gas-level disclosures required by ISO 14064-1 and ESRS E1.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionComponentCategory record, referenced by EmissionComponent, EmissionFactor, and EmissionComponentCategoryGroup records to classify gas-level data. |
| emission_component_category_group_id | String |  | Foreign key to the EmissionComponentCategoryGroup that this category belongs to, enabling roll-up from individual gas categories to higher-level groupings such as Fluorinated gases or Biogenic. |
| name | String |  | The standard name for the GHG category, such as Carbon Dioxide Fossil, Methane, or Nitrous Oxide, used in component-level disclosures and factor lookup tables. |
| gwp_ar4 | String |  | The global warming potential of this gas using IPCC Fourth Assessment Report (AR4) characterisation factors, retained for legacy reporting requirements that mandate AR4 values. |
| gwp_ar5 | String |  | The global warming potential using IPCC Fifth Assessment Report (AR5) characterisation factors, applicable to reporting periods under GHG Protocol Corporate Standard and many national inventories. |
| gwp_ar6 | String |  | The global warming potential using IPCC Sixth Assessment Report (AR6) characterisation factors, required for reporting under the latest ESRS E1 and PACT v2+ specifications. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionComponent](EmissionComponent.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e795","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategoryGroup","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentCategoryGroup.html"},{"id":"e780","label":"EmissionFactor","fullName":"EmissionFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactor.html"},{"id":"e778","label":"EmissionComponent","fullName":"EmissionComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponent.html"},{"id":"e792","label":"EmissionComponentCatego…","fullName":"EmissionComponentCategory","packageName":"Emissions","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e804","label":"EmissionCalculationMode…","fullName":"EmissionCalculationModelFactorArgument","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationModelFactorArgument.html"},{"id":"e803","label":"EmissionCalculationForm…","fullName":"EmissionCalculationFormulaComponent","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionCalculationFormulaComponent.html"},{"id":"e793","label":"EmissionActivityFactor","fullName":"EmissionActivityFactor","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFactor.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"UnitOfMeasure.html"},{"id":"e781","label":"EmissionFactorSource","fullName":"EmissionFactorSource","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionFactorSource.html"},{"id":"e789","label":"EmissionComponentPerSta…","fullName":"EmissionComponentPerStandard","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionComponentPerStandard.html"},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionStatement.html"}],"edges":[{"id":"c809","source":"e792","target":"e795","label":"Association","sourceLayer":"uml"},{"id":"c831","source":"e795","target":"e792","label":"Association","sourceLayer":"uml"},{"id":"c817","source":"e804","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c820","source":"e803","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c852","source":"e793","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c854","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c855","source":"e779","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c856","source":"e792","target":"e780","label":"Association","sourceLayer":"uml"},{"id":"c857","source":"e780","target":"e781","label":"Association","sourceLayer":"uml"},{"id":"c844","source":"e789","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c864","source":"e779","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c866","source":"e792","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c867","source":"e776","target":"e778","label":"Association","sourceLayer":"uml"},{"id":"c865","source":"e779","target":"e776","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 14:47:48*