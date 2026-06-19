# Product

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared. It acts as the anchor for all product-level sustainability data, linking the product to its classification codes (CentralProductClassificationCode), its product carbon footprint records (ProductFootprint), and the life-cycle stages used to structure its footprint assessment. In a supply-chain context, Product enables data exchange between buyer and supplier organisations under the WBCSD PACT framework.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this Product record, referenced by ProductFootprint, EmissionActivityFlow, and classification code association records. |
| name | String |  | The human-readable commercial or technical name of the product, used in disclosures, data exchanges, and product catalogue displays. |
| description | String |  | A free-text description of the product, including its functional unit, primary materials, and typical use context, providing the background needed to interpret its carbon footprint data. |
| product_id_type | String |  | The type of product identifier used in the product_id field, such as UUID, EAN, GTIN-13, or internal SKU code, enabling correct interpretation of the identifier by receiving systems. |
| product_id | String |  | The specific identifier for this product in the system indicated by product_id_type, such as a GTIN barcode or internal stock-keeping unit number, supporting cross-system product matching. |
| unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure that defines the declared unit (functional unit) for this product, such as 1 kg, 1 piece, or 1 kWh, establishing the denominator for all carbon intensity expressions. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Product is a master-data entity that represents a physical good or service whose carbon footprint can be measured and declared. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 810 → 819 |
| Association |  | 810 → 815 |
| Association |  | 810 → 813 |
| Association |  | 810 → 811 |

---

*Generated: 2026-06-19 11:58:39*