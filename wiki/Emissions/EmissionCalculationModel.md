# EmissionCalculationModel

**Type:** Class  
**Stereotype:** master-data  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity. A model links a set of EmissionCalculationFormulas and specifies the method type (spend-based, activity-based, supplier-specific, and so on) and the applicable standard. Models may be versioned and associated with specific jurisdictions or industry sectors, allowing a calculation engine to select the most appropriate model for a given emission activity and reporting context.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this EmissionCalculationModel record, referenced by EmissionStatement and EmissionActivityFactor records to trace which method produced a given result. |
| emission_calculation_method_type_id | String |  | Foreign key to the EmissionCalculationMethodType that classifies the calculation approach, such as activity-based, spend-based, or supplier-specific, supporting methodology disclosure. |
| standard_id | String |  | Foreign key to the Standard whose guidance governs this calculation model, ensuring that the method is clearly traceable to its normative source. |
| name | String |  | A descriptive label for the calculation model, such as DEFRA 2024 Electricity Consumption UK Grid or GHG Protocol Mobile Combustion Diesel, used for model selection and labelling in reports. |
| description | String |  | A narrative description of the model scope, assumptions, applicable activity types, and any known limitations, supporting informed methodology selection by practitioners. |
| version | String |  | The version identifier of this model definition, allowing calculations to be re-executed with a historically consistent method and supporting year-over-year comparability. |
| valid_from | String |  | The date from which this model version is applicable, used by calculation engines to select the correct model version for a given reporting period. |
| valid_to | String |  | The date after which this model version is superseded, ensuring that outdated methods are not inadvertently applied to new reporting periods. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | EmissionCalculationModel is a master-data entity that defines the methodological approach used to convert activity data into an emission quantity. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 805 → 777 |
| Association |  | 804 → 777 |
| Association |  | 793 → 777 |
| Association |  | 791 → 777 |
| Association |  | 777 → 790 |
| Association |  | 777 → 776 |
| Association |  | 734 → 777 |

---

*Generated: 2026-06-18 12:23:55*