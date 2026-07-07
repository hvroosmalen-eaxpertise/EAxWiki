---
ea_id: 819
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 7fd275b9
---

# <span class="sl" data-layer="uml">work-product-component</span> EnvironmentalProductDeclaration

**Type:** Class  **Stereotype:** work-product-component  **StereotypeEx:** work-product-component  **FQStereotype:** work-product-component  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
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
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e810&quot;,&quot;label&quot;:&quot;Product&quot;,&quot;fullName&quot;:&quot;Product&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Product.html&quot;},{&quot;id&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;EnvironmentalProductDec…&quot;,&quot;fullName&quot;:&quot;EnvironmentalProductDeclaration&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;CentralProductClassific…&quot;,&quot;fullName&quot;:&quot;CentralProductClassificationCode&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;CentralProductClassificationCode.html&quot;},{&quot;id&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;ProductLifeCycle&quot;,&quot;fullName&quot;:&quot;ProductLifeCycle&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductLifeCycle.html&quot;},{&quot;id&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;ProductFootprint&quot;,&quot;fullName&quot;:&quot;ProductFootprint&quot;,&quot;packageName&quot;:&quot;Products&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;ProductFootprint.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c797&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e819&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c799&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e815&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c801&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e813&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c805&quot;,&quot;source&quot;:&quot;e810&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c794&quot;,&quot;source&quot;:&quot;e811&quot;,&quot;target&quot;:&quot;e811&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*