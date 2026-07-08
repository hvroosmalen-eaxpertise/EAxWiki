---
ea_id: 737
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: de6729a6
---

# <span class="sl" data-layer="uml">master-data</span> Address

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

Address captures the postal or physical address associated with a contact person, facility, or organisational unit. A structured address representation is preferred over a free-text field because it enables geographic analysis, regulatory jurisdiction mapping, and integration with postal validation services. The Address entity is deliberately kept simple, covering the most commonly required address components, and is linked to a Location entity that provides ISO country coding. A single contact or organisation may have multiple address records representing, for example, registered, trading, and correspondence addresses.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Address record. It serves as the primary key referenced by ContactPerson and other entities that require a postal or physical address. It must be unique across all address records in the system and must not be reused after deletion. |
| line1_text | String |  | The first line of the street address, typically containing the building number and street name, or a named building. This is the primary navigational element of the address and is required for postal delivery purposes. It is rendered as the first address line in formatted address displays and on correspondence. |
| line2_text | String |  | The second line of the address, used for suite numbers, floor designations, care-of information, or other supplementary addressing details that do not fit on the first line. This field is optional and should be left blank when not required rather than populated with placeholder text. |
| city | String |  | The city, town, or locality component of the address. It is used in geographic aggregations, jurisdiction mapping, and together with postal_code provides sufficient information to uniquely identify the delivery locality in most postal systems. |
| postal_code | String |  | The postal or ZIP code associated with the address. The format varies by country and is validated against country-specific rules where possible. It is used for geographic analysis, logistics routing, and regulatory jurisdiction determination. |
| geopolitical_entity_type_id | String |  | A foreign key referencing the GeopoliticalEntityType that classifies the geographic entity in which this address resides. This supports geographic analysis and regulatory jurisdiction mapping for emission reporting purposes. |
| location_id | String |  | A foreign key identifying the Country or Location entity associated with this address. This attribute links the address to the standardised geographic reference hierarchy and enables alignment with country-level emission factor datasets and regulatory jurisdiction rules. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Address captures the postal or physical address associated with a contact person, facility, or organisational unit. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [Country](Country.md) |
| Association |  | [OrganizationAddress](OrganizationAddress.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Country](Country.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{&quot;nodes&quot;:[{&quot;id&quot;:&quot;e745&quot;,&quot;label&quot;:&quot;Country&quot;,&quot;fullName&quot;:&quot;Country&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Country.html&quot;},{&quot;id&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;OrganizationAddress&quot;,&quot;fullName&quot;:&quot;OrganizationAddress&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAddress.html&quot;},{&quot;id&quot;:&quot;e737&quot;,&quot;label&quot;:&quot;Address&quot;,&quot;fullName&quot;:&quot;Address&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:true,&quot;hasUrl&quot;:false,&quot;url&quot;:&quot;&quot;},{&quot;id&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;GeopoliticalEntity&quot;,&quot;fullName&quot;:&quot;GeopoliticalEntity&quot;,&quot;packageName&quot;:&quot;Facilities&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;../Facilities/GeopoliticalEntity.html&quot;},{&quot;id&quot;:&quot;e744&quot;,&quot;label&quot;:&quot;OrganizationAddressType&quot;,&quot;fullName&quot;:&quot;OrganizationAddressType&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;OrganizationAddressType.html&quot;},{&quot;id&quot;:&quot;e735&quot;,&quot;label&quot;:&quot;Organization&quot;,&quot;fullName&quot;:&quot;Organization&quot;,&quot;packageName&quot;:&quot;Organisation&quot;,&quot;layer&quot;:&quot;uml&quot;,&quot;isFocal&quot;:false,&quot;hasUrl&quot;:true,&quot;url&quot;:&quot;Organization.html&quot;}],&quot;edges&quot;:[{&quot;id&quot;:&quot;c893&quot;,&quot;source&quot;:&quot;e745&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c909&quot;,&quot;source&quot;:&quot;e745&quot;,&quot;target&quot;:&quot;e737&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c910&quot;,&quot;source&quot;:&quot;e744&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c911&quot;,&quot;source&quot;:&quot;e737&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c912&quot;,&quot;source&quot;:&quot;e735&quot;,&quot;target&quot;:&quot;e743&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;},{&quot;id&quot;:&quot;c884&quot;,&quot;source&quot;:&quot;e766&quot;,&quot;target&quot;:&quot;e766&quot;,&quot;label&quot;:&quot;Association&quot;,&quot;sourceLayer&quot;:&quot;uml&quot;}]}</div>

---

*Generated: 2026-07-08 15:12:49*