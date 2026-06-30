---
ea_id: 815
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">reference-data</span> CentralProductClassificationCode

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or supply-chain matching purposes. Linking a product to its CPC code supports automated mapping to applicable product category rules, sector-specific emission factors, and ESRS sector-classification requirements.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this CentralProductClassificationCode record, referenced by Product records and ProductCategoryRule records to establish the product classification. |
| classification_system | String |  | The name of the classification system from which this code is drawn, such as UN CPC v2.1, NACE Rev.2, or HS 2022, enabling correct code interpretation by receiving systems. |
| code | String |  | The alphanumeric code value within the referenced classification system, such as 27110 for Mining of hard coal under UN CPC, used for cross-system product matching and regulatory filing. |
| name | String |  | The human-readable label assigned to this code within its classification system, used in disclosures and product catalogues alongside the numeric code. |
| description | String |  | A description of the product scope covered by this classification code, supporting practitioners in assigning the correct code to products that span multiple categories. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or  |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
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
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e810","label":"Product","fullName":"Product","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"Product.html"},{"id":"e815","label":"CentralProductClassific…","fullName":"CentralProductClassificationCode","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e819","label":"EnvironmentalProductDec…","fullName":"EnvironmentalProductDeclaration","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"EnvironmentalProductDeclaration.html"},{"id":"e813","label":"ProductLifeCycle","fullName":"ProductLifeCycle","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycle.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"}],"edges":[{"id":"c797","source":"e810","target":"e819","label":"Association","sourceLayer":"uml"},{"id":"c799","source":"e810","target":"e815","label":"Association","sourceLayer":"uml"},{"id":"c801","source":"e810","target":"e813","label":"Association","sourceLayer":"uml"},{"id":"c805","source":"e810","target":"e811","label":"Association","sourceLayer":"uml"},{"id":"c794","source":"e811","target":"e811","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 17:14:23*