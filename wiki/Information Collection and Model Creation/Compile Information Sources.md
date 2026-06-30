# <span class="sl" data-layer="edgy-ar">Process</span> Compile Information Sources

**Type:** Interface  **Stereotype:** Process  **StereotypeEx:** Process  **FQStereotype:** EDGY::Process  **Status:** <span class="status-badge status-proposed">Proposed</span>  
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
| ControlFlow | Flow | [SDG](SDG.md) |
| ControlFlow | Flow | [ESRS](ESRS.md) |
| ControlFlow | Flow | [EDGY 23 Language Foundation](EDGY 23 Language Foundation.md) |
| ControlFlow | Flow | [EDGY -> XMI (Python)](EDGY -_ XMI (Python).md) |
| Association | Link | [AI - LLM](AI - LLM.md) |
| ControlFlow | Flow | [Openfootprint Data Model](Openfootprint Data Model.md) |
| ControlFlow | Flow | [LDM -> XMI (Python)](LDM -_ XMI (Python).md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Information Collection and Model Creation.html" class="diagram-thumb"><img src="diagrams/Information Collection and Model Creation.png" alt="Information Collection and Model Creation" loading="lazy"><span>Information Collection and Model Creation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | Link | [AI - LLM](AI - LLM.md) |
| ControlFlow | Flow | [EDGY 23 Language Foundation](EDGY 23 Language Foundation.md) |
| ControlFlow | Flow | [ESRS](ESRS.md) |
| ControlFlow | Flow | [Openfootprint Data Model](Openfootprint Data Model.md) |
| ControlFlow | Flow | [SDG](SDG.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e574","label":"SDG","fullName":"SDG","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"SDG.html"},{"id":"e578","label":"ESRS","fullName":"ESRS","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"ESRS.html"},{"id":"e582","label":"EDGY 23 Language Founda…","fullName":"EDGY 23 Language Foundation","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"EDGY 23 Language Foundation.html"},{"id":"e577","label":"EDGY -&gt; XMI (Python)","fullName":"EDGY -&gt; XMI (Python)","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"EDGY -_ XMI (Python).html"},{"id":"e594","label":"AI - LLM","fullName":"AI - LLM","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"AI - LLM.html"},{"id":"e625","label":"Openfootprint Data Model","fullName":"Openfootprint Data Model","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"Openfootprint Data Model.html"},{"id":"e627","label":"LDM -&gt; XMI (Python)","fullName":"LDM -&gt; XMI (Python)","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"LDM -_ XMI (Python).html"},{"id":"e585","label":"Compile Information Sou…","fullName":"Compile Information Sources","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":true,"hasUrl":false,"url":""},{"id":"e575","label":"SDG Website","fullName":"SDG Website","packageName":"Information Collection and Model Creation","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"SDG Website.html"},{"id":"e590","label":"ESRS Website","fullName":"ESRS Website","packageName":"Information Collection and Model Creation","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"ESRS Website.html"},{"id":"e592","label":"EFRAG Website","fullName":"EFRAG Website","packageName":"Information Collection and Model Creation","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"EFRAG Website.html"},{"id":"e581","label":"Intersection Group","fullName":"Intersection Group","packageName":"Information Collection and Model Creation","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"Intersection Group.html"},{"id":"e595","label":"EDGY Model Elementen en…","fullName":"EDGY Model Elementen en Relaties","packageName":"Information Collection and Model Creation","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"EDGY Model Elementen en Relaties.html"},{"id":"e589","label":"Constureren MD to XMI C…","fullName":"Constureren MD to XMI Convertor","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"Constureren MD to XMI Convertor.html"},{"id":"e626","label":"The Open Group Website","fullName":"The Open Group Website","packageName":"Information Collection and Model Creation","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"The Open Group Website.html"},{"id":"e628","label":"ERD Model","fullName":"ERD Model","packageName":"Information Collection and Model Creation","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"ERD Model.html"}],"edges":[{"id":"c612","source":"e575","target":"e574","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c613","source":"e574","target":"e585","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c615","source":"e578","target":"e585","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c624","source":"e590","target":"e578","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c626","source":"e592","target":"e578","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c618","source":"e581","target":"e582","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c619","source":"e582","target":"e585","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c614","source":"e577","target":"e595","label":"XMI Bestand","sourceLayer":"edgy-ar"},{"id":"c620","source":"e585","target":"e577","label":"MD Bestand","sourceLayer":"edgy-ar"},{"id":"c623","source":"e589","target":"e577","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c628","source":"e594","target":"e585","label":"Association","sourceLayer":"edgy-ar"},{"id":"c629","source":"e594","target":"e589","label":"Association","sourceLayer":"edgy-ar"},{"id":"c653","source":"e626","target":"e625","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c654","source":"e625","target":"e585","label":"OFP PDF","sourceLayer":"edgy-ar"},{"id":"c655","source":"e585","target":"e627","label":"MD Bestand","sourceLayer":"edgy-ar"},{"id":"c657","source":"e627","target":"e628","label":"ControlFlow","sourceLayer":"edgy-ar"}]}</div>

---

*Generated: 2026-06-30 11:43:28*