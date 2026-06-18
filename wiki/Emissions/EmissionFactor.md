# EmissionFactor

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source. Factors are typed by the component category they represent (e.g. CO2 fossil, CH4), scoped by applicability (geography, activity type, technology, time period), and versioned to support year-over-year comparability. They form the primary input to activity-based calculation models.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionFactor record, referenced by EmissionActivityFactor associations to map an activity type to the factor applicable in a given context. |
| emission_factor_source_id | String |  | Foreign key to the EmissionFactorSource that published this factor, enabling traceability to the originating database or official publication. |
| emission_component_category_id | String |  | Foreign key to the EmissionComponentCategory that this factor applies to, such as CO2 fossil or CH4, determining which component category the factor quantity feeds. |
| activity_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure of the activity parameter denominator, such as MWh for an electricity emission factor, defining what one unit of activity produces. |
| factor_unit_of_measure_id | String |  | Foreign key to the UnitOfMeasure of the emission quantity numerator, typically kgCO2 or kgCH4, expressing the gas mass emitted per unit of activity. |
| value | String |  | The numeric emission factor coefficient: the quantity of greenhouse gas emitted per unit of activity. |
| geography | String |  | The geographic scope for which this factor is applicable, expressed as an ISO 3166-1 alpha-2 country code or a regional grouping such as EU27, used to select the correct factor for a given facility location. |
| valid_from | String |  | The start date of the period for which this factor is valid, used by calculation engines to select the factor appropriate to a specific reporting year. |
| valid_to | String |  | The end date of the factor applicability period. A null value indicates the factor remains current, while a populated date triggers selection of a more recent factor for periods after this date. |
| version | String |  | The version label of this factor within its source database, such as 2024 v1.0, supporting audit trails and reproducibility of historical calculations. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionFactor is a master-data entity that records a single quantified coefficient expressing the amount of greenhouse gas emitted per unit of an activity parameter, drawn from a recognised emission factor source. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 804 → 780 |
| Association |  | 803 → 780 |
| Association |  | 793 → 780 |
| Association |  | 779 → 780 |
| Association |  | 779 → 780 |
| Association |  | 792 → 780 |
| Association |  | 780 → 781 |

---

*Generated: 2026-06-18 13:54:45*