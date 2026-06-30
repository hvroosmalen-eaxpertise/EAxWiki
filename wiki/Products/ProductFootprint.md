---
ea_id: 811
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
---

# <span class="sl" data-layer="uml">work-product-component</span> ProductFootprint

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration. It holds the declaration metadata including the status, version history, validity period, and preceding footprint reference, grouping one or more ProductCarbonFootprint records that carry the quantified GHG data. The ProductFootprint entity aligns with the PACT technical specification version 2 data model, where a footprint declaration can be updated or superseded while retaining the historical chain of prior versions.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductFootprint envelope record, typically a UUID, used to reference the footprint in data exchange messages and to link the footprint to its superseding or preceding records. |
| product_id | String |  | Foreign key to the Product that this footprint declaration covers, linking the quantified carbon data to its product master record. |
| spec_version | String |  | The version of the PACT technical specification (or other applicable framework) under which this footprint was compiled, such as 2.0.0 or 2.1.0, enabling receivers to apply the correct validation rules. |
| version | String |  | A sequential integer counter indicating the revision number of this footprint declaration, incremented each time a correction or update is published for the same product and scope. |
| status | String |  | The lifecycle status of the footprint declaration, drawn from PACT values: Active, Deprecated, or an implementation-specific value, used to determine whether the declaration is current or superseded. |
| status_comment | String |  | A free-text explanation of the current status, particularly when the status is Deprecated or Inactive, describing the reason for the change and any corrective action taken. |
| validity_period_start | String |  | The first date for which this footprint declaration is valid and may be used by receiving parties, establishing when the declaration becomes effective. |
| validity_period_end | String |  | The last date for which this footprint declaration is valid; a null value indicates no defined expiry, while a populated date requires the receiver to obtain a refreshed declaration after this date. |
| preceding_product_footprint_id | String |  | A self-referential foreign key pointing to the ProductFootprint record that this declaration supersedes or corrects, enabling receivers to navigate the revision history and always identify the latest active version. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductFootprint](ProductFootprint.md) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [Product](Product.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Product](Product.md) |
| Association |  | [ProductFootprint](ProductFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"},{"id":"e810","label":"Product","fullName":"Product","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"Product.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e821","label":"ProductFootprintDataQua…","fullName":"ProductFootprintDataQualityIndicator","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprintDataQualityIndicator.html"},{"id":"e820","label":"ProductCategoryRule","fullName":"ProductCategoryRule","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCategoryRule.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprintFactorSource.html"},{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycleFootprint.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"},{"id":"e819","label":"EnvironmentalProductDec…","fullName":"EnvironmentalProductDeclaration","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"EnvironmentalProductDeclaration.html"},{"id":"e815","label":"CentralProductClassific…","fullName":"CentralProductClassificationCode","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"CentralProductClassificationCode.html"},{"id":"e813","label":"ProductLifeCycle","fullName":"ProductLifeCycle","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycle.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c795","source":"e812","target":"e821","label":"Association","sourceLayer":"uml"},{"id":"c796","source":"e812","target":"e820","label":"Association","sourceLayer":"uml"},{"id":"c798","source":"e812","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c803","source":"e812","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c804","source":"e811","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c797","source":"e810","target":"e819","label":"Association","sourceLayer":"uml"},{"id":"c799","source":"e810","target":"e815","label":"Association","sourceLayer":"uml"},{"id":"c801","source":"e810","target":"e813","label":"Association","sourceLayer":"uml"},{"id":"c805","source":"e810","target":"e811","label":"Association","sourceLayer":"uml"},{"id":"c794","source":"e811","target":"e811","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-30 17:14:23*