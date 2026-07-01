---
ea_id: 748
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: 41d72825
---

# <span class="sl" data-layer="uml">master-data</span> OrganizationExternalIdentifier

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="748" data-status="" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="748" data-file-path="Organisation/OrganizationExternalIdentifier.md" data-api-port="8001">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number. An organisation may carry multiple external identifiers from different registries simultaneously, each classified by an OrganizationExternalIdentifierType. This supports cross-system reconciliation and supply chain data exchange, particularly in contexts where trading partners reference the organisation by different external codes.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the OrganizationExternalIdentifier record. It is the primary key and must remain stable across changes to the identifier value or type to preserve historical traceability. |
| value | String |  | The actual code or identifier string assigned by the external registry, such as "724500XWKLYJMHYXQ242" (LEI) or "12-3456789" (EIN). The format of this value depends entirely on the referenced OrganizationExternalIdentifierType and should be stored as received from the issuing registry without normalisation. |
| organization_id | String |  | A foreign key identifying the Organisation to which this external identifier has been assigned. One organisation may have many OrganizationExternalIdentifier records, one per external registry that has issued a code for it. |
| organization_external_identifier_type_id | String |  | A foreign key referencing the OrganizationExternalIdentifierType that classifies the scheme under which this identifier was issued, such as "DUNS" or "Employer Identification Number (EIN)". This attribute enables consuming systems to select the correct identifier for a specific registry. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | OrganizationExternalIdentifier records a specific external code assigned to an organisation by an external registry or authority, such as a tax registration number, DUNS number, or Chamber of Commerce registration number. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationExternalIdentifierType](OrganizationExternalIdentifierType.md) |
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
| Association |  | [Organization](Organization.md) |
| Association |  | [OrganizationExternalIdentifierType](OrganizationExternalIdentifierType.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e749","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifierType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationExternalIdentifierType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"Organization.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":"Organisation","layer":"uml","isFocal":true,"hasUrl":false,"url":""},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":"Facilities","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Facilities/Facility.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationIndustrySector.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":"Organisation","layer":"uml","isFocal":false,"hasUrl":true,"url":"OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":"Emissions","layer":"uml","isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c903","source":"e749","target":"e748","label":"Association","sourceLayer":"uml"},{"id":"c824","source":"e801","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c895","source":"e735","target":"e782","label":"Association","sourceLayer":"uml"},{"id":"c896","source":"e735","target":"e753","label":"Association","sourceLayer":"uml"},{"id":"c898","source":"e740","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c899","source":"e735","target":"e751","label":"Association","sourceLayer":"uml"},{"id":"c901","source":"e735","target":"e750","label":"Association","sourceLayer":"uml"},{"id":"c904","source":"e735","target":"e748","label":"Association","sourceLayer":"uml"},{"id":"c906","source":"e747","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c908","source":"e738","target":"e735","label":"Association","sourceLayer":"uml"},{"id":"c912","source":"e735","target":"e743","label":"Association","sourceLayer":"uml"},{"id":"c914","source":"e735","target":"e741","label":"Association","sourceLayer":"uml"},{"id":"c915","source":"e735","target":"e736","label":"Association","sourceLayer":"uml"},{"id":"c917","source":"e735","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c808","source":"e782","target":"e771","label":"Association","sourceLayer":"uml"},{"id":"c905","source":"e747","target":"e738","label":"Association","sourceLayer":"uml"}]}</div>

---

*Generated: 2026-07-01 11:29:54*