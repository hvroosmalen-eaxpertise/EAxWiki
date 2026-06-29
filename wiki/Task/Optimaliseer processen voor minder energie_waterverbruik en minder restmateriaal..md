# <span class="sl" data-layer="edgy-ex">Task</span> Optimaliseer processen voor minder energie/waterverbruik en minder restmateriaal.

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
| ControlFlow | Flow | [Sustainable Processes](../Process/Sustainable Processes.md) |
| Association | Link | [Energieverbruik en waterverbruik per eenheid](../Metrics/Energieverbruik en waterverbruik per eenheid.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Sustainable Processes](../Process/Sustainable Processes.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e130","label":"Sustainable Processes","fullName":"Sustainable Processes","packageName":"Process","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"../Process/Sustainable Processes.html"},{"id":"e176","label":"Energieverbruik en wate…","fullName":"Energieverbruik en waterverbruik per eenheid","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"../Metrics/Energieverbruik en waterverbruik per eenheid.html"},{"id":"e150","label":"Optimaliseer processen …","fullName":"Optimaliseer processen voor minder energie/waterverbruik en minder restmateriaal.","packageName":"Task","layer":"edgy-ex","isFocal":true,"hasUrl":false,"url":""},{"id":"e14","label":"SDG 12. Responsible Con…","fullName":"SDG 12. Responsible Consumption and Production","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 12. Responsible Consumption and Production.html"},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":"Sustainability Development Goals","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e38","label":"ESRS E3 Water and Marin…","fullName":"ESRS E3 Water and Marine Resources","packageName":"ESRS E3","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E3/ESRS E3 Water and Marine Resources.html"}],"edges":[{"id":"c44","source":"e130","target":"e150","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c69","source":"e14","target":"e130","label":"Association","sourceLayer":"edgy-id"},{"id":"c70","source":"e15","target":"e130","label":"Association","sourceLayer":"edgy-id"},{"id":"c100","source":"e36","target":"e176","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c101","source":"e38","target":"e176","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c128","source":"e150","target":"e176","label":"Association","sourceLayer":"edgy-ex"}]}</div>

---

*Generated: 2026-06-29 13:30:16*