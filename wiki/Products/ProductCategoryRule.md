---
ea_id: 820
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 8b6de569
---

# <span class="sl" data-layer="uml">master-data</span> ProductCategoryRule

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="820" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Products/ProductCategoryRule.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="820" data-file-path="Products/ProductCategoryRule.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product category. PCRs constrain the methodological choices practitioners can make when calculating a PCF, specifying system boundary, allocation rules, data quality requirements, and declared unit definitions for the product type. Linking a PCR to a ProductCarbonFootprint provides essential methodological context for comparison across competing products.</p>
<!--ea-notes-end-->
</div>
</div>

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

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductCategoryRule is a master-data entity that represents a product category rule (PCR) document that provides specific methodological requirements for conducting life-cycle assessments and producing EPDs or PCF declarations for a defined product catego |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductCarbonFootprint](ProductCarbonFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"},{"id":"e820","label":"ProductCategoryRule","fullName":"ProductCategoryRule","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e755","label":"Location","fullName":"Location","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Location.html"},{"id":"e821","label":"ProductFootprintDataQua…","fullName":"ProductFootprintDataQualityIndicator","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprintDataQualityIndicator.html"},{"id":"e818","label":"ProductCarbonFootprintF…","fullName":"ProductCarbonFootprintFactorSource","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprintFactorSource.html"},{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycleFootprint.html"},{"id":"e811","label":"ProductFootprint","fullName":"ProductFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductFootprint.html"},{"id":"e779","label":"UnitOfMeasure","fullName":"UnitOfMeasure","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/UnitOfMeasure.html"}],"edges":[{"id":"c792","source":"e812","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c795","source":"e812","target":"e821","label":"Association","sourceLayer":"uml"},{"id":"c796","source":"e812","target":"e820","label":"Association","sourceLayer":"uml"},{"id":"c798","source":"e812","target":"e818","label":"Association","sourceLayer":"uml"},{"id":"c803","source":"e812","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c804","source":"e811","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c833","source":"e779","target":"e812","label":"Association","sourceLayer":"uml"},{"id":"c881","source":"e755","target":"e755","label":"Association","sourceLayer":"uml"},{"id":"c794","source":"e811","target":"e811","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*