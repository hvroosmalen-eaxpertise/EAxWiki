# <span class="sl" data-layer="edgy-ar">Process</span> Constureren MD to XMI Convertor

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
| ControlFlow | Flow | [XMI Import Definition](XMI Import Definition.md) |
| ControlFlow | Flow | [EDGY -> XMI (Python)](EDGY -_ XMI (Python).md) |
| Association | Link | [AI - LLM](AI - LLM.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Information Collection and Model Creation](diagrams/Information Collection and Model Creation.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association | Link | [AI - LLM](AI - LLM.md) |
| ControlFlow | Flow | [XMI Import Definition](XMI Import Definition.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e586","label":"XMI Import Definition","fullName":"XMI Import Definition","packageName":"Information Collection and Model Creation","isFocal":false,"hasUrl":true,"url":"XMI Import Definition.html"},{"id":"e577","label":"EDGY -> XMI (Python)","fullName":"EDGY -> XMI (Python)","packageName":"Information Collection and Model Creation","isFocal":false,"hasUrl":true,"url":"EDGY -_ XMI (Python).html"},{"id":"e594","label":"AI - LLM","fullName":"AI - LLM","packageName":"Information Collection and Model Creation","isFocal":false,"hasUrl":true,"url":"AI - LLM.html"},{"id":"e589","label":"Constureren MD to XMI C…","fullName":"Constureren MD to XMI Convertor","packageName":"Information Collection and Model Creation","isFocal":true,"hasUrl":false,"url":""},{"id":"e595","label":"EDGY Model Elementen en…","fullName":"EDGY Model Elementen en Relaties","packageName":"Information Collection and Model Creation","isFocal":false,"hasUrl":true,"url":"EDGY Model Elementen en Relaties.html"},{"id":"e585","label":"Compile Information Sou…","fullName":"Compile Information Sources","packageName":"Information Collection and Model Creation","isFocal":false,"hasUrl":true,"url":"Compile Information Sources.html"}],"edges":[{"id":"c621","source":"e586","target":"e589","label":"ControlFlow"},{"id":"c614","source":"e577","target":"e595","label":"XMI Bestand"},{"id":"c620","source":"e585","target":"e577","label":"MD Bestand"},{"id":"c623","source":"e589","target":"e577","label":"ControlFlow"},{"id":"c628","source":"e594","target":"e585","label":"Association"},{"id":"c629","source":"e594","target":"e589","label":"Association"}]}</script>

---

*Generated: 2026-06-26 16:40:39*