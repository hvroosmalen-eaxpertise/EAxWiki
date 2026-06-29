# <span class="sl" data-layer="uml">master-data</span> ProductLifeCycle

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. It groups the individual ProductLifeCycleStage records that enumerate each stage (raw material extraction, manufacturing, distribution, use, end-of-life) and anchors the ProductLifeCycleFootprint records that hold stage-level emission quantities. Defining the life cycle as a structured entity rather than a string field enables stage-level comparisons across products and supply-chain tiers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycle record, referenced by ProductLifeCycleStage and ProductLifeCycleFootprint records to associate stage definitions with their footprint contributions. |
| product_id | String |  | Foreign key to the Product whose life-cycle system boundary this record defines, linking the life-cycle structure to the product master record. |
| name | String |  | A descriptive label for this life-cycle scope definition, such as Cradle-to-gate, Cradle-to-grave, or Gate-to-gate, indicating the extent of the boundary and enabling high-level comparison across products. |
| description | String |  | A narrative description of the system boundary, listing the included and excluded life-cycle stages, the functional unit, and any allocation decisions that apply to the life cycle as a whole. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycle is a master-data entity that defines the set of life-cycle stages considered in a product carbon footprint assessment, establishing the system boundary for a given product and footprint scope. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionActivityFlow](EmissionActivityFlow.md) |
| Association |  | [Product](Product.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Product](Product.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFlow.html"},{"id":"e810","label":"Product","fullName":"Product","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"Product.html"},{"id":"e813","label":"ProductLifeCycle","fullName":"ProductLifeCycle","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivity.html"},{"id":"e819","label":"EnvironmentalProductDec…","fullName":"EnvironmentalProductDeclaration","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"EnvironmentalProductDeclaration.html"},{"id":"e815","label":"CentralProductClassific…","fullName":"CentralProductClassificationCode","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"CentralProductClassificationCode.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"}],"edges":[{"id":"c800","source":"e813","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c836","source":"e773","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c797","source":"e810","target":"e819","label":"Association","sourceLayer":"uml"},{"id":"c799","source":"e810","target":"e815","label":"Association","sourceLayer":"uml"},{"id":"c801","source":"e810","target":"e813","label":"Association","sourceLayer":"uml"},{"id":"c805","source":"e810","target":"e811","label":"Association","sourceLayer":"uml"},{"id":"c863","source":"e773","target":"e773","label":"Association","sourceLayer":"uml"},{"id":"c794","source":"e811","target":"e811","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 13:30:17*