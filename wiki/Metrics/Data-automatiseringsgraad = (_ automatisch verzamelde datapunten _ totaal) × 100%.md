---
ea_id: 236
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-lb">Outcome</span> Data-automatiseringsgraad = (# automatisch verzamelde datapunten / totaal) × 100%

**Type:** Activity  **Stereotype:** Outcome  **StereotypeEx:** Outcome  **FQStereotype:** EDGY::Outcome  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="236" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Metrics/Data-automatiseringsgraad = (_ automatisch verzamelde datapunten _ totaal) × 100%.md" data-api-port="8001"><span class="status-badge status-proposed">Proposed</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2025-12-03  **Modified:** 2025-12-03


[Home](../index.md) / [Edgy](../Edgy/index.md) / [Metrics](index.md)

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Center | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [% scope 3 emissies met gemeten data](% scope 3 emissies met gemeten data.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Metrics.html" class="diagram-thumb"><img src="diagrams/Metrics.png" alt="Metrics" loading="lazy"><span>Metrics</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [% scope 3 emissies met gemeten data](% scope 3 emissies met gemeten data.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e183","label":"% scope 3 emissies met …","fullName":"% scope 3 emissies met gemeten data","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"% scope 3 emissies met gemeten data.html"},{"id":"e236","label":"Data-automatiseringsgra…","fullName":"Data-automatiseringsgraad = (# automatisch verzamelde datapunten / totaal) × 100%","packageName":"Metrics","layer":"edgy-lb","isFocal":true,"hasUrl":false,"url":""},{"id":"e36","label":"ESRS E1 Climate Change","fullName":"ESRS E1 Climate Change","packageName":"ESRS E1","layer":"edgy-id","isFocal":false,"hasUrl":true,"url":"../ESRS E1/ESRS E1 Climate Change.html"},{"id":"e157","label":"Implementeer dashboards…","fullName":"Implementeer dashboards voor monitoring van CO₂, afval en energie.","packageName":"Task","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"../Task/Implementeer dashboards voor monitoring van CO₂, afval en energie..html"},{"id":"e207","label":"CO₂e, energie, afval, w…","fullName":"CO₂e, energie, afval, water per proces","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"CO₂e, energie, afval, water per proces.html"},{"id":"e208","label":" Productievolumes / nor…","fullName":" Productievolumes / normaliserende operationele data","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Productievolumes _ normaliserende operationele data.html"},{"id":"e209","label":"Datakwaliteit: beschikb…","fullName":"Datakwaliteit: beschikbaarheid / updatefrequentie","packageName":"Metrics","layer":"edgy-lb","isFocal":false,"hasUrl":true,"url":"Datakwaliteit_ beschikbaarheid _ updatefrequentie.html"}],"edges":[{"id":"c108","source":"e36","target":"e183","label":"ControlFlow","sourceLayer":"edgy-id"},{"id":"c135","source":"e157","target":"e183","label":"Association","sourceLayer":"edgy-ex"},{"id":"c208","source":"e183","target":"e207","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c209","source":"e183","target":"e208","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c210","source":"e183","target":"e209","label":"Aggregation","sourceLayer":"edgy-lb"},{"id":"c235","source":"e183","target":"e236","label":"ControlFlow","sourceLayer":"edgy-lb"}]}</div>

---

*Generated: 2026-07-01 12:05:09*