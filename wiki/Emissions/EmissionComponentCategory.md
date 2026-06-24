# EmissionComponentCategory

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record. The seven Kyoto Protocol gases (CO2, CH4, N2O, HFCs, PFCs, SF6, and NF3) are the primary values, supplemented by aggregate categories such as CO2e total (AR5 GWPs) or biogenic CO2. Categorising components at this level enables gas-specific aggregation, GWP factor application, and the gas-level disclosures required by ISO 14064-1 and ESRS E1.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionComponentCategory record, referenced by EmissionComponent, EmissionFactor, and EmissionComponentCategoryGroup records to classify gas-level data. |
| emission_component_category_group_id | String |  | Foreign key to the EmissionComponentCategoryGroup that this category belongs to, enabling roll-up from individual gas categories to higher-level groupings such as Fluorinated gases or Biogenic. |
| name | String |  | The standard name for the GHG category, such as Carbon Dioxide Fossil, Methane, or Nitrous Oxide, used in component-level disclosures and factor lookup tables. |
| gwp_ar4 | String |  | The global warming potential of this gas using IPCC Fourth Assessment Report (AR4) characterisation factors, retained for legacy reporting requirements that mandate AR4 values. |
| gwp_ar5 | String |  | The global warming potential using IPCC Fifth Assessment Report (AR5) characterisation factors, applicable to reporting periods under GHG Protocol Corporate Standard and many national inventories. |
| gwp_ar6 | String |  | The global warming potential using IPCC Sixth Assessment Report (AR6) characterisation factors, required for reporting under the latest ESRS E1 and PACT v2+ specifications. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponentCategory is a reference entity that classifies the greenhouse gas type or aggregate measure represented by an EmissionComponent record. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionFactor](EmissionFactor.md) |
| Association |  | [EmissionComponent](EmissionComponent.md) |

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |
| Association |  | [EmissionComponentCategoryGroup](EmissionComponentCategoryGroup.md) |

---

*Generated: 2026-06-24 10:33:17*