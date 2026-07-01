---
ea_id: 574
status: Proposed
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: 9b0c660b
notes_hash: e3b0c442
---

# <span class="sl" data-layer="edgy-ar">Asset</span> SDG

**Type:** Class  **Stereotype:** Asset  **StereotypeEx:** Asset  **FQStereotype:** EDGY::Asset  **Status:** <span class="status-badge status-proposed">Proposed</span>  
**Created:** 2026-05-13  **Modified:** 2026-05-13


[Home](../index.md) / [Model Creation](../Model Creation/index.md) / [Information Collection and Model Creation](index.md)

<div id="ea-status-editor" class="ea-status-editor" data-ea-id="574" data-status="Proposed" data-options='["Approved","Implemented","Mandatory","Proposed","Validated"]' data-file-path="Information Collection and Model Creation/SDG.md" data-api-port="8001"></div>

## Tagged Values

| Name | Value | Notes |
|------|-------|-------|
| EDGY::TextAlign | Center | Default: Center  |

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| ControlFlow | Flow | [SDG Website](SDG Website.md) |
| ControlFlow | Flow | [Compile Information Sources](Compile Information Sources.md) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Information Collection and Model Creation.html" class="diagram-thumb"><img src="diagrams/Information Collection and Model Creation.png" alt="Information Collection and Model Creation" loading="lazy"><span>Information Collection and Model Creation</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| ControlFlow | Flow | [SDG Website](SDG Website.md) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container"></div>
<div id="ea-graph-data" style="display:none">{"nodes":[{"id":"e575","label":"SDG Website","fullName":"SDG Website","packageName":"Information Collection and Model Creation","layer":"edgy-ex","isFocal":false,"hasUrl":true,"url":"SDG Website.html"},{"id":"e585","label":"Compile Information Sou…","fullName":"Compile Information Sources","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"Compile Information Sources.html"},{"id":"e574","label":"SDG","fullName":"SDG","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":true,"hasUrl":false,"url":""},{"id":"e578","label":"ESRS","fullName":"ESRS","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"ESRS.html"},{"id":"e582","label":"EDGY 23 Language Founda…","fullName":"EDGY 23 Language Foundation","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"EDGY 23 Language Foundation.html"},{"id":"e577","label":"EDGY -&gt; XMI (Python)","fullName":"EDGY -&gt; XMI (Python)","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"EDGY -_ XMI (Python).html"},{"id":"e594","label":"AI - LLM","fullName":"AI - LLM","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"AI - LLM.html"},{"id":"e625","label":"Openfootprint Data Model","fullName":"Openfootprint Data Model","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"Openfootprint Data Model.html"},{"id":"e627","label":"LDM -&gt; XMI (Python)","fullName":"LDM -&gt; XMI (Python)","packageName":"Information Collection and Model Creation","layer":"edgy-ar","isFocal":false,"hasUrl":true,"url":"LDM -_ XMI (Python).html"}],"edges":[{"id":"c612","source":"e575","target":"e574","label":"ControlFlow","sourceLayer":"edgy-ex"},{"id":"c613","source":"e574","target":"e585","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c615","source":"e578","target":"e585","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c619","source":"e582","target":"e585","label":"ControlFlow","sourceLayer":"edgy-ar"},{"id":"c620","source":"e585","target":"e577","label":"MD Bestand","sourceLayer":"edgy-ar"},{"id":"c628","source":"e594","target":"e585","label":"Association","sourceLayer":"edgy-ar"},{"id":"c654","source":"e625","target":"e585","label":"OFP PDF","sourceLayer":"edgy-ar"},{"id":"c655","source":"e585","target":"e627","label":"MD Bestand","sourceLayer":"edgy-ar"}]}</div>

---

*Generated: 2026-07-01 10:25:44*