# EmissionComponent

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately. This granularity supports characterisation factor application, gas-specific disclosure requirements, and cross-protocol comparability beyond CO2-equivalent totals alone.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionComponent record, used when linking per-standard breakdowns via EmissionComponentPerStandard or when applying gas-specific characterisation factors. |
| emission_statement_id | String |  | Foreign key to the parent EmissionStatement that this component disaggregates, linking the gas-level detail to its aggregated emission total. |
| emission_component_category_id | String |  | Foreign key to the EmissionComponentCategory that classifies which greenhouse gas or aggregate category this component represents, such as CO2 fossil, CH4, or CO2e total. |
| quantity | String |  | The emission quantity for this specific gas component, expressed in the unit referenced by quantity_unit_of_measure_id, before global-warming-potential conversion. |
| quantity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure record that expresses the unit for this component quantity, such as t (metric tonnes) for gas mass or tCO2e for GWP-weighted totals. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionComponent is a work-product-component that disaggregates an EmissionStatement into its constituent greenhouse gas contributions, recording the quantity of each individual gas (CO2, CH4, N2O, HFCs, PFCs, SF6, NF3) or aggregate measure separately. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 789 → 778 |
| Association |  | 779 → 778 |
| Association |  | 792 → 778 |
| Association |  | 776 → 778 |

---

*Generated: 2026-06-18 13:54:45*