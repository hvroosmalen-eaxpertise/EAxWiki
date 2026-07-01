---
ea_id: 738
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 2476c091
---

# <span class="sl" data-layer="uml">master-data</span> ContactPerson

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  **Status:** <span class="status-badge status-not-set">Not Set</span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="738" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/ContactPerson.md" data-api-port="8001"></div>

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="738" data-file-path="Organisation/ContactPerson.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities. Contact persons may include sustainability officers, environmental compliance managers, data stewards, or external auditors. The entity captures sufficient name components to support formal correspondence, including title, first, middle, last, and full name, as well as a phone number for direct communication. A contact person is associated with an address for postal or physical contact purposes.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the ContactPerson record. It serves as the primary key and is referenced when linking a contact person to an organisation or address. It must remain stable across name or contact detail changes to preserve the integrity of historical associations. |
| title_name | String |  | The professional or social title of the contact person, such as "Dr.", "Prof.", "Mr.", or "Ms.". The title is used in formal written correspondence and official reports, and should be drawn from a controlled vocabulary where possible to ensure consistent rendering across communication channels. |
| first_name | String |  | The given name or first name of the contact person as they prefer to be addressed in professional communications. It is used in conjunction with middle_name and last_name to construct the full name and to support personalised communication in notifications and correspondence. |
| middle_name | String |  | The middle name or initial of the contact person, where applicable. This field is optional and is included primarily to support disambiguation between persons with identical first and last names, and to meet formal identification requirements in certain jurisdictions. |
| last_name | String |  | The family name or surname of the contact person. Together with first_name and middle_name, it forms the complete personal name used in official correspondence, audit trails, and disclosure documents. |
| full_name | String |  | The complete rendered name of the contact person as it should appear in reports and communications, which may include title, all name components, and any post-nominal qualifications. This field may be derived from the component name fields or separately maintained where the standard concatenation does not produce the correct rendering. |
| phone_number | String |  | The telephone number at which the contact person can be reached for queries related to emissions reporting. The number should be stored in international format (E.164) to support cross-border communication and automated dialling integrations. |
| address_id | String |  | A foreign key identifying the Address record that represents this contact person's physical or postal contact location. This attribute enables automated generation of formal correspondence addressed to the contact person at their registered address. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |
| Association |  | [Organization](Organization.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Organisation.html" class="diagram-thumb"><img src="diagrams/Organisation.png" alt="Organisation" loading="lazy"><span>Organisation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e746","label":"PersonOrganizationRoleT…","fullName":"PersonOrganizationRoleType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"PersonOrganizationRoleType.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifier.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c905","source":"e747","target":"e738","label":"Association","sourceLayer":"uml"},{"id":"c906","source":"e747","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c907","source":"e746","target":"e747","label":"Association","sourceLayer":"uml"},{"id":"c824","source":"e801","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c898","source":"e740","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c899","source":"e735","target":"e751","label":"Association","sourceLayer":"uml"},{"id":"c901","source":"e735","target":"e750","label":"Association","sourceLayer":"uml"},{"id":"c904","source":"e735","target":"e748","label":"Association","sourceLayer":"uml"},{"id":"c908","source":"e738","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c912","source":"e735","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c914","source":"e735","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c915","source":"e735","target":"e736","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 10:25:44*