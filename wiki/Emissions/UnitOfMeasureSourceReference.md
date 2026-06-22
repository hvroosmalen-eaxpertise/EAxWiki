# UnitOfMeasureSourceReference

**Type:** Class  **Stereotype:** reference-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

UnitOfMeasureSourceReference is a reference entity that identifies the authoritative registry or specification from which a UnitOfMeasure definition is drawn. Examples include the UN/CEFACT Common Codes for Units of Measurement, the QUDT Units Ontology, and the NIST SP 811 guide to SI units. Tracking the source reference ensures that unit definitions used in the model can be validated against a canonical authority and that imported data using the same reference can be reconciled without ambiguity.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this UnitOfMeasureSourceReference record, referenced by UnitOfMeasure records via unit_of_measure_source_reference_id to indicate the authority for each unit definition. |
| name | String |  | The standard name or acronym of the reference authority, such as UN/CEFACT Recommendation 20 or QUDT Units Ontology 2.1, used in citations and unit catalogue metadata. |
| url | String |  | A persistent URL or DOI to the authoritative source document or registry, enabling automated validation against the latest version of the reference. |
| description | String |  | A description of the scope and coverage of this reference, including which physical quantities it covers, the update cadence, and how it is used within the Open Footprint Data Model. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | UnitOfMeasureSourceReference is a reference entity that identifies the authoritative registry or specification from which a UnitOfMeasure definition is drawn. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [UnitOfMeasure](UnitOfMeasure.md) |

---

*Generated: 2026-06-22 17:27:40*