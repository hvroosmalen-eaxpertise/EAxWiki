# EmissionSink

**Type:** Class  
**Stereotype:** reference-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionSink is a reference entity that classifies a physical destination into which greenhouse gases are absorbed or sequestered, such as forest biomass, soil carbon, or geological storage. Sinks are used to record carbon removal activities alongside emission activities in an inventory, enabling net-emission calculations and supporting reporting of land-use, land-use-change, and forestry (LULUCF) activities as required by ISO 14064-1 and voluntary carbon standards.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionSink record, used by EmissionActivity records that represent removal activities to classify the type of sink involved. |
| name | String |  | The human-readable name of the emission sink, such as Forest Carbon Sequestration or Biochar Soil Amendment, used in inventory category tables and removal disclosure sections. |
| description | String |  | A technical description of the removal mechanism, the permanence characteristics, and any monitoring or verification approach applicable to this sink type, supporting credible removal claims. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionSink is a reference entity that classifies a physical destination into which greenhouse gases are absorbed or sequestered, such as forest biomass, soil carbon, or geological storage. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 785 → 773 |

---

*Generated: 2026-06-18 12:23:55*