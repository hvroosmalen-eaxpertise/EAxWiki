# ContactPerson

**Type:** Class  **Stereotype:** master-data  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Data Layer](../Data Layer/index.md) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.md) / [Organisation](index.md)

ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities. Contact persons may include sustainability officers, environmental compliance managers, data stewards, or external auditors. The entity captures sufficient name components to support formal correspondence, including title, first, middle, last, and full name, as well as a phone number for direct communication. A contact person is associated with an address for postal or physical contact purposes.

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

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| description | ContactPerson represents an individual who is designated as the primary or secondary contact for an organisation's emissions reporting activities. |  |

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |
| Association |  | [Organization](Organization.md) |

### Appears on Diagrams

- [Organisation](diagrams/Organisation.md)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |
| Association |  | [OrganizationPersonAssociation](OrganizationPersonAssociation.md) |

---

*Generated: 2026-06-25 10:51:16*