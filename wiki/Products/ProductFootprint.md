# ProductFootprint

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration. It holds the declaration metadata including the status, version history, validity period, and preceding footprint reference, grouping one or more ProductCarbonFootprint records that carry the quantified GHG data. The ProductFootprint entity aligns with the PACT technical specification version 2 data model, where a footprint declaration can be updated or superseded while retaining the historical chain of prior versions.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductFootprint envelope record, typically a UUID, used to reference the footprint in data exchange messages and to link the footprint to its superseding or preceding records. |
| product_id | String |  | Foreign key to the Product that this footprint declaration covers, linking the quantified carbon data to its product master record. |
| spec_version | String |  | The version of the PACT technical specification (or other applicable framework) under which this footprint was compiled, such as 2.0.0 or 2.1.0, enabling receivers to apply the correct validation rules. |
| version | String |  | A sequential integer counter indicating the revision number of this footprint declaration, incremented each time a correction or update is published for the same product and scope. |
| status | String |  | The lifecycle status of the footprint declaration, drawn from PACT values: Active, Deprecated, or an implementation-specific value, used to determine whether the declaration is current or superseded. |
| status_comment | String |  | A free-text explanation of the current status, particularly when the status is Deprecated or Inactive, describing the reason for the change and any corrective action taken. |
| validity_period_start | String |  | The first date for which this footprint declaration is valid and may be used by receiving parties, establishing when the declaration becomes effective. |
| validity_period_end | String |  | The last date for which this footprint declaration is valid; a null value indicates no defined expiry, while a populated date requires the receiver to obtain a refreshed declaration after this date. |
| preceding_product_footprint_id | String |  | A self-referential foreign key pointing to the ProductFootprint record that this declaration supersedes or corrects, enabling receivers to navigate the revision history and always identify the latest active version. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductFootprint is a work-product-component that serves as the versioned envelope record for a product carbon footprint declaration. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 811 → 811 |
| Association |  | 811 → 812 |
| Association |  | 810 → 811 |

---

*Generated: 2026-06-18 13:54:45*