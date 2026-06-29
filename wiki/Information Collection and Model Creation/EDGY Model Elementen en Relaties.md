# <span class="sl" data-layer="edgy-lb">Object</span> EDGY Model Elementen en Relaties

**Type:** Class  **Stereotype:** Object  **StereotypeEx:** Object  **FQStereotype:** EDGY::Object  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2026-05-13  **Modified:** 2026-05-13


[Home](../index.md) / [Model Creation](../Model Creation/index.md) / [Information Collection and Model Creation](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Center | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [EDGY -> XMI (Python)](EDGY -_ XMI (Python).md) |
| ControlFlow | Flow | [EurSuRA Model](EurSuRA Model.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Information Collection and Model Creation.html" class="diagram-thumb diagram-thumb--noimg"><span>Information Collection and Model Creation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [EDGY -> XMI (Python)](EDGY -_ XMI (Python).md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e577","label":"EDGY -&gt; XMI (Python)","fullName":"EDGY -&gt; XMI (Python)","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"EDGY -_ XMI (Python).html"},{"id":"e576","label":"EurSuRA Model","fullName":"EurSuRA Model","packageName":"Information Collection and Model Creation","layer":"edgy-ix","isFocal":false,"hasUrl":true,"url":"EurSuRA Model.html"},{"id":"e595","label":"EDGY Model Elementen en…","fullName":"EDGY Model Elementen en Relaties","packageName":"Information Collection and Model Creation","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e585","label":"Compile Information Sou…","fullName":"Compile Information Sources","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"Compile Information Sources.html"},{"id":"e589","label":"Constureren MD to XMI C…","fullName":"Constureren MD to XMI Convertor","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"Constureren MD to XMI Convertor.html"},{"id":"e579","label":"SDG Referentie Model","fullName":"SDG Referentie Model","packageName":"Information Collection and Model Creation","layer":"edgy-ix","isFocal":false,"hasUrl":true,"url":"SDG Referentie Model.html"},{"id":"e580","label":"ESRS Referentie Model","fullName":"ESRS Referentie Model","packageName":"Information Collection and Model Creation","layer":"edgy-ix","isFocal":false,"hasUrl":true,"url":"ESRS Referentie Model.html"},{"id":"e587","label":"Stakeholder Map","fullName":"Stakeholder Map","packageName":"Information Collection and Model Creation","layer":"edgy-ix","isFocal":false,"hasUrl":true,"url":"Stakeholder Map.html"},{"id":"e591","label":"Identity Map","fullName":"Identity Map","packageName":"Information Collection and Model Creation","layer":"edgy-ix","isFocal":false,"hasUrl":true,"url":"Identity Map.html"},{"id":"e629","label":"Logical Data Model","fullName":"Logical Data Model","packageName":"Information Collection and Model Creation","layer":"edgy-ix","isFocal":false,"hasUrl":true,"url":"Logical Data Model.html"},{"id":"e628","label":"ERD Model","fullName":"ERD Model","packageName":"Information Collection and Model Creation","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"ERD Model.html"}],"edges":[{"id":"c614","source":"e577","target":"e595","label":"XMI Bestand","sourceLayer":"edgy-ar"},{"id":"c620","source":"e585","target":"e577","label":"MD Bestand","sourceLayer":"edgy-ar"},{"id":"c623","source":"e589","target":"e577","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c616","source":"e576","target":"e579","label":"Aggregation","sourceLayer":"edgy-ix"},{"id":"c617","source":"e576","target":"e580","label":"Aggregation","sourceLayer":"edgy-ix"},{"id":"c622","source":"e576","target":"e587","label":"Aggregation","sourceLayer":"edgy-ix"},{"id":"c625","source":"e576","target":"e591","label":"Aggregation","sourceLayer":"edgy-ix"},{"id":"c630","source":"e595","target":"e576","label":"ControlFlow","sourceLayer":"edgy-lb"},{"id":"c656","source":"e576","target":"e629","label":"Aggregation","sourceLayer":"edgy-ix"},{"id":"c658","source":"e628","target":"e576","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-06-29 18:57:22*