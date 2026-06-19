# EmissionScopeType

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionScopeType is a reference entity that codifies the three GHG Protocol emission scopes used to classify where in a value chain an emission originates. Scope 1 covers direct emissions from sources owned or controlled by the organisation; Scope 2 covers indirect emissions from purchased energy; Scope 3 covers all other indirect emissions in the upstream and downstream value chain. This classification is mandatory for corporate GHG inventories and drives aggregation and boundary logic throughout the model.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for the EmissionScopeType record, typically a short code such as SCOPE1, SCOPE2, or SCOPE3 that is stable across standard revisions. |
| name | String |  | The human-readable label for the scope, such as "Scope 1 - Direct Emissions" or "Scope 3 - Other Indirect Emissions", used in report headings and UI labels. |
| description | String |  | A normative description of the emissions included in this scope as defined by the GHG Protocol, clarifying boundary rules and providing guidance on which activities fall within the scope. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionScopeType is a reference entity that codifies the three GHG Protocol emission scopes used to classify where in a value chain an emission originates. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 772 → 774 |
| Association |  | 772 → 776 |

---

*Generated: 2026-06-19 11:58:40*