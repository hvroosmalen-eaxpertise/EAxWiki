# EnvironmentalProductDeclaration

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

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

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EnvironmentalProductDeclaration is a work-product-component that represents a formal Type III environmental declaration published under a recognised EPD programme, such as those governed by ISO 14025. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 810 → 819 |

---

*Generated: 2026-06-19 13:04:05*