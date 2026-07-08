---
ea_id: 744
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 207b694b
---

# <span class="sl" data-layer="uml">reference-data</span> OrganizationAddressType

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

OrganizationAddressType provides the controlled vocabulary of address categories applicable to an organisation, such as "Visiting Address", "Goods Return Address", "Mail Address", "Delivery Address", or "Statutory Address". Classifying address types ensures that consuming systems can select the appropriate address for a given operational purpose without manual disambiguation. The code attribute supports alignment with external postal classification systems used by logistics providers.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationAddressType record. It must be stable so that OrganizationAddress intersection records that reference it remain valid when the type vocabulary is extended. |
| code | String |  | A short alphanumeric code for the address type, such as "VISIT" or "STAT", used in data exchange messages and API responses where the full name would be verbose. The code should be drawn from a standardised address type taxonomy where one exists for the relevant industry or jurisdiction. |
| name | String |  | The descriptive label for the address type, such as "Visiting Address" or "Statutory Address". The name must unambiguously identify the intended use of the address and should be consistent with terminology used in the organisation's legal jurisdiction for formal correspondence and regulatory filings. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationAddressType provides the controlled vocabulary of address categories applicable to an organisation, such as "Visiting Address", "Goods Return Address", "Mail Address", "Delivery Address", or "Statutory Address". |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationAddress](OrganizationAddress.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;OrganizationAddress&quot;,&quot;fullName&quot;:&quot;OrganizationAddress&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAddress.html&quot;},{&quot;id&quot;:&quot;e744&quot;,&quot;label&quot;:&quot;OrganizationAddressType&quot;,&quot;fullName&quot;:&quot;OrganizationAddressType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e737&quot;,&quot;label&quot;:&quot;Address&quot;,&quot;fullName&quot;:&quot;Address&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Address.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c910&quot;,&quot;source&quot;:&quot;e744&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c911&quot;,&quot;source&quot;:&quot;e737&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c912&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*