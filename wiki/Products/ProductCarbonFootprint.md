# ProductCarbonFootprint

**Type:** Class  **Stereotype:** work-product-component  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductCarbonFootprint is a work-product-component that records the quantified greenhouse gas emissions associated with a product within a defined system boundary, expressed per declared unit. The entity carries the full set of attributes required for WBCSD PACT-compliant data exchange, including biogenic carbon flows, land-use emissions, fossil carbon content, geography of production, allocation rules, assurance information, and cross-sectoral standards applied. It is the primary technical vehicle for sharing product-level Scope 3 emission data between organisations in a value chain.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductCarbonFootprint record, used to associate product life-cycle footprint records, factor sources, and data quality indicators with this specific PCF declaration. |
| product_footprint_id | String |  | Foreign key to the parent ProductFootprint envelope record that contains this PCF, linking the quantified data to its declaration metadata and version history. |
| declared_unit | String |  | The functional unit used as the denominator for all carbon intensity values in this record, such as 1 kg, 1 piece, or 1 kWh, which must match the declared unit defined on the parent Product. |
| pcf_excluding_biogenic | String |  | The product carbon footprint expressed in kgCO2e per declared unit, excluding all biogenic carbon flows, as required by the PACT technical specification for the primary carbon intensity disclosure. |
| pcf_including_biogenic | String |  | The product carbon footprint expressed in kgCO2e per declared unit, including all biogenic carbon flows (both emissions and removals), providing the gross biogenic-inclusive carbon intensity. |
| fossil_ghg_emissions | String |  | The total fossil-origin GHG emissions in kgCO2e per declared unit attributable to this product, excluding biogenic and land-use emissions, enabling fossil-versus-biogenic attribution analysis. |
| fossil_carbon_content | String |  | The mass of fossil carbon embedded in the product itself (not emitted) per declared unit expressed in kgC, required for waste-processing modelling by downstream users of the product. |
| biogenic_carbon_content | String |  | The mass of biogenic carbon embedded in the product per declared unit expressed in kgC, enabling biogenic carbon balance calculations for bio-based materials. |
| biogenic_carbon_withdrawal | String |  | The net biogenic CO2 removal from the atmosphere per declared unit expressed in kgCO2e, representing sequestration in biomass used to produce the product, to be reported as a negative value. |
| other_biogenic_emissions | String |  | Biogenic GHG emissions other than CO2 (principally biogenic CH4 and N2O) expressed in kgCO2e per declared unit, required for complete biogenic accounting per PACT v2. |
| i_luc_ghg_emissions | String |  | Indirect land-use change GHG emissions in kgCO2e per declared unit, covering emissions from land converted to agricultural use elsewhere due to the demand created by this product. |
| direct_land_use_ghg_emissions | String |  | Direct land-use change GHG emissions in kgCO2e per declared unit, covering emissions from land directly converted for the production of raw materials used in this product. |
| land_management_ghg_emissions | String |  | GHG emissions from ongoing land management activities (such as soil carbon change from agricultural practices) in kgCO2e per declared unit, required for comprehensive LULUCF accounting. |
| aircraft_ghg_emissions | String |  | GHG emissions from aircraft transport in kgCO2e per declared unit, required to be disclosed separately under PACT v2 because aviation emissions carry a higher warming impact than surface transport. |
| packaging_ghg_emissions | String |  | GHG emissions attributable to product packaging in kgCO2e per declared unit, disclosed separately when packaging_emissions_included is false to enable purchasers to account for packaging independently. |
| packaging_emissions_included | String |  | A flag indicating whether the packaging-related emissions are included in the pcf_excluding_biogenic value (true) or reported separately in packaging_ghg_emissions (false). |
| exempted_emissions_percent | String |  | The percentage of Scope 3 emissions that were excluded from the PCF calculation under the PACT exemption rule for minor categories, reported to support transparency on the completeness of the declared footprint. |
| exempted_emissions_description | String |  | A free-text explanation of which emission categories were excluded under the exemption and the technical justification for exclusion, as required for PACT-compliant disclosures. |
| geography_country | String |  | The ISO 3166-1 alpha-2 code of the country in which the product was manufactured or the activity with the greatest emission contribution took place, providing the primary geographic attribution for the PCF. |
| geography_country_subdivision | String |  | The ISO 3166-2 code of the specific country subdivision (state, province, or region) where production occurred, used when sub-national geographic precision is material to the footprint calculation. |
| geography_region_or_subregion | String |  | The UN M.49 code of the geographic region or sub-region applicable to this PCF when the production geography is broader than a single country, such as Northern Europe or South-East Asia. |
| location_id | String |  | Foreign key to the Location record that provides the precise geographic reference for the production site, linking the PCF to the facilities location model for detailed site-level attribution. |
| biogenic_accounting_methodology | String |  | The specific methodology used to account for biogenic carbon flows in this PCF, such as GHG Protocol Land Sector and Removals Guidance or PEF characterisation, enabling receivers to assess consistency with their own accounting approach. |
| boundary_process_description | String |  | A narrative description of the system boundary applied in the PCF, listing the life-cycle stages included and excluded and the rationale for any cut-off decisions. |
| allocation_rules_description | String |  | A description of the allocation rules applied when emissions from shared processes or co-products were partitioned among products, as required for transparency and for receivers assessing the fairness of cost assignment. |
| uncertainty_assessment_description | String |  | A free-text summary of the data quality and uncertainty assessment performed for this PCF, covering data completeness, temporal representativeness, technological representativeness, and geographic representativeness. |
| assurance | String |  | Information about any third-party assurance or verification obtained for this PCF declaration, including the name of the assurance provider, the standard applied, and the level of assurance achieved. |
| cross_sectoral_standards_used | String |  | A list of the cross-sectoral GHG accounting standards applied in producing this PCF, such as GHG Protocol Product Standard, ISO 14067:2018, or PACT Framework, supporting methodological traceability. |
| product_or_sector_specific_rules | String |  | A reference to any product category rules (PCR) or sector-specific accounting guidance applied, such as an EPD programme operator PCR or a PACT sector extension, supplementing the cross-sectoral standards. |
| secondary_emission_factor_sources | String |  | A list of secondary emission factor databases used for background processes in the PCF calculation when primary supplier-specific data was unavailable, such as ecoinvent v3.9 or GaBi databases. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductCarbonFootprint is a work-product-component that records the quantified greenhouse gas emissions associated with a product within a defined system boundary, expressed per declared unit. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Location](../Facilities/Location.md) |
| Association |  | [ProductFootprintDataQualityIndicator](ProductFootprintDataQualityIndicator.md) |
| Association |  | [ProductCategoryRule](ProductCategoryRule.md) |
| Association |  | [ProductCarbonFootprintFactorSource](ProductCarbonFootprintFactorSource.md) |
| Association |  | [ProductLifeCycleFootprint](ProductLifeCycleFootprint.md) |
| Association |  | [ProductFootprint](ProductFootprint.md) |
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [ProductFootprint](ProductFootprint.md) |
| Association |  | [ProductFootprint](ProductFootprint.md) |
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.md) |
| Association |  | [UnitOfMeasure](../Emissions/UnitOfMeasure.md) |

---

*Generated: 2026-06-25 10:51:16*