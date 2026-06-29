# <span class="sl" data-layer="uml">master-data</span> Product

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared. It acts as the anchor for all product-level sustainability data, linking the product to its classification codes (CentralProductClassificationCode), its product carbon footprint records (ProductFootprint), and the life-cycle stages used to structure its footprint assessment. In a supply-chain context, Product enables data exchange between buyer and supplier organisations under the WBCSD PACT framework.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this Product record, referenced by ProductFootprint, EmissionActivityFlow, and classification code association records. |
| name | String |  | The human-readable commercial or technical name of the product, used in disclosures, data exchanges, and product catalogue displays. |
| description | String |  | A free-text description of the product, including its functional unit, primary materials, and typical use context, providing the background needed to interpret its carbon footprint data. |
| product_id_type | String |  | The type of product identifier used in the product_id field, such as UUID, EAN, GTIN-13, or internal SKU code, enabling correct interpretation of the identifier by receiving systems. |
| product_id | String |  | The specific identifier for this product in the system indicated by product_id_type, such as a GTIN barcode or internal stock-keeping unit number, supporting cross-system product matching. |
| unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure that defines the declared unit (functional unit) for this product, such as 1 kg, 1 piece, or 1 kWh, establishing the denominator for all carbon intensity expressions. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EnvironmentalProductDeclaration](EnvironmentalProductDeclaration.md) |
| Association |  | [CentralProductClassificationCode](CentralProductClassificationCode.md) |
| Association |  | [ProductLifeCycle](ProductLifeCycle.md) |
| Association |  | [ProductFootprint](ProductFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e819","label":"EnvironmentalProductDec…","fullName":"EnvironmentalProductDeclaration","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"EnvironmentalProductDeclaration.html"},{"id":"e815","label":"CentralProductClassific…","fullName":"CentralProductClassificationCode","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"CentralProductClassificationCode.html"},{"id":"e813","label":"ProductLifeCycle","fullName":"ProductLifeCycle","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycle.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"},{"id":"e810","label":"Product","fullName":"Product","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e814","label":"EmissionActivityFlow","fullName":"EmissionActivityFlow","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"EmissionActivityFlow.html"},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"}],"edges":[{"id":"c797","source":"e810","target":"e819","label":"Association","sourceLayer":"uml"},{"id":"c799","source":"e810","target":"e815","label":"Association","sourceLayer":"uml"},{"id":"c800","source":"e813","target":"e814","label":"Association","sourceLayer":"uml"},{"id":"c801","source":"e810","target":"e813","label":"Association","sourceLayer":"uml"},{"id":"c794","source":"e811","target":"e811","label":"Association","sourceLayer":"uml"},{"id":"c804","source":"e811","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c805","source":"e810","target":"e811","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 18:57:23*