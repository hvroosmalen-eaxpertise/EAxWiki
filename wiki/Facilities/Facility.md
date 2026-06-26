# <span class="sl" data-layer="uml">master-data</span> Facility

**Type:** Class  **Stereotype:** master-data  **StereotypeEx:** master-data  **FQStereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.md) / [Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Facilities](index.md)

Facility represents the capability of an organisation to perform a particular business function or service. It is a concept used to describe the functional capability that may arise from the installation of equipment and materials provided by a collection of assets and different locations. A facility can represent both a general aggregate capability of the organisation or a specific asset that is built, installed, or established to serve a particular purpose, such as a Plant, Research Laboratory, Office, or Offshore Platform. Facilities are classified by FacilityType and are assigned to locations through FacilityLocationAssociation, enabling accurate geographic attribution of site-level emission data.

## Attributes

| Name | Type | Default | Description |
|------|------|---------|-------------|
| id | Key |  | The unique system-assigned identifier for the Facility record. It is the primary key referenced by EmissionInventory to associate an inventory with the facility at which the emissions were measured. It must be globally unique and must not be reused after a facility is decommissioned or retired from the system. |
| name | String |  | The operational or registered name of the facility, such as "Plant A", "Unit AA", or "Offshore Platform North Sea Block 42". The name is used in reports, maps, and dashboards to identify the physical site and should correspond to the name used in regulatory submissions and site permits. |
| type_id | String |  | A foreign key referencing the FacilityType that classifies this facility. The type determines the applicable emission factor sets, regulatory reporting categories, and benchmarking peer group for this facility. |

[↑ Back to top](#)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | Facility represents the capability of an organisation to perform a particular business function or service. |  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [FacilityEmissionAllocation](FacilityEmissionAllocation.md) |
| Association |  | [EmissionActivityParameter](EmissionActivityParameter.md) |
| Association |  | [EquipmentInstallation](EquipmentInstallation.md) |
| Association |  | [FacilityActivityParticipation](FacilityActivityParticipation.md) |
| Association |  | [FacilitySpecification](FacilitySpecification.md) |
| Association |  | [FacilityLocationAssociation](FacilityLocationAssociation.md) |
| Association |  | [FacilityStructure](FacilityStructure.md) |
| Association |  | [FacilityType](FacilityType.md) |
| Association |  | [Organization](../Organisation/Organization.md) |

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [Organization](../Organisation/Organization.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e764","label":"FacilityEmissionAllocat…","fullName":"FacilityEmissionAllocation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityEmissionAllocation.html"},{"id":"e762","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameter","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EmissionActivityParameter.html"},{"id":"e761","label":"EquipmentInstallation","fullName":"EquipmentInstallation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EquipmentInstallation.html"},{"id":"e760","label":"FacilityActivityPartici…","fullName":"FacilityActivityParticipation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityActivityParticipation.html"},{"id":"e759","label":"FacilitySpecification","fullName":"FacilitySpecification","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilitySpecification.html"},{"id":"e758","label":"FacilityLocationAssocia…","fullName":"FacilityLocationAssociation","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationAssociation.html"},{"id":"e757","label":"FacilityStructure","fullName":"FacilityStructure","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityStructure.html"},{"id":"e754","label":"FacilityType","fullName":"FacilityType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityType.html"},{"id":"e735","label":"Organization","fullName":"Organization","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/Organization.html"},{"id":"e753","label":"Facility","fullName":"Facility","packageName":Facilities,"isFocal":true,"hasUrl":false,"url":""},{"id":"e776","label":"EmissionStatement","fullName":"EmissionStatement","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionStatement.html"},{"id":"e775","label":"EmissionParameterType","fullName":"EmissionParameterType","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionParameterType.html"},{"id":"e763","label":"EmissionActivityParamet…","fullName":"EmissionActivityParameterValue","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"EmissionActivityParameterValue.html"},{"id":"e756","label":"Equipment","fullName":"Equipment","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Equipment.html"},{"id":"e773","label":"EmissionActivity","fullName":"EmissionActivity","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionActivity.html"},{"id":"e769","label":"FacilityLocationType","fullName":"FacilityLocationType","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"FacilityLocationType.html"},{"id":"e755","label":"Location","fullName":"Location","packageName":Facilities,"isFocal":false,"hasUrl":true,"url":"Location.html"},{"id":"e801","label":"OrganizationEmissionAll…","fullName":"OrganizationEmissionAllocation","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/OrganizationEmissionAllocation.html"},{"id":"e782","label":"EmissionReport","fullName":"EmissionReport","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionReport.html"},{"id":"e740","label":"OrganizationalBoundary","fullName":"OrganizationalBoundary","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationalBoundary.html"},{"id":"e751","label":"OrganizationEquityShare","fullName":"OrganizationEquityShare","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationEquityShare.html"},{"id":"e750","label":"OrganizationIndustrySec…","fullName":"OrganizationIndustrySector","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationIndustrySector.html"},{"id":"e748","label":"OrganizationExternalIde…","fullName":"OrganizationExternalIdentifier","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationExternalIdentifier.html"},{"id":"e747","label":"OrganizationPersonAssoc…","fullName":"OrganizationPersonAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationPersonAssociation.html"},{"id":"e738","label":"ContactPerson","fullName":"ContactPerson","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/ContactPerson.html"},{"id":"e743","label":"OrganizationAddress","fullName":"OrganizationAddress","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAddress.html"},{"id":"e741","label":"OrganizationAssociation","fullName":"OrganizationAssociation","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationAssociation.html"},{"id":"e736","label":"OrganizationType","fullName":"OrganizationType","packageName":Organisation,"isFocal":false,"hasUrl":true,"url":"../Organisation/OrganizationType.html"},{"id":"e771","label":"EmissionInventory","fullName":"EmissionInventory","packageName":Emissions,"isFocal":false,"hasUrl":true,"url":"../Emissions/EmissionInventory.html"}],"edges":[{"id":"c840","source":"e776","target":"e764","label":"Association"},{"id":"c873","source":"e753","target":"e764","label":"Association"},{"id":"c841","source":"e775","target":"e762","label":"Association"},{"id":"c874","source":"e762","target":"e763","label":"Association"},{"id":"c875","source":"e753","target":"e762","label":"Association"},{"id":"c877","source":"e753","target":"e761","label":"Association"},{"id":"c878","source":"e756","target":"e761","label":"Association"},{"id":"c842","source":"e773","target":"e760","label":"Association"},{"id":"c879","source":"e753","target":"e760","label":"Association"},{"id":"c880","source":"e753","target":"e759","label":"Association"},{"id":"c872","source":"e769","target":"e758","label":"Association"},{"id":"c888","source":"e755","target":"e758","label":"Association"},{"id":"c889","source":"e753","target":"e758","label":"Association"},{"id":"c890","source":"e757","target":"e757","label":"Association"},{"id":"c891","source":"e753","target":"e757","label":"Association"},{"id":"c892","source":"e753","target":"e754","label":"Association"},{"id":"c824","source":"e801","target":"e735","label":"Association"},{"id":"c895","source":"e735","target":"e782","label":"Association"},{"id":"c896","source":"e735","target":"e753","label":"Association"},{"id":"c898","source":"e740","target":"e735","label":"Association"},{"id":"c899","source":"e735","target":"e751","label":"Association"},{"id":"c901","source":"e735","target":"e750","label":"Association"},{"id":"c904","source":"e735","target":"e748","label":"Association"},{"id":"c906","source":"e747","target":"e735","label":"Association"},{"id":"c908","source":"e738","target":"e735","label":"Association"},{"id":"c912","source":"e735","target":"e743","label":"Association"},{"id":"c914","source":"e735","target":"e741","label":"Association"},{"id":"c915","source":"e735","target":"e736","label":"Association"},{"id":"c917","source":"e735","target":"e771","label":"Association"},{"id":"c825","source":"e801","target":"e776","label":"Association"},{"id":"c870","source":"e773","target":"e776","label":"Association"},{"id":"c871","source":"e771","target":"e776","label":"Association"},{"id":"c876","source":"e756","target":"e756","label":"Association"},{"id":"c863","source":"e773","target":"e773","label":"Association"},{"id":"c887","source":"e769","target":"e755","label":"Association"},{"id":"c881","source":"e755","target":"e755","label":"Association"},{"id":"c808","source":"e782","target":"e771","label":"Association"},{"id":"c905","source":"e747","target":"e738","label":"Association"}]};</script>

---

*Generated: 2026-06-26 13:44:31*