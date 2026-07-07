---
ea_id: 739
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 96ac0e12
---

# <span class="sl" data-layer="uml">reference-data</span> IndustrySectorType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection. Common sector classification systems include NACE (European), SIC (US), NAICS (North American), and ISIC (International). A self-referential parent relationship allows the construction of multi-level sector hierarchies, enabling both high-level sector summaries and granular sub-sector analysis. The code attribute supports direct alignment with external classification system codes, enabling automated crosswalk with reference datasets.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the IndustrySectorType record. It serves as the primary key and is also referenced in the self-referential parent association to build sector hierarchies. It must be globally unique across all sector classification records. |
| code | String |  | The standardised industry sector classification code as defined by the relevant classification system (e.g., NAICS code "325" for "Chemical Manufacturing" or "32511" for "Petrochemical Manufacturing"). The code enables automated alignment with external databases, emission factor repositories, and regulatory reporting schemas that reference industry sectors by code. |
| name | String |  | The human-readable name of the industry sector, such as "Oil and Gas" or "Iron and Steel". The name provides a user-friendly label for display in reports and selection interfaces and should match the official label used by the classification system referenced by the code attribute. |
| parent_id | String |  | A foreign key referencing the parent IndustrySectorType record in the sector hierarchy, enabling multi-level classifications such as Chemical Manufacturing (325) as the parent of Petrochemical Manufacturing (32511). This attribute must not create cycles; implementations should enforce acyclicity. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | IndustrySectorType provides a hierarchical classification of industry sectors used to categorise organisations for benchmarking, regulatory grouping, and sector-specific emissions factor selection. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [IndustrySectorType](IndustrySectorType.md) |
| Association |  | [OrganizationIndustrySector](OrganizationIndustrySector.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [IndustrySectorType](IndustrySectorType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;OrganizationIndustrySec…&quot;,&quot;fullName&quot;:&quot;OrganizationIndustrySector&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationIndustrySector.html&quot;},{&quot;id&quot;:&quot;e739&quot;,&quot;label&quot;:&quot;IndustrySectorType&quot;,&quot;fullName&quot;:&quot;IndustrySectorType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c901&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c902&quot;,&quot;source&quot;:&quot;e739&quot;,&quot;target&quot;:&quot;e750&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c900&quot;,&quot;source&quot;:&quot;e739&quot;,&quot;target&quot;:&quot;e739&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-07 13:30:30*