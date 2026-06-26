# <span class="sl" data-layer="uml">work-product-component</span> EnvironmentalProductDeclaration

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

EnvironmentalProductDeclaration is a work-product-component that represents a formal Type III environmental declaration published under a recognised EPD programme, such as those governed by ISO 14025. An EPD discloses the life-cycle environmental impacts of a product across multiple impact categories, of which the global warming potential (GWP) impact category is directly comparable to a product carbon footprint. Linking an EPD record to a product enables organisations to reference externally verified declarations alongside internally calculated footprints in their supply-chain disclosures.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EnvironmentalProductDeclaration record, referenced by the Product and ProductFootprint records that this EPD supports or supplements. |
| product_id | String |  | Foreign key to the Product that this EPD covers, linking the externally verified declaration to the product master data record. |
| programme_operator | String |  | The name of the EPD programme operator under whose rules and verification scheme this EPD was produced and registered, such as EPD International, Institut Bauen und Umwelt (IBU), or The Norwegian EPD Foundation. |
| registration_number | String |  | The unique registration number assigned by the programme operator to this EPD, providing a stable external reference for document retrieval and verification. |
| publication_date | String |  | The date on which the EPD was published by the programme operator, establishing the document date used for temporal representativeness assessments. |
| valid_until | String |  | The expiry date of the EPD, after which a re-verification or update is required, used to flag outdated declarations in data quality checks. |
| gwp_value | String |  | The global warming potential impact result from the EPD, expressed in kgCO2e per declared unit (A1-A5 or full cradle-to-grave depending on the product category rule), directly comparable to the ProductCarbonFootprint pcf_excluding_biogenic value. |
| gwp_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure in which the gwp_value is expressed, confirming the functional unit and enabling correct comparison with ProductCarbonFootprint records. |
| url | String |  | A persistent URL to the published EPD document in the programme operator registry, enabling automated retrieval and verification of the declaration. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EnvironmentalProductDeclaration is a work-product-component that represents a formal Type III environmental declaration published under a recognised EPD programme, such as those governed by ISO 14025. |  |

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
<script>window.eaGraphData={"nodes":[{"id":"e810","label":"Product","fullName":"Product","packageName":Products,"isFocal":false,"hasUrl":true,"url":"Product.html"},{"id":"e819","label":"EnvironmentalProductDec…","fullName":"EnvironmentalProductDeclaration","packageName":Products,"isFocal":true,"hasUrl":false,"url":""},{"id":"e815","label":"CentralProductClassific…","fullName":"CentralProductClassificationCode","packageName":Products,"isFocal":false,"hasUrl":true,"url":"CentralProductClassificationCode.html"},{"id":"e813","label":"ProductLifeCycle","fullName":"ProductLifeCycle","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductLifeCycle.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":Products,"isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"}],"edges":[{"id":"c797","source":"e810","target":"e819","label":"Association"},{"id":"c799","source":"e810","target":"e815","label":"Association"},{"id":"c801","source":"e810","target":"e813","label":"Association"},{"id":"c805","source":"e810","target":"e811","label":"Association"},{"id":"c794","source":"e811","target":"e811","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:44:31*