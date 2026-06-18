# EmissionCalculationFormula

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values. A formula belongs to one calculation model and may be decomposed into multiple EmissionCalculationFormulaComponent records that capture the individual multiplicative terms. Storing formulas as structured data rather than code allows them to be audited, versioned, and applied consistently by calculation engines without custom programming per model.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionCalculationFormula record, referenced by its parent EmissionCalculationModel and by the component records that decompose it into terms. |
| emission_calculation_model_id | String |  | Foreign key to the EmissionCalculationModel that this formula belongs to, ensuring that each model mathematical logic is fully traceable to its component terms. |
| name | String |  | A human-readable label for the formula, such as Fuel Combustion CO2 Formula Tier 2, used in model documentation and audit trail displays. |
| description | String |  | A narrative description of what the formula calculates, what inputs it requires, and any boundary conditions or applicability constraints that govern its use. |
| formula_expression | String |  | A symbolic representation of the formula in a standardised notation, such as E = Q x EF x GWP, providing a human-auditable record of the mathematical relationship before it is encoded as structured components. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationFormula is a master-data entity that encodes the mathematical expression used by an EmissionCalculationModel to derive an emission quantity from input parameter and factor values. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 803 → 790 |
| Association |  | 777 → 790 |

---

*Generated: 2026-06-18 13:11:38*