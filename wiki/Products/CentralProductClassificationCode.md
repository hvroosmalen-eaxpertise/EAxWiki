# CentralProductClassificationCode

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or supply-chain matching purposes. Linking a product to its CPC code supports automated mapping to applicable product category rules, sector-specific emission factors, and ESRS sector-classification requirements.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this CentralProductClassificationCode record, referenced by Product records and ProductCategoryRule records to establish the product classification. |
| classification_system | String |  | The name of the classification system from which this code is drawn, such as UN CPC v2.1, NACE Rev.2, or HS 2022, enabling correct code interpretation by receiving systems. |
| code | String |  | The alphanumeric code value within the referenced classification system, such as 27110 for Mining of hard coal under UN CPC, used for cross-system product matching and regulatory filing. |
| name | String |  | The human-readable label assigned to this code within its classification system, used in disclosures and product catalogues alongside the numeric code. |
| description | String |  | A description of the product scope covered by this classification code, supporting practitioners in assigning the correct code to products that span multiple categories. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | CentralProductClassificationCode is a reference entity that provides a classification code from the United Nations Central Product Classification (UN CPC) or another recognised product taxonomy used to categorise a product for regulatory, statistical, or  |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 810 → 815 |

---

*Generated: 2026-06-19 13:04:05*