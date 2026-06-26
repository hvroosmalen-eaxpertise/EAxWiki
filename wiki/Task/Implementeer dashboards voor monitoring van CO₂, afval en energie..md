# <span class="sl" data-layer="edgy-ex">Task</span> Implementeer dashboards voor monitoring van CO₂, afval en energie.

**Type:** Activity  **Stereotype:** Task  **Status:** <span class="status-badge status-proposed">Proposed</span>  
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
| ControlFlow | Flow | [Sustainable Data Assets](../Asset/Sustainable Data Assets.md) |
| Association | Link | [% scope 3 emissies met gemeten data](../Metrics/% scope 3 emissies met gemeten data.md) |

[↑ Back to top](#)

### Appears on Diagrams

- [Architecture](../Architecture/diagrams/Architecture.md)

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [Sustainable Data Assets](../Asset/Sustainable Data Assets.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<script>window.eaGraphData={"nodes":[{"id":"e136","label":"Sustainable Data Assets","fullName":"Sustainable Data Assets","packageName":Asset,"isFocal":false,"hasUrl":true,"url":"../Asset/Sustainable Data Assets.html"},{"id":"e183","label":"% scope 3 emissies met …","fullName":"% scope 3 emissies met gemeten data","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/% scope 3 emissies met gemeten data.html"},{"id":"e157","label":"Implementeer dashboards…","fullName":"Implementeer dashboards voor monitoring van CO₂, afval en energie.","packageName":Task,"isFocal":true,"hasUrl":false,"url":""},{"id":"e15","label":"SDG 13. Climate Action","fullName":"SDG 13. Climate Action","packageName":Sustainability Development Goals,"isFocal":false,"hasUrl":true,"url":"../Sustainability Development Goals/SDG 13. Climate Action.html"},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":ESRS E1,"isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e207","label":"CO₂e, energie, afval, w…","fullName":"CO₂e, energie, afval, water per proces","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/CO₂e, energie, afval, water per proces.html"},{"id":"e208","label":" Productievolumes / nor…","fullName":" Productievolumes / normaliserende operationele data","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Productievolumes _ normaliserende operationele data.html"},{"id":"e209","label":"Datakwaliteit: beschikb…","fullName":"Datakwaliteit: beschikbaarheid / updatefrequentie","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Datakwaliteit_ beschikbaarheid _ updatefrequentie.html"},{"id":"e236","label":"Data-automatiseringsgra…","fullName":"Data-automatiseringsgraad = (# automatisch verzamelde datapunten / totaal) × 100%","packageName":Metrics,"isFocal":false,"hasUrl":true,"url":"../Metrics/Data-automatiseringsgraad = (_ automatisch verzamelde datapunten _ totaal) × 100%.html"}],"edges":[{"id":"c51","source":"e136","target":"e157","label":"ControlFlow"},{"id":"c80","source":"e15","target":"e136","label":"Association"},{"id":"c108","source":"e36","target":"e183","label":"ControlFlow"},{"id":"c135","source":"e157","target":"e183","label":"Association"},{"id":"c208","source":"e183","target":"e207","label":"Aggregation"},{"id":"c209","source":"e183","target":"e208","label":"Aggregation"},{"id":"c210","source":"e183","target":"e209","label":"Aggregation"},{"id":"c235","source":"e183","target":"e236","label":"ControlFlow"}]};</script>

---

*Generated: 2026-06-26 13:25:34*