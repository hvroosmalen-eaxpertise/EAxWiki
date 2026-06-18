# RecordingUncertaintyAssessment

**Type:** Class  
**Stereotype:** work-product-component  


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Emissions](index.md)

RecordingUncertaintyAssessment is a work-product-component that captures the quantitative or qualitative uncertainty associated with a specific EmissionStatement, as required by ISO 14064-1 for first-party assurance. It records the uncertainty range, the assessment methodology, and the primary uncertainty drivers identified for that emission quantity. Systematic uncertainty documentation supports both the GHG Protocol accuracy principle and the assurance engagement process for third-party verification.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this RecordingUncertaintyAssessment record, used to link it to the parent EmissionStatement and to aggregate uncertainty across scope totals. |
| emission_statement_id | String |  | Foreign key to the EmissionStatement whose uncertainty this record assesses, establishing the one-to-one correspondence between an emission result and its uncertainty characterisation. |
| uncertainty_range_lower | String |  | The lower bound of the uncertainty range expressed as a percentage deviation from the central estimate, such as -15, indicating that the true value may be 15% lower than stated. |
| uncertainty_range_upper | String |  | The upper bound of the uncertainty range expressed as a percentage deviation from the central estimate, such as +20, indicating that the true value may be 20% higher than stated. |
| assessment_methodology | String |  | A description of the approach used to derive the uncertainty range, such as Monte Carlo simulation, IPCC Tier 1 uncertainty table, or expert elicitation, supporting methodological transparency. |
| primary_uncertainty_drivers | String |  | A free-text summary of the main sources of uncertainty identified for this emission statement, such as activity data estimation error or emission factor geographic applicability, guiding improvement prioritisation. |

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | RecordingUncertaintyAssessment is a work-product-component that captures the quantitative or qualitative uncertainty associated with a specific EmissionStatement, as required by ISO 14064-1 for first-party assurance. |  |

## Relationships

| Type | Stereotype | Source → Target |
|------|------------|-----------------|
| Association |  | 800 → 776 |

---

*Generated: 2026-06-18 12:23:55*