# ProductCategoryRule

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product category. PCRs constrain the methodological choices practitioners can make when calculating a PCF, specifying system boundary, allocation rules, data quality requirements, and declared unit definitions for the product type. Linking a PCR to a ProductCarbonFootprint provides essential methodological context for comparison across competing products.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductCategoryRule record, referenced by ProductCarbonFootprint records via product_or_sector_specific_rules to indicate which PCR governed the calculation methodology. |
| name | String |  | The title of the PCR document, such as PCR 2019:14 Construction products and construction services v1.3.4, used in citations and footprint disclosure notes. |
| product_category | String |  | The product category or CPC code range to which this PCR applies, indicating which types of product should apply the rule when declaring a carbon footprint or EPD. |
| programme_operator | String |  | The EPD programme operator or standards body that published the PCR, such as EPD International or ecoinvent Association, identifying the authority that must approve deviations from the rule. |
| version | String |  | The version or edition of the PCR document, used to ensure that footprint declarations reference the correct version in force during the assessment period and to manage PCR updates. |
| valid_from | String |  | The date from which this PCR version is applicable, used to select the correct rule for assessments conducted during a given period. |
| valid_to | String |  | The date after which this PCR version is superseded, triggering a requirement to update assessments using a newer version. |
| url | String |  | A persistent URL to the PCR document, enabling automated retrieval and supporting auditability of the methodological choices made in the footprint calculation. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product catego |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

---

*Generated: 2026-06-24 10:33:17*