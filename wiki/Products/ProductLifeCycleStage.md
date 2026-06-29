# <span class="sl" data-layer="uml">master-data</span> ProductLifeCycleStage

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Products](index.md)

ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life. Stages are defined at the vocabulary level as reference data so that all products in the model share a consistent set of stage labels, enabling stage-level emission comparisons across products, industries, and reporting frameworks including ISO 14040/14044 and ESRS E1.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique identifier for this ProductLifeCycleStage record, referenced by ProductLifeCycleFootprint records to attribute stage-level carbon contributions within a product life-cycle boundary. |
| name | String |  | The standard label for the life-cycle stage, such as Cradle (raw material extraction), Gate (end of manufacturing), Use, or End-of-Life, used in footprint breakdowns and disclosure tables. |
| description | String |  | A description of the processes, activities, and geographic scope typically included in this stage, providing guidance for consistent stage boundary assignment across different products and assessment practitioners. |
| stage_order | String |  | The default sequential order of this stage within a cradle-to-grave life cycle, used to present stage-level footprint data in a logical, temporally consistent sequence in reports and dashboards. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ProductLifeCycleStage is a reference entity that enumerates the individual stages in a product life cycle, such as Raw Material Extraction, Manufacturing, Distribution and Retail, Use, and End-of-Life. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [ProductLifeCycleFootprint](ProductLifeCycleFootprint.md) |
| Association |  | [ProductLifeCycleFootprint](ProductLifeCycleFootprint.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e817","label":"ProductLifeCycleFootpri…","fullName":"ProductLifeCycleFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductLifeCycleFootprint.html"},{"id":"e816","label":"ProductLifeCycleStage","fullName":"ProductLifeCycleStage","packageName":"Products","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e812","label":"ProductCarbonFootprint","fullName":"ProductCarbonFootprint","packageName":"Products","layer":"uml","isFocal":false,"hasUrl":true,"url":"ProductCarbonFootprint.html"}],"edges":[{"id":"c793","source":"e816","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c802","source":"e816","target":"e817","label":"Association","sourceLayer":"uml"},{"id":"c803","source":"e812","target":"e817","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-06-29 13:30:17*