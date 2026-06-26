# <span class="sl" data-layer="edgy-ex">Task</span> Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen.

**Type:** Activity  **Stereotype:** Task  **StereotypeEx:** Task  **FQStereotype:** EDGY::Task  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2025-12-02  **Modified:** 2025-12-15


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Experience](../Experience/index.md) / [Task](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Top | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [Emission Sources](../Asset/Emission Sources.md) |
| Association | Link | [Scope 1 uitstoot per bron (ton CO₂e)](../Metrics/Scope 1 uitstoot per bron (ton CO₂e).md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Emission Sources](../Asset/Emission Sources.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script type="application/json" id="ea-graph-data">{"nodes":[{"id":"e133","label":"Emission Sources","fullName":"Emission Sources","packageName":"Asset","isFocal":false,"hasUrl":true,"url":"../Asset/Emission Sources.html"},{"id":"e180","label":"Scope 1 uitstoot per br…","fullName":"Scope 1 uitstoot per bron (ton CO₂e)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Scope 1 uitstoot per bron (ton CO₂e).html"},{"id":"e154","label":"Identificeer uitstootbr…","fullName":"Identificeer uitstootbronnen (scope 1, 2, 3) en ontwikkel reductieplannen.","packageName":"Task","isFocal":true,"hasUrl":false,"url":""},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":"Sustainability Development Goals","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e197","label":"Brandstofgebruik (Scope…","fullName":"Brandstofgebruik (Scope 1)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Brandstofgebruik (Scope 1).html"},{"id":"e198","label":"Elektriciteitsverbruik …","fullName":"Elektriciteitsverbruik (Scope 2)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Elektriciteitsverbruik (Scope 2).html"},{"id":"e199","label":"Leveranciers-, transpor…","fullName":"Leveranciers-, transport-, gebruiksdata (Scope 3)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Leveranciers-, transport-, gebruiksdata (Scope 3).html"},{"id":"e233","label":"Totale uitstoot = Scope…","fullName":"Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e)","packageName":"Metrics","isFocal":false,"hasUrl":true,"url":"../Metrics/Totale uitstoot = Scope 1 + Scope 2 + Scope 3 (in ton CO₂e).html"}],"edges":[{"id":"c48","source":"e133","target":"e154","label":"ControlFlow"},{"id":"c77","source":"e15","target":"e133","label":"Association"},{"id":"c105","source":"e36","target":"e180","label":"ControlFlow"},{"id":"c132","source":"e154","target":"e180","label":"Association"},{"id":"c198","source":"e180","target":"e197","label":"Aggregation"},{"id":"c199","source":"e180","target":"e198","label":"Aggregation"},{"id":"c200","source":"e180","target":"e199","label":"Aggregation"},{"id":"c232","source":"e180","target":"e233","label":"ControlFlow"}]}</script>

---

*Generated: 2026-06-26 16:40:38*